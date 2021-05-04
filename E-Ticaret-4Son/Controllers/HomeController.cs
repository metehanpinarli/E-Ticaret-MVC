
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Text.RegularExpressions;
using E_Ticaret_4Son.Models;

namespace E_Ticaret_4Son.Controllers
{
    public class HomeController : Controller
    {
        E_TicaretDBEntities db = new E_TicaretDBEntities();
        public ActionResult Index()
        {
            ViewBag.KategoriListesi = db.Kategoriler.OrderByDescending(c=>c.KategoriID).ToList();
            ViewBag.SonKategoriler = db.Kategoriler.OrderByDescending(k => k.KategoriID).Skip(0).Take(7).ToList();
            ViewBag.SonUrunler = db.Urunler.OrderByDescending(u => u.UrunID).Skip(0).Take(8).ToList();

            //foreach (var item in db.Urunler)
            //{
            //    if (item.UrunAdı.Length > 44)
            //    {
            //        item.UrunAdı = item.UrunAdı.Substring(0, 44);
            //        item.UrunAdı = item.UrunAdı + "...";
            //    }
            //}

            return View();
        }
        public ActionResult Kategori(int id)
        {
            ViewBag.KategoriListesi = db.Kategoriler.ToList();
            ViewBag.Kategori = db.Kategoriler.Find(id);

            foreach (var item in db.Urunler)
            {
                if (item.UrunAdı.Length > 44)
                {
                    item.UrunAdı = item.UrunAdı.Substring(0, 44);
                    item.UrunAdı = item.UrunAdı + "...";
                }
            }

            return View(db.Urunler.Where(a => a.RefKategoriID == id).OrderBy(a => a.UrunAdı).ToList());

        }

        public ActionResult Urun(int id)
        {

            ViewBag.KategoriListesi = db.Kategoriler.ToList();

            return View(db.Urunler.Find(id));

        }
    }
}