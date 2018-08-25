using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DataModel.Extension;
using System.Threading.Tasks;
using System.Collections;
using PagedList;
using Microsoft.AspNet.SignalR;
using CMSPROJECT.Hubs;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class EmailManagerController : CoreBackEnd
    {
        // GET: AdminCMS/EmailManager
        public ActionResult Index(int? page, int? EmailStatus)
        {
            int pageNum = (page ?? 1);
            EmailSupportIndexViewModel model = new EmailSupportIndexViewModel();
            model.lstMod = cms_db.GetlstUser().Where(s => s.LstRole.Any(r => r.Name == "Mod")).ToList();
            long IdUser = long.Parse(User.Identity.GetUserId());
            if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
            {
                IQueryable<EmailSupport> tmp = cms_db.GetlstEmailSupport().Where(s => s.EmailTypeId
                         != (int)EnumCore.EmailStatus.da_xoa && (s.CrtdUserId != IdUser || s.DestinationId != IdUser));
                pageNum = 1;
                model.pageNum = pageNum;
                model.lstEmailSupport = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
                return View(model);
            }
            else
            {
                IQueryable<EmailSupport> tmp = cms_db.GetlstEmailSupport().Where(s => s.EmailTypeId
                         != (int)EnumCore.EmailStatus.da_xoa && (s.CrtdUserId == IdUser || s.DestinationId == IdUser));
                pageNum = 1;
                model.pageNum = pageNum;
                model.lstEmailSupport = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
                return View(model);
            }
        }
        public ActionResult GridListBalancesStatement()
        {
            try
            {
                int? page = null;
                int pageNum = (page ?? 1);
                EmailSupportIndexViewModel model = new EmailSupportIndexViewModel();
                model.lstMod = cms_db.GetlstUser().Where(s => s.LstRole.Any(r => r.Name == "Mod")).ToList();
                long IdUser = long.Parse(User.Identity.GetUserId());
                IQueryable<EmailSupport> tmp = cms_db.GetlstEmailSupport().Where(s => s.CrtdUserId != IdUser);

                pageNum = 1;
                model.lstEmailSupport = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
                return Json(model.lstEmailSupport, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<ActionResult> CreateNewEmail(long? ModId)
        {
            EmailSupportViewModel model = new EmailSupportViewModel();
            model.lstMod = cms_db.GetlstUser().Where(s => s.LstRole.Any(r => r.Name == "Mod")).ToList();
            if (ModId.HasValue)
            {
                User modName =await cms_db.GetObjUserById(ModId.Value);
                model.DestinationId = modName.Id;
                model.DestinationName = modName.FullName;
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewEmail(EmailSupportViewModel model)
        {

            try
            {
                EmailSupport newObject = model._MainObj;
                
                newObject.EmailName = model.Subject;
                newObject.CrtdDT = DateTime.Now;
                newObject.CrtdUserId = long.Parse(User.Identity.GetUserId());
                newObject.CrtdUserName = User.Identity.Name;
                newObject.StateId = (int)EnumCore.EmailStatus.cho_hoi_am;
                newObject.StateName = "Chờ phản hồi";
                int rs = cms_db.CreateEmailSupport(newObject);
                var context = GlobalHost.ConnectionManager.GetHubContext<NotifiHub>();
                context.Clients.All.notificationNewEmail();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreateNewEmail", "EmailManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
          
        }

        public async Task<ActionResult> DetailEmail(long? emailId)
        {
            EmailSupportViewModel model = new EmailSupportViewModel();
            model.lstMod = cms_db.GetlstUser().Where(s => s.LstRole.Any(r => r.Name == "Mod")).ToList();
            model._MainObj = cms_db.GetObjEmailSupport(emailId.Value);
            model.lstChild = cms_db.GetlstEmailSupport().Where(s => s.ParentId == emailId 
                    && s.StateId!=(int)EnumCore.EmailStatus.da_xoa).OrderByDescending(s=>s.CrtdDT).ToList();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyEmail(EmailSupportViewModel model)
        {

            try
            {
                EmailSupport newObject = model._MainObj;
                newObject.EmailName = model.Subject;
                newObject.CrtdDT = DateTime.Now;
                newObject.CrtdUserId = long.Parse(User.Identity.GetUserId());
                newObject.CrtdUserName = User.Identity.Name;
                newObject.StateId = (int)EnumCore.EmailStatus.cho_hoi_am;
                newObject.StateName = "Chờ phản hồi";
                int rs = cms_db.CreateEmailSupport(newObject);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreateNewEmail", "EmailManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }

        }



    }
}