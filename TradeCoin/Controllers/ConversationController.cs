using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;



namespace Alluneecms.Controllers
{
    public class ConversationController : CoreFronEndController
    {
        // GET: Conversation
        public ActionResult Chat(UserInfo user)
        {
            return View(user);
        }
        public ActionResult ColectInforChat()
        {
            return View();
        }
    }
}