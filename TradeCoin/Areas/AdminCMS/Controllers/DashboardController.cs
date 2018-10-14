using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    
    public class DashboardController : CoreBackEnd
    {
        private MyUserManager _userManager;
        public MyUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<MyUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Index()
        {

            DashboardViewModel model = new DashboardViewModel();
          

            model.NbrContentItem = cms_db.GetlstContentItem().Count();
            model.NbrContentItemE = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).Count();
            model.NbrContentItemD = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.khong_cho_phep).Count();

            /*
            model.NbrProduct = cms_db.GetlstProduct().Count();
            model.NbrProductE = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).Count();
            model.NbrProductD = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.khong_cho_phep).Count();

            model.NbrComment = cms_db.GetlstComment().Count();
            model.NbrCommentE = cms_db.GetlstComment().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).Count();
            model.NbrCommentD = cms_db.GetlstComment().Where(s => s.StateId == (int)EnumCore.StateType.khong_cho_phep).Count();
            */


            model.NbrTicker = cms_db.GetlstTicker().Count();
            model.NbrTickerE = cms_db.GetlstTicker().Where(s => s.StateId == (int)EnumCore.TickerStatusType.dang_chay).Count();
            model.NbrTickerD = cms_db.GetlstTicker().Where(s => s.StateId == (int)EnumCore.TickerStatusType.lo).Count();
            model.NbrTickerP = cms_db.GetlstTicker().Where(s => s.StateId == (int)EnumCore.TickerStatusType.loi).Count();

            model.NbrUser = cms_db.GetlstUser().Count();
            model.NbrUserE = cms_db.GetlstUser().Where(s => s.PackageId == 1 ).Count();
            model.NbrUserD = cms_db.GetlstUser().Where(s => s.EmailConfirmed != true).Count();


            model.NbrEmail= cms_db.GetlstEmailSupport().Count();
            model.NbrEmailE = cms_db.GetlstEmailSupport().Where(s => s.DestinationId > 0).Count();
            model.NbrEmailD = cms_db.GetlstEmailSupport().Where(s => s.DestinationId == -1).Count();

            /*

                 model.NbrUser = cms_db.G().Count();
              model.NbrUserE = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).Count();
              model.NbrUserD = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.khong_cho_phep).Count();

              model.NbrEmail cms_db.GetlstProduct().Count();
              model.NbrEmailE = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).Count();
              model.NbrEmailD = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.khong_cho_phep).Count();

               */



            return View(model);
        }



        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod")]
        public async Task<ActionResult> ModIndex()
        {
            MemberFrontEndViewModel model = new MemberFrontEndViewModel();


            List<ContentItemViewModels> lstmpNews = new List<ContentItemViewModels>();
            List<ContentItem> lstNews = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep)
                                            .Take((int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_News_In_Home).ToList();
            foreach (ContentItem _val in lstNews)
            {
                ContentItemViewModels tmp = new ContentItemViewModels(_val);
                tmp.lstNewsContentPackage = cms_db.GetlstObjContentPackage(tmp.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
                lstmpNews.Add(tmp);
            }

            List<TickerViewModel> lstmpTickers = new List<TickerViewModel>();
            List<Ticker> lstTicker = cms_db.GetlstTicker().Where(s => s.StateId != (int)EnumCore.TickerStatusType.da_xoa)
                                            .Take((int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home).ToList();

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
        public ActionResult MainSliderPartial()
        {

            MainSliderAdminViewModel MainModel = new MainSliderAdminViewModel();
            Config tmp2 = ExtFunction.Config();
            ViewBag.SiteName = tmp2.site_name;
            long iduser = long.Parse(User.Identity.GetUserId());
            bool rs = UserManager.IsInRole(iduser, "AdminUser");
            return PartialView("_MainSliderPartial", MainModel);
        }

        public ActionResult ControlSidebarPartial()
        {
            BoxMargin MainModel = new BoxMargin();
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Mod") || User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin"))
                {
                    MainModel.packageId = 5;
                }
                else
                {
                    long UserId = long.Parse(User.Identity.GetUserId());
                    User ObjectCurentUser = cms_db.GetObjUserByIdNoAsync(UserId);
                    MainModel.packageId = ObjectCurentUser.PackageId.Value;
                }
             
            }
            else
            {
                MainModel.packageId = 0;
            }

            try {


                MainModel.FreeObject = cms_db.GetMarginByPackageLinq((int)EnumCore.Package.free);
                MainModel.DemObject = cms_db.GetMarginByPackageLinq((int)EnumCore.Package.dem);
                MainModel.VangObject = cms_db.GetMarginByPackageLinq((int)EnumCore.Package.vang);
                MainModel.KCObject = cms_db.GetMarginByPackageLinq((int)EnumCore.Package.kimcuong);
            }
            catch {



            }
          
            return PartialView("_ControlSidebarPartial", MainModel);

        }

            


        public ActionResult MainHeaderPartial()
        {
            Config tmp2 = ExtFunction.Config();
            ViewBag.SiteName = tmp2.site_name;
            return PartialView("_MainHeaderPartial");
        }


    }
}