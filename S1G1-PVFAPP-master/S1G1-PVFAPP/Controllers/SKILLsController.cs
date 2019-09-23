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
    public class SKILLsController : Controller
    {
        private Entities3 db = new Entities3();

        // GET: SKILLs
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

            var skills = db.SKILLs.ToList();

            try
            {

                if (!string.IsNullOrEmpty(searchString))
                {
                    skills = db.SKILLs.Where(s => s.Skill1.ToUpper().Contains(searchString.ToUpper())).ToList();

                }

                switch (sortOrder)
                {
                    case "name_desc":
                        skills = skills.OrderByDescending(a => a.Skill1).ToList();
                        break;
                   
                    default:
                        skills = skills.OrderBy(u => u.Skill1).ToList();
                        break;
                }
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                return View(skills.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception exception)
            {
                ViewBag.Exception = exception.Message;
            }

            return View(new SKILL());
        }


        // GET: SKILLs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKILL sKILL = db.SKILLs.Find(id);
            if (sKILL == null)
            {
                return HttpNotFound();
            }
            return View(sKILL);
        }

        // GET: SKILLs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SKILLs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Skill1")] SKILL sKILL)
        {
            if (ModelState.IsValid)
            {
                db.SKILLs.Add(sKILL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sKILL);
        }

        // GET: SKILLs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKILL sKILL = db.SKILLs.Find(id);
            if (sKILL == null)
            {
                return HttpNotFound();
            }
            return View(sKILL);
        }

        // POST: SKILLs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Skill1")] SKILL sKILL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sKILL).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sKILL);
        }

        // GET: SKILLs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SKILL sKILL = db.SKILLs.Find(id);
            if (sKILL == null)
            {
                return HttpNotFound();
            }
            return View(sKILL);
        }

        // POST: SKILLs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SKILL sKILL = db.SKILLs.Find(id);
            db.SKILLs.Remove(sKILL);
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
