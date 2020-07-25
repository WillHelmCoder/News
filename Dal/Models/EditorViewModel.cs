using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dal.Models
{
    public class EditorViewModel
    {
        public User Editor { get; set; }
        public List<GroupNewsModel> News { get; set; }
    }
}