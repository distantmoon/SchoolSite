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
    public class NoticeController : Controller
    {
        private readonly SchoolString db = new SchoolString();

        //
        // GET: /NoticeModel/

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_NoticeList",
                    db.Notices.Take(11).OrderByDescending(p => p.CreateTime).Select(p => new NewSchool.Models.NoticeModel
                    {
                        Content = p.Content,
                        Id = p.Id,
                        CreateTime = p.CreateTime,
                        Title = p.Title,
                        ImageName = p.ImageName
                        
                    }).ToList());
            }
            return View(db.Notices.OrderByDescending(p => p.CreateTime).Select(p => new NewSchool.Models.NoticeModel
            {
                Content = p.Content,
                Id = p.Id,
                CreateTime = p.CreateTime,
                Title = p.Title,
                ImageName = p.ImageName
            }).ToList());
        }

        //
        // GET: /NoticeModel/Details/5

        public ActionResult Details(int id = 0)
        {
            Notices notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            NoticeModel model = new NoticeModel
            {
                Content = notice.Content,
                CreateTime = notice.CreateTime,
                Id=notice.Id,
                Title = notice.Title,
                ImageName = notice.ImageName
            };
            return View(model);
        }

        //
        // GET: /NoticeModel/Create
         [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /NoticeModel/Create
         [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(NoticeModel noticeModel)
        {
            if (ModelState.IsValid)
            {
                string imageName = (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    ? Guid.NewGuid().ToString()
                    : string.Empty;
                var entity = new Notices
                {
                    Content = noticeModel.Content,
                    ImageName = imageName,
                    Title = noticeModel.Title,
                    CreateTime = DateTime.Now
                };
                db.Notices.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(imageName))
                {
                    WebImage webImage = WebImage.GetImageFromRequest();
                    webImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "newsimage", entity.ImageName),
                        ImageFormat.Jpeg.ToString());
                }
                return RedirectToAction("Index");
            }

            return View(noticeModel);
        }

        //
        // GET: /NoticeModel/Edit/5
         [Authorize]
        public ActionResult Edit(int id = 0)
        {
            SchoolDAO.Notices noticeModel = db.Notices.Find(id);
            if (noticeModel == null)
            {
                return HttpNotFound();
            }
            return View(new NoticeModel
            {
                Content = noticeModel.Content,
                CreateTime = noticeModel.CreateTime,
                Id=noticeModel.Id,
                Title = noticeModel.Title
            });
        }

        //
        // POST: /NoticeModel/Edit/5
         [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(NoticeModel noticeModel)
        {
            if (ModelState.IsValid)
            {
                SchoolDAO.Notices find = db.Notices.Find(noticeModel.Id);
               UpdateModel(find);
               db.Entry(find).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(noticeModel);
        }

        //
        // GET: /NoticeModel/Delete/5
         [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Notices notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(new NoticeModel
            {
                Content = notice.Content,
                CreateTime = notice.CreateTime,
                Id = notice.Id,
                Title = notice.Title
            });
        }

        //
        // POST: /NoticeModel/Delete/5
         [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notices notice = db.Notices.Find(id);
            db.Notices.Remove(notice);
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