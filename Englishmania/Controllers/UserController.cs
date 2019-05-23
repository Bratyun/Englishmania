using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.Web.Models;
using Englishmania.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Englishmania.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IVocabularyService _vocabularyService;

        public UserController(IUserService userService, IVocabularyService vocabularyService)
        {
            _userService = userService;
            _vocabularyService = vocabularyService;
        }

        public class LoginResponseModel
        {
            public string Token { get; set; }
        }

        [HttpPost("login")]
        public ActionResult<LoginResponseModel> Login(LoginRequestModel model)
        {
            var claims = GetIdentity(model.Login, model.PasswordHash);
            if (claims == null) return StatusCode(401);

            var now = DateTime.UtcNow;
            var symmetricKey = Convert.FromBase64String(Startup.Key);
            // create JWT-token
            var jwt = new JwtSecurityToken(
                Startup.Issuer,
                notBefore: now,
                claims: claims.Claims,
                expires: now.Add(Startup.LifeTime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new LoginResponseModel { Token = encodedJwt };
        }

        [Authorize]
        [HttpGet("profile")]
        public ActionResult<UserProfile> Profile()
        {
            int id = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            var user =_userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            return new UserProfile
            {
                Id = user.Id,
                Email = user.Email,
                Login = user.Login,
                LevelId = user.LevelId
            };
        }

        [Authorize]
        [HttpPut("difficulty/{id}")]
        public ActionResult SetDifficulty(int id)
        {
            int userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            _userService.UpdateLevel(userId, id);
            return Ok();
        }

        [HttpPost("register")]
        public ActionResult Register(UserRegisterModel model)
        {
            var exist = _userService.IsExist(model.Login, model.PasswordHash);
            if (exist || !ModelState.IsValid) return StatusCode(409);
            var user = new User
            {
                Login = model.Login,
                Email = model.Email,
                PasswordHash = model.PasswordHash
            };
            _userService.Create(user);
            var dictionaries = _vocabularyService.GetAll();
            if (dictionaries != null)
            {
                foreach (var dictionary in dictionaries)
                {
                    _vocabularyService.ConnectUserWithDictionary(user.Id, dictionary.Id);
                }
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("progress")]
        public ActionResult<double> GetGlobalProgress()
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            double result = _vocabularyService.GetGlobalProgress(userId);
            
            return result;
        }
        private ClaimsIdentity GetIdentity(string login, string passwordHash)
        {
            var user = _userService.Get(login, passwordHash);
            if (user == null) return null;
            var claims = new List<Claim>
            {
                new Claim(TokenClaims.Id, user.Id.ToString()),
                new Claim(TokenClaims.Login, user.Login)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", TokenClaims.Login, TokenClaims.Login);
            return claimsIdentity;
        }
    }
}