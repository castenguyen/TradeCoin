using CMSPROJECT.Areas.AdminCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class ErrorManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,GuestUser")]
        public ActionResult Index(string cl, string ac, string path)
        {
            if (Session["errorServer"] != null)
            {
                var ex = Session["errorServer"] as Exception;
                var exHttp = (ex as HttpException);
                if (exHttp != null)
                {
                    ViewData["statusCode"] = exHttp.GetHttpCode();
                    ViewData["pathError"] = path;
                }
                else
                    ViewData["statusCode"] = "";
                cms_db.AddToExceptionLog(ac, cl, exHttp == null ? ex.ToString() : exHttp.ToString());
                Session.Remove("errorServer");
                return View();
            }
            else
            {
                return Redirect("/administrator/Dashboard");
            }
        }
    }
}