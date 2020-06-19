using EIS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIS.Controllers
{
    public class LoginController : Controller
    {

        EmployeeEntities db = new EmployeeEntities();
        // GET: Login
        public ActionResult Index()
        {
            Session["Regname"] = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EmpDetail objchk)
        {
            if(ModelState.IsValid)
            {
                using(EmployeeEntities db = new EmployeeEntities())
                {
                  
                    var obj = db.EmpDetails.Where(a => a.Email_id.Equals(objchk.Email_id) && a.Password.Equals(objchk.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Emp_id"] = obj.Emp_id.ToString();
                        Session["Fname"] = obj.Fname.ToString();
                        return RedirectToAction("Index", "EmpDetails");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The Email_id or password is incorrect");
                    }


                }

            }
           
            return View(objchk);
        }
        
        public ActionResult Register()
          {
              return View();
          }
          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Register([Bind(Include = "Emp_id,Fname,Lname,DOB,ContactNo,Email_id,PAN,Address,Password,Designation,Salary")] EmpDetail empDetail)
          {
            if (ModelState.IsValid)
            {

                Session["RegName"] = empDetail.Fname.ToString() + empDetail.Lname.ToString();


                db.EmpDetails.Add(empDetail);
                db.SaveChanges();
                return RedirectToAction("Register", "Login");
            }
            else
            {

                ModelState.AddModelError("", "Sorry could not create your account,maybe email id already exists");
                return RedirectToAction("Register", "Login");
            }
          }

        


         
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}