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


        [AllowAnonymous]
        public ActionResult ConfirmUpgrade(long UserId, long PackageId,int PackageTimeType)
        {
            try
            {
                long CurrentUserId = long.Parse(User.Identity.GetUserId());
                //Nếu user đang online ko phải là user nâng cấp=>failse
                //hoặc user không phải là member thì không nâng cấp
                if (CurrentUserId != UserId || !User.IsInRole("Member"))
                {
                    return RedirectToAction("AlertPage", "Extension", 
                        new { AlertString = "Không thể thực hiện nâng cấp với gói cước này", link = "", type = (int)EnumCore.AlertPageType.FullScrenn });
                }
                User ObjUser = cms_db.GetObjUserByIdNoAsync(UserId);

                Package ObjOldPackage = new Package();
                //nếu gói cước của user là null hoặc = 1 là free
                if (ObjUser.PackageId.HasValue)
                {
                    ObjOldPackage = cms_db.GetObjPackage(ObjUser.PackageId.Value);
                }
                else
                {
                    //1 là package free
                    ObjOldPackage = cms_db.GetObjPackage(1);
                }

                UserPackageViewModel model = new UserPackageViewModel();
                Package ObjNewPackage = cms_db.GetObjPackage(PackageId);
                if (ObjNewPackage.NumDay > ObjOldPackage.NumDay)
                {
                    model.ObjPackage = ObjNewPackage;
                    model.ObjUser = ObjUser;
                    model.PackageName = ObjNewPackage.PackageName;
                    model.UpgradeUID = ObjUser.Id;
                    model.UpgradeUserName = ObjUser.EMail;
                    model.CrtdDT = DateTime.Now;
                    if (PackageTimeType == (int)EnumCore.PackageTimeType.thang)
                    {
                        model.TotalPrice = ObjNewPackage.NewPrice.Value;
                        model.TotalDay = 30;
                    }

                    if (PackageTimeType == (int)EnumCore.PackageTimeType.quy)
                    {
                        model.TotalPrice = ObjNewPackage.NewPrice3M.Value;
                        model.TotalDay = 90;
                    }

                    if (PackageTimeType == (int)EnumCore.PackageTimeType.vinhvien)
                    {
                        model.TotalPrice = ObjNewPackage.ForeverPrice.Value;
                      
                    }
                       
                    string coderandom = DateTime.UtcNow.Ticks.ToString();
                    model.UpgradeToken = coderandom;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("AlertPage", "Extension", 
                        new { AlertString = "Không thể thực hiện nâng cấp với gói cước này", link = "", type = (int)EnumCore.AlertPageType.FullScrenn });
                }

            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("ConfirmUpdate", "PackageManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("AlertPage", "Extension",
                    new { AlertString = "Không thể thực hiện nâng cấp với gói cước này", link = "", type = (int)EnumCore.AlertPageType.FullScrenn });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpgradePackageUser(UserPackageViewModel model)
        {
            try {
                //Nếu user đang online ko phải là user nâng cấp=>failse
                //hoặc user không phải là member thì không nâng cấp
                long CurrentUserId = long.Parse(User.Identity.GetUserId());
                if (CurrentUserId != model.UpgradeUID.Value || !User.IsInRole("Member"))
                {
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = "Không thể thực hiện nâng cấp với gói cước này", link = "" });
                }
                User ObjUser = cms_db.GetObjUserByIdNoAsync(model.UpgradeUID.Value);
                Package ObjNewPackage = cms_db.GetObjPackage(model.PackageId.Value);
                Package ObjOldPackage = new Package();
                if (ObjUser.PackageId.HasValue)
                {
                    ObjOldPackage = cms_db.GetObjPackage(ObjUser.PackageId.Value);
                }
                else
                {
                    //1 là package free
                    ObjOldPackage = cms_db.GetObjPackage(1);
                }


                if (ObjNewPackage.NumDay > ObjOldPackage.NumDay)
                {
                    UserPackage objUserPackage = new UserPackage();

                    objUserPackage.OldPackageID = ObjOldPackage.PackageId;
                    objUserPackage.OldPackageName = ObjOldPackage.PackageName;
                    objUserPackage.CrtdDT = DateTime.Now;
                    objUserPackage.PackageId = ObjNewPackage.PackageId;
                    objUserPackage.PackageName = ObjNewPackage.PackageName;
                    objUserPackage.UpgradeUID = ObjUser.Id;
                    objUserPackage.UpgradeUserName = ObjUser.EMail;
                    objUserPackage.StateId = (int)EnumCore.StateType.cho_duyet;
                    objUserPackage.StateName = "Chờ duyệt";
                    objUserPackage.UpgradeToken = model.UpgradeToken;
                    objUserPackage.Price = model.TotalPrice;
                    if (model.TotalDay.HasValue)
                        objUserPackage.NumDay = model.TotalDay.Value;
                    else
                        objUserPackage.NumDay = 0;

                    cms_db.CreateUserPackage(objUserPackage);

                    ObjUser.AwaitPackageId = ObjNewPackage.PackageId;
                    ObjUser.AwaitPackageName = ObjNewPackage.PackageName;
                 
                    int rs=  await cms_db.UpdateUser(ObjUser);

                    return RedirectToAction("AlertPage", "Extension", 
                        new { AlertString = "Đã thực hiện nâng cấp vui lòng chờ xét duyệt", link = "", type = (int)EnumCore.AlertPageType.FullScrenn });
                }
                else
                {
                    return RedirectToAction("AlertPage", "Extension", 
                        new { AlertString = "Không thể thực hiện nâng cấp với gói cước này", link = "", type = (int)EnumCore.AlertPageType.FullScrenn });
                }
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("UpgradePackageUser", "PackageManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("AlertPage", "Extension", new { AlertString = "Không thể thực hiện nâng cấp với gói cước này", link = "", type = (int)EnumCore.AlertPageType.FullScrenn });
            }
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