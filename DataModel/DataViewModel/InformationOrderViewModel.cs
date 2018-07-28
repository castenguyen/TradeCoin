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
     public class InformationOrderViewModel
    {
        public Order Order { get; set; }
        public List<ItemInforProductViewModel> OrderProduct { get; set; }
    }

    public class ItemInforProductViewModel
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public string FriendlyUrl { get; set; }
        public int? Quantity { get; set; }
        public double? Unitprice { get; set; }
        public double? Discount { get; set; }
        public string ProductName { get; set; }
        public string ThumbUrl { get; set; }
        public long MicrositeId { get; set; }
        public string MicrositeName { get; set; }
        public long CateMicrositeID { get; set; }
        public string CateMicrositeName { get; set; }
    }

    public class OrderIndexViewModel
    {
        public IPagedList<Order> lstMainOrder { get; set; }
        public List<SelectListObj> lstState { get; set; }
        public int pageNum { get; set; }
        public int ContentState { get; set; }

    }
    public class OrderDetailViewModel
    {
        public Order MainObjOrder { get; set; }
        public List<OrderProductExt> lstProduct { get; set; }
        public SelectList lstStatus { get; set; }
        public int Status { get; set; }
        
    }
    public class OrderProductExt
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string MediaUrl { get; set; }
        public string MediaThumb { get; set; }
        public Nullable<double> OldPrice { get; set; }
        public Nullable<double> NewPrice { get; set; }
        public long OrderId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> Unitprice { get; set; }
        public Nullable<double> Discount { get; set; }

    }


}
