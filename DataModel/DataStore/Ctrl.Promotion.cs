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
        public IQueryable<PromotionContent> GetlstPromotionContent()
        {
            IQueryable<PromotionContent> lstScheme = db.PromotionContents;
            return lstScheme;
        }
        public IQueryable<PromotionContent> GetlstPromotionContent(long? MainContentId, int? MainCateId, int? ObjType, int? PromotionType, long? SubContentId)
        {
            IQueryable<PromotionContent> lstScheme = db.PromotionContents;
            if (MainContentId.HasValue)
            {
                lstScheme = lstScheme.Where(s => s.MainContentId == MainContentId);
            }
            if (MainCateId.HasValue)
            {
                lstScheme = lstScheme.Where(s => s.MainCateId == MainCateId);
            }
            if (ObjType.HasValue)
            {
                lstScheme = lstScheme.Where(s => s.ObjType == ObjType);
            }
            if (PromotionType.HasValue)
            {
                lstScheme = lstScheme.Where(s => s.PromotionType == PromotionType);
            }
            if (SubContentId.HasValue)
            {
                lstScheme = lstScheme.Where(s => s.SubContentId == SubContentId);
            }
            return lstScheme;
        }
        public async Task<int> CreatePromotionContent(PromotionContent ObjPromotionContent)
        {
            try
            {
                db.PromotionContents.Add(ObjPromotionContent);
                return await db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public PromotionContent GetObjPromotionContentById(long id)
        {
            PromotionContent Obj = db.PromotionContents.Find(id);
            return Obj;
        }

        public async Task<int> RemoveObjPromotionContent(PromotionContent model)
        {
           db.PromotionContents.Remove(model);
           await db.SaveChangesAsync();
           return (int)EnumCore.Result.action_true;
        }

    }
}
