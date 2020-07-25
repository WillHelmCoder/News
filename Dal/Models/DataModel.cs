using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class DataModel
    {
        public Int32 DataId { get; set; }
        public Int32? DataParentId { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 Order { get; set; }
        public DateTime CreationDate { get; set; }
        public Boolean IsDeleted { get; set; }
        public Int32? MagazineId { get; set;}
        public String Image { get; set; }
        public String ImageSign { get; set; }
        public String Url { get; set; }
    }
}