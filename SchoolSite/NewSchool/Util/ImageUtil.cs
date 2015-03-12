using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolDAO;

namespace NewSchool.Util
{
    public class ImageUtil
    {
        public  static List<ScrollImage> GetScroll()
        {
            using (var db=new SchoolString())
            {
                return db.ScrollImage.Take(3).OrderByDescending(p=>p.CreateTime).ToList();
            }
        }
    }
}