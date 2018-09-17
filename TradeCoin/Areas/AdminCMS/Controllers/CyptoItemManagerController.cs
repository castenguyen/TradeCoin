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
    public class CyptoItemManagerController : CoreBackEnd
    {


        [AdminAuthorize(Roles = "devuser,AdminUser")]
        public ActionResult Index(int? page, string letter, int? status)
        {
            int pageNum = (page ?? 1);
            IndexCyptoManager model = new IndexCyptoManager();
            IQueryable<CyptoItem> tmp = cms_db.GetlstCyptoItem();
            if (!String.IsNullOrEmpty(letter))
            {
                letter = letter.ToLower();
                tmp = tmp.Where(c => c.name.StartsWith(letter) || c.name.StartsWith(letter));
                model.letter = letter;
            }
            if (status.HasValue)
            {
                if (status.Value == 0)
                {
                    tmp = tmp.Where(s => s.is_active == false);
                }
                else
                {
                    tmp = tmp.Where(s => s.is_active == true);
                }

                model.status = status.Value;
            }



            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.page = pageNum;
            model.lstMainCypto = tmp.OrderBy(c => c.name).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            return View(model);

        }


        [AdminAuthorize(Roles = "devuser,AdminUser")]
        public ActionResult EditCypto(long id)
        {
            CyptoItemViewModel model = new CyptoItemViewModel();
            model._MainObj = cms_db.GetObjCyptoItem(id);
            return View(model);
        }

        [AdminAuthorize(Roles = "devuser,AdminUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditCypto(CyptoItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                CyptoItem ObjCyptoItem = cms_db.GetObjCyptoItem(model.id);
                ObjCyptoItem.is_active = model.is_active;
                ObjCyptoItem.allow_update = model.allow_update;

                int rs = await cms_db.UpdateCypto(ObjCyptoItem);
                return RedirectToAction("EditCypto", new { id = model.id});
            }
            return RedirectToAction("Index");
        }


        [AdminAuthorize(Roles = "devuser,AdminUser")]
        public ActionResult IndexCyptoPrice(int? page, string letter)
        {
            int pageNum = (page ?? 1);
            IndexCyptoPriceManager model = new IndexCyptoPriceManager();
            IQueryable<CyptoItemPrice> tmp = cms_db.GetlstCyptoItemPrice();
            if (!String.IsNullOrEmpty(letter))
            {
                letter = letter.ToLower();
                tmp = tmp.Where(c => c.name.StartsWith(letter) || c.name.StartsWith(letter));
                model.letter = letter;
            }
        
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.page = pageNum;
            model.lstMainCypto = tmp.OrderBy(c => c.name).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            return View(model);

        }


    }
}