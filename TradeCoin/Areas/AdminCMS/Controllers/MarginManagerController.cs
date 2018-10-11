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
using Microsoft.AspNet.SignalR;
using CMSPROJECT.Hubs;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class MarginManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "devuser,AdminUser,CreateMargin")]
        public ActionResult Index(int? page, int? status, int? MarginPackage)
        {
            int pageNum = (page ?? 1);
            IndexMarginManager model = new IndexMarginManager();
            IQueryable<Margin> tmp = cms_db.GetlstMargin();
          
            if (status.HasValue)
            {
                if (status.Value == 0)
                {
                    tmp = tmp.Where(s => s.StateId == (int)EnumCore.StateType.disable);
                }
                else
                {
                    tmp = tmp.Where(s => s.StateId == (int)EnumCore.StateType.enable);
                }

                model.status = status.Value;
            }


            if (MarginPackage.HasValue && MarginPackage.Value != 0)
            {
                if (MarginPackage != 0)
                {
                    foreach (Margin _val in tmp)
                    {

                        List<ContentPackage> lstpackageofticker = cms_db.GetlstObjContentPackage(_val.MarginId, (int)EnumCore.ObjTypeId.margin);
                        if (!lstpackageofticker.Select(s => s.PackageId).Contains(MarginPackage.Value))
                        {
                            tmp = tmp.Where(s => s.MarginId != _val.MarginId);
                        }
                    }

                    model.MarginPackage = MarginPackage.Value;
                }

            }

            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.page = pageNum;
            List<MarginViewModel> prelistmain = new List<MarginViewModel>();
            foreach (Margin _val in tmp)
            {
                MarginViewModel abc = new MarginViewModel(_val);
                abc.lstMarginContentPackage = cms_db.GetlstObjContentPackage(_val.MarginId, (int)EnumCore.ObjTypeId.margin);
                prelistmain.Add(abc);

            }



            model.lstMainMargin = prelistmain.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);



            model.lstMarginStatus = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.status_ticker), "value", "text");
            model.lstMarginPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text");
         
            return View(model);
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,CreateTicker")]
        public ActionResult Create()
        {
            MarginViewModel model = new MarginViewModel();
            model.lstPackage = cms_db.GetObjSelectListPackage();
        
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,CreateTicker")]
        public async Task<ActionResult> Create(MarginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Margin MainModel = model._MainObj;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.StateId = (int)EnumCore.TickerStatusType.dang_chay;
                    MainModel.StateName = "Có hiệu lực";
                    int rs = await cms_db.CreateMarginAsync(MainModel);

                    int SaveTickerPackage = this.SaveMarginPackage(model.lstMarginPackage, MainModel);

                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "Create", MainModel.MarginId, "Margin", "MarginManager", (int)EnumCore.ObjTypeId.margin);


                    var context = GlobalHost.ConnectionManager.GetHubContext<NotifiHub>();
                    context.Clients.All.notificationNewMargin();

                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Create", "MarginManagerController", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
        }




        public ActionResult ObjectNotificationMargin()
        {
            try
            {
                BoxMargin MainModel = new BoxMargin();
                MainModel.FreeObject = cms_db.GetMarginByPackageLinq((int)EnumCore.Package.free);
                MainModel.DemObject = cms_db.GetMarginByPackageLinq((int)EnumCore.Package.dem);
                MainModel.VangObject = cms_db.GetMarginByPackageLinq((int)EnumCore.Package.vang);
                MainModel.KCObject = cms_db.GetMarginByPackageLinq((int)EnumCore.Package.kimcuong);

                return Json(MainModel, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateMargin")]
        public ActionResult Update(int? id)
        {
            if (id == null)
                id = 1;
            Margin _obj = cms_db.GetObjMargin(id.Value);
            MarginViewModel model = new MarginViewModel(_obj);

            if (model.StateId == (int)EnumCore.StateType.enable)
            {
                model.MarginStatus = true;
            }
            else
            {
                model.MarginStatus = false;
            }

            if ((model.CrtdUserId == long.Parse(User.Identity.GetUserId()) && User.IsInRole("Mod")) || User.IsInRole("AdminUser") || User.IsInRole("devuser"))
            {
                model.lstPackage = cms_db.GetObjSelectListPackage();
                model.lstMarginPackage = cms_db.GetlstTickerPackage(model.MarginId, (int)EnumCore.ObjTypeId.margin);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,UpdateTicker")]
        public async Task<ActionResult> Update(MarginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                        Margin MainModel = model._MainObj;
                    if (model.MarginStatus == true)
                    {
                        model.StateId = (int)EnumCore.StateType.enable;
                        model.StateName = "Có hiệu lực";
                    }
                    else
                    {
                        model.StateId = (int)EnumCore.StateType.disable;
                        model.StateName = "Hết hiệu lực";
                    }

                    int rs = await cms_db.UpdateMargin(MainModel);

                        int SaveTickerPackage = this.SaveMarginPackage(model.lstMarginPackage, MainModel);
                        int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                            (int)EnumCore.ActionType.Update, "Update", MainModel.MarginId, "Margin", "MarginManager", (int)EnumCore.ObjTypeId.margin);

                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Update", "MarginManagerController", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
        }



        private int SaveMarginPackage(long[] model, Margin Margin)
        {
            try
            {
                int dl = cms_db.DeleteContentPackage(Margin.MarginId, (int)EnumCore.ObjTypeId.margin);
                foreach (int _val in model)
                {
                    ContentPackage tmp = new ContentPackage();
                    tmp.ContentId = Margin.MarginId;
                    tmp.ContentName = "Margin";
                    tmp.ContentType = (int)EnumCore.ObjTypeId.margin;
                    tmp.PackageId = _val;
                    tmp.PackageName = cms_db.GetPackageName(_val);

                    cms_db.CreateContentPackage(tmp);
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("SaveTickerPackage", "MarginManagerController", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return (int)EnumCore.Result.action_false;
            }
        }



        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                Margin MainModel = cms_db.GetObjMargin(id);
                if (MainModel != null)
                {
                   
                    int dl = cms_db.DeleteContentPackage(id, (int)EnumCore.ObjTypeId.margin);
                    int result = await cms_db.DeleteMargin(MainModel);
                    int ach = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                                         (int)EnumCore.ActionType.Delete, "Delete", MainModel.MarginId,"Margin", "MarginManager", (int)EnumCore.ObjTypeId.margin);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Delete", "MarginManager", e.ToString());
                return RedirectToAction("Index");
            }
        }




    }
}