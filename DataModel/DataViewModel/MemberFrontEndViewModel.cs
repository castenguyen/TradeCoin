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
    public class MemberFrontEndViewModel
    {
        public List<TickerViewModel> lstTicker { get; set; }
        public List<ContentItemViewModels> lstNews { get; set; }
        public User ObjectUser { get; set; }

    }

    public class TickerMemberViewModel
    {

        public IPagedList<MiniTickerViewModel> lstMainTicker { get; set; }

   
        public SelectList lstTickerStatus { get; set; }
        public SelectList lstPackage { get; set; }

        public int pageNum { get; set; }
        public int TickerStatus { get; set; }
        public int TickerPackage { get; set; }
        public string FillterTickerName { get; set; }
        public string Datetime { get; set; }
        public List<ContentPackage> lstContentPackage { get; set; }
        public Nullable<System.DateTime> StartDT { get; set; }
        public Nullable<System.DateTime> EndDT { get; set; }

        public long TotalTicker { get; set; }
        public double TotalProfit { get; set; }
        public double TotalDeficit { get; set; }
        public double TotalNumberBTC{ get; set; }

        public double Total { get; set; }


    }


    public class MediaMemberViewModel
    {
        public IPagedList<MiniMediaViewModel> lstMainTicker { get; set; }
      
        public SelectList lstPackage { get; set; }
        public int pageNum { get; set; }
        public int MediaPackage { get; set; }


        public List<ContentPackage> lstContentPackage { get; set; }

    }


    public class MiniMediaViewModel
    {
        public long MediaContentId { get; set; }
        public System.Guid MediaContentGuidId { get; set; }
        public string Filename { get; set; }
        public string FullURL { get; set; }
        public string ThumbURL { get; set; }
        public string MetadataDesc { get; set; }
        public string MetadataKeyword { get; set; }
        public Nullable<long> MediaContentSize { get; set; }
        public string EXIFInfo { get; set; }
        public Nullable<int> MediaTypeId { get; set; }
        public Nullable<int> ObjTypeId { get; set; }
        public Nullable<long> AprvdUID { get; set; }
        public Nullable<System.DateTime> AprvdDT { get; set; }
        public Nullable<long> CrtdUID { get; set; }
        public Nullable<System.DateTime> CrtdDT { get; set; }
        public long ViewCount { get; set; }
        public string Caption { get; set; }
        public string AlternativeText { get; set; }
        public string MediaDesc { get; set; }
        public Nullable<long> ContentObjId { get; set; }
        public string ContentObjName { get; set; }
        public string LinkHref { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string StatusName { get; set; }
        public int tmp { get; set; }


        public List<ContentPackage> lstVideoContentPackage { get; set; }

    }



 }
