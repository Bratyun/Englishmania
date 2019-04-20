using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Englishmania.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("login")]
        public ActionResult<string> Login(string login, string passwordHash)
        {
            ClaimsIdentity claims = GetIdentity(login, passwordHash);
            if (claims == null) return StatusCode(401);

            var now = DateTime.UtcNow;
            byte[] symmetricKey = Convert.FromBase64String(Startup.Key);
            // create JWT-token
            var jwt = new JwtSecurityToken(
                issuer: Startup.Issuer,
                notBefore: now,
                claims: claims.Claims,
                expires: now.Add(Startup.LifeTime),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(string login, string passwordHash)
        {
            User user = _userService.Get(login, passwordHash);
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
    }
}