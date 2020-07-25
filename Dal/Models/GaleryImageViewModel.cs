using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class GaleryImageViewModel
    {
        public Int32 GaleryId { get; set; }
        public Int32 GaleryImageId { get; set; }
        public Int32 ImageId { get; set; }
        public virtual Image Image { get; set; }
        public String Title { set; get; }
        public String Description { set; get; }
        public Int32 Order { get; set; }
    }
}