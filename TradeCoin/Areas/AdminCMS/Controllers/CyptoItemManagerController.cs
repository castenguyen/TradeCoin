using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using DataModel.DataViewModel;
using DataModel.DataStore;
using CMSPROJECT.Areas.AdminCMS.Core;
using System;
using System.Collections.Generic;
using DataModel.Extension;
using DataModel.DataEntity;
using PagedList;
using System.Linq;
//using MongoData.Models;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class CyptoItemManagerController : CoreBackEnd
    {


        [AdminAuthorize(Roles = "devuser,AdminUser")]
        public ActionResult Index(int? page, string letter, int? status)
        {
            int pageNum = (page ?? 1);
            IndexCyptoManager model = new IndexCyptoManager();
            IQueryable<CyptoItem> tmp = cms_db.GetlstCyptoItem();
            if (!String.IsNullOrEmpty(letter))
            {
                letter = letter.ToLower();
                tmp = tmp.Where(c => c.symbol.StartsWith(letter) || c.symbol.StartsWith(letter));
                model.letter = letter;
            }
            if (status.HasValue)
            {
                if (status.Value == 0)
                {
                    tmp = tmp.Where(s => s.is_active == false);
                }
                else
                {
                    tmp = tmp.Where(s => s.is_active == true);
                }

                model.status = status.Value;
            }



            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.page = pageNum;
            model.lstMainCypto = tmp.OrderBy(c => c.name).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            return View(model);

        }


        [AdminAuthorize(Roles = "devuser,AdminUser")]
        public ActionResult EditCypto(long id)
        {
            CyptoItemViewModel model = new CyptoItemViewModel();
            model._MainObj = cms_db.GetObjCyptoItem(id);
            return View(model);
        }

        [AdminAuthorize(Roles = "devuser,AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCypto(CyptoItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                CyptoItem ObjCyptoItem = cms_db.GetObjCyptoItem(model.id);
                ObjCyptoItem.is_active = model.is_active;
                ObjCyptoItem.allow_update = model.allow_update;

                int rs = await cms_db.UpdateCypto(ObjCyptoItem);
                return RedirectToAction("EditCypto", new { id = model.id});
            }
            return RedirectToAction("Index");
        }








        [AdminAuthorize(Roles = "devuser")]
        public ActionResult EditCyptoPrice(long CyptoItemPriceId)
        {
            CyptoItemPrice model = new CyptoItemPrice();
            model = cms_db.GetObjCyptoItemPrice(CyptoItemPriceId);
            return View(model);
        }

        [AdminAuthorize(Roles = "devuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCyptoPrice(CyptoItemPrice model)
        {
            if (ModelState.IsValid)
            {
                CyptoItemPrice ObjCyptoItemPryce = cms_db.GetObjCyptoItemPrice(model.CyptoItemPriceId);
                ObjCyptoItemPryce.USD_price = model.USD_price;
                ObjCyptoItemPryce.BTC_price = model.BTC_price;

                int rs = await cms_db.UpdateCyptoPrice(ObjCyptoItemPryce);
                return RedirectToAction("IndexCyptoPrice");
            }
            return RedirectToAction("IndexCyptoPrice");
        }



        [AdminAuthorize(Roles = "devuser")]
        public async Task<ActionResult> DoUpdatePriceTicker()
        {
            try
            {
                long[] CyptoItemPriceId = cms_db.GetLstCyptoItemPrice().Select(s => s.id).ToArray();

                List<Ticker> lstTickerNeedUpdate = cms_db.GetlstTicker().Where(s => s.StateId
                != (int)EnumCore.TickerStatusType.da_xoa && s.Flag != 3 && s.Flag != 4 && CyptoItemPriceId.Contains(s.CyptoID.Value)).ToList();


                foreach (Ticker mainTicker in lstTickerNeedUpdate)
                {
                    if (mainTicker.BTCInput.HasValue)
                    {
                        this.UpdatePriceForTickerBTC(mainTicker);
                    }
                    else if (mainTicker.USDInput.HasValue)
                    {
                        this.UpdatePriceForTickerUSDT(mainTicker);
                    }
                    else
                    {

                    }

                }
                return RedirectToAction("IndexCyptoPrice");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DoUpdatePriceTicker", "Member", e.ToString());
                return RedirectToAction("IndexCyptoPrice");
            }


        }





        /// <summary>
        /// cập nhật trạng thái lỗ lời cho tất cả kèo usdt
        /// </summary>
        /// <returns></returns>
        private bool UpdatePriceForTickerUSDT(Ticker MainModel)
        {
            try
            {

                //lấy tất cả các kèo usdt
                //  List<Ticker> lstTickerNeedUpdate = cms_db.GetlstTicker().Where(s => s.StateId != (int)EnumCore.TickerStatusType.da_xoa && s.USDInput.HasValue).ToList();

                string OldValue = "";
                string NewValue = "";
                CyptoItemPrice objPrice = cms_db.GetLstCyptoItemPrice().
                    Where(s => s.id == MainModel.CyptoID).OrderByDescending(s => s.CyptoItemPriceId).FirstOrDefault();
                if (objPrice != null)
                {
                    //nếu dã là lời target 3 thì ko cap nhat giá
                    if (MainModel.Flag == 3)
                    {

                        return true;
                    }
                    //nếu dã là lỗ  thì ko cap nhat giá
                    else if (MainModel.Flag == 4)
                    {

                        return true;
                    }
                    if (objPrice.USD_price >= MainModel.SellZone3)
                    {
                        OldValue = MainModel.Profit.ToString();
                        MainModel.Flag = 3;
                        MainModel.Profit = this.SumTicker(3, MainModel.BuyZone1.Value, MainModel.SellZone3.Value, MainModel.USDInput.Value);
                        MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                        MainModel.StateName = "Lời";
                        NewValue = MainModel.Profit.ToString();


                    }

                    else if (objPrice.USD_price >= MainModel.SellZone2)
                    {
                        OldValue = MainModel.Profit.ToString();
                        MainModel.Flag = 2;
                        MainModel.Profit = this.SumTicker(2, MainModel.BuyZone1.Value, MainModel.SellZone2.Value, MainModel.USDInput.Value);
                        MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                        MainModel.StateName = "Lời";
                        NewValue = MainModel.Profit.ToString();
                    }

                    else if (objPrice.USD_price >= MainModel.SellZone1)
                    {
                        OldValue = MainModel.Profit.ToString();
                        MainModel.Flag = 1;
                        MainModel.Profit = this.SumTicker(1, MainModel.BuyZone1.Value, MainModel.SellZone1.Value, MainModel.USDInput.Value);
                        MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                        MainModel.StateName = "Lời";
                        NewValue = MainModel.Profit.ToString();
                    }

                    else if (objPrice.USD_price <= MainModel.Deficit)
                    {
                        OldValue = MainModel.Deficit.ToString();
                        MainModel.Flag = 4;
                        MainModel.Deficit = this.SumTicker(4, MainModel.BuyZone1.Value, MainModel.DeficitControl.Value, MainModel.USDInput.Value);
                        MainModel.StateId = (int)EnumCore.TickerStatusType.lo;
                        MainModel.StateName = "Lỗ";
                        NewValue = MainModel.Deficit.ToString();
                    }

                    cms_db.UpdateTickerNoasync(MainModel);

                    int rs2 = cms_db.CreateUserHistoryNoAsync(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                  (int)EnumCore.ActionType.Update, "UpdatePriceForDetailTickerUSDT", MainModel.TickerId,
                  MainModel.TickerName, ConstantSystem.Table_ticker, (int)EnumCore.ObjTypeId.ticker, OldValue, NewValue);
                }


                return true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("UpdatePriceForTickerUSDT", " Member", e.ToString());
                return false;
            }

        }




        /// <summary>
        /// cập nhật trạng thái lỗ lời cho tất cả kèo BTC
        /// </summary>
        /// <returns></returns>
        private bool UpdatePriceForTickerBTC(Ticker MainModel)
        {
            try
            {

                string OldValue = "";
                string NewValue = "";
                CyptoItemPrice objPrice = cms_db.GetLstCyptoItemPrice().
                    Where(s => s.id == MainModel.CyptoID).OrderByDescending(s => s.CyptoItemPriceId).FirstOrDefault();
                if (objPrice != null)
                {
                    //nếu dã là lời target 3 thì ko cap nhat giá
                    if (MainModel.Flag == 3)
                    {

                        return true;
                    }
                    //nếu dã là lỗ  thì ko cap nhat giá
                    else if (MainModel.Flag == 4)
                    {

                        return true;
                    }
                    else if (objPrice.BTC_price >= MainModel.SellZone3)
                    {
                        OldValue = MainModel.Profit.ToString();
                        MainModel.Flag = 3;
                        MainModel.Profit = this.SumTicker(3, MainModel.BuyZone1.Value, MainModel.SellZone3.Value, MainModel.BTCInput.Value);
                        MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                        MainModel.StateName = "Lời";
                        NewValue = MainModel.Profit.ToString();


                    }

                    else if (objPrice.BTC_price >= MainModel.SellZone2)
                    {
                        OldValue = MainModel.Profit.ToString();
                        MainModel.Flag = 2;
                        MainModel.Profit = this.SumTicker(2, MainModel.BuyZone1.Value, MainModel.SellZone2.Value, MainModel.BTCInput.Value);
                        MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                        MainModel.StateName = "Lời";
                        NewValue = MainModel.Profit.ToString();
                    }

                    else if (objPrice.BTC_price >= MainModel.SellZone1 && MainModel.Flag != 2)
                    {
                        OldValue = MainModel.Profit.ToString();
                        MainModel.Flag = 1;
                        MainModel.Profit = this.SumTicker(1, MainModel.BuyZone1.Value, MainModel.SellZone1.Value, MainModel.BTCInput.Value);
                        MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                        MainModel.StateName = "Lời";
                        NewValue = MainModel.Profit.ToString();
                    }

                    else if (objPrice.BTC_price <= MainModel.DeficitControl)
                    {
                        OldValue = MainModel.Deficit.ToString();
                        MainModel.Flag = 4;
                        MainModel.Deficit = this.SumTicker(4, MainModel.BuyZone1.Value, MainModel.DeficitControl.Value, MainModel.BTCInput.Value);
                        MainModel.StateId = (int)EnumCore.TickerStatusType.lo;
                        MainModel.StateName = "Lỗ";
                        NewValue = MainModel.Deficit.ToString();
                    }

                    cms_db.UpdateTickerNoasync(MainModel);

                    int rs2 = cms_db.CreateUserHistoryNoAsync(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                  (int)EnumCore.ActionType.Update, "UpdatePriceForDetailTickerBTC", MainModel.TickerId,
                  MainModel.TickerName, ConstantSystem.Table_ticker, (int)EnumCore.ObjTypeId.ticker, OldValue, NewValue);
                }


                return true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("UpdatePriceForTickerUSDT", " Member", e.ToString());
                return false;
            }

        }





        private double SumTicker(int FlagZone, double zonebuy, double zonesell, double inputbtc)
        {
            ///cong thức là (vung bán - vùng mua) /vung mua * 100
            double result = 0;
            if (FlagZone == 1 || FlagZone == 2 || FlagZone == 3)
            {
                double minus = zonesell - zonebuy;
                result = minus / zonebuy * 100;

            }
            else if (FlagZone == 4)
            {
                double minus = zonesell - zonebuy;
                result = minus / zonebuy * 100 * -1;
            }
            return result;
        }









        [AdminAuthorize(Roles = "devuser,AdminUser")]
        public ActionResult IndexCyptoPrice(int? page, string letter)
        {
            int pageNum = (page ?? 1);
            IndexCyptoPriceManager model = new IndexCyptoPriceManager();
            IQueryable<CyptoItemPrice> tmp = cms_db.GetlstCyptoItemPrice();
            if (!String.IsNullOrEmpty(letter))
            {
                letter = letter.ToLower();
                tmp = tmp.Where(c => c.name.StartsWith(letter) || c.name.StartsWith(letter));
                model.letter = letter;
            }
        
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.page = pageNum;
            model.lstMainCypto = tmp.OrderBy(c => c.name).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            return View(model);

        }







    }
}