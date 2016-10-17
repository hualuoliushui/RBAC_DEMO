using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormsAuth_Demo2.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index(string userName)
        {
            ViewData["userName"] = userName;
            return View();
        }

        public JsonResult Admin(string userName)
        {
            return Json(new { userName = userName, userAge = 12 }, JsonRequestBehavior.AllowGet);
        }
    }
}
