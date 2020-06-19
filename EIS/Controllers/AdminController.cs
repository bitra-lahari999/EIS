using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EIS.Models;

namespace EIS.Controllers
{
    public class AdminController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();

        // GET: Admin
        public ActionResult Index()
        {
            Session["Fname"] = "admin";
            return View(db.EmpDetails.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpDetail empDetail = db.EmpDetails.Find(id);
            if (empDetail == null)
            {
                return HttpNotFound();
            }
            return View(empDetail);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Emp_id,Fname,Lname,DOB,ContactNo,Email_id,PAN,Address,Password,Designation,Salary")] EmpDetail empDetail)
        {
            if (ModelState.IsValid)
            {
                db.EmpDetails.Add(empDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empDetail);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpDetail empDetail = db.EmpDetails.Find(id);
            if (empDetail == null)
            {
                return HttpNotFound();
            }
            return View(empDetail);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Emp_id,Fname,Lname,DOB,ContactNo,Email_id,PAN,Address,Password,Designation,Salary")] EmpDetail empDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empDetail);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpDetail empDetail = db.EmpDetails.Find(id);
            if (empDetail == null)
            {
                return HttpNotFound();
            }
            return View(empDetail);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            EmpDetail empDetail = db.EmpDetails.Find(id);
            db.EmpDetails.Remove(empDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileContentResult Download()
        {
            String details = "-------Employee Details---------";
            if (Session["Fname"] != null)
            {
                List<EmpDetail> emplist = db.EmpDetails.ToList();
                foreach (EmpDetail det in emplist)
                {
                    details = details + "\n\nEmployee ID:\t" + det.Emp_id.ToString() +
                              ",\nEmployee Name:\t" + det.Fname.ToString() + " " + det.Lname.ToString() +
                              ",\nDate Of Birth:\t" + det.DOB.ToString().Substring(0, 10) +
                              ",\nContact Number:\t" + det.ContactNo +
                              ",\nEmail Id:\t" + det.Email_id.ToString() +
                              ",\nPAN:\t" + det.PAN +
                              ",\nAddress:\t" + det.Address +
                              ",\nDesignation:\t" + det.Designation +
                              ",\nSalary:\t" + det.Salary + "\n\n";
                }
                byte[] csvBytes = Encoding.UTF8.GetBytes(details); // or get your bytes the way you want

                string contentType = "text/csv";
                var result = new FileContentResult(csvBytes, contentType);

                return result;
            }
            else return new FileContentResult(Encoding.UTF8.GetBytes("no details"), "text/csv");
        }
        public void ExportCSV_Employee()
        {
            var sb = new StringBuilder();
            List <EmpDetail> emplist = db.EmpDetails.ToList();
            sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", "Employee Id", "Employee Name", "Date Of Birth ", "Contact Number", "Email Id", "PAN", "Address","Designation","Salary", Environment.NewLine);


            foreach (EmpDetail det in emplist)
            {
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", det.Emp_id, (det.Fname.ToString() + " " + det.Lname.ToString()) , det.DOB.ToString().Substring(0, 11), det.ContactNo, det.Email_id.ToString(), det.PAN, det.Address, det.Designation, det.Salary, Environment.NewLine);


            }
            //Get Current Response  
            var response = System.Web.HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=Employee.CSV ");
            response.ContentType = "text/plain";
            response.Write(sb.ToString());
            response.End();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
