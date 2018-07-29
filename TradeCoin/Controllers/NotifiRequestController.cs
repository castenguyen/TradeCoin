using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataEntity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using DataModel.Extension;

namespace Alluneecms.Controllers
{
    public class NotifiRequestController : CoreFronEndController
    {
        // GET: NotifiRequest
        public ActionResult Index()
        {
            return View();
        }

        #region "NotifiRequest"
        /*
         * TRI-27062016 Add function to get latest comment of a product (active when user post a new comment)
		 * id: product id
         * objTypeId: Object type id (This can retrive comment for many kind like: product, media, news, videos etc...)
         */
        [HttpPost]
        public async Task<ActionResult> SaveNotifiRequest(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return Json("{\"message\": \"Vui lòng nhập địa chỉ email hợp lệ.\", \"result\": \"0\"}");
            }
            else
            {
                long userId = 0;
                string userName = "";
                if (User.Identity.IsAuthenticated)
                {
                    userId = long.Parse(User.Identity.GetUserId());
                    userName = User.Identity.GetUserName();
                }

                var result = await cms_db.SaveNotifiRequest(userId, userName, email);

                if (result == (int)EnumCore.Result.action_true)
                {
                    return Json("{\"message\": \"Đăng ký nhận tin thành công!\", \"result\": \"1\"}");
                }
                else
                {
                    return Json("{\"message\": \"Email này đã được sử dụng để đăng ký nhận tin.\", \"result\": \"0\"}");
                }
            }
        }
        #endregion
    }
}