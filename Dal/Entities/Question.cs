using System;
using System.Collections.Generic;

namespace Dal.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public bool Open { get; set; }
        public bool IsDeleted { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public virtual List<Answer> Answers { get; set; }
        public virtual List<Option> Options { get; set; }
    }
}