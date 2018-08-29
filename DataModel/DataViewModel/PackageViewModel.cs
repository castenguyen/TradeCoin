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
    public class PackageViewModel
    {
        public Package _MainObj { get; set; }
        public PackageViewModel()
        {
            _MainObj = new Package();

        }
        public PackageViewModel(Package model)
        {
            _MainObj = model;

        }


        public long PackageId
        {
            get { return _MainObj.PackageId; }
            set { _MainObj.PackageId = value; }
        }
        [Required]
        [Display(Name = "Tên gói cước")]
        public string PackageName
        {
            get { return _MainObj.PackageName; }
            set { _MainObj.PackageName = value; }
        }

        public Nullable<double> OldPrice
        {
            get { return _MainObj.OldPrice; }
            set { _MainObj.OldPrice = value; }
        }
        [Required]
        [Display(Name = "Gía gói theo tháng")]
        public Nullable<double> NewPrice
        {
            get { return _MainObj.NewPrice; }
            set { _MainObj.NewPrice = value; }
        }

        [Required]
        [Display(Name = "Gía gói theo quý")]
        public Nullable<double> NewPrice3M
        {
            get { return _MainObj.NewPrice3M; }
            set { _MainObj.NewPrice3M = value; }
        }

        [Required]
        [Display(Name = "Gía gói theo năm")]
        public Nullable<double> NewPrice1Y
        {
            get { return _MainObj.NewPrice1Y; }
            set { _MainObj.NewPrice1Y = value; }
        }

        [Required]
        [Display(Name = "Gía gói vĩnh viễn")]
        public Nullable<double> ForeverPrice
        {
            get { return _MainObj.ForeverPrice; }
            set { _MainObj.ForeverPrice = value; }
        }

        [Required]
        [Display(Name = "Thời gian gói cước")]
        public Nullable<int> NumDay
        {
            get { return _MainObj.NumDay; }
            set { _MainObj.NumDay = value; }
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
        public string StateName
        {
            get { return _MainObj.StateName; }
            set { _MainObj.StateName = value; }
        }
        public Nullable<int> StateId
        {
            get { return _MainObj.StateId; }
            set { _MainObj.StateId = value; }
        }

    }

    public class UserPackageViewModel {

        public Package ObjPackage { get; set; }
        public User ObjUser { get; set; }
        public string UpgradeToken { get; set; }
        public Nullable<long> UpgradeUID { get; set; }
        public string AprvdUserName { get; set; }
        public Nullable<long> AprvdUID { get; set; }
        public Nullable<System.DateTime> AprvdDT { get; set; }
        public string StateName { get; set; }
        public Nullable<long> StateId { get; set; }
        public Nullable<long> PackageId { get; set; }
        public string PackageName { get; set; }
        public string UpgradeUserName { get; set; }
        public double TotalPrice { get; set; }
        public int? TotalDay { get; set; }
        public Nullable<System.DateTime> CrtdDT { get; set; }

    }


    public class TrackingFinanceViewModel
        {
        public IPagedList<UserPackage> lstMainUserPackage { get; set; }
        public int pageNum { get; set; }
        public string FillterName { get; set; }
        public string Datetime { get; set; }
        public Nullable<System.DateTime> StartDT { get; set; }
        public Nullable<System.DateTime> EndDT { get; set; }

        public SelectList lstPackage { get; set; }
        public int Packageid { get; set; }

    }

}
