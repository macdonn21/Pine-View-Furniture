using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using S1G1_PVFAPP.Models;

namespace S1G1_PVFAPP.Controllers
{
    public class USController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: US
        public ActionResult Index()
        {
            var uSES = db.USES.Include(u => u.PRODUCT).Include(u => u.RAWMATERIAL);
            return View(uSES.ToList());
        }

        // GET: US/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            US uS = db.USES.Find(id);
            if (uS == null)
            {
                return HttpNotFound();
            }
            return View(uS);
        }

        // GET: US/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.PRODUCTs, "ProductID", "ProductDescription");
            ViewBag.MaterialID = new SelectList(db.RAWMATERIALs, "MaterialID", "MaterialName");
            return View();
        }

        // POST: US/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,MaterialID,GoesIntoQuantity,UsesID")] US uS)
        {
            if (ModelState.IsValid)
            {
                uS.UsesID = GenerateID(uS.ProductID, uS.MaterialID);
               
                db.USES.Add(uS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.PRODUCTs, "ProductID", "ProductDescription", uS.ProductID);
            ViewBag.MaterialID = new SelectList(db.RAWMATERIALs, "MaterialID", "MaterialName", uS.MaterialID);
            return View(uS);
        }

        // GET: US/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            US uS = db.USES.Find(id);
            if (uS == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.PRODUCTs, "ProductID", "ProductDescription", uS.ProductID);
            ViewBag.MaterialID = new SelectList(db.RAWMATERIALs, "MaterialID", "MaterialName", uS.MaterialID);
            return View(uS);
        }

        // POST: US/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,MaterialID,GoesIntoQuantity,UsesID")] US uS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.PRODUCTs, "ProductID", "ProductDescription", uS.ProductID);
            ViewBag.MaterialID = new SelectList(db.RAWMATERIALs, "MaterialID", "MaterialName", uS.MaterialID);
            return View(uS);
        }

        // GET: US/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            US uS = db.USES.Find(id);
            if (uS == null)
            {
                return HttpNotFound();
            }
            return View(uS);
        }

        // POST: US/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            US uS = db.USES.Find(id);
            db.USES.Remove(uS);
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
