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


        //tesk ok
        //ssl disable
        public static string ServiceSMTP = "pro38.emailserver.vn";
        public static string ServiceAdmin = "noreply@service-ncoinclub.com";
        public static string ServicePassword = "nguyentran2617";
        public static int ServicePort = 587;

        //tesk ok
        //ssl disable
        public static string NotifySMTP = "pro38.emailserver.vn";
        public static string NotifyAdmin = "noreply@notify-ncoinclub.com";
        public static string NotifyPassword = "nguyentran2617";
        public static int NotifyPort = 587;



        /// <summary>
        /// REGISTER 28-09-2018
        /// </summary>
        //ssl disable
        //tesk ok

   
        public static string NotifyTopSMTP = "smtp.gmail.com";
        public static string NotifyTopAdmin = "ncoinclubthongbao2@gmail.com";
        public static string NotifyTopPassword = "Ncoinclub2018";
        public static int NotifyTopPort = 587;

        //ssl disable
        //tesk ok
        public static string ServiceTopSMTP = "smtp.gmail.com";
        public static string ServiceTopAdmin = "ncoinclubthongbao1@gmail.com";
        public static string ServiceTopPassword = "Ncoinclub2018";
        public static int ServiceTopPort = 587;

        //ssl disable
        //tesk ok
        public static string NcoinTopSMTP = "smtp.gmail.com";
        public static string NcoinTopAdmin = "ncoinclubthongbao3@gmail.com";
        public static string NcoinTopPassword = "Ncoinclub2018";
        public static int NcoinTopPort = 587;







        //ssl disable
        //tesk ok
        public static string NotifyXYZSMTP = "pro38.emailserver.vn";
        public static string NotifyXYZAdmin = "noreply@notify-ncoinclub.xyz";
        public static string NotifyXYZPassword = "nguyentran2617";
        public static int NotifyXYZPort = 587;

        //ssl disable
        //tesk ok
        public static string ServiceXYZSMTP = "pro38.emailserver.vn";
        public static string ServiceXYZAdmin = "noreply@service-ncoinclub.xyz";
        public static string ServiceXYZPassword = "nguyentran2617";
        public static int ServiceXYZPort = 587;


   



        //ssl disable
        //tesk ok
        public static string NcoinXYZSMTP = "pro38.emailserver.vn";
        public static string NcoinXYZAdmin = "noreply@ncoinclub.xyz";
        public static string NcoinXYZPassword = "nguyentran2617";
        public static int NcoinXYZPort = 587;








        //tesk ok
        //ssl enable
        public static string Email_SMTP_1 = "smtp.gmail.com";
        public static string Email_User_1 = "ncoinclubbaokeo@gmail.com";
        public static string Email_Password_1 = "Ncoinclub2018";
        public static int Email_Port_1 = 587;




        //tesk ok
        //ssl enable
        public static string EmailAdminSMTP = "smtp.gmail.com";
        public static string EmailAdmin = "ncoinclub@gmail.com";
        public static string EmailAdminPassword = "ncoinclubdnq";
        public static int Portmail = 587;



        //tesk ok
        //ssl enable
        public static string Email_SMTP_2 = "smtp.gmail.com";
        public static string Email_User_2 = "ncoinclubdem@gmail.com";
        public static string Email_Password_2 = "ncoinclubdnq123";
        public static int Email_Port_2 = 587;




        //tesk ok
        //ssl enable
        public static string Email_SMTP_3 = "smtp.gmail.com";
        public static string Email_User_3 = "ncoinclubkimcuong@gmail.com";
        public static string Email_Password_3 = "ncoinclubdnq123";
        public static int Email_Port_3 = 587;


        //tesk ok
        //ssl enable
        public static string Email_SMTP_4 = "smtp.gmail.com";
        public static string Email_User_4 = "ncoinclubthongbao@gmail.com";
        public static string Email_Password_4 = "Ncoinclub2018";
        public static int Email_Port_4 = 587;




        //tesk ok
        //ssl enable
        //public static string ContactSMTP = "smtp.gmail.com";
        //public static string ContactAdmin = "contact@ncoinclub.com";
        //public static string ContactPassword = "Ncoinclub2018";
        //public static int ContactPort = 587;




        //was: 5.5.1 Authentication Required. Learn more at
        //test not ok
        //public static string NoreplySMTP = "smtp.gmail.com";
        //public static string NoreplyEmail = "noreply@ncoinclub.com";
        //public static string NoreplyPassword = "Ncoinclub2019";
        //public static int NoreplyPort = 587;


        //was: 5.5.1 Authentication Required. Learn more at
        //test not ok
        //public static string SupportSMTP = "smtp.gmail.com";
        //public static string SupportEmail = "support@noinclub.com";
        //public static string SupportPassword = "Ncoinclub2018";
        //public static int SupportPort = 587;




        //was: 5.5.1 Authentication Required. Learn more at
        //test not ok
        //public static string EmailAdminSMTP = "smtp.gmail.com";
        //public static string EmailAdmin = "ncoinclubdangnhap@gmail.com";
        //public static string EmailAdminPassword = "Ncoinclub2018";
        //public static int Portmail = 587;




        //was: 5.5.1 Authentication Required. Learn more at
        //test not ok
        //public static string ContactSMTP = "smtp.gmail.com";
        //public static string ContactAdmin = "ncoinclubthongbao2@gmail.com";
        //public static string ContactPassword = "Ncoinclub2018";
        //public static int ContactPort = 587;



        //was: 5.5.1 Authentication Required. Learn more at
        //test not ok
        //public static string SupportSMTP = "smtp.gmail.com";
        //public static string SupportEmail = "ncoinclubthongbao1@gmail.com";
        //public static string SupportPassword = "Ncoinclub2018";
        //public static int SupportPort = 587;




        //was: 5.5.1 Authentication Required. Learn more at
        //test not ok
        //public static string NoreplySMTP = "smtp.gmail.com";
        //public static string NoreplyEmail = "ncoinclubthongbao3@gmail.com";
        //public static string NoreplyPassword = "Ncoinclub2018";
        //public static int NoreplyPort = 587;






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
