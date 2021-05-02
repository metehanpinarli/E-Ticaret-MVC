using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Ticaret_4Son.Models;
using Microsoft.AspNet.Identity;

namespace E_Ticaret_4Son.Controllers
{
    [Authorize]
    public class SepetsController : Controller
    {
        private E_TicaretDBEntities db = new E_TicaretDBEntities();

        // GET: Sepets
        public ActionResult Index()
        {
            string UserId = User.Identity.GetUserId();

            var sepet = db.Sepet.Where(a => a.RefAspNetUserID == UserId).Include(s => s.Urunler);
            return View(sepet.ToList());
        }
        public ActionResult SepeteEkle(int? adet, int id)// ? adet boş gelirse bir ekler null döndürmz
        {
            string userId = User.Identity.GetUserId();//kullanıcı ıd alırız 
            Sepet SepettekiUrun = db.Sepet.FirstOrDefault(a => a.RefUrunID == id && a.RefAspNetUserID == userId);
            Urunler Urun = db.Urunler.Find(id);
            //int ToplamTutar = (adet ?? 1) * Urun.UrunFiyati;
            if (SepettekiUrun == null)//sepet boş
            {
                Sepet YeniUrun = new Sepet()
                {
                    RefAspNetUserID = userId,
                    RefUrunID = id,
                    Adet = adet ?? 1,
                    ToplamTutar = (adet ?? 1) * Urun.UrunFiyati
                };
                db.Sepet.Add(YeniUrun);
                db.SaveChanges();

            }
            else
            {
                SepettekiUrun.Adet = SepettekiUrun.Adet + (adet ?? 1);
                SepettekiUrun.ToplamTutar = SepettekiUrun.Adet * Urun.UrunFiyati;

            }
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
