using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.DataEntity;
using DataModel.DataStore;
using DataModel.DataViewModel;
using DataModel.Extension;
using System.Threading.Tasks;
namespace Alluneecms.Controllers
{
    public class NewsController : CoreFronEndController
    {

        public ActionResult Index(int? id, string CataFriendlyUrl, int? page)
        {
            try
            {
                Classification Objcata = cms_db.GetObjClasscifiById(id.Value);
                if (Objcata != null)
                {
                    this.SetInforMeta(Objcata.ClassificationMetakeyword, Objcata.ClassificationMetaDes);
                    ListNewViewModel model = new ListNewViewModel();
                    model.CataObj = Objcata;
                    model.lstNews = cms_db.GetlstContentItemByCataId(Objcata.ClassificationId, 10);
                   // model.LstProductNews = cms_db.GetListNewsProduct(5);
                    return View(model);
                }

                return RedirectToAction("ViewError", "Home");
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Index", "News", e.ToString());
                return RedirectToAction("ViewError", "Home");
            }

        }

        public ActionResult Detail(string FriendlyUrl, string CataFriendlyUrl, int? id)
        {
            try
            {
                Classification Objcata = cms_db.GetObjClasscifiByFriendlyUrl(FriendlyUrl);
                if (Objcata != null)
                {
                    this.SetInforMeta(Objcata.ClassificationMetakeyword, Objcata.ClassificationMetaDes);
                    ListNewViewModel model = new ListNewViewModel();
                    model.CataObj = Objcata;
                    model.lstNews = cms_db.GetlstContentItemByCataId(Objcata.ClassificationId, 10);
                    return View("Index", model);
                }
                else
                {
                    DetailNewViewModel model = new DetailNewViewModel();
                    model.MainNews = cms_db.GetObjContentItemByFriendlyURL(FriendlyUrl);
                    model.CataObj = cms_db.GetObjClasscifiById(model.MainNews.CategoryId.Value);
                    model.lstSameNews = cms_db.GetlstContentItemByCataId(model.CataObj.ClassificationId, 3);
                    this.SetInforMeta(model.MainNews.MetadataKeyword, model.MainNews.MetadataDesc);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                cms_db.AddToExceptionLog("Detail", "News", e.ToString());
                return RedirectToAction("ViewError", "Home");
            }
        }

        private int UpdateViewCountContentItem(ContentItem Obj)
        {
            Obj.ViewCount = Obj.ViewCount + 1;
            return Task.Run(() => cms_db.UpdateContentItem(Obj)).Result;
        }


    }
}