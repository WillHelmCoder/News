using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class CategoryModel
    {
        public Int32 CategoryId { set; get; }
        public Int32? ParentCategoryId { set; get; }
        public String Name { set; get; }
        public String Image { set; get; }
        public String MagazineTitle { set; get; }
        public Int32 MagazineId { set; get; }
        public String Permalink { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}