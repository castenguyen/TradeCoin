using CMSPROJECT.Areas.AdminCMS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using CMSPROJECT.Hubs;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class ChatGroupController : CoreBackEnd
    {
        // GET: AdminCMS/ChatGroup


      
        public ActionResult Index()
        {
        
            return View();
        }
    }

    public class NotificationController : CoreBackEnd
    {
        public ActionResult NewNotifi()
        {

            return View();
        }

        public ActionResult SendNewNoifi()
        {

            var context = GlobalHost.ConnectionManager.GetHubContext<NotifiHub>();
            context.Clients.All.methodInJavascript("hello world");

          
            return View();
        }

    }




}