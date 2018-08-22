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

        #region Register-Login-Reset-LogOff-forget


        /// <summary>
        /// có 2 kieu đăng nhập
        /// 1 : Đăng nhập bằng usernam và password
        /// 2: Đăng nhập bằng mã token được gửi về email đăng ký
        /// ==>khi hệ thống đăng nhập bằng mã token thì LoginWithCode bật lên = 1 ngược lai là 0
        /// ==>khi hệ thống đăng nhập bằng mã token thì trang Register ko có trường nhập passwordd 
        /// view của trang register là RegisterWithCode
        /// </summary>
        [AllowAnonymous]
        public ActionResult Register()
        {
            if ((int)EnumCore.ProjectConfig_System.LoginWithCode == 1)
            {
                Config tmp2 = ExtFunction.Config();
                ViewBag.SiteName = tmp2.site_name;
                return View("RegisterWithCode");
            }
            else
            {
                Config tmp2 = ExtFunction.Config();
                ViewBag.SiteName = tmp2.site_name;
                return View();
            }

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
                    if (result.Succeeded)
                    {
                        string pass = UserManager.PasswordHasher.HashPassword("asdadasdasdsa");
                        UserManager.SetLockoutEnabled(user.Id, false);
                        await this.AddRoleForUser(user.Id, "Member");
                        UserManager.Update(user);
                        string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "AccountAdmin", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        EmailService email = new EmailService();
                        IdentityMessage message = new IdentityMessage();
                        message.Body = string.Format("Để xác thực email vui lòng nhấp vào liên kết: <a href='{0}'>Tại đây</a>", callbackUrl);
                        message.Subject = "Xác thực tài khoản";
                        message.Destination = user.Email;
                        await email.SendAsync(message, ConstantSystem.EmailAdmin, ConstantSystem.EmailAdmin,
                            ConstantSystem.EmailAdminPassword, ConstantSystem.EmailAdminSMTP, ConstantSystem.Portmail, true);

                        return RedirectToAction("Login", "AccountAdmin");
                    }
                    else
                    {
                        if ((int)EnumCore.ProjectConfig_System.LoginWithCode == 1)
                        {

                            AddErrors(result);
                            return View("RegisterWithCode");

                        }
                        else
                        {
                            AddErrors(result);
                            return View();
                        }

                    }
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


        /// <summary>
        /// có 2 kieu đăng nhập
        /// 1 : Đăng nhập bằng usernam và password
        /// 2: Đăng nhập bằng mã token được gửi về email đăng ký
        /// ==>khi hệ thống đăng nhập bằng mã token thì LoginWithCode bật lên = 1 ngược lai là 0
        /// ==>khi hệ thống đăng nhập bằng mã token thì trang Login ko có trường nhập passwordd 
        /// view của trang register là LoginWithCode
        /// </summary>

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            if ((int)EnumCore.ProjectConfig_System.LoginWithCode == 1)
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


        /// <summary>
        /// có 2 kieu đăng nhập
        /// 1 : Đăng nhập bằng usernam và password
        /// 2: Đăng nhập bằng mã token được gửi về email đăng ký
        /// ==>khi hệ thống đăng nhập bằng mã token thì LoginWithCode bật lên = 1 ngược lai là 0
        /// ==>khi hệ thống đăng nhập bằng mã token thì khi nhập email sẽ gữi link đang nhập có chứa token về email
        /// view của trang register là LoginWithCode
        /// </summary>

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            /// ==>khi hệ thống đăng nhập bằng mã token thì khi nhập email sẽ gữi link đang nhập có chứa token về email
            if ((int)EnumCore.ProjectConfig_System.LoginWithCode == 1)
            {
                var user = UserManager.Find(model.Email, "123456");
                //if (user.IsLogin == true)
                //{
                //    return RedirectToAction("AlertPage", "Extension", new { AlertString = "User đã login ở một  nợi khác vui lòng đăng xuất tất cả cả thiết bị trước khi đăng nhập lại",type= (int)EnumCore.AlertPageType.lockscreen});
                //}
                if (user == null)
                {
                    //ModelState.AddModelError("", "Invalid username");
                   // return View(model);
                    string AlertString = "Tài khoản chưa đúng";
                    return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.lockscreen });


                }
                else
                {
                    if (UserManager.GetLockoutEnabled(user.Id) == true)
                    {
                        string AlertString = "Tài khoản tạm khoá vui lòng liên hệ quản trị viên";
                        return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.lockscreen });
                    }
                    if (UserManager.IsEmailConfirmed(user.Id) == false)
                    {
                        string AlertString = "Tài khoản chưa kích hoạt vui lòng kiểm tra mail và kích hoạt";
                        return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.lockscreen });
                    }

                    string code = UserManager.GenerateUserToken("LoginWithToken", user.Id);
                    var callbackUrl = Url.Action("LoginWithToken", "AccountAdmin", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    EmailService email = new EmailService();
                    IdentityMessage message = new IdentityMessage();
                    message.Body = string.Format("Để đăng nhập vui lòng nhấp vào liên kết: <a href='{0}'>Tại đây</a>", callbackUrl);
                    message.Subject = "Đăng nhập";
                    message.Destination = user.Email;
                    await email.SendAsync(message, ConstantSystem.EmailAdmin, ConstantSystem.EmailAdmin,
                        ConstantSystem.EmailAdminPassword, ConstantSystem.EmailAdminSMTP, ConstantSystem.Portmail, true);

                    int ach = await cms_db.CreateUserHistory(user.Id, Request.ServerVariables["REMOTE_ADDR"],
                                            (int)EnumCore.ActionType.Login, "Login", 0, model.Email, "User", (int)EnumCore.ObjTypeId.nguoi_dung);

                    return RedirectToAction("LoginWithTokenAlert", "AccountAdmin");

                }

            }
            else  /// ==>khi hệ thống đăng nhập bằng password 
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
                            return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.lockscreen });
                        }

                        if (UserManager.IsEmailConfirmed(user.Id) == false)
                        {
                            string AlertString = "Tài khoản chưa kích hoạt vui lòng kiểm tra mail và kích hoạt";
                            return RedirectToAction("AlertPage", "Extension", new { AlertString = AlertString, type = (int)EnumCore.AlertPageType.lockscreen });
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
        public ActionResult LoginWithTokenAlert()
        {

            return View();

        }


        /// <summary>
        /// có 2 kieu đăng nhập
        /// 1 : Đăng nhập bằng usernam và password
        /// 2: Đăng nhập bằng mã token được gửi về email đăng ký
        /// ==>khi hệ thống đăng nhập bằng mã token thì LoginWithCode bật lên = 1 ngược lai là 0
        /// ==>khi hệ thống đăng nhập bằng mã token thì khi nhập email sẽ gữi link đang nhập có chứa token về email
        /// sau khi check mail sẽ có link đăng nhập =>link đang nhập có thông tin id user và mã token
        /// view của trang register là LoginWithCode
        /// </summary>

        [AllowAnonymous]
        public async Task<ActionResult> LoginWithToken(long userId, string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            var tokenCorrect = await UserManager.VerifyUserTokenAsync(userId, "LoginWithToken", code);
            if (tokenCorrect)
            {
                var user = UserManager.FindById(userId);
             
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username");
                    return View("Error");
                }
                await SignInAsync(user, true);
                User _ObjUser = await cms_db.GetObjUserById(userId);
                _ObjUser.IsLogin = true;
                    ///kiểm tra ngày hết hạn của user
               if (_ObjUser.ExpiredDay.HasValue)
                {
                    if (_ObjUser.ExpiredDay.Value < DateTime.Now)
                    {
                        _ObjUser.PackageId = 1;
                        _ObjUser.PackageName = "Free";
                        int CreateUpdateUserPackage = cms_db.CreateUpdateUserPackage(_ObjUser, 1, (int)EnumCore.UpgradeStatus.het_han, "Hết hạn", "");
                    }
                }
                int updateUser = await cms_db.UpdateUser(_ObjUser);
                int ach = await cms_db.CreateUserHistory(user.Id, Request.ServerVariables["REMOTE_ADDR"],
                                          (int)EnumCore.ActionType.Login, "LoginWithToken", 0, user.Email, "User", (int)EnumCore.ObjTypeId.nguoi_dung);
                if (User.IsInRole("Member"))
                {
                    return RedirectToAction("MemberDashBoard", "Member");
                }
                else if (User.IsInRole("Mod"))
                {
                    return RedirectToAction("ModIndex", "Dashboard");
                }
                else if (User.IsInRole("AdminUser"))
                {
                    return RedirectToAction("ModIndex", "Dashboard");
                }
                else {

                    return RedirectToAction("Index", "Dashboard");
                }
                
            }
            else
            {
                return RedirectToAction("Login", "AccountAdmin");
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
                    await email.SendAsync(message, ConstantSystem.EmailAdmin, ConstantSystem.EmailAdmin,
                        ConstantSystem.EmailAdminPassword, ConstantSystem.EmailAdminSMTP, ConstantSystem.Portmail, true);

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

        [AdminAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {
            try
            {
                User _ObjUser = await cms_db.GetObjUserById(long.Parse(User.Identity.GetUserId()));
                _ObjUser.IsLogin = false;
                int updateUser = await cms_db.UpdateUser(_ObjUser);
                AuthenticationManager.SignOut();
                return RedirectToAction("Login");
            }
            catch (Exception e)
            {
                return RedirectToAction("Login");

            }
        
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

        #region ChangePassword-ChangeState-RemoveAccountList
        [AdminAuthorize]
        public async Task<ActionResult> ChangePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess
                ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess
                    ? "Your password has been set."
                    : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed." : message == ManageMessageId.Error ? "An error has occurred." : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
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
        public async Task<ActionResult> ChangePassword(ManageUserViewModel model)
        {
            var hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    User _ObjUser = await cms_db.GetObjUserById(long.Parse(User.Identity.GetUserId()));
                    int _rs = await cms_db.UpdateUser(_ObjUser);
                    var result = await UserManager.ChangePasswordAsync(long.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        int ach = await cms_db.CreateUserHistory(_ObjUser.Id, Request.ServerVariables["REMOTE_ADDR"],
                                         (int)EnumCore.ActionType.Login, "ChangePasswor", 0, model.Email, "User", (int)EnumCore.ObjTypeId.nguoi_dung);
                        var user = await UserManager.FindByIdAsync(long.Parse(User.Identity.GetUserId()));
                        await SignInAsync(user, false);
                        return RedirectToAction(
                            "ChangePassword",
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
            return RedirectToAction("ManagerUser", "AccountAdmin", new { id = id });

        }


        #endregion


        #region Profile-SaveImageForUser
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

            mainobj.BirthDay = model.BirthDay;

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

        #region ListUser-DetailUser-ManagerUser-ListUserUpgrade-DetailUpgradeUser-UpgradePackage


        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUser")]
        public ActionResult ListUser(string letter, string RoleName, int? page = 1)
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
            model.LstAllUser = tmp.OrderBy(c => c.FullName).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.Page = pageNum;
            model.letter = letter;
            return View(model);
        }





        /// <summary>
        /// HIỂN THỊ THÔNG TIN CHI TIẾT USER
        /// THÔNG TIN VỀ TÀI KHOẢN
        /// LỊCH SỬ HOẠT ĐỘNG ---NẾU LÀ MEMBER THI CÓ LỊCH SỬ NÂNG CẤP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUser")]
        public async Task<ActionResult> DetailUser(long id)
        {
            User _ObjUser = await cms_db.GetObjUserById(id);
            UserAndRoles model = new UserAndRoles();
            model.LstCurPermission = await UserManager.GetRolesAsync(id);
            model.ObjUser = _ObjUser;
            model.LstAllPermission = new SelectList(cms_db.GetRoleListReturnList(), "Id", "Name");
            return View(model);
        }


        /// <summary>
        /// PHÂN QUYỀN CHO USER 
        /// THAY ĐỔI GÓI PACKAGE CHO USER (NÂNG CẤP USER)
        /// /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUser")]
        public async Task<ActionResult> ManagerUser(long id, string alertMessage)
        {
            User _ObjUser = await cms_db.GetObjUserById(id);
            UserAndRoles model = new UserAndRoles();
            model.LstCurPermission = await UserManager.GetRolesAsync(id);
            model.LstAllPermission = new SelectList(cms_db.GetlstRole().Where(s=>s.IsGroup==false), "Id", "Name");
            model.ObjUser = _ObjUser;

            model.LstPackages = new SelectList(cms_db.GetlstPackage(), "PackageId", "PackageName");
            model.LstCurUserType = await UserManager.GetRolesAsync(id);
            model.LstAllUserType = new SelectList(cms_db.GetlstRole().Where(s => s.IsGroup == true), "Id", "Name");
            if (UserManager.IsInRole(id, "Member"))
            {
                if(!model.ObjUser.PackageId.HasValue)
                {
                    model.ObjUser.PackageId = 1;
                    model.ObjUser.PackageName = "Free";

                }
             
            }

            if (model.ObjUser.AwaitPackageId.HasValue)
            {
                model.UpgradeToken = cms_db.GetLastUpgradeToken(model.ObjUser.Id,model.ObjUser.AwaitPackageId.Value);
            }

            if (!String.IsNullOrEmpty(alertMessage))
            {
                model.AlertMessage = alertMessage;
            }

           




            return View(model);
        }

        /// <summary>
        /// Danh sách user chờ upgrade
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="RoleName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUser")]
        public ActionResult ListUserUpgrade(string letter, string RoleName, int? page = 1)
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
            model.LstAllUser = tmp.Where(s=>s.AwaitPackageId.Value>0).OrderBy(c => c.FullName).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.Page = pageNum;
            model.letter = letter;
            return View(model);
        }


        /// <summary>
        /// Chi tiết thông tin nâng cấp của 1 tài khoản
        /// Lịch sử nâng cấp
        /// Tổng giá trị nâng cấp
        /// </summary>
        /// <param name="id"></param>
        /// <param name="alertMessage"></param>
        /// <returns></returns>
        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUser")]
        public async Task<ActionResult> DetailUpgradeUser(long id, string alertMessage)
        {
            User _ObjUser = await cms_db.GetObjUserById(id);
            DetailUserUpgrade model = new DetailUserUpgrade();
            model.ObjUser = _ObjUser;
            model.LstPackages = new SelectList(cms_db.GetlstPackage(), "PackageId", "PackageName");
            model.LstHistoryUpgrade = cms_db.GetlstUserPackage(id);

            UserPackage objAwaitUserPackage = cms_db.GetlastAwaitUserPackage(id);
            if (objAwaitUserPackage != null)
            {
                model.objAwaitUserPackage = cms_db.GetlastAwaitUserPackage(id);
                model.PackageID = model.objAwaitUserPackage.PackageId.Value;
                model.Price = model.objAwaitUserPackage.Price.Value;
                model.ExpiryDay = DateTime.Now.AddDays(model.objAwaitUserPackage.NumDay.Value);
                ///nếu so ngay nâng cấp lớn hon 9 thi goi đó là tháng hoac quý
                if (model.objAwaitUserPackage.NumDay > 0)
                {
                    model.Datetime = model.ExpiryDay.ToShortDateString();
                }//nguoc lai là vinh vien
                else
                {
                    model.checkForerver = true;
                }
                if (UserManager.IsInRole(id, "Member"))
                {
                    if (!model.ObjUser.PackageId.HasValue)
                    {
                        model.ObjUser.PackageId = 1;
                        model.ObjUser.PackageName = "Free";
                    }
                }
                if (model.ObjUser.AwaitPackageId.HasValue)
                {
                    model.UpgradeToken = model.objAwaitUserPackage.UpgradeToken;
                }

            }
   
           

            if (!String.IsNullOrEmpty(alertMessage))
            {
                model.AlertMessage = alertMessage;
            }
            return View(model);
        }

        /// <summary>
        /// NÂNG CẤP 1 TÀI KHOẢN
        /// </summary>
        /// <param name="id"></param>
        /// <param name="packageid"></param>
        /// <returns></returns>
        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUser")]
        public async Task<ActionResult> UpgradePackage(DetailUserUpgrade model)
        {
            try
            {
                User _ObjUser = await cms_db.GetObjUserById(model.ObjUser.Id);
                Package ObjCurrentPackage = new Package();
                ///Láy gòi cước hiện tại
                if (_ObjUser.PackageId.HasValue)
                {
                    ObjCurrentPackage = cms_db.GetObjPackage(_ObjUser.PackageId.Value);
                }
                else
                {
                    //1 là package free
                    ObjCurrentPackage = cms_db.GetObjPackage(1);
                }

                ///Láy gòi cước mới
                Package ObjNewPackage = cms_db.GetObjPackage(model.PackageID);

                //cập nhật lại thông tin gói cước cho user
                _ObjUser.PackageId = ObjNewPackage.PackageId;
                _ObjUser.PackageName = ObjNewPackage.PackageName;

                // chuyển lại gói cước chờ nâng cấp thành null
                _ObjUser.AwaitPackageId = 0;
                _ObjUser.AwaitPackageName = "";

                //cập nhat lai ngay hết hạn cho user
                //nếu thời gian của gói cước lớn hơn 0
                //thì co nghi a la nâng cấp tháng-quý
                if (!String.IsNullOrEmpty(model.Datetime))
                {
                        _ObjUser.ExpiredDay = this.SpritDateTime(model.Datetime)[1];
                }
                else
                {
                    _ObjUser.ExpiredDay = null;
                }
                int rs = await cms_db.UpdateUser(_ObjUser);


                ////CẬP NHẬT LỊCH SỬ NÂNG CẤP
                UserPackage objUserPackage = new UserPackage();
                objUserPackage.CrtdDT = DateTime.Now;
                objUserPackage.AprvdDT = DateTime.Now;
                objUserPackage.AprvdUID =long.Parse(User.Identity.GetUserId());
                objUserPackage.AprvdUserName = User.Identity.GetUserName();

                objUserPackage.PackageId = ObjNewPackage.PackageId;
                objUserPackage.PackageName = ObjNewPackage.PackageName;
                objUserPackage.UpgradeUID = _ObjUser.Id;
                objUserPackage.UpgradeUserName = _ObjUser.EMail;

                objUserPackage.StateId = (int)EnumCore.StateType.cho_phep;
                objUserPackage.StateName = "Duyệt";
                objUserPackage.OldPackageID = ObjCurrentPackage.PackageId;
                objUserPackage.OldPackageName = ObjCurrentPackage.PackageName;
                if (ObjNewPackage.PackageId == (int)EnumCore.Package.free)
                {
                    objUserPackage.Price =0;
                }
                else {
                    objUserPackage.Price = model.Price;

                }
               

                cms_db.CreateUserPackage(objUserPackage);

                return RedirectToAction("DetailUpgradeUser", "AccountAdmin", new { id = model.ObjUser.Id, alertMessage = "Nâng cấp gói cước mới thành công" });

            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DetailUpgradeUser", "AccountAdmin", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("DetailUpgradeUser", "AccountAdmin", new { id = model.ObjUser.Id, alertMessage = "Nâng cấp gói cước không thành công" });
            }
        }
        private DateTime[] SpritDateTime(string datetime)
        {
            try {
                DateTime[] model = new DateTime[] { Convert.ToDateTime(datetime), Convert.ToDateTime(datetime) };
                return model;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("SpritDateTime", "AccountAdmin", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return null;
            }
        }

        #endregion

        #region AddUserRole-AddRoleForUser-RemoveUserRole

        /// <summary>
        /// THÊM ROLE MỚI CHO USER
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUser")]

        public async Task<ActionResult> AddUserRole(long Id, string RoleName)
        {
            try
            {
                if (!String.IsNullOrEmpty(RoleName))
                {
                    await this.AddRoleForUser(Id, RoleName);
                }
                return RedirectToAction("ManagerUser", "AccountAdmin", new { id = Id, alertMessage = "Thêm quyền mới thành công" });
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("AddUserRole", "AccountAdmin", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("ManagerUser", "AccountAdmin", new { id = Id, alertMessage = "Thêm quyền mới không thành công" });
            }
        }


        private async Task<int> AddRoleForUser(long Id, string RoleName)
        {
            try
            {
                var result = await UserManager.AddToRoleAsync(Id, RoleName);
                Role objParentRole = cms_db.GetObjRoleByName(RoleName);
                long[] lstChildRole = cms_db.GetlstPermission(objParentRole.Id);
                foreach (int _val in lstChildRole)
                {
                    Role obj = cms_db.GetObjRoleById(_val);
                    await UserManager.AddToRoleAsync(Id, obj.Name);
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("AddRoleForUser", "AccountAdmin", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return (int)EnumCore.Result.action_false;
            }


        }
        /// <summary>
        /// LOẠI BỎ MỘT ROLE CỦA USER
        /// </summary>
        /// <param name="id"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>

        [AdminAuthorize(Roles = "supperadmin,devuser,ManagerUser")]
        public async Task<ActionResult> RemoveUserRole(long id, string RoleName)
        {

            try
            {
                if (!String.IsNullOrEmpty(RoleName))
                {
                    var result = await UserManager.RemoveFromRoleAsync(id, RoleName);
                    Role objParentRole = cms_db.GetObjRoleByName(RoleName);
                    long[] lstChildRole = cms_db.GetlstPermission(objParentRole.Id);
                    foreach (int _val in lstChildRole)
                    {
                        Role obj = cms_db.GetObjRoleById(_val);
                        await UserManager.RemoveFromRoleAsync(id, obj.Name);
                    }
                }
                return RedirectToAction("ManagerUser", "AccountAdmin", new { id = id, alertMessage = "Xoá quyền thành công" });
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("RemoveUserRole", "AccountAdmin", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("ManagerUser", "AccountAdmin", new { id = id, alertMessage = "Xoá quyền không thành công" });
            }
        }
        #endregion

        #region AddErrors- HasPassword -RedirectToLocal -SendEmail -SignInAsync -Dispose

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



            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

       
        

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
