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
using System.Net;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{

    public class ShopCartManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,ApproveOrder")]
        public ActionResult Index(int? page, int? state)
        {

            int pageNum = (page ?? 1);
            OrderIndexViewModel model = new OrderIndexViewModel();
            IQueryable<Order> tmp = cms_db.GetlstOrder().Where(s => s.StatusId != (int)EnumCore.StateType.da_xoa);
            if (state.HasValue && state.Value != 0)
            {
                tmp = tmp.Where(s => s.StatusId == state);
                model.ContentState = state.Value;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            model.lstMainOrder = tmp.OrderByDescending(c => c.Id).ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);
            model.lstState = cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.trang_thai_don_hang).ToList();
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,ApproveOrder")]
        public ActionResult OrderDetail(long? id)
        {
            OrderDetailViewModel model = new OrderDetailViewModel();
            model.MainObjOrder = cms_db.GetObjOrderById(id.Value);
            model.lstProduct = cms_db.GetlstOrderProductExt(id.Value);
            model.lstStatus = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.trang_thai_don_hang), "value", "text");
            model.Status = model.MainObjOrder.StatusId.Value;
            return View(model);
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,ApproveOrder")]
        [HttpPost]
        public async Task<ActionResult> OrderDetail(OrderDetailViewModel model)
        {
            Order _MainObj = new Order();
            _MainObj = cms_db.GetObjOrderById(model.MainObjOrder.Id);
            _MainObj.StatusId = model.Status;
            _MainObj.StatusName = cms_db.GetsttringClassName(model.Status);
            _MainObj.PlaceDT = model.MainObjOrder.PlaceDT;
            await cms_db.UpdateObjOrder(_MainObj);
            return RedirectToAction("Index");
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,ApproveOrder")]
        public async Task<ActionResult> DeleteOrder(long? id)
        {
            int rs = await cms_db.DeleteObjOrder(id.Value);
            return RedirectToAction("Index");
        }

        //private bool CheckLink(string url)
        //{
        //    bool pageExists = false;
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = WebRequestMethods.Http.Head;
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        pageExists = response.StatusCode == HttpStatusCode.OK;
        //    }
        //    catch (Exception e)
        //    {
        //        //Do what ever you want when its no working...
        //        //Response.Write( e.ToString());
        //    }
        //    return pageExists;
        //}
        //public void Kiemtralink()
        //{
        //    var de = new alluneedbEntities();
        //    var product = de.Products.ToList();
        //    foreach (var item in product)
        //    {
        //        var idproduct = de.Products.Single(p => p.ProductId == item.ProductId);
        //        var kt = CheckLink("http://localhost:50656/" + idproduct.MediaThumb);
        //        if (kt == false)
        //        {
        //            de.Products.Remove(idproduct);
        //            de.SaveChanges();
        //        }
        //        Response.Write("http://localhost:50656/" + idproduct.MediaThumb + "<br/>");
        //    }
        //}

    }
}