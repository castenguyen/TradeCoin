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

namespace Alluneecms.Controllers
{
    public class BlockPartialController : CoreFronEndController
    {
        //public ActionResult ContentRight()
        //{
        //    return PartialView("_ContentRightPartial");
        //}
        //public ActionResult BestNewsPartial()
        //{
        //    List<ContentItem> Model = new List<ContentItem>();
        //    Model = cms_db.GetlstContentItem().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep && s.ObjCategory.ClassificationSchemeId!=(int)EnumCore.ClassificationScheme.page_infor)
        //                                                .OrderByDescending(s => s.ViewCount).Take((int)EnumCore.IndexConst.NbrNews_In_rightcontent).ToList();
        //    return PartialView("_BestNewsPartial", Model);
        //}
        //public ActionResult AdsRightContentPartial()
        //{
        //    List<HomeSliderPartialViewModel> model = new List<HomeSliderPartialViewModel>();
        //    model.Clear();
        //    List<DisplayContent> lstDisplayContent = cms_db.GetlstDisplayContent(null, 
        //        (int)EnumCore.Classification_DisplayType.banner_right_top, null).Take((int)EnumCore.IndexConst.NbrAds_In_TopRight).ToList();
        //    foreach (DisplayContent _item in lstDisplayContent)
        //    {
        //        HomeSliderPartialViewModel ChildItem = new HomeSliderPartialViewModel();
        //        MediaContent MediaContent_tmp = new MediaContent();
        //        if (_item.ObjType == (int)EnumCore.ObjTypeId.banner)
        //        {
        //            MediaContent Banner_tmp = cms_db.GetObjMediaContent(_item.ContentId.Value);
        //            ChildItem.MediaUrl = Banner_tmp.FullURL;
        //            ChildItem.SliderName = Banner_tmp.Filename;
        //            ChildItem.LinkHref = Banner_tmp.LinkHref;
        //        }
        //        model.Add(ChildItem);
        //    }
        //    return PartialView("_AdsRightContentPartial", model);
        //}
        //public ActionResult AdsRightMidlePartial()
        //{
        //    List<HomeSliderPartialViewModel> model = new List<HomeSliderPartialViewModel>();
        //    model.Clear();
        //    List<DisplayContent> lstDisplayContent = cms_db.GetlstDisplayContent(null, 
        //        (int)EnumCore.Classification_DisplayType.banner_right_midle, null).Take((int)EnumCore.IndexConst.NbrAds_In_MidleRight).ToList();
        //    foreach (DisplayContent _item in lstDisplayContent)
        //    {
        //        HomeSliderPartialViewModel ChildItem = new HomeSliderPartialViewModel();
        //        MediaContent MediaContent_tmp = new MediaContent();
        //        if (_item.ObjType == (int)EnumCore.ObjTypeId.banner)
        //        {
        //            MediaContent Banner_tmp = cms_db.GetObjMediaContent(_item.ContentId.Value);
        //            ChildItem.MediaUrl = Banner_tmp.FullURL;
        //            ChildItem.SliderName = Banner_tmp.Filename;
        //            ChildItem.LinkHref = Banner_tmp.LinkHref;
        //        }
        //        model.Add(ChildItem);
        //    }
        //    return PartialView("_AdsRightMidlePartial", model);
        //}
        //public ActionResult BestProductPartial()
        //{
        //    List<Product> Model = new List<Product>();
        //    Model = cms_db.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep)
        //        .OrderByDescending(s => s.ViewCount).Take((int)EnumCore.IndexConst.NbrProduct_In_rightcontent).ToList();
        //    return PartialView("_BestProductPartial", Model);
        //}
        //public ActionResult PageInforPartial()
        //{
        //    List<PageInforViewModel> model = new List<PageInforViewModel>();
        //    List<Classification> LstPageTitle = new List<Classification>();
        //    LstPageTitle = cms_db.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.page_infor).ToList();
        //    foreach (Classification _val in LstPageTitle)
        //    {
        //        PageInforViewModel childpage = new PageInforViewModel();
        //        childpage.PageTitle = _val;
        //        childpage.PageContent = cms_db.GetlstContentItem().Where(s => s.CategoryId == _val.ClassificationId)
        //                                                        .OrderByDescending(s => s.ContentItemId).FirstOrDefault();
        //        model.Add(childpage);

        //    }
        //    return PartialView("_PageInforPartial", model);
        //}

        public ActionResult IndexAD1Partial()
         {
            HomeSliderPartialViewModel model = new HomeSliderPartialViewModel();
            DisplayContent DisplayContent = cms_db.GetlstDisplayContent(null, (int)EnumCore.Classification_DisplayType.HomeSlider, null).FirstOrDefault();
            if (DisplayContent == null)
                return PartialView("_IndexAD1Partial", model);
            MediaContent Banner_tmp = cms_db.GetObjMediaContent(DisplayContent.ContentId.Value);
            model.MediaUrl = Banner_tmp.FullURL;
            model.SliderName = Banner_tmp.Filename;
            model.SliderCaption = Banner_tmp.Caption;
            model.SliderDes = Banner_tmp.MediaDesc;
            model.LinkHref = Banner_tmp.LinkHref;
            return PartialView("_IndexAD1Partial", model);

        }
    

      
    }
}