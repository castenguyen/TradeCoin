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
    public class EmailSupportViewModel
    {
        public EmailSupport _MainObj { get; set; }
        public EmailSupportViewModel()
        {
            _MainObj = new EmailSupport();

        }
        public EmailSupportViewModel(EmailSupport model)
        {
            _MainObj = model;

        }




        public long EmailId
        {
            get { return _MainObj.EmailId; }
            set { _MainObj.EmailId = value; }
        }
        public string EmailName
        {
            get { return _MainObj.EmailName; }
            set { _MainObj.EmailName = value; }
        }
        [Display(Name = "Tiêu đề thư")]
        public string Subject
        {
            get { return _MainObj.Subject; }
            set { _MainObj.Subject = value; }
        }

        [Required]
        [AllowHtml]
        [Display(Name = "Nội dung")]
        public string Content
        {
            get { return _MainObj.Content; }
            set { _MainObj.Content = value; }
        }
        public Nullable<long> DestinationId
        {
            get { return _MainObj.DestinationId; }
            set { _MainObj.DestinationId = value; }
        }
        public string DestinationName
        {
            get { return _MainObj.DestinationName; }
            set { _MainObj.DestinationName = value; }
        }
        public Nullable<long> ParentId
        {
            get { return _MainObj.ParentId; }
            set { _MainObj.ParentId = value; }
        }
        public string ParentName
        {
            get { return _MainObj.ParentName; }
            set { _MainObj.ParentName = value; }
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
        public Nullable<System.DateTime> CrtdDT
        {
            get { return _MainObj.CrtdDT; }
            set { _MainObj.CrtdDT = value; }
        }
        public string AprvdUserName
        {
            get { return _MainObj.AprvdUserName; }
            set { _MainObj.AprvdUserName = value; }
        }
        public Nullable<long> AprvdUID
        {
            get { return _MainObj.AprvdUID; }
            set { _MainObj.AprvdUID = value; }
        }
        public Nullable<System.DateTime> AprvdDT
        {
            get { return _MainObj.AprvdDT; }
            set { _MainObj.AprvdDT = value; }
        }
        public string StateName
        {
            get { return _MainObj.StateName; }
            set { _MainObj.StateName = value; }
        }
        public Nullable<long> StateId
        {
            get { return _MainObj.StateId; }
            set { _MainObj.StateId = value; }
        }
        public Nullable<long> EmailTypeId
        {
            get { return _MainObj.EmailTypeId; }
            set { _MainObj.EmailTypeId = value; }
        }
        public string EmailTypeName
        {
            get { return _MainObj.EmailTypeName; }
            set { _MainObj.EmailTypeName = value; }
        }

        public string StateName2
        {
            get { return _MainObj.StateName2; }
            set { _MainObj.StateName2 = value; }
        }
        public Nullable<long> StateId2
        {
            get { return _MainObj.StateId2; }
            set { _MainObj.StateId2 = value; }
        }
        public List<EmailSupport> lstChild { get; set; }
    }

    public class EmailSupportIndexViewModel
    {
     
        public IPagedList<EmailSupport> lstEmailSupport { get; set; }
        public int EmailStatus { get; set; }
        public int pageNum { get; set; }
        public long[]  lstViewed { get; set; }
    }

    public class MiniEmailSupportViewModel
    {
        public long EmailId { get; set; }
        public string EmailName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Nullable<long> DestinationId { get; set; }
        public string DestinationName { get; set; }
        public Nullable<long> ParentId { get; set; }
        public string ParentName { get; set; }
        public string CrtdUserName { get; set; }
        public Nullable<long> CrtdUserId { get; set; }
        public Nullable<System.DateTime> CrtdDT { get; set; }
        public string AprvdUserName { get; set; }
        public Nullable<long> AprvdUID { get; set; }
        public Nullable<System.DateTime> AprvdDT { get; set; }
        public string StateName { get; set; }
        public Nullable<long> StateId { get; set; }
        public Nullable<long> EmailTypeId { get; set; }
        public string EmailTypeName { get; set; }
        public string StateName2 { get; set; }
        public Nullable<long> StateId2 { get; set; }

        public int tmp { get; set; }

    }

}
