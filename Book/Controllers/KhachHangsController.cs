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
    public class KhachHangsController : Controller
    {
        private DoAnEntities1 db = new DoAnEntities1();


        // GET: KhachHangs
        public ActionResult Index()
        {
            return View(db.KhachHangs.ToList());
        }

        public ActionResult Purchases()
        {
            return View(db.KhachHangs.ToList());
        }
        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                if (Session["ad"].ToString() == "manager")
                {
                    

                        return View();
                   

                }
                else
                {

                    return RedirectToAction("Login");
                }
            }
            else
            {

                return RedirectToAction("Login");

            }
        } 


            // Login
            public ActionResult Login()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Login(KhachHang user)
            {
                if (ModelState.IsValid)
                {

                    var obj = db.KhachHangs.Where(a => a.Gmail.Equals(user.Gmail) && a.PassWord.Equals(user.PassWord)).FirstOrDefault();
                    if (obj != null)
                    {

                        Session["UserID"] = obj.Gmail.ToString();
                        Session["UserName"] = obj.Gmail.ToString();
                        Session["ID"] = obj.MaKH.ToString();
                        Session["ad"] = null;
                        if (obj.Quyen.ToString() == "admin")
                        {
                            Session["ad"] = "manager";
                            return RedirectToAction("UserDashBoard");
                        }
                        return RedirectToAction("Index", "Saches");
                    }

                }
                ViewBag.error = "Login failed";
                return View(user);
            }


            // GET: KhachHangs/Details/5
            public ActionResult Details(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                KhachHang khachHang = db.KhachHangs.Find(id);
                if (khachHang == null)
                {
                    return HttpNotFound();
                }
                return View(khachHang);
            }

            // GET: KhachHangs/Create
            public ActionResult Register()
            {
                return View();
            }

            // POST: KhachHangs/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Register([Bind(Include = "MaKH,HovaTen,SoDienThoai,DiaChi,PassWord,Gmail,Quyen")] KhachHang khachHang)
            {
                if (ModelState.IsValid)
                {
                    db.KhachHangs.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(khachHang);
            }

            // GET: KhachHangs/Edit/5
            public ActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                KhachHang khachHang = db.KhachHangs.Find(id);
                if (khachHang == null)
                {
                    return HttpNotFound();
                }
                return View(khachHang);
            }

            // POST: KhachHangs/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "MaKH,HovaTen,SoDienThoai,DiaChi,PassWord,Gmail,Quyen")] KhachHang khachHang)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(khachHang).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(khachHang);
            }

            // GET: KhachHangs/Delete/5
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                KhachHang khachHang = db.KhachHangs.Find(id);
                if (khachHang == null)
                {
                    return HttpNotFound();
                }
                return View(khachHang);
            }

            // POST: KhachHangs/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int? id)
            {
                KhachHang khachHang = db.KhachHangs.Find(id);
                db.KhachHangs.Remove(khachHang);
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
