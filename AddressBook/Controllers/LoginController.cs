using AddressBook.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AddressBook.Controllers
{
    public class LoginController : Controller
    {
        AdresDefteriEntities1 db = new AdresDefteriEntities1();

        // GET: Login
        public ActionResult Login()
        {
            if (Session["userLogin"] == null)
            {
                return View();
            }
            return RedirectToAction("Index", "Registration");
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                User data = db.User.Where(x =>
                x.Username == model.Username &&
                x.Password == model.Password).SingleOrDefault();

                if (data != null)
                {
                    if (data.isActive == false)
                    {
                        ViewBag.Userchecked = "Bu Kullanıcı Silinmiştir";
                        return View();
                    }
                    Session["userLogin"] = data;
                    return RedirectToAction("Index", "Registration");
                }
               
            }

            return View();
        }


        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(User model)
        {
            User checkUser = db.User.FirstOrDefault(x => x.Mail == model.Mail || x.Username == model.Username);
            if (checkUser != null)
            {
                if (checkUser.Username == model.Username)
                {
                    ViewBag.Userchecked = "Böyle bir kullanıcı kayıtlı";
                    return View();
                }
                if (checkUser.Mail == model.Mail)
                {
                    ViewBag.Userchecked = "Böyle bir Email kayıtlı";
                    return View();
                }
            }

            model.isActive = true;
            db.User.Add(model);
            db.SaveChanges();
            return RedirectToAction("Login", "Login");
        }


        public ActionResult EditProfile()
        {
            if (Session["userLogin"] != null)
            {
                User currentUser = Session["userLogin"] as User;
                User data = db.User.Where(x => x.UserID == currentUser.UserID).SingleOrDefault();
                return View(data);
            }
            return RedirectToAction("Login");
        }


        [HttpPost]
        public ActionResult EditProfile(User model, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {
                if (ProfileImage != null)
                {
                    string filename = $"user_{model.UserID}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/Images/user/{filename}"));
                    model.ProfileImage = filename;
                }
                model.isActive = true;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditProfile");

            }
            return View();
        }



        public ActionResult DeleteProfile()
        {
            User currentUser = Session["userLogin"] as User;
            User data = db.User.Where(x => x.UserID == currentUser.UserID).SingleOrDefault();
            data.isActive = false;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Logout");
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}