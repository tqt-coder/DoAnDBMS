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
    public class SachesController : Controller
    {
        private DoAnEntities db = new DoAnEntities();

        // GET: Saches
        public ActionResult Index()
        {
            var saches = db.Saches.Include(s => s.NhaXuatBan);
            return View(saches.ToList());
        }

        public ActionResult NgoaiNgu()
        {
            var saches = db.Database.SqlQuery<recommend_Result>("recommend N'Ngoại ngữ'");
            return View(saches.ToList());
        }

        public ActionResult ThieuNhi()
        {
            var saches = db.Database.SqlQuery<recommend_Result>("recommend N'Thiếu nhi'");
            return View(saches.ToList());
        }

        public ActionResult KhoaHoc()
        {
            var saches = db.Database.SqlQuery<recommend_Result>("recommend N'Khoa học'");
            return View(saches.ToList());
        }

        public ActionResult KyNangSong()
        {
            var saches = db.Database.SqlQuery<recommend_Result>("recommend N'Kỹ năng sống'");
            return View(saches.ToList());
        }

        public ActionResult TrinhTham()
        {
            //var saches = db.Saches.Include(s => s.NhaXuatBan)
            //     .Where(s => s.TheLoai == "Trinh thám");
            var saches = db.Database.SqlQuery<recommend_Result>("recommend N'Trinh thám'");

            return View(saches.ToList());
        }

        // GET: Saches/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: Saches/Create
        public ActionResult Create()
        {
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB");
            return View();
        }

        // POST: Saches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaSach,TenSach,TenTacGia,MaNXB,TheLoai,SoLuong,GiaBan,HinhAnh")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Saches.Add(sach);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        // GET: Saches/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        // POST: Saches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSach,TenSach,TenTacGia,MaNXB,TheLoai,SoLuong,GiaBan,HinhAnh")] Sach sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans, "MaNXB", "TenNXB", sach.MaNXB);
            return View(sach);
        }

        // GET: Saches/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Saches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Sach sach = db.Saches.Find(id);
            db.Saches.Remove(sach);
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
