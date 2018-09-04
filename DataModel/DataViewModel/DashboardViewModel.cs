using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataViewModel
{
    public class DashboardViewModel
    {
        public int NbrProduct { get; set; }
        public int NbrProductE { get; set; }
        public int NbrProductD { get; set; }
        public int NbrContentItem { get; set; }
        public int NbrContentItemE { get; set; }
        public int NbrContentItemD { get; set; }
        public int NbrComment { get; set; }
        public int NbrCommentE { get; set; }
        public int NbrCommentD { get; set; }


        public int NbrUser { get; set; }
        public int NbrUserE { get; set; }
        public int NbrUserD { get; set; }

        public int NbrTicker { get; set; }
        public int NbrTickerE { get; set; }
        public int NbrTickerD { get; set; }
        public int NbrTickerP { get; set; }

        public int NbrEmail { get; set; }
        public int NbrEmailE { get; set; }
        public int NbrEmailD { get; set; }

    }

    public class MainSliderAdminViewModel
    { 
        public bool ClassViewwAccess { get; set; }
        public bool CommentViewwAccess { get; set; }
        public bool ProductViewwAccess { get; set; }
        public bool NewsViewwAccess { get; set; }
        public bool MediaViewwAccess { get; set; }
        public bool DisplayViewwAccess { get; set; }
        public bool TagViewwAccess { get; set; }
        public bool AccountViewwAccess { get; set; }
        public bool ConfigViewwAccess { get; set; }
        public bool HistViewwAccess { get; set; }
        public bool OrderViewwAccess { get; set; }
    
    }
}
