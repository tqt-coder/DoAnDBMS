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
        private DoAnEntities1 db = new DoAnEntities1();

        // GET: Saches
        public ActionResult Index()
        {
            var saches = db.Saches.Include(s => s.NhaXuatBan).Where(s => s.SoLuong > 1).Take(10);
            return View(saches.ToList());
        }

        public ActionResult Catetogy(string li)
        {

            ViewBag.header = li;
            var saches = db.Saches.Include(s => s.NhaXuatBan)
                 .Where(s => s.TheLoai == li);
            return View(saches.ToList());
        }

        // Post detail

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "MaSach")] ChiTietHoaDon dt)
        {
            if (Session["UserID"] == null)
            {
                // Trả về trang login để đăng nhập
                return RedirectToAction("Login", "KhachHangs");
            }
            int SoLuong = Convert.ToInt32(Request.Params.Get("SoLuong"));
            String date = "2001-01-01";
            db.Don(Convert.ToInt32(Session["ID"].ToString()), DateTime.Parse(date), dt.MaSach, SoLuong);
            return RedirectToAction("ViewCart", "Gios");
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


        // view Search
        public ActionResult Search(String sa)
        {
            var saches = db.Database.SqlQuery<searchBook_Result>("searchBook N'%" + sa + "%'");
            return View(saches.ToList());
        }
        // post search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(Sach sa)
        {
           
            return Search(sa.TenSach);
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
