using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
using DataModel.Extension;
using System.Data.Entity;
using System.Collections;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {

        public async Task<int> CreateContentViewAsync(ContentView ObjContentView)
        {
            try
            {
                db.ContentViews.Add(ObjContentView);
                return await db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }

        public int CreateContentView(ContentView ObjContentView)
        {
            try
            {
                db.ContentViews.Add(ObjContentView);
                return db.SaveChanges();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }



        public int DeleteContentView(long ContentId, int Type)
        {
            List<ContentView> lstContentView = db.ContentViews.Where(s => s.ContentId == ContentId && s.ContentType == Type).ToList();
            foreach (ContentView item in lstContentView)
            {
                db.ContentViews.Remove(item);
                db.SaveChanges();
            }
            return (int)EnumCore.Result.action_true;
        }


        public long[] GetlstContentView(long UserId, int Type)
        {
            long[] lstContentView = db.ContentViews.Where(s => s.UserId == UserId && s.ContentType == Type).Select(s => s.ContentId).ToArray();
            return lstContentView;
        }

        public long[] GetlstUserView(long ContentId, int Type)
        {
            long[] lstUserView = db.ContentViews.Where(s => s.ContentId == ContentId && s.ContentType == Type).Select(s => s.UserId).ToArray();
            return lstUserView;
        }


        public List<ContentView> GetlstObjContentView(long ContentId, int Type)
        {
            List<ContentView> lstContentView = db.ContentViews.Where(s => s.ContentId == ContentId && s.ContentType == Type).ToList();
            return lstContentView;
        }

        public ContentView GetObjContentView(long ContentId, int Type,long UserID)
        {
            ContentView lstContentView = db.ContentViews.Where(s => s.ContentId == ContentId && s.ContentType == Type && s.UserId== UserID).FirstOrDefault();
            return lstContentView;
        }






    }
}
