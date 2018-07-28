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
        public bool CheckUserExistInInvite(CountLike count)
        {
            var check = db.CountLikes.Where(x => x.TypeId == (int)EnumCore.Classification.moi_user_xem_gh && x.IdUser == count.IdUser && x.IdMicrosite == count.IdMicrosite).Count();
            return check > 0;
        }

        public int AddUserInvite(CountLike count)
        {
            var check = db.CountLikes.SingleOrDefault(x => x.TypeId == (int)EnumCore.Classification.moi_user_xem_gh && x.IdUser == count.IdUser && x.IdMicrosite == count.IdMicrosite);
            if (check != null)
            {
                return 1;
            }
            else
            {
                db.CountLikes.Add(count);
                return db.SaveChanges();
            }
        }
    }
}
