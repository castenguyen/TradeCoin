using CMSPROJECT.Areas.AdminCMS.Core;
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
using Microsoft.AspNet.SignalR;
using CMSPROJECT.Hubs;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class ContentItemController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,CreateNews")]
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

        public ActionResult ListNotificationNewPost()
        {
            try
            {
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    ContentItemIndexViewModel model = new ContentItemIndexViewModel();
                    IQueryable<ContentItem> tmp = cms_db.GetlstContentItem().Where(s => (s.MicrositeID == null || s.MicrositeID == 0) && s.StateId != (int)EnumCore.StateType.da_xoa && s.ObjTypeId == (int)EnumCore.ObjTypeId.tin_tuc);
                    model.lstMainContent = tmp.OrderByDescending(c => c.ContentItemId).ToPagedList(1, 20);

                    var result = new
                    {
                        TotalRows = model.lstMainContent.Count(),
                        Rows = model.lstMainContent.Select(x => new
                        {
                            CrtdUserName = x.CrtdUserName,
                            ContentTitle = x.ContentTitle,
                            ContentItemId = x.ContentItemId
                        })
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    try
                    {
                        List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;

                        MemberFrontEndViewModel model = new MemberFrontEndViewModel();
                        List<ContentItemViewModels> lstmpNews = new List<ContentItemViewModels>();
                        List<ContentItem> lstNews = cms_db.GetListContentItemByUser(long.Parse(User.Identity.GetUserId()), (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_News_In_Home);
                        foreach (ContentItem _val in lstNews)
                        {
                            ContentItemViewModels tmp = new ContentItemViewModels(_val);
                            tmp.lstNewsContentPackage = cms_db.GetlstObjContentPackage(tmp.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
                            lstmpNews.Add(tmp);
                        }
                        var result = new
                        {
                            TotalRows = lstNews.Count(),
                            Rows = lstNews.Select(x => new
                            {
                                MicrositeID = x.MicrositeID,
                                CrtdUserName = x.CrtdUserName,
                                ContentTitle = x.ContentTitle,
                                ContentItemId = x.ContentItemId
                            })
                        };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {

                        return Json("", JsonRequestBehavior.AllowGet);
                    }
                    
                }
               
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
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
            model.lstPackage = cms_db.GetObjSelectListPackage();
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
        public async Task<ActionResult> Create(ContentItemViewModels model, HttpPostedFileBase Default_files)
        {
            try {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                    ContentItem MainModel = model._MainObj;
                    MainModel.CommentCount = 0;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.ObjTypeId = (int)EnumCore.ObjTypeId.tin_tuc;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    MainModel.ViewCount = 0;
                    MainModel.StateId = (int)EnumCore.StateType.cho_phep;
                    MainModel.StateName = this.StateName_Enable;
                    int rs = await cms_db.CreateContentItem(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForContentItem(Default_files, MainModel.ContentItemId);
                        int rsup = await this.UpdateImageUrlForContentItem(rsdf, MainModel);
                    }


                    int SaveTickerPackage = this.SaveContentItemPackage(model.lstTickerPackage, MainModel);
                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "Create", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);
                    //int SaveRelatedContent = await this.SaveRelateContent(MainModel.ContentItemId, model.related_content);//lưu noi dung liên quan cho tin tức này
                    //int SaveRelatedTag = await this.SaveRelateTag(MainModel.ContentItemId, model.related_tag);//lưu tag liên quan cho tin tức này
                    //int UpdateDefaultMedia = await this.UpdateContentObjForMedia(_objmedia, MainModel.ContentItemId);//cập nhật id tin tức này cho hình ảnh bên bảng mediacontent

                    var context = GlobalHost.ConnectionManager.GetHubContext<NotifiHub>();
                    context.Clients.All.notificationNewPost();
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
            model.lstPackage = cms_db.GetObjSelectListPackage();
            model.lstTickerPackage = cms_db.GetlstTickerPackage(model.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateNews")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ContentItemViewModels model, HttpPostedFileBase Default_files)
        {
            try {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                    ContentItem MainModel = cms_db.GetObjContentItemById(model.ContentItemId);
                    MainModel.ContentTitle = model.ContentTitle;
                    MainModel.ContentText = model.ContentText;
                    MainModel.ContentExcerpt = model.ContentExcerpt;
                    MainModel.MetadataDesc = model.MetadataDesc;
                    MainModel.MetadataKeyword = model.MetadataKeyword;
                    MainModel.CategoryId = model.CategoryId;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    int UpdateContent = await cms_db.UpdateContentItem(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForContentItem(Default_files, MainModel.ContentItemId);
                        int rsup = await this.UpdateImageUrlForContentItem(rsdf, MainModel);
                    }
                    int SaveTickerPackage = this.SaveContentItemPackage(model.lstTickerPackage, MainModel);


                    int rs = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                                    (int)EnumCore.ActionType.Update, "Update", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);

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


        private async Task<MediaContentViewModels> SaveDefaultImageForContentItem(HttpPostedFileBase file, long ContentItemId)
        {
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.tin_tuc
                    && s.MediaTypeId == (int)EnumCore.mediatype.hinh_anh_dai_dien && s.ContentObjId == ContentItemId).FirstOrDefault();
            if (CurrentMediaId != null)
            {
                int rs = await cms_db.DeleteMediaContent(CurrentMediaId.MediaContentId);
            }
            ImageUploadViewModel item = new ImageUploadViewModel();
            item = cms_db.UploadHttpPostedFileBase(file);
            MediaContentViewModels _Media = new MediaContentViewModels();
            _Media.Filename = item.ImageName;
            _Media.FullURL = item.ImageUrl;
            _Media.ContentObjId = ContentItemId;
            _Media.ObjTypeId = (int)EnumCore.ObjTypeId.tin_tuc;
            _Media.ViewCount = 0;
            _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh_dai_dien;
            _Media.CrtdDT = DateTime.UtcNow;
            _Media.MediaContentSize = file.ContentLength;
            _Media.ThumbURL = item.ImageThumbUrl;
            _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
            await cms_db.AddNewMediaContent(_Media);
            return _Media;

        }
        private async Task<int> UpdateImageUrlForContentItem(MediaContentViewModels ImageObj, ContentItem ContentObj)
        {
            ContentObj.MediaUrl = ImageObj.FullURL;
            ContentObj.MediaThumb = ImageObj.ThumbURL;
            return await cms_db.UpdateContentItem(ContentObj);
        }

        private int SaveContentItemPackage(long[] model, ContentItem ContentObj)
        {
            try
            {
                int dl = cms_db.DeleteContentPackage(ContentObj.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
                foreach (int _val in model)
                {
                    ContentPackage tmp = new ContentPackage();
                    tmp.ContentId = ContentObj.ContentItemId;
                    tmp.ContentName = ContentObj.ContentTitle;
                    tmp.ContentType = (int)EnumCore.ObjTypeId.tin_tuc;
                    tmp.PackageId = _val;
                    tmp.PackageName = cms_db.GetPackageName(_val);

                    cms_db.CreateContentPackage(tmp);
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("SaveContentItemPackage", "ContentItem", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return (int)EnumCore.Result.action_false;
            }
        }








        [AdminAuthorize(Roles = "supperadmin,devuser,CreatePageInfor")]
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
        public async Task<ActionResult> CreatePageInfor(ContentItemViewModels model,HttpPostedFileBase Default_files)
        {
            try {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                    ContentItem MainModel = model._MainObj;
                    MainModel.CommentCount = 0;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.ObjTypeId = (int)EnumCore.ObjTypeId.page_infor;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    MainModel.ViewCount = 0;
                    MainModel.StateId = (int)EnumCore.StateType.cho_phep;
                    MainModel.StateName = this.StateName_Enable;
                    int rs = await cms_db.CreateContentItem(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForPageinfo(Default_files, MainModel.ContentItemId);
                        int rsup = await this.UpdateImageUrlForContentItem(rsdf, MainModel);
                    }

                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "CreatePageInfor", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);
                 
               
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
        public async Task<ActionResult> EditPageInfor(ContentItemViewModels model, HttpPostedFileBase Default_files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                    ContentItem MainModel = cms_db.GetObjContentItemById(model.ContentItemId);
                    MainModel.ContentTitle = model.ContentTitle;
                    MainModel.ContentText = model.ContentText;
                    MainModel.ContentExcerpt = model.ContentExcerpt;
                    MainModel.MetadataDesc = model.MetadataDesc;
                    MainModel.MetadataKeyword = model.MetadataKeyword;
                    MainModel.CategoryId = model.CategoryId;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    int UpdateContent = await cms_db.UpdateContentItem(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForPageinfo(Default_files, MainModel.ContentItemId);
                        int rsup = await this.UpdateImageUrlForPageinfo(rsdf, MainModel);
                    }

                    int rs = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                                    (int)EnumCore.ActionType.Update, "EditPageInfor", MainModel.ContentItemId, MainModel.ContentTitle, "ContentItem", (int)EnumCore.ObjTypeId.tin_tuc);

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


        private async Task<MediaContentViewModels> SaveDefaultImageForPageinfo(HttpPostedFileBase file, long ContentItemId)
        {
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.page_infor
                    && s.MediaTypeId == (int)EnumCore.mediatype.hinh_anh_dai_dien && s.ContentObjId == ContentItemId).FirstOrDefault();
            if (CurrentMediaId != null)
            {
                int rs = await cms_db.DeleteMediaContent(CurrentMediaId.MediaContentId);
            }
            ImageUploadViewModel item = new ImageUploadViewModel();
            item = cms_db.UploadHttpPostedFileBase(file);
            MediaContentViewModels _Media = new MediaContentViewModels();
            _Media.Filename = item.ImageName;
            _Media.FullURL = item.ImageUrl;
            _Media.ContentObjId = ContentItemId;
            _Media.ObjTypeId = (int)EnumCore.ObjTypeId.page_infor;
            _Media.ViewCount = 0;
            _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh_dai_dien;
            _Media.CrtdDT = DateTime.UtcNow;
            _Media.MediaContentSize = file.ContentLength;
            _Media.ThumbURL = item.ImageThumbUrl;
            _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
            await cms_db.AddNewMediaContent(_Media);
            return _Media;

        }

        private async Task<int> UpdateImageUrlForPageinfo(MediaContentViewModels ImageObj, ContentItem ContentObj)
        {
            ContentObj.MediaUrl = ImageObj.FullURL;
            ContentObj.MediaThumb = ImageObj.ThumbURL;
            return await cms_db.UpdateContentItem(ContentObj);
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