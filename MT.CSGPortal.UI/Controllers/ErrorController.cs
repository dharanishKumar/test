using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MT.CSGPortal.UI.Controllers
{
    public class ErrorController : Controller
    {
       
        // GET: /Error/
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View("Error");    
        }
	}
}