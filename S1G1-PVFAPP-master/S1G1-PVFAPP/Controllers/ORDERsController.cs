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
    public class ORDERsController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: ORDERs
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

            var order = db.ORDERs.ToList();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    order = db.ORDERs.Where(s => s.CUSTOMER.CustomerName.ToUpper().Contains(searchString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        order = order.OrderByDescending(a => a.CUSTOMER.CustomerName).ToList();
                        break;
                    case "id_desc":
                        order = order.OrderByDescending(a => a.OrderID).ToList();
                        break;

                    default:
                        order = order.OrderBy(u => u.CUSTOMER.CustomerName).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(order.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new ORDER());
        }

        // GET: ORDERs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            return View(oRDER);
        }

        // GET: ORDERs/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.CUSTOMERs, "CustomerID", "CustomerName");
            ViewBag.OrderID = new SelectList(db.OrderedLines, "OrderID", "ProductID");
            return View();
        }

        // POST: ORDERs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,OrderDate,CustomerID")] ORDER oRDER)
        {
            if (ModelState.IsValid)
            {
                oRDER.OrderDate = DateTime.Now;
                var count = db.ORDERs.ToList().Count();
                if (count == 0)
                {
                    oRDER.OrderID = "01";
                    oRDER.CUSTOMER.CustomerName = "Ikechukwu";

                }
                else
                {
                    oRDER.OrderID = "0" + (count + 1).ToString();
                }
                db.ORDERs.Add(oRDER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.CUSTOMERs, "CustomerID", "CustomerName", oRDER.CustomerID);
            ViewBag.OrderID = new SelectList(db.OrderedLines, "OrderID", "ProductID", oRDER.OrderID);
            return View(oRDER);
        }

        // GET: ORDERs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.CUSTOMERs, "CustomerID", "CustomerName", oRDER.CustomerID);
            ViewBag.OrderID = new SelectList(db.OrderedLines, "OrderID", "ProductID", oRDER.OrderID);
            return View(oRDER);
        }

        // POST: ORDERs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,CustomerID")] ORDER oRDER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oRDER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.CUSTOMERs, "CustomerID", "CustomerName", oRDER.CustomerID);
            ViewBag.OrderID = new SelectList(db.OrderedLines, "OrderID", "ProductID", oRDER.OrderID);
            return View(oRDER);
        }

        // GET: ORDERs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER oRDER = db.ORDERs.Find(id);
            if (oRDER == null)
            {
                return HttpNotFound();
            }
            return View(oRDER);
        }

        // POST: ORDERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ORDER oRDER = db.ORDERs.Find(id);
            db.ORDERs.Remove(oRDER);
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
