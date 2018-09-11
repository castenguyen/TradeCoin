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
    public class TickerViewModel
    {
        public Ticker _MainObj { get; set; }
        public TickerViewModel()
        {
            _MainObj = new Ticker();

        }
        public TickerViewModel(Ticker model)
        {
            _MainObj = model;

        }

        public long TickerId
        {
            get { return _MainObj.TickerId; }
            set { _MainObj.TickerId = value; }
        }
        [Required]
        [Display(Name = "Tên Coin")]
        public string TickerName
        {
            get { return _MainObj.TickerName; }
            set { _MainObj.TickerName = value; }
        }

        [Required]
        [Display(Name = "Vùng mua")]
        public Nullable<double> BuyZone1
        {
            get { return _MainObj.BuyZone1; }
            set { _MainObj.BuyZone1 = value; }
        }

        [Required]
        [Display(Name = "Vùng bán 1")]
        public Nullable<double> SellZone1
        {
            get { return _MainObj.SellZone1; }
            set { _MainObj.SellZone1 = value; }
        }

        [Required]
        [Display(Name = "Vùng bán 2")]
        public Nullable<double> SellZone2
        {
            get { return _MainObj.SellZone2; }
            set { _MainObj.SellZone2 = value; }
        }


        [Required]
        [Display(Name = "Vùng bán 3")]
        public Nullable<double> SellZone3
        {
            get { return _MainObj.SellZone3; }
            set { _MainObj.SellZone3 = value; }
        }

      
        [Display(Name = "Số BTC tối đa vào lệnh")]
        public Nullable<double> BTCInput
        {
            get { return _MainObj.BTCInput; }
            set { _MainObj.BTCInput = value; }
        }

        [Required]
        [Display(Name = "Cắt lỗ")]
        public Nullable<double> DeficitControl
        {
            get { return _MainObj.DeficitControl; }
            set { _MainObj.DeficitControl = value; }
        }

        [Required]
        [AllowHtml]
        [Display(Name = "Đánh giá")]
        public string Description
        {
            get { return _MainObj.Description; }
            set { _MainObj.Description = value; }
        }


   
        [Display(Name = "Mô tả")]
        public string Excerpt
        {
            get { return _MainObj.Excerpt; }
            set { _MainObj.Excerpt = value; }
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

        public string MediaUrl
        {
            get { return _MainObj.MediaUrl; }
            set { _MainObj.MediaUrl = value; }
        }
        public string MediaThumb
        {
            get { return _MainObj.MediaThumb; }
            set { _MainObj.MediaThumb = value; }
        }

        public int? Flag
        {
            get { return _MainObj.Flag; }
            set { _MainObj.Flag = value; }
        }


        public Nullable<double> Profit
        {
            get { return _MainObj.Profit; }
            set { _MainObj.Profit = value; }
        }
        public Nullable<double> Deficit
        {
            get { return _MainObj.Deficit; }
            set { _MainObj.Deficit = value; }
        }
        public Nullable<int> tmp
        {
            get { return _MainObj.tmp; }
            set { _MainObj.tmp = value; }
        }
        [Display(Name = "Số USD tối đa vào lệnh")]
        public Nullable<double> USDInput
        {
            get { return _MainObj.USDInput; }
            set { _MainObj.USDInput = value; }
        }

        public List<SelectListObj> lstPackage { get; set; }
        [Display(Name = "Hình đại diện")]
        public long ImgdefaultId { get; set; }
        public long[] lstTickerPackage { get; set; }

        /// <summary>
        /// danh cho frontend
        /// </summary>
        /// 
        [Display(Name = "Loại sàn")]
        public SelectList lstMarketItem { get; set; }
      
        public Nullable<int> MarketID
        {
            get { return _MainObj.MarketID; }
            set { _MainObj.MarketID = value; }
        }

        [Display(Name = "Loại Coin")]
        public SelectList lstCyptoItem { get; set; }

        public Nullable<long> CyptoID
        {
            get { return _MainObj.CyptoID; }
            set { _MainObj.CyptoID = value; }
        }

     
        public List<TickerViewModel> lstsameTickers { get; set; }
        public List<ContentPackage> lstTickerContentPackage { get; set; }
    }



    public class TickerAdminViewModel
    {
        public IPagedList<Ticker> lstMainTicker { get; set; }

        public IPagedList<TickerViewModel> lstMainTickerViewModel { get; set; }
        public SelectList lstTickerStatus { get; set; }
        public SelectList lstPackage { get; set; }

        public int pageNum { get; set; }
        public int TickerStatus { get; set; }
        public int TickerPackage { get; set; }
        public string FillterTickerName { get; set; }

        public List<ContentPackage> lstContentPackage  { get; set; }

        [Display(Name = "Loại sàn")]
        public SelectList lstMarketItem { get; set; }
        public int MarketItemID { get; set; }


        [Display(Name = "Loại Coin")]
        public SelectList lstCyptoItem { get; set; }
        public long CyptoItemID { get; set; }

    }



    public class MiniTickerViewModel
    {
        public long TickerId { get; set; }
        public string TickerName { get; set; }
        public Nullable<double> BuyZone1 { get; set; }
        public Nullable<double> SellZone1 { get; set; }
        public Nullable<double> SellZone2 { get; set; }
        public Nullable<double> SellZone3 { get; set; }
        public Nullable<double> BTCInput { get; set; }
        public Nullable<double> DeficitControl { get; set; }
        public string Description { get; set; }

        public string CrtdUserName { get; set; }
        public string Excerpt { get; set; }
        public Nullable<long> CrtdUserId { get; set; }
        public Nullable<System.DateTime> CrtdDT { get; set; }
        public string AprvdUserName { get; set; }
        public Nullable<long> AprvdUID { get; set; }
        public Nullable<System.DateTime> AprvdDT { get; set; }
        public string StateName { get; set; }
        public Nullable<long> StateId { get; set; }
        public string MediaUrl { get; set; }
        public string MediaThumb { get; set; }
        public Nullable<int> Flag { get; set; }
        public Nullable<double> Profit { get; set; }
        public Nullable<double> Deficit { get; set; }
        public Nullable<int> tmp { get; set; }
        public List<ContentPackage> lstTickerContentPackage { get; set; }

    }

}
