using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolSite.Models;

namespace SchoolSite.Controllers
{
    public class NoticeController : Controller
    {
        private SchoolDbContext db = new SchoolDbContext();

        //
        // GET: /Notice/

        public ActionResult Index()
        {
            if (this.Request.IsAjaxRequest())
            {
                return PartialView("_NoticeList", db.Notices.Take(11).ToList());
            }
            return View(db.Notices.ToList());
        }

        //
        // GET: /Notice/Details/5

        public ActionResult Details(int id = 0)
        {
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        //
        // GET: /Notice/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Notice/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Notice notice)
        {
            if (ModelState.IsValid)
            {
               
                    db.Notices.Add(notice);
                    db.SaveChanges();
               
                
                return RedirectToAction("Index");
            }

            return View(notice);
        }

        //
        // GET: /Notice/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        //
        // POST: /Notice/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Notice notice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        //
        // GET: /Notice/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        //
        // POST: /Notice/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notice notice = db.Notices.Find(id);
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