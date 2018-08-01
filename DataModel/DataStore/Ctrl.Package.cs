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
        public async Task<int> CreatePackageAsync(Package ObjPackage)
        {
            try
            {
                if (!string.IsNullOrEmpty(ObjPackage.PackageName))
                {
                    db.Packages.Add(ObjPackage);
                    return await db.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public Package GetObjPackage(int PackageId)
        {
            var ObjPackage = db.Packages.FirstOrDefault(c => c.PackageId == PackageId);
            return ObjPackage;
        }


        public IQueryable<Package> GetlstPackage()
        {
            var lstPackage = db.Packages;
            return lstPackage;
        }

        public async Task<int> UpdatePackage(Package ObjPackage)
        {
            try
            {
                db.Entry(ObjPackage).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public List<SelectListObj> GetObjSelectListPackage()
        {
            List<SelectListObj> lstobj = this.GetlstPackage().Where(s => s.StateId == (int)EnumCore.StateType.enable).
                Select(p => new SelectListObj { value = p.PackageId, text = p.PackageName }).ToList();
            return lstobj;
        }


    }
}
