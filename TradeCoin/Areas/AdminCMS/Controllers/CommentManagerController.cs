using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;



namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
     //[AdminAuthorize]
    public class CommentManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser")]
        public ActionResult Index(int? page, int? filterstate)
        {
            int pageNum = (page ?? 1);
            ContentCommentIndexViewModels model = new ContentCommentIndexViewModels();
            IQueryable<ContentComment> tmp = cms_db.GetlstComment();
            if (filterstate.HasValue && filterstate.Value != 0)
            {
                tmp = tmp.Where(s => s.StateId == filterstate);
                model.CommentState = filterstate.Value;
            }
           
            model.pageNum = pageNum;
            model.lstMainComment = tmp.OrderBy(c => c.CommentId).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.lstCommentState = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.state_type);
            //List<ViewModelComment> tmpmodel = new List<ViewModelComment>();
            //tmpmodel = cms_db.GetlstModelObjCommentFromStore(1003, null);
            return View(model);
        }
         [AdminAuthorize(Roles = "supperadmin,devuser,ApproveComment")]
        public async Task<ActionResult> ChangeState(int id, int state, int? page, int? filterstate)
        {
                int i = await cms_db.UpdateStateComment(id, state, DateTime.Now, User.Identity.GetUserName(), long.Parse(User.Identity.GetUserId()));
             return RedirectToAction("Index", new { page = page, filterstate = filterstate });
        }

	}
}