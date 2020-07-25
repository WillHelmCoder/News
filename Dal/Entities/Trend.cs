using System;

namespace Dal.Models
{
    public class Trend
    {
        public int TrendId { set; get; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime CreationDate { set; get; }

        public virtual User Users { set; get; }
    }
}