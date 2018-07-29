using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alluneecms.Controllers
{
    public class ErrorMessageController : CoreFronEndController
    {

        public ActionResult Index(string cl, string ac, string path)
        {
            if (Session["errorServer"] != null)
            {
                var ex = Session["errorServer"] as Exception;
                var exHttp = (ex as HttpException);
                ViewData["pathError"] = path;
                cms_db.AddToExceptionLog(ac, cl, exHttp == null ? ex.ToString() : exHttp.ToString());
                if (exHttp != null)
                    ViewData["codeError"] = exHttp.ErrorCode;
                Session.Remove("errorServer");
                return View();
            }
            else
            {
                return Redirect("/");
            }
        }

        public ActionResult ErrorExeption()
        {
            return View();
        }
    }
}