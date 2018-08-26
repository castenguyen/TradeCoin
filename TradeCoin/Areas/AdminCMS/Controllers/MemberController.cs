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
        public ActionResult ListTicker(int? page)
        {
            int pageNum = (page ?? 1);
            TickerMemberViewModel model = new TickerMemberViewModel();
            IQueryable<MiniTickerViewModel> tmp = cms_db.GetTickerByUserLinq(long.Parse(User.Identity.GetUserId()));
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
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]

        public ActionResult DetailTicker(long tickerId)
        {
            try {
                if (cms_db.CheckTickerUserPackage(tickerId, long.Parse(User.Identity.GetUserId())))
                {
                    long UID = long.Parse(User.Identity.GetUserId());
                    TickerViewModel model = new TickerViewModel();
                    Ticker mainObj = cms_db.GetObjTicker(tickerId);
                    model._MainObj = mainObj;
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
        public ActionResult ListNews(int? page, int? catalogry)
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
            if (catalogry.HasValue && catalogry.Value != 0)
            {
                tmp = tmp.Where(s => s.CategoryId == catalogry);
                model.ContentCatalogry = catalogry.Value;
            }

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
                if (cms_db.CheckContentItemUerPackage(id, long.Parse(User.Identity.GetUserId())) || User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
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
        public ActionResult ListVideo(int? page, int? MediaStatus, int? MediaPackage, string FillterMediaName)
        {
            int pageNum = (page ?? 1);
            MediaMemberViewModel model = new MediaMemberViewModel();
            IQueryable<MediaContent> tmp = cms_db.GetMediaByUserLinq(long.Parse(User.Identity.GetUserId()));

            //if (MediaStatus.HasValue)
            //{
            //    tmp = tmp.Where(s => s.StateId == TickerStatus);
            //    model.TickerStatus = TickerStatus.Value;
            //}

            if (MediaPackage.HasValue && MediaPackage.Value != 0)
            {
                // tmp = tmp.Where(s => s.StateId == Pakage);
                model.MediaPackage = MediaPackage.Value;
            }

            if (!String.IsNullOrEmpty(FillterMediaName))
            {
                tmp = tmp.Where(s => s.Filename.ToLower().Contains(FillterMediaName.ToLower()));
                model.FillterMediaName = FillterMediaName;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;

            IPagedList<MediaContent> tmplstticker = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);

            List<MediaContentViewModels> mainlstticker = new List<MediaContentViewModels>();

            foreach (MediaContent _item in tmplstticker)
            {
                MediaContentViewModels abc = new MediaContentViewModels(_item);
                abc.lstTickerContentPackage = cms_db.GetlstObjContentPackage(_item.MediaContentId, (int)EnumCore.ObjTypeId.ticker);
                mainlstticker.Add(abc);
            }

            model.lstMainTicker = mainlstticker.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
           // model.lstTickerStatus = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.status_ticker), "value", "text");
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text");

            return View(model);
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult DetailVideo(long id)
        {
            MediaContentViewModels model = new MediaContentViewModels();
            MediaContent mainObj = cms_db.GetObjMediaContent(id);
            model.objMediaContent = mainObj;
            model.lstSameVideo = cms_db.GetLstMediaContent().Where(s=>s.MediaTypeId==(int)EnumCore.ObjTypeId.video).Take(10).ToList();


            return View(model);

        }

    }
}