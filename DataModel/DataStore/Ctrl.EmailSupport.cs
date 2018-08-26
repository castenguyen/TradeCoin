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

        public IQueryable<MiniEmailSupportViewModel> GetEmailSupportByLinq(int isUserAdmin,long UserId)
        {
          

            IQueryable<MiniEmailSupportViewModel> rs = from em in db.EmailSupports
                                                 join cv in db.ContentViews on em.EmailId equals cv.ContentId  into all
                                                 from l in all.DefaultIfEmpty()
                                                  select (new MiniEmailSupportViewModel
                                                 {
                                                      EmailId =em.EmailId,
                                                     EmailName = em.EmailName,
                                                     Subject = em.Subject,
                                                     Content = em.Content,
                                                     DestinationId = em.DestinationId,
                                                     DestinationName = em.DestinationName,
                                                     ParentId = em.ParentId,
                                                     ParentName = em.ParentName,
                                                     CrtdUserName = em.CrtdUserName,
                                                     CrtdUserId = em.CrtdUserId,
                                                     CrtdDT = em.CrtdDT,
                                                     AprvdUserName = em.AprvdUserName,
                                                     AprvdUID = em.AprvdUID,
                                                     AprvdDT = em.AprvdDT,
                                                     StateName = em.StateName,
                                                     StateId = em.StateId,
                                                     EmailTypeId = em.EmailTypeId,
                                                     EmailTypeName = em.EmailTypeName,
                                                     StateName2 = em.StateName2,
                                                     StateId2 = em.StateId2,
                                                     tmp = (l.ContentId > 0) ? 1 : 0
                                                 });

            if (isUserAdmin == 1)
            {
                rs = rs.Where(s => s.StateId != (int)EnumCore.EmailStatus.da_xoa);
            }
            else {

                rs = rs.Where(s => s.StateId2 != (int)EnumCore.EmailStatus.da_xoa);
            }
            return rs.Distinct();
        }





    }
}
