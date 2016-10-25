using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyCardSystem.Models;
using System.Web.Security;

namespace EasyCardSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(EmpData emplogin)
        {
            using (EasyCardEntities4 data = new EasyCardEntities4())
            {
                var obj = data.EmpData.Where(x => x.EmpID == emplogin.EmpID && x.EmpPwd == emplogin.EmpPwd).FirstOrDefault();
                if (obj != null)
                {
                    Session["EmpID"] = obj.EmpID;
                    Session["EmpName"] = obj.EmpName;
                    Session["EmpPwd"] = obj.EmpPwd;
                    Session["EmpDept"] = obj.EmpDept;
                    Session["EmpRole"] = obj.Role;
                    Session["EmpState"] = obj.EmpState;

                    if (obj.Role == "admin"|| obj.Role == "Superadmin")
                    {
                        return RedirectToAction("AdmBorrowIndex", "AdminBorrow");
                    }
                    else
                    {
                        return RedirectToAction("UserBorrow", "UserBorrow");
                    }

                }

                //密碼錯誤訊息
                ModelState.AddModelError("EmpPwd", "登入錯誤，請輸入正確帳號、密碼。");
                return View(emplogin);
            }

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}