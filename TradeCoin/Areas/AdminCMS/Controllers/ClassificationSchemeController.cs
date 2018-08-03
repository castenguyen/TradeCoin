using CMSPROJECT.Areas.AdminCMS.Core;

using DataModel.DataViewModel;
using DataModel.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using PagedList;
using DataModel.Extension;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{

    public class ClassificationSchemeController : CoreBackEnd
    {
       [AdminAuthorize(Roles = "supperadmin,devuser,CreateClass")]
        public ActionResult Index(int? page)
        {
            
            int pageNum = (page ?? 1);
            IPagedList<ClassificationScheme> _lstClassScheme = cms_db.GetlstScheme()
                    .OrderBy(c => c.ClassificationSchemeNM).ToPagedList(pageNum,(int)EnumCore.BackendConst.page_size);
            return View(_lstClassScheme);
        }
         [AdminAuthorize(Roles = "supperadmin,devuser,CreateClass")]
        public ActionResult Create()
        {
            ClassificationSchemeViewModels model = new ClassificationSchemeViewModels();
            model.ParentList = new SelectList(cms_db.GetLstClassificationScheme(), "ClassificationSchemeId", "ClassificationSchemeNM");
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateClass")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassificationSchemeViewModels model)
        {
            ClassificationScheme _ObjClassScheme = model._ModelObj;
            _ObjClassScheme.CrtdUID = long.Parse(User.Identity.GetUserId());
            _ObjClassScheme.CrtdDT = DateTime.Now;
            int rs = await cms_db.AddClassScheme(_ObjClassScheme);
            int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()),Request.ServerVariables["REMOTE_ADDR"], (int)EnumCore.ActionType.Create, "Create",
                _ObjClassScheme.ClassificationSchemeId, _ObjClassScheme.ClassificationSchemeNM, "ClassificationScheme", (int)EnumCore.ObjTypeId.danh_muc);
            if (rs == (int)EnumCore.Result.action_true && rs2 == (int)EnumCore.Result.action_true)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Create");
        }

         [AdminAuthorize(Roles = "supperadmin,devuser,UpdateClass")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                id = 1;
            ClassificationScheme _obj = cms_db.GetObjScheme(id.Value);
            ClassificationSchemeViewModels model = new ClassificationSchemeViewModels(_obj);
            if (_obj.IsSystem == 1)
            {
                model.IsSystemVM = true;
            }
            else {
                model.IsSystemVM = false;
            }
          

            model.ParentList = new SelectList(cms_db.GetLstClassificationScheme(), "ClassificationSchemeId", "ClassificationSchemeNM");
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateClass")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClassificationSchemeViewModels model)
        {
            ClassificationScheme _ObjClassScheme = model._ModelObj;
            if (model.IsSystemVM == true)
            {
                _ObjClassScheme.IsSystem = 1;
            }
            else
            {
                _ObjClassScheme.IsSystem = 0;
            }
            _ObjClassScheme.LstModDT = DateTime.Now;
            
            _ObjClassScheme.LstModUID = long.Parse(User.Identity.GetUserId());
            int rs = await cms_db.EditClassScheme(_ObjClassScheme);
            int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                (int)EnumCore.ActionType.Update, "Update", model.ClassificationSchemeId, model.ClassificationSchemeNM, "ClassificationScheme", (int)EnumCore.ObjTypeId.danh_muc);
            if (rs == (int)EnumCore.Result.action_true && rs2 == (int)EnumCore.Result.action_true)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }



    }
}