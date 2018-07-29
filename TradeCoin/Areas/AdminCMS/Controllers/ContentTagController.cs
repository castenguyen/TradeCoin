using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class ContentTagController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            List<Tag> model = cms_db.GetlstTag().ToList();
            return View(model.ToPagedList(pageNum, 10));
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateTag")]
        public ActionResult CreateTag()
        {
            return View();
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateTag")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTag(TagViewModels model)
        {
            Tag Tagmodel = model._MainModel;
            int rs = await cms_db.AddTag(Tagmodel);
            return RedirectToAction("Index");
        }
    }
}