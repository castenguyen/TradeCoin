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
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod,Member")]
        public ActionResult Index(int? page, int? EmailStatus)
        {
            try {
                int pageNum = (page ?? 1);
                EmailSupportIndexViewModel model = new EmailSupportIndexViewModel();
                long IdUser = long.Parse(User.Identity.GetUserId());

                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {

                    IQueryable<EmailSupport> tmp = cms_db.GetlstEmailSupport().Where(s => s.StateId != (int)EnumCore.EmailStatus.da_xoa && s.ParentId == null);
                    if (EmailStatus.HasValue)
                    {
                        tmp = tmp.Where(s => s.StateId == EmailStatus.Value);

                    }


                    //lấy danh sách email daxem cua user
                    model.lstViewed = cms_db.GetlstContentView().Where(s => s.UserId == IdUser
                            && s.ContentType == (int)EnumCore.ObjTypeId.emailsupport).Select(s => s.ContentId).ToArray();
                    pageNum = 1;
                    model.pageNum = pageNum;
                    model.lstEmailSupport = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
                    return View(model);
                }
                else
                {

                    IQueryable<EmailSupport> tmp = cms_db.GetlstEmailSupport().Where(s => s.StateId2 != (int)EnumCore.EmailStatus.da_xoa && s.ParentId == null);
                    if (EmailStatus.HasValue)
                    {
                        tmp = tmp.Where(s => s.StateId2 == EmailStatus.Value);

                    }
                    //lấy danh sách email daxem cua user
                    model.lstViewed = cms_db.GetlstContentView().Where(s => s.UserId == IdUser
                           && s.ContentType == (int)EnumCore.ObjTypeId.emailsupport).Select(s => s.ContentId).ToArray();
                    pageNum = 1;
                    model.pageNum = pageNum;
                    model.lstEmailSupport = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
                    return View(model);
                }


            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreateNewEmail", "EmailManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });

            }
         
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod,Member")]
        public ActionResult GridListBalancesStatement()
        {
            try
            {
                long IdUser = long.Parse(User.Identity.GetUserId());
                int? page = null;
                int pageNum = (page ?? 1);
                EmailSupportIndexViewModel model = new EmailSupportIndexViewModel();
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    IQueryable<EmailSupport> tmp = cms_db.GetlstEmailSupport().Where(s => s.StateId != (int)EnumCore.EmailStatus.da_xoa);
                    pageNum = 1;
                    model.lstEmailSupport = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
                    return Json(model.lstEmailSupport, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IQueryable<EmailSupport> tmp = cms_db.GetlstEmailSupport().Where(s => s.StateId != (int)EnumCore.EmailStatus.da_xoa);
                    pageNum = 1;
                    model.lstEmailSupport = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
                    return Json(model.lstEmailSupport, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod,Member")]
        public async Task<ActionResult> CreateNewEmail()
        {
            try
            {
                EmailSupportViewModel model = new EmailSupportViewModel();
                return View(model);
            }
          
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreateNewEmail", "EmailManager", e.ToString());
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int) EnumCore.AlertPageType.FullScrenn });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod,Member")]
        public ActionResult CreateNewEmail(EmailSupportViewModel model)
        {

            try
            {
                long UID = long.Parse(User.Identity.GetUserId());
                EmailSupport newObject = model._MainObj;
                newObject.EmailName = model.Subject;
                newObject.CrtdDT = DateTime.Now;
                newObject.CrtdUserId = long.Parse(User.Identity.GetUserId());
                newObject.CrtdUserName = User.Identity.Name;
                newObject.StateId = (int)EnumCore.EmailStatus.chua_xem;
                newObject.StateName = "Chờ phản hồi";
                int rs = cms_db.CreateEmailSupport(newObject);

                //member tạo ra chắc chn81 là member đã xems
                ContentView ck = cms_db.GetObjContentView(newObject.EmailId, (int)EnumCore.ObjTypeId.emailsupport, UID);
                if (ck == null)
                {
                    ContentView tmp = new ContentView();
                    tmp.UserId = UID;
                    tmp.UserName = User.Identity.GetUserName();
                    tmp.ContentId = newObject.EmailId;
                    tmp.ContentType = (int)EnumCore.ObjTypeId.emailsupport;
                    tmp.ContentName = newObject.Subject;
                    cms_db.CreateContentView(tmp);
                }


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

        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod,Member")]
        public async Task<ActionResult> DetailEmail(long? emailId)
        {
            try
            {
                long UID = long.Parse(User.Identity.GetUserId());
                EmailSupportViewModel model = new EmailSupportViewModel();
                model._MainObj = cms_db.GetObjEmailSupport(emailId.Value);
                model.lstChild = cms_db.GetlstEmailSupport().Where(s => s.ParentId == emailId
                        && s.StateId != (int)EnumCore.EmailStatus.da_xoa).OrderByDescending(s => s.CrtdDT).ToList();

                ContentView ck = cms_db.GetObjContentView(model.EmailId, (int)EnumCore.ObjTypeId.emailsupport, UID);
                if (ck == null)
                {
                    ContentView tmp = new ContentView();
                    tmp.UserId = UID;
                    tmp.UserName = User.Identity.GetUserName();
                    tmp.ContentId = model.EmailId;
                    tmp.ContentType = (int)EnumCore.ObjTypeId.emailsupport;
                    tmp.ContentName = model.Subject;
                    cms_db.CreateContentView(tmp);
                }
                return View(model);
            }
                catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreateNewEmail", "EmailManager", e.ToString());
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod")]
        public async Task<ActionResult> ReplyEmail(EmailSupportViewModel model)
        {
            try
            {

                string AlertString = "";

              long ModUserId = long.Parse(User.Identity.GetUserId());
                User ModUser = await cms_db.GetObjUserById(ModUserId); 
               
                EmailSupport SubjectEmail = cms_db.GetObjEmailSupport(model.EmailId);

                if (SubjectEmail.DestinationId != null)
                {
                     AlertString = "Tạo mới thư trả lời thư không thành công đã có mod hỗ trợ";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
                }
                SubjectEmail.DestinationId = ModUser.Id;
                SubjectEmail.DestinationName = ModUser.FullName;
                ///cập nhật lại người nhận là mod cho email hỏi
                await cms_db.UpdateEmailSupport(SubjectEmail);

                User MemberUser = await cms_db.GetObjUserById(SubjectEmail.CrtdUserId.Value);
                EmailSupport ReplyEmailSuport = new EmailSupport();

                //vì day là email trả lời mod trả lời nên DestinationId là id của member tạo ra thư 
                ReplyEmailSuport.DestinationName = MemberUser.FullName;
                ReplyEmailSuport.DestinationId = MemberUser.Id;

                //vì day là email trả lời mod trả lời nên ParentId là id của thư  chủ 
                ReplyEmailSuport.ParentId = SubjectEmail.EmailId;
                ReplyEmailSuport.ParentName = SubjectEmail.Subject;
                ReplyEmailSuport.Content = model.Content;
                ReplyEmailSuport.EmailName = SubjectEmail.Subject;
                ReplyEmailSuport.CrtdDT = DateTime.Now;
                ReplyEmailSuport.CrtdUserId = ModUserId;
                ReplyEmailSuport.CrtdUserName = ModUser.FullName;
                ReplyEmailSuport.StateId = (int)EnumCore.EmailStatus.da_xem;
                ReplyEmailSuport.StateName = "Đã Xem";

                int rs = cms_db.CreateEmailSupport(ReplyEmailSuport);


                //mod tạo ra mail này chắc nchan moad dã xem mình trả lời

                ContentView ck = cms_db.GetObjContentView(ReplyEmailSuport.EmailId, (int)EnumCore.ObjTypeId.emailsupport, ModUser.Id);
                if (ck == null)
                {
                    ContentView tmp = new ContentView();
                    tmp.UserId = ModUser.Id;
                    tmp.UserName = ModUser.FullName;
                    tmp.ContentId = ReplyEmailSuport.EmailId;
                    tmp.ContentType = (int)EnumCore.ObjTypeId.emailsupport;
                    tmp.ContentName = model.Subject;
                    cms_db.CreateContentView(tmp);
                }

                 AlertString = "Tạo mới thư trả lời thư thành công";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreateNewEmail", "EmailManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                string AlertString = "Tạo mới thư trả lời thư không thành công";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
               
            }

        }



    }
}