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

    public class PackageManagerController : CoreBackEnd
    {
       
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            IPagedList<Package> _lstPackage = cms_db.GetlstPackage()
                    .OrderBy(c => c.PackageId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            return View(_lstPackage);
        }

        public ActionResult FrontEndIndex()
        {
            IPagedList<Package> _lstPackage = cms_db.GetlstPackage()
                    .OrderBy(c => c.PackageId).ToPagedList(1, (int)EnumCore.BackendConst.page_size);
            return View(_lstPackage);
        }

        public ActionResult ConfirmUpdate(long UserId,long PackageId)
        {
            try {

                long CurrentUserId= long.Parse(User.Identity.GetUserId());
                if (CurrentUserId != UserId)
                {
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = "Không thể thực hiện nâng cấp với gói cước này", link = "" });
                }
                UserPackageViewModel model = new UserPackageViewModel();
                User ObjUser = cms_db.GetObjUserByIdNoAsync(UserId);
                Package ObjNewPackage = cms_db.GetObjPackage(PackageId);
                Package ObjOldPackage = cms_db.GetObjPackage(ObjUser.PackageId.Value);

                if (ObjNewPackage.NumDay > ObjOldPackage.NumDay && (ObjUser.AwaitPackageId != null || ObjUser.AwaitPackageId != 0))
                {
                    model.ObjPackage = cms_db.GetObjPackage(PackageId);
                    model.ObjUser = cms_db.GetObjUserByIdNoAsync(UserId);
                    return View(model);
                }
                return RedirectToAction("AlertPage", "Extension", new { AlertString="Không thể thực hiện nâng cấp với gói cước này", link="" });
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("ConfirmUpdate", "PackageManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("FrontEndIndex");
            }
        }

        public ActionResult UpdatePackageForUser(long UserId, long PackageId)
        {
            UserPackageViewModel model = new UserPackageViewModel();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PackageViewModel model)
        {
            Package MainModel = model._MainObj;
            MainModel.CrtdDT = DateTime.Now;
            MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
            MainModel.CrtdUserName = User.Identity.Name;
            MainModel.StateId = (int)EnumCore.StateType.enable;
            MainModel.StateName = "Enable";
            int rs = await cms_db.CreatePackageAsync(MainModel);
            int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                (int)EnumCore.ActionType.Create, "Create", MainModel.PackageId, MainModel.PackageName, "PackageManager", (int)EnumCore.ObjTypeId.package);
            return RedirectToAction("Index");
        }

      public ActionResult Update(int? id)
        {
            if (id == null)
                id = 1;
            Package _obj = cms_db.GetObjPackage(id.Value);
            PackageViewModel model = new PackageViewModel(_obj);
            return View(model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(PackageViewModel model)
        {
            Package _ObjPackage = model._MainObj;
            int rs = await cms_db.UpdatePackage(_ObjPackage);
            int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                (int)EnumCore.ActionType.Update, "Update", model.PackageId, model.PackageName, "PackageManager", (int)EnumCore.ObjTypeId.package);
            if (rs == (int)EnumCore.Result.action_true && rs2 == (int)EnumCore.Result.action_true)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult ChangeStatus()
        {
            return View();
        }

    }
}