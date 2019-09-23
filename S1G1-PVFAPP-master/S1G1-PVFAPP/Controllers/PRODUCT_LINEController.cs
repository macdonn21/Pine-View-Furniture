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
    public class PRODUCT_LINEController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: PRODUCT_LINE
        public ActionResult Index()
        {
            return View(db.PRODUCT_LINE.ToList());
        }

        // GET: PRODUCT_LINE/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT_LINE pRODUCT_LINE = db.PRODUCT_LINE.Find(id);
            if (pRODUCT_LINE == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT_LINE);
        }

        // GET: PRODUCT_LINE/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PRODUCT_LINE/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductLineID,ProductLineName")] PRODUCT_LINE pRODUCT_LINE)
        {
            if (ModelState.IsValid)
            {
                db.PRODUCT_LINE.Add(pRODUCT_LINE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pRODUCT_LINE);
        }

        // GET: PRODUCT_LINE/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT_LINE pRODUCT_LINE = db.PRODUCT_LINE.Find(id);
            if (pRODUCT_LINE == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT_LINE);
        }

        // POST: PRODUCT_LINE/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductLineID,ProductLineName")] PRODUCT_LINE pRODUCT_LINE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pRODUCT_LINE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pRODUCT_LINE);
        }

        // GET: PRODUCT_LINE/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PRODUCT_LINE pRODUCT_LINE = db.PRODUCT_LINE.Find(id);
            if (pRODUCT_LINE == null)
            {
                return HttpNotFound();
            }
            return View(pRODUCT_LINE);
        }

        // POST: PRODUCT_LINE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PRODUCT_LINE pRODUCT_LINE = db.PRODUCT_LINE.Find(id);
            db.PRODUCT_LINE.Remove(pRODUCT_LINE);
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
