using course1Folder.CustomAuthorization;
using course1Folder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace course1Folder.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //при первом входе на страницу
        public ActionResult Registration()
        {
            var regModel = new RegistrationModel() {isShared = true};
            return View(regModel);
        }

        //при обновнелии или непрохождении валидации
        [HttpPost]
        public ActionResult Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                var salt = BLL.Hash.CreateSalt(16);
                var passhash = BLL.Hash.GenerateSaltedHash(model.Password, salt);
                var result = BLL.Data.CreateOrUpdateUser(new BLL.DTO.UserDTO
                {
                    Salt = Convert.ToBase64String(salt),
                    PasswordHash = Convert.ToBase64String(passhash),
                    Nickname = model.Nickname,
                    BirthDate = model.BirthDate,
                    RegDate = DateTime.Now,
                    SharedProfile = model.isShared,
                    Login = model.Login
                });
                ViewBag.Message = result;
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }


            return View(model);
        }

        //при первом входе на страницу
        public ActionResult Login(string returnUrl = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOut();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();

        }

        [HttpPost]
        public ActionResult Login(Models.LoginModel model, string ReturnUrl = "")
        { 
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.login, model.password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(model.login, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new Models.CustomSerializeModel()
                        {
                            userId = user.userId,
                            nick = user.nickname,
                            email = user.Email,
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, model.login, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                        Response.Cookies.Add(faCookie);
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Profile");
                    }
                }
            }
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(model);
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
        }
    }
}