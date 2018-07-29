using CMSPROJECT.Areas.AdminCMS.Core;

using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
      [AdminAuthorize(Roles = "supperadmin,devuser")]
    public class ConfigController : CoreBackEnd
    {
        //
        // GET: /AdminCMS/Config/
        public ActionResult Index()
        {
           Config _config = cms_db.GetConfig();
           return View(_config);
        }
        public async Task<ActionResult> Edit(Config _config)
        {
            int rs = await cms_db.EditConfig(_config);
            if (rs == (int)EnumCore.Result.action_true)
                return RedirectToAction("Index");
            return RedirectToAction("Index");
        }
	}
}