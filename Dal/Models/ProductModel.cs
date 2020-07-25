using System;

namespace Dal.Models
{
    public class NewsModel
    {
        public int NewsId { set; get; }

        public string Title { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }
        public string Body { set; get; }
        public string CategoryName { set; get; }
        public string Date { set; get; }
        public string Alt { set; get; }
        public string MetaDesc { set; get; }
        public string Keywords { set; get; }
        public string Permalink { set; get; }
        public string VideoEmbed { get; set; }
        public string Thumbnail { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual Advertise HeaderAd { get; set; }
        public virtual Advertise RightAd { get; set; }
        public virtual Advertise FooterAd { get; set; }

        public int? Visitas { set; get; }
        public int? UpVotes { get; set; }
        public int? DownVotes { get; set; }

        public DateTime CreationDate { get; set; }

        public bool WithVideo { get; set; }
    }
}