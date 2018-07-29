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
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
  
    public class ClassificationController : CoreBackEnd
    {
        //
        // GET: /AdminCMS/Classification/
        //luc nay am chay action này.nhưng in dex thì cung phai có tham số truyền vào mà
        /// <summary>
        /// <param name="SchemeId"></param>
        /// <param name="ScheNM"></param>
        /// <param name="classId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
         [AdminAuthorize(Roles = "supperadmin,devuser,CreateClass")]
        public ActionResult Index(int SchemeId, int? classId, int? page)
        {
            ClassifiIndexViewModels model = new ClassifiIndexViewModels();
            int pageNum = (page ?? 1);
            model.MainLst = cms_db.GetlstClassifiBySchemeId(SchemeId).
                 Where(c => c.ParentClassificationId == classId).OrderByDescending(c => c.DisplayOrder).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            if (classId.HasValue && classId.Value !=0)
                model.classId = classId.Value;
            model.SchemeId = SchemeId;
            if (page.HasValue)
                model.page = page.Value;
            return View(model);

        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateClass")]
        public ActionResult Create(int SchemeId, int? classId)
        {
            ClassificationViewModels model = new ClassificationViewModels();

            if (classId.HasValue && classId.Value != 0)
            {
                Classification tmp = cms_db.GetlstClassifi().Where(s => s.ClassificationId == classId.Value).FirstOrDefault();
                if (tmp.ParentClassificationId != null && tmp.ParentClassificationId != 0)
                {
                    model.ParentList = new SelectList(cms_db.GetlstClassifiByParentId(tmp.ParentClassificationId.Value).AsEnumerable(), "ClassificationId", "ClassificationNM");
                }
                else {
                    model.ParentList = new SelectList(cms_db.GetlstClassifiBySchemeId(SchemeId).
                   Where(s => s.ParentClassificationId == null || s.ParentClassificationId == 0).AsEnumerable(), "ClassificationId", "ClassificationNM");
                }
            }
            else
            {

                model.ParentList = new SelectList(cms_db.GetlstClassifiBySchemeId(SchemeId).
                    Where(s => s.ParentClassificationId == null || s.ParentClassificationId == 0).AsEnumerable(), "ClassificationId", "ClassificationNM");
            }


            model.ParentClassificationId = classId;
            model.SchemeList = new SelectList(cms_db.GetLstClassificationScheme(), "ClassificationSchemeId", "ClassificationSchemeNM");
            model.ClassificationSchemeId = SchemeId;

            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateClass")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClassificationViewModels model)
        {
            if (ModelState.IsValid)
            {
                MediaContent _objmedia = cms_db.GetObjMediaContent(model.ImgdefaultId);
                Classification _ObjClassifi = model._ModelObj;
                _ObjClassifi.FriendlyURL = this.GetFriendlyUrlFromstring(_ObjClassifi.ClassificationNM, -1);
                _ObjClassifi.CrtdUID = long.Parse(User.Identity.GetUserId());
                _ObjClassifi.CrtdDT = DateTime.Now;
                if (model.IsEnabledbool == true)
                    _ObjClassifi.IsEnabled = (int)EnumCore.StateType.enable;
                if (model.IsEnabledbool == false)
                    _ObjClassifi.IsEnabled = (int)EnumCore.StateType.disable;

                _ObjClassifi.DisplayOrder = this.GetMaxDisplayOrderClassifi(_ObjClassifi.ClassificationSchemeId, _ObjClassifi.ParentClassificationId);
                if (_ObjClassifi.ParentClassificationId != null)
                {
                    Classification parent = new Classification();
                    parent = cms_db.GetObjClasscifiById(_ObjClassifi.ParentClassificationId.Value);

                }
                int rs = await cms_db.Classifi(_ObjClassifi);//Lưu classification
                if (_objmedia != null)
                    await this.UpdateClassObjIdForMedia(_objmedia, _ObjClassifi.ClassificationId);//cập nhật id classification này cho hình ảnh bên bảng mediacontent
                int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                    (int)EnumCore.ActionType.Create, "Create", _ObjClassifi.ClassificationId, _ObjClassifi.ClassificationNM, "Classification", (int)EnumCore.ObjTypeId.danh_muc);

                if (rs == (int)EnumCore.Result.action_true && rs2 == (int)EnumCore.Result.action_true)
                    return RedirectToAction("Index", "ClassificationScheme");
                return RedirectToAction("Create");
            }
            model.ParentList = new SelectList(cms_db.GetLstClassification(), "ClassificationId", "ClassificationNM");
            model.SchemeList = new SelectList(cms_db.GetLstClassificationScheme(), "ClassificationSchemeId", "ClassificationSchemeNM");
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateClass")]
        public ActionResult Edit(int? Classid)
        {
            if (Classid == null)
                Classid = 1;
            Classification _obj = cms_db.GetObjClasscifiById(Classid.Value);
            ClassificationViewModels model = new ClassificationViewModels(_obj);
            model.SchemeList = new SelectList(cms_db.GetLstClassificationScheme(), "ClassificationSchemeId", "ClassificationSchemeNM");
            if (cms_db.GetLstClassification() == null)
            {
                model.ParentList = null;
            }
            else
            {
                model.ParentList = new SelectList(cms_db.GetLstClassification(), "ClassificationId", "ClassificationNM");
            }
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateClass")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClassificationViewModels model)
        {
            Classification _ObjClass = model._ModelObj;
            ClassificationScheme _objScheme = cms_db.GetObjScheme(_ObjClass.ClassificationSchemeId);
            _ObjClass.LstModDT = DateTime.Now;
            _ObjClass.LstModUID = long.Parse(User.Identity.GetUserId());
            if (model.IsEnabledbool == true)
                _ObjClass.IsEnabled = 1;
            if (model.IsEnabledbool == false)
                _ObjClass.IsEnabled = 0;
            int rs = await cms_db.EditClass(_ObjClass);

            MediaContent OldDefaultImg = cms_db.GetObjDefaultMediaByContentIdvsType(_ObjClass.ClassificationId, (int)EnumCore.ObjTypeId.danh_muc);
            MediaContent NewDefaultImg = cms_db.GetObjMediaContent(model.ImgdefaultId);
            int UpdateImage = await this.UpdateImageForClassifi(NewDefaultImg, OldDefaultImg, _ObjClass.ClassificationId);

            int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                (int)EnumCore.ActionType.Create, "Create", _ObjClass.ClassificationId, _ObjClass.ClassificationNM, "Classification", (int)EnumCore.ObjTypeId.danh_muc);
            if (rs == (int)EnumCore.Result.action_true && rs2 == (int)EnumCore.Result.action_true)
                return RedirectToAction("Index", new { page = 1, SchemeId = _ObjClass.ClassificationSchemeId, ScheNM = _objScheme.ClassificationSchemeNM, classId = _ObjClass.ParentClassificationId.ToString() });
            return RedirectToAction("Index", new { page = 1, SchemeId = _ObjClass.ClassificationSchemeId, ScheNM = _objScheme.ClassificationSchemeNM, classId = _ObjClass.ParentClassificationId.ToString() });
        }
        /// <summary>
        /// tang displayorder
        /// </summary>
        /// <param name="IdChange"></param>
        /// <param name="_ParentClass"></param>
        /// <param name="_SchemeId"></param>
        /// <param name="ScheNMM"></param>
        /// <returns></returns>
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateClass")]
        public async Task<ActionResult> OrderbyDe(int IdChange, int? _ParentClass, int _SchemeId)
        {
            try
            {
                int x = await cms_db.ChangeOrderByDe(IdChange, _ParentClass, _SchemeId);
                if (_ParentClass == null)
                    return RedirectToAction("Index", new { page = 1, SchemeId = _SchemeId });
                return RedirectToAction("Index", new { page = 1, SchemeId = _SchemeId, classId = _ParentClass });
            }
            catch (Exception ex)
            {
                cms_db.AddToExceptionLog(ex.ToString());
                return RedirectToAction("Index", new { page = 1, SchemeId = _SchemeId, classId = _ParentClass });

            }

        }
         [AdminAuthorize(Roles = "devuser,DeleteClass")]
        public async Task<ActionResult> Delete(int IdChange, int? _ParentClass, int _SchemeId)
        {
             List<Classification> tmp =new List<Classification>();
             Classification _obj = cms_db.GetObjClasscifiById(IdChange);
             tmp=cms_db.GetlstClassifiByParentId(_obj.ClassificationId).ToList();
             foreach (Classification child in tmp)
             {
                 List<Classification> tmp2 = new List<Classification>();
                 tmp2 = cms_db.GetlstClassifiByParentId(child.ClassificationId).ToList();
                 foreach (Classification child2 in tmp2)
                 {
                      await cms_db.DeleteClass(child2.ClassificationId);
                 }
                 await cms_db.DeleteClass(child.ClassificationId);
             
             }
             await cms_db.DeleteClass(IdChange);
             return RedirectToAction("Index", new { page = 1, SchemeId = _SchemeId, classId = _ParentClass });
        }



        /// <summary>
        /// for select list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetLstClassifiBySchemeId(int id)
        {

            List<SelectListObj> result = cms_db.Getclasscatagory(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _UploadImagePartial()
        {
            PartialUploadViewModels model = new PartialUploadViewModels();
            return PartialView("_UploadImagePartial", model);
        }
        public ActionResult _UploadImagePartialE(long id)
        {
            PartialUploadViewModels model = new PartialUploadViewModels();
            MediaContent _obj = cms_db.GetObjDefaultMediaByContentIdvsType(id, (int)EnumCore.ObjTypeId.danh_muc);
            if (_obj != null)
            {
                model.FullUrl = _obj.FullURL;
                model.ThumbUrl = _obj.ThumbURL;
                model.MediaId = _obj.MediaContentId;
            }
            return PartialView("_UploadImagePartial", model);
        }
        /// <summary>
        /// Function cập nhật hình mặc định cho 1 danh mục
        /// Nếu hình mặc định cũ khác hình mặc định mới thì cập nhật 
        /// Nếu ko có hình cũ mà có hình mới thì cập nhật
        /// nếu hình mặc đinh cũ giông hình mặc định mới thì ko làm gì
        /// 
        /// </summary>
        /// <param name="_objnewmedia"></param>
        /// <param name="_objoldmedia"></param>
        /// <param name="ClassifiId"></param>
        /// <returns></returns>
        private async Task<int> UpdateImageForClassifi(MediaContent _objnewmedia, MediaContent _objoldmedia, long ClassifiId)
        {
            if (_objnewmedia != null && _objoldmedia != null)
            {
                if (_objnewmedia.MediaContentId != _objoldmedia.MediaContentId)//Nếu hình mới khác hình củ thì cap nhật lại
                {
                    int _dlmedia = await cms_db.DeleteMediaContent(_objoldmedia.MediaContentId);
                    int UpdateDefaultMedia = await this.UpdateClassObjIdForMedia(_objnewmedia, ClassifiId);
                    return (int)EnumCore.Result.action_true;
                }
            }
            if (_objnewmedia != null && _objoldmedia == null)//nếu chưa có hình cũ thì cập nhật
            {
                int UpdateDefaultMedia = await this.UpdateClassObjIdForMedia(_objnewmedia, ClassifiId);
                return (int)EnumCore.Result.action_true;
            }
            return (int)EnumCore.Result.action_false;
        }
        /// <summary>
        /// Funcion cập nhật id của danh mục cho Object Mediacontent
        /// </summary>
        /// <param name="Media"></param>
        /// <param name="ClassifiId"></param>
        /// <returns></returns>
        private async Task<int> UpdateClassObjIdForMedia(MediaContent Media, long ClassifiId)
        {
            Media.ContentObjId = ClassifiId;
            Media.ObjTypeId = (int)EnumCore.ObjTypeId.danh_muc;
            int rs = await cms_db.UpdateMediaContent(Media);
            return (int)EnumCore.Result.action_true;
        }



    }
}