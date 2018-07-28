using DataModel.DataEntity;
using DataModel.DataViewModel;
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
        public async Task<int> AddObjMicroClassifiAsync(MicroClassification ObjClass)
        {
            try
            {
                if (!string.IsNullOrEmpty(ObjClass.MicroClassifiNM))
                {
                    db.MicroClassifications.Add(ObjClass);
                    return await db.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }

        public int AddObjMicroClassifi(MicroClassification ObjClass)
        {
            try
            {
                if (!string.IsNullOrEmpty(ObjClass.MicroClassifiNM))
                {
                    db.MicroClassifications.Add(ObjClass);
                    return db.SaveChanges();
                }
                return 0;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }

        public MicroClassification GetObjMicroClassifiByID(long id)
        {
            MicroClassification ObjMicroClassifi = db.MicroClassifications.Single(m => m.MicroClassifiId == id);
            return ObjMicroClassifi;
        }

        public MicroClassification GetObjMicroClassifiByFriendlyUrl(string FriendlyURL)
        {
            try
            {
                MicroClassification Obj = db.MicroClassifications.FirstOrDefault(c => c.FriendlyURL == FriendlyURL);
                return Obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<MicroClassification> GetlstMicroClassificationByEmailUser(string email)
        {
            return db.MicroClassifications.Where(x=>x.CrtdUserName == email);
        }

        public async Task<int> UpdateMicroClassAsync(MicroClassification ObjClass)
        {
            try
            {
                db.Entry(ObjClass).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public IQueryable<MicroClassification> GetlstMicroClassifi(long? Parent, string FriendlyUrl, long? CrtdUserID, long? MicroID)
        {
            IQueryable<MicroClassification> lstClassifi = db.MicroClassifications;
            if (Parent.HasValue)
                lstClassifi = lstClassifi.Where(s => s.MicroParentClassifiId == Parent);
            if (CrtdUserID.HasValue)
                lstClassifi = lstClassifi.Where(s => s.CrtdUID == CrtdUserID);
            if (MicroID.HasValue)
                lstClassifi = lstClassifi.Where(s => s.MicroID == MicroID);
            if (!string.IsNullOrEmpty(FriendlyUrl))
                lstClassifi = lstClassifi.Where(s => s.FriendlyURL == FriendlyUrl);
            return lstClassifi;
        }
        /// <summary>
        /// //lấy catagory cua microsite theo id của microsite kieu trả về là List<SelectListObj>
        /// </summary>
        /// <param name="schemeid"></param>
        /// <returns></returns>
        public List<SelectListObj> GetlstMicroCateByMicroId(long MicrositeId)
        {
            List<SelectListObj> mylist = new List<SelectListObj>();

            mylist = db.MicroClassifications.Where(s => s.MicroID == MicrositeId).
                Select(p => new SelectListObj { value = p.MicroClassifiId, text = p.MicroClassifiNM }).ToList();
            return mylist;
        }


        public async Task<int> MicroChangeOrderByUp(int idchange, int? _ParentClass, long MicrositeID)// Tăng DisplayOrder
        {
            MicroClassification _MainObj = db.MicroClassifications.Single(c => c.MicroClassifiId == idchange);
            MicroClassification _ClassNext = db.MicroClassifications.Where(c => c.MicroID == MicrositeID && (c.MicroParentClassifiId == null || c.MicroParentClassifiId == 0)
                    && c.DisplayOrder > _MainObj.DisplayOrder).OrderBy(c => c.DisplayOrder).FirstOrDefault();
            if (_ClassNext != null)
            {
                _MainObj.DisplayOrder = _MainObj.DisplayOrder + 1;
                _ClassNext.DisplayOrder = _ClassNext.DisplayOrder - 1;
                db.Entry(_MainObj).State = EntityState.Modified;
                db.Entry(_ClassNext).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return (int)EnumCore.Result.action_true;
        }
        public async Task<int> MicroChangeOrderByDown(int idchange, int? _ParentClass, long MicrositeID)// Giảm DisplayOrder
        {
            MicroClassification _MainObj = db.MicroClassifications.Single(c => c.MicroClassifiId == idchange);
            MicroClassification _ClassNext = db.MicroClassifications.Where(c => c.MicroID == MicrositeID && (c.MicroParentClassifiId == null || c.MicroParentClassifiId == 0)
                    && c.DisplayOrder < _MainObj.DisplayOrder).OrderByDescending(c => c.DisplayOrder).FirstOrDefault();
            if (_ClassNext != null)
            {
                _MainObj.DisplayOrder = _MainObj.DisplayOrder - 1;
                _ClassNext.DisplayOrder = _ClassNext.DisplayOrder + 1;
                db.Entry(_MainObj).State = EntityState.Modified;
                db.Entry(_ClassNext).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return (int)EnumCore.Result.action_true;
        }
        public int GetMaxDisplayOrderForCateMicrosite(long MicrositeID)
        {
            try {
                int rs = db.MicroClassifications.Where(s => s.MicroID == MicrositeID).Max(s => s.DisplayOrder).Value;
                if (rs == 0)
                    return 1;
                return rs;

            }
            catch {
                return 1;
            }
         
        }
        public async Task<int> RemoveMicroClassById(long id)
        {
            MicroClassification model = db.MicroClassifications.Find(id);
            if (model != null)
            {
                db.MicroClassifications.Remove(model);
                await db.SaveChangesAsync();
            }

            return (int)EnumCore.Result.action_true;
        }



    }
}
