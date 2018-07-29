using CMSPROJECT.Areas.AdminCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class HistoryManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "devuser")]
        public ActionResult Index(int? page)
        {
            return View(cms_db.GetlstExceptionLog().OrderByDescending(x=>x.CrtdDT).ToPagedList(page ?? 1, 30));
        }

        [AdminAuthorize(Roles = "devuser")]
        public ActionResult Delete(int id)
        {
            int success = cms_db.DeleteExceptionLog(id);
            return RedirectToAction("Index");
        }
    }
}