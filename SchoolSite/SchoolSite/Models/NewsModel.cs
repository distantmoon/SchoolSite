using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace SchoolSite.Models
{
    public class NewsModel
    {
        public int Id { get; set; }
        [Display(Name = "标题")]
        public string Title { get; set; }
         [Display(Name = "内容")]
        public string Content { get; set; }
         [Display(Name = "添加日期")]
        public DateTime DateTime { get; set; }

        public virtual byte[] Images { get; set; }
    }
}