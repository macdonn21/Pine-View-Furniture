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
    public class SUPPLiesController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: SUPPLies
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

            var supplies = db.SUPPLIES.ToList();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    supplies = db.SUPPLIES.Where(s => s.MaterialID.ToUpper().Contains(searchString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        supplies = supplies.OrderByDescending(a => a.VendorID).ToList();
                        break;

                    default:
                        supplies = supplies.OrderBy(u => u.MaterialID).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(supplies.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new SUPPLy());
        }


        // GET: SUPPLies/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUPPLy sUPPLy = db.SUPPLIES.Find(id);
            if (sUPPLy == null)
            {
                return HttpNotFound();
            }
            return View(sUPPLy);
        }

        // GET: SUPPLies/Create
        public ActionResult Create()
        {
            ViewBag.MaterialID = new SelectList(db.RAWMATERIALs, "MaterialID", "MaterialName");
            ViewBag.VendorID = new SelectList(db.VENDORs, "VendorID", "VendorName");
            return View();
        }

        // POST: SUPPLies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VendorID,MaterialID,SupplyUnitPrice,SupplyID")] SUPPLy sUPPLy)
        {
            if (ModelState.IsValid)
            {
                sUPPLy.SupplyID = GenerateID(sUPPLy.VendorID, sUPPLy.MaterialID);
                db.SUPPLIES.Add(sUPPLy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaterialID = new SelectList(db.RAWMATERIALs, "MaterialID", "MaterialName", sUPPLy.MaterialID);
            ViewBag.VendorID = new SelectList(db.VENDORs, "VendorID", "VendorName", sUPPLy.VendorID);
            return View(sUPPLy);
        }

        // GET: SUPPLies/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUPPLy sUPPLy = db.SUPPLIES.Find(id);
            if (sUPPLy == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaterialID = new SelectList(db.RAWMATERIALs, "MaterialID", "MaterialName", sUPPLy.MaterialID);
            ViewBag.VendorID = new SelectList(db.VENDORs, "VendorID", "VendorName", sUPPLy.VendorID);
            return View(sUPPLy);
        }

        // POST: SUPPLies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VendorID,MaterialID,SupplyUnitPrice, SupplyID")] SUPPLy sUPPLy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sUPPLy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaterialID = new SelectList(db.RAWMATERIALs, "MaterialID", "MaterialName", sUPPLy.MaterialID);
            ViewBag.VendorID = new SelectList(db.VENDORs, "VendorID", "VendorName", sUPPLy.VendorID);
            return View(sUPPLy);
        }

        // GET: SUPPLies/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUPPLy sUPPLy = db.SUPPLIES.Find(id);
            if (sUPPLy == null)
            {
                return HttpNotFound();
            }
            return View(sUPPLy);
        }

        // POST: SUPPLies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SUPPLy sUPPLy = db.SUPPLIES.Find(id);
            db.SUPPLIES.Remove(sUPPLy);
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

        public string GenerateID(string key1, string key2)
        {
            if (string.IsNullOrEmpty(key1))
            {
                key1 = "RE";
            }
            if (string.IsNullOrEmpty(key2))
            {
                key2 = "AP";
            }
            
            Guid guid = Guid.NewGuid();
            var generatedKey = key1.Substring(0, 1).ToUpper() + key2.Substring(0, 2).ToUpper();
            string finalKey = generatedKey + guid.ToString().Substring(0, 4);

            return finalKey;
        }
    }
}
