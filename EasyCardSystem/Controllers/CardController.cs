using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyCardSystem.Models;

namespace EasyCardSystem.Controllers
{
    public class CardController : Controller
    {
        // GET: Card
        /// <summary>
        /// 顯示所有資料
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public ActionResult CardIndex(string cardid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var dept = Session["EmpDept"].ToString(); //各部門管理者只能看到自己部門資料
                List<CardData> carddata = (from x in Data.CardData
                                           where x.CardDisable == "False"// 顯示未刪除的所有資料

                                           select x).ToList();

                if (Session["EmpRole"].ToString() == "admin")  //各部門管理者只能看到自己部門資料
                {
                    carddata = carddata.Where(x => x.CardDept == dept).ToList();
                }

                if (!string.IsNullOrWhiteSpace(cardid))//搜尋功能
                {
                    carddata = carddata.Where(x => x.CardID == cardid).ToList();
                }

                //if (carddata.Count() == 0)
                //{
                //    Response.Write("error");
                //}
                return View(carddata);
            }
        }



        //GET: Card/Create
        public ActionResult CardCreate()
        {
            return View();
        }

        // POST: Card/Create
        [HttpPost]
        public ActionResult CardCreate(CardData createdata)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                createdata.CardDisable = "False";
                Data.CardData.Add(createdata);
                Data.SaveChanges();
            }
            return RedirectToAction("CardIndex");

        }

        // GET: Card/Edit/5
        public ActionResult CardEdit(string cardid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var carddata = (from x in Data.CardData
                                where x.CardID == cardid
                                select x).FirstOrDefault();
                return View(carddata);
            }

        }

        // POST: Card/Edit/5
        [HttpPost]
        public ActionResult CardEdit(CardData getdata)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                if (ModelState.IsValid)
                {
                    CardData carddata = Data.CardData.Where(x => x.CardID == getdata.CardID).FirstOrDefault();
                    carddata.CardID = getdata.CardID;
                    carddata.CardDept = getdata.CardDept;
                    carddata.CardName = getdata.CardName;
                    carddata.CardState = getdata.CardState;
                    carddata.CardAmount = getdata.CardAmount;
                    Data.SaveChanges();
                }
                return RedirectToAction("CardIndex");
            }

        }

        // GET: Card/Delete/5
        //[AcceptVerbs(HttpVerbs.Delete)] //有別於get post
        public ActionResult CardDelete(string cardid)
        {
            using (EasyCardEntities4 Data = new EasyCardEntities4())
            {
                var carddata = (from x in Data.CardData
                                where x.CardID == cardid
                                select x).FirstOrDefault();
                if (carddata == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    carddata.CardDisable = "True";
                    Data.SaveChanges();
                }
                return RedirectToAction("CardIndex");
            }

        }




    }
}
