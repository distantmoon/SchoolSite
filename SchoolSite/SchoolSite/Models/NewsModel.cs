using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace SchoolSite.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string Content { get; set; }
        public DateTime DateTime { get; set; }

        public virtual byte[] Images { get; set; }
    }
}