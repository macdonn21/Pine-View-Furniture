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
    public class SALESPERSONsController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: SALESPERSONs
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

            var salesperson = db.SALESPERSONs.ToList();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    salesperson = db.SALESPERSONs.Where(s => s.SalespersonID.ToUpper().Contains(searchString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        salesperson = salesperson.OrderByDescending(a => a.SalespersonID).ToList();
                        break;

                    default:
                        salesperson = salesperson.OrderBy(u => u.SalespersonName).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(salesperson.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new SALESPERSON());
        }

        // GET: SALESPERSONs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALESPERSON sALESPERSON = db.SALESPERSONs.Find(id);
            if (sALESPERSON == null)
            {
                return HttpNotFound();
            }
            return View(sALESPERSON);
        }

        // GET: SALESPERSONs/Create
        public ActionResult Create()
        {
            ViewBag.TerritoryID = new SelectList(db.TERRITORies, "TerritoryID", "TerritoryName");
            return View();
        }

        // POST: SALESPERSONs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalespersonID,SalespersonName,SalespersonTelephone,SalespersonFax,TerritoryID")] SALESPERSON sALESPERSON)
        {
            if (ModelState.IsValid)
            {
                var count = db.SALESPERSONs.ToList().Count();
                if (count == 0)
                {
                    sALESPERSON.SalespersonID = "01";
                    sALESPERSON.SalespersonName = "Ikechukwu";
                    sALESPERSON.SalespersonTelephone = "2265068940";
                    sALESPERSON.SalespersonFax = "555673";
                    sALESPERSON.TerritoryID = "03";

                }
                else
                {
                    sALESPERSON.SalespersonID = "0" + (count + 1).ToString();
                }
                db.SALESPERSONs.Add(sALESPERSON);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TerritoryID = new SelectList(db.TERRITORies, "TerritoryID", "TerritoryName", sALESPERSON.TerritoryID);
            return View(sALESPERSON);
        }

        // GET: SALESPERSONs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALESPERSON sALESPERSON = db.SALESPERSONs.Find(id);
            if (sALESPERSON == null)
            {
                return HttpNotFound();
            }
            ViewBag.TerritoryID = new SelectList(db.TERRITORies, "TerritoryID", "TerritoryName", sALESPERSON.TerritoryID);
            return View(sALESPERSON);
        }

        // POST: SALESPERSONs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalespersonID,SalespersonName,SalespersonTelephone,SalespersonFax,TerritoryID")] SALESPERSON sALESPERSON)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sALESPERSON).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TerritoryID = new SelectList(db.TERRITORies, "TerritoryID", "TerritoryName", sALESPERSON.TerritoryID);
            return View(sALESPERSON);
        }

        // GET: SALESPERSONs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALESPERSON sALESPERSON = db.SALESPERSONs.Find(id);
            if (sALESPERSON == null)
            {
                return HttpNotFound();
            }
            return View(sALESPERSON);
        }

        // POST: SALESPERSONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SALESPERSON sALESPERSON = db.SALESPERSONs.Find(id);
            db.SALESPERSONs.Remove(sALESPERSON);
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
