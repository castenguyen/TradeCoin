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
        #region Begin BackEnd
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjContentItem"></param>
        /// <returns></returns>
        public async Task<int> CreateContentItem(ContentItem ObjContentItem)
        {
            try
            {
                db.ContentItems.Add(ObjContentItem);
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {

                return (int)EnumCore.Result.action_false;
            }
        }

        public int CreateContentItemNoAsync(ContentItem ObjContentItem)
        {
            try
            {
                db.ContentItems.Add(ObjContentItem);
                 db.SaveChanges();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {

                return (int)EnumCore.Result.action_false;
            }
        }


        public async Task<int> DeleteContentItemByObj(ContentItem ObjContentItem)
        {
            try
            {
                //db.ContentItems.Remove(ObjContentItem);
                ObjContentItem.StateId = (int)EnumCore.StateType.da_xoa;
                ObjContentItem.StateName = "Đã Xoá";
                db.Entry(ObjContentItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjContentItem"></param>
        /// <returns></returns>
        public async Task<int> UpdateContentItem(ContentItem ObjContentItem)
        {
            try
            {
                db.Entry(ObjContentItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception ex)
            {
                return (int)EnumCore.Result.action_false;
            }
        }
        /// <summary>
        /// insert 1 nội dung liên quan vào DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public async Task<int> CreateRelatedContent(RelatedContentItem model)
        {
            try
            {
                db.RelatedContentItems.Add(model);
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {

                return (int)EnumCore.Result.action_false;
            }
        }
        public IQueryable<ContentItem> GetlstContentItem()
        {
            var lstContentItems = db.ContentItems;
            return lstContentItems;
        }
        public IEnumerable GetContentItemList()
        {
            try
            {
                List<ContentItem> lstobj = this.GetlstContentItem().ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ContentItem GetObjContentItemByFriendlyURL(string FriendlyURL)
        {
            try
            {
                ContentItem Obj = db.ContentItems.FirstOrDefault(c => c.FriendlyURL == FriendlyURL);
                return Obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ContentItem GetObjContentItemById(long id)
        {
            try
            {
                ContentItem Obj = db.ContentItems.FirstOrDefault(c => c.ContentItemId == id);
                return Obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ContentItem GetObjPageInforMicrosite(long MicrositeId, int PageType)
        {
            ContentItem Obj = db.ContentItems.FirstOrDefault(c => c.MicrositeID == MicrositeId
                && c.ObjTypeId == (int)EnumCore.ObjTypeId.page_infor_microsite && c.CategoryId == PageType);
            return Obj;
        }



        /// <summary>
        /// Lấy List nội dung liên quan ở đây là ContentItem liên quan với ContentItem
        /// trả về sẽ là 1 list ContentItem
        /// </summary>
        /// <param name="id">id của contenitem đưa vào</param>
        /// <returns></returns>
        public List<ContentItem> GetlstRelatedContentByContentId(long id)
        {
            List<ContentItem> model = new List<ContentItem>();
            List<RelatedContentItem> lstRelatedContent = new List<RelatedContentItem>();
            lstRelatedContent = db.RelatedContentItems.Where(t => t.ObjContentItemId == id).ToList();
            foreach (RelatedContentItem _item in lstRelatedContent)
            {
                model.Add(this.GetObjContentItemById(_item.SubjectContentItemId));
            }
            return model;
        }
        public async Task<int> RemoveRelatedContentByContentItemId(long id, int Relatedtype)
        {
            try
            {
                List<RelatedContentItem> lstRelatedContent = new List<RelatedContentItem>();
                lstRelatedContent = db.RelatedContentItems.Where(t => t.ObjContentItemId == id && t.ContentRelatedTypeId == Relatedtype).ToList();
                foreach (RelatedContentItem _item in lstRelatedContent)
                {
                    db.RelatedContentItems.Remove(_item);
                    await db.SaveChangesAsync();
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }


        public IList<ContentItem> GetlstPaginationContentItemDesc(int? row, int page,int type) 
        {
            int rowSkip = row == null || page == 1 ? 0: (row.Value*page)+1;
            return db.Database.SqlQuery<ContentItem>("exec ContentItemPagination @rowSkip, @rowTake, @type", new SqlParameter("@rowSkip", rowSkip), new SqlParameter("@rowTake", row), new SqlParameter("@type", type)).ToList();
        }
   

        public IEnumerable GetContentListForSelectlist()
        {
            try
            {
                var lstobj = (from c in this.GetlstContentItem() select new { c.ContentItemId, c.ContentTitle }).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<ContentItem> GetlstContentItemByCataId(int cata, int num)
        {
            try
            {
                List<ContentItem> lstobj = this.GetlstContentItem().Where(p => p.CategoryId == cata).Take(num).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        #endregion Stop BackEnd
    }
}
