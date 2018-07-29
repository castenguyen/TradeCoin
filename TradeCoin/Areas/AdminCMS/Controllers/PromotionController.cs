using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataViewModel;
using DataModel.DataEntity;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using PagedList;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class PromotionController : CoreBackEnd
    {
        //
        // GET: /AdminCMS/Promotion/
        public ActionResult Index(int? page, int? CatelogryId, int? PromotionType)
        {
            int pageNum = (page ?? 1);
            PromotionIndexAdminViewModel model = new PromotionIndexAdminViewModel();
            IQueryable<PromotionContent> tmp = cms_db.GetlstPromotionContent();
            if (CatelogryId.HasValue && CatelogryId.Value != 0)
            {
                tmp = tmp.Where(s => s.MainCateId == CatelogryId);
                model.CatelogryId = CatelogryId.Value;
            }

            if (PromotionType.HasValue && PromotionType.Value != 0)
            {
                tmp = tmp.Where(s => s.PromotionType == PromotionType);
                model.PromotionTypeId = PromotionType.Value;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.MainLstContent = tmp.OrderByDescending(c => c.CrtdDT).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.lstPromotionType = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.loai_khuyen_mai);
            model.lstCatelo = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.san_pham);
            return View(model);

        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreatePromotion")]
        public ActionResult Create(long pro_id)
        {
            Product _pro = cms_db.GetObjProductById(pro_id);
            PromotionContentViewModel model = new PromotionContentViewModel();
            model.LstPromotionType = new SelectList(cms_db.GetClassifiListForSelectlist((int)EnumCore.ClassificationScheme.loai_khuyen_mai), "ClassificationId", "ClassificationNM");
            model.LstPromotionContent = cms_db.GetlstPromotionContent(pro_id, null, null, null, null).ToList();
            model.lstProductCatalogry = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.san_pham), "value", "text");
            model.MainContentId = _pro.ProductId;
            model.Mainpro = _pro;
            return View(model);
        }

           [AdminAuthorize(Roles = "supperadmin,devuser,CreatePromotion")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PromotionContentViewModel model)
        {
            int _tmp = cms_db.GetlstPromotionContent(model.MainContentId,null,model.ObjType,model.PromotionType,null).Count();
            if (_tmp > 0)
                return RedirectToAction("Create", new { pro_id = model.MainContentId });

            if (model.PromotionProductId !=null && model.PromotionProductId.Count() > 0)
            {
                foreach (long item in model.PromotionProductId)
                {
                    PromotionContent Mainmodel = new PromotionContent();
                    Mainmodel.MainContentId = model.MainContentId;
                    Mainmodel.MainContentName = cms_db.GetlstProduct().Where(s => s.ProductId == Mainmodel.MainContentId).FirstOrDefault().ProductName;
                    Mainmodel.PromotionType = model.PromotionType;
                    Mainmodel.PromotionTypeName = cms_db.GetNameObjClasscifiById(model.PromotionType);
                    Mainmodel.CrtdUID = long.Parse(User.Identity.GetUserId());
                    Mainmodel.CrtdName = User.Identity.GetUserName();
                    Mainmodel.CrtdDT = DateTime.Now;
                    Mainmodel.ObjType = model.ObjType;
                    Mainmodel.ObjTypeName = cms_db.GetNameObjClasscifiById(model.ObjType);
                    Mainmodel.MainCateId = cms_db.GetlstProduct().Where(s => s.ProductId == Mainmodel.MainContentId).FirstOrDefault().CategoryId;
                    Mainmodel.MainCatetName = cms_db.GetlstProduct().Where(s => s.ProductId == Mainmodel.MainContentId).FirstOrDefault().CategoryName;
                    Mainmodel.StartDT = this.SpritDateTime(model.DateTimeTxt)[0];
                    Mainmodel.EndDT = this.SpritDateTime(model.DateTimeTxt)[1];
                    Mainmodel.SubContentId = item;
                    Mainmodel.MainCatetName = cms_db.GetlstProduct().Where(s => s.ProductId == item).FirstOrDefault().CategoryName;
                    await cms_db.CreatePromotionContent(Mainmodel);
                }
            }
            else {

                PromotionContent Mainmodel = new PromotionContent();
                Mainmodel.MainContentId = model.MainContentId;
                Mainmodel.MainContentName = cms_db.GetlstProduct().Where(s => s.ProductId == Mainmodel.MainContentId).FirstOrDefault().ProductName;
                Mainmodel.PromotionType = model.PromotionType;
                Mainmodel.PromotionTypeName = cms_db.GetNameObjClasscifiById(model.PromotionType);
                Mainmodel.CrtdUID = long.Parse(User.Identity.GetUserId());
                Mainmodel.CrtdName = User.Identity.GetUserName();
                Mainmodel.CrtdDT = DateTime.Now;
                Mainmodel.ObjType = model.ObjType;
                Mainmodel.ObjTypeName = cms_db.GetNameObjClasscifiById(model.ObjType);
                Mainmodel.MainCateId = cms_db.GetlstProduct().Where(s => s.ProductId == Mainmodel.MainContentId).FirstOrDefault().CategoryId;
                Mainmodel.MainCatetName = cms_db.GetlstProduct().Where(s => s.ProductId == Mainmodel.MainContentId).FirstOrDefault().CategoryName;
                Mainmodel.StartDT = this.SpritDateTime(model.DateTimeTxt)[0];
                Mainmodel.EndDT = this.SpritDateTime(model.DateTimeTxt)[1];
                await cms_db.CreatePromotionContent(Mainmodel);
            }
            return RedirectToAction("Create", new { pro_id = model.MainContentId });
        }

           [AdminAuthorize(Roles = "supperadmin,devuser,DeletePromotion")]
        public async Task<ActionResult> Delete(long PromotionId,long ProductId)
        {
            PromotionContent Mainmodel = cms_db.GetObjPromotionContentById(PromotionId);
            if (Mainmodel.CrtdUID == (long.Parse(User.Identity.GetUserId()))) ;
              await cms_db.RemoveObjPromotionContent(Mainmodel);
            return RedirectToAction("Index");
        }

        private DateTime[] SpritDateTime(string datetime)
        {
            if (datetime != null)
            {
                string[] words = datetime.Split('-');
                DateTime[] model = new DateTime[] { Convert.ToDateTime(words[0]), Convert.ToDateTime(words[1]) };
                return model;
            } else
            {
                DateTime dtime = new DateTime();
                DateTime[] model = new DateTime[] { dtime, dtime };
                return model;
            }
        }


        public JsonResult GetSelectListProductByCate(int id)
        {

            List<Product> result = cms_db.GetlstPromotionProductByCateId(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}