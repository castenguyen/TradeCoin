using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using DataModel.Extension;
using System.Threading.Tasks;
using PagedList;
using System.Text;


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

            List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;
            if (lstPackageOfUser == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            MemberFrontEndViewModel model = new MemberFrontEndViewModel();
            List<ContentItemViewModels> lstmpNews = new List<ContentItemViewModels>();
            List<ContentItem> lstNews = new List<ContentItem>();
            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                lstNews = cms_db.GetListContentItemByUser(long.Parse(User.Identity.GetUserId()),
                    (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_News_In_Home, long.Parse("5"));
            }
            else
            {
                lstNews = cms_db.GetListContentItemByUser(long.Parse(User.Identity.GetUserId()),
                    (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_News_In_Home, lstPackageOfUser[0].PackageId);
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
               lstTicker = cms_db.GetListTickerByUser(long.Parse(User.Identity.GetUserId()),
                                (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, long.Parse("5"));
            }
            else
            {
                lstTicker = cms_db.GetListTickerByUser(long.Parse(User.Identity.GetUserId()),
                        (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, lstPackageOfUser[0].PackageId);

            }
               
            foreach (Ticker _val in lstTicker)
            {
                TickerViewModel tmp = new TickerViewModel(_val);
                tmp.lstTickerContentPackage = cms_db.GetlstObjContentPackage(tmp.TickerId, (int)EnumCore.ObjTypeId.ticker);
                lstmpTickers.Add(tmp);
            }


            model.lstNews = lstmpNews;
            model.lstTicker = lstmpTickers;
            model.ObjectUser = await cms_db.GetObjUserById(long.Parse(User.Identity.GetUserId()));
            Config cf = new Config();
            cf = cms_db.GetConfig();
            this.SetInforMeta(cf.site_metadatakeyword, cf.site_metadadescription);

            return View(model);
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult ListTicker(int? page, int ? TickerStatus, string FillterTickerName, string Datetime)
        {
            long packageID = 0;
            List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;
            if (lstPackageOfUser == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                packageID = 5;
            }
            else
            {
                packageID = lstPackageOfUser[0].PackageId;
            }
            int pageNum = (page ?? 1);
            TickerMemberViewModel model = new TickerMemberViewModel();
            IQueryable<MiniTickerViewModel> tmp = cms_db.GetTickerByUserLinq(long.Parse(User.Identity.GetUserId()), packageID);
            if (TickerStatus.HasValue)
            {
                tmp = tmp.Where(s => s.StateId == TickerStatus.Value);
                model.TickerStatus = TickerStatus.Value;
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

            model.lstMainTicker = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            foreach (MiniTickerViewModel _item in model.lstMainTicker)
            {
                _item.lstTickerContentPackage = cms_db.GetlstObjContentPackage(_item.TickerId, (int)EnumCore.ObjTypeId.ticker);
            }

            model.lstTickerStatus = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.status_ticker), "value", "text");
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text");
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
                long packageID = 0;
                List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;
                if (lstPackageOfUser == null)
                {
                    return RedirectToAction("Login", "AccountAdmin");
                }
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    packageID = 5;
                }
                else
                {
                    packageID = lstPackageOfUser[0].PackageId;
                }
                if (cms_db.CheckTickerUserPackage(tickerId, long.Parse(User.Identity.GetUserId()), packageID))
                {
                    long UID = long.Parse(User.Identity.GetUserId());
                    TickerViewModel model = new TickerViewModel();
                    Ticker mainObj = cms_db.GetObjTicker(tickerId);
                    model._MainObj = mainObj;

                    List<Ticker> lsttmpSameTicker = new List<Ticker>();
                    model.lstsameTickers = new List<TickerViewModel>();

                        lsttmpSameTicker = cms_db.GetListTickerByUser(long.Parse(User.Identity.GetUserId()),
                                (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, packageID);

                   
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
            long packageID = 0;
            List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;
            if (lstPackageOfUser == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                packageID = 5;
            }
            else
            {
                packageID = lstPackageOfUser[0].PackageId;
            }
            int pageNum = (page ?? 1);
            ContentItemMemberViewModel model = new ContentItemMemberViewModel();
            IQueryable<MiniContentItemViewModel> tmp = cms_db.GetContentItemByUserLinq(long.Parse(User.Identity.GetUserId()), packageID);
        

            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;

            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                model.lstTicker = cms_db.GetListTickerByUser(long.Parse(User.Identity.GetUserId()),
                                 (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, long.Parse("5"));
            }
            else
            {
                model.lstTicker = cms_db.GetListTickerByUser(long.Parse(User.Identity.GetUserId()),
                        (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, lstPackageOfUser[0].PackageId);

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

            model.lstMainContent = tmp.OrderByDescending(c => c.ContentItemId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
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
                if (cms_db.CheckContentItemUerPackage(id, long.Parse(User.Identity.GetUserId())) || User.IsInRole("AdminUser") 
                    || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    long packageID = 0;
                    List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;
                    if (lstPackageOfUser == null)
                    {
                        return RedirectToAction("Login", "AccountAdmin");
                    }
                    if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                    {
                        packageID = 5;
                    }
                    else
                    {
                        packageID = lstPackageOfUser[0].PackageId;
                    }
                    long UID = long.Parse(User.Identity.GetUserId());
                    ContentItemViewModels model = new ContentItemViewModels(); 
                    ContentItem mainObj = cms_db.GetObjContentItemById(id);
                    model._MainObj = mainObj;
                    model.lstSameNews = cms_db.GetContentItemByUserLinq(UID, packageID).Where(s => s.CategoryId == mainObj.CategoryId).Take(10).ToList();
                    ContentView ck = cms_db.GetObjContentView(id, (int)EnumCore.ObjTypeId.tin_tuc, UID);
                    if (ck == null)
                    {
                        ContentView tmp = new ContentView();
                        tmp.UserId = UID;
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
            long packageID = 0;
            List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;
            if (lstPackageOfUser == null)
            {
                return RedirectToAction("Login", "AccountAdmin");
            }
            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                packageID = 5;
            }
            else
            {
                packageID = lstPackageOfUser[0].PackageId;
            }
            int pageNum = (page ?? 1);
            MediaMemberViewModel model = new MediaMemberViewModel();



            IQueryable<MiniMediaViewModel> tmp = cms_db.GetMediaByUserLinq(long.Parse(User.Identity.GetUserId()), packageID);
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


             

                long packageID = 0;
                List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;
                if (lstPackageOfUser == null)
                {
                    return RedirectToAction("Login", "AccountAdmin");
                }
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    packageID = 5;
                }
                else
                {
                    packageID = lstPackageOfUser[0].PackageId;
                }
                if (cms_db.CheckVideoUserPackage(id, long.Parse(User.Identity.GetUserId()), packageID) || User.IsInRole("AdminUser")
                    || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    long UID = long.Parse(User.Identity.GetUserId());
                    MediaContentViewModels model = new MediaContentViewModels();
                    MediaContent mainObj = cms_db.GetObjMediaContent(id);
                    model.objMediaContent = mainObj;
                    model.lstSameVideo = cms_db.GetListVideoByUser(long.Parse(User.Identity.GetUserId()),
                                (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, packageID);

                    ContentView ck = cms_db.GetObjContentView(id, (int)EnumCore.ObjTypeId.video, UID);
                    if (ck == null)
                    {
                        ContentView tmp = new ContentView();
                        tmp.UserId = UID;
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