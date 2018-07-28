using DataModel.DataEntity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataModel.DataViewModel
{
    public class PromotionContentViewModel
    {
        public PromotionContent _MainObj { get; set; }
        public PromotionContentViewModel() {
            _MainObj= new PromotionContent();
        }
        public PromotionContentViewModel(PromotionContent model)
        {
            _MainObj = model;
        }

        public int Id
        {
            get { return _MainObj.Id; }
            set { _MainObj.Id = value; }
        }
        public Nullable<long> MainContentId
        {
            get { return _MainObj.MainContentId; }
            set { _MainObj.MainContentId = value; }
        }
        public string MainContentName
        {
            get { return _MainObj.MainContentName; }
            set { _MainObj.MainContentName = value; }
        }
        public Nullable<int> MainCateId
        {
            get { return _MainObj.MainCateId; }
            set { _MainObj.MainCateId = value; }
        }
        public string MainCatetName
        {
            get { return _MainObj.MainCatetName; }
            set { _MainObj.MainCatetName = value; }
        }
        public Nullable<int> ObjType
        {
            get { return _MainObj.ObjType; }
            set { _MainObj.ObjType = value; }
        }
        public string ObjTypeName
        {
            get { return _MainObj.ObjTypeName; }
            set { _MainObj.ObjTypeName = value; }
        }
        public Nullable<int> PromotionType
        {
            get { return _MainObj.PromotionType; }
            set { _MainObj.PromotionType = value; }
        }
        public string PromotionTypeName
        {
            get { return _MainObj.PromotionTypeName; }
            set { _MainObj.PromotionTypeName = value; }
        }
        public Nullable<System.DateTime> CrtdDT
        {
            get { return _MainObj.CrtdDT; }
            set { _MainObj.CrtdDT = value; }
        }
        public Nullable<long> CrtdUID
        {
            get { return _MainObj.CrtdUID; }
            set { _MainObj.CrtdUID = value; }
        }
        public string CrtdName
        {
            get { return _MainObj.CrtdName; }
            set { _MainObj.CrtdName = value; }
        }
        public Nullable<System.DateTime> StartDT
        {
            get { return _MainObj.StartDT; }
            set { _MainObj.StartDT = value; }
        }
        public Nullable<System.DateTime> EndDT
        {
            get { return _MainObj.EndDT; }
            set { _MainObj.EndDT = value; }
        }
        public Nullable<int> DisplayOrder
        {
            get { return _MainObj.DisplayOrder; }
            set { _MainObj.DisplayOrder = value; }
        }
        public Nullable<long> SubContentId
        {
            get { return _MainObj.SubContentId; }
            set { _MainObj.SubContentId = value; }
        }
        public string SubContentName
        {
            get { return _MainObj.SubContentName; }
            set { _MainObj.SubContentName = value; }
        }
        public Nullable<int> PromotionPercent
        {
            get { return _MainObj.PromotionPercent; }
            set { _MainObj.PromotionPercent = value; }
        }
        public Nullable<int> PromotionAmount
        {
            get { return _MainObj.PromotionAmount; }
            set { _MainObj.PromotionAmount = value; }
        }

        public string DateTimeTxt { get; set; }
        public SelectList LstPromotionType { get; set; }
        public List<PromotionContent> LstPromotionContent { get; set; }
        public Product Mainpro { get; set; }

        public SelectList lstProductCatalogry { get; set; }
        public int ProductCatalogry { get; set; }

        public long[] PromotionProductId { get; set; }


    }

    public class PromotionIndexAdminViewModel
    {
        public IPagedList<PromotionContent> MainLstContent { get; set; }
        public List<SelectListObj> lstCatelo { get; set; }
        public List<SelectListObj> lstPromotionType { get; set; }

        public int pageNum { get; set; }
        public int CatelogryId { get; set; }
        public int PromotionTypeId { get; set; }
    
    
    }
}
