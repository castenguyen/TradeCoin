using DataModel.DataEntity;
using DataModel.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using DataModel.DataViewModel;


namespace Alluneecms
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           // Application["PageConfig"] = this.GetObjConfig();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
        }

        //private Config GetObjConfig()
        //{
        //    alluneedbEntities db = new alluneedbEntities();
        //    Config tmp = db.Configs.FirstOrDefault();
        //    return tmp;
        //}

    }
}
