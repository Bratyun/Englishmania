using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Englishmania.BLL.Interfaces;
using Englishmania.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Englishmania.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        private readonly IWordService _wordService;
        private readonly IVocabularyService _vocabularyService;
        private readonly ITopicService _topicService;
        private readonly IHostingEnvironment _appEnvironment;

        public BaseController(IWordService wordService, IVocabularyService vocabularyService,
            ITopicService topicService, IHostingEnvironment appEnvironment)
        {
            _wordService = wordService;
            _vocabularyService = vocabularyService;
            _topicService = topicService;
            _appEnvironment = appEnvironment;
        }

        private object lockObj = new Object();

        [HttpGet("init")]
        [Authorize(Roles = "Roman")]
        public void UpdateDb()
        {
            lock (lockObj)
            {
                string filePath = Path.Combine(_appEnvironment.ContentRootPath, "wwwroot/Files/Result.txt");
                if (!System.IO.File.Exists(filePath)) return;
                // Read a text file line by line.
                string[] lines = System.IO.File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    if (line == "")
                    {
                        continue;
                    }

                    string[] obj = line.Split(";").ToArray<string>();

                    var word = new Word
                    {
                        English = obj[0],
                        Russian = obj[1]
                    };
                    _wordService.Create(word);
                    //word = _wordService.GetByEng(word.English);

                    string vocabularyName = obj[2] + " " + obj[3];
                    bool exist = _vocabularyService.IsExist(vocabularyName);
                    Vocabulary vocabulary;
                    if (exist)
                    {
                        vocabulary = _vocabularyService.GetByName(vocabularyName);
                    }
                    else
                    {
                        vocabulary = new Vocabulary
                        {
                            LevelId = int.Parse(obj[3]),
                            IsPrivate = false,
                            Name = vocabularyName
                        };
                        _vocabularyService.Create(vocabulary);
                        //vocabulary = _vocabularyService.GetByName(vocabulary.Name);
                    }

                    _vocabularyService.AddWord(word.Id, vocabulary.Id);
                }
            }
        }
    }
}