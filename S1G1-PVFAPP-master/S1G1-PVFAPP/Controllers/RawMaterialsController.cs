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
    public class RawMaterialsController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: RawMaterials

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

            var rawMaterials = db.RAWMATERIALs.ToList();
            ViewBag.RMCount = db.RAWMATERIALs.ToList().Count();

            try
            {
               
                    if (!string.IsNullOrEmpty(searchString))
                    {
                    rawMaterials = db.RAWMATERIALs.Where(rawMat => rawMat.MaterialName.ToUpper().Contains(searchString.ToUpper())).ToList();
                        
                }

                switch (sortOrder)
                    {
                        case "name_desc":
                        rawMaterials = rawMaterials.OrderByDescending(a => a.MaterialName).ToList();
                            break;
                        case "id_desc":
                        rawMaterials = rawMaterials.OrderByDescending(a => a.MaterialID).ToList();
                            break;
                        default:
                        rawMaterials = rawMaterials.OrderBy(u => u.MaterialStandardCost).ToList();
                            break;
                    }
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    return View(rawMaterials.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new RAWMATERIAL());
        }
        //public ActionResult Index()
        //{
        //    return View(db.RawMaterials.ToList());
        //}

        // GET: RawMaterials/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RAWMATERIAL rawMaterial = db.RAWMATERIALs.Find(id);
            if (rawMaterial == null)
            {
                return HttpNotFound();
            }
            return View(rawMaterial);
        }

        // GET: RawMaterials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RawMaterials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaterialID,MaterialName,MaterialStandardCost,UnitOfMeasure")] RAWMATERIAL rawMaterial)
        {
            if (ModelState.IsValid)
            {
                var count = db.RAWMATERIALs.ToList().Count();
                if (count == 0)
                {
                    rawMaterial.MaterialID = "01";
                    rawMaterial.MaterialName = "Doe Material";
                    rawMaterial.MaterialStandardCost = "200";
                    rawMaterial.UnitOfMeasure = "1";

                }
                else
                {
                    rawMaterial.MaterialID = "0"+(count + 1).ToString();
                    rawMaterial.UnitOfMeasure = rawMaterial.UnitOfMeasure ?? "1";
                }

                db.RAWMATERIALs.Add(rawMaterial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rawMaterial);
        }

        // GET: RawMaterials/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RAWMATERIAL rawMaterial = db.RAWMATERIALs.Find(id);
            if (rawMaterial == null)
            {
                return HttpNotFound();
            }
            return View(rawMaterial);
        }

        // POST: RawMaterials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaterialID,MaterialName,MaterialStandardCost,UnitOfMeasure")] RAWMATERIAL rawMaterial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rawMaterial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rawMaterial);
        }

        // GET: RawMaterials/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RAWMATERIAL rawMaterial = db.RAWMATERIALs.Find(id);
            if (rawMaterial == null)
            {
                return HttpNotFound();
            }
            return View(rawMaterial);
        }

        // POST: RawMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RAWMATERIAL rawMaterial = db.RAWMATERIALs.Find(id);
            db.RAWMATERIALs.Remove(rawMaterial);
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
