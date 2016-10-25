using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyCardSystem.Models;


namespace EasyCardSystem.Controllers
{
    
    public class AllRecordController : Controller
    {
        // GET: AllRecord
        public ActionResult AllRecordIndex(string empid, string cardname,string timelend)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var dept = Session["EmpDept"].ToString(); //各部門管理者只能看到自己部門資料

                List<RecordDataViewModel> RecData = new List<RecordDataViewModel>();
                var rdata = (from x in Data.EmpData
                             join y in Data.RecordData on x.EmpID equals y.EmpID
                             join z in Data.CardData on y.CardID equals z.CardID
                             where y.RecordDisable == "False"
                             orderby y.RecordID descending //降冪                             
                             select new RecordDataViewModel
                             {
                                 RecordID = y.RecordID,
                                 EmpID = x.EmpID,
                                 EmpName = x.EmpName,
                                 EmpDept = x.EmpDept,
                                 EmpState = x.EmpState,
                                 CardName = z.CardName,
                                 TimeLend = y.TimeLend,
                                 TimeReturn = y.TimeReturn,
                                 CardState = z.CardState,
                                 UseDay = y.UseDay,
                                 CardAmount = z.CardAmount,
                                 TotalSpent = y.TotalSpent
                             }).ToList();
                RecData = rdata;

                if (Session["EmpRole"].ToString() == "admin")  //各部門管理者只能看到自己部門資料
                {
                    RecData = RecData.Where(x => x.EmpDept == dept).ToList();
                }            
                
                               
                if (!string.IsNullOrWhiteSpace(empid)) //搜尋功能
                {
                    RecData = RecData.Where(x => x.EmpID == empid).ToList();
                }
                if (!string.IsNullOrWhiteSpace(cardname)) //搜尋功能
                {
                    RecData = RecData.Where(z => z.CardName == cardname).ToList();
                }
                if (!string.IsNullOrWhiteSpace(timelend)) //搜尋功能
                {
                    RecData = RecData.Where(y => y.TimeLend == DateTime.Parse(timelend)).ToList();
                }
                return View(RecData);
            }
        }
    }
}