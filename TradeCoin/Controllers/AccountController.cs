using DataModel.DataEntity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using DataModel.DataViewModel;
using DataModel.Extension;
namespace Alluneecms.Controllers
{
    [Authorize]
    public class AccountController : CoreFronEndController
    {
        private MyUserManager _userManager;
        public AccountController()
        {
        }

        public AccountController(MyUserManager userManager)
        {
            UserManager = userManager;
        }

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

        [HttpGet]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Profile(ProfileViewModel model)
        {
            User mainobj = cms_db.GetObjUserByIdNoAsync(model.Id);
            mainobj.Id = model.Id;
            mainobj.BankAccountHolder = model.BankAccountHolder;
            mainobj.BankAccountNbr = model.BankAccountNbr;
            mainobj.BankAdress = model.BankAdress;
            mainobj.BankName = model.BankName;
            mainobj.BirthDay = model.BirthDay;
            mainobj.CMND = model.CMND;
            mainobj.EMail = model.EMail;
            mainobj.GenderId = model.GenderId;
            mainobj.GenderName = model.GenderId == (int)EnumCore.Classification.gioi_tinh_nam ? "Nam" : (model.GenderId == (int)EnumCore.Classification.gioi_tinh_nu ? "Nữ" : "Khác");
            mainobj.PhoneNumber = model.PhoneNumber;
            mainobj.FullName = model.FullName;
            await cms_db.UpdateUser(mainobj);
            return RedirectToAction("Profile", "Account");
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePasswordUser(string oldP, string newP, string newPCom)
        {
            long idUser = long.Parse(User.Identity.GetUserId());
            if (oldP != null && newP != null && newPCom != null && newP == newPCom)
            {

                var result = await UserManager.ChangePasswordAsync(long.Parse(User.Identity.GetUserId()), oldP, newP);
                return Json(result.Succeeded, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> UpdatePhotoUser()
        {
            long idUser = long.Parse(User.Identity.GetUserId());
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file != null)
                {
                    long result = await SaveImageForUser(file, idUser);
                    MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.nguoi_dung
                                                        && s.ContentObjId == idUser).FirstOrDefault();

                    string userPhotoUrl = "/Media/no_photo.png";
                    if (CurrentMediaId != null)
                        userPhotoUrl = CurrentMediaId.FullURL;
                    return Json("{\"photo_url\": \"" + userPhotoUrl + "\", \"result\": \"1\"}");

                }
                else
                    return Json("{\"photo_url\": \"" + "/Media/no_photo.png" + "\", \"result\": \"0\"}");
            }
            catch (Exception ex)
            {
                DataModel.DataStore.Core core = new DataModel.DataStore.Core();
                core.AddToExceptionLog("UpdatePhotoUser", "AccountController", "Upload photo Error: " + ex.Message, idUser);
                return Json("{\"photo_url\": \"" + "/Media/no_photo.png" + "\", \"result\": \"0\"}");
            }

        }

        [HttpPost]
        public ActionResult CropImage(
            string imagePath,
            int? cropPointX,
            int? cropPointY,
            int? imageCropWidth,
            int? imageCropHeight)
        {
            if (string.IsNullOrEmpty(imagePath)
                || !cropPointX.HasValue
                || !cropPointY.HasValue
                || !imageCropWidth.HasValue
                || !imageCropHeight.HasValue)
            {
                return new HttpStatusCodeResult((int)System.Net.HttpStatusCode.BadRequest);
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(Server.MapPath(imagePath));
            byte[] croppedImage = cms_db.CropImage(imageBytes, cropPointX.Value, cropPointY.Value, imageCropWidth.Value, imageCropHeight.Value);

            long idUser = long.Parse(User.Identity.GetUserId());

            try
            {
                if (croppedImage != null)
                {
                    MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.nguoi_dung
                                                        && s.ContentObjId == idUser).FirstOrDefault();
                    string userPhotoUrl = "~/" + CurrentMediaId.FullURL;

                    System.IO.FileStream _FileStream = new System.IO.FileStream(Server.MapPath(userPhotoUrl), System.IO.FileMode.Create,
                                  System.IO.FileAccess.Write);
                    _FileStream.Write(croppedImage, 0, croppedImage.Length);

                    _FileStream.Close();

                    return Json("{\"photo_url\": \"" + CurrentMediaId.FullURL + "\", \"result\": \"1\"}");

                }
                else
                    return Json("{\"photo_url\": \"" + "/Media/no_photo.png" + "\", \"result\": \"0\"}");
            }
            catch (Exception ex)
            {
                DataModel.DataStore.Core core = new DataModel.DataStore.Core();
                core.AddToExceptionLog("UpdatePhotoUser", "AccountController", "Upload photo Error: " + ex.Message, idUser);
                return Json("{\"photo_url\": \"" + "/Media/no_photo.png" + "\", \"result\": \"0\"}");
            }
        }

        private async Task<long> SaveImageForUser(HttpPostedFileBase file, long UserId)
        {
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.nguoi_dung && s.ContentObjId == UserId).FirstOrDefault();
            if (CurrentMediaId != null)
            {
                int rs = await cms_db.DeleteMediaContent(CurrentMediaId.MediaContentId);
            }
            ImageUploadViewModel item = new ImageUploadViewModel();
            item = cms_db.UploadHttpPostedFileBaseFrond(file);
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, bool isAjax)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return isAjax ? Json(true, JsonRequestBehavior.AllowGet) : RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            if (isAjax)
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, bool isAjax)
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
                        GenderId = model.Gender,
                        GenderName = model.Gender == (int)EnumCore.Classification.gioi_tinh_nam ? "Nam" : (model.Gender == (int)EnumCore.Classification.gioi_tinh_nu ? "Nữ" : "Khác")
                    };
                    var result = UserManager.Create(user, model.Password);
                    if (result.Succeeded)
                    {
                      //  UserManager.SetLockoutEnabled(user.Id, true);
                        //UserManager.Update(user);
                        //string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                        //var callbackUrl = Url.Action("ConfirmEmail", "AccountAdmin", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        //EmailService email = new EmailService();
                        //IdentityMessage message = new IdentityMessage();
                        //message.Body = string.Format("Để xác thực email vui lòng nhấp vào liên kết: <a href='{0}'>Tại đây</a>", callbackUrl);
                        //message.Subject = "Xác thực tài khoản";
                        //message.Destination = user.Email;
                        //try
                        //{
                        //    await email.SendAsync(message, EmailService.EmailAdmin, EmailService.EmailAdmin,
                        //    EmailService.EmailAdminPassword, EmailService.EmailAdminSMTP, EmailService.Portmail, true);
                        //}
                        //catch { }
                        if (isAjax)
                            return Json(true, JsonRequestBehavior.AllowGet);
                        else
                            return RedirectToAction("Login", "Account");
                    }
                    AddErrors(result);
                }
                if (isAjax)
                    return Json(false, JsonRequestBehavior.AllowGet);
                else
                    return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("function Register Public", "Account", e.ToString());
                return null;
            }
        }

        [AllowAnonymous]
        public ActionResult RegAPI(string userName, string password, string fullName, string email)
        {
            if (userName == null || password == null)
            {
                return Json(new { ms = "Tài khoản hoặc mật khẩu trống !", check = false }, JsonRequestBehavior.AllowGet);
            }
            bool newuser = cms_db.CheckAnyUsername(userName);
            if (newuser)
            {
                return Json(new { ms = "Tài khoản hoặc email đã tồn tại", check = false }, JsonRequestBehavior.AllowGet);
            }

            var user = new MyUser
            {
                UserName = userName,
                Email = email,
                FullName = fullName,
                GenderId = (int)EnumCore.Classification.gioi_tinh_nam,
                GenderName = "Nam",
                AccountName = "TRV"
            };
            UserManager.Create(user, password);

            return Json(new { ms = "Thành công !", check = false }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<bool> ApiLogin(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            var user = await UserManager.FindAsync(username, password);
            if (user != null)
            {
                return true;
            }

            return false;
        }

        [AllowAnonymous]
        public ActionResult ApiInfoUser(string username)
        {
            if (username == null)
                return Json(new { Username = "", Name = "", Email = "", Phone = "", Avatar = "" }, JsonRequestBehavior.AllowGet);
            var de = new alluneedbEntities();
            var user = de.Users.Single(p => p.Login == username);
            var avartar = "images/gallery.gif";
            if (de.MediaContents.Any(p => p.ContentObjId == user.Id))
                avartar = de.MediaContents.Single(p => p.ContentObjId == user.Id).FullURL;
            return Json(new { Username = user.Login, Name = user.FullName, Email = user.EMail, Phone = user.PhoneNumber, Avatar = avartar }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public bool LogAPI(string userName, string password)
        {
            var user = UserManager.FindAsync(userName, password);
            if (user != null)
            {
                return true;
            }
            return false;
        }


        [AllowAnonymous]
        public bool CheckUserAny(string userName)
        {
            return cms_db.CheckAnyUsername(userName);
        }


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
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
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
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var user = await UserManager.FindAsync(loginInfo.Login);
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        // If the user does not have an account, then prompt the user to create an account
        //        ViewBag.ReturnUrl = returnUrl;
        //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
        //    }
        //}

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        //public async Task<ActionResult> LinkLoginCallback()
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //    }
        //    var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Manage");
        //    }
        //    return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser() { UserName = model.UserName };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInAsync(user, isPersistent: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
                AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //[ChildActionOnly]
        //public ActionResult RemoveAccountList()
        //{
        //    var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
        //    ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
        //    return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Replace("Name", "Chú ý ").Replace("is already taken", "đã được sử dụng"));
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

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
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
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}