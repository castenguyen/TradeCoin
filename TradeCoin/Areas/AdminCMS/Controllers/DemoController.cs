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

        public ActionResult CreatePackage()
        {
            return View();
        }

        public ActionResult UpdatePackage()
        {
            return View();
        }
        public ActionResult IndexPackage()
        {
            return View();
        }

        public ActionResult IndexCatalogry()
        {
            return View();
        }

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