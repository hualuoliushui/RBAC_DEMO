using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FormsAuth_Demo2.Models;
using DAO.DAOFactory;
using DAO.DAO;
using DAO.DAOVO;

namespace FormsAuth_Demo2.Controllers
{
    public class Result
    {
        public List<int> codes;
        public string msg;
        public object resultObject;
    }

    public class AccountController : Controller
    {
        //
        // GET: /Account/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {

            string userName = form["userName"];
            string password = form["password"];
            if (!Forms.Verigy(userName, password))
            {
                RedirectToAction("Index");
            }
            Forms.Login(userName, 20);
            return RedirectToAction("Index", "Admin", new { userName = userName });
        }

    }
}
