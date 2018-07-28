using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;
using PagedList;
using DataModel.DataStore;
namespace DataModel.DataViewModel
{
    #region Partial Action

    public class HomeSliderPartialViewModel
    {
        public Classification CataObj { get; set; }
        public string MediaUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string SliderName { get; set; }
        public string SliderDes { get; set; }
        public string SliderCaption { get; set; }
        public double ProductPrice { get; set; }
        public double ProductOldPrice { get; set; }
        public string ProductDes { get; set; }
        public long ViewCount { get; set; }
        public string url { get; set; }
        public string LinkHref { get; set; }

    }
    public class PageInforViewModel
    {
        public Classification PageTitle { get; set; }
        public ContentItem PageContent { get; set; }

    }

    public class ProductDetaiViewModel
    {
        public Product MainProduct { get; set; }
        public List<Product> LstSameProduct { get; set; }
        public Classification CateObj { get; set; }

    }

    public class NewsHomeViewModel
    {
        public ContentItem ContentItem { get; set; }
        public Classification ObjCategory { get; set; }
    }

    public class DetailNewsAjaxViewModel
    {
        public ContentItem Item { get; set; }
        public IList<ContentItem> ListItem { get; set; }
    }

    public class DetailProductViewModel
    {
        public Product MainProduct { get; set; }
        public List<Product> lstSameProduct { get; set; }
        public Classification CataObj { get; set; }
        public List<Product> lstUserProduct { get; set; }
        public List<MediaContent> lstImage { get; set; }
    }

    public class DataMicrositeInvolve
    {
        public Microsite Microsite { get; set; }
        public List<Product> LstProduct { get; set; }
    }

    public class DetailPublicMicrositeProductViewModel
    {
        public Product MainProduct { get; set; }
        public List<Product> lstSameProduct { get; set; }
        public MicroClassification CataObj { get; set; }
        public List<Product> lstUserProduct { get; set; }
        public List<MediaContent> lstImage { get; set; }
        public string FriendlyUrlShop { get; set; }
        public long Idshop { get; set; }
       
    }


    public class DetailPublicNewMicrositeViewModel
    {
        public ContentItem MainNews { get; set; }
        public Classification CataObj { get; set; }
        public string FriendlyUrlShop { get; set; }
        public long Idshop { get; set; }
       
    }

    public class ListProductViewModel
    {
        public Classification ParentCataObj { get; set; }
        public Classification CataObj { get; set; }
        public List<Product> lstProduct { get; set; }

    }

    public class PageAddImageProductViewModel
    {
        public Product ProductObj { get; set; }
        public List<MediaContent> lstImage { get; set; }

        public string FriendlyUrlShop { get; set; }
        public long Idshop { get; set; }
    }

    public class SendMessageViewModel
    {
        [Required(ErrorMessage ="Không được bỏ trống")]
        [MaxLength(255,ErrorMessage ="Tối đa 255 ký tự")]
        public string EmailTo { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        [MaxLength(255, ErrorMessage = "Tối đa 255 ký tự")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        public string Content { get; set; }
    }

    public class BlockPageInforViewModel
    {
        public Classification PageInforObj { get; set; }
        public List<ContentItem> lstContentItem { get; set; }

    }
    public class FooterViewModel
    {
        public List<SubMenuViewModels> CataList { get; set; }
       

    }

    public class IndexNewHomeViewModel
    {
        public BlockPageInforViewModel lang_nghe_viet { get; set; }
        public BlockPageInforViewModel chuyen_dong { get; set; }
        public BlockPageInforViewModel khoi_nghiep { get; set; }
        public BlockPageInforViewModel kinh_doanh { get; set; }
        public BlockPageInforViewModel su_kien_van_hoa { get; set; }
        public BlockPageInforViewModel allunee { get; set; }

    }

 

    #endregion

}
