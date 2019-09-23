using S1G1_PVFAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S1G1_PVFAPP.Controllers
{
    public class HomeController : Controller
    {
        private Entities3 db = new Entities3();
        public ActionResult Index()
        {
            Session["RM_Count"] = db.RAWMATERIALs.ToList().Count;
            Session["E_Count"] =  db.EMPLOYEEs.ToList().Count;
            Session["Skill_Count"] =  db.SKILLs.ToList().Count;
            Session["WC_Count"] =  db.WORKCENTERs.ToList().Count;
            Session["Product_Count"] =  db.PRODUCTs.ToList().Count;
            Session["ProductLines_Count"] =  db.PRODUCT_LINE.ToList().Count;
            Session["Territory_Count"] =  db.TERRITORies.ToList().Count;
            Session["Vendor_Count"] =  db.VENDORs.ToList().Count;
            Session["SalesPerson_Count"] =  db.SALESPERSONs.ToList().Count;
            Session["Customer_Count"] =  db.CUSTOMERs.ToList().Count;
            Session["Order_Count"] =  db.ORDERs.ToList().Count;
            Session["OrderedLines_Count"] =  db.OrderedLines.ToList().Count;

                    return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Descriptions";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}