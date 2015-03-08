using System;
using System.ComponentModel.DataAnnotations;

namespace NewSchool.Models
{
    public class ProfessionModel
    {
        public int Id { get; set; }
        [Display(Name = "姓名")]
        public string Title { get; set; }
        [Display(Name = "简介")]
        public string Content { get; set; }
        [Display(Name = "添加日期")]
        public DateTime CreateTime { get; set; }

        public string ImageName { get; set; }

        public string  Remark { get; set; }
    }
}