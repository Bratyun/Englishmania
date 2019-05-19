using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Englishmania.BLL.Dto;
using Englishmania.BLL.Interfaces;
using Englishmania.Web.Models;
using Englishmania.Web.Models.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Englishmania.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("eng-to-rus-translation")]
        public ActionResult<GameTranslationDto> WordToTranslationToRus()
        {
            int userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            var model = _gameService.GetTranslationGameFromEngToRus(userId);
            return model;
        }

        [HttpGet("rus-to-eng-translation")]
        public ActionResult<GameTranslationDto> WordToTranslationToEng()
        {
            int userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            var model = _gameService.GetTranslationGameFromRusToEng(userId);
            return model;
        }

        [HttpPut("translation-result")]
        public ActionResult WordToTranslationResult(GameTranslationResult model)
        {
            int userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            _gameService.SetWordProgress(userId, model);
            return Ok();
        }
    }
}