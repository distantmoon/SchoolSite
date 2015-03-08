using System;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using NewSchool.Models;
using SchoolDAO;

namespace NewSchool.Controllers
{
    public class CommunicationController : Controller
    {
        private readonly SchoolString db = new SchoolString();

        //
        // GET: /News/

        public ActionResult Index()
        {
           
            return View(db.Comm.OrderByDescending(p => p.CreateTime).Select(p => new CommModel()
            {
                Content = p.Content,
                CreateTime = p.CreateTime,
                Id = p.Id,
                Title = p.Title,
                ImageName = p.ImageName
            }).ToList());
        }

        public ActionResult Show()
        {
            return PartialView("_NewsShowList",
                db.News.Take(3).OrderByDescending(p => p.CreateTime).Select(p => new NewsModel
                {
                    Content = p.Content,
                    CreateTime = p.CreateTime,
                    Id = p.Id,
                    Title = p.Title,
                    ImageName = p.ImageName
                }).ToList());
        }

        //
        // GET: /News/Details/5

        public ActionResult Details(int id = 0)
        {
            Comm news = db.Comm.Find(id);

            if (news == null)
            {
                return HttpNotFound();
            }
            var newsmodel = new CommModel
            {
                Content = news.Content,
                Title = news.Title,
                CreateTime = news.CreateTime,
                Id = news.Id,
                ImageName = news.ImageName
            };
            return View(newsmodel);
        }

        //
        // GET: /News/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /News/Create
        public ActionResult GetImage(int id)
        {
            News newsModel = db.News.Find(id);
            if (newsModel == null || string.IsNullOrEmpty(newsModel.ImageName))
            {
                return new EmptyResult();
            }

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "newsimage",
                newsModel.ImageName + ".jpeg");
            if (System.IO.File.Exists(filePath))
            {
                return new FilePathResult(filePath, "image/jpeg");
            }
            return new EmptyResult();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create(CommModel newsmodel)
        {
            if (ModelState.IsValid)
            {
                string imageName = (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    ? Guid.NewGuid().ToString()
                    : string.Empty;
                var entity = new Comm
                {
                    Content = newsmodel.Content,
                    ImageName = imageName,
                    Title = newsmodel.Title,
                    CreateTime = DateTime.Now
                };
                db.Comm.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(imageName))
                {
                    WebImage webImage = WebImage.GetImageFromRequest();
                    webImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "newsimage", entity.ImageName),
                        ImageFormat.Jpeg.ToString());
                }


                return RedirectToAction("Index");
            }

            return View(newsmodel);
        }

        //
        // GET: /News/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Comm newsmodel = db.Comm.Find(id);
            if (newsmodel == null)
            {
                return HttpNotFound();
            }
            var news = new CommModel
            {
                Id = newsmodel.Id,
                Title = newsmodel.Title,
                Content = newsmodel.Content,
                CreateTime = newsmodel.CreateTime
            };
            return View(news);
        }

        //
        // POST: /News/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommModel newsmodel)
        {
            if (ModelState.IsValid)
            {
                Comm find = db.Comm.Find(newsmodel.Id);
                UpdateModel(find);
                db.Entry(find).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsmodel);
        }

        //
        // GET: /News/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Comm newsmodel = db.Comm.Find(id);
            if (newsmodel == null)
            {
                return HttpNotFound();
            }
            var news = new CommModel
            {
                Id = newsmodel.Id,
                Title = newsmodel.Title,
                Content = newsmodel.Content,
                CreateTime = newsmodel.CreateTime
            };
            return View(news);
        }

        //
        // POST: /News/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comm newsmodel = db.Comm.Find(id);
            db.Comm.Remove(newsmodel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}