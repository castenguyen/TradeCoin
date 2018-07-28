using DataModel.DataEntity;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public async Task<int> CreateDisplayContent(DisplayContent ObjDisplayContent)
        {
            try
            {
                db.DisplayContents.Add(ObjDisplayContent);
                return await db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public async Task<int> EditDisplayContent(DisplayContent ObjDisplayContent)
        {
            try
            {
                db.Entry(ObjDisplayContent).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IQueryable<DisplayContent> GetObjDisplayContent(long ContentObjId, int ObjTypeId)
        {
            var lstDisplayContent = db.DisplayContents.Where(s => s.ContentId == ContentObjId && s.ObjType == ObjTypeId);
            return lstDisplayContent;
        }

        public IQueryable<DisplayContent> GetlstDisplayContent()
        {
            var lstDisplayContent = db.DisplayContents;
            return lstDisplayContent;
        }
        public IQueryable<DisplayContent> GetlstDisplayContent(long? ContentId, int? DisplayTypeId, int? ObjType)
        {
            IQueryable<DisplayContent> lstDisplayContent = db.DisplayContents;
            if (ContentId.HasValue)
            {
                lstDisplayContent = lstDisplayContent.Where(s => s.ContentId == ContentId.Value);
            }
            if (DisplayTypeId.HasValue)
            {
                lstDisplayContent = lstDisplayContent.Where(s => s.DisplayType == DisplayTypeId.Value);
            }
            if (ObjType.HasValue)
            {
                lstDisplayContent = lstDisplayContent.Where(s => s.ObjType == ObjType.Value);
            }
            return lstDisplayContent;
        }
        public async Task<int> GetMaxOrderDisplayForDisplaycontent(int DisplayTypeId)
        {
            try
            {
                int rs = await db.DisplayContents.Where(s => s.DisplayType == DisplayTypeId).MaxAsync(s => s.DisplayOrder.Value);
                return rs;
            }
            catch
            {
                return 0;
            }

        }
        public async Task<int> DeleteDisplayContent(DisplayContent ObjDisplayContent)
        {
            try
            {
                db.DisplayContents.Remove(ObjDisplayContent);
                return await db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }
        }
        public async Task<DisplayContent> GetObjDisplayContent(int DisplayContentId)
        {
            try
            {
                DisplayContent model = new DisplayContent();
                model = await db.DisplayContents.FindAsync(DisplayContentId);
                return model;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<int> DeleteDisplayContentByContentObj(long ContentObjId, int ObjTypeId)
        {
            try
            {
                List<DisplayContent> LstObjDisplayContent = this.GetObjDisplayContent(ContentObjId, ObjTypeId).ToList();
                foreach (DisplayContent _val in LstObjDisplayContent)
               {
                   await this.DeleteDisplayContent(_val);
               }
                return await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }
        }


    }
}
