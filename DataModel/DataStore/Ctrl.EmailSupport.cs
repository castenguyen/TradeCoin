using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
using System.Data.Entity;
using System.Collections;
using DataModel.Extension;
using DataModel.DataViewModel;
namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public async Task<int> CreateEmailSupportAsync(EmailSupport ObjEmailSupport)
        {
            try
            {
                    db.EmailSupports.Add(ObjEmailSupport);
                    return await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }
        }

        public int CreateEmailSupport(EmailSupport ObjEmailSupport)
        {
            try
            {
                 db.EmailSupports.Add(ObjEmailSupport);
                return  db.SaveChanges();
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }
        }

        public EmailSupport GetObjEmailSupport(long EmailSupportId)
        {
            var ObjEmailSupport = db.EmailSupports.FirstOrDefault(c => c.EmailId == EmailSupportId);
            return ObjEmailSupport;
        }


        public IQueryable<EmailSupport> GetlstEmailSupport()
        {
            var lstEmailSupport = db.EmailSupports;
            return lstEmailSupport;
        }

        public async Task<int> UpdateEmailSupport(EmailSupport ObjEmailSupport)
        {
            try
            {
                db.Entry(ObjEmailSupport).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteEmailSupport(EmailSupport EmailSupportObj)
        {
            try
            {
                db.EmailSupports.Remove(EmailSupportObj);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
