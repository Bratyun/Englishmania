using System;
using Englishmania.DAL.Entities;
using Englishmania.DAL.Interfaces;

namespace Englishmania.BLL.Dto
{
    public class WordDto
    {
        public int Id { get; set; }
        public string English { get; set; }
        public string Russian { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }

        public WordDto()
        {
        }

        public WordDto(int wordId, int userId, IUnitOfWork unitOfWork)
        {
            var word = unitOfWork.WordRepository.Get(wordId);
            var wordUser = unitOfWork.WordUserRepository.Get(x => x.WordId == word.Id && x.UserId == userId);
            if (word == null || wordUser == null)
            {
                Id = 0;
                Count = 0;
                English = String.Empty;
                Russian = String.Empty;
                UserId = 0;
                return;
            }

            Id = word.Id;
            Count = wordUser.Count;
            English = word.English;
            Russian = word.Russian;
            UserId = userId;
        }
    }
}