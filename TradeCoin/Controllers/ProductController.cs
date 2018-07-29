using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
namespace Alluneecms.Controllers
{
    public class ProductController : CoreFronEndController
    {
        public ActionResult ProductDetail(string FriendlyUrl, string string1, string string2)
        {
            try
            {
                Classification Objcata = cms_db.GetObjClasscifiByFriendlyUrl(FriendlyUrl);
                if (Objcata != null)
                {
                    this.SetInforMeta(Objcata.ClassificationMetakeyword, Objcata.ClassificationMetaDes);
                    ListProductViewModel model = new ListProductViewModel();
                    model.CataObj = Objcata;
                    model.lstProduct = cms_db.GetlstProductByCataId(Objcata.ClassificationId);
                    return View("ListPartial", model);
                }
                else
                {
                    DetailProductViewModel model = new DetailProductViewModel();
                    model.MainProduct = cms_db.GetObjProductByFriendlyURL(FriendlyUrl);
                    model.CataObj = cms_db.GetObjClasscifiById(model.MainProduct.CategoryId.Value);
                    model.lstSameProduct = cms_db.GetlstProductByCataIdHasNum(model.MainProduct.CategoryId.Value, 8);
                    model.lstImage = cms_db.GetLstMediaContent(model.MainProduct.ProductId, (int)EnumCore.ObjTypeId.san_pham).ToList();
                    this.SetInforMeta(model.MainProduct.MetadataKeyword, model.MainProduct.MetadataDesc);
                    return View("DetailPartial", model);
                }
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("function ProductDetails", "ProductController(Public)", e.ToString());
                return RedirectToAction("ErrorExeption", "ErrorMessage");
            }
        }

        public ActionResult ProductList(string FriendlyUrl)
        {
            try
            {
                Classification Objcata = cms_db.GetObjClasscifiByFriendlyUrl(FriendlyUrl);
                if (Objcata != null)
                {
                    this.SetInforMeta(Objcata.ClassificationMetakeyword, Objcata.ClassificationMetaDes);
                    ListProductViewModel model = new ListProductViewModel();
                    model.CataObj = Objcata;
                    if (cms_db.GetlstClassifiByParentId(Objcata.ClassificationId).Count() > 0)
                    {
                        List<ListProductViewModel> _tmpmodel = new List<ListProductViewModel>();
                        List<Classification> lstCata = cms_db.GetlstClassifiByParentId(Objcata.ClassificationId).ToList();
                        foreach (Classification _val in lstCata)
                        {
                            ListProductViewModel tmp = new ListProductViewModel();
                            tmp.CataObj = _val;
                            tmp.lstProduct = cms_db.GetlstProductByCataIdHasNum(_val.ClassificationId, 4);
                            ViewBag.MainCata = Objcata;
                            _tmpmodel.Add(tmp);
                        }
                        return View("ListCatalogryPro", _tmpmodel);
                    }
                    else
                    {
                        model.lstProduct = cms_db.GetlstProductByCataId(Objcata.ClassificationId);
                    }
                    return View("ListPartial", model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }


            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("function ListProduct", "ProductController(Public)", e.ToString());
                return RedirectToAction("ErrorExeption", "ErrorMessage");
            }

        }
        public ActionResult AllProduct()
        {
            try
            {

                this.SetInforMeta("", "");
                List<ListProductViewModel> _tmpmodel = new List<ListProductViewModel>();
                List<Classification> lstCata = cms_db.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.san_pham).ToList();
                List<Classification> lstCata2 = new List<Classification>();
                foreach (Classification _val in lstCata)
                {
                    List<Classification> tmpcate = cms_db.GetlstClassifiByParentId(_val.ClassificationId).ToList();
                    if (tmpcate.Count == 0)
                    {
                        lstCata2.Add(_val);
                    }
                }
                foreach (Classification _val in lstCata2)
                {
                    ListProductViewModel tmp = new ListProductViewModel();
                    if (_val.ObjPrent != null)
                        tmp.ParentCataObj = _val.ObjPrent;
                    tmp.CataObj = _val;
                    tmp.lstProduct = cms_db.GetlstProductByCataIdHasNum(_val.ClassificationId,3);
                    _tmpmodel.Add(tmp);
                }
                return View("ListAll", _tmpmodel);

            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("function ListProduct", "ProductController(Public)", e.ToString());
                return RedirectToAction("ErrorExeption", "ErrorMessage");
            }

        }



    }
}