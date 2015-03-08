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
    public class TeacherController : Controller
    {
        private readonly SchoolString db = new SchoolString();

        //
        // GET: /News/

        public ActionResult Index()
        {
            return View(db.Teacher.OrderByDescending(p => p.CreateTime).Select(p => new TeacherModel
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
            Teacher teacher = db.Teacher.Find(id);

            if (teacher == null)
            {
                return HttpNotFound();
            }
            var model = new TeacherModel
            {
                Content = teacher.Content,
                Title = teacher.Title,
                CreateTime = teacher.CreateTime,
                Id = teacher.Id,
                ImageName = teacher.ImageName
                
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
        public ActionResult Create(TeacherModel teachModel)
        {
            if (ModelState.IsValid)
            {
                string imageName = (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    ? Guid.NewGuid().ToString()
                    : string.Empty;
                var entity = new Teacher
                {
                    Content = teachModel.Content,
                    ImageName = imageName,
                    Title = teachModel.Title,
                    CreateTime = DateTime.Now
                };
                db.Teacher.Add(entity);
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
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            var model = new TeacherModel
            {
                Id = teacher.Id,
                Title = teacher.Title,
                Content = teacher.Content,
                CreateTime = teacher.CreateTime
            };
            return View(model);
        }

        //
        // POST: /News/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(TeacherModel teacherModel)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = db.Teacher.Find(teacherModel.Id);
                UpdateModel(teacher);

                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacherModel);
        }

        //
        // GET: /News/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            var news = new TeacherModel
            {
                Id = teacher.Id,
                Title = teacher.Title,
                Content = teacher.Content,
                CreateTime = teacher.CreateTime,
                ImageName = teacher.ImageName
            };
            return View(news);
        }

        //
        // POST: /News/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teacher.Find(id);
            db.Teacher.Remove(teacher);
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