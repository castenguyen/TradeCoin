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
        public async Task<int> CreateContentPackageAsync(ContentPackage ObjContentPackage)
        {
            try
            {
                    db.ContentPackages.Add(ObjContentPackage);
                    return await db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }

        public int CreateContentPackage(ContentPackage ObjContentPackage)
        {
            try
            {
                db.ContentPackages.Add(ObjContentPackage);
                return  db.SaveChanges();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }



        public int DeleteContentPackage(long ContentId, int Type)
        {
            List<ContentPackage> lstContentPackage = db.ContentPackages.Where(s => s.ContentId == ContentId && s.ContentType == Type).ToList();
            foreach (ContentPackage item in lstContentPackage)
            {
                db.ContentPackages.Remove(item);
                db.SaveChanges();
            }
            return (int)EnumCore.Result.action_true;
        }


        public long[] GetlstTickerPackage(long ContentId, int Type)
        {
            long[] lstContentPackage = db.ContentPackages.Where(s => s.ContentId == ContentId && s.ContentType == Type).Select(s=>s.PackageId).ToArray();
            return lstContentPackage;
        }

        public long[] GetlstContentPackage(long ContentId, int Type)
        {
            long[] lstContentPackage = db.ContentPackages.Where(s => s.ContentId == ContentId && s.ContentType == Type).Select(s => s.PackageId).ToArray();
            return lstContentPackage;
        }


        public List<ContentPackage> GetlstObjContentPackage(long ContentId, int Type)
        {
            List<ContentPackage> lstContentPackage = db.ContentPackages.Where(s => s.ContentId == ContentId && s.ContentType == Type).ToList();
            return lstContentPackage;
        }

        public string GetPackageName(long PackageID)
        {
            if (PackageID == 1)
                return "Free";
            if (PackageID == 2)
                return "Đêm";
            if (PackageID == 3)
                return "Đồng";
            if (PackageID == 4)
                return "Vàng";
            else
                return "Kim Cương";

        }

    }
}
