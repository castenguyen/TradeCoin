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
        public async Task<bool> DeleteProductMicrosite(long idProduct, long idUser)
        {
            Product product = db.Products.SingleOrDefault(x => x.CrtdUserId == idUser && x.ProductId == idProduct);
            if (product != null)
            {
                var orderOfProduct = db.OrderProducts.FirstOrDefault(x => x.ProductId == product.ProductId);
                if (orderOfProduct != null)
                {
                    product.StateId = (int)EnumCore.StateType.da_xoa;
                }
                else
                {
                    IQueryable<ContentComment> comment = db.ContentComments.Where(x => x.ContentObjId == idProduct && x.ObjTypeId == (int)EnumCore.ObjTypeId.san_pham);
                    db.ContentComments.RemoveRange(comment);
                    IQueryable<OrderProduct> itemOrder = db.OrderProducts.Where(x => x.ProductId == idProduct);
                    foreach (OrderProduct item in itemOrder.Distinct())
                    {
                        Order order = db.Orders.SingleOrDefault(x => x.Id == item.OrderId);
                        db.Orders.Remove(order);
                    }
                    db.OrderProducts.RemoveRange(itemOrder);
                    db.Products.Remove(product);
                }
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Order> AddOrder(Order order)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Order od = db.Orders.Add(order);
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return od;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }

        public Order GetObjOrderById(long id)
        {
            return db.Orders.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Order> GetlstOrderOfCustomer(long id)
        {
            return db.Orders.Where(x => x.CustomerId == id);
        }

        public IQueryable<Order> GetlstOrder()
        {
            return db.Orders;
        }

        public async Task<int> UpdateObjOrder(Order model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public async Task<int> DeleteObjOrder(long id)
        {

            try
            {
                Order model = db.Orders.Find(id);
                model.StatusId = (int)EnumCore.StateType.da_xoa;
                db.Entry(model).State = EntityState.Modified;
                //List<OrderProduct> lstOrderProduct = db.OrderProducts.Where(s => s.OrderId == id).ToList();
                //foreach (OrderProduct _item in lstOrderProduct)
                //{
                //    db.OrderProducts.Remove(_item);
                //}
                //db.Orders.Remove(model);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

    }
}
