using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Extension
{

    public class ConstantSystem
    {
        #region Email service

        public static string PageName = "HQDN";

        public static string EmailAdminSMTP = "smtp.gmail.com";
        public static string EmailAdmin = "ncoinclub@gmail.com";
        public static string EmailAdminPassword = "ncoinclubdnq";
        public static int Portmail = 587;


        //public static string EmailAdminSMTP = "smtp.gmail.com";
        //public static string EmailAdmin = "ncoinclubdem@gmail.com";
        //public static string EmailAdminPassword = "ncoinclubdnq123";
        //public static int Portmail = 587;

        //public static string EmailAdminSMTP = "smtp.gmail.com";
        //// public static string EmailAdmin = "support@noinclub.com";
        //public static string EmailAdmin = "contact@ncoinclub.com";
        //public static string EmailAdminPassword = "Ncoinclub2018";
        //public static int Portmail = 587;


        public static string EmailAdminSMTP2 = "smtp.gmail.com";
        public static string EmailAdmin2 = "ncoinclubkimcuong@gmail.com";
        public static string EmailAdminPassword2 = "ncoinclubdnq123";
        public static int Portmail2 = 587;


        


        //public static string EmailAdminSMTP = "mail.mailviet.vn";
        //public static string EmailAdmin = "hotro@ncoinclub.com";
        //public static string EmailAdminPassword = "Ncoinclub2018";
        //public static int Portmail = 465;

        #endregion




        #region Media
        public static int ResizeWidth =500;
        public static int ResizeHeight = 400;
        public static string PrefixImageName = "ncoinclub";
        public static string PrefixImageContentURL = "ncoinclub";
        public static string DefaulMediaContentURL = "";
        public static string FolderImage = "images";
        public static string FolderVideo= "Video";
        public static string FolderFrontEnd = "Media";
        public static string FolderFTPImage = "ftp://103.3.250.22/BDSAdmin/images/media";
        public static string FTPUser = "NGUYENHUY";
        public static string FTPPass = "123456";
        public static string FTPFolder = "CMSADMIN";
        public static string FTPURL = "ftp://103.3.250.22";
        public static string WaterMask = "";

        #endregion


        #region Table name in db

        public static string Table_ticker = "Ticker";
        public static string Table_ContentItem = "ContentItem";

        #endregion


    }
}
