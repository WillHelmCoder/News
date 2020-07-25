using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class GroupBySocialModel
    {
        public Int32 Id { set; get; }
        public Int32 Count { set; get; }

        public string Social { get; set; }
    }
}