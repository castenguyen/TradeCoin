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
    public class UserHistViewModel
    {



    }

    public class IndexHistoryAction
    {
        public IPagedList<Userhist> lstMainUserHist { get; set; }
        public int page { get; set; }
        public int ObjectTypeid { get; set; }
        public int ActionTypeid { get; set; }
        public string Datetime { get; set; }
        
        public Nullable<System.DateTime> StartDT { get; set; }
        public Nullable<System.DateTime> EndDT { get; set; }


        public SelectList lstObjectType { get; set; }
        public SelectList lstActionType { get; set; }

    }



}
