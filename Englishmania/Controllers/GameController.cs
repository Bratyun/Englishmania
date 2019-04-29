using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Englishmania.BLL.Interfaces;
using Englishmania.Web.Models.Game;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Englishmania.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IUserService _userService;
        private readonly IVocabularyService _vocabularyService;
        private readonly IWordService _wordService;

        public GameController(IUserService userService, IWordService wordService, IVocabularyService vocabularyService,
            ITopicService topicService)
        {
            _userService = userService;
            _wordService = wordService;
            _vocabularyService = vocabularyService;
            _topicService = topicService;
        }

        //[HttpGet("eng-to-rus-translation")]
        //public ActionResult WordToTranslation()
        //{

        //}

        //[HttpPut("rus-to-eng-translation")]
        //public ActionResult WordToTranslationResult(GameTranslationResultModel model)
        //{

        //}
    }
}