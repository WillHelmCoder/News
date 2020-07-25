using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class NearlyNews
    {
        public Int32 MagazineId { set; get; }
        public String MagazineName { set; get; }

        public List<News> NewsList { set; get; }
    }
}