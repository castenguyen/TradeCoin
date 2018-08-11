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
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class MemberController : Controller
    {
        // GET: AdminCMS/Member

        /// <summary>
        /// VIEW FOR MEMBER
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult MemberDashBoard()
        {

            return View();
        }

        public ActionResult ListTicker()
        {

            return View();
        }

        public ActionResult DetailTicker()
        {
            return View();
        }
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


 
    }
}