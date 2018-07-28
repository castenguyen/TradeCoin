using DataModel.DataEntity;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace DataModel.DataViewModel
{

    public class ProductViewModels
    {
        public Product _MainObj { get; set; }
        public ProductViewModels()
        {
            _MainObj = new Product();

        }
        public ProductViewModels(Product model)
        {
            _MainObj = model;

        }

        public long ProductId
        {
            get { return _MainObj.ProductId; }
            set { _MainObj.ProductId = value; }
        }

        [Display(Name = "Danh mục của bạn")]
        public long CateMicrositeID { get; set; }
        [Required]
        [Display(Name = "Đường dẩn thân thiện")]
        public string FriendlyURL
        {
            get { return _MainObj.FriendlyURL; }
            set { _MainObj.FriendlyURL = value; }
        }
        public string FriendlyURLMicro { get; set; }
        public Nullable<long> ParentId
        {
            get { return _MainObj.ParentId; }
            set { _MainObj.ParentId = value; }
        }
        public string ProductCD
        {
            get { return _MainObj.ProductCD; }
            set { _MainObj.ProductCD = value; }
        }
        public Nullable<long> ViewCount
        {
            get { return _MainObj.ViewCount; }
            set { _MainObj.ViewCount = value; }
        }
        public Nullable<long> CommentCount
        {
            get { return _MainObj.CommentCount; }
            set { _MainObj.CommentCount = value; }
        }

        [Required]
        public Nullable<byte> IsCommentEnabled
        {
            get
            {
                if (this.IsCommentEnabledVM == true)
                {
                    return 1;
                }

                else
                {
                    return 0;
                };
            }
            set
            {
                if (this.IsCommentEnabledVM == true)
                {
                    _MainObj.IsCommentEnabled = 1;
                }

                else
                {
                    _MainObj.IsCommentEnabled = 0;
                };
            }
        }

        [Required]
        [Display(Name = "Cho phép comment")]
        public bool IsCommentEnabledVM { get; set; }

        [Required]
        [Display(Name = "Danh mục")]
        public Nullable<int> CategoryId
        {
            get { return _MainObj.CategoryId; }
            set { _MainObj.CategoryId = value; }
        }
        public string CategoryName
        {
            get { return _MainObj.CategoryName; }
            set { _MainObj.CategoryName = value; }
        }

        public string AprvdUserName
        {
            get { return _MainObj.AprvdUserName; }
            set { _MainObj.AprvdUserName = value; }
        }

        [Required]
        [AllowHtml]
        [Display(Name = "Nội dung")]
        public string ProductDetail
        {
            get { return _MainObj.ProductDetail; }
            set { _MainObj.ProductDetail = value; }
        }

        [Required]
        [Display(Name = "Tên")]
        public string ProductName
        {
            get { return _MainObj.ProductName; }
            set { _MainObj.ProductName = value; }
        }

        [Required]
        [Display(Name = "Mô tả")]
        public string ProductDes
        {
            get { return _MainObj.ProductDes; }
            set { _MainObj.ProductDes = value; }
        }

        [Required]
        [Display(Name = "Mô tả SEO")]
        public string MetadataDesc
        {
            get { return _MainObj.MetadataDesc; }
            set { _MainObj.MetadataDesc = value; }
        }

        [Required]
        [Display(Name = "Từ khoá SEO")]
        public string MetadataKeyword
        {
            get { return _MainObj.MetadataKeyword; }
            set { _MainObj.MetadataKeyword = value; }
        }
        [Display(Name = "MediaUrl")]
        public string MediaUrl
        {
            get { return _MainObj.MediaUrl; }
            set { _MainObj.MediaUrl = value; }
        }
        [Display(Name = "MediaThumb")]
        public string MediaThumb
        {
            get { return _MainObj.MediaThumb; }
            set { _MainObj.MediaThumb = value; }
        }
        public string CrtdUserName
        {
            get { return _MainObj.CrtdUserName; }
            set { _MainObj.CrtdUserName = value; }
        }
        public Nullable<long> CrtdUserId
        {
            get { return _MainObj.CrtdUserId; }
            set { _MainObj.CrtdUserId = value; }
        }
        public Nullable<long> AprvdUID
        {
            get { return _MainObj.AprvdUID; }
            set { _MainObj.AprvdUID = value; }
        }

        public Nullable<System.DateTime> CrtdDT
        {
            get { return _MainObj.CrtdDT; }
            set { _MainObj.CrtdDT = value; }
        }
        public Nullable<System.DateTime> AprvdDT
        {
            get { return _MainObj.AprvdDT; }
            set { _MainObj.AprvdDT = value; }
        }
        public Nullable<byte> Indexed
        {
            get { return _MainObj.Indexed; }
            set { _MainObj.Indexed = value; }
        }
        public Nullable<long> StateId
        {
            get { return _MainObj.StateId; }
            set { _MainObj.StateId = value; }
        }

        [Display(Name = "Giá")]
        public Nullable<double> OldPrice
        {
            get { return _MainObj.OldPrice; }
            set { _MainObj.OldPrice = value; }
        }

        [Display(Name = "Giá Khuyến Mãi")]
        public Nullable<double> NewPrice
        {
            get { return _MainObj.NewPrice; }
            set { _MainObj.NewPrice = value; }
        }

        public Nullable<int> BrandNameID
        {
            get { return _MainObj.BrandNameID; }
            set { _MainObj.BrandNameID = value; }
        }

        [Display(Name = "Thương hiệu")]
        public string BrandName
        {
            get { return _MainObj.BrandName; }
            set { _MainObj.BrandName = value; }
        }
        public Nullable<System.DateTime> PriorityDay
        {
            get { return _MainObj.PriorityDay; }
            set { _MainObj.PriorityDay = value; }
        }

        [Display(Name = "Đơn Vị")]
        public Nullable<int> Units
        {
            get { return _MainObj.Units; }
            set { _MainObj.Units = value; }
        }
        public string UnitsName
        {
            get { return _MainObj.UnitsName; }
            set { _MainObj.UnitsName = value; }
        }

        [Display(Name = "Nơi Sản Xuất")]
        public Nullable<int> MadeIn
        {
            get { return _MainObj.MadeIn; }
            set { _MainObj.MadeIn = value; }
        }
        [Display(Name = "Nơi Sản Xuất")]
        public string MadeInName
        {
            get { return _MainObj.MadeInName; }
            set { _MainObj.MadeInName = value; }
        }

        [Display(Name = "Màu sắc")]
        public string CorlorName
        {
            get { return _MainObj.CorlorName; }
            set { _MainObj.CorlorName = value; }
        }

        [Display(Name = "Màu Sắc")]
        public Nullable<int> CorlorID
        {
            get { return _MainObj.CorlorID; }
            set { _MainObj.CorlorID = value; }
        }

        [Display(Name = "Chiều Rộng")]
        public string With
        {
            get { return _MainObj.With; }
            set { _MainObj.With = value; }
        }

        [Display(Name = "Chiều cao")]
        public string Depth
        {
            get { return _MainObj.Depth; }
            set { _MainObj.Depth = value; }
        }

        [Display(Name = "Chiều dài")]
        public string Height
        {
            get { return _MainObj.Height; }
            set { _MainObj.Height = value; }
        }

        [Display(Name = "Trọng Lượng")]
        public string Weight
        {
            get { return _MainObj.Weight; }
            set { _MainObj.Weight = value; }
        }


        [Display(Name = "Chiều Rộng vận chuyển")]
        public string TranWith
        {
            get { return _MainObj.TranWith; }
            set { _MainObj.TranWith = value; }
        }

        [Display(Name = "Chiều cao vận chuyển")]
        public string TranDepth
        {
            get { return _MainObj.TranDepth; }
            set { _MainObj.TranDepth = value; }
        }

        [Display(Name = "Chiều dài vận chuyển")]
        public string TranHeight
        {
            get { return _MainObj.TranHeight; }
            set { _MainObj.TranHeight = value; }
        }

        [Display(Name = "Trọng Lượng vận chuyển")]
        public string TranWeight
        {
            get { return _MainObj.TranWeight; }
            set { _MainObj.TranWeight = value; }
        }


        public string StatusProductTypeName
        {
            get { return _MainObj.StatusProductTypeName; }
            set { _MainObj.StatusProductTypeName = value; }
        }

        [Display(Name = "Tình Trạng Sản Phẩm")]
        public Nullable<int> StatusProductType
        {
            get { return _MainObj.StatusProductType; }
            set { _MainObj.StatusProductType = value; }
        }


        public Nullable<System.DateTime> PromotionFrom
        {
            get { return _MainObj.PromotionFrom; }
            set { _MainObj.PromotionFrom = value; }
        }

        public Nullable<System.DateTime> PromotionTo
        {
            get { return _MainObj.PromotionTo; }
            set { _MainObj.PromotionTo = value; }
        }


        public Nullable<int> PromotionAmount
        {
            get { return _MainObj.PromotionAmount; }
            set { _MainObj.PromotionAmount = value; }
        }

        [Display(Name = "Thời Gian Bảo Hành")]
        public string SupportTimeName
        {
            get { return _MainObj.SupportTimeName; }
            set { _MainObj.SupportTimeName = value; }
        }

        [Display(Name = "Thời Gian Bảo Hành")]
        public Nullable<int> SupportTimeID
        {
            get { return _MainObj.SupportTimeID; }
            set { _MainObj.SupportTimeID = value; }
        }

        [Display(Name = "Hình Thức Giao Hàng")]
        public string TranTypeName
        {
            get { return _MainObj.TranTypeName; }
            set { _MainObj.TranTypeName = value; }
        }

        [Display(Name = "Hình Thức Giao Hàng")]
        public Nullable<int> TranTypeID
        {
            get { return _MainObj.TranTypeID; }
            set { _MainObj.TranTypeID = value; }
        }

        [Display(Name = "Mã SKU")]
        public string SKUCode
        {
            get { return _MainObj.SKUCode; }
            set { _MainObj.SKUCode = value; }
        }

        [Display(Name = "IsEvent")]
        public int? IsEvent
        {
            get { return _MainObj.IsEvent; }
            set { _MainObj.IsEvent = value; }
        }



        public long? MicrositeID
        {
            get { return _MainObj.MicrositeID; }
            set { _MainObj.MicrositeID = value; }
        }

        public string MicrositeName
        {
            get { return _MainObj.MicrositeName; }
            set { _MainObj.MicrositeName = value; }
        }



        public SelectList CatalogryList { get; set; }
        public SelectList MainCatalogryList { get; set; }
        public SelectList CateMicrositeList { get; set; }
        public SelectList BrandList { get; set; }
        public SelectList UnitsList { get; set; }
        public SelectList MadeInList { get; set; }
        public SelectList CorlorList { get; set; }
        public SelectList SupportTimeList { get; set; }
        public SelectList TranTypeList { get; set; }
        public SelectList Status_Productlst { get; set; }
        public SelectList lstSize { get; set; }


        [Display(Name = "Hình chi tiết")]
        public List<MediaContent> lstDetailImage { get; set; }

        [Display(Name = "Từ khoá liên quan")]
        public List<int> related_tag { get; set; }


        [Display(Name = "Hình đại diện")]
        public long ImgdefaultId { get; set; }


        public string[] ColorSku { get; set; }
        public int[] lstcbsize { get; set; }
        /// <summary>
        /// for edit
        /// </summary>
        public List<ProductColorSKUModels> lstColorSKU { get; set; }


        
    }

    public class ProductAdminViewModel
    {
        public IPagedList<Product> lstMainProduct { get; set; }
        public SelectList lstProductState { get; set; }


        public SelectList lstProductCatalogry { get; set; }

        public int pageNum { get; set; }
        public int ProductState { get; set; }
        public int ProductCatalogry { get; set; }
        public int FixFlag { get; set; }
        public int IsEventFlag { get; set; }



        public string FillterSKU { get; set; }
        public string FillterProductCD { get; set; }
        public string FillterProductName { get; set; }

        public string FriendlyUrlShop { get; set; }
        public long Idshop { get; set; }


    }


    public class PublicMicrositeViewModel
    {
        public IPagedList<Product> lstMainProduct { get; set; }
        public int pageNum { get; set; }
        public string FriendlyUrlShop { get; set; }
        public long Idshop { get; set; }
        public bool IsBossOfStore { get; set; }
        public string NameMicroshop { get; set; }
        public string ColorTop { get; set; }
        public string ColorBottom { get; set; }
        public string UrlAvartarMicroshop { get; set; }
        public long CountProduct { get; set; }
        public long CountProductDiscout { get; set; }
        public long CountNews { get; set; }
        public long CountOrder { get; set; }
        public List<DataContenComment> LstContenComment { get; set; }
    }

    public class PublicNewMicrositeViewModel
    {
        public IPagedList<ContentItem> lstMainNews { get; set; }
        public int pageNum { get; set; }
        public string FriendlyUrlShop { get; set; }
        public long Idshop { get; set; }


    }


    public class ProductUploadViewModels
    {
        public long ContentObjId { get; set; }
        public int ObjTypeId { get; set; }
        public IEnumerable<MediaContent> list { get; set; }
    }
    public class ProductAjaxViewModels
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductLink { get; set; }
        public string ProductImg { get; set; }
        public double Price { get; set; }
        public double NewPrice { get; set; }
    }


    public class ProductColorSKUModels
    {
        public long ProductId { get; set; }
        public string SKU { get; set; }
        public int color { get; set; }
        public string colorName { get; set; }
    }






}
