using System;
using System.Collections.Generic;

namespace Dal.Models
{
    public class Comment
    {
        public int CommentId { set; get; }
        public int? UpVotes { get; set; }
        public int? DownVotes { get; set; }

        public string Content { set; get; }
        
        public DateTime CreationDate { set; get; }

        public bool IsDeleted { set; get; }

        public int NewsId { set; get; }
        public virtual News News { set; get; }

        public int UserId { set; get; }
        public virtual User Users { set; get; }

        public virtual List<Vote> Votes { get; set; }
    }
}