using System;

namespace Dal.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int? Value { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}