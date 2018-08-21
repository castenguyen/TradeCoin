using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            model.NbrComment = cms_db.GetlstComment().Count();
            model.NbrCommentE = cms_db.GetlstComment().Where(s=>s.StateId==(int)EnumCore.StateType.cho_phep).Count();
            model.NbrCommentD = cms_db.GetlstComment().Where(s => s.StateId == (int)EnumCore.StateType.khong_cho_phep).Count();

            model.NbrContentItem = cms_db.GetlstContentItem().Count();
            model.NbrContentItemE = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).Count();
            model.NbrContentItemD = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.khong_cho_phep).Count();

            model.NbrProduct = cms_db.GetlstProduct().Count();
            model.NbrProductE = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).Count();
            model.NbrProductD = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.khong_cho_phep).Count();

            return View(model);
        }



        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod")]
        public ActionResult ModIndex()
        {
            return View();

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


        public ActionResult MainHeaderPartial()
        {
            Config tmp2 = ExtFunction.Config();
            ViewBag.SiteName = tmp2.site_name;
            return PartialView("_MainHeaderPartial");
        }


    }
}