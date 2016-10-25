using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyCardSystem.Models;

namespace EasyCardSystem.Controllers
{
    
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult EmployeeIndex(string empid)
        {

            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var dept = Session["EmpDept"].ToString(); //各部門管理者只能看到自己部門資料
                List<EmpData> empdata = (from x in Data.EmpData
                                         where x.EmpDisable == "False"
                                         select x).ToList();

                if (Session["EmpRole"].ToString() == "admin")  //各部門管理者只能看到自己部門資料
                {
                    empdata = empdata.Where(x => x.EmpDept == dept).ToList();
                }

                if (!string.IsNullOrWhiteSpace(empid))//搜尋功能
                {
                    empdata = empdata.Where(x => x.EmpID == empid).ToList();
                }

                return View(empdata);
            }

        }


        public ActionResult EmpCreate()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult EmpCreate(EmpData empdata)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                empdata.EmpDisable = "False";
                Data.EmpData.Add(empdata);
                Data.SaveChanges();
            }
            return RedirectToAction("EmployeeIndex");
        }

        // GET: Employee/Edit/5
        public ActionResult EmpEdit(string empid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var empdata = (from x in Data.EmpData
                               where x.EmpID == empid
                               select x).FirstOrDefault();
                return View(empdata);
            }
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult EmpEdit(EmpData getempdata)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                if (ModelState.IsValid)
                {
                    EmpData empdata = Data.EmpData.Where(x => x.EmpID == getempdata.EmpID).FirstOrDefault();
                    empdata.EmpID = getempdata.EmpID;
                    empdata.EmpCardCode = getempdata.EmpCardCode;
                    empdata.EmpDept = getempdata.EmpDept;
                    empdata.EmpName = getempdata.EmpName;
                    empdata.Email = getempdata.Email;
                    empdata.EmpState = getempdata.EmpState;
                    empdata.Role = getempdata.Role;
                    Data.SaveChanges();
                }
                return RedirectToAction("EmployeeIndex");
            }
        }

        // GET: Employee/Delete/5
        public ActionResult EmpDelete(string empid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var empddata = (from x in Data.EmpData
                                where x.EmpID == empid
                                select x).FirstOrDefault();
                if (empddata == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    empddata.EmpDisable = "True";
                    Data.SaveChanges();
                }
                return RedirectToAction("EmployeeIndex");
            }
        }


    }
}