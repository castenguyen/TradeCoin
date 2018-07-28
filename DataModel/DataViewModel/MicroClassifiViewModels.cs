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
    public class MicroClassifiViewModels
    {
        public MicroClassification _ModelObj { get; set; }
        public MicroClassifiViewModels()
        {
            _ModelObj = new MicroClassification();

        }
        public MicroClassifiViewModels(MicroClassification model)
        {
            _ModelObj = model;

        }

        public long MicroClassifiId
        {
            get { return _ModelObj.MicroClassifiId; }
            set { _ModelObj.MicroClassifiId = value; }
        }
        public Nullable<int> MicroParentClassifiId
        {
            get { return _ModelObj.MicroParentClassifiId; }
            set { _ModelObj.MicroParentClassifiId = value; }
        }
        public string MicroParentName
        {
            get { return _ModelObj.MicroParentName; }
            set { _ModelObj.MicroParentName = value; }
        }

        [Required]
        [Display(Name = "Tên Danh Mục")]
        public string MicroClassifiNM
        {
            get { return _ModelObj.MicroClassifiNM; }
            set { _ModelObj.MicroClassifiNM = value; }
        }


        [Display(Name = "Mô Tả Danh Mục")]
        public string MicroClassifiDesc
        {
            get { return _ModelObj.MicroClassifiDesc; }
            set { _ModelObj.MicroClassifiDesc = value; }
        }

        [Display(Name = "Từ Khoá SEO")]
        public string MicroClassifiMetakeyword
        {
            get { return _ModelObj.MicroClassifiMetakeyword; }
            set { _ModelObj.MicroClassifiMetakeyword = value; }
        }

        [Display(Name = "Mô Tả SEO")]
        public string MicroClassifiMetaDes
        {
            get { return _ModelObj.MicroClassifiMetaDes; }
            set { _ModelObj.MicroClassifiMetaDes = value; }
        }
        public int MicroClassifiSchemeId
        {
            get { return _ModelObj.MicroClassifiSchemeId; }
            set { _ModelObj.MicroClassifiSchemeId = value; }
        }
        public string MicroClassifiCD
        {
            get { return _ModelObj.MicroClassifiCD; }
            set { _ModelObj.MicroClassifiCD = value; }
        }

        [Display(Name = "Đường Dẫn Thân Thiện")]
        public string FriendlyURL
        {
            get { return _ModelObj.FriendlyURL; }
            set { _ModelObj.FriendlyURL = value; }
        }
        public byte IsEnabled
        {
            get { return _ModelObj.IsEnabled; }
            set { _ModelObj.IsEnabled = value; }
        }
        public Nullable<System.DateTime> CrtdDT
        {
            get { return _ModelObj.CrtdDT; }
            set { _ModelObj.CrtdDT = value; }
        }
        public Nullable<long> CrtdUID
        {
            get { return _ModelObj.CrtdUID; }
            set { _ModelObj.CrtdUID = value; }
        }
        public string CrtdUserName
        {
            get { return _ModelObj.CrtdUserName; }
            set { _ModelObj.CrtdUserName = value; }
        }
        public string ShortNM
        {
            get { return _ModelObj.ShortNM; }
            set { _ModelObj.ShortNM = value; }
        }
        public Nullable<int> DisplayOrder
        {
            get { return _ModelObj.DisplayOrder; }
            set { _ModelObj.DisplayOrder = value; }
        }
        public Nullable<long> MicroID
        {
            get { return _ModelObj.MicroID; }
            set { _ModelObj.MicroID = value; }
        }
        public string MicroName
        {
            get { return _ModelObj.MicroName; }
            set { _ModelObj.MicroName = value; }
        }
        public string MicroFriendlyURL { get; set; }
    }

    public class MicroClassifiIndexView {
        public string MicroFriendlyUrl { get; set; }
        public long MicroId { get; set; }
        public IPagedList<MicroClassification> ListMainoBJ { get; set; }
    
    }

}
