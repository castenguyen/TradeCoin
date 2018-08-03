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
        public static string EmailAdmin = "botogiasi@gmail.com";
        public static string EmailAdminPassword = "Botogiasi2016";
        public static int Portmail = 587;

        #endregion
        #region Media

        public static string PrefixImageName = "phong-lan-gia-si";
        public static string PrefixImageContentURL = "phong-lan-gia-si";
        public static string DefaulMediaContentURL = "";
        public static string FolderImage = "images";
        public static string FolderFrontEnd = "Media";
        public static string FolderFTPImage = "ftp://103.3.250.22/BDSAdmin/images/media";
        public static string FTPUser = "NGUYENHUY";
        public static string FTPPass = "123456";
        public static string FTPFolder = "CMSADMIN";
        public static string FTPURL = "ftp://103.3.250.22";
        public static string WaterMask = "";

        #endregion


    }
}
