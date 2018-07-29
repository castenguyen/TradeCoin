using DataModel.DataEntity;
using DataModel.DataViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using DataModel.Extension;

namespace Alluneecms.Controllers
{
    public class OrderController : CoreFronEndController
    {
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult BuyItem(string urlReturn, int quantity, int id)
        {
            Product product = cms_db.GetObjProductById(id);
            if (string.IsNullOrEmpty(urlReturn) || product == null)
            {
                return RedirectToRoute("Index", "Home");
            }
            Dictionary<long, ItemCartViewModel> lstP = new Dictionary<long, ItemCartViewModel>();
            if (Session["cart"] == null)
            {
                lstP.Add(product.ProductId, new ItemCartViewModel() { Product = product, Quantity = quantity });
            }
            else
            {
                lstP = Session["cart"] as Dictionary<long, ItemCartViewModel>;
                if (lstP.Any(x => x.Key == product.ProductId))
                {
                    if (quantity == 0)
                        lstP.Remove(product.ProductId);
                    else
                        lstP[product.ProductId].Quantity = quantity;
                }
                else if (quantity > 0)
                    lstP.Add(product.ProductId, new ItemCartViewModel() { Product = product, Quantity = quantity });

            }
            Session["cart"] = lstP;
            return Redirect(urlReturn);
        }

        public ActionResult DeleteItemCartAjax(long id,string url)
        {
            if (Session["cart"] != null)
            {
                Dictionary<long,ItemCartViewModel> cart = Session["cart"] as Dictionary<long, ItemCartViewModel>;
                if (cart.Any(x => x.Key == id))
                {
                    cart.Remove(id);
                    Session["cart"] = cart;
                    return Json(new { valid = true, count = cart.Count, reload = ((url == Url.Action("Cart", "Order") || url == Url.Action("CheckOut", "Order")) ? true:false) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { valid = false}, JsonRequestBehavior.AllowGet);
                }
            }
            else {
                return Json(new { valid = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> CheckOut()
        {
            if (Session["cart"] == null || (Session["cart"] as Dictionary<long, ItemCartViewModel>).Count < 0)
                return RedirectToAction("Index", "Home");
            CheckoutViewModel model = new CheckoutViewModel();
            long idUser = long.Parse(User.Identity.GetUserId());
            User user = await cms_db.GetObjUserById(idUser);
            model.FullName = user.FullName;
            model.UserAdress = user.UserAdress;
            model.PhoneNumber = user.PhoneNumber;
            model.EMail = user.EMail;
            List<SelectListItem> lstItemCity = new List<SelectListItem>();
            lstItemCity.Add(new SelectListItem() { Text = "Vui lòng chọn Tỉnh/Thành phố", Value = "", Selected = true });


            List<SelectListItem> lstItemDistrict = new List<SelectListItem>();
            lstItemDistrict.Add(new SelectListItem() { Text = "Vui lòng chọn Quận/Huyện", Value = "", Selected = true });


            List<SelectListItem> lstItemWard = new List<SelectListItem>();
            lstItemWard.Add(new SelectListItem() { Text = "Vui lòng chọn Phường/Xã", Value = "", Selected = true });


        
            model.ListItemCity = lstItemCity;
            model.ListItemDistrict = lstItemDistrict;
            model.ListItemWard = lstItemWard;
            return View(model);
        }
        
        public ActionResult GetCombobox(int id)
        {
            List<object> lstObj = new List<object>();
       
            return Json(lstObj, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckOut(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Session["cart"] == null)
                    return RedirectToAction("Index","Home");
                Order order = new Order();
                order.CustomerId = long.Parse(User.Identity.GetUserId());
                order.StatusId = (int)EnumCore.StatusOrder.don_hang_duoc_ghi_nhan;
                order.StatusName = "Đơn hàng được ghi nhận";
                double totalPrice = (Session["cart"] as Dictionary<long, ItemCartViewModel>).Sum(x => x.Value.TotalPrice);
                order.Cost = totalPrice;
                order.NameCustomerBuy = model.FullName;
                order.AddressCustomer = model.UserAdress;
                order.PostCode = model.PostalCode;
                order.IdProvince = model.IdProvice;
                order.IdDistrict = model.IdDistrict;
                order.IdWard = model.IdWard;
                order.PhoneCustomer = model.PhoneNumber;
                order.EmailCustomer = model.EMail;
                order.ShippingMethodId = model.IdShippingMethod;
                order.PaymentMethodId = model.IdPaymentMethod;
                order.PaIdDT = DateTime.Now;
             
                DateTime codeGeneric = DateTime.Now;
                order.OrderCode = codeGeneric.ToString("dd MM yy hh mm ss t").Replace(" ", "") + "" + codeGeneric.Ticks.ToString().Substring(codeGeneric.Ticks.ToString().Length-6,6);
                order = await cms_db.AddOrder(order);
                List<OrderProduct> lstP = new List<OrderProduct>();
                foreach (ItemCartViewModel item in (Session["cart"] as Dictionary<long, ItemCartViewModel>).Values)
                {
                    OrderProduct orderProduct = new OrderProduct();
                    orderProduct.Unitprice = item.Product.NewPrice > 0 ? item.Product.NewPrice : item.Product.OldPrice;
                    orderProduct.Quantity = item.Quantity;
                    orderProduct.ProductId = item.Product.ProductId;
                    orderProduct.OrderId = order.Id;
                    lstP.Add(orderProduct);
                }
                IEnumerable<OrderProduct> IlstProduct = cms_db.AddOrderProduct(lstP);
                if (IlstProduct == null)
                    return View(model);
                else
                {
                    foreach (var item in IlstProduct)
                    {
                        Product p = cms_db.GetObjProductById(item.ProductId);
                        p.BuyCount = p.BuyCount == null ? 1: p.BuyCount+1;
                        var success = await cms_db.UpdateProduct(p);
                    }
                    Session["cart"] = null;
                    return RedirectToAction("Success", new { id = order.Id });
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Success(int id)
        {
            ViewData["IdOrder"] = id;
            return View();
        }

        [Authorize]
        public ActionResult DonHang(long id)
        {
            InformationOrderViewModel model = new InformationOrderViewModel();
            model.Order = cms_db.GetObjOrderById(id);
            model.OrderProduct = cms_db.GetlstOrderProductsByIdOrder(id);
            if (model.Order == null || model.OrderProduct == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        public ActionResult Cart()
        {
            return View();
        }

        [Authorize]
        public ActionResult LisOrderCustomer()
        {
            long idUser = long.Parse(User.Identity.GetUserId());
            List<Order> lstOrder = cms_db.GetlstOrderOfCustomer(idUser).OrderByDescending(x => x.Id).ToList();
            return View(lstOrder);
        }
    }
}