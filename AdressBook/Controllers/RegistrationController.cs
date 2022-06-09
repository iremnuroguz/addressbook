using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AddressBook.Models.Entity;

namespace AddressBook.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Entry
        AdresDefteriEntities1 db = new AdresDefteriEntities1();
        public ActionResult Index()
        {
            if (Session["userLogin"] != null)
            {
                User user = Session["userLogin"] as User;
                var degerler = db.Registration.Where(x => x.UserId == user.UserID && x.isActive == true).ToList();
                return View(degerler);
            }
            return RedirectToAction("Login", "Login");
        }
        [HttpGet]
        public ActionResult Add()
        {
            if (Session["userLogin"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Registration");
        }

        [HttpPost]
        public ActionResult Add(Registration p1, HttpPostedFileBase file)
        {
            User userLogin = Session["userLogin"] as User;

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    p1.Image = new byte[file.ContentLength];
                    file.InputStream.Read(p1.Image, 0, file.ContentLength);
                }
                else
                {
                    p1.Image = null;
                }

                p1.UserId = userLogin.UserID;
                p1.isActive = true;
                p1.CreatedDate = DateTime.Now;
                db.Registration.Add(p1);
                //db.Entry(p1).State = EntityState.Added;
                db.SaveChanges();
                return View();
            }

            return View(p1);

        }

        public ActionResult Update(int id)
        {
            Registration kayit = db.Registration.Where(x => x.Id == id).SingleOrDefault();
            return View(kayit);
        }


        [HttpPost]
        public ActionResult Update(Registration model, HttpPostedFileBase file)
        {
            User user = Session["userLogin"] as User;
            Registration kayit = db.Registration.Where(x => x.Id == model.Id).SingleOrDefault();

            if (file != null)
            {
                kayit.Image = new byte[file.ContentLength];
                file.InputStream.Read(kayit.Image, 0, file.ContentLength);
            }
            else
            {
                if (kayit.Image == null)
                {
                    kayit.Image = null;
                }
            }

            kayit.UserId = user.UserID;
            kayit.Name = model.Name;
            kayit.Surname = model.Surname;
            kayit.Phone1 = model.Phone1;
            kayit.Phone2 = model.Phone2;
            kayit.Mail1 = model.Mail1;
            kayit.Mail2 = model.Mail2;
            kayit.Adress1 = model.Adress1;
            kayit.Adress2 = model.Adress2;
            kayit.CreatedDate = model.CreatedDate;
            kayit.isActive = true;
            db.Entry(kayit).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Registration kayit = db.Registration.Find(id);
            kayit.isActive = false;
            db.Entry(kayit).State = EntityState.Modified;
            //db.TBL_ENTRYS.Remove(kayit);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}