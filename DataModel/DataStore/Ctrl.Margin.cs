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
        public async Task<int> CreateMarginAsync(Margin ObjMargin)
        {
            try
            {

                db.Margins.Add(ObjMargin);
                return await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public Margin GetObjMargin(long MarginId)
        {
            var ObjTicker = db.Margins.FirstOrDefault(c => c.MarginId == MarginId);
            return ObjTicker;
        }

        public Margin GetlastMargin()
        {
            var ObjTicker = db.Margins.OrderByDescending(s => s.CrtdDT).FirstOrDefault();
            return ObjTicker;
        }





        public IQueryable<Margin> GetlstMargin()
        {
            var lstMargin = db.Margins;
            return lstMargin;
        }





        public async Task<int> UpdateMargin(Margin ObjMargin)
        {
            try
            {
                db.Entry(ObjMargin).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<int> DeleteMargin(Margin ObjMargin)
        {
            try
            {
                db.Margins.Remove(ObjMargin);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// lấy danh sách margin theo user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public MarginViewModel GetMarginByPackageLinq(long PackageId)
        {


            ///lấy list id của margin ko trung lap
            long lstMarginid = (  from cp in db.ContentPackages
                                  where cp.ContentType == (int)EnumCore.ObjTypeId.margin && cp.PackageId== PackageId
                                  orderby cp.ContentId descending
                                  select cp.ContentId
                                  ).FirstOrDefault();




            //lấy list margin
            MarginViewModel rs = (from ma in db.Margins
                                             join co in db.ContentPackages on ma.MarginId equals co.ContentId into all
                                             from l in all.DefaultIfEmpty()
                                             where ma.MarginId== lstMarginid


                                             select (new MarginViewModel
                                             {
                                                 MarginId = ma.MarginId,
                                                 MarginName = ma.MarginName,
                                                 Long = ma.Long,
                                                 LongStop = ma.LongStop,
                                                 LongRate = ma.LongStop,
                                                 LongNote = ma.LongNote,
                                                 Short = ma.Short,
                                                 ShortStop = ma.ShortStop,
                                                 ShortRate = ma.ShortRate,
                                                 ShortNote = ma.ShortNote,
                                                 CrtdUserName = ma.CrtdUserName,
                                                 CrtdUserId = ma.CrtdUserId,
                                                 CrtdDT = ma.CrtdDT,
                                                 StateName = ma.StateName,
                                                 StateId = ma.StateId,
                                                 PackageName = l.PackageName,
                                                 PackageId = l.PackageId

                                             })).FirstOrDefault();

            return rs;
        }







    }
}
