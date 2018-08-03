using System;
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
        public long[] GetlstPermission(long groupid)
        {
            long[] lstPermission = db.RoleGroups.Where(s => s.RoleGroupId == groupid).Select(s => s.RoleId).ToArray();
            return lstPermission;
        }


        public async Task<int> CreateRoleGroupAsync(RoleGroup ObjRoleGroup)
        {
            try
            {
                db.RoleGroups.Add(ObjRoleGroup);
                return await db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }

        public int CreateRoleGroup(RoleGroup ObjRoleGroup)
        {
            try
            {
                db.RoleGroups.Add(ObjRoleGroup);
                return db.SaveChanges();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }



        public int DeleteRoleGroup(long RoleGroupId)
        {
            List<RoleGroup> lstRoleGroup = db.RoleGroups.Where(s => s.RoleGroupId == RoleGroupId).ToList();
            foreach (RoleGroup item in lstRoleGroup)
            {
                db.RoleGroups.Remove(item);
                db.SaveChanges();
            }
            return (int)EnumCore.Result.action_true;
        }


    }
}
