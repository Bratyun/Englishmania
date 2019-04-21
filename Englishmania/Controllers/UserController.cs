using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Englishmania.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IUserService _userService;
        private readonly IVocabularyService _vocabularyService;
        private readonly IWordService _wordService;

        public UserController(IUserService userService, IWordService wordService, IVocabularyService vocabularyService,
            ITopicService topicService)
        {
            _userService = userService;
            _wordService = wordService;
            _vocabularyService = vocabularyService;
            _topicService = topicService;
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginRequestModel model)
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
            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(string login, string passwordHash)
        {
            var user = _userService.Get(login, passwordHash);
            if (user == null) return null;
            var claims = new List<Claim>
            {
                new Claim(TokenClaims.Id, user.Id.ToString()),
                new Claim(TokenClaims.Name, user.Name),
                new Claim(TokenClaims.Login, user.Login)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token", TokenClaims.Name, TokenClaims.Login);
            return claimsIdentity;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterModel model)
        {
            var exist = _userService.IsExist(model.Login, model.PasswordHash);
            if (exist || !ModelState.IsValid) return StatusCode(409);
            var user = new User
            {
                Login = model.Login,
                Name = model.Name,
                PasswordHash = model.PasswordHash
            };
            _userService.Create(user);
            return StatusCode(200);
        }

        [Authorize]
        [HttpGet("progress")]
        public ActionResult<double> GetGlobalProgress()
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            var vocabularies = _vocabularyService.GetByUser(userId);
            double res = 0;
            var count = 0;
            foreach (var vocabulary in vocabularies)
            {
                res += _vocabularyService.GetProgress(userId, vocabulary.Id);
                count++;
            }

            if (count == 0) return StatusCode(404);
            return res / count;
        }

        [Authorize]
        [HttpGet("dictionaries")]
        public ActionResult<List<VocabularyWithProgressModel>> GetDictionaries()
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            var vocabularies = _vocabularyService.GetByUser(userId);
            if (vocabularies == null) return new List<VocabularyWithProgressModel>();
            var results = new List<VocabularyWithProgressModel>();
            foreach (var item in vocabularies)
            {
                var topics = _topicService.GetByVocabulary(item.Id);
                var progress = _vocabularyService.GetProgress(userId, item.Id);
                var model = new VocabularyWithProgressModel
                {
                    Id = item.Id,
                    LevelId = item.LevelId,
                    Name = item.Name,
                    Progress = progress,
                    Topics = TopicsToModels(topics)
                };
                results.Add(model);
            }

            return results;
        }

        private List<TopicModel> TopicsToModels(List<Topic> topics)
        {
            var results = new List<TopicModel>();
            foreach (var item in topics)
            {
                var topicModel = new TopicModel(item);
                results.Add(topicModel);
            }

            return results;
        }
    }
}