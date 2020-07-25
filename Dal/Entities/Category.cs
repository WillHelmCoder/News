using System.Collections.Generic;

namespace Dal.Models
{
    public class Category
    {
        public int CategoryId { set; get; }
        public int? Order { get; set; }
        public int? ParentCategoryId { get; set; }
        public int Height { set; get; }
        public int Width { set; get; }

        public string Name { set; get; }
        public string Image { set; get; }
        public string Permalink { get; set; }

        public bool? showImage { set; get; }
        public bool IsDeleted { set; get; }

        public int MagazineId { set; get; }
        public virtual Magazine Magazine { set; get; }

        public virtual List<News> Newses { set; get; }
    }
}