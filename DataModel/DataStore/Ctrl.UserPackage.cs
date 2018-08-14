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
        public async Task<int> CreateUserPackageAsync(UserPackage ObjUserPackage)
        {
            try
            {
                db.UserPackages.Add(ObjUserPackage);
                return await db.SaveChangesAsync();

            }
            catch(Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }

        public int CreateUserPackage(UserPackage ObjUserPackage)
        {
            try
            {
                db.UserPackages.Add(ObjUserPackage);
                return db.SaveChanges();
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }



        //public int DeleteUserPackage(long ContentId)
        //{
        //    List<UserPackage> lstUserPackage = db.UserPackages.Where(s => s.ContentId == ContentId && s.ContentType == Type).ToList();
        //    foreach (UserPackage item in lstUserPackage)
        //    {
        //        db.UserPackages.Remove(item);
        //        db.SaveChanges();
        //    }
        //    return (int)EnumCore.Result.action_true;
        //}

    }
}
