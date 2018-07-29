using DataModel.DataStore;
using DataModel.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataModel.Extension;
using System.Security.Principal;


namespace CMSPROJECT.Areas.AdminCMS.Core
{
    public class CoreBackEnd : AsyncController
    {

        protected readonly string StateName_Enable="Enable";
        protected readonly string StateName_Disable="Disable";
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
                        RouteValueDictionary(new { controller = "Extension", action = "ErrorExeption" }));
                    }
                }
            }

        }

        public string GetFriendlyUrlFromstring(string title, int objtype)
        {

            string result = title.ToFriendlyURL();
            string _tmp = result;
            for (int i = 1; this.CheckDuplicateFriendlyUrl(result, objtype) != true; i++)
            {
                result = _tmp;
                result = result + "-" + i;
            }
            return result;
        }
        /// <summary>
        /// Kiểm tra trùng lặp FriendUrl
        /// </summary>
        /// <param name="FriendlyURL"></param>
        /// <param name="objtype">Loại đối tượng</param>
        /// <returns></returns>
        public bool CheckDuplicateFriendlyUrl(string FriendlyURL, int objtype)
        {
            if (objtype == (int)EnumCore.ObjTypeId.tin_tuc)
            {
                ContentItem objnew = cms_db.GetObjContentItemByFriendlyURL(FriendlyURL);
                if (objnew == null)
                    return true;
                else
                {
                    return false;
                }
            }
            if (objtype == (int)EnumCore.ObjTypeId.san_pham)
            {
                Product objpro = cms_db.GetObjProductByFriendlyURL(FriendlyURL);
                if (objpro == null)
                    return true;
                else {
                    return false;
                }
            }
            Classification rs = cms_db.GetObjClasscifiByFriendlyUrl(FriendlyURL);
            if (rs == null)
                return true;
            return false;
        }
        public int GetMaxDisplayOrderClassifi(int SchemeId, int? ParentId)
        {
            int max = 1;
            if (ParentId == null)
            {
                if (cms_db.GetlstClassifiBySchemeId(SchemeId) == null)
                {
                    return 1;
                }
                else
                {
                    max = cms_db.GetlstClassifiBySchemeId(SchemeId).Count();
                    return max;
                }

            }
            else
            {
                max = cms_db.GetlstClassifiByParentId(ParentId.Value).Count();
                if (max == null)
                    return 1;
                return max;
            }
        }

    }
}