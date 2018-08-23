using DataModel.DataEntity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Collections;
using DataModel.DataViewModel;
using DataModel.Extension;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Net;


namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {

        public async Task<User> GetObjUserById(long id)
        {
            try
            {
                User model = new User();
                model = await db.Users.FindAsync(id);
                if (model != null)
                    return model;
                return null;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetObjUserById", "BackEndCode", ex.ToString());
                return null;
            }
        }
        public User GetObjUserByEmail(string Email)
        {
            try
            {
                User model = new User();
                model = db.Users.Where(s => s.EMail == Email).FirstOrDefault();
                if (model != null)
                    return model;
                return null;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetObjUserById", "BackEndCode", ex.ToString());
                return null;
            }
        }

        public User GetObjUserByLogin(string login)
        {
            try
            {
                User model = new User();
                model = db.Users.Where(s => s.Login == login).FirstOrDefault();
                if (model != null)
                    return model;
                return null;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetObjUserById", "BackEndCode", ex.ToString());
                return null;
            }
        }

        public bool CheckAnyUsername( string userName)
        {

            if (db.Users.Any(s => s.Login == userName))
                return true;
            return false;
        }


        public User GetObjUserByIdNoAsync(long id)
        {
            try
            {
                User model = new User();
                model = db.Users.Single(s => s.Id == id);
                if (model != null)
                    return model;
                return null;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetObjUserByIdNoAsync", "BackEndCode", ex.ToString());
                return null;
            }
        }
        public async Task<int> UpdateUser(User _user)
        {
            try
            {
                db.Entry(_user).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function UpdateUser", "BackEndCode", ex.ToString());
                return 0;
            }
        }
        public async Task<int> AddRole(Role _Role)
        {
            try
            {
                db.Roles.Add(_Role);
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function AddRole", "BackEndCode", ex.ToString());
                return 0;
            }
        }
        public IQueryable<Role> GetlstRole()
        {
            var lstRole = db.Roles;
            return lstRole;
        }


        public IQueryable<User> GetlstUser()
        {
            var lstUser = db.Users;
            return lstUser;
        }
        public IEnumerable GetRoleList()//lấy list role
        {
                List<Role> lstobjRole = this.GetlstRole().ToList();
                return lstobjRole;
        }

        public List<Role> GetRoleList2()//lấy list role
        {
            List<Role> lstobjRole = this.GetlstRole().ToList();
            return lstobjRole;
        }
        public IEnumerable GetUserList()//lấy list User
        {
            try
            {
                List<User> lstobjUser = this.GetlstUser().ToList();
                return lstobjUser;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetUserList", "BackEndCode", ex.ToString());
                return null;
            }
        }

        public List<SelectListObj> GetUserListNotIsRoleId(int RoleId, int PageNum, int RecordNum)//lấy list User mà không phai la user thuộc role deuser
        {
            try
            {
                List<SelectListObj> lstobjUser = this.db.Database.SqlQuery<SelectListObj>("exec GetLstUserNotIsRole @RoleID,@PageNum,@RecorNum", 
                            new SqlParameter("@RoleID", RoleId), new SqlParameter("@PageNum", PageNum), new SqlParameter("@RecorNum", RecordNum)).ToList();
                return lstobjUser;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetUserList", "BackEndCode", ex.ToString());
                return null;
            }
        }

        public IQueryable<User> GetUsersNotInRoleByLinkq(string roleName)
        {
            Role data = db.Roles.Where(s => s.Name == roleName).FirstOrDefault();
            List<long> lstUinRole = data.Users.Select(s=>s.Id).ToList();
            IQueryable<User> model = db.Users.Where(x => !lstUinRole.Contains(x.Id));
            return model;
        }

        public IQueryable<User> GetUsersForAdminByLinkq()
        {

           
            Role data = db.Roles.Where(s => s.Name == "devuser").FirstOrDefault();
            Role data2 = db.Roles.Where(s => s.Name == "supperadmin").FirstOrDefault();
            Role data3 = db.Roles.Where(s => s.Name == "AdminUser").FirstOrDefault();

            List<long> lstUinRole = data.Users.Select(s => s.Id).ToList();
            List<long> lstUinRole2 = data2.Users.Select(s => s.Id).ToList();
            List<long> lstUinRole3 = data3.Users.Select(s => s.Id).ToList();

            IQueryable<User> model = db.Users.Where(x => !lstUinRole.Contains(x.Id));
            model= model.Where(x => !lstUinRole2.Contains(x.Id));
            model = model.Where(x => !lstUinRole3.Contains(x.Id));

            return model;
        }


        public IQueryable<User> GetUsersInRoleByLinkq(string roleName)
        {
            Role data = db.Roles.Where(s => s.Name == roleName).FirstOrDefault();
            List<long> lstUinRole = data.Users.Select(s => s.Id).ToList();
            IQueryable<User> model = db.Users.Where(x => lstUinRole.Contains(x.Id));
            return model;
        }
      
      

        public List<Role> GetRoleListReturnList()
        {
            try
            {
                List<Role> lstobjRole = this.GetlstRole().ToList();
                return lstobjRole;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetRoleListReturnList", "BackEndCode", ex.ToString());
                return null;
            }
        }
        public Role GetObjRoleById(long id)
        {
            try
            {
                Role ObjRole = db.Roles.Find(id);
                return ObjRole;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetObjRoleById", "BackEndCode", ex.ToString());
                return null;
            }
        }

        public Role GetObjRoleByName(string name)
        {
            try
            {
                Role ObjRole = db.Roles.Where(s=>s.Name==name).FirstOrDefault();
                return ObjRole;
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function GetObjRoleById", "BackEndCode", ex.ToString());
                return null;
            }
        }
        public async Task<int> UpdateRole(Role _Role)
        {
            try
            {
                db.Entry(_Role).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.AddToExceptionLog("function UpdateRole", "BackEndCode", ex.ToString());
                return 0;
            }
        }

        public async Task <float> GetMoney(string userName)
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["userName"] = userName;
                var response = client.UploadValues("http://thienrongviet.com/api/getmoney", values);
                var sodu = Task.Run(() => Encoding.Default.GetString(response)).Result;
                return float.Parse(sodu);
            }
        }




        #region ChatHub
        public bool CheckAdmin(User UserObj)
        {
            Role Obj = UserObj.LstRole.Where(s => s.Name == "AdminUser").FirstOrDefault();
            if (Obj != null)
                return true;
            return false;
        }
        public UserInfo CreateUserInfo(string UserName, string UserEmail,string ConectionID)
        {
            UserInfo userInfo = new UserInfo();
            User CurrentUser = this.GetObjUserByEmail(UserEmail);
            if (CurrentUser != null)
            {
                if (this.CheckAdmin(CurrentUser))//nếu là userdamin
                {
                    userInfo.ConnectionId = ConectionID;
                    userInfo.UserID = CurrentUser.Id;
                    userInfo.UserGroup = CurrentUser.Id.ToString();
                    userInfo.UserName = UserName;
                    userInfo.UserEmail = CurrentUser.EMail;
                    userInfo.freeflag =(int)EnumCore.FlagForChatHub.admin_free;
                    userInfo.tpflag = (int)EnumCore.FlagForChatHub.user_admin;
                    userInfo.AdminCode = (int)EnumCore.FlagForChatHub.user_admin;
                }
                else//nếu ko là user admin là user thường
                {
                    userInfo.ConnectionId = ConectionID;
                    userInfo.AdminCode = (int)EnumCore.FlagForChatHub.user_guest;
                    userInfo.UserID = CurrentUser.Id;
                    userInfo.UserName = UserName;
                    userInfo.UserEmail = CurrentUser.EMail;
                    userInfo.freeflag = (int)EnumCore.FlagForChatHub.admin_free;
                    userInfo.tpflag = (int)EnumCore.FlagForChatHub.user_guest;
                    userInfo.waitflag = (int)EnumCore.FlagForChatHub.waitting;
                }
            }
            else//nếu ko là user admin là user khách
            {
                userInfo.ConnectionId = ConectionID;
                userInfo.UserEmail = UserEmail;
                userInfo.UserName = UserName;
                userInfo.freeflag = (int)EnumCore.FlagForChatHub.admin_free;
                userInfo.tpflag = (int)EnumCore.FlagForChatHub.user_guest;
                userInfo.waitflag = (int)EnumCore.FlagForChatHub.waitting;
                userInfo.AdminCode = (int)EnumCore.FlagForChatHub.user_guest;
            }
            return userInfo;
        }

        #endregion ChatHub

    }
}
