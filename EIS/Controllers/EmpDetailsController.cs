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
    public class EmpDetailsController : Controller
    {
        private EmployeeEntities db = new EmployeeEntities();
      



        // GET: EmpDetails
        public ActionResult Index()
        {
          return View(db.EmpDetails.ToList());
        }

        // GET: EmpDetails/Details/5
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

        // GET: EmpDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpDetails/Create
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

        // GET: EmpDetails/Edit/5
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

        // POST: EmpDetails/Edit/5
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



        // GET: EmpDetails/Delete/5
        public ActionResult Delete(int?  id)
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

        // POST: EmpDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EmpDetail empDetail = db.EmpDetails.Find(id);
            db.EmpDetails.Remove(empDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        
        public void Downloads(int? id)
        {
            if (id != null)
            {
                EmpDetail det = db.EmpDetails.Find(id);
                var sb = new StringBuilder();
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", "Employee Id", "Employee Name", "Date Of Birth ", "Contact Number", "Email Id", "PAN", "Address", "Designation", "Salary", Environment.NewLine);
                sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", det.Emp_id, (det.Fname.ToString() + " " + det.Lname.ToString()), det.DOB.ToString().Substring(0, 11), det.ContactNo, det.Email_id.ToString(), det.PAN, det.Address, det.Designation, det.Salary, Environment.NewLine);

                var response = System.Web.HttpContext.Current.Response;
                response.BufferOutput = true;
                response.Clear();
                response.ClearHeaders();
                response.ContentEncoding = Encoding.Unicode;
                response.AddHeader("content-disposition", "attachment;filename=YourDetails.CSV ");
                response.ContentType = "text/plain";
                response.Write(sb.ToString());
                response.End();
            }
           
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
