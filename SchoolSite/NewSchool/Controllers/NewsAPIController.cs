using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SchoolDAO;

namespace NewSchool.Controllers
{
    public class NewsAPIController : ApiController
    {
        //
        // GET: /NewAPI/
        private readonly SchoolString db = new SchoolString();



        public IEnumerable<News> GetAll()
        {
            return db.News.ToList();
        }

        public News GetOne(int id)
        {
            return db.News.Find(id);
        }

        public bool PostNew(News mode)
        {
            db.News.Add(mode);
            return true;
        }

        public int Delete(int id)
        {
            return db.News.Remove(db.News.Find(id)).Id;
        }

        public int DeleteAll()
        {
            return 1;
        }

        public int PutOne(string key, string value)
        {
            return 1;
        }
    }
}
