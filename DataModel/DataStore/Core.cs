using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
namespace DataModel.DataStore
{
    public class Core
    {
        protected alluneedbEntities db = new alluneedbEntities();
        public int AddToExceptionLog(string Action, string Controller, string Exception)
        {
            try
            {
                if (String.IsNullOrEmpty(Action))
                {
                    throw new Exception("Input action name");
                }
                if (String.IsNullOrEmpty(Controller))
                {
                    throw new Exception("Input controller name");
                }
                if (String.IsNullOrEmpty(Exception))
                {
                    throw new Exception("Input exception");
                }
                ExceptionLog model = new ExceptionLog();
                model.Controller = Controller;
                model.Action = Action;
                model.ErrorMessage = Exception;
                model.CrtdDT = DateTime.Now;
                db.ExceptionLogs.Add(model);
                db.SaveChanges();
            }
            catch
            {
                return 0;
            }
            return 1;
        }

        public int AddToExceptionLog(string Action, string Controller, string Exception, long? UserId = null)
        {
            try
            {
                if (String.IsNullOrEmpty(Action))
                {
                    throw new Exception("Input action name");
                }
                if (String.IsNullOrEmpty(Controller))
                {
                    throw new Exception("Input controller name");
                }
                if (String.IsNullOrEmpty(Exception))
                {
                    throw new Exception("Input exception");
                }
                ExceptionLog model = new ExceptionLog();
                model.Controller = Controller;
                model.Action = Action;
                model.ErrorMessage = Exception;
                model.CrtdDT = DateTime.Now;
                model.UserId = UserId;
                db.ExceptionLogs.Add(model);
                db.SaveChanges();

            }
            catch
            {
                return 0;
            }
            return 1;
        }
        public int AddToExceptionLog(string Exception)
        {
            try
            {
                string action = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");
                string controller = System.Web.HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");

                if (String.IsNullOrEmpty(action))
                {
                    throw new Exception("Input action name");
                }
                if (String.IsNullOrEmpty(controller))
                {
                    throw new Exception("Input controller name");
                }
                if (String.IsNullOrEmpty(Exception))
                {
                    throw new Exception("Input exception");
                }

                ExceptionLog model = new ExceptionLog();
                model.Controller = controller;
                model.Action = action;
                model.ErrorMessage = Exception;
                model.CrtdDT = DateTime.Now;
                db.ExceptionLogs.Add(model);
                db.SaveChanges();
            }
            catch
            {
                return 0;
            }
            return 1;
        }

    }
}
