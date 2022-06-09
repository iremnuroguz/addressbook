using AddressBook.Models.Entity;
using AddressBook.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AddressBook.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Help()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Help(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(model.Mail);
                message.To.Add(model.Mail);
                message.Subject = model.Subject;

                
                message.Body = "Sayın " + model.Name + " " + model.Surname +" Talebiniz alınmıştır. ";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("", "");
                smtp.EnableSsl = true;
                smtp.Send(message);
                return RedirectToAction("Index", "Registration");

            }

            return View();
        }
    }
}