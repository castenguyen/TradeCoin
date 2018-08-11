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



namespace Alluneecms.Controllers
{
    public class HomeController : CoreFronEndController
    {
        public ActionResult Index()
        {
          //  string ip = HttpContext.Request.UserHostAddress;
            IndexViewModel MainModel = new IndexViewModel();
            List<HomeListProductViewModel> ProductModel = new List<HomeListProductViewModel>();
            List<Classification> lstParent = cms_db.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.san_pham).Where(s=>s.ParentClassificationId==null).ToList();
            foreach (Classification _item in lstParent)
            {
                HomeListProductViewModel blockpro = new HomeListProductViewModel();
                blockpro = cms_db.GetLstHomeProductByCataId(_item.ClassificationId, (int)EnumCore.IndexConst.NbrProduct_In_Home);
                if (blockpro.lstProduct.Count() > 0)
                    ProductModel.Add(blockpro);
            }
            MainModel.HomeProduct = ProductModel;
          //  MainModel.ListNewProduct = cms_db.GetListNewsProduct(4);
            MainModel.lstNews = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep && s.CategoryId == (int)EnumCore.Classifi_news_index.baiviet).OrderByDescending(s => s.CrtdDT).Take(4).ToList();
            MainModel.lstIdeas = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep && s.CategoryId == (int)EnumCore.Classifi_news_index.ykien).OrderByDescending(s => s.CrtdDT).Take(10).ToList();
            MainModel.lstParner = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep && s.CategoryId == (int)EnumCore.Classifi_news_index.doitac).OrderByDescending(s => s.CrtdDT).Take(10).ToList();
            MainModel.lstWhy = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep && s.CategoryId == (int)EnumCore.Classifi_news_index.visao).OrderByDescending(s => s.CrtdDT).Take(3).ToList();
            //  MainModel.ListViewProduct = cms_db.GetListViewProduct(4);

            Config cf = new Config();
            cf = cms_db.GetConfig();
            this.SetInforMeta(cf.site_metadatakeyword, cf.site_metadadescription);
            return View(MainModel);
        }

        public ActionResult HeaderPartial()
        {
            try
            {
                List<SubMenuViewModels> Model = new List<SubMenuViewModels>();

                List<Classification> ParentListMenu = cms_db.GetMainMenuList(null);
                foreach (Classification item in ParentListMenu)
                {
                    SubMenuViewModels MenuItem = new SubMenuViewModels();
                    MenuItem.Parent = item;
                    MenuItem.lstChild = cms_db.GetMainMenuList(item.ClassificationId);
                    Model.Add(MenuItem);
                }
                return PartialView("_HeaderPartial", Model);

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
                List<SubMenuViewModels> Model = new List<SubMenuViewModels>();
                List<Classification> ParentListMenu = cms_db.GetMainMenuList(null);
                foreach (Classification item in ParentListMenu)
                {
                    SubMenuViewModels MenuItem = new SubMenuViewModels();
                    MenuItem.Parent = item;
                    MenuItem.lstChild = cms_db.GetMainMenuList(item.ClassificationId);
                    Model.Add(MenuItem);
                }
                return PartialView("_FooterPartial", Model);
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("navMenuPartial", "HomeController Public", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
        }

        private BlockPageInforViewModel GetDataForBlockFooter(int CataPageInfor)
        {
            BlockPageInforViewModel model = new BlockPageInforViewModel();
            model.PageInforObj = cms_db.GetObjClasscifiById(CataPageInfor);
            model.lstContentItem = cms_db.GetlstContentItemByCataId(CataPageInfor, 10).Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).ToList();
            return model;

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
            List<DisplayContent> lstDisplayContent = cms_db.GetlstDisplayContent(null, (int)EnumCore.Classification_DisplayType.HomeSlider, null).ToList();
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