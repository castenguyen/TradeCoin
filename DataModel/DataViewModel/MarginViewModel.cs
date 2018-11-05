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
    public class MarginViewModel
    {

        public Margin _MainObj { get; set; }
        public MarginViewModel()
        {
            _MainObj = new Margin();

        }
        public MarginViewModel(Margin model)
        {
            _MainObj = model;

        }



        public long MarginId
        {
            get { return _MainObj.MarginId; }
            set { _MainObj.MarginId = value; }
        }
        public string MarginName
        {
            get { return _MainObj.MarginName; }
            set { _MainObj.MarginName = value; }
        }
        public Nullable<double> Long
        {
            get { return _MainObj.Long; }
            set { _MainObj.Long = value; }
        }
        public Nullable<double> LongStop
        {
            get { return _MainObj.LongStop; }
            set { _MainObj.LongStop = value; }
        }
        public Nullable<double> LongRate
        {
            get { return _MainObj.LongRate; }
            set { _MainObj.LongRate = value; }
        }
        public string LongNote
        {
            get { return _MainObj.LongNote; }
            set { _MainObj.LongNote = value; }
        }
        public Nullable<double> Short
        {
            get { return _MainObj.Short; }
            set { _MainObj.Short = value; }
        }
        public Nullable<double> ShortStop
        {
            get { return _MainObj.ShortStop; }
            set { _MainObj.ShortStop = value; }
        }
        public Nullable<double> ShortRate
        {
            get { return _MainObj.ShortRate; }
            set { _MainObj.ShortRate = value; }
        }
        public string ShortNote
        {
            get { return _MainObj.ShortNote; }
            set { _MainObj.ShortNote = value; }
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
        public Nullable<long> StateId
        {
            get { return _MainObj.StateId; }
            set { _MainObj.StateId = value; }
        }
        public string PackageName
        {
            get { return _MainObj.PackageName; }
            set { _MainObj.PackageName = value; }
        }
        public Nullable<long> PackageId
        {
            get { return _MainObj.PackageId; }
            set { _MainObj.PackageId = value; }
        }
        public Nullable<int> Flag
        {
            get { return _MainObj.Flag; }
            set { _MainObj.Flag = value; }
        }
        public string ExContent
        {
            get { return _MainObj.ExContent; }
            set { _MainObj.ExContent = value; }
        }

        public bool MarginStatus
        {
            get; set;
        }

        public List<SelectListObj> lstPackage { get; set; }
        public long[] lstMarginPackage { get; set; }
        public List<ContentPackage> lstMarginContentPackage { get; set; }
    }


    public class BoxMargin
    {

        public long packageId { get; set; }
        public MarginViewModel FreeObject { get; set; }
        public MarginViewModel DemObject { get; set; }
        public MarginViewModel VangObject { get; set; }
        public MarginViewModel KCObject { get; set; }

    }


    public class IndexMarginManager
    {


        public IPagedList<MarginViewModel> lstMainMargin { get; set; }
        public int page { get; set; }
        public int status { get; set; }

        public int pageNum { get; set; }

        public int MarginPackage { get; set; }

        public SelectList lstMarginStatus { get; set; }
        public SelectList lstMarginPackage { get; set; }
    }

}
