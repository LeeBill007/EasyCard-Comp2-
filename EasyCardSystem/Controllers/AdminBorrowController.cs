using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyCardSystem.Models;

namespace EasyCardSystem.Controllers
{
    
    public class AdminBorrowController : Controller
    {
        // 顯示目前已預約資料
        public ActionResult AdmBorrowIndex(string empid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var dept = Session["EmpDept"].ToString(); //各部門管理者只能看到自己部門資料
                List<RecordDataViewModel> RecData = new List<RecordDataViewModel>();
                var rdata = (from x in Data.EmpData
                             join y in Data.RecordData on x.EmpID equals y.EmpID
                             where x.EmpState == "已預約"  && y.CardID=="0001" && y.RecordDisable == "False" //顯示目前未刪除的已預約  
                             select new RecordDataViewModel
                             {
                                 RecordID = y.RecordID,
                                 EmpID = x.EmpID,
                                 EmpName = x.EmpName,
                                 EmpDept = x.EmpDept,
                                 TimeLend = y.TimeLend,
                                 EmpState = x.EmpState,
                                 UseDay = y.UseDay
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
                return View(RecData);
            }
        }

        // GET: AdminBorrow/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // 現場借用
        public ActionResult AdmBorrow()
        {
            return View();
        }

        // 現場借用
        [HttpPost]
        public ActionResult AdmBorrow(RecordDataViewModel getrecorddata)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                CardData carddata = Data.CardData.Where(x => x.CardName == getrecorddata.CardName).FirstOrDefault();
                //if (carddata.CardState == "已借出")//防止選到已借出的卡片
                //{
                //}
                //else
                {
                    carddata.CardState = "已借出";
                    getrecorddata.CardID = carddata.CardID;
                }

                EmpData empdata = Data.EmpData.Where(x => x.EmpID == getrecorddata.EmpID).FirstOrDefault();
                //if (empdata.EmpState == "已借用")//防止重複借卡
                //{

                //}
                //else
                {
                    empdata.EmpState = "已借用";
                }

                RecordData recorddata = new RecordData()
                {
                    RecordID = Data.RecordData.Count() + 1,
                    EmpID = getrecorddata.EmpID,
                    CardID = getrecorddata.CardID,
                    TimeLend = getrecorddata.TimeLend,
                    UseDay = getrecorddata.UseDay,
                    RecordDisable = "False",
                    RecordState ="未歸還",
                };
                Data.RecordData.Add(recorddata);
                Data.SaveChanges();
            }
            return RedirectToAction("AdmReturnIndex");
        }

        // 預約確認 或 修改
        public ActionResult AdmEdit(int recorid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var recorddata = (from x in Data.EmpData
                                  join y in Data.RecordData on x.EmpID equals y.EmpID
                                  where y.RecordID == recorid
                                  select new RecordDataViewModel
                                  {
                                      RecordID = y.RecordID,
                                      EmpID = x.EmpID,
                                      EmpName = x.EmpName,
                                      EmpDept = x.EmpDept,
                                      TimeLend = y.TimeLend,
                                      UseDay = y.UseDay
                                  }).FirstOrDefault();
                return View(recorddata);
            };
        }

        // 預約確認 或 修改
        [HttpPost]
        public ActionResult AdmEdit(RecordDataViewModel getrecorddata)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                CardData carddata = Data.CardData.Where(x => x.CardName == getrecorddata.CardName).FirstOrDefault();
                //if (carddata.CardState =="已借出")//防止選到已借出的卡片
                //{

                //}
                //else 
                {
                    carddata.CardState = "已借出";
                }

                EmpData empdata = Data.EmpData.Where(x => x.EmpID == getrecorddata.EmpID).FirstOrDefault();
                empdata.EmpState = "已借用";

                //RecordData 儲存
                RecordData recorddata = Data.RecordData.Where(x => x.RecordID == getrecorddata.RecordID).FirstOrDefault();
                recorddata.EmpID = getrecorddata.EmpID;
                recorddata.CardID = carddata.CardID;
                recorddata.TimeLend = getrecorddata.TimeLend;
                recorddata.UseDay = getrecorddata.UseDay;
                recorddata.RecordState = "未歸還";
                Data.SaveChanges();
            }
            return RedirectToAction("AdmReturnIndex");
        }

        // 歸還畫面
        public ActionResult AdmReturnIndex(string cardname)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var dept = Session["EmpDept"].ToString(); //各部門管理者只能看到自己部門資料
                List<RecordDataViewModel> RecData = new List<RecordDataViewModel>();
                var rdata = (from x in Data.EmpData
                             join y in Data.RecordData on x.EmpID equals y.EmpID
                             join z in Data.CardData on y.CardID equals z.CardID
                             where y.TimeReturn == null && y.CardID !="0001"     //顯示一筆
                             select new RecordDataViewModel
                             {
                                 RecordID = y.RecordID,
                                 EmpID = x.EmpID,
                                 EmpName = x.EmpName,
                                 EmpDept = x.EmpDept,
                                 CardName = z.CardName,
                                 TimeLend = y.TimeLend,
                                 EmpState = x.EmpState,
                                 UseDay = y.UseDay
                             }).ToList();
                RecData = rdata;

                if (Session["EmpRole"].ToString() == "admin")  //各部門管理者只能看到自己部門資料
                {
                    RecData = RecData.Where(x => x.EmpDept == dept).ToList();
                }

                if (!string.IsNullOrWhiteSpace(cardname)) //搜尋功能
                {
                    RecData = RecData.Where(x => x.CardName == cardname).ToList();
                }
                return View(RecData);
            }
        }

        // 歸還確認        
        public ActionResult AdmReturnConfirm(int recorid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var rdata = (from x in Data.EmpData
                             join y in Data.RecordData on x.EmpID equals y.EmpID
                             join z in Data.CardData on y.CardID equals z.CardID
                             where y.RecordID == recorid
                             select new RecordDataViewModel
                             {
                                 RecordID = y.RecordID,
                                 EmpID = x.EmpID,
                                 EmpName = x.EmpName,
                                 EmpDept = x.EmpDept,
                                 CardName = z.CardName,
                                 TimeLend = y.TimeLend,
                                 UseDay = y.UseDay,
                                 
                             }).FirstOrDefault();

                return View(rdata);
            }

        }

        [HttpPost]
        // 歸還確認  
        public ActionResult AdmReturnConfirm(RecordDataViewModel getrecorddata)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                CardData carddata = Data.CardData.Where(x => x.CardName == getrecorddata.CardName).FirstOrDefault();
                carddata.CardState = "可使用";
                carddata.CardAmount = getrecorddata.CardAmount;
                EmpData empdata = Data.EmpData.Where(x => x.EmpID == getrecorddata.EmpID).FirstOrDefault();
                empdata.EmpState = "可借用";

                //RecordData 儲存
                RecordData recorddata = Data.RecordData.Where(x => x.RecordID == getrecorddata.RecordID).FirstOrDefault();
                recorddata.EmpID = getrecorddata.EmpID;
                recorddata.CardID = carddata.CardID;
                recorddata.TimeLend = getrecorddata.TimeLend;
                recorddata.UseDay = getrecorddata.UseDay;
                recorddata.TimeReturn = DateTime.Now;
                recorddata.RecordState = "已歸還";
                Data.SaveChanges();

                return RedirectToAction("AdmReturnIndex");
            }
        }
    }
}
