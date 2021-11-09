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
    public class GiosController : Controller
    {
        private DoAnEntities1 db = new DoAnEntities1();

        // GET: Gios
        public ActionResult ViewCart()
        {
            if (Session["UserID"] != null)
            {
                int maKH = Convert.ToInt32(Session["ID"]);
                var cart = db.Database.SqlQuery<ViewCart_Result>("ViewCart " + maKH);
                return View(cart.ToList());
            }
            else
            {
                return RedirectToAction("Login","KhachHangs");
            }
           
           
        }

        // GET: Gios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gio gio = db.Gios.Find(id);
            if (gio == null)
            {
                return HttpNotFound();
            }
            return View(gio);
        }

        // GET: Gios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Gios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHD,MaSach,SoLuong,ThanhTien,HinhAnh,MaKH")] Gio gio)
        {
            if (ModelState.IsValid)
            {
                db.Gios.Add(gio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gio);
        }

        // GET: Gios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gio gio = db.Gios.Find(id);
            if (gio == null)
            {
                return HttpNotFound();
            }
            return View(gio);
        }

        // POST: Gios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHD,MaSach,SoLuong,ThanhTien,HinhAnh,MaKH")] Gio gio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gio);
        }


        // POST: Gios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? MaHD, string MaSach)
        {
            db.XoaHoaDon(MaHD, MaSach);
            db.SaveChanges();
            return RedirectToAction("ViewCart");
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
