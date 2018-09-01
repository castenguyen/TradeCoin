using CMSPROJECT.Areas.AdminCMS.Core;
using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.SignalR;
using CMSPROJECT.Hubs;

namespace CMSPROJECT.Areas.AdminCMS.Controllers
{

    public class TickerManagerController : CoreBackEnd
    {
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,CreateTicker")]
        public ActionResult Index(int? page, int? TickerStatus, int? TickerPackage,  string FillterTickerName)
        {
            int pageNum = (page ?? 1);
            TickerAdminViewModel model = new TickerAdminViewModel();
            IQueryable<Ticker> tmp = cms_db.GetlstTicker().Where(s => s.StateId != (int)EnumCore.TickerStatusType.da_xoa);
            if (TickerStatus.HasValue)
            {
                tmp = tmp.Where(s => s.StateId == TickerStatus);
                model.TickerStatus = TickerStatus.Value;
            }

            if (TickerPackage.HasValue && TickerPackage.Value != 0)
            {
                foreach (Ticker _val in tmp)
                {
                  
                    List<ContentPackage> lstpackageofticker= cms_db.GetlstObjContentPackage(_val.TickerId, (int)EnumCore.ObjTypeId.ticker);
                    if (!lstpackageofticker.Select(s => s.PackageId).Contains(TickerPackage.Value))
                    {
                        tmp = tmp.Where(s=>s.TickerId!= _val.TickerId);
                    }
                  

                }

                model.TickerPackage = TickerPackage.Value;
            }

            if (!String.IsNullOrEmpty(FillterTickerName))
            {
                tmp = tmp.Where(s => s.TickerName.ToLower().Contains(FillterTickerName.ToLower()));
                model.FillterTickerName = FillterTickerName;
            }
            if (tmp.Count() < (int)EnumCore.BackendConst.page_size)
                pageNum = 1;
            model.pageNum = pageNum;
            List<TickerViewModel> prelistmain = new List<TickerViewModel>();
            foreach (Ticker _val in tmp)
            {
                TickerViewModel abc = new TickerViewModel(_val);
                abc.lstTickerContentPackage = cms_db.GetlstObjContentPackage(_val.TickerId, (int)EnumCore.ObjTypeId.ticker);
                prelistmain.Add(abc);

            }
            model.lstMainTickerViewModel = prelistmain.ToPagedList(pageNum, (int)EnumCore.BackendConst.page_size);

            model.lstTickerStatus = new SelectList(cms_db.Getclasscatagory((int)EnumCore.ClassificationScheme.status_ticker), "value", "text");
            model.lstPackage = new SelectList(cms_db.GetObjSelectListPackage(), "value", "text"); 
            return View(model);
        }

