using System;

namespace Dal.Models
{
    public class Visit
    {
        public int VisitId { get; set; }
        public int NewsId { set; get; }
        public int? UserId { get; set; }

        public string Ip { set; get; }
        public string cookievalue { set; get; }
        public string social { set; get; }

        public DateTime Date { set; get; }

        public virtual News News { set; get; }
        public virtual User Users { set; get; }
    }
}