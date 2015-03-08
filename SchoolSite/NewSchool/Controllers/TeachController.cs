using System;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Util;
using NewSchool.Models;
using SchoolDAO;

namespace NewSchool.Controllers
{
    public class TeachController : Controller
    {
        private readonly SchoolString db = new SchoolString();

        //
        // GET: /News/

        public ActionResult Index()
        {
            return View(db.EducationWorks.OrderByDescending(p => p.CreateTime).Select(p => new TeachModel
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
            EducationWorks works = db.EducationWorks.Find(id);

            if (works == null)
            {
                return HttpNotFound();
            }
            var model = new TeachModel
            {
                Content = works.Content,
                Title = works.Title,
                CreateTime = works.CreateTime,
                Id = works.Id,
                ImageName = works.ImageName
            };
            return View(model);
        }

        //
        // GET: /News/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /News/Create
        /*    public ActionResult GetImage(int id)
        {
            News newsModel = db.News.Find(id);
            if (newsModel==null || string.IsNullOrEmpty(newsModel.ImageName))
            {
                return new EmptyResult();
            }

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "newsimage", newsModel.ImageName + ".jpeg");
            if (System.IO.File.Exists(filePath))
            {
                return new FilePathResult(filePath, "image/jpeg");
            }
            return new EmptyResult();
            
        }*/


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(TeachModel teachModel)
        {
            if (ModelState.IsValid)
            {
                string imageName = (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                     ? Guid.NewGuid().ToString()
                     : string.Empty;
                var entity = new EducationWorks
                {
                    Content = teachModel.Content,
                    ImageName = imageName,
                    Title = teachModel.Title,
                    CreateTime = DateTime.Now
                };
                db.EducationWorks.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(imageName))
                {
                    WebImage webImage = WebImage.GetImageFromRequest();
                    webImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "newsimage", entity.ImageName),
                        ImageFormat.Jpeg.ToString());
                }


                return RedirectToAction("Index");
            }

            return View(teachModel);
        }

        //
        // GET: /News/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EducationWorks educationWorks = db.EducationWorks.Find(id);
            if (educationWorks == null)
            {
                return HttpNotFound();
            }
            var model = new TeachModel
            {
                Id = educationWorks.Id,
                Title = educationWorks.Title,
                Content = educationWorks.Content,
                CreateTime = educationWorks.CreateTime,
                ImageName = educationWorks.ImageName
            };
            return View(model);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(TeachModel teachModel)
        {
            if (ModelState.IsValid)
            {
                EducationWorks educationWorks = db.EducationWorks.Find(teachModel.Id);
                UpdateModel(educationWorks);
                
                db.Entry(educationWorks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teachModel);
        }

        //
        // GET: /News/Delete/5

        public ActionResult Delete(int id = 0)
        {
            EducationWorks educationWorks = db.EducationWorks.Find(id);
            if (educationWorks == null)
            {
                return HttpNotFound();
            }
            var news = new TeachModel
            {
                Id = educationWorks.Id,
                Title = educationWorks.Title,
                Content = educationWorks.Content,
                CreateTime = educationWorks.CreateTime,
                ImageName = educationWorks.ImageName
            };
            return View(news);
        }

        //
        // POST: /News/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EducationWorks educationWorks = db.EducationWorks.Find(id);
            db.EducationWorks.Remove(educationWorks);
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