using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Alluneecms
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.RouteExistingFiles = true;



            routes.MapRoute(
            name: "Chat",
            url: "chat",
            defaults: new
            {
                controller = "Conversation",
                action = "Chat"
            });

            routes.MapRoute(
         name: "BChat",
         url: "BChat",
         defaults: new
         {
             controller = "Conversation",
             action = "ColectInforChat"
         });

            #region trang chu
            routes.MapRoute(
               name: "trang-chu",
               url: "",
               defaults: new
               {
                   controller = "Home",
                   action = "Index",
                   FriendlyUrl = UrlParameter.Optional
               });

            routes.MapRoute(
            name: "trang-chu-2",
            url: "Home",
            defaults: new
            {
                controller = "Home",
                action = "Index",
                FriendlyUrl = UrlParameter.Optional
            });

            routes.MapRoute(
       name: "chinh-sach-hoat-dong",
       url: "chinh-sach-hoat-dong",
       defaults: new
       {
           controller = "Home",
           action = "About",
           FriendlyUrl = UrlParameter.Optional
       });

            routes.MapRoute(
          name: "lien-he",
          url: "lien-he",
          defaults: new
          {
              controller = "Home",
              action = "Contact",
              FriendlyUrl = UrlParameter.Optional
          });

            #endregion trang chu

            //#region tin tuc

            //routes.MapRoute(
            //       name: "NewsList",
            //       url: "tin-tuc/{CataFriendlyUrl}-{id}",
            //       defaults: new
            //       {
            //           controller = "News",
            //           action = "Index",
            //           CataFriendlyUrl = UrlParameter.Optional,
            //           id = UrlParameter.Optional

            //       });

            //routes.MapRoute(
            //       name: "NewsDetail",
            //       url: "tin-tuc/{CataFriendlyUrl}/{FriendlyUrl}-{id}.html",
            //       defaults: new
            //       {
            //           controller = "News",
            //           action = "Detail",
            //           FriendlyUrl = UrlParameter.Optional,
            //           CataFriendlyUrl = UrlParameter.Optional,
            //           id = UrlParameter.Optional

            //       });

            //#endregion tin tuc

            #region san pham

            routes.MapRoute(
                 name: "AllProduct",
             url: "san-pham",
             defaults: new
             {
                 controller = "Product",
                 action = "AllProduct",
                 FriendlyUrl = UrlParameter.Optional
             });


            routes.MapRoute(
            name: "ProductList",
             url: "{FriendlyUrl}",
             defaults: new
             {
                 controller = "Product",
                 action = "ProductList",
                 FriendlyUrl = UrlParameter.Optional
             });

            routes.MapRoute(
                    name: "ProductDetail1",
                    url: "{string}/{FriendlyUrl}",
                    defaults: new
                    {
                        controller = "Product",
                        action = "ProductDetail",
                        FriendlyUrl = UrlParameter.Optional
                    });

            routes.MapRoute(
                  name: "ProductDetail2",
                  url: "{string1}/{string2}/{FriendlyUrl}",
                  defaults: new
                  {
                      controller = "Product",
                      action = "ProductDetail",
                      FriendlyUrl = UrlParameter.Optional,
                      string1 = UrlParameter.Optional,
                      string2 = UrlParameter.Optional
                  });


            #endregion san pham

            #region Account
            routes.MapRoute(
               name: "account-login",
               url: "tai-khoan/dang-nhap/{id}",
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
               namespaces: new string[] { "Alluneecms.Controllers" }
           );
            routes.MapRoute(
                name: "account-register",
                url: "tai-khoan/dang-ky-tai-khoan/{id}",
                defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
                namespaces: new string[] { "Alluneecms.Controllers" }
            );
            routes.MapRoute(
                name: "account-logout",
                url: "tai-khoan/dang-xuat",
                defaults: new { controller = "Account", action = "LogOff" },
                namespaces: new string[] { "Alluneecms.Controllers" }
            );
            routes.MapRoute(
                name: "account-update-avartar",
                url: "tai-khoan/cap-nhat-anh-dai-dien",
                defaults: new { controller = "Account", action = "UpdatePhotoUser" },
                namespaces: new string[] { "Alluneecms.Controllers" }
            );

            routes.MapRoute(
                name: "account-update-password",
                url: "tai-khoan/cap-nhat-mat-khau",
                defaults: new { controller = "Account", action = "UpdatePasswordUser" },
                namespaces: new string[] { "Alluneecms.Controllers" }
            );

            routes.MapRoute(
                name: "account-information",
                url: "tai-khoan/thong-tin-tai-khoan",
                defaults: new { controller = "Account", action = "Profile" },
                namespaces: new string[] { "Alluneecms.Controllers" }
            );
            #endregion

            #region thong báo

            routes.MapRoute(
           name: "ErrorMessage_ErrorExeption",
           url: "thong-bao-loi",
           defaults: new { controller = "ErrorMessage", action = "ErrorExeption" }
       );

            routes.MapRoute(
        name: "ErrorMessage_Index",
        url: "thong-bao",
        defaults: new
        {
            controller = "ErrorMessage",
            action = "Index"
        });

            #endregion thongbao

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
