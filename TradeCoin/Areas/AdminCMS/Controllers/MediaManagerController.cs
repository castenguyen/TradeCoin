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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;

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
            model = cms_db.GetLstMediaContent().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.video).ToList();
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateVideo")]
        public ActionResult CreateVideo()
        {
            VideoViewModels model = new VideoViewModels();
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateVideo")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateVideo(VideoViewModels model)
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
                    MainObj.ObjTypeId = (int)EnumCore.ObjTypeId.video;
                    int rs = await cms_db.UpdateMediaContent(MainObj);
                }
                return RedirectToAction("VideoManager");
            }
            return RedirectToAction("VideoManager");
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateVideo")]
        public ActionResult EditVideo(long id)
        {
            VideoViewModels model = new VideoViewModels(cms_db.GetObjMediaContent(id));
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateVideo")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditVideo(VideoViewModels model)
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
    }
}