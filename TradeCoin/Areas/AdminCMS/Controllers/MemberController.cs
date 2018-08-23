using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{

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

            MemberFrontEndViewModel model = new MemberFrontEndViewModel();
            List<ContentItemViewModels> lstmpNews = new List<ContentItemViewModels>();
            List<ContentItem> lstNews = cms_db.GetListContentItemByUser(long.Parse(User.Identity.GetUserId()),(int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_News_In_Home);
            foreach (ContentItem _val in lstNews)
            {
                ContentItemViewModels tmp =new ContentItemViewModels(_val);
                tmp.lstNewsContentPackage = cms_db.GetlstObjContentPackage(tmp.ContentItemId,(int)EnumCore.ObjTypeId.tin_tuc);
                lstmpNews.Add(tmp);
            }




            List<TickerViewModel> lstmpTickers = new List<TickerViewModel>();
       
            List<Ticker> lstTicker = cms_db.GetListTickerByUser(long.Parse(User.Identity.GetUserId()), (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home);
            foreach (Ticker _val in lstTicker)
            {
                TickerViewModel tmp = new TickerViewModel(_val);
                tmp.lstTickerContentPackage = cms_db.GetlstObjContentPackage(tmp.TickerId, (int)EnumCore.ObjTypeId.ticker);
                lstmpTickers.Add(tmp);
            }


            model.lstNews = lstmpNews;
            model.lstTicker = lstmpTickers;
            model.ObjectUser =await cms_db.GetObjUserById(long.Parse(User.Identity.GetUserId()));
            Config cf = new Config();
            cf = cms_db.GetConfig();
           this.SetInforMeta(cf.site_metadatakeyword, cf.site_metadadescription);

            return View(model);
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult ListTicker(int? page, int? TickerStatus, int? TickerPackage, string FillterTickerName)
        {
            int pageNum = (page ?? 1);
            TickerMemberViewModel model = new TickerMemberViewModel();
            IQueryable<Ticker> tmp = cms_db.GetlstTicker().Where(s => s.StateId != (int)EnumCore.TickerStatusType.da_xoa);

            if (TickerStatus.HasValue)
            {
                tmp = tmp.Where(s => s.StateId == TickerStatus);
                model.TickerStatus = TickerStatus.Value;
            }

            if (TickerPackage.HasValue && TickerPackage.Value != 0)
            {
                // tmp = tmp.Where(s => s.StateId == Pakage);
                model.TickerPackage = TickerPackage.Value;
            }

            if (!String.IsNullOrEmpty(FillterTickerName))
            {
                tmp = tmp.Where(s => s.TickerName.ToLower().Contains(FillterTickerName.ToLower()));
                model.FillterTickerName = FillterTickerName;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;

            IPagedList<Ticker> tmplstticker = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);

            List<TickerViewModel> mainlstticker = new List<TickerViewModel>();

            foreach (Ticker _item in tmplstticker)
            {
                TickerViewModel abc = new TickerViewModel(_item);
                abc.lstTickerContentPackage = cms_db.GetlstObjContentPackage(_item.TickerId, (int)EnumCore.ObjTypeId.ticker);
                mainlstticker.Add(abc);
            }

            model.lstMainTicker = mainlstticker.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.lstTickerStatus = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.status_ticker), "value", "text");
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text");

            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]

        public ActionResult DetailTicker(long tickerId)
        {
            TickerViewModel model = new TickerViewModel();
            Ticker mainObj = cms_db.GetObjTicker(tickerId);
            model._MainObj = mainObj;
            return View(model);
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult ListNews(int? page, int? catalogry, int? state)
        {
            int pageNum = (page ?? 1);
            ContentItemIndexViewModel model = new ContentItemIndexViewModel();
            IQueryable<ContentItem> tmp = cms_db.GetlstContentItem().Where(s => (s.MicrositeID == null || s.MicrositeID == 0) && s.StateId != (int)EnumCore.StateType.da_xoa && s.ObjTypeId == (int)EnumCore.ObjTypeId.tin_tuc);
            if (catalogry.HasValue && catalogry.Value != 0)
            {
                tmp = tmp.Where(s => s.CategoryId == catalogry);
                model.ContentCatalogry = catalogry.Value;
            }

            if (state.HasValue && state.Value != 0)
            {
                tmp = tmp.Where(s => s.StateId == state);
                model.ContentState = state.Value;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.lstTicker = cms_db.GetlstTicker().Where(s => s.StateId != (int)EnumCore.TickerStatusType.da_xoa).Take(10).ToList();
            model.lstMainContent = tmp.OrderByDescending(c => c.ContentItemId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
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
            ContentItemViewModels model = new ContentItemViewModels();
            ContentItem mainObj = cms_db.GetObjContentItemById(id);
            model._MainObj = mainObj;
            model.lstSameNews = cms_db.GetlstContentItemByCataId(mainObj.CategoryId.Value,10);


            return View(model);
       
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Member")]
        public ActionResult ListVideo(int? page, int? MediaStatus, int? MediaPackage, string FillterMediaName)
        {
            int pageNum = (page ?? 1);
            MediaMemberViewModel model = new MediaMemberViewModel();
            IQueryable<MediaContent> tmp = cms_db.GetLstMediaContent().Where(s => s.MediaTypeId != (int)EnumCore.ObjTypeId.video && s.ObjTypeId== (int)EnumCore.ObjTypeId.video);

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