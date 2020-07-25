using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Models;

namespace Dal.Models
{
    public class AnswerIndexViewModel
    {
        public List<Answer> AnswersList { get; set; }
        public Int32 AnswerId { get; set; }
        public virtual Question Question { get; set; }
        public Int32 QuestionId { get; set; }
        public String Description { get; set; }
        public DateTime CreationDate { get; set; }
        public Boolean IsDeleted { get; set; }
    }
}