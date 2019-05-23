using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Englishmania.Web.Models;
using Englishmania.Web.Models.Topic;
using Englishmania.Web.Models.Vocabulary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace Englishmania.Web.Controllers
{
    [Authorize]
    [Route("api/user/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IVocabularyService _vocabularyService;
        private readonly IWordService _wordService;

        public DictionariesController(IWordService wordService, IVocabularyService vocabularyService,
            ITopicService topicService)
        {
            _wordService = wordService;
            _vocabularyService = vocabularyService;
            _topicService = topicService;
        }

        [HttpGet]
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
        
        [HttpGet("all")]
        public ActionResult<List<VocabularyWithWords>> GetDictionariesAll()
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            var dictionariesAll = _vocabularyService.GetAll();
            if (dictionariesAll == null)
            {
                return NotFound();
            }

            List<VocabularyWithWords> models = new List<VocabularyWithWords>();
            foreach (var item in dictionariesAll)
            {
                var obj = new VocabularyWithWords
                {
                    Id = item.Id,
                    IsPrivate = item.IsPrivate,
                    Name = item.Name,
                    LevelId = item.LevelId,
                    Words = _wordService.GetByVocabulary(userId, item.Id)
                };
                models.Add(obj);
            }
            return models;
        }
        
        [HttpGet("{id}")]
        public ActionResult<VocabularyWithWords> GetDictionary(int id)
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            var dictionary = _vocabularyService.Get(id);
            if (dictionary == null)
            {
                return NotFound();
            }

            return new VocabularyWithWords
            {
                Id = dictionary.Id,
                IsPrivate = dictionary.IsPrivate,
                Name = dictionary.Name,
                LevelId = dictionary.LevelId,
                Words = _wordService.GetByVocabulary(userId, dictionary.Id)
            };
        }
        
        [HttpPut("{id}")]
        public ActionResult AddDictionaryForUser(int id)
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            _vocabularyService.ConnectUserWithDictionary(userId, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDictionaryForUser(int id)
        {
            var userId = int.Parse(User.FindFirst(TokenClaims.Id).Value);
            _vocabularyService.DeleteDictionaryFromUser(userId, id);
            return Ok();
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