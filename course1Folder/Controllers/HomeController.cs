using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace course1Folder.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            
            return View();
        }

        [Authorize]
        public ActionResult Index2()
        {
            return View();

        }
    }
}