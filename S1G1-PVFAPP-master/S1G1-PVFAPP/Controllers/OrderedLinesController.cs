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
    public class OrderedLinesController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: OrderedLines
        public ActionResult Index()
        {
            var orderedLines = db.OrderedLines.Include(o => o.ORDER).Include(o => o.PRODUCT);
            return View(orderedLines.ToList());
        }

        // GET: OrderedLines/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderedLine orderedLine = db.OrderedLines.Find(id);
            if (orderedLine == null)
            {
                return HttpNotFound();
            }
            return View(orderedLine);
        }

        // GET: OrderedLines/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.ORDERs, "OrderID", "CustomerID");
            ViewBag.ProductID = new SelectList(db.PRODUCTs, "ProductID", "ProductDescription");
            return View();
        }

        // POST: OrderedLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,ProductID,OrderedQuantity")] OrderedLine orderedLine)
        {
            if (ModelState.IsValid)
            {

                orderedLine.OrderdedLineID = GenerateID(orderedLine.OrderID, orderedLine.ProductID,orderedLine.OrderedQuantity.ToString());
                db.OrderedLines.Add(orderedLine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.ORDERs, "OrderID", "CustomerID", orderedLine.OrderID);
            ViewBag.ProductID = new SelectList(db.PRODUCTs, "ProductID", "ProductDescription", orderedLine.ProductID);
            return View(orderedLine);
        }

        // GET: OrderedLines/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderedLine orderedLine = db.OrderedLines.Find(id);
            if (orderedLine == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.ORDERs, "OrderID", "CustomerID", orderedLine.OrderID);
            ViewBag.ProductID = new SelectList(db.PRODUCTs, "ProductID", "ProductDescription", orderedLine.ProductID);
            return View(orderedLine);
        }

        // POST: OrderedLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,ProductID,OrderedQuantity,OrderdedLineID")] OrderedLine orderedLine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderedLine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.ORDERs, "OrderID", "CustomerID", orderedLine.OrderID);
            ViewBag.ProductID = new SelectList(db.PRODUCTs, "ProductID", "ProductDescription", orderedLine.ProductID);
            return View(orderedLine);
        }

        // GET: OrderedLines/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderedLine orderedLine = db.OrderedLines.Find(id);
            if (orderedLine == null)
            {
                return HttpNotFound();
            }
            return View(orderedLine);
        }

        // POST: OrderedLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OrderedLine orderedLine = db.OrderedLines.Find(id);
            db.OrderedLines.Remove(orderedLine);
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

        public string GenerateID(string key1, string key2, string key3)
        {
            if (string.IsNullOrEmpty(key1))
            {
                key1 = "RE";
            }
            if (string.IsNullOrEmpty(key2))
            {
                key2 = "AP";
            }
            if (string.IsNullOrEmpty(key3))
            {
                key3 = "ER";
            }
            Guid guid = Guid.NewGuid();
            var generatedKey =  key1.Substring(0, 2).ToUpper() + key2.Substring(0, 2).ToUpper() ;
            string finalKey = generatedKey + guid.ToString().Substring(0, 6);

            return finalKey;
        }
    }
}
