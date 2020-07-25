using System.Collections.Generic;

namespace Dal.Models
{
    public class Magazine
    {
        public int MagazineId { set; get; }

        public string Title { set; get; }
        public string Code { set; get; }
        public string Description { set; get; }
        public string Logo { get; set; }
        public string Address { set; get; }
        public string Email { set; get; }
        public string Guid { get; set; }
        public string Domain { get; set; }
        public string FacebookPage { get; set; }
        public string TwitterPage { get; set; }
        public string GoogleAnalyticsId { get; set; }

        public bool IsPrivate { get; set; }

        public int UserId { set; get; }
        public virtual User User { set; get; }

        public int CityId { get; set; }
        public virtual City City { get; set; }

        public virtual List<Category> Categories { set; get; }
        public virtual List<Media> Medias { get; set; }
        public virtual List<NewsLetter> Newsletters { get; set; }
        public virtual List<AdCategory> AdCategories { get; set; }
        public virtual List<Galery> Galeries { get; set; }
        public virtual List<ItemList> ItemsList { get; set; }
    }
}