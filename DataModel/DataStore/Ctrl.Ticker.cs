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
        public async Task<int> CreateTickerAsync(Ticker ObjTicker)
        {
            try
            {
                if (!string.IsNullOrEmpty(ObjTicker.TickerName))
                {
                    db.Tickers.Add(ObjTicker);
                    return await db.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public Ticker GetObjTicker(long TickerId)
        {
            var ObjTicker = db.Tickers.FirstOrDefault(c => c.TickerId == TickerId);
            return ObjTicker;
        }


        public IQueryable<Ticker> GetlstTicker()
        {
            var lstTicker = db.Tickers;
            return lstTicker;
        }

        public async Task<int> UpdateTicker(Ticker ObjTicker)
        {
            try
            {
                db.Entry(ObjTicker).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteTicker(Ticker TickerObj)
        {
            try
            {
                TickerObj.StateId = (int)EnumCore.TickerStatusType.da_xoa;
                TickerObj.StateName = "Đã Xoá";
                db.Entry(TickerObj).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }




        public List<Ticker> GetListTickerByUser(long UserId, int Num)
        {
            var lstContentItem = db.Database.SqlQuery<Ticker>("exec GetTickerByUser @Useruid, @NumReord ",
                 new SqlParameter("@Useruid", UserId),
                 new SqlParameter("@NumReord", Num)

                 ).ToList();
            return lstContentItem;
        }



    }
}
