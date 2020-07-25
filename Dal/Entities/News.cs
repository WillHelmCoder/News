using System;
using System.Collections.Generic;

namespace Dal.Models
{
    public class News
    {
        public int NewsId { set; get; }
        public int? UserId { set; get; }
        public long? Rank { set; get; }
        public int ClonedFrom { get; set; }
        public int? UpVotes { get; set; }
        public int? DownVotes { get; set; }
        public int CommentsCount { get; set; }
        public int? VisitsCount { get; set; }

        public string Title { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public string Thumbnail { set; get; }
        public string Body { set; get; }
        public string VideoEmbed { get; set; }
        public string Alt { set; get; }
        public string MetaDesc { set; get; }
        public string Keywords { set; get; }
        public string Permalink { set; get; }
        public string ThankNote { get; set; }

        public DateTime CreationDate { set; get; }

        public bool WithVideo { get; set; }
        public bool IsClon { get; set; }
        public bool IsDeleted { set; get; }

        public int CategoryId { set; get; }
        public virtual Category Category { set; get; }
        
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Vote> Votes { get; set; }
        public virtual List<Visit> Visits { set; get; }

    }
}