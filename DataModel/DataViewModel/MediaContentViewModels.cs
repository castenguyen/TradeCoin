using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace DataModel.DataViewModel
{
    public class MediaContentViewModels
    {
          public MediaContent objMediaContent { get; set; }
        public MediaContentViewModels()
        {
            objMediaContent = new MediaContent();
        }

        public MediaContentViewModels(MediaContent _objMediaContent)
        {
            objMediaContent = _objMediaContent;
        }

        public long MediaContentId
        {
            get { return objMediaContent.MediaContentId; }
            set { objMediaContent.MediaContentId = value; }
        }
        public string Filename
        {
            get { return objMediaContent.Filename; }
            set { objMediaContent.Filename = value; }
        }
        public string FullURL
        {
            get { return objMediaContent.FullURL; }
            set { objMediaContent.FullURL = value; }
        }
        public string ThumbURL
        {
            get { return objMediaContent.ThumbURL; }
            set { objMediaContent.ThumbURL = value; }
        }
        public string MetadataDesc
        {
            get { return objMediaContent.MetadataDesc; }
            set { objMediaContent.MetadataDesc = value; }
        }
        public string MetadataKeyword
        {
            get { return objMediaContent.MetadataKeyword; }
            set { objMediaContent.MetadataKeyword = value; }
        }
        public Nullable<long> MediaContentSize
        {
            get { return objMediaContent.MediaContentSize; }
            set { objMediaContent.MediaContentSize = value; }
        }
        public Nullable<System.DateTime> CrtdDT
        {
            get { return objMediaContent.CrtdDT; }
            set { objMediaContent.CrtdDT = value; }
        }
        public Nullable<System.DateTime> AprvdDT
        {
            get { return objMediaContent.AprvdDT; }
            set { objMediaContent.AprvdDT = value; }
        }
        public string EXIFInfo
        {
            get { return objMediaContent.EXIFInfo; }
            set { objMediaContent.EXIFInfo = value; }
        }
        public Nullable<int> MediaTypeId
        {
            get { return objMediaContent.MediaTypeId; }
            set { objMediaContent.MediaTypeId = value; }
        }
        public Nullable<int> ObjTypeId
        {
            get { return objMediaContent.ObjTypeId; }
            set { objMediaContent.ObjTypeId = value; }
        }
        public Nullable<long> AprvdUID
        {
            get { return objMediaContent.AprvdUID; }
            set { objMediaContent.AprvdUID = value; }
        }
        public Nullable<long> CrtdUID
        {
            get { return objMediaContent.CrtdUID; }
            set { objMediaContent.CrtdUID = value; }
        }
        public long ViewCount
        {
            get { return objMediaContent.ViewCount; }
            set { objMediaContent.ViewCount = value; }
        }
        public string Caption
        {
            get { return objMediaContent.Caption; }
            set { objMediaContent.Caption = value; }
        }
        public string AlternativeText
        {
            get { return objMediaContent.AlternativeText; }
            set { objMediaContent.AlternativeText = value; }
        }
        [AllowHtml]
        public string MediaDesc
        {
            get { return objMediaContent.MediaDesc; }
            set { objMediaContent.MediaDesc = value; }
        }
        public Nullable<long> ContentObjId
        {
            get { return objMediaContent.ContentObjId; }
            set { objMediaContent.ContentObjId = value; }
        }

        public string LinkHref
        {
            get { return objMediaContent.LinkHref; }
            set { objMediaContent.LinkHref = value; }
        }


        public List<ContentPackage> lstTickerContentPackage { get; set; }
        public List<MiniMediaViewModel> lstSameVideo { get; set; }
    }
    public class AlbumMediaViewModels
    {
        public Classification Album { get; set; }
        public string ImgAvatars { get; set; }
        public string ImgCrtdUser { get; set; }
        public int NbrPic { get; set; }

    }

    public class ImageUploadViewModel
    {
        public long MediaContentId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageThumbUrl { get; set; }
        public string ImageName { get; set; }
        public MediaContent objMediaContent { get; set; }
    }

    public class BannerViewModels 
    {
        public MediaContent objMediaContent { get; set; }
        public BannerViewModels()
        {
            objMediaContent = new MediaContent();
        }
        public BannerViewModels(MediaContent _objMediaContent)
        {
            objMediaContent = _objMediaContent;
        }
        public long MediaContentId
        {
            get { return objMediaContent.MediaContentId; }
            set { objMediaContent.MediaContentId = value; }
        }

        [Required]
        [Display(Name = "Tên Banner")]
        public string Filename
        {
            get { return objMediaContent.Filename; }
            set { objMediaContent.Filename = value; }
        }
        public string FullURL
        {
            get { return objMediaContent.FullURL; }
            set { objMediaContent.FullURL = value; }
        }
        public string ThumbURL
        {
            get { return objMediaContent.ThumbURL; }
            set { objMediaContent.ThumbURL = value; }
        }
        public string MetadataDesc
        {
            get { return objMediaContent.MetadataDesc; }
            set { objMediaContent.MetadataDesc = value; }
        }
        public string MetadataKeyword
        {
            get { return objMediaContent.MetadataKeyword; }
            set { objMediaContent.MetadataKeyword = value; }
        }
        public Nullable<long> MediaContentSize
        {
            get { return objMediaContent.MediaContentSize; }
            set { objMediaContent.MediaContentSize = value; }
        }
        public Nullable<System.DateTime> CrtdDT
        {
            get { return objMediaContent.CrtdDT; }
            set { objMediaContent.CrtdDT = value; }
        }
        public Nullable<System.DateTime> AprvdDT
        {
            get { return objMediaContent.AprvdDT; }
            set { objMediaContent.AprvdDT = value; }
        }
        public string EXIFInfo
        {
            get { return objMediaContent.EXIFInfo; }
            set { objMediaContent.EXIFInfo = value; }
        }
        public Nullable<int> MediaTypeId
        {
            get { return objMediaContent.MediaTypeId; }
            set { objMediaContent.MediaTypeId = value; }
        }
        public Nullable<int> ObjTypeId
        {
            get { return objMediaContent.ObjTypeId; }
            set { objMediaContent.ObjTypeId = value; }
        }
        public Nullable<long> AprvdUID
        {
            get { return objMediaContent.AprvdUID; }
            set { objMediaContent.AprvdUID = value; }
        }
        public Nullable<long> CrtdUID
        {
            get { return objMediaContent.CrtdUID; }
            set { objMediaContent.CrtdUID = value; }
        }
        public long ViewCount
        {
            get { return objMediaContent.ViewCount; }
            set { objMediaContent.ViewCount = value; }
        }
        public string Caption
        {
            get { return objMediaContent.Caption; }
            set { objMediaContent.Caption = value; }
        }
        public string AlternativeText
        {
            get { return objMediaContent.AlternativeText; }
            set { objMediaContent.AlternativeText = value; }
        }
        [AllowHtml]
        public string MediaDesc
        {
            get { return objMediaContent.MediaDesc; }
            set { objMediaContent.MediaDesc = value; }
        }
        public Nullable<long> ContentObjId
        {
            get { return objMediaContent.ContentObjId; }
            set { objMediaContent.ContentObjId = value; }
        }

        public string LinkHref
        {
            get { return objMediaContent.LinkHref; }
            set { objMediaContent.LinkHref = value; }
        }

        [Required]
        [Display(Name = "Hình đại diện")]
        public long ImgdefaultId { get; set; }
    }

    public class DetailAlbumViewModel 
    {
        public Classification Album { get; set; }
        public List<MediaContent> lstImage { get; set; }
    
    }

    public class BannerMicroViewModels
    {
        public MediaContent _MainObj { get; set; }
        public BannerMicroViewModels()
        {
            _MainObj = new MediaContent();
        }
        public BannerMicroViewModels(MediaContent _objMediaContent)
        {
            _MainObj = _objMediaContent;
        }
        public long MediaContentId
        {
            get { return _MainObj.MediaContentId; }
            set { _MainObj.MediaContentId = value; }
        }
        [Display(Name = "Tên Banner")]
        public string Filename
        {
            get { return _MainObj.Filename; }
            set { _MainObj.Filename = value; }
        }
        public string FullURL
        {
            get { return _MainObj.FullURL; }
            set { _MainObj.FullURL = value; }
        }
        public string ThumbURL
        {
            get { return _MainObj.ThumbURL; }
            set { _MainObj.ThumbURL = value; }
        }
        public string MetadataDesc
        {
            get { return _MainObj.MetadataDesc; }
            set { _MainObj.MetadataDesc = value; }
        }
        public string MetadataKeyword
        {
            get { return _MainObj.MetadataKeyword; }
            set { _MainObj.MetadataKeyword = value; }
        }
        public Nullable<long> MediaContentSize
        {
            get { return _MainObj.MediaContentSize; }
            set { _MainObj.MediaContentSize = value; }
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
        public string EXIFInfo
        {
            get { return _MainObj.EXIFInfo; }
            set { _MainObj.EXIFInfo = value; }
        }
        public Nullable<int> MediaTypeId
        {
            get { return _MainObj.MediaTypeId; }
            set { _MainObj.MediaTypeId = value; }
        }
        public Nullable<int> ObjTypeId
        {
            get { return _MainObj.ObjTypeId; }
            set { _MainObj.ObjTypeId = value; }
        }
        public Nullable<long> AprvdUID
        {
            get { return _MainObj.AprvdUID; }
            set { _MainObj.AprvdUID = value; }
        }
        public Nullable<long> CrtdUID
        {
            get { return _MainObj.CrtdUID; }
            set { _MainObj.CrtdUID = value; }
        }
        public long ViewCount
        {
            get { return _MainObj.ViewCount; }
            set { _MainObj.ViewCount = value; }
        }
        public string Caption
        {
            get { return _MainObj.Caption; }
            set { _MainObj.Caption = value; }
        }
        public string AlternativeText
        {
            get { return _MainObj.AlternativeText; }
            set { _MainObj.AlternativeText = value; }
        }
        [AllowHtml]
        public string MediaDesc
        {
            get { return _MainObj.MediaDesc; }
            set { _MainObj.MediaDesc = value; }
        }
        public Nullable<long> ContentObjId
        {
            get { return _MainObj.ContentObjId; }
            set { _MainObj.ContentObjId = value; }
        }
        [Required]
        [Display(Name = "Vị trí hiên thị")]
        public long DisplayTypeId { get; set; }
        public string LinkHref
        {
            get { return _MainObj.LinkHref; }
            set { _MainObj.LinkHref = value; }
        }
        public SelectList DisplayBlockMicro { get; set; }
        public string FriendLyUrlShop { get; set; }
        public long MicrositeId { get; set; }
       
    }

    public class VideoViewModels
    {
        public MediaContent objMediaContent { get; set; }
        public VideoViewModels()
        {
            objMediaContent = new MediaContent();
        }
        public VideoViewModels(MediaContent _objMediaContent)
        {
            objMediaContent = _objMediaContent;
        }
        public long MediaContentId
        {
            get { return objMediaContent.MediaContentId; }
            set { objMediaContent.MediaContentId = value; }
        }

        [Required]
        [Display(Name = "Tên Video")]
        public string Filename
        {
            get { return objMediaContent.Filename; }
            set { objMediaContent.Filename = value; }
        }
        public string FullURL
        {
            get { return objMediaContent.FullURL; }
            set { objMediaContent.FullURL = value; }
        }
        public string ThumbURL
        {
            get { return objMediaContent.ThumbURL; }
            set { objMediaContent.ThumbURL = value; }
        }


        public string MetadataDesc
        {
            get { return objMediaContent.MetadataDesc; }
            set { objMediaContent.MetadataDesc = value; }
        }
        public string MetadataKeyword
        {
            get { return objMediaContent.MetadataKeyword; }
            set { objMediaContent.MetadataKeyword = value; }
        }
        public Nullable<long> MediaContentSize
        {
            get { return objMediaContent.MediaContentSize; }
            set { objMediaContent.MediaContentSize = value; }
        }
        public Nullable<System.DateTime> CrtdDT
        {
            get { return objMediaContent.CrtdDT; }
            set { objMediaContent.CrtdDT = value; }
        }
        public Nullable<System.DateTime> AprvdDT
        {
            get { return objMediaContent.AprvdDT; }
            set { objMediaContent.AprvdDT = value; }
        }
        public string EXIFInfo
        {
            get { return objMediaContent.EXIFInfo; }
            set { objMediaContent.EXIFInfo = value; }
        }
        public Nullable<int> MediaTypeId
        {
            get { return objMediaContent.MediaTypeId; }
            set { objMediaContent.MediaTypeId = value; }
        }
        public Nullable<int> ObjTypeId
        {
            get { return objMediaContent.ObjTypeId; }
            set { objMediaContent.ObjTypeId = value; }
        }
        public Nullable<long> AprvdUID
        {
            get { return objMediaContent.AprvdUID; }
            set { objMediaContent.AprvdUID = value; }
        }
        public Nullable<long> CrtdUID
        {
            get { return objMediaContent.CrtdUID; }
            set { objMediaContent.CrtdUID = value; }
        }
        public long ViewCount
        {
            get { return objMediaContent.ViewCount; }
            set { objMediaContent.ViewCount = value; }
        }
        public string Caption
        {
            get { return objMediaContent.Caption; }
            set { objMediaContent.Caption = value; }
        }

        [Display(Name = "Tên Hiển Thị")]
        public string AlternativeText
        {
            get { return objMediaContent.AlternativeText; }
            set { objMediaContent.AlternativeText = value; }
        }
        [AllowHtml]
        public string MediaDesc
        {
            get { return objMediaContent.MediaDesc; }
            set { objMediaContent.MediaDesc = value; }
        }
        public Nullable<long> ContentObjId
        {
            get { return objMediaContent.ContentObjId; }
            set { objMediaContent.ContentObjId = value; }
        }
        [Display(Name = "Đường dẫn Video")]
        public string LinkHref
        {
            get { return objMediaContent.LinkHref; }
            set { objMediaContent.LinkHref = value; }
        }

        [Required]
        [Display(Name = "Hình đại diện")]
        public long ImgdefaultId { get; set; }

        [Display(Name = "Danh Mục")]
        public SelectList CatalogryList { get; set; }

        public List<SelectListObj> lstPackage { get; set; }

        public long[] lstTickerPackage { get; set; }
    }
}
