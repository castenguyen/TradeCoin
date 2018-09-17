using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DataModel.Extension;
using System.Threading.Tasks;
using PagedList;
using System.Text;
using DataModel.CoinMaket;
using DataModel.CoinmaketEntity;
using DataModel.CoinmaketEnum;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class MemberController : CoreBackEnd
    {
        // GET: AdminCMS/Member

        /// <summary>
        /// VIEW FOR MEMBER
        /// </summary>
        /// <returns></returns>
        /// 
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public async Task<ActionResult> MemberDashBoard()
        {
            try
            {
                long Package = 0;
                long UserId = long.Parse(User.Identity.GetUserId());
                User ObjectCurentUser = await cms_db.GetObjUserById(UserId);

                MemberFrontEndViewModel model = new MemberFrontEndViewModel();
                List<ContentItemViewModels> lstmpNews = new List<ContentItemViewModels>();
                List<ContentItem> lstNews = new List<ContentItem>();
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    Package = 5;
                    lstNews = cms_db.GetListContentItemByUser(UserId,
                        (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_News_In_Home, Package);
                }
                else
                {
                    Package = ObjectCurentUser.PackageId.Value;
                    lstNews = cms_db.GetListContentItemByUser(UserId,
                        (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_News_In_Home, Package);
                }
                foreach (ContentItem _val in lstNews)
                {
                    ContentItemViewModels tmp = new ContentItemViewModels(_val);
                    tmp.lstNewsContentPackage = cms_db.GetlstObjContentPackage(tmp.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
                    lstmpNews.Add(tmp);
                }




                List<TickerViewModel> lstmpTickers = new List<TickerViewModel>();
                List<Ticker> lstTicker = new List<Ticker>();
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    lstTicker = cms_db.GetListTickerByUser(UserId,
                                     (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, Package);
                }
                else
                {
                    lstTicker = cms_db.GetListTickerByUser(UserId,
                            (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, Package);

                }

                foreach (Ticker _val in lstTicker)
                {
                    TickerViewModel tmp = new TickerViewModel(_val);
                    tmp.lstTickerContentPackage = cms_db.GetlstObjContentPackage(tmp.TickerId, (int)EnumCore.ObjTypeId.ticker);
                    lstmpTickers.Add(tmp);
                }


                model.lstNews = lstmpNews;
                model.lstTicker = lstmpTickers;
                model.ObjectUser = ObjectCurentUser;
                Config cf = new Config();
                cf = cms_db.GetConfig();
                this.SetInforMeta(cf.site_metadatakeyword, cf.site_metadadescription);
               await this.CheckPriceUpdate();
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("MemberDashBoard", "Member", e.ToString());
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
            }

        }

        private async Task<bool> CheckPriceUpdate()
        {
            try {

                string StringLastTimeUpdate = "";
                ///lấy ngày tháng update sau cùng tu bien application
                if (HttpContext.Application["LastTimePriceUpdate"] != null)
                {
                    StringLastTimeUpdate = HttpContext.Application["LastTimePriceUpdate"] as string;
                }
                else
                {
                    HttpContext.Application["LastTimePriceUpdate"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                    StringLastTimeUpdate = HttpContext.Application["LastTimePriceUpdate"] as string;
                }
                ///chuển qua date time
                DateTime lasttime = DateTime.Parse(StringLastTimeUpdate);
                int minute = DateTime.Now.Subtract(lasttime).Minutes;
                ///nếu lớn hơn 1 phút thì lấy tất cả các dong tiền ở kèo
                if (minute >1)
                {
                    long[] listCyptoNeedUpdatePrice = cms_db.GetlstCyptoItem()
                            .Where(s=>s.allow_update==true).Select(s=>s.id).ToArray();

                    string query = this.MakeQueryListCyptoId(listCyptoNeedUpdatePrice);
                    if (String.IsNullOrEmpty(query))
                    {
                        return true;
                    }
                    else
                    {
                        ///xoá cac giá cũ trước khi update
                        List<CyptoItemPrice> tmp = cms_db.GetLstCyptoItemPrice().Where(s => listCyptoNeedUpdatePrice.Contains(s.id)).ToList();
                        cms_db.RemoveListCyptoItemPrice(tmp);


                        //lấy giá của cá đồng tiền có trong hệ thống
                         bool a= await this.DoUpdatePriceCypto(query);
                       

                    }

                }
                return true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CheckPriceUpdate", "Member", e.ToString());
                return false;
            }
        }
        private string MakeQueryListCyptoId(long[] Cyptoid)
        {
            string result = "";
            foreach (long id in Cyptoid)
            {
                if (String.IsNullOrEmpty(result))
                {
                    result = result + id.ToString();
                }
                else {
                    result = result +","+ id.ToString();
                }
               
            }
            return result;
        }

        private async  Task<bool>  DoUpdatePriceCypto(string ListCyptoIdQuery)
        {
            try
            {
                CoinMarketCapClient _client = new CoinMarketCapClient();
                IDictionary<string, CyproPriceEntity> coinmaketresult = await _client.GetListPriceCyptoItemAsync(ListCyptoIdQuery);
                foreach (CyproPriceEntity Item in coinmaketresult.Values)
                {
                    CyptoItemPriceViewModel objCyptoItemPrice = new CyptoItemPriceViewModel();
                    objCyptoItemPrice.id = Item.id;
                    objCyptoItemPrice.name = Item.name;
                    objCyptoItemPrice.symbol = Item.symbol;
                    objCyptoItemPrice.slug = Item.slug;
                    objCyptoItemPrice.CyptoItemPriceUpdate = DateTime.Now;
                    objCyptoItemPrice.is_active = true;
                    objCyptoItemPrice.cmc_rank = Item.cmc_rank;
                    objCyptoItemPrice.num_market_pairs = Item.num_market_pairs;
                    objCyptoItemPrice.circulating_supply = Item.circulating_supply;
                    objCyptoItemPrice.total_supply = Item.total_supply;
                    objCyptoItemPrice.max_supply = Item.max_supply ?? 0;
                    try
                    {
                        objCyptoItemPrice.last_updated = DateTime.Parse(Item.last_updated);
                    }
                    catch
                    {
                        objCyptoItemPrice.last_updated = DateTime.Now;
                    }
                    PriceEntity USD = Item.quote.Values.First();
                    objCyptoItemPrice.USD_price = USD.price;
                    objCyptoItemPrice.USD_volume_24h = USD.volume_24h;
                    objCyptoItemPrice.USD_percent_change_1h = USD.percent_change_1h;
                    objCyptoItemPrice.USD_percent_change_7d = USD.percent_change_7d;
                    objCyptoItemPrice.USD_market_cap = USD.market_cap;
                    CyptoItemPrice modelmain = objCyptoItemPrice._MainObj;

                    await cms_db.CreateCyptoItemPrice(objCyptoItemPrice._MainObj);
                }
                HttpContext.Application["LastTimePriceUpdate"] = DateTime.Now.ToString();
                return true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("ApiUpdatePriceForTicker", "Member", e.ToString());
                return false;
            }
           

        }


        public async Task<JsonResult> ApiUpdatePriceForTicker(string listId)
        {
            try {

                CoinMarketCapClient _client = new CoinMarketCapClient();
                IDictionary<string, CyproPriceEntity> coinmaketresult = await _client.GetListPriceCyptoItemAsync(listId);
                foreach (CyproPriceEntity Item in coinmaketresult.Values)
                {
                    CyptoItemPriceViewModel objCyptoItemPrice = new CyptoItemPriceViewModel();
                    objCyptoItemPrice.id = Item.id;
                    objCyptoItemPrice.name = Item.name;
                    objCyptoItemPrice.symbol = Item.symbol;
                    objCyptoItemPrice.slug = Item.slug;
                    objCyptoItemPrice.CyptoItemPriceUpdate = DateTime.Now;
                    objCyptoItemPrice.is_active = true;
                    objCyptoItemPrice.cmc_rank = Item.cmc_rank;
                    objCyptoItemPrice.num_market_pairs = Item.num_market_pairs;
                    objCyptoItemPrice.circulating_supply = Item.circulating_supply;
                    objCyptoItemPrice.total_supply = Item.total_supply;
                    objCyptoItemPrice.max_supply = Item.max_supply ?? 0;
                    try
                    {
                        objCyptoItemPrice.last_updated = DateTime.Parse(Item.last_updated);
                    }
                    catch
                    {
                        objCyptoItemPrice.last_updated = DateTime.Now;
                    }

                    PriceEntity USD = Item.quote.Values.First();
                    objCyptoItemPrice.USD_price = USD.price;
                    objCyptoItemPrice.USD_volume_24h = USD.volume_24h;
                    objCyptoItemPrice.USD_percent_change_1h = USD.percent_change_1h;
                    objCyptoItemPrice.USD_percent_change_7d = USD.percent_change_7d;
                    objCyptoItemPrice.USD_market_cap = USD.market_cap;

                    CyptoItemPrice modelmain = objCyptoItemPrice._MainObj;

                    await cms_db.CreateCyptoItemPrice(objCyptoItemPrice._MainObj);
                }
                HttpContext.Application["LastTimePriceUpdate"] = DateTime.Now.ToString();
                return Json(coinmaketresult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("ApiUpdatePriceForTicker", "Member", e.ToString());
                return Json(e, JsonRequestBehavior.AllowGet);
            }

          
        }

        private bool UpdatePriceForTickerUSDT()
        {
            try
            {
                List<Ticker> lstTickerNeedUpdate = cms_db.GetlstTicker().Where(s => s.StateId != (int)EnumCore.TickerStatusType.da_xoa && s.USDInput.HasValue).ToList();
                foreach (Ticker MainModel in lstTickerNeedUpdate)
                {
                    string OldValue = "";
                    string NewValue = "";
                    CyptoItemPrice objPrice = cms_db.GetLstCyptoItemPrice().
                        Where(s => s.id == MainModel.CyptoID).OrderByDescending(s => s.CyptoItemPriceId).FirstOrDefault();
                    if (objPrice != null)
                    {
                        if (objPrice.USD_price > MainModel.SellZone3)
                        {
                            OldValue = MainModel.Profit.ToString();
                            MainModel.Flag = 3;
                            MainModel.Profit = this.SumTicker(3, MainModel.BuyZone1.Value, MainModel.SellZone3.Value, MainModel.USDInput.Value);
                            MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                            MainModel.StateName = "Lời";
                            NewValue = MainModel.Profit.ToString();
                        }

                        if (objPrice.USD_price > MainModel.SellZone2)
                        {
                            OldValue = MainModel.Profit.ToString();
                            MainModel.Flag = 2;
                            MainModel.Profit = this.SumTicker(2, MainModel.BuyZone1.Value, MainModel.SellZone2.Value, MainModel.USDInput.Value);
                            MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                            MainModel.StateName = "Lời";
                            NewValue = MainModel.Profit.ToString();
                        }

                        if (objPrice.USD_price > MainModel.SellZone1)
                        {
                            OldValue = MainModel.Profit.ToString();
                            MainModel.Flag = 1;
                            MainModel.Profit = this.SumTicker(1, MainModel.BuyZone1.Value, MainModel.SellZone1.Value, MainModel.USDInput.Value);
                            MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                            MainModel.StateName = "Lời";
                            NewValue = MainModel.Profit.ToString();
                        }

                        if (objPrice.USD_price <= MainModel.Deficit)
                        {
                            OldValue = MainModel.Deficit.ToString();
                            MainModel.Flag = 4;
                            MainModel.Deficit = this.SumTicker(4, MainModel.BuyZone1.Value, MainModel.DeficitControl.Value, MainModel.BTCInput.Value);
                            MainModel.StateId = (int)EnumCore.TickerStatusType.lo;
                            MainModel.StateName = "Lỗ";
                            NewValue = MainModel.Deficit.ToString();
                        }
                        cms_db.UpdateTickerNoasync(MainModel);

                        int rs2 =  cms_db.CreateUserHistoryNoAsync(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                      (int)EnumCore.ActionType.Update, "UpdatePriceForTickerUSDT", MainModel.TickerId, 
                      MainModel.TickerName, ConstantSystem.Table_ticker, (int)EnumCore.ObjTypeId.ticker, OldValue, NewValue);
                    }

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


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult ListTicker(int? page, int ? TickerStatus, long? CyptoItemID, int? MarketItemID, string FillterTickerName, string Datetime)
        {

            string StringLastTimeUpdate = "";
            ///lấy ngày tháng update sau cùng tu bien application
            if (HttpContext.Application["LastTimePriceUpdate"] != null)
            {
                StringLastTimeUpdate = HttpContext.Application["LastTimePriceUpdate"] as string;
            }
            else
            {
                HttpContext.Application["LastTimePriceUpdate"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                StringLastTimeUpdate = HttpContext.Application["LastTimePriceUpdate"] as string;
            }
            ///chuển qua date time
            DateTime lasttime = DateTime.Parse(StringLastTimeUpdate);
            int minute = DateTime.Now.Subtract(lasttime).Minutes;
            ///nếu lớn hơn 1 phút thì lấy tất cả các dong tiền ở kèo
            if (minute >60)
            {
              //  this.UpdatePriceForTickerUSDT();
            }

            long Packageid = 0;
            long UserId = long.Parse(User.Identity.GetUserId());
            User ObjectCurentUser = cms_db.GetObjUserByIdNoAsync(UserId);

            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                Packageid = 5;
            }
            else
            {
                Packageid = ObjectCurentUser.PackageId.Value;
            }
            int pageNum = (page ?? 1);
            TickerMemberViewModel model = new TickerMemberViewModel();
            IQueryable<MiniTickerViewModel> tmp = cms_db.GetTickerByUserLinq(UserId, Packageid);
            if (TickerStatus.HasValue)
            {
                tmp = tmp.Where(s => s.StateId == TickerStatus.Value);
                model.TickerStatus = TickerStatus.Value;
            }
            if (CyptoItemID.HasValue)
            {
                tmp = tmp.Where(s => s.CyptoID == CyptoItemID.Value);
                model.CyptoItemID = CyptoItemID.Value;
            }

            if (MarketItemID.HasValue)
            {
                tmp = tmp.Where(s => s.MarketID == MarketItemID.Value);
                model.MarketItemID = MarketItemID.Value;
            }
            if ( !String.IsNullOrEmpty(FillterTickerName))
            {
                tmp = tmp.Where(s => s.TickerName == FillterTickerName);
                model.FillterTickerName = FillterTickerName;
            }
            if (!String.IsNullOrEmpty(Datetime))
            {
                model.Datetime = Datetime;
                model.StartDT = this.SpritDateTime(model.Datetime)[0];
                model.EndDT = this.SpritDateTime(model.Datetime)[1];
                tmp = tmp.Where(s => s.CrtdDT > model.StartDT && s.CrtdDT < model.EndDT);
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.lstViewUserContent = cms_db.GetlstContentView().Where(s => s.ContentType
                          == (int)EnumCore.ObjTypeId.ticker && s.UserId == UserId).Select(s => s.ContentId).ToArray();
            model.lstMainTicker = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            foreach (MiniTickerViewModel _item in model.lstMainTicker)
            {
                _item.lstTickerContentPackage = cms_db.GetlstObjContentPackage(_item.TickerId, (int)EnumCore.ObjTypeId.ticker);
            }

            model.lstTickerStatus = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.status_ticker), "value", "text");
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text");
            model.lstCyptoItem = new SelectList(cms_db.GetlstCyptoItem().Where(s => s.is_active == true).ToList(), "id", "name");
            model.lstMarketItem = new SelectList(cms_db.GetlstMarketItem().ToList(), "Marketid", "MarketName");

            return View(model);
        }

        private DateTime[] SpritDateTime(string datetime)
        {
            string[] words = datetime.Split('-');
            DateTime[] model = new DateTime[] { Convert.ToDateTime(words[0]), Convert.ToDateTime(words[1]) };
            return model;
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]

        public ActionResult DetailTicker(long tickerId)
        {
            try {


                long Packageid = 0;
                long UserId = long.Parse(User.Identity.GetUserId());
                User ObjectCurentUser = cms_db.GetObjUserByIdNoAsync(UserId);


                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    Packageid = 5;
                }
                else
                {
                    Packageid = ObjectCurentUser.PackageId.Value;
                }
                if (cms_db.CheckTickerUserPackage(tickerId, UserId, Packageid))
                {
                    long UID = long.Parse(User.Identity.GetUserId());
                    TickerViewModel model = new TickerViewModel();
                    Ticker mainObj = cms_db.GetObjTicker(tickerId);
                    model._MainObj = mainObj;

                    List<Ticker> lsttmpSameTicker = new List<Ticker>();
                    model.lstsameTickers = new List<TickerViewModel>();

                        lsttmpSameTicker = cms_db.GetListTickerByUser(UserId,
                                (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, Packageid);

                   
                    foreach (Ticker _val in lsttmpSameTicker)
                    {
                        TickerViewModel tmp = new TickerViewModel(_val);
                        tmp.lstTickerContentPackage = cms_db.GetlstObjContentPackage(tmp.TickerId, (int)EnumCore.ObjTypeId.ticker);
                        model.lstsameTickers.Add(tmp);
                    }

                    ContentView ck = cms_db.GetObjContentView(mainObj.TickerId, (int)EnumCore.ObjTypeId.ticker, UID);
                    if (ck == null)
                    {
                        ContentView tmp = new ContentView();
                        tmp.UserId = UID;
                        tmp.UserName = User.Identity.GetUserName();
                        tmp.ContentId = mainObj.TickerId;
                        tmp.ContentType = (int)EnumCore.ObjTypeId.ticker;
                        tmp.ContentName = mainObj.TickerName;
                        cms_db.CreateContentView(tmp);
                    }
                    return View(model);
                }
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DetailTicker", " Member", e.ToString());
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int) EnumCore.AlertPageType.FullScrenn });
            }
          
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult ListNews(int? page, int? ContentCatalogry, string FillterContenName, string Datetime)
        {
            long Packageid = 0;
            long UserId = long.Parse(User.Identity.GetUserId());
            User ObjectCurentUser = cms_db.GetObjUserByIdNoAsync(UserId);


       
            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                Packageid = 5;
            }
            else
            {
                Packageid = ObjectCurentUser.PackageId.Value;
            }
            int pageNum = (page ?? 1);
            ContentItemMemberViewModel model = new ContentItemMemberViewModel();
            IQueryable<MiniContentItemViewModel> tmp = cms_db.GetContentItemByUserLinq(UserId, Packageid);
        

            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                model.lstTicker = cms_db.GetListTickerByUser(UserId,
                                 (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, Packageid);
            }
            else
            {
                model.lstTicker = cms_db.GetListTickerByUser(UserId,
                        (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, Packageid);

            }
            if (ContentCatalogry.HasValue && ContentCatalogry.Value != 0)
            {
                tmp = tmp.Where(s => s.CategoryId == ContentCatalogry);
                model.ContentCatalogry = ContentCatalogry.Value;
            }
            if (!String.IsNullOrEmpty(FillterContenName))
            {
                tmp = tmp.Where(s => s.ContentTitle.ToLower().Contains(FillterContenName.ToLower()));
                model.FillterContenName = FillterContenName;
            }

            if (!String.IsNullOrEmpty(Datetime))
            {
                model.Datetime = Datetime;
                model.StartDT = this.SpritDateTime(model.Datetime)[0];
                model.EndDT = this.SpritDateTime(model.Datetime)[1];
                tmp = tmp.Where(s => s.CrtdDT > model.StartDT && s.CrtdDT < model.EndDT);
            }
            model.lstViewUserContent = cms_db.GetlstContentView().Where(s => s.ContentType 
                            == (int)EnumCore.ObjTypeId.tin_tuc && s.UserId == UserId).Select(s => s.ContentId).ToArray();
            model.lstMainContent = tmp.OrderByDescending(c => c.ContentItemId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);

            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;

            foreach (MiniContentItemViewModel _val in model.lstMainContent)
            {
                _val.lstContentPackage = cms_db.GetlstObjContentPackage(_val.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
            
            }
            model.lstContentState = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.state_type);
            model.lstContentCatalogry = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.tin_tuc_bai_viet);
            return View(model);
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult DetailNews(long id)
        {
            try
            {


                long Packageid = 0;
                long UserId = long.Parse(User.Identity.GetUserId());
                User ObjectCurentUser = cms_db.GetObjUserByIdNoAsync(UserId);


                if (cms_db.CheckContentItemUerPackage(id, UserId) || User.IsInRole("AdminUser") 
                    || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                  
               
                    if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                    {
                        Packageid = 5;
                    }
                    else
                    {
                        Packageid = ObjectCurentUser.PackageId.Value;
                    }
                
                    ContentItemViewModels model = new ContentItemViewModels(); 
                    ContentItem mainObj = cms_db.GetObjContentItemById(id);
                    model._MainObj = mainObj;

                    model.lstSameNews = cms_db.GetContentItemByUserLinq(UserId, Packageid).Where(s => s.CategoryId == mainObj.CategoryId).Take(10).ToList();
                    model.lstViewUserContent = cms_db.GetlstContentView().Where(s => s.ContentType
                        == (int)EnumCore.ObjTypeId.tin_tuc && s.UserId == UserId).Select(s => s.ContentId).ToArray();

                    ContentView ck = cms_db.GetObjContentView(id, (int)EnumCore.ObjTypeId.tin_tuc, UserId);
                    if (ck == null)
                    {
                        ContentView tmp = new ContentView();
                        tmp.UserId = UserId;
                        tmp.UserName = User.Identity.GetUserName();
                        tmp.ContentId = id;
                        tmp.ContentType = (int)EnumCore.ObjTypeId.tin_tuc;
                        tmp.ContentName = mainObj.ContentTitle;
                        cms_db.CreateContentView(tmp);
                    }
                 
                    return View(model);
                }
                else
                {
                    string AlertString = "Nội dung xem không khả dụng";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
                }
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DetailNews", "MemberDashBoard", e.ToString());
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
            }
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult ListVideo(int? page, int? MediaPackage)
        {
            long Packageid = 0;
            long UserId = long.Parse(User.Identity.GetUserId());
            User ObjectCurentUser = cms_db.GetObjUserByIdNoAsync(UserId);
         
          
            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                Packageid = 5;
            }
            else
            {
                Packageid = ObjectCurentUser.PackageId.Value;
            }
            int pageNum = (page ?? 1);
            MediaMemberViewModel model = new MediaMemberViewModel();



            IQueryable<MiniMediaViewModel> tmp = cms_db.GetMediaByUserLinq(long.Parse(User.Identity.GetUserId()), Packageid);
            if (MediaPackage.HasValue && MediaPackage.Value != 0)
            {
                // tmp = tmp.Where(s => s.StateId == Pakage);
                model.MediaPackage = MediaPackage.Value;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;

            model.lstMainTicker = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            foreach (MiniMediaViewModel _item in model.lstMainTicker)
            {
                _item.lstVideoContentPackage = cms_db.GetlstObjContentPackage(_item.MediaContentId, (int)EnumCore.ObjTypeId.video);
            }
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text");
            return View(model);
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult DetailVideo(long id)
        {

            try
            {
                long Packageid = 0;
                long UserId = long.Parse(User.Identity.GetUserId());
                User ObjectCurentUser = cms_db.GetObjUserByIdNoAsync(UserId);



               
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    Packageid = 5;
                }
                else
                {
                    Packageid = ObjectCurentUser.PackageId.Value;
                }
                if (cms_db.CheckVideoUserPackage(id, UserId, Packageid) || User.IsInRole("AdminUser")
                    || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                  
                    MediaContentViewModels model = new MediaContentViewModels();
                    MediaContent mainObj = cms_db.GetObjMediaContent(id);
                    model.objMediaContent = mainObj;
                    model.lstSameVideo = cms_db.GetListVideoByUser(UserId,
                                (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, Packageid);

                    ContentView ck = cms_db.GetObjContentView(id, (int)EnumCore.ObjTypeId.video, UserId);
                    if (ck == null)
                    {
                        ContentView tmp = new ContentView();
                        tmp.UserId = UserId;
                        tmp.UserName = User.Identity.GetUserName();
                        tmp.ContentId = id;
                        tmp.ContentType = (int)EnumCore.ObjTypeId.video;
                        tmp.ContentName = mainObj.AlternativeText;
                        cms_db.CreateContentView(tmp);
                    }
                    
                    model.objMediaContent.MediaContentGuidId = Guid.NewGuid();

                    Response.Cookies["ncoincookie"].Expires = DateTime.Now.AddMinutes(20);
                        Response.Cookies["ncoincookie"].Value = "nguyenhuyvanguoidep";
                    return View(model);

                }
                else
                {
                    string AlertString = "Nội dung xem không khả dụng";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });

                }
             

            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DetailNews", "MemberDashBoard", e.ToString());
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
            }
           
        }
        public ActionResult iframeVideo()
        {
           
        
            return View();

        }


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult TrackingTicker(int? page,int ? package, string FillterTickerName, string Datetime)
        {
            int pageNum = (page ?? 1);
            TickerMemberViewModel model = new TickerMemberViewModel();
            IQueryable<MiniTickerViewModel> tmp = cms_db.GetTickerLinq();

            tmp = tmp.Where(s => s.StateId != (int)EnumCore.TickerStatusType.dang_chay  && s.StateId != (int)EnumCore.TickerStatusType.da_xoa);
            if (package.HasValue)
            {
               tmp = cms_db.GetTickerLinqByPackage(package.Value);
            }

         

            
            if (!String.IsNullOrEmpty(Datetime))
            {
                model.Datetime = Datetime;
                model.StartDT = this.SpritDateTime(model.Datetime)[0];
                model.EndDT = this.SpritDateTime(model.Datetime)[1];
                tmp = tmp.Where(s => s.CrtdDT > model.StartDT && s.CrtdDT < model.EndDT);
            }

            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;

            model.lstMainTicker = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);

            model.TotalDeficit = 0;
            model.TotalProfit = 0;
            model.TotalNumberBTC = 0;
            foreach (MiniTickerViewModel _item in model.lstMainTicker)
            {
                _item.lstTickerContentPackage = cms_db.GetlstObjContentPackage(_item.TickerId, (int)EnumCore.ObjTypeId.ticker);
                if (_item.BTCInput.HasValue && _item.Deficit.HasValue && _item.Profit.HasValue)
                {
                   
                    if (_item.Flag == 1 || _item.Flag == 2 || _item.Flag == 3)
                    {
                        double tmpProfit = (_item.Profit.Value) * _item.BTCInput.Value;
                        model.TotalProfit = model.TotalProfit + tmpProfit;
                    }
                    else if (_item.Flag == 4)
                    {

                        double tmpDeficit = (_item.Deficit.Value) * _item.BTCInput.Value;
                        model.TotalDeficit = model.TotalDeficit + tmpDeficit;
                    }

                    model.TotalNumberBTC = model.TotalNumberBTC + _item.BTCInput.Value;
                }
            }
            model.Total = model.TotalProfit - model.TotalDeficit;
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text");
            return View(model);
        }
        



    }


}