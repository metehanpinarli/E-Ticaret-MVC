using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Ticaret_4Son.Models;

namespace E_Ticaret_4Son.Controllers
{
    public class UrunlersController : Controller
    {
        private E_TicaretDBEntities db = new E_TicaretDBEntities();

        // GET: Urunlers
        public ActionResult Index()
        {
            var urunler = db.Urunler.Include(u => u.Kategoriler);
            foreach (var item in db.Urunler)
            {
                if (item.UrunAdı.Length > 60)
                {
                    item.UrunAdı = item.UrunAdı.Substring(0, 60);
                    item.UrunAdı = item.UrunAdı + "...";
                }
            }
            return View(urunler.ToList());
        }

        // GET: Urunlers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // GET: Urunlers/Create
        public ActionResult Create()
        {
            ViewBag.RefKategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdı");
            return View();
        }

        // POST: Urunlers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "UrunID,UrunAdı,RefKategoriID,UrunAciklamasi,UrunFiyati")] Urunler urunler, HttpPostedFileBase UrunResim)
        {
            if (ModelState.IsValid)
            {
                db.Urunler.Add(urunler);
                db.SaveChanges();
                if (UrunResim != null && UrunResim.ContentLength > 0)
                {
                    String filePath = Path.Combine(Server.MapPath("~/İmage"), urunler.UrunID + ".jpg");
                    UrunResim.SaveAs(filePath);
                }
                return RedirectToAction("Index");
            }

            ViewBag.RefKategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdı", urunler.RefKategoriID);
            return View(urunler);
        }

        // GET: Urunlers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            ViewBag.RefKategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdı", urunler.RefKategoriID);
            return View(urunler);
        }

        // POST: Urunlers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "UrunID,UrunAdı,RefKategoriID,UrunAciklamasi,UrunFiyati")] Urunler urunler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(urunler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RefKategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdı", urunler.RefKategoriID);
            return View(urunler);
        }

        // GET: Urunlers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Urunler urunler = db.Urunler.Find(id);
            if (urunler == null)
            {
                return HttpNotFound();
            }
            return View(urunler);
        }

        // POST: Urunlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Urunler urunler = db.Urunler.Find(id);
            db.Urunler.Remove(urunler);
            db.SaveChanges();
            String filePath = Path.Combine(Server.MapPath("~/İmage"), urunler.UrunID + ".jpg");

            FileInfo fi = new FileInfo(filePath);

            if (fi.Exists)
                fi.Delete();
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
