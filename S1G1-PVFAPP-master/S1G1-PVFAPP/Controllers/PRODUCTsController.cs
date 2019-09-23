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
    public class PRODUCTsController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: PRODUCTs
        public ActionResult Index()
        {
            var pRODUCTs = db.PRODUCTs.Include(p => p.PRODUCT_LINE);
            return View(pRODUCTs.ToList());
        }

        // GET: PRODUCTs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCTs.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT);
        }

        // GET: PRODUCTs/Create
        public ActionResult Create()
        {
            ViewBag.ProductLineID = new SelectList(db.PRODUCT_LINE, "ProductLineID", "ProductLineName");
            return View();
        }

        // POST: PRODUCTs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductDescription,ProductFinish,ProductStandardPrice,ProductLineID")] PRODUCT pRODUCT)
        {
            if (ModelState.IsValid)
            {
                db.PRODUCTs.Add(pRODUCT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductLineID = new SelectList(db.PRODUCT_LINE, "ProductLineID", "ProductLineName", pRODUCT.ProductLineID);
            return View(pRODUCT);
        }

        // GET: PRODUCTs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCTs.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductLineID = new SelectList(db.PRODUCT_LINE, "ProductLineID", "ProductLineName", pRODUCT.ProductLineID);
            return View(pRODUCT);
        }

        // POST: PRODUCTs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductDescription,ProductFinish,ProductStandardPrice,ProductLineID")] PRODUCT pRODUCT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pRODUCT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductLineID = new SelectList(db.PRODUCT_LINE, "ProductLineID", "ProductLineName", pRODUCT.ProductLineID);
            return View(pRODUCT);
        }

        // GET: PRODUCTs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT pRODUCT = db.PRODUCTs.Find(id);
            if (pRODUCT == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT);
        }

        // POST: PRODUCTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PRODUCT pRODUCT = db.PRODUCTs.Find(id);
            db.PRODUCTs.Remove(pRODUCT);
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
