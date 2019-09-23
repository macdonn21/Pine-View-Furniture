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
    public class WORKCENTERsController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: WORKCENTERs
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

            var workcenter = db.WORKCENTERs.ToList();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    workcenter = db.WORKCENTERs.Where(s => s.WorkCenterID.ToUpper().Contains(searchString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        workcenter = workcenter.OrderByDescending(a => a.WorkCenterID).ToList();
                        break;

                    default:
                        workcenter = workcenter.OrderBy(u => u.WorkCenterLocation).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(workcenter.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new WORKCENTER());
        }

        // GET: WORKCENTERs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WORKCENTER wORKCENTER = db.WORKCENTERs.Find(id);
            if (wORKCENTER == null)
            {
                return HttpNotFound();
            }
            return View(wORKCENTER);
        }

        // GET: WORKCENTERs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WORKCENTERs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkCenterID,WorkCenterLocation")] WORKCENTER wORKCENTER)
        {
            if (ModelState.IsValid)
            {
                var count = db.WORKCENTERs.ToList().Count();
                if (count == 0)
                {
                    wORKCENTER.WorkCenterID = "01";
                    wORKCENTER.WorkCenterLocation = "101 Dalmation St";
          
                }
                else
                {
                    wORKCENTER.WorkCenterID = "0" + (count + 1).ToString();
                }
                db.WORKCENTERs.Add(wORKCENTER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wORKCENTER);
        }

        // GET: WORKCENTERs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WORKCENTER WORKCENTER = db.WORKCENTERs.Find(id);
            if (WORKCENTER == null)
            {
                return HttpNotFound();
            }
            return View(new WORKCENTER());
        }

        // POST: WORKCENTERs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkCenterID,WorkCenterLocation")] WORKCENTER wORKCENTER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wORKCENTER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wORKCENTER);
        }

        // GET: WORKCENTERs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WORKCENTER wORKCENTER = db.WORKCENTERs.Find(id);
            if (wORKCENTER == null)
            {
                return HttpNotFound();
            }
            return View(wORKCENTER);
        }

        // POST: WORKCENTERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            WORKCENTER wORKCENTER = db.WORKCENTERs.Find(id);
            db.WORKCENTERs.Remove(wORKCENTER);
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
