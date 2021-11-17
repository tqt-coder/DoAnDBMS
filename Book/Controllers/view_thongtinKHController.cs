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
    public class view_thongtinKHController : Controller
    {
        private DoAnEntities1 db = new DoAnEntities1();

        // GET: view_thongtinKH
        public ActionResult Index()
        {
<<<<<<< HEAD
            return View(db.view_thongtinKH.Distinct().ToList());
        }

        // GET: view_thongtinKH/Details/5
        public ActionResult Details(int? id)
=======
            return View(db.view_thongtinKH.ToList());
        }

        // GET: view_thongtinKH/Details/5
        public ActionResult Details(DateTime id)
>>>>>>> 4e476ba05ccde6dfe595d44dc2e9f6770c07ff0a
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            view_thongtinKH view_thongtinKH = db.view_thongtinKH.Find(id);
            if (view_thongtinKH == null)
            {
                return HttpNotFound();
            }
            return View(view_thongtinKH);
        }

        // GET: view_thongtinKH/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: view_thongtinKH/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public ActionResult Create([Bind(Include = "MaKH,HovaTen,SoDienThoai,MaDH,NgayDat,NgayNhan,TenSach,TongTien")] view_thongtinKH view_thongtinKH)
=======
        public ActionResult Create([Bind(Include = "HovaTen,NgayDat,NgayNhan,TenSach,TongTien,MaKH")] view_thongtinKH view_thongtinKH)
>>>>>>> 4e476ba05ccde6dfe595d44dc2e9f6770c07ff0a
        {
            if (ModelState.IsValid)
            {
                db.view_thongtinKH.Add(view_thongtinKH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(view_thongtinKH);
        }

        // GET: view_thongtinKH/Edit/5
<<<<<<< HEAD
        public ActionResult Edit(int? id)
=======
        public ActionResult Edit(DateTime id)
>>>>>>> 4e476ba05ccde6dfe595d44dc2e9f6770c07ff0a
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            view_thongtinKH view_thongtinKH = db.view_thongtinKH.Find(id);
            if (view_thongtinKH == null)
            {
                return HttpNotFound();
            }
            return View(view_thongtinKH);
        }

        // POST: view_thongtinKH/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public ActionResult Edit([Bind(Include = "MaKH,HovaTen,SoDienThoai,MaDH,NgayDat,NgayNhan,TenSach,TongTien")] view_thongtinKH view_thongtinKH)
=======
        public ActionResult Edit([Bind(Include = "HovaTen,NgayDat,NgayNhan,TenSach,TongTien,MaKH")] view_thongtinKH view_thongtinKH)
>>>>>>> 4e476ba05ccde6dfe595d44dc2e9f6770c07ff0a
        {
            if (ModelState.IsValid)
            {
                db.Entry(view_thongtinKH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(view_thongtinKH);
        }

        // GET: view_thongtinKH/Delete/5
<<<<<<< HEAD
        public ActionResult Delete(int? id)
=======
        public ActionResult Delete(DateTime id)
>>>>>>> 4e476ba05ccde6dfe595d44dc2e9f6770c07ff0a
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            view_thongtinKH view_thongtinKH = db.view_thongtinKH.Find(id);
            if (view_thongtinKH == null)
            {
                return HttpNotFound();
            }
            return View(view_thongtinKH);
        }

        // POST: view_thongtinKH/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public ActionResult DeleteConfirmed(int id)
=======
        public ActionResult DeleteConfirmed(DateTime id)
>>>>>>> 4e476ba05ccde6dfe595d44dc2e9f6770c07ff0a
        {
            view_thongtinKH view_thongtinKH = db.view_thongtinKH.Find(id);
            db.view_thongtinKH.Remove(view_thongtinKH);
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
