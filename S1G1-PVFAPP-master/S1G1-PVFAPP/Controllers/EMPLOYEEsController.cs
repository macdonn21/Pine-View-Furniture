using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using S1G1_PVFAPP.Models;

namespace S1G1_PVFAPP.Controllers
{
    public class EMPLOYEEsController : Controller
    {
        private Entities3 db = new Entities3();


        // GET: EMPLOYEEs
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var employees = db.EMPLOYEEs.ToList();
            ViewBag.E_Count = db.EMPLOYEEs.ToList().Count();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    employees = db.EMPLOYEEs.Where(employee => employee.EmployeeName.ToUpper().Contains(searchString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        employees = employees.OrderByDescending(a => a.EmployeeName).ToList();
                        break;
                    case "id_desc":
                        employees = employees.OrderByDescending(a => a.EmployeeID).ToList();
                        break;
                    default:
                        employees = employees.OrderBy(u => u.EmployeeName).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(employees.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new EMPLOYEE());
        }


        // GET: EMPLOYEEs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEEs.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName");
            return View();
        }

        // POST: EMPLOYEEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,EmployeeName,EmployeeAddress,Supervisor")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
              
                var count = db.EMPLOYEEs.ToList().Count();
                if (count == 0)
                {
                    eMPLOYEE.EmployeeID = "1";
                   eMPLOYEE.Supervisor = "CEO";
                    db.EMPLOYEEs.Add(eMPLOYEE);
                }
                else
                {
                    eMPLOYEE.EmployeeID = (count + 1).ToString();

                    string supervisor = (db.EMPLOYEEs.Find(eMPLOYEE.Supervisor).EmployeeName) ?? " ";
                    eMPLOYEE.Supervisor = supervisor;
                    db.EMPLOYEEs.Add(eMPLOYEE);
                }
              
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", eMPLOYEE.EmployeeID);
          
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEEs.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", eMPLOYEE.EmployeeID);
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,EmployeeName,EmployeeAddress,Supervisor")] EMPLOYEE eMPLOYEE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eMPLOYEE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", eMPLOYEE.EmployeeID);
            ViewBag.EmployeeID = new SelectList(db.EMPLOYEEs, "EmployeeID", "EmployeeName", eMPLOYEE.EmployeeID);
            return View(eMPLOYEE);
        }

        // GET: EMPLOYEEs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EMPLOYEE eMPLOYEE = db.EMPLOYEEs.Find(id);
            if (eMPLOYEE == null)
            {
                return HttpNotFound();
            }
            return View(eMPLOYEE);
        }

        // POST: EMPLOYEEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EMPLOYEE eMPLOYEE = db.EMPLOYEEs.Find(id);
            db.EMPLOYEEs.Remove(eMPLOYEE);
            db.SaveChanges();
            return RedirectToAction("Index");
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
