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
    public class ContentCommentViewModels
    {
        public ContentComment _ModelObj { get; set; }
        public ContentCommentViewModels()
        {
            _ModelObj = new ContentComment();
        }

        public ContentCommentViewModels(ContentComment model)
        {
            _ModelObj = model;

        }

        public long CommentId
        {
            get { return _ModelObj.CommentId; }
            set { _ModelObj.CommentId = value; }
        }

        public Nullable<long> ParentCommentId
        {
            get { return _ModelObj.ParentCommentId; }
            set { _ModelObj.ParentCommentId = value; }
        }

        public Nullable<long> ContentObjId
        {
            get { return _ModelObj.ContentObjId; }
            set { _ModelObj.ContentObjId = value; }
        }

        [Required]
        [Display(Name = "Title")]
        public string Title
        {
            get { return _ModelObj.Title; }
            set { _ModelObj.Title = value; }
        }

        [Required]
        [Display(Name = "CommentText")]
        public string CommentText
        {
            get { return _ModelObj.CommentText; }
            set { _ModelObj.CommentText = value; }
        }

        [Required]
        [Display(Name = "FullName")]
        public string FullName
        {
            get { return _ModelObj.FullName; }
            set { _ModelObj.FullName = value; }
        }

        [Required]
        [Display(Name = "EmailAddress")]
        public string EmailAddress
        {
            get { return _ModelObj.EmailAddress; }
            set { _ModelObj.EmailAddress = value; }
        }


        public Nullable<System.DateTime> CrtdDT
        {
            get { return _ModelObj.CrtdDT; }
            set { _ModelObj.CrtdDT = value; }
        }


        public Nullable<System.DateTime> AprvdDT
        {
            get { return _ModelObj.AprvdDT; }
            set { _ModelObj.AprvdDT = value; }
        }

        public Nullable<long> StateId
        {
            get { return _ModelObj.StateId; }
            set { _ModelObj.StateId = value; }
        }

        public Nullable<Guid> CrtdGuestUserId
        {
            get { return _ModelObj.CrtdGuestUserId; }
            set { _ModelObj.CrtdGuestUserId = value; }
        }

        public Nullable<long> LikeCount
        {
            get { return _ModelObj.LikeCount; }
            set { _ModelObj.LikeCount = value; }
        }

        public Nullable<System.DateTime> LastLikedDT
        {
            get { return _ModelObj.LastLikedDT; }
            set { _ModelObj.LastLikedDT = value; }
        }

        public Nullable<long> AprvdUID
        {
            get { return _ModelObj.AprvdUID; }
            set { _ModelObj.AprvdUID = value; }
        }

        public string IPAddress
        {
            get { return _ModelObj.IPAddress; }
            set { _ModelObj.IPAddress = value; }
        }

        public Nullable<int> ObjTypeId
        {
            get { return _ModelObj.ObjTypeId; }
            set { _ModelObj.ObjTypeId = value; }
        }

    }

    public class CommentBlockViewModel
    {
        public ContentComment MainObj { get; set; }
        public List<ContentComment> ListChild { get; set; }

    }
    public class CommentPartialViewModel
    {
        public int NbrComment { get; set; }
        public int ObjTypeID { get; set; }
        public long MainContentID { get; set; }
        public string MainContentName { get; set; }
        public List<CommentBlockViewModel> ListMainObj { get; set; }
    }
    public class ContentCommentIndexViewModels
    {
        public IPagedList<ContentComment> lstMainComment { get; set; }
        public List<SelectListObj> lstCommentState { get; set; }

        public int pageNum { get; set; }
        public int CommentState { get; set; }
    }

    public class DataContenComment
    {
        public ItemDataContenComment Parent { get; set; }
        public List<ItemDataContenComment> LstChild { get; set; }
    }

    public class ItemDataContenComment
    {
        public long CommentId { get; set; }
        public string FullName { get; set; }
        public string ImgUser { get; set; }
        public string Comment { get; set; }
        public long LikeCount { get; set; }
    }

}
