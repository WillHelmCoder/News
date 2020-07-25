using System;
using System.Collections.Generic;

namespace Dal.Models
{
    public class Galery
    {
        public int GaleryId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Alt { set; get; }
        public string MetaDesc { set; get; }
        public string Keywords { set; get; }
        public string Permalink { set; get; }

        public DateTime CreationDate { get; set; }

        public bool IsDeleted { get; set; }

        public int MagazineId { get; set; }
        public virtual Magazine Magazine { get; set; }

        public virtual List<GaleryImage> GaleryImages { get; set; }
    }
}