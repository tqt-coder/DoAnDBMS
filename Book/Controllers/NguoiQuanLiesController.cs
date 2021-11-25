using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Book.Models;

namespace Book.Controllers
{
    public class NguoiQuanLiesController : Controller
    {
        private DoAnEntities1 db = new DoAnEntities1();

        // GET: NguoiQuanLies
        public ActionResult Index()
        {
            var nguoiQuanLies = db.NguoiQuanLies.Include(n => n.KhachHang);
            return View(nguoiQuanLies.ToList());
        }

        // GET: NguoiQuanLies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiQuanLy nguoiQuanLy = db.NguoiQuanLies.Find(id);
            if (nguoiQuanLy == null)
            {
                return HttpNotFound();
            }
            return View(nguoiQuanLy);
        }

        // GET: NguoiQuanLies/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Gmail");
            return View();
        }

        // POST: NguoiQuanLies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="MaNQL,Ten,MaKH")] NguoiQuanLy nguoiQuanLy)
        {
            if (ModelState.IsValid)
            {
                db.NguoiQuanLies.Add(nguoiQuanLy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Gmail", nguoiQuanLy.MaKH);
            return View(nguoiQuanLy);
        }

        // GET: NguoiQuanLies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiQuanLy nguoiQuanLy = db.NguoiQuanLies.Find(id);
            if (nguoiQuanLy == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Gmail", nguoiQuanLy.MaKH);
            return View(nguoiQuanLy);
        }

        // POST: NguoiQuanLies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNQL,Ten,MaKH")] NguoiQuanLy nguoiQuanLy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nguoiQuanLy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "Gmail", nguoiQuanLy.MaKH);
            return View(nguoiQuanLy);
        }

        // GET: NguoiQuanLies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiQuanLy nguoiQuanLy = db.NguoiQuanLies.Find(id);
            if (nguoiQuanLy == null)
            {
                return HttpNotFound();
            }
            return View(nguoiQuanLy);
        }

        // POST: NguoiQuanLies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NguoiQuanLy nguoiQuanLy = db.NguoiQuanLies.Find(id);
            db.NguoiQuanLies.Remove(nguoiQuanLy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
