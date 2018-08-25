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

        public List<Ticker> GetListTickerByUserToPageList(long UserId, int skip,int take)
        {
            var lstContentItem = db.Database.SqlQuery<Ticker>("exec GetListTickerByUserToPageList @Useruid, @take,@skip ",
                 new SqlParameter("@Useruid", UserId),
                 new SqlParameter("@take", take),
                 new SqlParameter("@skip", skip)

                 ).ToList();
            return lstContentItem;
        }



        public IQueryable<Ticker> GetTickerByUserLinq(long UserId)
        {

            long packageiduser = (from user in db.Users where user.Id == UserId select user.PackageId.Value).ToList().FirstOrDefault();

            long[] lstpackageid = (from pa in db.Packages where pa.PackageId < packageiduser select pa.PackageId).ToArray();

            long[] lstTickerid = (from tk in db.Tickers

                                      join cp in db.ContentPackages on tk.TickerId equals cp.ContentId

                                      where cp.ContentType == (int)EnumCore.ObjTypeId.ticker && tk.StateId == (int)EnumCore.TickerStatusType.dang_chay && lstpackageid.Contains(cp.PackageId)

                                      select tk.TickerId).ToArray();

            IQueryable<Ticker> rs = from tk in db.Tickers where lstTickerid.Contains(tk.TickerId) select tk;

            return rs;
        }


    }
}
