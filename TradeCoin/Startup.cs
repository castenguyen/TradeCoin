using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(Alluneecms.Startup))]
namespace Alluneecms
{
    public partial class Startup
    {
      

        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            //  Enable attribute based routing
            config.MapHttpAttributeRoutes();

            app.UseWebApi(config);
            ConfigureAuth(app);
        }
    }
}
