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
    public class ResearchController : Controller
    {
        private readonly SchoolString db = new SchoolString();

        //
        // GET: /News/

        public ActionResult Index()
        {
            return View(db.StudyWorks.OrderByDescending(p => p.CreateTime).Select(p => new ResearchModel
            {
                Content = p.Content,
                CreateTime = p.CreateTime,
                Id = p.Id,
                Title = p.Title
            }).ToList());
        }

        //
        // GET: /News/Details/5

        public ActionResult Details(int id = 0)
        {
            StudyWorks works = db.StudyWorks.Find(id);

            if (works == null)
            {
                return HttpNotFound();
            }
            var newsmodel = new ResearchModel
            {
                Content = works.Content,
                Title = works.Title,
                CreateTime = works.CreateTime,
                Id = works.Id
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

         [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ResearchModel newsmodel)
        {
            if (ModelState.IsValid)
            {
                string imageName = (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                     ? Guid.NewGuid().ToString()
                     : string.Empty;
                var entity = new StudyWorks
                {
                    Content = newsmodel.Content,
                    ImageName = imageName,
                    Title = newsmodel.Title,
                    CreateTime = DateTime.Now
                };
                db.StudyWorks.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(imageName))
                {
                    WebImage webImage = WebImage.GetImageFromRequest();
                    webImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "newsimage", entity.ImageName),
                        ImageFormat.Jpeg.ToString());
                }
                /*  if (Request.Files.Count > 0  && Request.Files[0].ContentLength>0)
                {
                    WebImage webImage = WebImage.GetImageFromRequest();
                    newsmodel.ImageName =entity.ImageName;
                    webImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"newsimage", entity.ImageName),ImageFormat.Jpeg.ToString());
                }*/


                return RedirectToAction("Index");
            }

            return View(newsmodel);
        }

        //
        // GET: /News/Edit/5
         [Authorize]
        public ActionResult Edit(int id = 0)
        {
            StudyWorks studyWorks = db.StudyWorks.Find(id);
            if (studyWorks == null)
            {
                return HttpNotFound();
            }
            var news = new ResearchModel
            {
                Id = studyWorks.Id,
                Title = studyWorks.Title,
                Content = studyWorks.Content,
                CreateTime = studyWorks.CreateTime
            };
            return View(news);
        }

        //
        // POST: /News/Edit/5
         [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ResearchModel researchModel)
        {
            if (ModelState.IsValid)
            {
                StudyWorks studyWorks = db.StudyWorks.Find(researchModel.Id);
                UpdateModel(studyWorks);

                db.Entry(studyWorks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(researchModel);
        }

        //
        // GET: /News/Delete/5
         [Authorize]
        public ActionResult Delete(int id = 0)
        {
            StudyWorks studyWorks = db.StudyWorks.Find(id);
            if (studyWorks == null)
            {
                return HttpNotFound();
            }
            var news = new ResearchModel
            {
                Id = studyWorks.Id,
                Title = studyWorks.Title,
                Content = studyWorks.Content,
                CreateTime = studyWorks.CreateTime
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
            StudyWorks studyWorks = db.StudyWorks.Find(id);
            db.StudyWorks.Remove(studyWorks);
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