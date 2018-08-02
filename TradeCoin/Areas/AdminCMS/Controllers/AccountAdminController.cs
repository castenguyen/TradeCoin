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
//using MongoData.Models;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{

    public class AccountAdminController : CoreBackEnd
    {
        #region constants

        private const string XsrfKey = "XsrfId";

        /// <summary>
        /// có 2 kieu đăng nhập
        /// 1 : Đăng nhập bằng usernam và password
        /// 2: Đăng nhập bằng mã token được gửi về email đăng ký
        /// ==>khi hệ thống đăng nhập bằng mã token thì LoginWithCode bật lên = 1 ngược lai là 0
        /// </summary>
        private const int LoginWithCode = 1;

        #endregion

        #region member vars

        private MyUserManager _userManager;

        #endregion

        #region constructors and destructors

        public AccountAdminController()
        {
        }

        public AccountAdminController(MyUserManager userManager)
        {
            UserManager = userManager;
        }

        #endregion

        #region enums

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        #endregion

        #region properties

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

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion

        #region External Method
        //
        // POST: /Account/LinkLogin
        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        [AdminAuthorize]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction(
                    "Manage",
                    new
                    {
                        Message = ManageMessageId.Error
                    });
            }
            var result = await UserManager.AddLoginAsync(long.Parse(User.Identity.GetUserId()), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction(
                "Manage",
                new
                {
                    Message = ManageMessageId.Error
                });
        }

        //
        //
        // POST: /Account/Disassociate
        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            var result = await UserManager.RemoveLoginAsync(long.Parse(User.Identity.GetUserId()), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(long.Parse(User.Identity.GetUserId()));
                await SignInAsync(user, false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction(
                "Manage",
                new
                {
                    Message = message
                });
        }

        //
        // GET: /Account/Manage

        //
        // POST: /Account/ExternalLogin
        [AdminAuthorize]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            //Request a redirect to the external login provider
            return new ChallengeResult(
                provider,
                Url.Action(
                    "ExternalLoginCallback",
                    "AccountAdmin",
                    new
                    {
                        ReturnUrl = returnUrl
                    }));
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AdminAuthorize]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, false);
                return RedirectToLocal(returnUrl);
            }
            // If the user does not have an account, then prompt the user to create an account
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            return View(
                "ExternalLoginConfirmation",
                new ExternalLoginConfirmationViewModel
                {
                    Email = loginInfo.Email
                });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new MyUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #endregion

        #region Register-Login-Reset

        [AllowAnonymous]
        public ActionResult Register()
        {
            Config tmp2 = ExtFunction.Config();
            ViewBag.SiteName = tmp2.site_name;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new MyUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FullName = model.FullName,

                    };
                    var result = UserManager.Create(user, model.Password);
                    string pass = UserManager.PasswordHasher.HashPassword("asdadasdasdsa");
                    UserManager.SetLockoutEnabled(user.Id, true);
                    UserManager.Update(user);
                    if (result.Succeeded)
                    {
                        string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "AccountAdmin", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        EmailService email = new EmailService();
                        IdentityMessage message = new IdentityMessage();
                        message.Body = string.Format("Để xác thực email vui lòng nhấp vào liên kết: <a href='{0}'>Tại đây</a>", callbackUrl);
                        message.Subject = "Xác thực tài khoản";
                        message.Destination = user.Email;
                        await email.SendAsync(message, EmailService.EmailAdmin, EmailService.EmailAdmin,
                            EmailService.EmailAdminPassword, EmailService.EmailAdminSMTP, EmailService.Portmail, true);

                        return RedirectToAction("Login", "AccountAdmin");
                    }
                    AddErrors(result);
                }
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("function Register", "AccountAdmin", e.ToString());
                return null;
            }

        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            if (LoginWithCode == 1)
            {
                ViewBag.ReturnUrl = returnUrl;
                Config tmp2 = ExtFunction.Config();
                ViewBag.SiteName = tmp2.site_name;
                return View("LoginWithCode");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                Config tmp2 = ExtFunction.Config();
                ViewBag.SiteName = tmp2.site_name;
                return View();
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            if (LoginWithCode == 1)
            {
                var user = UserManager.Find(model.Email, "123456");
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(model);
                }
                else
                {
                    if (UserManager.GetLockoutEnabled(user.Id) == true)
                    {
                        string AlertString = "Tài khoản tạm khoá vui lòng liên hệ quản trị viên";
                        return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString });
                    }
                    if (UserManager.IsEmailConfirmed(user.Id) == false)
                    {
                        string AlertString = "Tài khoản chưa kích hoạt vui lòng kiểm tra mail và kích hoạt";
                        return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString });
                    }
                    await SignInAsync(user, model.RememberMe);
                    int ach = await cms_db.CreateUserHistory(user.Id, Request.ServerVariables["REMOTE_ADDR"],
                                              (int)EnumCore.ActionType.Login, "Login", 0, model.Email, "User", (int)EnumCore.ObjTypeId.nguoi_dung);
                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }

            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = UserManager.Find(model.Email, model.Password);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                        return View(model);
                    }
                    else
                    {
                        if (UserManager.GetLockoutEnabled(user.Id) == true)
                        {
                            string AlertString = "Tài khoản tạm khoá vui lòng liên hệ quản trị viên";
                            return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString });
                        }

                        if (UserManager.IsEmailConfirmed(user.Id) == false)
                        {
                            string AlertString = "Tài khoản chưa kích hoạt vui lòng kiểm tra mail và kích hoạt";
                            return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString });
                        }
                        await SignInAsync(user, model.RememberMe);
                        int ach = await cms_db.CreateUserHistory(user.Id, Request.ServerVariables["REMOTE_ADDR"],
                                                  (int)EnumCore.ActionType.Login, "Login", 0, model.Email, "User", (int)EnumCore.ObjTypeId.nguoi_dung);

                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            return Redirect(returnUrl);
                        }
                    }
                }
                else
                {
                    return View(model);
                }
            }
        }




        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            Config tmp2 = ExtFunction.Config();
            ViewBag.SiteName = tmp2.site_name;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByNameAsync(model.Email);
                    if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    {
                        ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                        return View();
                    }
                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "AccountAdmin", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    EmailService email = new EmailService();
                    IdentityMessage message = new IdentityMessage();
                    message.Body = string.Format("Vui lòng nhấp vào đường link: <a href='{0}'>Tại đây</a> để lấy lại mật khẩu", callbackUrl);
                    message.Subject = "Lấy lại mật khẩu";
                    message.Destination = user.Email;
                    await email.SendAsync(message, EmailService.EmailAdmin, EmailService.EmailAdmin,
                        EmailService.EmailAdminPassword, EmailService.EmailAdminSMTP, EmailService.Portmail, true);

                    return RedirectToAction("ForgotPassword", "AccountAdmin");
                }
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("function ForgotPassword", "AccountAdmin", e.ToString());
                return View();

            }

        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(long userId, string code)
        {
            if (code == null)
            {
                return View("Error");
            }

            var result = UserManager.ConfirmEmail(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            AddErrors(result);
            return View();
        }

        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Login");
        }

  


        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            Config tmp2 = ExtFunction.Config();
            ViewBag.SiteName = tmp2.site_name;
            if (code == null)
            {
                return View("Error");
            }
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Code = code;
            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "AccountAdmin");
                }
                AddErrors(result);
                return View();
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region methods


        [AdminAuthorize]
        public async Task<ActionResult> Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess
                ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess
                    ? "Your password has been set."
                    : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed." : message == ManageMessageId.Error ? "An error has occurred." : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            User _ObjUser = await cms_db.GetObjUserById(long.Parse(User.Identity.GetUserId()));
            ManageUserViewModel model = new ManageUserViewModel(_ObjUser);
            model.Myhistory = cms_db.GetLstUserHistByUserId(long.Parse(User.Identity.GetUserId()), 10);
            return View(model);
        }

        //
        // POST: /Account/Manage
        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            var hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    User _ObjUser = await cms_db.GetObjUserById(long.Parse(User.Identity.GetUserId()));
                    // _ObjUser.FullName = model.FullName;
                    int _rs = await cms_db.UpdateUser(_ObjUser);
                    var result = await UserManager.ChangePasswordAsync(long.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        int ach = await cms_db.CreateUserHistory(_ObjUser.Id, Request.ServerVariables["REMOTE_ADDR"],
                                         (int)EnumCore.ActionType.Login, "ChangePasswor", 0, model.Email, "User", (int)EnumCore.ObjTypeId.nguoi_dung);
                        var user = await UserManager.FindByIdAsync(long.Parse(User.Identity.GetUserId()));
                        await SignInAsync(user, false);
                        return RedirectToAction(
                            "Manage",
                            new
                            {
                                Message = ManageMessageId.ChangePasswordSuccess
                            });
                    }
                    AddErrors(result);
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                var state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    var result = await UserManager.AddPasswordAsync(long.Parse(User.Identity.GetUserId()), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(
                            "Manage",
                            new
                            {
                                Message = ManageMessageId.SetPasswordSuccess
                            });
                    }
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(long.Parse(User.Identity.GetUserId()));
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,ApproveUser")]
        public async Task<ActionResult> ChangeState(long id, bool state)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                await UserManager.SetLockoutEnabledAsync(id, state);
                await UserManager.UpdateAsync(user);
            }
            return RedirectToAction("ManagerUserRole", "AccountAdmin", new { id = id });

        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(long.Parse(User.Identity.GetUserId()));
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        private async Task SignInAsync(MyUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(
                new AuthenticationProperties
                {
                    IsPersistent = isPersistent
                },
                await user.GenerateUserIdentityAsync(UserManager));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Profile
        [AdminAuthorize]
        public ActionResult Profile()
        {
            long CurrentUid = long.Parse(User.Identity.GetUserId());
            ProfileViewModel model = new ProfileViewModel();
            model._ModelObj = cms_db.GetObjUserByIdNoAsync(CurrentUid);
            model.GenderList = new SelectList(cms_db.GetCatagoryForSelectList((int)EnumCore.ClassificationScheme.GenderType), "ClassificationId", "ClassificationNM");
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.nguoi_dung
                                                        && s.ContentObjId == CurrentUid).FirstOrDefault();
            if (CurrentMediaId != null)
                model.ImgUrl = CurrentMediaId.FullURL;
            return View(model);
        }

        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(ProfileViewModel model, HttpPostedFileBase Imgfiles)
        {
            User mainobj = cms_db.GetObjUserByIdNoAsync(model.Id);
            mainobj.Id = model.Id;
            mainobj.BankAccountHolder = model.BankAccountHolder;
            mainobj.BankAccountNbr = model.BankAccountNbr;
            mainobj.BankAdress = model.BankAdress;
            mainobj.BankName = model.BankName;
            mainobj.BirthDay = model.BirthDay;
            mainobj.CMND = model.CMND;
            mainobj.GenderId = model.GenderId;
            mainobj.GenderName = model.GenderName;
            mainobj.PhoneNumber = model.PhoneNumber;

            await this.SaveImageForUser(Imgfiles, mainobj.Id);
            await cms_db.UpdateUser(mainobj);
            return RedirectToAction("Profile", "AccountAdmin");
        }

        private async Task<long> SaveImageForUser(HttpPostedFileBase file, long UserId)
        {
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.nguoi_dung && s.ContentObjId == UserId).FirstOrDefault();
            if (CurrentMediaId != null)
            {
                int rs = await cms_db.DeleteMediaContent(CurrentMediaId.MediaContentId);
            }
            ImageUploadViewModel item = new ImageUploadViewModel();
            item = cms_db.UploadHttpPostedFileBase(file);
            MediaContentViewModels _Media = new MediaContentViewModels();
            _Media.Filename = item.ImageName;
            _Media.FullURL = item.ImageUrl;
            _Media.ContentObjId = UserId;
            _Media.ObjTypeId = (int)EnumCore.ObjTypeId.nguoi_dung;
            _Media.ViewCount = 0;
            _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh;
            _Media.CrtdDT = DateTime.UtcNow;
            _Media.MediaContentSize = file.ContentLength;
            _Media.ThumbURL = item.ImageThumbUrl;
            _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
            _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh;
            return await cms_db.AddNewMediaContent(_Media);

        }

        #endregion End Profile

        #region role
        [AdminAuthorize(Roles = "devuser")]
        public ActionResult AddNewRole()
        {
            RoleViewModel model = new RoleViewModel();
            model.ListRole = cms_db.GetRoleListReturnList();
            return View(model);
        }

        [AdminAuthorize(Roles = "devuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role _Role = new Role();
                _Role.Name = model.Name;
                _Role.RoleDes = model.RoleDes;
                int rs = await cms_db.AddRole(_Role);
                return RedirectToAction("AddNewRole");
            }
            return RedirectToAction("Dashboard", "HomeAdmin");
        }

        [AdminAuthorize(Roles = "devuser")]
        public ActionResult EditRole(long id)
        {
            RoleViewModel model = new RoleViewModel();
            model._Role = cms_db.GetObjRoleById(id);
            model.ListRole = cms_db.GetRoleListReturnList();
            return View(model);
        }

        [AdminAuthorize(Roles = "devuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role _Role = cms_db.GetObjRoleById(model.Id);
                _Role.RoleDes = model.RoleDes;
                int rs = await cms_db.UpdateRole(_Role);
                return RedirectToAction("AddNewRole");
            }
            return RedirectToAction("Dashboard", "HomeAdmin");
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUserRole")]
        public ActionResult ManagerUser(string letter, string RoleName, int? page = 1)
        {
            int pageNum = (page ?? 1);
            UserRoleViewModel model = new UserRoleViewModel();
            model.LstRole = cms_db.GetRoleList2();
            var CurrentUser = UserManager.FindById(long.Parse(User.Identity.GetUserId()));
            IQueryable<User> tmp = null;
            if (UserManager.IsInRole(long.Parse(User.Identity.GetUserId()), "devuser"))
            {
                tmp = cms_db.GetUsersNotInRoleByLinkq("devuser");
            }
            if (UserManager.IsInRole(long.Parse(User.Identity.GetUserId()), "supperadmin"))
            {
                tmp = cms_db.GetUsersNotInRoleByLinkq("supperadmin");
            }
            if (!String.IsNullOrEmpty(letter))
            {
                letter = letter.ToLower();
                tmp = tmp.Where(c => c.Login.StartsWith(letter) || c.EMail.StartsWith(letter));
            }
            if (!String.IsNullOrEmpty(RoleName))
            {
                tmp = cms_db.GetUsersInRoleByLinkq(RoleName);
            }
            model.LstAllUser = tmp.OrderBy(c => c.AccountName).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.Page = pageNum;
            model.letter = letter;
            return View("AddUserRole", model);
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUserRole")]
        public async Task<ActionResult> ManagerUserRole(long id)
        {
            User _ObjUser = await cms_db.GetObjUserById(id);
            UserAndRoles model = new UserAndRoles();
            model.LstCurRole = await UserManager.GetRolesAsync(id);
            model.ObjUser = _ObjUser;
            model.LstAllRole = new SelectList(cms_db.GetRoleListReturnList(), "Id", "Name");
            return View(model);
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUserRole")]
        public async Task<ActionResult> AddUserRole(long Id, string RoleName)
        {
            if (!String.IsNullOrEmpty(RoleName))
            {
                var result = await UserManager.AddToRoleAsync(Id, RoleName);
            }
            return RedirectToAction("ManagerUserRole", "AccountAdmin", new { id = Id });
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUserRole")]
        public async Task<ActionResult> RemoveUserRole(long id, string RoleName)
        {
            if (!String.IsNullOrEmpty(RoleName))
            {
                var result = await UserManager.RemoveFromRoleAsync(id, RoleName);
            }
            return RedirectToAction("ManagerUserRole", "AccountAdmin", new { id = id });
        }




        #endregion
        private class ChallengeResult : HttpUnauthorizedResult
        {
            #region constructors and destructors

            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            #endregion

            #region properties

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            #endregion

            #region methods

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = RedirectUri
                };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }

            #endregion
        }

    }
}
