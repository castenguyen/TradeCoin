using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using DataModel.DataEntity;
using DataModel.Extension;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public async Task<int> CreateUserHistory(long idUser, string IpAddress, int ActionTypeID, string ActionTypeName,
                        long ActionObj, string ActionObjName, string tableName,int ObjTypeId)
        {
            Userhist userHist = new Userhist();
            userHist.CrtdUID = idUser;
            userHist.CrtdDT = DateTime.Now;
            userHist.IPadress = IpAddress;
            userHist.ActionObjId = ActionObj;
            userHist.ActionTypeId = ActionTypeID;
            userHist.ActionTypeName = ActionTypeName;
            userHist.ActionObjName = ActionObjName;
            userHist.Tablename = tableName;
            userHist.ObjTypeId = ObjTypeId;
            try
            {
                db.Userhists.Add(userHist);
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }
        }


        public int CreateUserHistoryNoAsync(long idUser, string IpAddress, int ActionTypeID, string ActionTypeName,
                      long ActionObj, string ActionObjName, string tableName, int ObjTypeId,string OldValue,string NewValue)
        {
            Userhist userHist = new Userhist();
            userHist.CrtdUID = idUser;
            userHist.CrtdDT = DateTime.Now;
            userHist.IPadress = IpAddress;
            userHist.ActionObjId = ActionObj;
            userHist.ActionTypeId = ActionTypeID;
            userHist.ActionTypeName = ActionTypeName;
            userHist.ActionObjName = ActionObjName;
            userHist.Tablename = tableName;
            userHist.ObjTypeId = ObjTypeId;

            userHist.Oldvalue = OldValue;
            userHist.Newvalue = NewValue;

            try
            {
                db.Userhists.Add(userHist);
                db.SaveChanges();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }
        }

        public List<Userhist> GetLstUserHistByUserId(long UserId, int Num)
        {
                List<Userhist> model = new List<Userhist>();
                model = db.Userhists.Where(s => s.CrtdUID == UserId).OrderByDescending(s => s.Id).Take(Num).ToList();
                return model;
        }

        public IQueryable<Userhist> GetlstActionHistory()
        {
            return db.Userhists;
        }




    }
}
