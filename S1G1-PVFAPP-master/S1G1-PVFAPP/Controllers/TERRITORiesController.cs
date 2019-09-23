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
    public class TERRITORiesController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: TERRITORies
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

            var territories = db.TERRITORies.ToList();
            ViewBag.E_Count = db.TERRITORies.ToList().Count();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    territories = db.TERRITORies.Where(employee => employee.TerritoryName.ToUpper().Contains(searchString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        territories = territories.OrderByDescending(a => a.TerritoryName).ToList();
                        break;
                    case "id_desc":
                        territories = territories.OrderByDescending(a => a.TerritoryID).ToList();
                        break;
                    default:
                        territories = territories.OrderBy(u => u.TerritoryID).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(territories.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new EMPLOYEE());
        }

        // GET: TERRITORies/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TERRITORY tERRITORY = db.TERRITORies.Find(id);
            if (tERRITORY == null)
            {
                return HttpNotFound();
            }
            return View(tERRITORY);
        }

        // GET: TERRITORies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TERRITORies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TerritoryID,TerritoryName")] TERRITORY tERRITORY)
        {
            if (ModelState.IsValid)
            {
                if(tERRITORY.TerritoryName == null)
                {
                    ViewBag.Error = "Null Input!";
                    Session["ErrorMessage"] = ViewBag.Error;
                    return View(tERRITORY);
                }
                var count = db.TERRITORies.ToList().Count();
                if (count == 0)
                {
                    tERRITORY.TerritoryID = "1";
                    tERRITORY.TerritoryName = "Canada";
                    db.TERRITORies.Add(tERRITORY);
                }
                else
                {
                    tERRITORY.TerritoryID = "0"+(count + 1).ToString();
                    db.TERRITORies.Add(tERRITORY);
                }

                db.TERRITORies.Add(tERRITORY);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Invalid Input!";
            }

            return View(tERRITORY);
        }

        // GET: TERRITORies/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TERRITORY tERRITORY = db.TERRITORies.Find(id);
            if (tERRITORY == null)
            {
                return HttpNotFound();
            }
            return View(tERRITORY);
        }

        // POST: TERRITORies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TerritoryID,TerritoryName")] TERRITORY tERRITORY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tERRITORY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tERRITORY);
        }

        // GET: TERRITORies/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TERRITORY tERRITORY = db.TERRITORies.Find(id);
            if (tERRITORY == null)
            {
                return HttpNotFound();
            }
            return View(tERRITORY);
        }

        // POST: TERRITORies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TERRITORY tERRITORY = db.TERRITORies.Find(id);
            db.TERRITORies.Remove(tERRITORY);
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
