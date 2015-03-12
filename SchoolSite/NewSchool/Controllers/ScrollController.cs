using System;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using NewSchool.Models;
using SchoolDAO;

namespace NewSchool.Controllers
{
    public class ScrollController : Controller
    {
        private readonly SchoolString db = new SchoolString();

        //
        // GET: /News/

    
        // GET: /News/Details/5

      

        //
        // GET: /News/Create
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(ScrollModel showModel)
        {
            if (ModelState.IsValid)
            {

                string imageName = (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    ? Guid.NewGuid().ToString()
                    : string.Empty;
                var entity = new ScrollImage
                {
                    Content = "Test",
                    ImageName = imageName,
                    Title = "Test",
                    CreateTime = DateTime.Now
                };
                db.ScrollImage.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(imageName))
                {
                    WebImage webImage = WebImage.GetImageFromRequest();
                    webImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "newsimage", entity.ImageName),
                        ImageFormat.Jpeg.ToString());
                }

                return RedirectToAction("Index","Scroll");
            }

            return RedirectToAction("Index", "Scroll");
        }

        //
        // GET: /News/Edit/5
     

        public ActionResult GetImage(int id)
        {
            ScrollImage newsModel = db.ScrollImage.Find(id);
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


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}