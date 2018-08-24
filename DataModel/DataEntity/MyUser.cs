using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataEntity
{
    public class MyUser : IdentityUser<long, MyLogin, MyUserRole, MyClaim>
    {
        #region properties


        public string FolerName { get; set; }
        public string GenderName { get; set; }
        public int GenderId { get; set; }
        public string ActivationToken { get; set; }
        public string PasswordAnswer { get; set; }
        public string PasswordQuestion { get; set; }
        public string FullName { get; set; }

        public Nullable<bool> IsLogin { get; set; }

        public Nullable<long> PackageId { get; set; }
        public string PackageName { get; set; }

        #endregion

        #region methods

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(MyUserManager userManager)
        {
            ClaimsIdentity userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #endregion
    }
}
