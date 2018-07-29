using DataModel.DataEntity;
using DataModel.DataStore;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Alluneecms.Controllers
{
    public class CoreFronEndController : AsyncController
    {
        protected Ctrl cms_db = new Ctrl();
       
        public class AdminAuthorize : AuthorizeAttribute
        {
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    base.HandleUnauthorizedRequest(filterContext);
                    filterContext.Result = new RedirectToRouteResult(new
                   RouteValueDictionary(new { controller = "AccountAdmin", action = "Login" }));
                }
            }
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                var uri = filterContext.HttpContext.Request.Url.AbsoluteUri;
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    base.HandleUnauthorizedRequest(filterContext);
                    filterContext.Result = new RedirectToRouteResult(new
                   RouteValueDictionary(new { controller = "AccountAdmin", action = "Login", returnUrl = uri }));
                }
                else
                {

                    if (!base.AuthorizeCore(filterContext.HttpContext))
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "ErrorMessage", action = "ErrorExeption" }));
                    }
                }
            }

        }
        public int SetInforMeta(string metakey, string metades)
        {
            Config model = new Config();
            model.site_metadadescription = metades;
            model.site_metadatakeyword = metakey;
            HttpContext.Application["PageConfig"] = model;
            return (int)EnumCore.Result.action_true;
        }
    
    }
}