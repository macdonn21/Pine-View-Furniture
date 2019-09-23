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
    public class VENDORsController : Controller
    {
        private Entities3 db = new Entities3();

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

            var vendors = db.VENDORs.ToList();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    vendors = db.VENDORs.Where(s => s.VendorName.ToUpper().Contains(searchString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        vendors = vendors.OrderByDescending(a => a.VendorName).ToList();
                        break;

                    default:
                        vendors = vendors.OrderBy(u => u.VendorName).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(vendors.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new VENDOR());
        }

        // GET: VENDORs
        //public ActionResult Index()
        //{
        //    var vENDORs = db.VENDORs.Include(v => v.SUPPLy);
        //    return View(vENDORs.ToList());
        //}

        // GET: VENDORs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDOR vENDOR = db.VENDORs.Find(id);
            if (vENDOR == null)
            {
                return HttpNotFound();
            }
            return View(vENDOR);
        }

        // GET: VENDORs/Create
        public ActionResult Create()
        {
            ViewBag.VendorID = new SelectList(db.VENDORs, "VendorID", "MaterialID");
            return View();
        }

        // POST: VENDORs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendorID,VendorName,VendorAddress")] VENDOR vENDOR)
        {
            if (ModelState.IsValid)
            {
                var count = db.VENDORs.ToList().Count();
                if (count == 0)
                {
                    vENDOR.VendorID = "01";
                    vENDOR.VendorAddress = "588 Riverside";
                    vENDOR.VendorName = "Jane Dine";
                   

                }
                else
                {
                    vENDOR.VendorID = "0" + (count + 1).ToString();
                }

                db.VENDORs.Add(vENDOR);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendorID = new SelectList(db.VENDORs, "VendorID", "MaterialID", vENDOR.VendorID);
            return View(vENDOR);
        }

        // GET: VENDORs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDOR vENDOR = db.VENDORs.Find(id);
            if (vENDOR == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorID = new SelectList(db.VENDORs, "VendorID", "MaterialID", vENDOR.VendorID);
            return View(vENDOR);
        }

        // POST: VENDORs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VendorID,VendorName,VendorAddress")] VENDOR vENDOR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vENDOR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorID = new SelectList(db.VENDORs, "VendorID", "MaterialID", vENDOR.VendorID);
            return View(vENDOR);
        }

        // GET: VENDORs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDOR vENDOR = db.VENDORs.Find(id);
            if (vENDOR == null)
            {
                return HttpNotFound();
            }
            return View(vENDOR);
        }

        // POST: VENDORs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            VENDOR vENDOR = db.VENDORs.Find(id);
            db.VENDORs.Remove(vENDOR);
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
