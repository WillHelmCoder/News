using System;
using System.Collections.Generic;

namespace Dal.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationDate { set; get; }

        public int MagazineId { get; set; }
        public virtual Magazine Magazine { get; set; }

        public virtual List<Question> Questions { get; set; }
    }
}