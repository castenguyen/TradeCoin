using DataModel.DataEntity;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace DataModel.DataViewModel
{
    public  class UserPackagesViewModel
    {
        public UserPackage _MainObj { get; set; }
        public UserPackagesViewModel()
        {
            _MainObj = new UserPackage();

        }
        public UserPackagesViewModel(UserPackage model)
        {
            _MainObj = model;

        }
        public long Id
        {
            get { return _MainObj.Id; }
            set { _MainObj.Id = value; }
        }
        public string UpgradeUserName
        {
            get { return _MainObj.UpgradeUserName; }
            set { _MainObj.UpgradeUserName = value; }
        }
        public Nullable<long> UpgradeUID
        {
            get { return _MainObj.UpgradeUID; }
            set { _MainObj.UpgradeUID = value; }
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
        public Nullable<long> PackageId
        {
            get { return _MainObj.PackageId; }
            set { _MainObj.PackageId = value; }
        }
        public string PackageName
        {
            get { return _MainObj.PackageName; }
            set { _MainObj.PackageName = value; }
        }
        public string UpgradeToken
        {
            get { return _MainObj.UpgradeToken; }
            set { _MainObj.UpgradeToken = value; }
        }
        public Nullable<System.DateTime> CrtdDT
        {
            get { return _MainObj.CrtdDT; }
            set { _MainObj.CrtdDT = value; }
        }
        public Nullable<double> Price
        {
            get { return _MainObj.Price; }
            set { _MainObj.Price = value; }
        }
        public Nullable<long> OldPackageID
        {
            get { return _MainObj.OldPackageID; }
            set { _MainObj.OldPackageID = value; }
        }
        public string OldPackageName
        {
            get { return _MainObj.OldPackageName; }
            set { _MainObj.OldPackageName = value; }
        }
        public Nullable<int> NumDay
        {
            get { return _MainObj.NumDay; }
            set { _MainObj.NumDay = value; }
        }
    }
}
