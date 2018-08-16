using CMSPROJECT.Areas.AdminCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.Extension;
using DataModel.DataViewModel;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class ExtensionController : CoreBackEnd
    {

        //Chuyển sang friendlyurl
        //classification
        [HttpPost]
        public JsonResult GetFriendlyurlFromTitle(string friendlyurl, int objtype)
        {
            var result = this.GetFriendlyUrlFromstring(friendlyurl, objtype);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UploadOneImgReturnUrl()
        {
            try
            {
                List<object> _kq = new List<object>();
                if (Request.Files.Count > 0)
                {
                    ImageUploadViewModel _ImageUpload = await cms_db.UploadOneImageToServer(Request);
                    string prefix = "\\";
                    _kq.Add(new { id = _ImageUpload.MediaContentId, url = prefix + _ImageUpload.ImageUrl, thumb = prefix + _ImageUpload.ImageThumbUrl });
                }
                return Json(_kq, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog(e.ToString());
                return Json((int)EnumCore.Result.action_false);
            }
        }


        [HttpPost]
        public async Task<JsonResult> UploadImgForUserReturnUrl()
        {
            try
            {
                List<object> _kq = new List<object>();
                if (Request.Files.Count > 0)
                {
                    ImageUploadViewModel _ImageUpload = await cms_db.UploadOneImageToServer(Request);

                    string prefix = "\\";
                    _kq.Add(new { id = _ImageUpload.MediaContentId, url = prefix + _ImageUpload.ImageUrl, thumb = prefix + _ImageUpload.ImageThumbUrl });
                }
                return Json(_kq, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog(e.ToString());
                return Json((int)EnumCore.Result.action_false);
            }
        }


        [HttpPost]
        public async Task<string> CK_ImgUpload()
        {
            try
            {
                List<object> _kq = new List<object>();
                List<ImageUploadViewModel> _lstImageUpload = await cms_db.UploadImageServerForCK(Request);
                ImageUploadViewModel _ImageUpload = _lstImageUpload.First();
                _ImageUpload.ImageUrl ="/" + _ImageUpload.ImageUrl;
                string result = "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction(1, '" + _ImageUpload.ImageUrl+ "', 'Upload thành công');</script>";
                return result;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog(e.ToString());
                return "upload ko thành công";
            }
        }
        public ActionResult ErrorExeption()
        {
            return View();
        }
        public ActionResult AlertPage( string AlertString ,string link,int? type)
        {
            if (!type.HasValue)
                type = (int)EnumCore.AlertPageType.lockscreen;
            ///lockscreen
            if (type == (int)EnumCore.AlertPageType.lockscreen)
            {
                AlertPageViewModel model = new AlertPageViewModel();
                model.AlertString = AlertString;
                model.AlertLink = link;
                return View("AlertPage", model);
            }else
            {
                AlertPageViewModel model = new AlertPageViewModel();
                model.AlertString = AlertString;
                model.AlertLink = link;
                return View("AlertPageFullScreen", model);
            }
           
        }
        public ActionResult ConfirmDelete(long id, string controller, string action)
        {
            ConfirmDeleteViewModel model = new ConfirmDeleteViewModel();
            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> DeleteDetailImage(long MediaContentId, int objtype)
        {
            int result =await cms_db.DeleteMediaContent(MediaContentId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }




    }
}