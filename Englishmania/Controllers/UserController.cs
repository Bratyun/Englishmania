﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.Web.Models;
using Englishmania.Web.Models.Topic;
using Englishmania.Web.Models.User;
using Englishmania.Web.Models.Vocabulary;
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
                Login = user.Login
            };
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

        [HttpGet("dictionaries-all")]
        public ActionResult<List<Vocabulary>> GetDictionariesAll()
        {
            var result = _vocabularyService.GetAll();
            if (result == null)
            {
                return new List<Vocabulary>();
            }

            return result;
        }

        [Authorize]
        [HttpPut("dictionaries/{id}")]
        public ActionResult SetDictionaryForUser(int id)
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            _vocabularyService.ConnectUserWithDictionary(userId, id);
            return StatusCode(200);
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