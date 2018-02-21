using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace crud.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Database1Entities db = new Database1Entities();
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (Database1Entities obj = new Database1Entities())
            {
                var employees = obj.EmployeeTables.OrderBy(a => a.FirstName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
              //  return Json(employees);
                
            }      
  
        }
        [HttpGet]
        public ActionResult Save(int id)
        {
            using (Database1Entities obj = new Database1Entities())
            {
                var v = obj.EmployeeTables.Where(a => a.EmployeeId == id).FirstOrDefault();
                return View(v);
            }
        }

        

        [HttpPost]
        public ActionResult Save(EmployeeTable emp)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                try
                {
                    using (Database1Entities dc = new Database1Entities())
                    {
                        if (emp.EmployeeId > 0)
                        {
                            //Edit 
                            var v = dc.EmployeeTables.Where(a => a.EmployeeId == emp.EmployeeId).FirstOrDefault();
                            if (v != null)
                            {
                                v.FirstName = emp.FirstName;
                                v.LastName = emp.LastName;
                                v.EmailID = emp.EmailID;
                                v.City = emp.City;
                                v.Country = emp.Country;
                            }
                        }
                        else
                        {
                            //Save
                            dc.EmployeeTables.Add(emp);
                        }
                        dc.SaveChanges();
                        status = true;
                    }
                }
                catch(Exception e)
                {
                    throw;

                }
            }
            return new JsonResult { Data = new { status = status } };
        }



        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.EmployeeTables.Where(a => a.EmployeeId == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            using (Database1Entities dc = new Database1Entities())
            {
                var v = dc.EmployeeTables.Where(a => a.EmployeeId == id).FirstOrDefault();
                if (v != null)
                {
                    dc.EmployeeTables.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

    }
}