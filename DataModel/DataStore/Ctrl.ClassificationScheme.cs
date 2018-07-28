using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
using System.Data.Entity;
using System.Collections;
namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public async Task<int> AddClassScheme(ClassificationScheme ObjClassScheme)
        {
            if (!string.IsNullOrEmpty(ObjClassScheme.ClassificationSchemeNM))
            {
                db.ClassificationSchemes.Add(ObjClassScheme);
                return await db.SaveChangesAsync();
            }
            return 0;
        }

        public IQueryable<ClassificationScheme> GetlstScheme()
        {
            var lstScheme = db.ClassificationSchemes;
            return lstScheme;
        }

        public IEnumerable GetLstClassificationScheme()
        {
            List<ClassificationScheme> lstobj = this.GetlstScheme().ToList();
            return lstobj;
        }

        public ClassificationScheme GetObjScheme(int SchemeId)
        {
            var ObjScheme = db.ClassificationSchemes.FirstOrDefault(c => c.ClassificationSchemeId == SchemeId);
            return ObjScheme;
        }


        public async Task<int> EditClassScheme(ClassificationScheme ObjClassScheme)
        {
            try
            {
                db.Entry(ObjClassScheme).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        //public async Task<int> DeleteClassScheme(int? SchemeId)
        //{
        //    ClassificationScheme ObjClassScheme = db.ClassificationSchemes.FirstOrDefault(c => c.ClassificationSchemeId == SchemeId);
        //    db.ClassificationSchemes.Remove(ObjClassScheme);
        //    return await db.SaveChangesAsync();
        //}
        //public IQueryable<ClassificationScheme> GetlstScheme()
        //{
        //    var lstScheme = db.ClassificationSchemes;
        //    return lstScheme;
        //}
       

        
    }
}
