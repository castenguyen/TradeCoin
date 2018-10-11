using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public async Task<int> CreateCyptoItemPrice(CyptoItemPrice ObjCyptoItemPrice)
        {
            try
            {
              
                db.CyptoItemPrices.Add(ObjCyptoItemPrice);
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }
        }
        public async Task<int> UpdateCyptoItemPrice(CyptoItemPrice ObjCyptoItemPrice)
        {
            try
            {
                db.Entry(ObjCyptoItemPrice).State = EntityState.Modified;
                int rs = await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception ex)
            {
                return (int)EnumCore.Result.action_false;
            }
        }


        public  IQueryable<CyptoItemPrice> GetLstCyptoItemPrice()
        {
            try
            {
                IQueryable<CyptoItemPrice> tmp = db.CyptoItemPrices;
                return tmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool RemoveListCyptoItemPrice(List<CyptoItemPrice> lst)
        {
            try
            {
                db.CyptoItemPrices.RemoveRange(lst);
                int rs =  db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool TruncatableCyptoItemPrice()
        {
            try
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [CyptoItemPrice]");
                int rs = db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
