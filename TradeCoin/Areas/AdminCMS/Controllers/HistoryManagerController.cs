using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using DataModel.DataViewModel;
using DataModel.DataStore;
using CMSPROJECT.Areas.AdminCMS.Core;
using System;
using System.Collections.Generic;
using DataModel.Extension;
using DataModel.DataEntity;
using PagedList;
using System.Linq;
//using MongoData.Models;
namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class HistoryManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "devuser")]
        public ActionResult Index(int? page)
        {
            return View(cms_db.GetlstExceptionLog().OrderByDescending(x=>x.CrtdDT).ToPagedList(page ?? 1, 30));
        }

        [AdminAuthorize(Roles = "devuser")]
        public ActionResult Delete(int id)
        {
            int success = cms_db.DeleteExceptionLog(id);
            return RedirectToAction("Index");
        }

        [AdminAuthorize(Roles = "devuser")]
        public ActionResult HistoryAction(int? page,int ? ActionTypeid, int? ObjectTypeid , string Datetime)
        {
            int pageNum = (page ?? 1);
            IndexHistoryAction model = new IndexHistoryAction();
            IQueryable<Userhist> tmp = cms_db.GetlstActionHistory();

            if (ActionTypeid.HasValue)
            {
                tmp = tmp.Where(s => s.ActionTypeId == ActionTypeid.Value);
                model.ActionTypeid = ActionTypeid.Value;
            }
            if (ObjectTypeid.HasValue)
            {
                tmp = tmp.Where(s => s.ObjTypeId == ObjectTypeid.Value);
                model.ObjectTypeid = ObjectTypeid.Value;
            }

            if (!String.IsNullOrEmpty(Datetime))
            {
                model.Datetime = Datetime;
                model.StartDT = this.SpritDateTime(model.Datetime)[0];
                model.EndDT = this.SpritDateTime(model.Datetime)[1];
                tmp = tmp.Where(s => s.CrtdDT > model.StartDT && s.CrtdDT < model.EndDT);
            }

            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.page = pageNum;

            model.lstObjectType = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.ObjectType), "value", "text");
            model.lstActionType = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.ActionType), "value", "text");
            model.lstMainUserHist = tmp.OrderByDescending(c => c.Id).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            return View(model);

        }
        private DateTime[] SpritDateTime(string datetime)
        {
            string[] words = datetime.Split('-');
            DateTime[] model = new DateTime[] { Convert.ToDateTime(words[0]), Convert.ToDateTime(words[1]) };
            return model;
        }
    }
}