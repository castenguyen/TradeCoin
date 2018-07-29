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

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using PagedList;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    [AdminAuthorize]
    public class ProductManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Index(int? page, int? ProductState, int? ProductCatalogry, string FillterSKU, string FillterProductCD, string FillterProductName)
        {
            int pageNum = (page ?? 1);
            ProductAdminViewModel model = new ProductAdminViewModel();
            IQueryable<Product> tmp = cms_db.GetlstProduct().Where(s => (s.MicrositeID == null || s.MicrositeID == 0) && s.StateId != (int)EnumCore.StateType.da_xoa);
            if (ProductCatalogry.HasValue && ProductCatalogry.Value != 0)
            {
                tmp = tmp.Where(s => s.CategoryId == ProductCatalogry);
                model.ProductCatalogry = ProductCatalogry.Value;
            }

            if (ProductState.HasValue && ProductState.Value != 0)
            {
                tmp = tmp.Where(s => s.StateId == ProductState);
                model.ProductState = ProductState.Value;
            }
            if (!String.IsNullOrEmpty(FillterSKU))
            {
                tmp = tmp.Where(s => s.SKUCode == FillterSKU);
                model.FillterSKU = FillterSKU;
            }
            if (!String.IsNullOrEmpty(FillterProductCD))
            {
                tmp = tmp.Where(s => s.ProductCD == FillterProductCD);
                model.FillterProductCD = FillterProductCD;
            }

            if (!String.IsNullOrEmpty(FillterProductName))
            {
                tmp = tmp.Where(s => s.ProductName.ToLower().Contains(FillterProductName.ToLower()));
                model.FillterProductCD = FillterProductCD;
            }

            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.lstMainProduct = tmp.OrderByDescending(c => c.ProductId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.lstProductState = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.state_type), "value", "text");
            model.lstProductCatalogry = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.san_pham), "value", "text");

            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateProduct")]
        public ActionResult Create()
        {
            ProductViewModels model = new ProductViewModels();
            model.CatalogryList = new SelectList(cms_db.GetProductCatagory(), "value", "text");
            //model.MainCatalogryList = new SelectList(cms_db.GetProductCatagory(), "value", "text");
            //model = this.LoadSelectlist(model);
            return View(model);

        }
        [AdminAuthorize(Roles = "supperadmin,devuser,CreateProduct")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductViewModels model, HttpPostedFileBase Default_files, HttpPostedFileBase[] Detail_files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                    Product MainModel = model._MainObj;
                    MainModel.CommentCount = 0;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    MainModel.ViewCount = 0;
                    MainModel.StateId = (int)EnumCore.StateType.cho_duyet;
                    MainModel.StateName = "Chờ Duyệt";

                    //MainModel.BrandNameID = model.BrandNameID;
                    //MainModel.Units = model.Units;
                    //MainModel.CorlorID = model.CorlorID;
                    //MainModel.MadeIn = model.MadeIn;
                    //MainModel.StatusProductType = model.StatusProductType;
                    //MainModel.SupportTimeID = model.SupportTimeID;

                    //MainModel.BrandName = cms_db.GetNameObjClasscifiById(model.BrandNameID);
                    //MainModel.UnitsName = cms_db.GetNameObjClasscifiById(model.Units);
                    //MainModel.CorlorName = cms_db.GetNameObjClasscifiById(model.CategoryId);
                    //MainModel.MadeInName = cms_db.GetNameObjClasscifiById(model.MadeIn);
                    //MainModel.StatusProductTypeName = cms_db.GetNameObjClasscifiById(model.StatusProductType);
                    //MainModel.SupportTimeName = cms_db.GetNameObjClasscifiById(model.SupportTimeID);


                    int rs = await cms_db.CreateProduct(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForProduct(Default_files, MainModel.ProductId);
                        int rsup = await this.UpdateImageUrlForProduct(rsdf, MainModel);
                    }

                    int SaveDetailImageForProduct = await this.SaveDetailImageForProduct(Detail_files, MainModel.ProductId);
                    //int SaveColorSKU = this.SaveColorSKU(model.ColorSku, MainModel.ProductId);
                    //int SaveProductSize = this.SaveProductSize(model.lstcbsize, MainModel.ProductId);

                    int ach = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                      (int)EnumCore.ActionType.Create, "Create", MainModel.ProductId, MainModel.ProductName, "Product", (int)EnumCore.ObjTypeId.san_pham);
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Create");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Create", "ProductManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }

        }

        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateProduct")]
        public ActionResult Edit(long id)
        {
            Product _tmp = cms_db.GetObjProductById(id);
            ProductViewModels model = new ProductViewModels(cms_db.GetObjProductById(id));
            Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
            //if (_objcata.ParentClassificationId == null || _objcata.ParentClassificationId == 0)
            //    model.MainCatalogryList = new SelectList(string.Empty, "Value", "Text");
            //else
            //{
            //    model.MainCatalogryList = new SelectList(cms_db.GetProductCatagoryByPrentId(_objcata.ParentClassificationId.Value), "value", "text");
            //}


            //model.lstcbsize = cms_db.GetProductInforExt(id, (int)EnumCore.ClassificationScheme.kich_thuoc, null, null, null).Select(s => s.Size.Value).ToArray();
            //model.lstColorSKU = cms_db.GetProductInforExt(id, (int)EnumCore.ClassificationScheme.color_list, null, null, null)
            //    .Select(p => new ProductColorSKUModels { ProductId = p.ProductId.Value, SKU = p.SKU, color = p.Color.Value, colorName = p.ColorName }).ToList();


            model.CatalogryList = new SelectList(cms_db.GetProductCatagory(), "value", "text");
            //model = this.LoadSelectlist(model);
            model.lstDetailImage = cms_db.GetLstMediaContentByKey(null, (int)EnumCore.mediatype.hinh_anh, (int)EnumCore.ObjTypeId.san_pham, id).ToList();
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateProduct")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductViewModels model, HttpPostedFileBase Default_files, HttpPostedFileBase[] Detail_files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Classification _objcata = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                    Product MainModel = cms_db.GetObjProductById(model.ProductId);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForProduct(Default_files, model.ProductId);
                        MainModel.MediaUrl = rsdf.FullURL;
                        MainModel.MediaThumb = rsdf.ThumbURL;
                    }
                    MainModel.ProductCD = model.ProductCD;
                    MainModel.FriendlyURL = model.FriendlyURL;
                    MainModel.NewPrice = model.NewPrice;
                    MainModel.OldPrice = model.OldPrice;
                    MainModel.ProductName = model.ProductName;
                    MainModel.ProductDetail = model.ProductDetail;
                    MainModel.ProductDes = model.ProductDes;
                    MainModel.MetadataDesc = model.MetadataDesc;
                    MainModel.MetadataKeyword = model.MetadataKeyword;
                    MainModel.CategoryId = model.CategoryId;
                    MainModel.CategoryName = _objcata.ClassificationNM;
                    MainModel.StateId = (int)EnumCore.StateType.enable;

                    //MainModel.BrandNameID = model.BrandNameID;
                    //MainModel.Units = model.Units;
                    //MainModel.CorlorID = model.CorlorID;
                    //MainModel.MadeIn = model.MadeIn;
                    //MainModel.StatusProductType = model.StatusProductType;
                    //MainModel.SupportTimeID = model.SupportTimeID;

                    //MainModel.BrandName = cms_db.GetNameObjClasscifiById(model.BrandNameID);
                    //MainModel.UnitsName = cms_db.GetNameObjClasscifiById(model.Units);
                    //MainModel.CorlorName = cms_db.GetNameObjClasscifiById(model.CorlorID);
                    //MainModel.MadeInName = cms_db.GetNameObjClasscifiById(model.MadeIn);
                    //MainModel.StatusProductTypeName = cms_db.GetNameObjClasscifiById(model.StatusProductType);
                    //MainModel.SupportTimeName = cms_db.GetNameObjClasscifiById(model.SupportTimeID);


                    int UpdateContent = await cms_db.UpdateProduct(MainModel);
                    int rsdt = await this.SaveDetailImageForProduct(Detail_files, MainModel.ProductId);

                    //int SaveColorSKU = this.SaveColorSKU(model.ColorSku, MainModel.ProductId);
                    //int SaveProductSize = this.SaveProductSize(model.lstcbsize, MainModel.ProductId);


                    int ach = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                      (int)EnumCore.ActionType.Create, "Edit", MainModel.ProductId, MainModel.ProductName, "Product", (int)EnumCore.ObjTypeId.san_pham);

                    return RedirectToAction("Index");
                }

                //Classification _objcata2 = cms_db.GetObjClasscifiById(model.CategoryId.Value);
                //if (_objcata2.ParentClassificationId == null || _objcata2.ParentClassificationId == 0)

                //    model.MainCatalogryList = new SelectList(string.Empty, "Value", "Text");
                //else
                //{
                //    model.MainCatalogryList = new SelectList(cms_db.GetProductCatagoryByPrentId(_objcata2.ParentClassificationId.Value), "value", "text");
                //}
                model.CatalogryList = new SelectList(cms_db.GetProductCatagory(), "value", "text");
                //model = this.LoadSelectlist(model);
                return View(model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Edit", "ProductManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }

        }
        [AdminAuthorize(Roles = "supperadmin,devuser,ApproveProduct")]
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
        [AdminAuthorize(Roles = "supperadmin,devuser,DeleteProduct")]
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






        private async Task<MediaContentViewModels> SaveDefaultImageForProduct(HttpPostedFileBase file, long ProductId)
        {
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.san_pham
                    && s.MediaTypeId == (int)EnumCore.mediatype.hinh_anh_dai_dien && s.ContentObjId == ProductId).FirstOrDefault();
            if (CurrentMediaId != null)
            {
                int rs = await cms_db.DeleteMediaContent(CurrentMediaId.MediaContentId);
            }
            ImageUploadViewModel item = new ImageUploadViewModel();
            item = cms_db.UploadHttpPostedFileBase(file);
            MediaContentViewModels _Media = new MediaContentViewModels();
            _Media.Filename = item.ImageName;
            _Media.FullURL = item.ImageUrl;
            _Media.ContentObjId = ProductId;
            _Media.ObjTypeId = (int)EnumCore.ObjTypeId.san_pham;
            _Media.ViewCount = 0;
            _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh_dai_dien;
            _Media.CrtdDT = DateTime.UtcNow;
            _Media.MediaContentSize = file.ContentLength;
            _Media.ThumbURL = item.ImageThumbUrl;
            _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
            await cms_db.AddNewMediaContent(_Media);
            return _Media;

        }
        private async Task<int> UpdateImageUrlForProduct(MediaContentViewModels ImageObj, Product ProductObj)
        {
            ProductObj.MediaUrl = ImageObj.FullURL;
            ProductObj.MediaThumb = ImageObj.ThumbURL;
            return await cms_db.UpdateProduct(ProductObj);
        }
        private async Task<int> SaveDetailImageForProduct(HttpPostedFileBase[] file, long ProductId)
        {
            if (file.Count() > 0 && file[0] != null)
            {
                foreach (HttpPostedFileBase _item in file)
                {
                    ImageUploadViewModel item = new ImageUploadViewModel();
                    item = cms_db.UploadHttpPostedFileBase(_item);
                    MediaContentViewModels _Media = new MediaContentViewModels();
                    _Media.Filename = item.ImageName;
                    _Media.FullURL = item.ImageUrl;
                    _Media.ContentObjId = ProductId;
                    _Media.ObjTypeId = (int)EnumCore.ObjTypeId.san_pham;
                    _Media.ViewCount = 0;
                    _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh;
                    _Media.CrtdDT = DateTime.UtcNow;
                    _Media.MediaContentSize = _item.ContentLength;
                    _Media.ThumbURL = item.ImageThumbUrl;
                    _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
                    await cms_db.AddNewMediaContent(_Media);
                }
            }
            return (int)EnumCore.Result.action_true;

        }
        private async Task<int> UpdateMediaForContent(MediaContent _objnewmedia, MediaContent _objoldmedia, long ProductId)
        {
            if (_objnewmedia != null && _objoldmedia != null)
            {
                if (_objnewmedia.MediaContentId != _objoldmedia.MediaContentId)
                {
                    int _dlmedia = await cms_db.DeleteMediaContent(_objoldmedia.MediaContentId);
                    int UpdateDefaultMedia = await this.UpdateContentObjForMedia(_objnewmedia, ProductId);
                    return (int)EnumCore.Result.action_true;
                }
            }
            if (_objnewmedia != null && _objoldmedia == null)
            {
                int UpdateDefaultMedia = await this.UpdateContentObjForMedia(_objnewmedia, ProductId);
                return (int)EnumCore.Result.action_true;
            }
            return (int)EnumCore.Result.action_false;
        }
        private async Task<int> UpdateContentObjForMedia(MediaContent Media, long ProductId)
        {
            Media.ContentObjId = ProductId;
            Media.ObjTypeId = (int)EnumCore.ObjTypeId.san_pham;
            int rs = await cms_db.UpdateMediaContent(Media);
            return (int)EnumCore.Result.action_true;
        }

        public JsonResult GetLstcatelogryProductByparent(int id)
        {

            List<SelectListObj> result = cms_db.GetclassCatagory(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectListBySchemeId(int id)
        {
            List<SelectListObj> result = cms_db.Getclasscatagory(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private ProductViewModels LoadSelectlist(ProductViewModels model)
        {
            model.BrandList = new SelectList(cms_db.GetCatagoryForSelectList((int)
                 EnumCore.ClassificationScheme.Product_brand), "ClassificationId", "ClassificationNM");

            model.MadeInList = new SelectList(cms_db.GetCatagoryForSelectList((int)
                EnumCore.ClassificationScheme.xuat_xu), "ClassificationId", "ClassificationNM");

            model.UnitsList = new SelectList(cms_db.GetCatagoryForSelectList((int)
                EnumCore.ClassificationScheme.don_vi), "ClassificationId", "ClassificationNM");

            model.CorlorList = new SelectList(cms_db.GetCatagoryForSelectList((int)
                 EnumCore.ClassificationScheme.color_list), "ClassificationId", "ClassificationNM");

            model.SupportTimeList = new SelectList(cms_db.GetCatagoryForSelectList((int)
                 EnumCore.ClassificationScheme.support_time), "ClassificationId", "ClassificationNM");

            model.TranTypeList = new SelectList(cms_db.GetCatagoryForSelectList((int)
                 EnumCore.ClassificationScheme.tran_type), "ClassificationId", "ClassificationNM");

            model.Status_Productlst = new SelectList(cms_db.GetCatagoryForSelectList((int)
                  EnumCore.ClassificationScheme.statusproduct_type), "ClassificationId", "ClassificationNM");

            model.lstSize = new SelectList(cms_db.GetCatagoryForSelectList((int)
                 EnumCore.ClassificationScheme.kich_thuoc), "ClassificationId", "ClassificationNM");
            return model;
        }

        //private int SaveColorSKU(string[] model, long ProductId)
        //{
        //    try
        //    {
        //        int dl = cms_db.DeleteProductInforExtByProductIdVsType(ProductId, (int)EnumCore.ClassificationScheme.color_list);
        //        foreach (string _val in model)
        //        {
        //            string[] catchuoi = _val.Split('-');
        //            ProductInforExt tmp = new ProductInforExt();
        //            tmp.ProductId = ProductId;
        //            tmp.Type = (int)EnumCore.ClassificationScheme.color_list;
        //            tmp.Color = int.Parse(catchuoi[0]);
        //            tmp.ColorName = cms_db.GetNameObjClasscifiById(tmp.Color);
        //            tmp.SKU = catchuoi[1];
        //            cms_db.CreateProductInforExt(tmp);
        //        }
        //        return (int)EnumCore.Result.action_true;
        //    }
        //    catch (Exception e)
        //    {
        //        cms_db.AddToExceptionLog("SaveColorSKU", "ProductManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
        //        return (int)EnumCore.Result.action_false;
        //    }
        //}
        //private int SaveProductSize(int[] model, long ProductId)
        //{
        //    try
        //    {
        //        int dl = cms_db.DeleteProductInforExtByProductIdVsType(ProductId, (int)EnumCore.ClassificationScheme.kich_thuoc);
        //        foreach (int _val in model)
        //        {
        //            ProductInforExt tmp = new ProductInforExt();
        //            tmp.ProductId = ProductId;
        //            tmp.Type = (int)EnumCore.ClassificationScheme.kich_thuoc;
        //            tmp.Size = _val;
        //            tmp.SizeName = cms_db.GetNameObjClasscifiById(_val);
        //            cms_db.CreateProductInforExt(tmp);
        //        }
        //        return (int)EnumCore.Result.action_true;
        //    }
        //    catch (Exception e)
        //    {
        //        cms_db.AddToExceptionLog("SaveProductSize", "ProductManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
        //        return (int)EnumCore.Result.action_false;
        //    }
        //}

    }
}