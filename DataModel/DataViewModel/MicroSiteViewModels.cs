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
    public class MicroSiteViewModels
    {
        public Microsite _ModelObj { get; set; }
        public MicroSiteViewModels()
        {
            _ModelObj = new Microsite();

        }
        public MicroSiteViewModels(Microsite model)
        {
            _ModelObj = model;

        }

        public long Id
        {
            get { return _ModelObj.Id; }
            set { _ModelObj.Id = value; }
        }

        [Required]
        [Display(Name = "Tên Gian Hàng")]
        public string Name
        {
            get { return _ModelObj.Name; }
            set { _ModelObj.Name = value; }
        }
        public Nullable<System.DateTime> CreationDate
        {
            get { return _ModelObj.CreationDate; }
            set { _ModelObj.CreationDate = value; }
        }

        [Display(Name = "Chủ Sở Hữu")]
        public string CreatUserName
        {
            get { return _ModelObj.CreatUserName; }
            set { _ModelObj.CreatUserName = value; }
        }
        public Nullable<long> CreatUserId
        {
            get { return _ModelObj.CreatUserId; }
            set { _ModelObj.CreatUserId = value; }
        }
        public Nullable<System.DateTime> ApprovalDate
        {
            get { return _ModelObj.ApprovalDate; }
            set { _ModelObj.ApprovalDate = value; }
        }
        public string ApprUserName
        {
            get { return _ModelObj.ApprUserName; }
            set { _ModelObj.ApprUserName = value; }
        }
        public Nullable<long> ApprUserId
        {
            get { return _ModelObj.ApprUserId; }
            set { _ModelObj.ApprUserId = value; }
        }
        [Required]
        [Display(Name = "Địa Chỉ")]
        public string FullAdress
        {
            get { return _ModelObj.FullAdress; }
            set { _ModelObj.FullAdress = value; }
        }

        [Display(Name = "Điện Thoại Bàn")]
        public string HomePhone
        {
            get { return _ModelObj.HomePhone; }
            set { _ModelObj.HomePhone = value; }
        }

        [Display(Name = "Điện Thoại Cá Nhân")]
        public string MobilePhone
        {
            get { return _ModelObj.MobilePhone; }
            set { _ModelObj.MobilePhone = value; }
        }

        [Display(Name = "Fax")]
        public string Fax
        {
            get { return _ModelObj.Fax; }
            set { _ModelObj.Fax = value; }
        }

        [Display(Name = "Facebook")]
        public string Facebook
        {
            get { return _ModelObj.Facebook; }
            set { _ModelObj.Facebook = value; }
        }

        [Display(Name = "Google")]
        public string Google
        {
            get { return _ModelObj.Google; }
            set { _ModelObj.Google = value; }
        }
        public string MapCode
        {
            get { return _ModelObj.MapCode; }
            set { _ModelObj.MapCode = value; }
        }

        [Display(Name = "Email")]
        public string Email
        {
            get { return _ModelObj.Email; }
            set { _ModelObj.Email = value; }
        }

        [Display(Name = "Website")]
        public string Website
        {
            get { return _ModelObj.Website; }
            set { _ModelObj.Website = value; }
        }
        public string LicensePP
        {
            get { return _ModelObj.LicensePP; }
            set { _ModelObj.LicensePP = value; }
        }

        [Display(Name = "Giấy Phép Kinh Doanh")]
        public string LicenseNbr
        {
            get { return _ModelObj.LicenseNbr; }
            set { _ModelObj.LicenseNbr = value; }
        }

        [Display(Name = "Ngày Cấp Giấy Phép")]
        public Nullable<System.DateTime> LicenseDate
        {
            get { return _ModelObj.LicenseDate; }
            set { _ModelObj.LicenseDate = value; }
        }

        [Display(Name = "Nơi Cấp Giấy Phép")]
        public string LicenseLocation
        {
            get { return _ModelObj.LicenseLocation; }
            set { _ModelObj.LicenseLocation = value; }
        }

        [Display(Name = "CMND")]
        public string CMDDNbr
        {
            get { return _ModelObj.CMDDNbr; }
            set { _ModelObj.CMDDNbr = value; }
        }

        [Display(Name = "Ngày Cấp CMND")]
        public Nullable<System.DateTime> CMDDDate
        {
            get { return _ModelObj.CMDDDate; }
            set { _ModelObj.CMDDDate = value; }
        }

        [Display(Name = "Nơi Cấp CMND")]
        public string CMDDLocation
        {
            get { return _ModelObj.CMDDLocation; }
            set { _ModelObj.CMDDLocation = value; }
        }
        public Nullable<int> Type
        {
            get { return _ModelObj.Type; }
            set { _ModelObj.Type = value; }
        }
        public string TypeName
        {
            get { return _ModelObj.TypeName; }
            set { _ModelObj.TypeName = value; }
        }
        [Required]
        [Display(Name = "Gian Hàng Kinh Doanh")]
        public Nullable<int> CataType
        {
            get { return _ModelObj.CataType; }
            set { _ModelObj.CataType = value; }
        }

        public string CataTypeName
        {
            get { return _ModelObj.CataTypeName; }
            set { _ModelObj.CataTypeName = value; }
        }
        public Nullable<int> NbrOfProduct
        {
            get { return _ModelObj.NbrOfProduct; }
            set { _ModelObj.NbrOfProduct = value; }
        }
        public Nullable<int> NbrOfSussDeal
        {
            get { return _ModelObj.NbrOfSussDeal; }
            set { _ModelObj.NbrOfSussDeal = value; }
        }

        public string ImgLogo
        {
            get { return _ModelObj.ImgLogo; }
            set { _ModelObj.ImgLogo = value; }
        }
        public string ImgLogoThumb
        {
            get { return _ModelObj.ImgLogoThumb; }
            set { _ModelObj.ImgLogoThumb = value; }
        }


        [Display(Name = "Mô Tả Gian Hàng")]
        public string Description
        {
            get { return _ModelObj.Description; }
            set { _ModelObj.Description = value; }
        }

        [Display(Name = "Từ Khoá SEO")]
        public string Metakeyword
        {
            get { return _ModelObj.Metakeyword; }
            set { _ModelObj.Metakeyword = value; }
        }
        [Display(Name = "Mô Tả SEO")]
        public string MetaDescription
        {
            get { return _ModelObj.MetaDescription; }
            set { _ModelObj.MetaDescription = value; }
        }

        public SelectList SelectCatalogry { get; set; }
        public SelectList SelectDistrictLis { get; set; }
        public string CMDDDatetxt { get; set; }
        public string LicenseDatetxt { get; set; }
        public string CreationDatetxt { get; set; }

    }
    public class MenuLeftPartialViewModel
    {
        public User User { get; set; }
        public int MenuFlag { get; set; }
        public long IdShop { get; set; }
        public string FriendlyUrlShop { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
        public bool isShowProfile { get; set; }
    }
    public class MyStoreViewModel
    {
        public Microsite _ModelObj { get; set; }
        public MyStoreViewModel()
        {
            _ModelObj = new Microsite();

        }
        public MyStoreViewModel(Microsite model)
        {
            _ModelObj = model;

        }

        public long Id
        {
            get { return _ModelObj.Id; }
            set { _ModelObj.Id = value; }
        }

        [Required]
        [Display(Name = "Tên Gian Hàng")]
        public string Name
        {
            get { return _ModelObj.Name; }
            set { _ModelObj.Name = value; }
        }
        public Nullable<System.DateTime> CreationDate
        {
            get { return _ModelObj.CreationDate; }
            set { _ModelObj.CreationDate = value; }
        }
        [Required]
        [Display(Name = "Chủ Sở Hữu")]
        public string CreatUserName
        {
            get { return _ModelObj.CreatUserName; }
            set { _ModelObj.CreatUserName = value; }
        }
        public Nullable<long> CreatUserId
        {
            get { return _ModelObj.CreatUserId; }
            set { _ModelObj.CreatUserId = value; }
        }
        public Nullable<System.DateTime> ApprovalDate
        {
            get { return _ModelObj.ApprovalDate; }
            set { _ModelObj.ApprovalDate = value; }
        }
        public string ApprUserName
        {
            get { return _ModelObj.ApprUserName; }
            set { _ModelObj.ApprUserName = value; }
        }

        public string FriendlyUrl
        {
            get { return _ModelObj.FriendlyUrl; }
            set { _ModelObj.FriendlyUrl = value; }
        }

        public Nullable<long> ApprUserId
        {
            get { return _ModelObj.ApprUserId; }
            set { _ModelObj.ApprUserId = value; }
        }
        [Required]
        [Display(Name = "Địa Chỉ")]
        public string FullAdress
        {
            get { return _ModelObj.FullAdress; }
            set { _ModelObj.FullAdress = value; }
        }
        [Required]
        [Display(Name = "Điện Thoại Bàn")]
        public string HomePhone
        {
            get { return _ModelObj.HomePhone; }
            set { _ModelObj.HomePhone = value; }
        }
        [Required]
        [Display(Name = "Điện Thoại Cá Nhân")]
        public string MobilePhone
        {
            get { return _ModelObj.MobilePhone; }
            set { _ModelObj.MobilePhone = value; }
        }
        [Required]
        [Display(Name = "Fax")]
        public string Fax
        {
            get { return _ModelObj.Fax; }
            set { _ModelObj.Fax = value; }
        }
        [Required]
        [Display(Name = "Facebook")]
        public string Facebook
        {
            get { return _ModelObj.Facebook; }
            set { _ModelObj.Facebook = value; }
        }
        [Required]
        [Display(Name = "Google")]
        public string Google
        {
            get { return _ModelObj.Google; }
            set { _ModelObj.Google = value; }
        }
        public string MapCode
        {
            get { return _ModelObj.MapCode; }
            set { _ModelObj.MapCode = value; }
        }
        [Required]
        [Display(Name = "Email")]
        public string Email
        {
            get { return _ModelObj.Email; }
            set { _ModelObj.Email = value; }
        }
        [Required]
        [Display(Name = "Website")]
        public string Website
        {
            get { return _ModelObj.Website; }
            set { _ModelObj.Website = value; }
        }
        public string LicensePP
        {
            get { return _ModelObj.LicensePP; }
            set { _ModelObj.LicensePP = value; }
        }
        [Required]
        [Display(Name = "Giấy Phép Kinh Doanh")]
        public string LicenseNbr
        {
            get { return _ModelObj.LicenseNbr; }
            set { _ModelObj.LicenseNbr = value; }
        }
        [Required]
        [Display(Name = "Ngày Cấp Giấy Phép")]
        public Nullable<System.DateTime> LicenseDate
        {
            get { return _ModelObj.LicenseDate; }
            set { _ModelObj.LicenseDate = value; }
        }
        [Required]
        [Display(Name = "Nơi Cấp Giấy Phép")]
        public string LicenseLocation
        {
            get { return _ModelObj.LicenseLocation; }
            set { _ModelObj.LicenseLocation = value; }
        }
        [Required]
        [Display(Name = "CMND")]
        public string CMDDNbr
        {
            get { return _ModelObj.CMDDNbr; }
            set { _ModelObj.CMDDNbr = value; }
        }
        [Required]
        [Display(Name = "Ngày Cấp CMND")]
        public Nullable<System.DateTime> CMDDDate
        {
            get { return _ModelObj.CMDDDate; }
            set { _ModelObj.CMDDDate = value; }
        }
        [Required]
        [Display(Name = "Nơi Cấp CMND")]
        public string CMDDLocation
        {
            get { return _ModelObj.CMDDLocation; }
            set { _ModelObj.CMDDLocation = value; }
        }
        public Nullable<int> Type
        {
            get { return _ModelObj.Type; }
            set { _ModelObj.Type = value; }
        }
        public string TypeName
        {
            get { return _ModelObj.TypeName; }
            set { _ModelObj.TypeName = value; }
        }
        [Required]
        [Display(Name = "Gian Hàng Kinh Doanh")]
        public Nullable<int> CataType
        {
            get { return _ModelObj.CataType; }
            set { _ModelObj.CataType = value; }
        }

        public string CataTypeName
        {
            get { return _ModelObj.CataTypeName; }
            set { _ModelObj.CataTypeName = value; }
        }
        public Nullable<int> NbrOfProduct
        {
            get { return _ModelObj.NbrOfProduct; }
            set { _ModelObj.NbrOfProduct = value; }
        }
        public Nullable<int> NbrOfSussDeal
        {
            get { return _ModelObj.NbrOfSussDeal; }
            set { _ModelObj.NbrOfSussDeal = value; }
        }

        public string ImgLogo
        {
            get { return _ModelObj.ImgLogo; }
            set { _ModelObj.ImgLogo = value; }
        }
        public string ImgLogoThumb
        {
            get { return _ModelObj.ImgLogoThumb; }
            set { _ModelObj.ImgLogoThumb = value; }
        }

        [Required]
        [Display(Name = "Mô Tả Gian Hàng")]
        public string Description
        {
            get { return _ModelObj.Description; }
            set { _ModelObj.Description = value; }
        }

        [Display(Name = "Từ Khoá SEO")]
        public string Metakeyword
        {
            get { return _ModelObj.Metakeyword; }
            set { _ModelObj.Metakeyword = value; }
        }
        [Display(Name = "Mô Tả SEO")]
        public string MetaDescription
        {
            get { return _ModelObj.MetaDescription; }
            set { _ModelObj.MetaDescription = value; }
        }

        public long NbrPro { get; set; }
        public long NbrProE { get; set; }
        public long NbrProD { get; set; }

        public long NbrNews { get; set; }
        public long NbrNewsE { get; set; }
        public long NbrNewsD { get; set; }

        public long NbrOrder { get; set; }
        public long NbrOrderE { get; set; }
        public long NbrOrderD { get; set; }

    }
    public class PublicMenuMicroPartialViewModel
    {
        public List<MicroClassification> lstMainCate { get; set; }
        // nhu thang list tren nhung trong day co them count san pham cua mot menu trong microshop
        public List<ItemMenuAndCountProductMicro> lstItemMenuMicroshop { get; set; }
        public long IdShop { get; set; }
        public string FriendlyUrlShop { get; set; }
        public string action { get; set; }
        public string controller { get; set; }
        public List<ContentItem> lstNews { get; set; }
        public Microsite ObjMicrosite { get; set; }
        //public ContentItem _gioithieu { get; set; }
        //public ContentItem _chinhsach { get; set; }
        //public ContentItem _lienhe { get; set; }
    }

    public class ItemMenuAndCountProductMicro
    {
        public long CountProduct { get; set; }
        public MicroClassification MicroClassification { get; set; }
    }

    public class ListProductMicroSiteViewModel
    {
        public MicroClassification CataObj { get; set; }
        public IPagedList<Product> lstProduct { get; set; }
        public int pageNum { get; set; }
        public long IdShop { get; set; }
        public string FriendlyUrlShop { get; set; }

    }

    public class DashboardMicrositeViewmodel
    {
        public Microsite _ModelObj { get; set; }

        public int NbrPro { get; set; }
        public int NbrProE { get; set; }
        public int NbrProD { get; set; }
        public int NbrProW { get; set; }

        public int NbrNews { get; set; }
        public int NbrNewsE { get; set; }
        public int NbrNewsD { get; set; }
        public int NbrNewsW { get; set; }

        public int NbrOrder { get; set; }
        public int NbrOrderE { get; set; }
        public int NbrOrderD { get; set; }
        public int NbrOrderW { get; set; }

        public string FriendlyUrlShop { get; set; }
        public long IdShop { get; set; }
    }


}
