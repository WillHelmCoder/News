using System;
using System.Collections.Generic;

namespace Dal.Models
{
    public class User
    {
        public int UserId { set; get; }
        public int StageId { set; get; }

        public string Email { set; get; }
        public string UserName { set; get; }
        public string Code { set; get; }
        public string CreateDate { set; get; }
        public string ImageProfile { get; set; }
        public string FbId { get; set; }
        public string PayPalToken { set; get; }

        public DateTime? ActivationDate { set; get; }

        public virtual PaymentStages Stage { set { StageId = (int)value; } get { return (PaymentStages)StageId; } }

        public virtual List<Magazine> Magazines { set; get; }
    }
}