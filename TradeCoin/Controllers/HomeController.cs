using DataModel.DataEntity;
using DataModel.DataViewModel;
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
using PagedList;

namespace Alluneecms.Controllers
{
    public class HomeController : CoreFronEndController
    {
        public ActionResult Index()
        {
            try {

                IndexViewModel model = new IndexViewModel();

                long[] lstContentid = cms_db.GetlstContentPackageIquery().Where(s => s.ContentType 
                            == (int)EnumCore.ObjTypeId.tin_tuc && s.PackageId==1).OrderByDescending(s=>s.ContentId).Select(s=>s.ContentId).Take(30).ToArray();

                model.lstNews = cms_db.GetlstContentItem().Where(s => lstContentid.Contains(s.ContentItemId)
                                                    && s.StateId!=(int)EnumCore.StateType.da_xoa).Take(3).ToList();
                model.crypto = this.GetDatPageInforViewModelaPageInfor((int)ConstFrontEnd.FrontendPageinfor.crypto);
                model.indexvideo = this.GetDatPageInforViewModelaPageInfor((int)ConstFrontEnd.FrontendPageinfor.indexvideo);
                model.intro = this.GetDatPageInforViewModelaPageInfor((int)ConstFrontEnd.FrontendPageinfor.intro);
                model.trade = this.GetDatPageInforViewModelaPageInfor((int)ConstFrontEnd.FrontendPageinfor.trade);

                IPagedList<Package> _lstPackage = cms_db.GetlstPackage()
                        .OrderBy(c => c.PackageId).ToPagedList(1, (int)EnumCore.BackendConst.page_size);
                foreach (var item in _lstPackage)
                {
                    if (item.PackageId == 2)
                    {
                        ViewBag.Night1M = item.NewPrice;
                        ViewBag.Night3M = item.NewPrice3M;
                        ViewBag.NightForever = item.ForeverPrice;
                    }
                    if (item.PackageId == 3)
                    {
                        ViewBag.Silver1M = item.NewPrice;
                        ViewBag.Silver3M = item.NewPrice3M;
                        ViewBag.SilverForever = item.ForeverPrice;
                    }
                    if (item.PackageId == 4)
                    {
                        ViewBag.Gold1M = item.NewPrice;
                        ViewBag.Gold3M = item.NewPrice3M;
                        ViewBag.GoldForever = item.ForeverPrice;
                    }
                    if (item.PackageId == 5)
                    {
                        ViewBag.Diamond1M = item.NewPrice;
                        ViewBag.Diamond3M = item.NewPrice3M;
                        ViewBag.DiamondForever = item.ForeverPrice;
                    }
                }
                return View(model);
            }
            catch (Exception e) {
                DataModel.DataStore.Core core = new DataModel.DataStore.Core();
                core.AddToExceptionLog("UpdatePhotoUser", "AccountController", "Upload photo Error: " + e.Message);
                return View();

            }
         
        }

        private PageInforViewModel GetDatPageInforViewModelaPageInfor(int CataPageInfor)
        {
            PageInforViewModel model = new PageInforViewModel();

            model.PageTitle = cms_db.GetObjClasscifiById(CataPageInfor);
            model.PageContent = cms_db.GetlstContentItem().Where(s => s.CategoryId == CataPageInfor
                    && s.StateId != (int)EnumCore.StateType.da_xoa).OrderByDescending(s => s.ContentItemId).FirstOrDefault();
            return model;
        }






        public ActionResult HeaderPartial()
        {
            try
            {
              
                return PartialView("_HeaderPartial");

            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("header", "HomeController Public", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }

        }

        public ActionResult MetaPartial()
        {
            Config model = HttpContext.Application["PageConfig"] as Config;
            return PartialView("_MetaPartial", model);
        }

        public ActionResult FooterPartial()
        {
            try
            {
              
                return PartialView("_FooterPartial");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("navMenuPartial", "HomeController Public", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
        }

     

        public ActionResult About()
        {
           
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Page404()
        {
            return View();
        }


        public ActionResult RightSlidePartial2()
        {
            RightSliderBarViewModel Model = new RightSliderBarViewModel();
            Model.LstProductViewMore =  cms_db.GetListViewProduct(5);
            Model.LstProductNews = cms_db.GetListNewsProduct(5);
            Model.ListDayProduct = cms_db.GetListDayProduct(5);
            Model.lstIdea = cms_db.GetlstContentItemByCataId(11883, 3);

            return PartialView("_RightSlidePartial2", Model);
        }
        private List<HomeSliderPartialViewModel> GetHomSlider()
        {
            List<HomeSliderPartialViewModel> model = new List<HomeSliderPartialViewModel>();
            model.Clear();
            List<DisplayContent> lstDisplayContent = cms_db.GetlstDisplayContent(null, (int)ConstFrontEnd.FontEndConstDisplayType.HomeSlider, null).ToList();
            foreach (DisplayContent _item in lstDisplayContent)
            {
                HomeSliderPartialViewModel ChildItem = new HomeSliderPartialViewModel();
                MediaContent MediaContent_tmp = new MediaContent();
                if (_item.ObjType == (int)EnumCore.ObjTypeId.tin_tuc)
                {
                    ContentItem ContentItem_tmp = cms_db.GetObjContentItemById(_item.ContentId.Value);
                    MediaContent_tmp = cms_db.GetObjDefaultMediaByContentIdvsType(ContentItem_tmp.ContentItemId, (int)EnumCore.ObjTypeId.tin_tuc);
                    ChildItem.MediaUrl = MediaContent_tmp.FullURL;
                    ChildItem.SliderName = ContentItem_tmp.ContentTitle;

                }

                if (_item.ObjType == (int)EnumCore.ObjTypeId.san_pham)
                {
                    Product Product_tmp = cms_db.GetObjProductById(_item.ContentId.Value);
                    MediaContent_tmp = cms_db.GetObjDefaultMediaByContentIdvsType(Product_tmp.ProductId, (int)EnumCore.ObjTypeId.san_pham);
                    ChildItem.CataObj = Product_tmp.ObjCatelogry;
                    if (ChildItem.CataObj.ParentClassificationId != null)
                    {
                        ChildItem.url = ChildItem.CataObj.ObjPrent.FriendlyURL + "/" + ChildItem.CataObj.FriendlyURL + "/" + Product_tmp.FriendlyURL;
                    }
                    else
                    {
                        ChildItem.url = ChildItem.CataObj.FriendlyURL + "/" + Product_tmp.FriendlyURL;

                    }
                    ChildItem.MediaUrl = MediaContent_tmp.FullURL;
                    ChildItem.ThumbUrl = MediaContent_tmp.ThumbURL;
                    ChildItem.SliderName = Product_tmp.ProductName;
                    ChildItem.ViewCount = Product_tmp.ViewCount.Value;
                    ChildItem.ProductPrice = Product_tmp.NewPrice.Value;
                    ChildItem.ProductOldPrice = Product_tmp.OldPrice.Value;

                }

                if (_item.ObjType == (int)EnumCore.ObjTypeId.banner)
                {
                    MediaContent Banner_tmp = cms_db.GetObjMediaContent(_item.ContentId.Value);
                    ChildItem.MediaUrl = Banner_tmp.FullURL;
                    ChildItem.SliderName = Banner_tmp.Filename;
                }

                model.Add(ChildItem);
            }

            return model;

        }

    }




}