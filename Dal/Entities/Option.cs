using System;

namespace Dal.Models
{
    public class Option
    {
        public int OptionId { get; set; }
        public int? Value { get; set; }
        public int QuestionId { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual Question Question { get; set; }
    }
}