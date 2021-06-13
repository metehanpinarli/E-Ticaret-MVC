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
    public class SiparisController : Controller
    {
        private E_TicaretDBEntities db = new E_TicaretDBEntities();

        // GET: Siparis
        public ActionResult Index()
        {
            var siparis = db.Siparis;
            return View(siparis.ToList());
        }
        public ActionResult SiparisDetay(int id)
        {
            return View(db.SiparisKalem.Where(a=>a.RefSiparisID==id).ToList());
        }

        public ActionResult SiparisTamamla()
        {
            string userID = User.Identity.GetUserId();
            List<Sepet> sepetUrunleri = db.Sepet.Where(a => a.RefAspNetUserID == userID).ToList();
            string ClientId = "100300000";
            string Amount = sepetUrunleri.Sum(t => t.ToplamTutar).ToString();
            string Oid = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            string OnayURL = "https://localhost:44367/Siparis/Tamamlandi";
            string HataURL = "https://localhost:44367/Siparis/Hatali";
            string RDN = "asdf";
            string StoreKey = "123456";
            string TransActionType = "Auth";
            string Instalment = "";
            string HashStr = ClientId + Oid + Amount + OnayURL + HataURL + TransActionType + Instalment + RDN + StoreKey;
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] HashBytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(HashStr);
            byte[] InputBytes = sha.ComputeHash(HashBytes);
            string Hash = Convert.ToBase64String(InputBytes);
            ViewBag.ClientId = ClientId;
            ViewBag.Oid = Oid;
            ViewBag.okUrl = OnayURL;
            ViewBag.failUrl = HataURL;
            ViewBag.TransActionType = TransActionType;
            ViewBag.RDN = RDN;
            ViewBag.Hash = Hash;
            ViewBag.Amount = Amount;
            ViewBag.StoreType ="3d_pay_hosting";
            ViewBag.Description = "";
            ViewBag.XID = "";
            ViewBag.Lang= "tr";
            ViewBag.Email= "meteerkan1919@gmail.com";
            ViewBag.UserID = "11c Sinifi";
            ViewBag.PostURL = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate";
            return View();
        }

    }
}
