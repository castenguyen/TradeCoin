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

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public IEnumerable<OrderProduct> AddOrderProduct(IEnumerable<OrderProduct> orderProducts)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    orderProducts = db.OrderProducts.AddRange(orderProducts);
                    db.SaveChanges();
                    transaction.Commit();
                    return orderProducts;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public List<ItemInforProductViewModel> GetlstOrderProductsByIdOrder(long id)
        {
            var data = db.OrderProducts.Where(x => x.OrderId == id).ToList();
            var result = data.Select(x => new ItemInforProductViewModel()
            {
                Discount = x.Discount,
                OrderId = x.OrderId,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                Unitprice = x.Unitprice
            }).ToList();
            foreach (ItemInforProductViewModel item in result)
            {
                Product product = db.Products.SingleOrDefault(p => p.ProductId == item.ProductId);
                item.ProductName = product.ProductName;
                item.ThumbUrl = product.MediaThumb;
                item.MicrositeId = product.MicrositeID ?? 0;
                item.MicrositeName = product.MicrositeName;
                item.CateMicrositeID = product.CateMicrositeID ?? 0;
                item.CateMicrositeName = product.CateMicrositeName;
                item.FriendlyUrl = product.FriendlyURL;
            }
            return result;
        }

        public List<OrderProductExt> GetlstOrderProductExt(long id)
        {
            try
            {
                List<OrderProductExt> lstobj = this.db.Database.SqlQuery<OrderProductExt>("exec GetLstProductOederExt @OrderId",
                            new SqlParameter("@OrderId", id)).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




    }
}
