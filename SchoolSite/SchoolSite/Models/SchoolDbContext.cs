using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolSite.Models
{
    public class SchoolDbContext:DbContext
    {
        public DbSet<NewsModel> NewsModels { get; set; }
    }
}