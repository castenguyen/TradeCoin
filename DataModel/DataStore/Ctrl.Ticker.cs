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




        public List<Ticker> GetListTickerByUser(long UserId, int Num, long PackageId)
        {
            var lstContentItem = db.Database.SqlQuery<Ticker>("exec GetTickerByUser @Useruid, @NumReord,  @PackageId ",
                 new SqlParameter("@Useruid", UserId),
                 new SqlParameter("@NumReord", Num),
                new SqlParameter("@PackageId", PackageId)).ToList();
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



        public IQueryable<MiniTickerViewModel> GetTickerByUserLinq(long UserId)
        {
            ///lấy package id của user
            long packageiduser = (from user in db.Users where user.Id == UserId select user.PackageId.Value).ToList().FirstOrDefault();

            //lấy gói package mà user có thể xem được
            long[] lstpackageid = (from pa in db.Packages where pa.PackageId <= packageiduser select pa.PackageId).ToArray();

            //lấy danh sách id của ticker thuoc gói cước bên trên
            long[] lstTickerid = (from tk in db.Tickers

                                      join cp in db.ContentPackages on tk.TickerId equals cp.ContentId

                                      where cp.ContentType == (int)EnumCore.ObjTypeId.ticker && tk.StateId == (int)EnumCore.TickerStatusType.dang_chay 
                                        
                                      && lstpackageid.Contains(cp.PackageId)
                                      //tránh trùng lặp
                                      select tk.TickerId).Distinct().ToArray();


            IQueryable<MiniTickerViewModel> rs = from tk in db.Tickers
                                             join cv in db.ContentViews on tk.TickerId equals cv.ContentId into all
                                             from l in all.DefaultIfEmpty()

                                             where lstTickerid.Contains(tk.TickerId)


                                             select (new MiniTickerViewModel
                                             {
                                        TickerId =tk.TickerId,
                                        TickerName = tk.TickerName,
                                        BuyZone1 = tk.BuyZone1,
                                        SellZone1 = tk.SellZone1,
                                        SellZone2 = tk.SellZone2,
                                        SellZone3 = tk.SellZone3,
                                        BTCInput = tk.BTCInput,
                                        DeficitControl = tk.DeficitControl,
                                        Description = tk.Description,
                                        CrtdUserName = tk.CrtdUserName,
                                        CrtdUserId = tk.CrtdUserId,
                                        CrtdDT = tk.CrtdDT,
                                        AprvdUserName = tk.AprvdUserName,
                                        AprvdUID = tk.AprvdUID,
                                        AprvdDT = tk.AprvdDT,
                                        StateName = tk.StateName,
                                        StateId = tk.StateId,
                                        MediaUrl = tk.MediaUrl,
                                        MediaThumb = tk.MediaThumb,
                                        Flag = tk.Flag,
                                        Profit = tk.Profit,
                                        Deficit = tk.Profit,
                                        tmp = (l.ContentId > 0) ? 1 : 0
                                      
                                    });
            return rs.Distinct();
        }



        public bool CheckTickerUserPackage(long TickerId, long UserId)
        {
            long packageiduser = (from user in db.Users where user.Id == UserId select user.PackageId.Value).ToList().FirstOrDefault();
            long[] lstContentItemsPackage = (from pa in db.ContentPackages
                                             where pa.ContentType == (int)EnumCore.ObjTypeId.ticker && pa.ContentId == TickerId
                                             select pa.PackageId).ToArray();
            foreach (long _val in lstContentItemsPackage)
            {
                if (packageiduser >= _val)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
