using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Models;

namespace Dal.Models
{
    public class GroupByCategory
    {
        public Int32 CategoryId { get; set; }
        public String Category { get; set; }
        public Int32 Count { get; set; }
    }
}