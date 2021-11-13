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
    public class ChiTietHoaDonsController : Controller
    {
        private DoAnEntities1 db = new DoAnEntities1();

        // GET: ChiTietHoaDons
        public ActionResult Index()
        {
            var chiTietHoaDons = db.ChiTietHoaDons.Include(c => c.DonHang).Include(c => c.Sach);
            return View(chiTietHoaDons.ToList());
        }

        // GET: ChiTietHoaDons/Details/5
        public ActionResult Details(int? id, String maSach)
        {
            if (id == null || maSach == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHoaDon hd = db.ChiTietHoaDons.Where(s => s.MaHD == id && s.MaSach == maSach).SingleOrDefault();
            if (hd == null)
            {
                return HttpNotFound();
            }
            return View(hd);
        }

        // GET: ChiTietHoaDons/Create
        public ActionResult Create()
        {
            ViewBag.MaHD = new SelectList(db.DonHangs, "MaDH", "MaDH");
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach");
            return View();
        }

        // POST: ChiTietHoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,MaSach,SoLuong,ThanhTien")] ChiTietHoaDon chiTietHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietHoaDons.Add(chiTietHoaDon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHD = new SelectList(db.DonHangs, "MaDH", "MaDH", chiTietHoaDon.MaHD);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", chiTietHoaDon.MaSach);
            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDons/Edit/5
        public ActionResult Edit(int? id, String maSach)
        {
            if (id == null || maSach == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHoaDon hd = db.ChiTietHoaDons.Where(s => s.MaHD == id && s.MaSach == maSach).SingleOrDefault();
            if (hd == null)
            {
                return HttpNotFound();
            }
        
            ViewBag.MaHD = new SelectList(db.DonHangs, "MaDH", "MaDH", hd.MaHD);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", hd.MaSach);
            return View(hd);
        }

        public ActionResult Update(ChiTietHoaDon hd)
        {
            db.Entry(hd).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index","Saches");
        }
        // POST: ChiTietHoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,MaSach,SoLuong,ThanhTien")] ChiTietHoaDon chiTietHoaDon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietHoaDon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHD = new SelectList(db.DonHangs, "MaDH", "MaDH", chiTietHoaDon.MaHD);
            ViewBag.MaSach = new SelectList(db.Saches, "MaSach", "TenSach", chiTietHoaDon.MaSach);
            return View(chiTietHoaDon);
        }

        // GET: ChiTietHoaDons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDons.Find(id);
            if (chiTietHoaDon == null)
            {
                return HttpNotFound();
            }
            return View(chiTietHoaDon);
        }

        // POST: ChiTietHoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietHoaDon chiTietHoaDon = db.ChiTietHoaDons.Find(id);
            db.ChiTietHoaDons.Remove(chiTietHoaDon);
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
