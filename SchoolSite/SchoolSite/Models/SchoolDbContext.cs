using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolSite.Models
{
    public class SchoolDbContext:DbContext
    {

        public SchoolDbContext()
            : base("SchoolDBContext")
        {
            
        }
        public DbSet<NewsModel> NewsModels { get; set; }
        public DbSet<Notice> Notices { get; set; }
    }
}