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

    public class RoleManagerController : CoreBackEnd
    {

        [AdminAuthorize(Roles = "devuser")]
        public ActionResult Index()
        {
            RoleViewModel model = new RoleViewModel();
            model.ListRole = cms_db.GetRoleListReturnList();
            return View(model);
        }

        [AdminAuthorize(Roles = "devuser")]
        public ActionResult CreateRoleGroup(long id)
        {
            RoleViewModel model = new RoleViewModel();
            model._Role = cms_db.GetObjRoleById(id);
            model.ListRole = cms_db.GetlstRole().Where(s=>s.IsGroup==false).ToList();
            model.lstPermission = cms_db.GetlstPermission(id);
            return View(model);
        }


        [AdminAuthorize(Roles = "devuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRoleGroup(RoleViewModel model)
        {
            try
            {
                int dl = cms_db.DeleteRoleGroup(model.Id);
                foreach (int _val in model.lstPermission)
                {
                    RoleGroup tmp = new RoleGroup();
                    tmp.RoleGroupId = model.Id;
                    tmp.RoleId = _val;
                    tmp.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    tmp.CrtdUserName = User.Identity.Name;
                    tmp.CrtdDT = DateTime.Now;
                    cms_db.CreateRoleGroup(tmp);
                }
                return RedirectToAction("CreateRoleGroup", new { id = model.Id });
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("CreateRoleGroup", "RoleManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("CreateRoleGroup", new { id = model.Id });
            }
          
        }


        [AdminAuthorize(Roles = "devuser")]
        public ActionResult AddNewRole()
        {
            RoleViewModel model = new RoleViewModel();
            model.ListRole = cms_db.GetRoleListReturnList();
            return View(model);
        }
        [AdminAuthorize(Roles = "devuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role _Role = new Role();
                _Role.Name = model.Name;
                _Role.RoleDes = model.RoleDes;
                int rs = await cms_db.AddRole(_Role);
                return RedirectToAction("AddNewRole");
            }
            return RedirectToAction("Dashboard", "HomeAdmin");
        }

        [AdminAuthorize(Roles = "devuser")]
        public ActionResult EditRole(long id)
        {
            RoleViewModel model = new RoleViewModel();
            model._Role = cms_db.GetObjRoleById(id);
            model.ListRole = cms_db.GetRoleListReturnList();
            return View(model);
        }

        [AdminAuthorize(Roles = "devuser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Role _Role = cms_db.GetObjRoleById(model.Id);
                _Role.RoleDes = model.RoleDes;
                _Role.IsGroup = model.IsGroup;
                int rs = await cms_db.UpdateRole(_Role);
                return RedirectToAction("EditRole", new { id = model.Id });
            }
            return RedirectToAction("Dashboard", "HomeAdmin");
        }




      


    }
}