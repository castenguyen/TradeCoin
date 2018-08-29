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
using ClosedXML.Excel;
using System.Data;
using System.IO;
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

        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Create()
        {
            return View();
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
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
        public ActionResult ConfirmUpgrade(long UserId, long PackageId, int PackageTimeType)
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
            try
            {
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

                    int rs = await cms_db.UpdateUser(ObjUser);

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
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Update(int? id)
        {
            if (id == null)
                id = 1;
            Package _obj = cms_db.GetObjPackage(id.Value);
            PackageViewModel model = new PackageViewModel(_obj);
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
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


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult TrackingFinance(int? page, int? Packageid, string FillterName, string Datetime)
        {
            int pageNum = (page ?? 1);
            TrackingFinanceViewModel model = new TrackingFinanceViewModel();
            IQueryable<UserPackage> tmp = cms_db.GetlstUserPackageIquery().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep);
            if (!String.IsNullOrEmpty(FillterName))
            {
                tmp = tmp.Where(s => s.UpgradeUserName == FillterName);
                model.FillterName = FillterName;
            }
            if (Packageid.HasValue)
            {
                tmp = tmp.Where(s => s.PackageId == Packageid);
                model.Packageid = Packageid.Value;

            }
            if (!String.IsNullOrEmpty(Datetime))
            {
                model.Datetime = Datetime;
                model.StartDT = this.SpritDateTime(model.Datetime)[0];
                model.EndDT = this.SpritDateTime(model.Datetime)[1];
                tmp = tmp.Where(s => s.CrtdDT > model.StartDT && s.CrtdDT < model.EndDT);
            }

            model.lstMainUserPackage = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text");
            return View(model);

        }


        [HttpGet]
        public FileResult Export(int? page, int? Packageid, string FillterName, string Datetime)
        {

            try
            {
                int pageNum = (page ?? 1);
                TrackingFinanceViewModel model = new TrackingFinanceViewModel();
                IQueryable<UserPackage> tmp = cms_db.GetlstUserPackageIquery().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep);
                if (!String.IsNullOrEmpty(FillterName))
                {
                    tmp = tmp.Where(s => s.UpgradeUserName == FillterName);
                }
                if (Packageid.HasValue)
                {
                    if (Packageid.Value > 0)
                    {
                        tmp = tmp.Where(s => s.PackageId == Packageid);
                    }
                }
                if (!String.IsNullOrEmpty(Datetime))
                {
                    model.Datetime = Datetime;
                    model.StartDT = this.SpritDateTime(model.Datetime)[0];
                    model.EndDT = this.SpritDateTime(model.Datetime)[1];
                    tmp = tmp.Where(s => s.CrtdDT > model.StartDT && s.CrtdDT < model.EndDT);
                }
                List<UserPackage> dataexport = new List<UserPackage>();
                dataexport = tmp.OrderByDescending(c => c.CrtdDT).Skip((pageNum - 1) * 50).Take(50).ToList();
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Tên"),
                                            new DataColumn("Ngày nâng cấp"),
                                            new DataColumn("Gói nâng cấp"),
                                            new DataColumn("Tiền"),
                                             new DataColumn("Lịch sử")});

                double sum = 0;
                foreach (UserPackage item in dataexport)
                {
                    dt.Rows.Add(item.UpgradeUserName, item.CrtdDT, item.PackageName, item.Price, item.OldPackageName + "->" + item.PackageName);
                    sum = sum + item.Price.Value;
                }
                dt.Rows.Add("", "", "Tổng tiền", sum, "");


                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ncoin_history.xlsx");
                    }
                }
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Export", "PackageManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return null;
            }


        }




        private DateTime[] SpritDateTime(string datetime)
        {
            string[] words = datetime.Split('-');
            DateTime[] model = new DateTime[] { Convert.ToDateTime(words[0]), Convert.ToDateTime(words[1]) };
            return model;
        }
    }
}