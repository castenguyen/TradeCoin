using CMSPROJECT.Areas.AdminCMS.Core;

using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
 
    public class DemoController : CoreBackEnd
    {
        /// FOR MEMBER
        /// <summary>
        /// index for member
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateEmail()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult UserManager()
        {
            return View();
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailUser()
        {
            return View();
        }


        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateInfoAcc()
        {
            return View();
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult Upgrade()
        {
            return View();
        }

        /// FOR MOD/SUPPORT
        /// <summary>
        /// index for mod/supporter
        /// </summary>
        /// <returns></returns>
        public ActionResult ModIndex()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexNews()
        {
            return View();
        } 
        /// <summary>
           /// ok
           /// </summary>
           /// <returns></returns>
        public ActionResult CreateNews()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateNews()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult ListNews()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailNews()
        {
            return View();
        }


        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexTicker()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTicker()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateTicker()
        {
            return View();
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult ListTicker()
        {
            return View();
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailTicker()
        {
            return View();
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult MailIndex()
        {
            return View();
        }
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult ReplyMail()
        {
            return View();
        }


        //// FOR ADMIN
        public ActionResult IndexAdmin()
        {
            return View();
        }
     
        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult CreatePackage()
        {
            return View();
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePackage()
        {
            return View();
        }

        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexPackage()
        {
            return View();
        }


        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCatalogry()
        {
            return View();
        }


        /// <summary>
        /// ok
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateCatalogry()
        {
            return View();
        }





        public ActionResult DemoMainHeader()
        {
            return PartialView("_DemoMainHeaderPartial");
        }
        public ActionResult DemoMainSlider()
        {
            return PartialView("_DemoMainSliderPartial");
        }



        
    }
}