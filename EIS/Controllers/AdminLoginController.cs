using EIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIS.Controllers
{
    public class AdminLoginController : Controller
    {

        EmployeeEntities db1 = new EmployeeEntities();
        // GET: AdminLogin
        public ActionResult Index()
        {
            Session["Regname"] = null;
            return View();
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EmpDetail objm)
        {

            if(ModelState.IsValid)
            {
                using(EmployeeEntities db1 = new EmployeeEntities())
                {
                    var k = "admin@gmail.com";
                    var obj1 = db1.EmpDetails.Where(b => k.Equals(objm.Email_id) && b.Password.Equals(objm.Password)).FirstOrDefault();
                    if (obj1 != null)
                    {
                        Session["name"] = obj1.Designation.ToString();
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Admin id or password is incorrect");
                    }
                }
            }

            
            return View(objm);
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "AdminLogin");
        }
    }
}