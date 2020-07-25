using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class MagazineIndexViewModel
    {
        public Int32 MagazineId { get; set; }
        public Int32 ViewsCount { get; set; }
        public Int32 NewsCount { get; set; }
        public String MostViewedCat { get; set; }
        public Int32 MostViewedCatCount { get; set; }
        public String MostUsedCat { get; set; }
        public Int32 MostUsedCatCount { get; set; }
        public List<Last10CountModel> Top10News { get; set; }
        public List<Last10CountModel> Last10News { get; set; }
        public List<TopInfluencers> Top10Influencers { get; set; }
        public List<Category> Categories { get; set; }
        public List<News> News { get; set; }
        public List<Int32> Visits { get; set; }
        public List<MagazineToCLone> Magazines { get; set; } 
    }

    public class MagazineToCLone
    {
        public Int32 MagazineId { get; set; }
        public String Title { get; set; }
    }
}