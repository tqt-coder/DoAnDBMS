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
    public class NhaXuatBansController : Controller
    {
        private DoAnEntities1 db = new DoAnEntities1();

        // GET: NhaXuatBans
        public ActionResult Index()
        {
            return View(db.NhaXuatBans.ToList());
        }

        // GET: NhaXuatBans/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaXuatBan nhaXuatBan = db.NhaXuatBans.Find(id);
            if (nhaXuatBan == null)
            {
                return HttpNotFound();
            }
            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NhaXuatBans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNXB,TenNXB")] NhaXuatBan nhaXuatBan)
        {
            if (ModelState.IsValid)
            {
                db.NhaXuatBans.Add(nhaXuatBan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaXuatBan nhaXuatBan = db.NhaXuatBans.Find(id);
            if (nhaXuatBan == null)
            {
                return HttpNotFound();
            }
            return View(nhaXuatBan);
        }

        // POST: NhaXuatBans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNXB,TenNXB")] NhaXuatBan nhaXuatBan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaXuatBan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhaXuatBan);
        }

        // GET: NhaXuatBans/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaXuatBan nhaXuatBan = db.NhaXuatBans.Find(id);
            if (nhaXuatBan == null)
            {
                return HttpNotFound();
            }
            return View(nhaXuatBan);
        }

        // POST: NhaXuatBans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhaXuatBan nhaXuatBan = db.NhaXuatBans.Find(id);
            db.NhaXuatBans.Remove(nhaXuatBan);
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
