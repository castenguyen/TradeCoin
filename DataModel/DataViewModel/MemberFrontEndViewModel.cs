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

        public List<ContentPackage> lstContentPackage { get; set; }

    }


    public class MediaMemberViewModel
    {
        public IPagedList<MediaContentViewModels> lstMainTicker { get; set; }
      
        public SelectList lstPackage { get; set; }

        public int pageNum { get; set; }
        public int MediaStatus { get; set; }
        public int MediaPackage { get; set; }
        public string FillterMediaName { get; set; }

        public List<ContentPackage> lstContentPackage { get; set; }

    }




}
