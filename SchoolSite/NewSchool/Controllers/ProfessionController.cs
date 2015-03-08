﻿using System;
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
    public class ProfessionController : Controller
    {
        private readonly SchoolString db = new SchoolString();

        //
        // GET: /News/

        public ActionResult Index()
        {
            return View(db.Profession.OrderByDescending(p => p.CreateTime).Select(p => new ProfessionModel
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
            Profession find = db.Profession.Find(id);

            if (find == null)
            {
                return HttpNotFound();
            }
            var model = new ProfessionModel
            {
                Content = find.Content,
                Title = find.Title,
                CreateTime = find.CreateTime,
                Id = find.Id,
                ImageName = find.ImageName

            };
            return View(model);
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
        public ActionResult Create(ProfessionModel showModel)
        {
            if (ModelState.IsValid)
            {

                string imageName = (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    ? Guid.NewGuid().ToString()
                    : string.Empty;
                var entity = new Profession
                {
                    Content = showModel.Content,
                    ImageName = imageName,
                    Title = showModel.Title,
                    CreateTime = DateTime.Now
                };
                db.Profession.Add(entity);
                db.SaveChanges();
                if (!string.IsNullOrEmpty(imageName))
                {
                    WebImage webImage = WebImage.GetImageFromRequest();
                    webImage.Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "newsimage", entity.ImageName),
                        ImageFormat.Jpeg.ToString());
                }


                return RedirectToAction("Index");
            }

            return View(showModel);
        }

        //
        // GET: /News/Edit/5
         [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Profession find = db.Profession.Find(id);
            if (find == null)
            {
                return HttpNotFound();
            }
            var model = new ProfessionModel
            {
                Id = find.Id,
                Title = find.Title,
                Content = find.Content,
                CreateTime = find.CreateTime,
                ImageName = find.ImageName
            };
            return View(model);
        }

        //
        // POST: /News/Edit/5
         [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(ProfessionModel showModel)
        {
            if (ModelState.IsValid)
            {
                Profession entity = db.Profession.Find(showModel.Id);
                UpdateModel(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(showModel);
        }

        //
        // GET: /News/Delete/5
         [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Profession find = db.Profession.Find(id);
            if (find == null)
            {
                return HttpNotFound();
            }
            var model = new ProfessionModel
            {
                Id = find.Id,
                Title = find.Title,
                Content = find.Content,
                CreateTime = find.CreateTime,
                ImageName = find.ImageName
            };
            return View(model);
        }

        //
        // POST: /News/Delete/5
         [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profession find = db.Profession.Find(id);
            db.Profession.Remove(find);
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