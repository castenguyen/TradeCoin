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

    public class TickerManagerController : CoreBackEnd
    {
        public ActionResult Index(int? page, int? TickerStatus, int? TickerPackage,  string FillterTickerName)
        {
            int pageNum = (page ?? 1);
            TickerAdminViewModel model = new TickerAdminViewModel();
            IQueryable<Ticker> tmp = cms_db.GetlstTicker().Where(s => s.StateId != (int)EnumCore.StateType.da_xoa);



            if (TickerStatus.HasValue)
            {
                tmp = tmp.Where(s => s.StateId == TickerStatus);
                model.TickerStatus = TickerStatus.Value;
            }

            if (TickerPackage.HasValue && TickerPackage.Value != 0)
            {
               // tmp = tmp.Where(s => s.StateId == Pakage);
                model.TickerPackage = TickerPackage.Value;
            }

            if (!String.IsNullOrEmpty(FillterTickerName))
            {
                tmp = tmp.Where(s => s.TickerName.ToLower().Contains(FillterTickerName.ToLower()));
                model.FillterTickerName = FillterTickerName;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.lstMainTicker = tmp.OrderByDescending(c => c.TickerId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.lstTickerStatus = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.state_type), "value", "text");
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text"); 

            return View(model);
        }
        public ActionResult Create()
        {
            TickerViewModel model = new TickerViewModel();
            model.lstPackage = cms_db.GetObjSelectListPackage();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TickerViewModel model, HttpPostedFileBase Default_files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ticker MainModel = model._MainObj;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.StateId = (int)EnumCore.StateType.cho_duyet;
                    MainModel.StateName = "Chờ Duyệt";
                    int rs = await cms_db.CreateTickerAsync(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForTicker(Default_files, MainModel.TickerId);
                        int rsup = await this.UpdateImageUrlForTicker(rsdf, MainModel);
                    }
                    int SaveTickerPackage = this.SaveTickerPackage(model.lstTickerPackage, MainModel.TickerId);
                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "Create", MainModel.TickerId, MainModel.TickerName, "TickerManager", (int)EnumCore.ObjTypeId.ticker);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Create", "TickerManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
          }



        public ActionResult Update(int? id)
        {
            if (id == null)
                id = 1;
            Ticker _obj = cms_db.GetObjTicker(id.Value);
            TickerViewModel model = new TickerViewModel(_obj);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(TickerViewModel model, HttpPostedFileBase Default_files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ticker MainModel = model._MainObj;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.StateId = (int)EnumCore.StateType.cho_duyet;
                    MainModel.StateName = "Chờ Duyệt";
                    int rs = await cms_db.CreateTickerAsync(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForTicker(Default_files, MainModel.TickerId);
                        int rsup = await this.UpdateImageUrlForTicker(rsdf, MainModel);
                    }
                    int SaveTickerPackage = this.SaveTickerPackage(model.lstTickerPackage, MainModel.TickerId);
                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "Create", MainModel.TickerId, MainModel.TickerName, "TickerManager", (int)EnumCore.ObjTypeId.ticker);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Create", "TickerManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
        }
        public async Task<ActionResult> ChangeState(long id, int state)
        {
            try
            {
                Product MainModel = cms_db.GetObjProductById(id);
                MainModel.AprvdUID = long.Parse(User.Identity.GetUserId());
                MainModel.AprvdDT = DateTime.Now;

                MainModel.StateId = state;
                if (MainModel.StateId == (int)EnumCore.StateType.cho_phep)
                    MainModel.StateName = this.StateName_Enable;
                if (MainModel.StateId == (int)EnumCore.StateType.khong_cho_phep)
                    MainModel.StateName = this.StateName_Disable;
                await cms_db.UpdateProduct(MainModel);
                int ach = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                 (int)EnumCore.ActionType.Create, "ChangeState", MainModel.ProductId, MainModel.ProductName, "Product", (int)EnumCore.ObjTypeId.san_pham);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("ChangeState", "ProductManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
        }
    
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                Product MainModel = cms_db.GetObjProductById(id);
                if (MainModel != null)
                {
                    //MediaContent _objoldmedia = cms_db.GetObjDefaultMediaByContentIdvsType(id, (int)EnumCore.ObjTypeId.san_pham);
                    //if (_objoldmedia != null)
                    //    await cms_db.DeleteMediaContent(_objoldmedia.MediaContentId);
                    //int _DeleteRelatedTag = await cms_db.DeleteRelatedTag(id, (int)EnumCore.ObjTypeId.san_pham);
                    int result = await cms_db.DeleteProductByObj(MainModel);
                    int ach = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
               (int)EnumCore.ActionType.Create, "Delete", MainModel.ProductId, MainModel.ProductName, "Product", (int)EnumCore.ObjTypeId.san_pham);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Delete", "Product", e.ToString());
                return RedirectToAction("Index");
            }
        }



        private async Task<MediaContentViewModels> SaveDefaultImageForTicker(HttpPostedFileBase file, long TickerId)
        {
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.ticker
                    && s.MediaTypeId == (int)EnumCore.mediatype.hinh_anh_dai_dien && s.ContentObjId == TickerId).FirstOrDefault();
            if (CurrentMediaId != null)
            {
                int rs = await cms_db.DeleteMediaContent(CurrentMediaId.MediaContentId);
            }
            ImageUploadViewModel item = new ImageUploadViewModel();
            item = cms_db.UploadHttpPostedFileBase(file);
            MediaContentViewModels _Media = new MediaContentViewModels();
            _Media.Filename = item.ImageName;
            _Media.FullURL = item.ImageUrl;
            _Media.ContentObjId = TickerId;
            _Media.ObjTypeId = (int)EnumCore.ObjTypeId.ticker;
            _Media.ViewCount = 0;
            _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh_dai_dien;
            _Media.CrtdDT = DateTime.UtcNow;
            _Media.MediaContentSize = file.ContentLength;
            _Media.ThumbURL = item.ImageThumbUrl;
            _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
            await cms_db.AddNewMediaContent(_Media);
            return _Media;

        }
        private async Task<int> UpdateImageUrlForTicker(MediaContentViewModels ImageObj, Ticker TickerObj)
        {
            TickerObj.MediaUrl = ImageObj.FullURL;
            TickerObj.MediaThumb = ImageObj.ThumbURL;
            return await cms_db.UpdateTicker(TickerObj);
        }

        private int SaveTickerPackage(int[] model, long TickerId)
        {
            try
            {
                int dl = cms_db.DeleteContentPackage(TickerId, (int)EnumCore.ObjTypeId.ticker);
                foreach (int _val in model)
                {
                    ContentPackage tmp = new ContentPackage();
                    tmp.ContentId = TickerId;
                    tmp.ContentType = (int)EnumCore.ObjTypeId.ticker;
                    tmp.PackageId = _val;
                 
                    cms_db.CreateContentPackage(tmp);
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("SaveProductSize", "ProductManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return (int)EnumCore.Result.action_false;
            }
        }




















    }
}