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


namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public IQueryable<ExceptionLog> GetlstExceptionLog()
        {
            return  db.ExceptionLogs;
        }

        public int DeleteExceptionLog(int id)
        {
            var obj = db.ExceptionLogs.SingleOrDefault(x=>x.Id == id);
            if (obj != null)
            {
                db.ExceptionLogs.Remove(obj);
                var code = db.SaveChanges();
                return code;
            }
            else
                return 0;
        }

    }
}
