using CMSPROJECT.Areas.AdminCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class ChatGroupController : CoreBackEnd
    {
        // GET: AdminCMS/ChatGroup
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Index()
        {
            return View();
        }
    }
}