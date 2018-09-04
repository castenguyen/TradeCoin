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
        public ActionResult Index(int? page, int? EmailStatus, string EmailName, string Datetime)
        {
            try
            {
                int pageNum = (page ?? 1);
                EmailSupportIndexViewModel model = new EmailSupportIndexViewModel();
                long IdUser = long.Parse(User.Identity.GetUserId());

                //Nếu user đang dang nhập là admin

                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {

                    IQueryable<EmailSupport> tmp = cms_db.GetlstEmailSupport().Where(s => s.StateId != (int)EnumCore.EmailStatus.da_xoa && s.ParentId == null);
                    if (EmailStatus.HasValue)
                    {
                        if (EmailStatus.Value == (int)EnumCore.EmailStatus.cho_ho_tro)
                        {
                            tmp = tmp.Where(s => s.DestinationId.Value < 0);
                        }
                        if (EmailStatus.Value == (int)EnumCore.EmailStatus.da_ho_tro)
                        {
                            tmp = tmp.Where(s => s.DestinationId.Value > 0);
                        }
                    }

                    if (!String.IsNullOrEmpty(EmailName))
                    {
                        tmp = tmp.Where(s => s.Subject.ToLower().Contains(EmailName.ToLower()));
                        model.EmailName = EmailName;
                    }

                    if (!String.IsNullOrEmpty(Datetime))
                    {
                        model.Datetime = Datetime;
                        model.StartDT = this.SpritDateTime(model.Datetime)[0];
                        model.EndDT = this.SpritDateTime(model.Datetime)[1];
                        tmp = tmp.Where(s => s.CrtdDT > model.StartDT && s.CrtdDT < model.EndDT);
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
                        if (EmailStatus.Value == (int)EnumCore.EmailStatus.cho_ho_tro)
                        {
                            tmp = tmp.Where(s => s.DestinationId.Value < 0);
                        }
                        if (EmailStatus.Value == (int)EnumCore.EmailStatus.da_ho_tro)
                        {
                            tmp = tmp.Where(s => s.DestinationId.Value > 0);
                        }
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
                cms_db.AddToExceptionLog("Index", "EmailManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                string AlertString = "Nội dung xem không khả dụng";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });

            }

        }


        private DateTime[] SpritDateTime(string datetime)
        {
            string[] words = datetime.Split('-');
            DateTime[] model = new DateTime[] { Convert.ToDateTime(words[0]), Convert.ToDateTime(words[1]) };
            return model;
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
                if (!User.IsInRole("Member"))
                {
                    string AlertString = "Không thể tạo thư hỗ trợ do bạn không phải là thành viên";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
                }
                EmailSupportViewModel model = new EmailSupportViewModel();
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
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod,Member")]
        public async Task<ActionResult> CreateNewEmail(EmailSupportViewModel model)
        {

            try
            {
                if (!User.IsInRole("Member"))
                {
                    string AlertString = "Không thể tạo thư hỗ trợ do bạn không phải là thành viên";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
                }
                long UID = long.Parse(User.Identity.GetUserId());
                EmailSupport newObject = model._MainObj;
                newObject.EmailName = model.Subject;
                newObject.CrtdDT = DateTime.Now;
                newObject.CrtdUserId = long.Parse(User.Identity.GetUserId());
                newObject.CrtdUserName = User.Identity.Name;
                newObject.StateId = (int)EnumCore.EmailStatus.cho_ho_tro;
                newObject.StateName = "Chờ hỗ trợ";
                newObject.DestinationId = -1;
                int rs = cms_db.CreateEmailSupport(newObject);

                //member tạo ra chắc chắn là member đã xems
                //tạo d8ã xem cho email mới tạo
                int rsv = this.CreateEmailView(newObject, UID);
                int ach = await cms_db.CreateUserHistory(UID, Request.ServerVariables["REMOTE_ADDR"],
                             (int)EnumCore.ActionType.Update, "CreateNewEmail", newObject.EmailId, newObject.Subject, "EmailSupport", (int)EnumCore.ObjTypeId.emailsupport);

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

                //tạo đã xem cho email chính
                int rs = this.CreateEmailView(model._MainObj, UID);

                //tạo đã xem cho tấ cả email con
                foreach (EmailSupport _item in model.lstChild)
                {
                    int rsc = this.CreateEmailView(_item, UID);
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

        //tạo đã xem cho email
        private int CreateEmailView(EmailSupport item, long UID)
        {
            ContentView ck = cms_db.GetObjContentView(item.EmailId, (int)EnumCore.ObjTypeId.emailsupport, UID);
            if (ck == null)
            {
                ContentView tmp = new ContentView();
                tmp.UserId = UID;
                tmp.UserName = User.Identity.GetUserName();
                tmp.ContentId = item.EmailId;
                tmp.ContentType = (int)EnumCore.ObjTypeId.emailsupport;
                tmp.ContentName = item.Subject;
                cms_db.CreateContentView(tmp);
            }
            return 1;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod")]
        public async Task<ActionResult> ReplyEmail(EmailSupportViewModel model)
        {
            try
            {
                string AlertString = "";

                ///phải là mod hay admin mới có thể tạo thư trả lời
                if (!User.IsInRole("AdminUser") && !User.IsInRole("devuser") && !User.IsInRole("supperadmin") && !User.IsInRole("Mod"))
                {
                    AlertString = "Tạo mới thư trả lời thư không thành công do bạn không phải là mod";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
                }

                long ModUserId = long.Parse(User.Identity.GetUserId());
                User ModUser = await cms_db.GetObjUserById(ModUserId);

                EmailSupport SubjectEmail = cms_db.GetObjEmailSupport(model.EmailId);

                if (SubjectEmail.DestinationId != -1)
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
                ReplyEmailSuport.StateId = (int)EnumCore.EmailStatus.da_ho_tro;
                ReplyEmailSuport.StateName = "Đã hỗ trợ";

                int rs = cms_db.CreateEmailSupport(ReplyEmailSuport);
                //mod tạo ra mail này chắc nchan moad dã xem mình trả lời
                int rsv = this.CreateEmailView(ReplyEmailSuport, ModUserId);
                int ach = await cms_db.CreateUserHistory(ModUserId, Request.ServerVariables["REMOTE_ADDR"],
                            (int)EnumCore.ActionType.Update, "ReplyEmail", ReplyEmailSuport.EmailId, ReplyEmailSuport.Subject, "EmailSupport", (int)EnumCore.ObjTypeId.emailsupport);




                AlertString = "Tạo mới thư trả lời thư thành công";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreateNewEmail", "EmailManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                string AlertString = "Tạo mới thư trả lời thư không thành công do có lỗi khi tạo";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,Mod,Member")]
        public async Task<ActionResult> ClearEmail(long[] lstClear)
        {
            try
            {
                string AlertString = "";
                ///phải là mod hay admin mới có thể tạo thư trả lời
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    foreach(long item in lstClear)
                    {

                        EmailSupport objF = cms_db.GetObjEmailSupport(item);
                        List<EmailSupport> lstChild = cms_db.GetlstEmailSupport().Where(s=>s.ParentId== item).ToList();
                        foreach (EmailSupport childItem in lstChild)
                        {
                           await cms_db.ClearEmailSupport(childItem,(int)EnumCore.EmailClearType.xoa_mail_mod);
                        }
                        await cms_db.ClearEmailSupport(objF, (int)EnumCore.EmailClearType.xoa_mail_mod);
                    }


                    AlertString = "Đã xoá thư thành công";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
                }
                else //User.IsInRole("Member")
                {
                    foreach (long item in lstClear)
                    {

                        EmailSupport objF = cms_db.GetObjEmailSupport(item);
                        List<EmailSupport> lstChild = cms_db.GetlstEmailSupport().Where(s => s.ParentId == item).ToList();
                        foreach (EmailSupport childItem in lstChild)
                        {
                            await cms_db.ClearEmailSupport(childItem, (int)EnumCore.EmailClearType.xoa_mail_member);
                        }
                        await cms_db.ClearEmailSupport(objF, (int)EnumCore.EmailClearType.xoa_mail_mod);
                    }

                    AlertString = "Đã xoá thư thành công";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });
                }
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("ClearEmail", "EmailManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                string AlertString = "Có lỗi khi xoá thư vui lòng thử lại sau";
                return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.FullScrenn });

            }

        }

    }
}