using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataViewModel;
using DataModel.DataEntity;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class DisplayManagerController : CoreBackEnd
    {
       [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Index()
        {
            return View();
        }
          [AdminAuthorize(Roles = "supperadmin,devuser,CreateDisplay")]
        public ActionResult Create(int? DisplayTypeId)
        {
            CreateDisplayTypeViewModel model = new CreateDisplayTypeViewModel();
            model.LstDisplayContent = this.GetlstDisplayTypeViewModels(DisplayTypeId);
            model.LstContent = new SelectList(cms_db.GetContentListForSelectlist(), "ContentItemId", "ContentTitle");
            model.LstProduct = new SelectList(cms_db.GetProductListForSelectlist(), "ProductId", "ProductName");
            model.LstBanner = new SelectList(cms_db.GetBannerListForSelectlist(), "MediaContentId", "Filename");
            model.LstDisplayType = new SelectList(cms_db.GetClassifiListForSelectlist((int)EnumCore.ClassificationScheme.display_type),
                                                                                                        "ClassificationId", "ClassificationNM");

            return View(model);
        }
         [AdminAuthorize(Roles = "supperadmin,devuser,CreateDisplay")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDisplayTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                DisplayContent MainObj = new DisplayContent();
                if (model.ObjTypeId == (int)EnumCore.ObjTypeId.tin_tuc)
                {
                    MainObj.ContentId = model.ContentId;
                }
                if (model.ObjTypeId == (int)EnumCore.ObjTypeId.san_pham)
                {
                    MainObj.ContentId = model.ProductId;
                }
                if (model.ObjTypeId == (int)EnumCore.ObjTypeId.banner)
                {
                    MainObj.ContentId = model.BannerId;
                }
                MainObj.ObjType = model.ObjTypeId;
                MainObj.CrtdDT = DateTime.Now;
                MainObj.CrtdUID = long.Parse(User.Identity.GetUserId());
                MainObj.DisplayType = model.DisplayTypeId;
                MainObj.DisplayOrder = await cms_db.GetMaxOrderDisplayForDisplaycontent(model.DisplayTypeId) + 1;
                MainObj.StartDT = this.SpritDateTime(model.Datetime)[0];
                MainObj.EndDT = this.SpritDateTime(model.Datetime)[1];
                int rs = await cms_db.CreateDisplayContent(MainObj);
            }                                                
            return RedirectToAction("Create");
        }
          [AdminAuthorize(Roles = "supperadmin,devuser,DeleteDisplay")]
        public async Task<ActionResult> Delete(int DisplayContentId)
        {
            try
            {
                DisplayContent model = new DisplayContent();
                model = await cms_db.GetObjDisplayContent(DisplayContentId);
                if (model != null)
                    await cms_db.DeleteDisplayContent(model);
                return RedirectToAction("Create");
            }
            catch (Exception e)
            {
                return RedirectToAction("Create");
            }
        }
        private string GetContentNameByDisplayContent(DisplayContent model)
        {
            string name = "";
            if (model.ObjType == (int)EnumCore.ObjTypeId.tin_tuc)
            {
                ContentItem tmp = cms_db.GetObjContentItemById(model.ContentId.Value);
                name = tmp.ContentTitle;
            }
            if (model.ObjType == (int)EnumCore.ObjTypeId.san_pham)
            {
                Product tmp = cms_db.GetObjProductById(model.ContentId.Value);
                name = tmp.ProductName;
            }
            if (model.ObjType == (int)EnumCore.ObjTypeId.banner)
            {
              //  MediaContent tmp = cms_db.GetObjMedia(model.ContentId.Value, (int)EnumCore.ObjTypeId.banner);
                MediaContent tmp = cms_db.GetObjMedia().Where(s => s.MediaContentId == model.ContentId.Value).FirstOrDefault();
                name = tmp.Filename;
            }
            return name;
        }
        private List<DisplayTypeViewModels> GetlstDisplayTypeViewModels(int? type)
        {
            List<DisplayTypeViewModels> lsttmp = new List<DisplayTypeViewModels>();
            IQueryable<DisplayContent> tmp = cms_db.GetlstDisplayContent();
            if (type.HasValue)
                tmp=tmp.Where(s => s.DisplayType == type.Value);
            List<DisplayContent> lstDisplay = tmp.ToList();
            foreach (DisplayContent _val in lstDisplay)
            {
                DisplayTypeViewModels item = new DisplayTypeViewModels(_val);
                item.ContentName = this.GetContentNameByDisplayContent(_val);
                lsttmp.Add(item);
            }
            return lsttmp;
        }

        private DateTime[] SpritDateTime(string datetime)
        {
            string[] words = datetime.Split('-');
            DateTime[] model = new DateTime[] { Convert.ToDateTime(words[0]), Convert.ToDateTime(words[1]) };
            return model;
        }
    }
}