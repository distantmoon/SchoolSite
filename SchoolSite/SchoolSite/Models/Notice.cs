using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolSite.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
    }
}