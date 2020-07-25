using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class Response_GaleryModel
    {
        public Int32 GaleryId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Alt { set; get; }
        public String MetaDesc { set; get; }
        public String Keywords { set; get; }
        public String Permalink { set; get; }
        public String CreationDate { get; set; }
        public DateTime CreationDateTime { get; set; }
        public List<Response_GaleryImageModel> Images { get; set; }
    }

    public class Response_GaleryImageModel
    {
        public Int32 GaleryImageId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String ImageCode { get; set; }
    }
}