        public ActionResult ListNotificationNewKeo()
        {
            try
            {
                List<Package> lstPackageOfUser = Session["ListPackageOfUser"] as List<Package>;
                List<TickerViewModel> lstmpTickers = new List<TickerViewModel>();

                List<Ticker> lstTicker = new List<Ticker>();
                if (User.IsInRole("AdminUser") || User.IsInRole("devuser") || User.IsInRole("supperadmin") || User.IsInRole("Mod"))
                {
                    lstTicker = cms_db.GetListTickerByUser(long.Parse(User.Identity.GetUserId()),
                                        (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, long.Parse("5"));
                }
                else
                {
                    lstTicker = cms_db.GetListTickerByUser(long.Parse(User.Identity.GetUserId()),
                            (int)ConstFrontEnd.FontEndConstNumberRecord.Nbr_Ticker_In_Home, lstPackageOfUser[0].PackageId);

                }

                foreach (Ticker _val in lstTicker)
                {
                    TickerViewModel tmp = new TickerViewModel(_val);
                    tmp.lstTickerContentPackage = cms_db.GetlstObjContentPackage(tmp.TickerId, (int)EnumCore.ObjTypeId.ticker);
                    lstmpTickers.Add(tmp);
                }
                var result = new
                {
                    TotalRows = lstmpTickers.Count(),
                    Rows = lstmpTickers.Select(x => new
                    {
                        tmp = x.tmp,
                        TickerId = x.TickerId,
                        CrtdUserName = x.CrtdUserName,
                        TickerName = x.TickerName,
                    })
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,CreateTicker")]
        public ActionResult Create()
        {
            TickerViewModel model = new TickerViewModel();
            model.lstPackage = cms_db.GetObjSelectListPackage();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,CreateTicker")]
        public async Task<ActionResult> Create(TickerViewModel model, HttpPostedFileBase Default_files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Ticker MainModel = model._MainObj;
                    MainModel.CrtdDT = DateTime.Now;
                    MainModel.CrtdUserId = long.Parse(User.Identity.GetUserId());
                    MainModel.CrtdUserName = User.Identity.Name;
                    MainModel.StateId = (int)EnumCore.TickerStatusType.dang_chay;
                    MainModel.StateName = "Đang chạy";
                    MainModel.Flag = model.Flag;
                    int rs = await cms_db.CreateTickerAsync(MainModel);
                    if (Default_files != null)
                    {
                        MediaContentViewModels rsdf = await this.SaveDefaultImageForTicker(Default_files, MainModel.TickerId);
                        int rsup = await this.UpdateImageUrlForTicker(rsdf, MainModel);
                    }
                    int SaveTickerPackage = this.SaveTickerPackage(model.lstTickerPackage, MainModel);
                    int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                        (int)EnumCore.ActionType.Create, "Create", MainModel.TickerId, MainModel.TickerName, "TickerManager", (int)EnumCore.ObjTypeId.ticker);

                    var context = GlobalHost.ConnectionManager.GetHubContext<NotifiHub>();
                    context.Clients.All.notificationNewKeo();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Create", "TickerManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
          }

        [AdminAuthorize(Roles = "supperadmin,devuser,UpdateTicker")]
        public ActionResult Update(int? id)
        {
            if (id == null)
                id = 1;
            Ticker _obj = cms_db.GetObjTicker(id.Value);
            TickerViewModel model = new TickerViewModel(_obj);
            if (  (model.CrtdUserId == long.Parse(User.Identity.GetUserId()) && User.IsInRole("Mod")) || User.IsInRole("AdminUser") || User.IsInRole("devuser"))
            {
                model.lstPackage = cms_db.GetObjSelectListPackage();
                model.lstTickerPackage = cms_db.GetlstTickerPackage(model.TickerId, (int)EnumCore.ObjTypeId.ticker);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,UpdateTicker")]
        public async Task<ActionResult> Update(TickerViewModel model, HttpPostedFileBase Default_files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if ((model.CrtdUserId == long.Parse(User.Identity.GetUserId()) && User.IsInRole("Mod")) || User.IsInRole("AdminUser") || User.IsInRole("devuser"))
                    {
                        Ticker MainModel = model._MainObj;
                        MainModel.StateId = (int)EnumCore.TickerStatusType.dang_chay;
                        MainModel.StateName = "Enable";
                        MainModel.Flag = model.Flag;
                        if (model.Flag.HasValue)
                        {
                            if (model.Flag.Value == 1)
                            {
                                MainModel.Profit = this.SumTicker(model.Flag.Value, MainModel.BuyZone1.Value, MainModel.SellZone1.Value, MainModel.BTCInput.Value);
                                MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                                MainModel.StateName = "Lời";
                            }
                            else if (model.Flag.Value == 2)
                            {
                                MainModel.Profit = this.SumTicker(model.Flag.Value, MainModel.BuyZone1.Value, MainModel.SellZone2.Value, MainModel.BTCInput.Value);
                                MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                                MainModel.StateName = "Lời";
                            }
                            else if (model.Flag.Value == 3)
                            {
                                MainModel.Profit = this.SumTicker(model.Flag.Value, MainModel.BuyZone1.Value, MainModel.SellZone3.Value, MainModel.BTCInput.Value);
                                MainModel.StateId = (int)EnumCore.TickerStatusType.loi;
                                MainModel.StateName = "Lời";
                            }
                            else if (model.Flag.Value == 4)
                            {
                                MainModel.Deficit = this.SumTicker(model.Flag.Value, MainModel.BuyZone1.Value, MainModel.DeficitControl.Value, MainModel.BTCInput.Value);
                                MainModel.StateId = (int)EnumCore.TickerStatusType.lo;
                                MainModel.StateName = "Lỗ";
                            }
                            else if (model.Flag.Value == 0)
                            {

                                MainModel.Profit = 0;
                                MainModel.Deficit = 0;
                                MainModel.StateId = (int)EnumCore.TickerStatusType.dang_chay;
                                MainModel.StateName = "Đang chạy";
                            }

                        }
                        int rs = await cms_db.UpdateTicker(MainModel);
                        if (Default_files != null)
                        {
                            MediaContentViewModels rsdf = await this.SaveDefaultImageForTicker(Default_files, MainModel.TickerId);
                            int rsup = await this.UpdateImageUrlForTicker(rsdf, MainModel);
                        }
                        int SaveTickerPackage = this.SaveTickerPackage(model.lstTickerPackage, MainModel);
                        int rs2 = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                            (int)EnumCore.ActionType.Update, "Update", MainModel.TickerId, MainModel.TickerName, "TickerManager", (int)EnumCore.ObjTypeId.ticker);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Create", "TickerManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// tinh toan so lieu cho kèo sau khi cập nhật
        /// numbuy là giá mua
        /// input là giá bán có thể là lỗ hoặc lời
        /// </summary>
        /// <param name="FlagZone"></param>
        /// <param name="numbuy"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private double SumTicker(int FlagZone, double numbuy ,double input,double btc)
        {
            double result = 0;
            if (FlagZone == 1 || FlagZone == 2 || FlagZone == 3)
            {
                result = input / numbuy * 100 * btc;
            }
            else if (FlagZone == 4)
            {
                result = input / numbuy * 100 * btc;
            }
            return result;
        }


        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,UpdateTicker")]
        public async Task<ActionResult> ChangeState(long id, int state)
        {
            try
            {
                Ticker MainModel = cms_db.GetObjTicker(id);
                MainModel.AprvdUID = long.Parse(User.Identity.GetUserId());
                MainModel.AprvdDT = DateTime.Now;
                MainModel.StateId = state;

                if (MainModel.StateId == (int)EnumCore.TickerStatusType.lo)
                    MainModel.StateName = "Lỗ";
                if (MainModel.StateId == (int)EnumCore.TickerStatusType.loi)
                    MainModel.StateName ="Lời";
                await cms_db.UpdateTicker(MainModel);
                int ach = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                 (int)EnumCore.ActionType.Update, "ChangeState", MainModel.TickerId, MainModel.TickerName, "Ticker", (int)EnumCore.ObjTypeId.ticker);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("ChangeState", "TickerManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return RedirectToAction("Index");
            }
        }
        [AdminAuthorize(Roles = "supperadmin,devuser,AdminUser,DeleteTicker")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                Ticker MainModel = cms_db.GetObjTicker(id);
                if (MainModel != null)
                {
                    MediaContent _objoldmedia = cms_db.GetObjDefaultMediaByContentIdvsType(MainModel.TickerId, (int)EnumCore.ObjTypeId.ticker);
                    if (_objoldmedia != null)
                        await cms_db.DeleteMediaContent(_objoldmedia.MediaContentId);
                    int dl = cms_db.DeleteContentPackage(id, (int)EnumCore.ObjTypeId.ticker);
                    int result = await cms_db.DeleteTicker(MainModel);
                    int ach = await cms_db.CreateUserHistory(long.Parse(User.Identity.GetUserId()), Request.ServerVariables["REMOTE_ADDR"],
                                         (int)EnumCore.ActionType.Delete, "Delete", MainModel.TickerId, MainModel.TickerName, "Ticker", (int)EnumCore.ObjTypeId.ticker);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Delete", "TickerManager", e.ToString());
                return RedirectToAction("Index");
            }
        }

        private async Task<MediaContentViewModels> SaveDefaultImageForTicker(HttpPostedFileBase file, long TickerId)
        {
            MediaContent CurrentMediaId = cms_db.GetObjMedia().Where(s => s.ObjTypeId == (int)EnumCore.ObjTypeId.ticker
                    && s.MediaTypeId == (int)EnumCore.mediatype.hinh_anh_dai_dien && s.ContentObjId == TickerId).FirstOrDefault();
            if (CurrentMediaId != null)
            {
                int rs = await cms_db.DeleteMediaContent(CurrentMediaId.MediaContentId);
            }
            ImageUploadViewModel item = new ImageUploadViewModel();
            item = cms_db.UploadHttpPostedFileBase(file);
            MediaContentViewModels _Media = new MediaContentViewModels();
            _Media.Filename = item.ImageName;
            _Media.FullURL = item.ImageUrl;
            _Media.ContentObjId = TickerId;
            _Media.ObjTypeId = (int)EnumCore.ObjTypeId.ticker;
            _Media.ViewCount = 0;
            _Media.MediaTypeId = (int)EnumCore.mediatype.hinh_anh_dai_dien;
            _Media.CrtdDT = DateTime.UtcNow;
            _Media.MediaContentSize = file.ContentLength;
            _Media.ThumbURL = item.ImageThumbUrl;
            _Media.CrtdUID = long.Parse(User.Identity.GetUserId());
            await cms_db.AddNewMediaContent(_Media);
            return _Media;

        }
        private async Task<int> UpdateImageUrlForTicker(MediaContentViewModels ImageObj, Ticker TickerObj)
        {
            TickerObj.MediaUrl = ImageObj.FullURL;
            TickerObj.MediaThumb = ImageObj.ThumbURL;
            return await cms_db.UpdateTicker(TickerObj);
        }

        private int SaveTickerPackage(long[] model, Ticker Ticker)
        {
            try
            {
                int dl = cms_db.DeleteContentPackage(Ticker.TickerId, (int)EnumCore.ObjTypeId.ticker);
                foreach (int _val in model)
                {
                    ContentPackage tmp = new ContentPackage();
                    tmp.ContentId = Ticker.TickerId;
                    tmp.ContentName = Ticker.TickerName;
                    tmp.ContentType = (int)EnumCore.ObjTypeId.ticker;
                    tmp.PackageId = _val;
                    tmp.PackageName = cms_db.GetPackageName(_val);

                    cms_db.CreateContentPackage(tmp);
                }
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("SaveTickerPackage", "TickerManager", e.ToString(), long.Parse(User.Identity.GetUserId()));
                return (int)EnumCore.Result.action_false;
            }
        }

       
    }
}