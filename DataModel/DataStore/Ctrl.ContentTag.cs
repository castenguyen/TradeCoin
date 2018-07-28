using DataModel.DataEntity;
using DataModel.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public async Task<int> AddTag(Tag _Tag)
        {
            try
            {
                db.Tags.Add(_Tag);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return (int)EnumCore.Result.action_false;
            }
        }
        public IQueryable<Tag> GetlstTag()
        {
            var lstTag = db.Tags;
            return lstTag;
        }

        public async Task<int> RemoveRelatedTagByContentItemId(long id,int type)
        {
            try
            {
                List<ContentTag> lstContentItemTag = new List<ContentTag>();
                lstContentItemTag = db.ContentTags.Where(t => t.ObjcontentId == id && t.ObjTypeId == type).ToList();
                foreach (ContentTag _item in lstContentItemTag)
                {
                    db.ContentTags.Remove(_item);
                    await db.SaveChangesAsync();
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public IEnumerable GetTagList()//lấy list TAG
        {
            try
            {
                List<Tag> lstobjTag = this.GetlstTag().ToList();
                return lstobjTag;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<int> CreateRelatedTag(ContentTag model)
        {
            try
            {
                db.ContentTags.Add(model);
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {

                return (int)EnumCore.Result.action_false;
            }
        }
        public async Task<int> DeleteRelatedTag(long ContentId, int Objtype)
        {

            int rs = await this.RemoveRelatedTagByContentItemId(ContentId, Objtype);
            return rs;

        }

    }
}
