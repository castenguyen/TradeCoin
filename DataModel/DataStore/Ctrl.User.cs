using DataModel.DataEntity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Collections;
using DataModel.DataViewModel;
using DataModel.Extension;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Net;


namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public List<User> GetLstUserWhenUserBuyProduct(long idPro)
        {
            List<OrderProduct> lstOrderProduct = db.OrderProducts.Where(x=>x.ProductId == idPro).ToList();
            List<Order> lstOrder = new List<Order>();
            foreach (var item in lstOrderProduct)
            {
                 Order od = db.Orders.SingleOrDefault(x=>x.Id == item.OrderId);
                if (od != null)
                    lstOrder.Add(od);
            }
            List<User> lstUser = new List<User>();
            foreach (var item in lstOrder)
            {
                User user = db.Users.SingleOrDefault(x=>x.Id == item.CustomerId);
                if (user!=null)
                {
                    lstUser.Add(user);
                }
            }
            return lstUser;
        }

    }
}
