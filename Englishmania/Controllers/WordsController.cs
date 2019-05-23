using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Englishmania.Web.Controllers
{
    [Authorize]
    [Route("api/user/dictionaries/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IVocabularyService _vocabularyService;
        private readonly IWordService _wordService;

        public WordsController(IWordService wordService, IVocabularyService vocabularyService)
        {
            _wordService = wordService;
            _vocabularyService = vocabularyService;
        }

        [HttpPost]
        public ActionResult AddWord(Word word)
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            _vocabularyService.AddCustomWord(userId, word);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteWord(int id)
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            _vocabularyService.DeleteCustomWord(userId, id);
            return Ok();
        }
    }
}