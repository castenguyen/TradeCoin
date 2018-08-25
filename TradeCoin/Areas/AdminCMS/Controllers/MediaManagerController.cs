using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.Extension;
using DataModel.DataViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using System.IO;
using Microsoft.AspNet.SignalR;
using CMSPROJECT.Hubs;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class MediaManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            List<AlbumMediaViewModels> model = new List<AlbumMediaViewModels>();
            List<Classification> _lstAlbum = new List<Classification>();
            _lstAlbum = cms_db.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.hinh_anh).OrderBy(c => c.DisplayOrder).ToList();
            foreach (Classification obj in _lstAlbum)
            {
                AlbumMediaViewModels _val = new AlbumMediaViewModels();
                _val.Album = obj;
                _val.ImgCrtdUser = "/Areas/AdminCMS/dist/img/user6-128x128.jpg";
                _val.ImgAvatars = "/Areas/AdminCMS/dist/img/photo1.png";
                _val.NbrPic = 35;
                model.Add(_val);
            }
            return View(model.ToPagedList(pageNum, 10));
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult DetailAllbum(int id)
        {
            try
            {
                DetailAlbumViewModel model = new DetailAlbumViewModel();
                model.Album = cms_db.GetObjClasscifiById(id);
                model.lstImage = cms_db.GetLstMediaContentByAlbumId(id);
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DetailAllbum", "MediaManager", e.ToString());
                return RedirectToAction("ErrorExeption", "Extension");
            }

        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        [HttpPost]
        public async Task<ActionResult> DetailAllbum(HttpPostedFileBase[] files, int _abid)
        {
            try
            {
                foreach (HttpPostedFileBase file in files)
                {
                    ImageUploadViewModel item = new ImageUploadViewModel();
                    item = cms_db.UploadHttpPostedFileBase(file);
                    MediaContentViewModels _Media = new MediaContentViewModels();
                    _Media.Filename = item.ImageName;
                    _Media.FullURL = item.ImageUrl;
                    _Media.ContentObjId = _abid;
                    _Media.ObjTypeId = (int)EnumCore.ObjTypeId.album;
                    _Media.ViewCount = 0;
                    _Media.CrtdDT = DateTime.UtcNow;
                    _Media.MediaContentSize = file.ContentLength;
                    _Media.ThumbURL = item.ImageThumbUrl;
                    _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
                    _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh;
                    long _ImgId = await cms_db.AddNewMediaContent(_Media);
                }
                DetailAlbumViewModel model = new DetailAlbumViewModel();
                model.Album = cms_db.GetObjClasscifiById(_abid);
                model.lstImage = cms_db.GetLstMediaContentByAlbumId(_abid);
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DetailAllbum", "MediaManager", e.ToString());
                return RedirectToAction("ErrorExeption", "Extension");
            }
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateAllbum")]

        public ActionResult CreateAlbum()
        {
            return View();
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateAllbum")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAlbum(ClassificationViewModels model)
        {
            Classification _ObjClassifi = model._ModelObj;
            _ObjClassifi.ClassificationSchemeId = (int)EnumCore.ClassificationScheme.hinh_anh;
            _ObjClassifi.FriendlyURL = this.GetFriendlyUrlFromstring(_ObjClassifi.ClassificationNM, -1);
            _ObjClassifi.ClassificationCD = model.ClassificationNM.ToFriendlyURL();
            _ObjClassifi.CrtdUID = long.Parse(User.Identity.GetUserId());
            _ObjClassifi.CrtdDT = DateTime.Now;
            _ObjClassifi.DisplayOrder = this.GetMaxDisplayOrderClassifi((int)EnumCore.ClassificationScheme.hinh_anh, null);
            int rs = await cms_db.Classifi(_ObjClassifi);
            if (rs == (int)EnumCore.Result.action_true)
                return RedirectToAction("Index");
            return RedirectToAction("CreateAlbum");
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,DeleteAllbum")]
        public async Task<ActionResult> DeleteAlBum(int abid)
        {
            List<MediaContent> model = new List<MediaContent>();
            model = cms_db.GetLstMediaContentByAlbumId(abid);
            foreach (MediaContent _item in model)
            {
                int rs = await cms_db.DeleteMediaContent(_item.MediaContentId);
            }
            int result = await cms_db.DeleteClass(abid);
            return RedirectToAction("Index");
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,DeleteAllbum")]

        public async Task<ActionResult> DeleteImage(long id, int abid)
        {
            try
            {
                int rs = await cms_db.DeleteMediaContent(id);
                return RedirectToAction("DetailAllbum", new { id = abid });
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DetailAllbum", "MediaManager", e.ToString());
                return RedirectToAction("ErrorExeption", "Extension");
            }

        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateAllbum")]
        public ActionResult Edit(long id)
        {
            MediaContent model = new MediaContent();
            model = cms_db.GetObjMediaContent(id);
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateAllbum")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MediaContent model)
        {
            if (ModelState.IsValid)
            {
                MediaContent mainmodel = new MediaContent();
                mainmodel = cms_db.GetObjMediaContent(model.MediaContentId);
                mainmodel.AlternativeText = model.AlternativeText;
                mainmodel.Caption = model.Caption;
                mainmodel.MediaDesc = model.MediaDesc;
                mainmodel.MetadataDesc = model.MetadataDesc;
                mainmodel.MetadataKeyword = model.MetadataKeyword;
                int rs = await cms_db.UpdateMediaContent(mainmodel);
                return View(mainmodel);
            }
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult BannerManager()
        {
            List<MediaContent> model = new List<MediaContent>();
            model = cms_db.GetLstMediaContent().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.banner).ToList();
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateBanner")]
        public ActionResult CreateBanner()
        {
            BannerViewModels model = new BannerViewModels();
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateBanner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBanner(BannerViewModels model)
        {
            if (ModelState.IsValid)
            {

                MediaContent MainObj = cms_db.GetObjMediaContent(model.ImgdefaultId);
                if (MainObj != null)
                {
                    MainObj.Filename = model.Filename;
                    MainObj.CrtdUID = long.Parse(User.Identity.GetUserId());
                    MainObj.CrtdDT = DateTime.Now;
                    MainObj.AlternativeText = model.AlternativeText;
                    MainObj.Caption = model.Caption;
                    MainObj.EXIFInfo = model.EXIFInfo;
                    MainObj.MediaDesc = model.MediaDesc;
                    MainObj.MetadataDesc = model.MetadataDesc;
                    MainObj.MetadataKeyword = model.MetadataKeyword;
                    MainObj.LinkHref = model.LinkHref;
                    MainObj.ObjTypeId = (int)EnumCore.ObjTypeId.banner;
                    int rs = await cms_db.UpdateMediaContent(MainObj);
                }
                return RedirectToAction("BannerManager");
            }
            return RedirectToAction("BannerManager");
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateBanner")]
        public ActionResult EditBanner(long id)
        {
            BannerViewModels model = new BannerViewModels(cms_db.GetObjMediaContent(id));
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateBanner")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBanner(BannerViewModels model)
        {
            if (ModelState.IsValid)
            {
                MediaContent MainObj = cms_db.GetObjMediaContent(model.MediaContentId);
                if (MainObj != null)
                {

                    MainObj.Filename = model.Filename;
                    MainObj.AlternativeText = model.AlternativeText;
                    MainObj.Caption = model.Caption;
                    MainObj.EXIFInfo = model.EXIFInfo;
                    MainObj.MediaDesc = model.MediaDesc;
                    MainObj.MetadataDesc = model.MetadataDesc;
                    MainObj.LinkHref = model.LinkHref;
                    MainObj.MetadataKeyword = model.MetadataKeyword;
                    int rs = await cms_db.UpdateMediaContent(MainObj);
                }
                return RedirectToAction("BannerManager");
            }
            return RedirectToAction("BannerManager");
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,DeleteBanner")]
        public async Task<ActionResult> DeleteBanner(long id)
        {
            try
            {
                int rsd = await cms_db.DeleteDisplayContentByContentObj(id, (int)EnumCore.ObjTypeId.banner);
                int rs = await cms_db.DeleteMediaContent(id);
                return RedirectToAction("BannerManager");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DetailAllbum", "MediaManager", e.ToString());
                return RedirectToAction("ErrorExeption", "Extension");
            }

        }

        public ActionResult _UploadImagePartial()
        {
            PartialUploadViewModels model = new PartialUploadViewModels();
            return PartialView("_UploadImagePartial", model);
        }
        public ActionResult _UploadImagePartialE(long id)
        {
            PartialUploadViewModels model = new PartialUploadViewModels();
            MediaContent _obj = cms_db.GetObjMediaContent(id);
            if (_obj != null)
            {
                model.FullUrl = _obj.FullURL;
                model.ThumbUrl = _obj.ThumbURL;
                model.MediaId = _obj.MediaContentId;
            }
            return PartialView("_UploadImagePartial", model);
        }















        [AdminAuthorize(Roles = "supperadmin,devuser,CreateVideo")]
        public ActionResult VideoManager()
        {
            List<MediaContent> model = new List<MediaContent>();
            model = cms_db.GetLstMediaContent().Where(s => s.MediaTypeId == (int)EnumCore.ObjTypeId.video).ToList();
            return View(model);
        }

        public ActionResult ListNotificationNewKeo()
        {
            try
            {
                List<MediaContent> model = new List<MediaContent>();
                model = cms_db.GetLstMediaContent().Where(s => s.MediaTypeId == (int)EnumCore.ObjTypeId.video).ToList();

                var result = new
                {
                    TotalRows = model.Count(),
                    Rows = model.Select(x => new
                    {
                        MediaContentId = x.MediaContentId,
                        Filename = x.Filename,
                        Caption = x.Caption,
                    })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,CreateVideo")]
        public ActionResult CreateVideo()
        {
            VideoViewModels model = new VideoViewModels();
         
            model.lstPackage = cms_db.GetObjSelectListPackage();
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateVideo")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateVideo(VideoViewModels model, HttpPostedFileBase Default_files)
        {
            if (ModelState.IsValid)
            {
                MediaContent MainModel = model.objMediaContent;
       
                if (MainModel != null)
                {
                    MainModel.Filename = model.Filename;
                    MainModel.CrtdUID = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.AlternativeText = model.AlternativeText;
                    MainModel.Caption = model.Caption;
                    MainModel.EXIFInfo = model.EXIFInfo;
                    MainModel.MediaDesc = model.MediaDesc;
                    MainModel.MetadataDesc = model.MetadataDesc;
                    MainModel.MetadataKeyword = model.MetadataKeyword;
                    MainModel.LinkHref = model.LinkHref;
                    MainModel.MediaTypeId = (int)EnumCore.ObjTypeId.video;
                    MainModel.ObjTypeId = (int)EnumCore.ObjTypeId.video;
                    await cms_db.AddObjMediaContent(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForVideo(Default_files, MainModel.MediaContentId);
                        int rsup = await this.UpdateImageUrlForVideo(rsdf, MainModel);
                    }
                  
                    int SaveTickerPackage = this.SaveVideoPackage(model.lstTickerPackage, MainModel);
                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "Create", MainModel.MediaContentId, MainModel.Filename, "MediaManage", (int)EnumCore.ObjTypeId.video);

                    var context = GlobalHost.ConnectionManager.GetHubContext<NotifiHub>();
                    context.Clients.All.notificationNewVideo();
                }
                return RedirectToAction("VideoManager");
            }
            return RedirectToAction("VideoManager");
        }

        private async Task<MediaContentViewModels> SaveDefaultImageForVideo(HttpPostedFileBase file, long Videoid)
        {
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.video
                    && s.MediaTypeId == (int)EnumCore.mediatype.hinh_anh_dai_dien && s.ContentObjId == Videoid).FirstOrDefault();
            if (CurrentMediaId != null)
            {
                int rs = await cms_db.DeleteMediaContent(CurrentMediaId.MediaContentId);
            }
            ImageUploadViewModel item = new ImageUploadViewModel();
            item = cms_db.UploadHttpPostedFileBase(file);
            MediaContentViewModels _Media = new MediaContentViewModels();
            _Media.Filename = item.ImageName;
            _Media.FullURL = item.ImageUrl;
            _Media.ContentObjId = Videoid;
            _Media.ObjTypeId = (int)EnumCore.ObjTypeId.video;
            _Media.ViewCount = 0;
            _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh_dai_dien;
            _Media.CrtdDT = DateTime.UtcNow;
            _Media.MediaContentSize = file.ContentLength;
            _Media.ThumbURL = item.ImageThumbUrl;
            _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
            await cms_db.AddNewMediaContent(_Media);
            return _Media;

        }

        private async Task<int> UpdateImageUrlForVideo(MediaContentViewModels ImageObj, MediaContent ContentObj)
        {
            try {
                ContentObj.FullURL = ImageObj.FullURL;
                ContentObj.ThumbURL = ImageObj.ThumbURL;
                return await cms_db.UpdateMediaContent(ContentObj);
            }
            catch (Exception e) {
                cms_db.AddToExceptionLog("UpdateImageUrlForVideo", "MediaManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
               
                return 0;
            }
           
        }

        private int SaveVideoPackage(long[] model, MediaContent Video)
        {
            try
            {
                int dl = cms_db.DeleteContentPackage(Video.MediaContentId, (int)EnumCore.ObjTypeId.video);
                foreach (int _val in model)
                {
                    ContentPackage tmp = new ContentPackage();
                    tmp.ContentId = Video.MediaContentId;
                    tmp.ContentName = Video.Filename;
                    tmp.ContentType = (int)EnumCore.ObjTypeId.video;
                    tmp.PackageId = _val;
                    tmp.PackageName = cms_db.GetPackageName(_val);
                    cms_db.CreateContentPackage(tmp);
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("SaveVideoPackage", "MediaManage", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return (int)EnumCore.Result.action_false;
            }
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateVideo")]
        public ActionResult EditVideo(long id)
        {
            VideoViewModels model = new VideoViewModels(cms_db.GetObjMediaContent(id));
            model.lstPackage = cms_db.GetObjSelectListPackage();
            model.lstTickerPackage = cms_db.GetlstContentPackage(model.MediaContentId, (int)EnumCore.ObjTypeId.video);
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateVideo")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditVideo(VideoViewModels model, HttpPostedFileBase Default_files)
        {
            if (ModelState.IsValid)
            {
                MediaContent MainModel = cms_db.GetObjMediaContent(model.MediaContentId);
                if (MainModel != null)
                {
                    MainModel.Filename = model.Filename;
                    MainModel.AlternativeText = model.AlternativeText;
                    MainModel.Caption = model.Caption;
                    MainModel.EXIFInfo = model.EXIFInfo;
                    MainModel.MediaDesc = model.MediaDesc;
                    MainModel.MetadataDesc = model.MetadataDesc;
                    MainModel.MetadataKeyword = model.MetadataKeyword;
                    MainModel.LinkHref = model.LinkHref;
                    int rs = await cms_db.UpdateMediaContent(MainModel);

                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForVideo(Default_files, MainModel.MediaContentId);
                        int rsup = await this.UpdateImageUrlForVideo(rsdf, MainModel);
                    }

                    int SaveTickerPackage = this.SaveVideoPackage(model.lstTickerPackage, MainModel);
                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "EditVideo", MainModel.MediaContentId, MainModel.Filename, "MediaManage", (int)EnumCore.ObjTypeId.video);


                }
                return RedirectToAction("VideoManager");
            }
            return RedirectToAction("VideoManager");
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,DeleteVideo")]
        public async Task<ActionResult> DeleteVideo(long id)
        {
            try
            {
                int rsd = await cms_db.DeleteDisplayContentByContentObj(id, (int)EnumCore.ObjTypeId.video);
                int rs = await cms_db.DeleteMediaContent(id);
                return RedirectToAction("VideoManager");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("DeleteVideo", "MediaManager", e.ToString());
                return RedirectToAction("ErrorExeption", "Extension");
            }

        }

        [HttpPost]
        public JsonResult UploadVideo()
        {
            try
            {
                string uploadPath = Server.MapPath("~/Video");
                // Create new folder that does not exist yet
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var resultList = new List<ViewDataUploadFilesResult>();
                var httpRequest = HttpContext.Request;
                foreach (String inputTagName in httpRequest.Files)
                {
                    var headers = httpRequest.Headers;
                    var file = httpRequest.Files[inputTagName];
                    if (string.IsNullOrEmpty(headers["X-File-Name"]))
                    {
                        for (int i = 0; i < httpRequest.Files.Count; i++)
                        {
                            ViewDataUploadFilesResult item = new ViewDataUploadFilesResult();
                            var file1 = httpRequest.Files[i];
                            var ReplaceFileName = file.FileName.Replace(" ", string.Empty);
                            item.FileName = Path.GetFileName(DateTime.Now.ToString("HH-mm-ss") + ReplaceFileName);
                            item.FilePath = Path.Combine(uploadPath, item.FileName);
                            item.type = Path.GetExtension(item.FileName);
                            file.SaveAs(item.FilePath);
                            resultList.Add(item);
                            return Json(item);
                        }
                    }
                    else
                    {
                    }
                }
                return Json(resultList);
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return Json("Error");
            }
        }

        public class ViewDataUploadFilesResult
        {
            public string FileName { get; set; }
            public int size { get; set; }
            public string type { get; set; }
            public string FilePath { get; set; }
            public string deleteUrl { get; set; }
            public string thumbnailUrl { get; set; }
            public string deleteType { get; set; }
        }
    }
}