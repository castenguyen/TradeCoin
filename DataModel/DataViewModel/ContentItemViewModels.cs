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
namespace DataModel.DataViewModel
{
    public class ContentItemViewModels
    {
           public ContentItem _MainObj { get; set; }
        public ContentItemViewModels()
        {
            _MainObj = new ContentItem();

        }
        public ContentItemViewModels(ContentItem model)
        {
            _MainObj = model;

        }
        public long ContentItemId
        {
            get { return _MainObj.ContentItemId; }
            set { _MainObj.ContentItemId = value; }
        }

        [Required]
        [Display(Name = "Đường dẩn thân thiện")]
        public string FriendlyURL
        {
            get { return _MainObj.FriendlyURL; }
            set { _MainObj.FriendlyURL = value; }
        }
        public Nullable<long> ParentId
        {
            get { return _MainObj.ParentId; }
            set { _MainObj.ParentId = value; }
        }
        public string ContentItemCD
        {
            get { return _MainObj.ContentItemCD; }
            set { _MainObj.ContentItemCD = value; }
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
        public Nullable<int> ObjTypeId
        {
            get { return _MainObj.ObjTypeId; }
            set { _MainObj.ObjTypeId = value; }
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
        public string ContentText
        {
            get { return _MainObj.ContentText; }
            set { _MainObj.ContentText = value; }
        }

        [Required]
        [Display(Name = "Tên bài viết")]
        public string ContentTitle
        {
            get { return _MainObj.ContentTitle; }
            set { _MainObj.ContentTitle = value; }
        }

        [Required]
        [Display(Name = "Mô tả")]
        public string ContentExcerpt
        {
            get { return _MainObj.ContentExcerpt; }
            set { _MainObj.ContentExcerpt = value; }
        }

        [Required]
        [Display(Name = "Mô tả SEO")]
        public string MetadataDesc
        {
            get { return _MainObj.MetadataDesc; }
            set { _MainObj.MetadataDesc = value; }
        }


        //public Nullable<System.DateTime> LastCommentDT
        //{
        //    get { return _MainObj.LastCommentDT; }
        //    set { _MainObj.LastCommentDT = value; }
        //}

        [Required]
        [Display(Name = "Từ Khoá SEO")]
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
        //public Nullable<byte> Indexed
        //{
        //    get { return _MainObj.Indexed; }
        //    set { _MainObj.Indexed = value; }
        //}
        public Nullable<long> StateId
        {
            get { return _MainObj.StateId; }
            set { _MainObj.StateId = value; }
        }

        public Nullable<long> MicrositeID
        {
            get { return _MainObj.MicrositeID; }
            set { _MainObj.MicrositeID = value; }
        }
        public string MicrositeName
        {
            get { return _MainObj.MicrositeName; }
            set { _MainObj.MicrositeName = value; }
        }

        [Display(Name = "Danh Mục")]
        public SelectList CatalogryList { get; set; }

        [Display(Name = "Nội dung liên quan")]
        public List<int> related_content { get; set; }

        [Display(Name = "Từ khoá liên quan")]
        public List<int> related_tag { get; set; }

        [Required]
        [Display(Name = "Hình đại diện")]
        public long ImgdefaultId { get; set; }

        public string FriendlyUrlShop { get; set; }

        public List<SelectListObj> lstPackage { get; set; }

        public long[] lstTickerPackage { get; set; }

        public List<ContentPackage> lstNewsContentPackage { get; set; }
        public List<ContentItem> lstSameNews { get; set; }

    }

    public class ContentRelatedViewModels
    {
        public RelatedContentItem _MainObj { get; set; }
        public ContentRelatedViewModels()
        {
            _MainObj = new RelatedContentItem();

        }
        public ContentRelatedViewModels(RelatedContentItem model)
        {
            _MainObj = model;

        }

        [Display(Name = "RelatedContent")]
        public List<int> RelatedContent { get; set; }
    
    }

    public class SelectListViewModels
    {
        public long Value {get;set;  }
        public string Name { get; set; }
    }

    public class PartialUploadViewModels
    {
        public long MediaId { get; set; }
        public string FullUrl { get; set; }
        public string ThumbUrl { get; set; }
    }

    public class ContentItemIndexViewModel
    {
        public IPagedList<ContentItem> lstMainContent { get; set; }
        public List<Ticker> lstTicker { get; set; }
        public List<SelectListObj> lstContentState { get; set; }
        public List<SelectListObj> lstContentCatalogry { get; set; }

        public int pageNum { get; set; }
        public int ContentState { get; set; }
        public int ContentCatalogry { get; set; }

        public string FriendlyUrlShop { get; set; }
        public long Idshop { get; set; }

    }



}
