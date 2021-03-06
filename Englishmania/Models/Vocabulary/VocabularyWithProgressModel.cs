﻿using System.Collections.Generic;
using Englishmania.Web.Models.Topic;

namespace Englishmania.Web.Models.Vocabulary
{
    public class VocabularyWithProgressModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; }
        public List<TopicModel> Topics { get; set; }
        public double Progress { get; set; }
    }
}