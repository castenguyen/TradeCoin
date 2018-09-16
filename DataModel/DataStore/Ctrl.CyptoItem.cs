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

        public async Task<int> CreateCyptoItem(CyptoItem ObjCyptoItem)
        {
            try
            {
                db.CyptoItems.Add(ObjCyptoItem);
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }
        }
        public async Task<int> UpdateCyptoItem(CyptoItem ObjCyptoItem)
        {
            try
            {
                db.Entry(ObjCyptoItem).State = EntityState.Modified;
                int rs = await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception ex)
            {
                return (int)EnumCore.Result.action_false;
            }
        }



        public IQueryable<CyptoItem> GetlstCyptoItem()
        {
            var lstcypto = db.CyptoItems;
            return lstcypto;
        }
        public IQueryable<CyptoItemPrice> GetlstCyptoItemPrice()
        {
            var lstcypto = db.CyptoItemPrices;
            return lstcypto;
        }

        public CyptoItem GetObjCyptoItem(long id)
        {
            try {
                CyptoItem ObjCyptoItem = db.CyptoItems.Find(id);
                return ObjCyptoItem;
            }
            catch (Exception e)
            {

                return null;
            }
         

        }


        public async Task<int> UpdateCypto(CyptoItem ObjCyptoItem)
        {
            try
            {
                db.Entry(ObjCyptoItem).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("UpdateCypto", "Ctrl.CyptoItem", ex.ToString());
                return 0;
            }
        }
        public string  GetCyptoName(long id)
        {
            string lstcypto = db.CyptoItems.Where(s=>s.id== id).Select(s=>s.name).First();
            return lstcypto;
        }

    }
}
