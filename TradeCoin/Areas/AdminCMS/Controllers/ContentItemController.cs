﻿using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DataModel.Extension;
using System.Threading.Tasks;
using System.Collections;
using PagedList;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class ContentItemController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Index(int? page, int? catalogry, int? state)
        {
            int pageNum = (page ?? 1);
            ContentItemIndexViewModel model = new ContentItemIndexViewModel();
            IQueryable<ContentItem> tmp = cms_db.GetlstContentItem().Where(s => (s.MicrositeID == null || s.MicrositeID == 0) && s.StateId != (int)EnumCore.StateType.da_xoa && s.ObjTypeId == (int)EnumCore.ObjTypeId.tin_tuc);
            if (catalogry.HasValue && catalogry.Value != 0)
            {
                tmp = tmp.Where(s => s.CategoryId == catalogry);
                model.ContentCatalogry = catalogry.Value;
            }

            if (state.HasValue && state.Value != 0)
            {
                tmp = tmp.Where(s => s.StateId == state);
                model.ContentState = state.Value;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.lstMainContent = tmp.OrderByDescending(c => c.ContentItemId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.lstContentState = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.state_type);
            model.lstContentCatalogry = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.tin_tuc_bai_viet);
            return View(model);
        }
        /// <summary>
        /// Quy trình thêm 1 ContentItem :
        /// 
        /// -Đầu tiên upload file hinh vào server trước lúc này chưa biết được id của contentItem nên trong objMedia minh bỏ trống trường
        /// ContentObjId và ObjTypeId.Sau khi tạo dc contentItem thì lấy id của contentitem cập nhật lại cho media.
        /// 
        /// -Load danh sách các contenitem đã có và các tag để chọn nội dun gliên quan và tag liên quan.
        /// -Sau khi lưu được ContentItem thì tiếp tực lưu nội dung liên quan cho nó.Lưu nội dung liên quan với SubjectContentItemId là các nội dung liên quan
        /// ObjContentItemId là id contentitemvừa tạo.ContentRelatedTypeId là loại liên quan.ở đây là contenItem liên quan với ContenItem
        /// -Tag liên quan lưu tương tự như nội dung liên quan.
        /// </summary>
        /// <returns></returns>
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateNews")]
        public ActionResult Create()
        {
            ContentItemViewModels model = new ContentItemViewModels();
            model.CatalogryList = new SelectList(cms_db.Getnewcatagory(), "ClassificationId", "ClassificationNM");
            return View(model);
        }
        /// <summary>
        /// Tạo 1 tin tức.
        /// trong file view có nhúng 3 partial view là 
        /// -partial view upload hình ảnh.
        /// -partial view từ khoá liên quan
        /// -partial view noi dung liên quan
        /// ===>3 parial view này có thể xài lại
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateNews")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ContentItemViewModels model)
        {
            try {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                    MediaContent _objmedia = cms_db.GetObjMediaContent(model.ImgdefaultId);
                    ContentItem MainModel = model._MainObj;
                    MainModel.CommentCount = 0;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.ObjTypeId = (int)EnumCore.ObjTypeId.tin_tuc;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    MainModel.ViewCount = 0;
                 
                    if (_objmedia != null)
                    {
                        MainModel.MediaUrl = _objmedia.FullURL;
                        MainModel.MediaThumb = _objmedia.ThumbURL;
                    }
                    MainModel.StateId = (int)EnumCore.StateType.cho_phep;
                    MainModel.StateName = this.StateName_Enable;
                    int rs = await cms_db.CreateContentItem(MainModel);
                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "Create", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);
                    //int SaveRelatedContent = await this.SaveRelateContent(MainModel.ContentItemId, model.related_content);//lưu noi dung liên quan cho tin tức này
                    //int SaveRelatedTag = await this.SaveRelateTag(MainModel.ContentItemId, model.related_tag);//lưu tag liên quan cho tin tức này
                    //int UpdateDefaultMedia = await this.UpdateContentObjForMedia(_objmedia, MainModel.ContentItemId);//cập nhật id tin tức này cho hình ảnh bên bảng mediacontent
                    return RedirectToAction("Index");
                }
                model.CatalogryList = new SelectList(cms_db.Getnewcatagory(), "ClassificationId", "ClassificationNM");
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Create", "ContentItem", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
          
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateNews")]
        public ActionResult Edit(long id)
        {
            ContentItemViewModels model = new ContentItemViewModels(cms_db.GetObjContentItemById(id));
            model.CatalogryList = new SelectList(cms_db.Getnewcatagory(), "ClassificationId", "ClassificationNM");
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateNews")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ContentItemViewModels model)
        {
            try {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);

                    MediaContent _objnewmedia = cms_db.GetObjMediaContent(model.ImgdefaultId);
                    MediaContent _objoldmedia = cms_db.GetObjDefaultMediaByContentIdvsType(model.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);

                    ContentItem MainModel = cms_db.GetObjContentItemById(model.ContentItemId);
                    MainModel.ContentTitle = model.ContentTitle;
                    MainModel.ContentText = model.ContentText;
                    MainModel.ContentExcerpt = model.ContentExcerpt;
                    MainModel.MetadataDesc = model.MetadataDesc;
                    MainModel.MetadataKeyword = model.MetadataKeyword;
                    MainModel.CategoryId = model.CategoryId;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    if (_objoldmedia != null)
                    {
                        MainModel.MediaUrl = _objnewmedia.FullURL;
                        MainModel.MediaThumb = _objnewmedia.ThumbURL;
                    }
                    int UpdateContent = await cms_db.UpdateContentItem(MainModel);
                    int rs = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                                    (int)EnumCore.ActionType.Update, "Update", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);

                    //int _DeleteRelatedTag = await cms_db.DeleteRelatedTag(MainModel.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
                    //int _DeleteRelatedContent = await this.DeleteRelatedContent(MainModel.ContentItemId);
                    //int SaveRelatedContent = await this.SaveRelateContent(MainModel.ContentItemId, model.related_content);//lưu noi dung liên quan cho tin tức này
                    //int SaveRelatedTag = await this.SaveRelateTag(MainModel.ContentItemId, model.related_tag);//lưu tag liên quan cho tin tức này
                    int UpdateMediaForContent = await this.UpdateMediaForContent(_objnewmedia, _objoldmedia, model.ContentItemId);

                    return RedirectToAction("Index");
                }
                model.CatalogryList = new SelectList(cms_db.Getnewcatagory(), "ClassificationId", "ClassificationNM");
                return View(model);
            
            }catch(Exception e)
            {
                cms_db.AddToExceptionLog("Edit", "ContentItem", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
          
        }



        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult PageInforIndex(int? page, int? catalogry, int? state)
        {
            int pageNum = (page ?? 1);
            ContentItemIndexViewModel model = new ContentItemIndexViewModel();
            IQueryable<ContentItem> tmp = cms_db.GetlstContentItem().Where(s => (s.MicrositeID == null || s.MicrositeID == 0)
                                && s.StateId != (int)EnumCore.StateType.da_xoa && s.ObjTypeId == (int)EnumCore.ObjTypeId.page_infor);
            if (catalogry.HasValue && catalogry.Value != 0)
            {
                tmp = tmp.Where(s => s.CategoryId == catalogry);
                model.ContentCatalogry = catalogry.Value;
            }

            if (state.HasValue && state.Value != 0)
            {
                tmp = tmp.Where(s => s.StateId == state);
                model.ContentState = state.Value;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.lstMainContent = tmp.OrderByDescending(c => c.ContentItemId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.lstContentState = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.state_type);
            model.lstContentCatalogry = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.page_infor);
            return View(model);
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,CreatePageInfor")]
        public ActionResult CreatePageInfor()
        {
            ContentItemViewModels model = new ContentItemViewModels();
            model.CatalogryList = new SelectList(cms_db.GetPageInforCatagory(), "ClassificationId", "ClassificationNM");
            return View(model);
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,CreatePageInfor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePageInfor(ContentItemViewModels model)
        {
            try {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                    MediaContent _objmedia = cms_db.GetObjMediaContent(model.ImgdefaultId);
                    ContentItem MainModel = model._MainObj;
                    MainModel.CommentCount = 0;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.ObjTypeId = (int)EnumCore.ObjTypeId.page_infor;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    MainModel.ViewCount = 0;
                    if (_objmedia != null)
                    {
                        MainModel.MediaUrl = _objmedia.FullURL;
                        MainModel.MediaThumb = _objmedia.ThumbURL;
                    }
                    MainModel.StateId = (int)EnumCore.StateType.cho_phep;
                    MainModel.StateName = this.StateName_Enable;
                    int rs = await cms_db.CreateContentItem(MainModel);
                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "Create", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);
                    //int SaveRelatedContent = await this.SaveRelateContent(MainModel.ContentItemId, model.related_content);//lưu noi dung liên quan cho tin tức này
                    //int SaveRelatedTag = await this.SaveRelateTag(MainModel.ContentItemId, model.related_tag);//lưu tag liên quan cho tin tức này
                    if (_objmedia != null)
                    {
                        int UpdateDefaultMedia = await this.UpdateContentObjForMedia(_objmedia, MainModel.ContentItemId);//cập nhật id tin tức này cho hình ảnh bên bảng mediacontent
                    }
                    return RedirectToAction("PageInforIndex");
                }
                model.CatalogryList = new SelectList(cms_db.GetPageInforCatagory(), "ClassificationId", "ClassificationNM");
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreatePageInfor", "ContentItem", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("PageInforIndex");
            
            }
       
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,UpdatePageInfor")]
        public ActionResult EditPageInfor(long id)
        {
            ContentItemViewModels model = new ContentItemViewModels(cms_db.GetObjContentItemById(id));
            model.CatalogryList = new SelectList(cms_db.GetPageInforCatagory(), "ClassificationId", "ClassificationNM");
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdatePageInfor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPageInfor(ContentItemViewModels model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);

                    MediaContent _objnewmedia = cms_db.GetObjMediaContent(model.ImgdefaultId);
                    MediaContent _objoldmedia = cms_db.GetObjDefaultMediaByContentIdvsType(model.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);

                    ContentItem MainModel = cms_db.GetObjContentItemById(model.ContentItemId);
                    MainModel.ContentTitle = model.ContentTitle;
                    MainModel.ContentText = model.ContentText;
                    MainModel.ContentExcerpt = model.ContentExcerpt;
                    MainModel.MetadataDesc = model.MetadataDesc;
                    MainModel.MetadataKeyword = model.MetadataKeyword;
                    MainModel.CategoryId = model.CategoryId;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    if (_objnewmedia != null)
                    {
                        MainModel.MediaUrl = _objnewmedia.FullURL;
                        MainModel.MediaThumb = _objnewmedia.ThumbURL;
                    }
                    int UpdateContent = await cms_db.UpdateContentItem(MainModel);
                    int rs = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                                    (int)EnumCore.ActionType.Update, "Update", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);

                    //int _DeleteRelatedTag = await cms_db.DeleteRelatedTag(MainModel.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
                    //int _DeleteRelatedContent = await this.DeleteRelatedContent(MainModel.ContentItemId);
                    //int SaveRelatedContent = await this.SaveRelateContent(MainModel.ContentItemId, model.related_content);//lưu noi dung liên quan cho tin tức này
                    //int SaveRelatedTag = await this.SaveRelateTag(MainModel.ContentItemId, model.related_tag);//lưu tag liên quan cho tin tức này
                    int UpdateMediaForContent = await this.UpdateMediaForContent(_objnewmedia, _objoldmedia, model.ContentItemId);

                    return RedirectToAction("PageInforIndex");
                }
                model.CatalogryList = new SelectList(cms_db.GetPageInforCatagory(), "ClassificationId", "ClassificationNM");
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("EditPageInfor", "ContentItem", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("PageInforIndex");
            }
           
        }









        /// <summary>
        /// Cập nhật lai id tin tức cho media
        /// 
        /// </summary>
        /// <param name="_objnewmedia"></param>
        /// <param name="_objoldmedia"></param>
        /// <param name="ContentId"></param>
        /// <returns></returns>
        private async Task<int> UpdateMediaForContent(MediaContent _objnewmedia, MediaContent _objoldmedia, long ContentId)
        {
            if (_objnewmedia != null && _objoldmedia != null)
            {
                if (_objnewmedia.MediaContentId != _objoldmedia.MediaContentId)
                {
                    int _dlmedia = await cms_db.DeleteMediaContent(_objoldmedia.MediaContentId);
                    int UpdateDefaultMedia = await this.UpdateContentObjForMedia(_objnewmedia, ContentId);
                    return (int)EnumCore.Result.action_true;
                }
            }
            if (_objnewmedia != null && _objoldmedia == null)
            {
                int UpdateDefaultMedia = await this.UpdateContentObjForMedia(_objnewmedia, ContentId);
                return (int)EnumCore.Result.action_true;
            }
            if (_objnewmedia == null && _objoldmedia == null)
            {
               // int UpdateDefaultMedia = await this.UpdateContentObjForMedia(_objnewmedia, ContentId);
                return (int)EnumCore.Result.action_true;
            }
            return (int)EnumCore.Result.action_false;
        }

        private async Task<int> UpdateContentObjForMedia(MediaContent Media, long ContentId)
        {
            Media.ContentObjId = ContentId;
            Media.ObjTypeId = (int)EnumCore.ObjTypeId.tin_tuc;
            int rs = await cms_db.UpdateMediaContent(Media);
            return (int)EnumCore.Result.action_true;
        }



        [AdminAuthorize(Roles = "supperadmin,devuser,ApproveNews")]
        public async Task<ActionResult> ChangeState(long id, int state, int ObjType)
        {
            try
            {
                ContentItem MainModel = cms_db.GetObjContentItemById(id);
                MainModel.AprvdUID = long.Parse(User.Identity.GetUserId());
                MainModel.AprvdDT = DateTime.Now;

                MainModel.StateId = state;
                if (MainModel.StateId == (int)EnumCore.StateType.cho_phep)
                    MainModel.StateName = this.StateName_Enable;
                if (MainModel.StateId == (int)EnumCore.StateType.khong_cho_phep)
                    MainModel.StateName = this.StateName_Disable;
                await cms_db.UpdateContentItem(MainModel);
                int rs = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                              (int)EnumCore.ActionType.Update, "ChangeState", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);
                if (ObjType == (int)EnumCore.ObjTypeId.page_infor)
                    return RedirectToAction("PageInforIndex");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("ChangeState", "ContentItemController", e.ToString());
                return RedirectToAction("Index");
            }

        }
        [AdminAuthorize(Roles = "supperadmin,devuser,DeleteNews")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {

                ContentItem MainModel = cms_db.GetObjContentItemById(id);
                if (MainModel != null)
                {
                    //MediaContent _objoldmedia = cms_db.GetObjDefaultMediaByContentIdvsType(id, (int)EnumCore.ObjTypeId.tin_tuc);
                    //if (_objoldmedia != null)
                    //    await cms_db.DeleteMediaContent(_objoldmedia.MediaContentId);
                    //int _DeleteRelatedTag = await cms_db.DeleteRelatedTag(id, (int)EnumCore.ObjTypeId.tin_tuc);
                    //int _DeleteRelatedContent = await this.DeleteRelatedContent(id);
                    int result = await cms_db.DeleteContentItemByObj(MainModel);
                    int rs = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                            (int)EnumCore.ActionType.Update, "Delete", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);

                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Delete", "ContentItemController", e.ToString());
                return RedirectToAction("Index");
            }

        }
        /// <summary>
        /// Lưu nội dung liên quan
        /// </summary>
        /// <param name="ContentItemId">ID của nội dung</param>
        /// <param name="ContentRelatedId">List id của noi dung liên quan</param>
        /// <returns></returns>
        private async Task<int> SaveRelateContent(long ContentItemId, List<int> ContentRelatedId)
        {
            try
            {
                foreach (long RelatedId in ContentRelatedId)
                {
                    RelatedContentItem model = new RelatedContentItem();
                    model.ContentRelatedTypeId = (int)EnumCore.ContentRelatedType.contentitem_contenitem;
                    model.ObjContentItemId = ContentItemId;
                    model.SubjectContentItemId = RelatedId;
                    model.CrtdDT = DateTime.Now;
                    int rs = await cms_db.CreateRelatedContent(model);
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("SaveRelateContent", "ContentItemController", e.ToString());
                return (int)EnumCore.Result.action_false;
            }

        }
        /// <summary>
        /// Lưu từ khoá lien quan cho bài viết
        /// </summary>
        /// <param name="ContentItemId">id bài viết</param>
        /// <param name="lstTagId">list id của các từ khoá</param>
        /// <returns></returns>
        private async Task<int> SaveRelateTag(long ContentItemId, List<int> lstTagId)
        {
            try
            {
                foreach (long TagId in lstTagId)
                {
                    ContentTag model = new ContentTag();
                    model.ObjcontentId = ContentItemId;
                    model.TagId = TagId;
                    model.ObjTypeId = (int)EnumCore.ObjTypeId.tin_tuc;
                    model.CrtdDT = DateTime.Now;
                    int rs = await cms_db.CreateRelatedTag(model);
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("SaveRelateTag", "ContentItemController", e.ToString());
                return (int)EnumCore.Result.action_false;
            }

        }
        private async Task<int> DeleteRelatedContent(long ContentId)
        {
            int rs = await cms_db.RemoveRelatedContentByContentItemId(ContentId, (int)EnumCore.ContentRelatedType.contentitem_contenitem);
            return rs;

        }
        /// <summary>
        /// cập nhật id Obicontent cho hình ảnh
        /// do hình ảnh upload lên server trước khi tạp bài viết nên 
        /// sau khi tạo bài viết phải lấy id của bài viết cập nhật lại cho hình ảnh
        /// </summary>
        /// <param name="Media"></param>
        /// <param name="ContentId"></param>
        /// <returns></returns>
  
        public ActionResult _UploadImagePartial()
        {
            PartialUploadViewModels model = new PartialUploadViewModels();
            return PartialView("_UploadImagePartial", model);
        }
        /// <summary>
        /// partial view từ khoá liên quan cho action tạo bài viết
        /// trả về là file partial view  _RelatedTag file này xài đi xài lại nhiều nơi
        /// </summary>
        /// <returns></returns>
        public ActionResult _RelatedTag()
        {
            List<SelectListViewModels> model = new List<SelectListViewModels>();
            //model.TagSelectList = new SelectList(cms_db.GetTagList(), "TagId", "TagNM");
            return PartialView("_RelatedTag", model);
        }
        /// <summary>
        /// partial view nội dung liên quan cho action tạo bài viết
        /// trả về là file partial view  _RelatedContent file này xài đi xài lại nhiều nơi
        /// </summary>
        /// <returns></returns>
        public ActionResult _RelatedContent()
        {
            List<SelectListViewModels> model = new List<SelectListViewModels>();
            //model.RelatedContent = new SelectList(cms_db.GetContentItemList(), "ContentItemId", "ContentTitle");
            return PartialView("_RelatedContent", model);
        }
        /// <summary>
        /// partial view từ khoá liên quan cho action EDIT bài viết
        /// trả về là file partial view  _RelatedTag file này xài đi xài lại nhiều nơi
        /// </summary>
        /// <returns></returns>
        public ActionResult _RelatedTagE(long id)
        {
            List<SelectListViewModels> model = new List<SelectListViewModels>();
            //List<Tag> lsttag = cms_db.GetlstTagByContentId(id, (int)EnumCore.ObjTypeId.tin_tuc);
            //foreach (Tag _val in lsttag)
            //{
            //    SelectListViewModels _tmp = new SelectListViewModels { };
            //    _tmp.Value = _val.TagId;
            //    _tmp.Name = _val.TagNM;
            //    model.Add(_tmp);
            //}
            return PartialView("_RelatedTag", model);
        }
        /// <summary>
        /// partial view nội dung liên quan cho action EDIT bài viết
        /// trả về là file partial view  _RelatedContent file này xài đi xài lại nhiều nơi
        /// </summary>
        /// <returns></returns>
        public ActionResult _RelatedContentE(long id)
        {
            List<SelectListViewModels> model = new List<SelectListViewModels>();
            List<ContentItem> lstContent = cms_db.GetlstRelatedContentByContentId(id);
            foreach (ContentItem _val in lstContent)
            {
                SelectListViewModels _tmp = new SelectListViewModels { };
                _tmp.Value = _val.ContentItemId;
                _tmp.Name = _val.ContentTitle;
                model.Add(_tmp);
            }
            return PartialView("_RelatedContent", model);
        }
        public ActionResult _UploadImagePartialE(long id)
        {
            PartialUploadViewModels model = new PartialUploadViewModels();
            MediaContent _obj = cms_db.GetObjDefaultMediaByContentIdvsType(id, (int)EnumCore.ObjTypeId.tin_tuc);
            if (_obj != null)
            {
                model.FullUrl = _obj.FullURL;
                model.ThumbUrl = _obj.ThumbURL;
                model.MediaId = _obj.MediaContentId;
            }
            return PartialView("_UploadImagePartial", model);
        }
        /// <summary>
        /// LẤY DANH SÁCH CÁC TỪ KHOÁ
        /// KẾT QUẢ TRẢ VỀ LÀ JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRelatedTag()
        {
            var result = cms_db.GetlstTag().Select(s => new { s.TagId, s.TagNM }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// LẤY DANH SÁCH TẤT CẢ CÁC BÀI VIẾT
        /// KẾT QUẢ TRẢ VỀ LÀ JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult RelatedContent()
        {
            var result = cms_db.GetlstContentItem().Select(s => new { s.ContentItemId, s.ContentTitle }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}