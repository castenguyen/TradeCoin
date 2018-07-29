
using CMSPROJECT.Areas.AdminCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class HomeAdminController : CoreBackEnd
    {
        public ActionResult Dashboard()
        {
            return View();
        }
	}
}