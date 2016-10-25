using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyCardSystem.Models;

namespace EasyCardSystem.Controllers
{
    public class UserBorrowController : Controller
    {
        // GET: UserBorrow
        public ActionResult UserIndex()
        {
            return View();
        }

        // 使用者借用
        public ActionResult UserBorrow()
        {
            if (Session["EmpState"].ToString() == "已借用")//防止重複借卡
            {
                ModelState.AddModelError("UseDay", "尚有未歸還或已預約卡片，請先歸還");

            }
            return View();
        }

        // 使用者借用
        [HttpPost]
        public ActionResult UserBorrow(RecordDataViewModel getrecorddata)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                if (getrecorddata.EmpID != Session["EmpID"].ToString() || getrecorddata.EmpName != Session["EmpName"].ToString())
                {
                    ModelState.AddModelError("UseDay", "請使用本人ID、姓名借用");
                    return View(getrecorddata);

                }

                EmpData empdata = Data.EmpData.Where(x => x.EmpID == getrecorddata.EmpID).FirstOrDefault();
                if (empdata.EmpState == "已借用")//防止重複借卡
                {
                    ModelState.AddModelError("UseDay", "尚有未歸還或已預約卡片，請先歸還");
                    return View(getrecorddata);
                }
                else
                {
                    empdata.EmpState = "已預約";
                    RecordData recorddata = new RecordData()
                    {
                        RecordID = Data.RecordData.Count() + 1,
                        EmpID = getrecorddata.EmpID,
                        TimeLend = getrecorddata.TimeLend,
                        UseDay = getrecorddata.UseDay,
                        CardID = "0001", //預約時，預設卡號"0001"為預約卡
                        RecordDisable = "False",
                        RecordState = "已預約",
                    };
                    Data.RecordData.Add(recorddata);
                    Data.SaveChanges();
                    return RedirectToAction("UserRecord");
                }
            }

        }




        // 使用者借用紀錄    
        public ActionResult UserRecord()
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var userid = Session["EmpID"].ToString();
                List<RecordDataViewModel> userdata = new List<RecordDataViewModel>();
                var rdata = (from x in Data.EmpData
                             join y in Data.RecordData on x.EmpID equals y.EmpID
                             join z in Data.CardData on y.CardID equals z.CardID
                             where x.EmpID == userid && y.RecordDisable == "False"
                             orderby y.RecordID descending //降冪                             
                             select new RecordDataViewModel
                             {
                                 RecordID = y.RecordID,
                                 CardID = y.CardID,
                                 CardName = z.CardName,
                                 TimeLend = y.TimeLend,
                                 TimeReturn = y.TimeReturn,
                                 UseDay = y.UseDay,
                                 RecordState = y.RecordState
                             }).ToList();

                userdata = rdata;

                return View(userdata);
            }
        }


        // 使用者修改預約
        public ActionResult UserEdit(int recordid)
        {
            using (EasyCardEntities4 data = new EasyCardEntities4())
            {
                var recorddata = (from x in data.EmpData
                                  join y in data.RecordData on x.EmpID equals y.EmpID
                                  where y.RecordID == recordid
                                  select new RecordDataViewModel
                                  {
                                      RecordID = y.RecordID,
                                      EmpID = x.EmpID,
                                      EmpName = x.EmpName,
                                      TimeLend = y.TimeLend,
                                      UseDay = y.UseDay
                                  }).FirstOrDefault();


                return View(recorddata);
            }
        }

        // 使用者修改預約
        [HttpPost]
        public ActionResult UserEdit(RecordDataViewModel editdata)
        {
            using (EasyCardEntities4 data = new EasyCardEntities4())
            {
                if (ModelState.IsValid)
                {
                    RecordData recdata = data.RecordData.Where(x => x.RecordID == editdata.RecordID).FirstOrDefault();
                    recdata.TimeLend = editdata.TimeLend;
                    recdata.UseDay = editdata.UseDay;
                    data.SaveChanges();
                }
                return RedirectToAction("UserRecord");
            }
        }

        // 使用者取消預約
        public ActionResult UserDelete(int recordid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var recdata = (from x in Data.RecordData
                               where x.RecordID == recordid
                               select x).FirstOrDefault();
                if (recdata == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    recdata.RecordDisable = "True";
                    Data.SaveChanges();
                }
                return RedirectToAction("UserRecord");
            }
        }


        // 使用者續借
        public ActionResult UserRenew(int recordid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var recdata = (from x in Data.RecordData
                               where x.RecordID == recordid
                               select x).FirstOrDefault();
                recdata.UseDay = recdata.UseDay + 5;
                Data.SaveChanges();

                return RedirectToAction("UserRecord");
            }

        }


    }
}
