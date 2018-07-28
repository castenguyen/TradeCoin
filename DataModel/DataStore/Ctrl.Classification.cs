using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
using System.Data.Entity;
using System.Collections;
using DataModel.Extension;
using DataModel.DataViewModel;
namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        #region Begin BackEnd
        public async Task<int> Classifi(Classification ObjClass)
        {
            try
            {
                if (!string.IsNullOrEmpty(ObjClass.ClassificationNM))
                {
                    db.Classifications.Add(ObjClass);
                    return await db.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public async Task<int> EditClass(Classification ObjClass)
        {
            try
            {
                db.Entry(ObjClass).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public IQueryable<Classification> GetlstClassifi()
        {
            var lstScheme = db.Classifications;
            return lstScheme;
        }

        public IQueryable<Classification> GetlstClassifiByParentId(int ParentId)
        {
            var lstClassifi = db.Classifications.Where(m => m.ParentClassificationId == ParentId);
            return lstClassifi;
        }
        public IQueryable<Classification> GetlstClassifiBySchemeId(int SchemeId)
        {
            var lstClassifi = db.Classifications.Where(m => m.ClassificationSchemeId == SchemeId);
            return lstClassifi;
        }

        public IEnumerable GetLstClassification()
        {
            try
            {
                List<Classification> lstobj = this.GetlstClassifi().ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable GetListGroupClassifi()
        {
            List<SubMenuViewModels> lstGroupClass = new List<SubMenuViewModels>();
            List<Classification> Newspattem = this.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.tin_tuc_bai_viet)
                                                               .Where(s => s.ParentClassificationId == null).ToList();
            foreach (Classification obj in Newspattem)
            {
                SubMenuViewModels _item = new SubMenuViewModels();
                List<Classification> childP = GetlstClassifi().Where(s => s.ParentClassificationId == obj.ClassificationId).ToList();
                _item.Parent = obj;
                _item.lstChild = childP;
                lstGroupClass.Add(_item);
            }
            return lstGroupClass;
        }

        public IEnumerable Getnewcatagory()//lấy catagory tin tức
        {
            List<object> mylist = new List<object>();
            mylist.Clear();
            List<Classification> Newspattem = this.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.tin_tuc_bai_viet)
                                                                                .Where(s => s.ParentClassificationId == null).ToList();
            //List<Classification> Newspattem2 = this.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.page_infor)
            //                                                                  .Where(s => s.ParentClassificationId == null).ToList();

            foreach (Classification objclass in Newspattem)
            {
                mylist.Add(objclass);
                List<Classification> childP = this.GetlstClassifi().Where(s => s.ParentClassificationId == objclass.ClassificationId).ToList();
                foreach (Classification objChild in childP)
                {
                    objChild.ClassificationNM = "-->" + objChild.ClassificationNM;
                    mylist.Add(objChild);
                }
            }

            //foreach (Classification objclass in Newspattem2)
            //{
            //    mylist.Add(objclass);
            //    List<Classification> childP = this.GetlstClassifi().Where(s => s.ParentClassificationId == objclass.ClassificationId).ToList();
            //    foreach (Classification objChild in childP)
            //    {
            //        objChild.ClassificationNM = "-->" + objChild.ClassificationNM;
            //        mylist.Add(objChild);
            //    }
            //}
            return mylist;
        }


        public IEnumerable GetPageInforCatagory()//lấy catagory trang thông tin
        {
            List<object> mylist = new List<object>();
            mylist.Clear();

            List<Classification> Newspattem2 = this.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.page_infor).Where(s=>s.IsEnabled==1).ToList();
            foreach (Classification objclass in Newspattem2)
            {
                mylist.Add(objclass);
                List<Classification> childP = this.GetlstClassifi().Where(s => s.ParentClassificationId == objclass.ClassificationId).ToList();
                foreach (Classification objChild in childP)
                {
                    objChild.ClassificationNM = "-->" + objChild.ClassificationNM;
                    mylist.Add(objChild);
                }
            }
            return mylist;
        }

        public IEnumerable GetProductCatagory()//lấy catagory sản phẩm
        {
            List<SelectListObj> mylist = new List<SelectListObj>();
            mylist.Clear();
            List<SelectListObj> Newspattem = this.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.san_pham)
              .Where(s => s.ParentClassificationId == null).Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();
            foreach (SelectListObj objclass in Newspattem)
            {
                mylist.Add(objclass);
                List<SelectListObj> childP = this.GetlstClassifi().Where(s => s.ParentClassificationId == objclass.value)
                            .Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();
                foreach (SelectListObj objChild in childP)
                {
                    objChild.text = "--" + objChild.text;
                    mylist.Add(objChild);

                    //List<SelectListObj> childP2 = this.GetlstClassifi().Where(s => s.ParentClassificationId == objChild.value)
                    //       .Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();
                    //foreach (SelectListObj objChild2 in childP2)
                    //{
                    //    objChild2.text = "---" + objChild2.text;
                    //    mylist.Add(objChild2);
                    //}

                }
            }
            return mylist;
        }

        public IEnumerable GetProductCatagoryByPrentId(int parent)//lấy catagory sản phẩm theo danh mục cha
        {
            List<SelectListObj> mylist = new List<SelectListObj>();
            mylist.Clear();
            List<SelectListObj> Newspattem = this.GetlstClassifiBySchemeId((int)EnumCore.ClassificationScheme.san_pham)
              .Where(s => s.ParentClassificationId == parent).Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();
            foreach (SelectListObj objclass in Newspattem)
            {
                mylist.Add(objclass);
                List<SelectListObj> childP = this.GetlstClassifi().Where(s => s.ParentClassificationId == objclass.value)
                            .Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();
                foreach (SelectListObj objChild in childP)
                {
                    objChild.text = "--" + objChild.text;
                    mylist.Add(objChild);

                    //List<SelectListObj> childP2 = this.GetlstClassifi().Where(s => s.ParentClassificationId == objChild.value)
                    //       .Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();
                    //foreach (SelectListObj objChild2 in childP2)
                    //{
                    //    objChild2.text = "---" + objChild2.text;
                    //    mylist.Add(objChild2);
                    //}

                }
            }
            return mylist;
        }

        /// <summary>
        /// Lấy danh mục sản pham trong microsite
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetProductMicrositeCateById(long id)
        {
            List<object> mylist = new List<object>();
            mylist.Clear();
            List<MicroClassification> Newspattem = this.GetlstMicroClassifi(null, null, null, id).ToList();
            foreach (MicroClassification objclass in Newspattem)
            {
                mylist.Add(objclass);
            }
            return mylist;
        }


        public IEnumerable GetCatagoryForSelectList(int SchemeId)//lấy catagory bất kỳ
        {
            List<object> mylist = new List<object>();
            mylist.Clear();
            List<Classification> Newspattem = this.GetlstClassifiBySchemeId(SchemeId)
                                                                                .Where(s => s.ParentClassificationId == null).ToList();
            foreach (Classification objclass in Newspattem)
            {
                mylist.Add(objclass);
                List<Classification> childP = this.GetlstClassifi().Where(s => s.ParentClassificationId == objclass.ClassificationId).ToList();
                foreach (Classification objChild in childP)
                {
                    objChild.ClassificationNM = "-->" + objChild.ClassificationNM;
                    mylist.Add(objChild);
                }
            }
            return mylist;
        }

        public List<SelectListObj> Getclasscatagory(int schemeid)//lấy catagory bất kỳ
        {
            List<SelectListObj> mylist = new List<SelectListObj>();

            List<SelectListObj> lstobj = this.GetlstClassifi().Where(s => s.ParentClassificationId == null && s.ClassificationSchemeId == schemeid && s.IsEnabled==1).
                Select(p => new SelectListObj {value=p.ClassificationId,text=p.ClassificationNM }).ToList();
                                

            foreach (SelectListObj objclass in lstobj)
            {
                mylist.Add(objclass);
                List<SelectListObj> childP = this.GetlstClassifi().Where(s => s.ParentClassificationId == objclass.value).
                                 Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();

                foreach (SelectListObj objChild in childP)
                {
                    objChild.text = "  --" + objChild.text;
                    mylist.Add(objChild);
                }
            }
            return mylist;
        }

        /// <summary>
        /// LẤY DANH MỤC THEO ID DANH MUC CHA
        /// </summary>
        /// <param name="schemeid"></param>
        /// <returns></returns>
        public List<SelectListObj> GetclassCatagory(int parent)//lấy catagory bất kỳ
        {
            List<SelectListObj> mylist = new List<SelectListObj>();

            List<SelectListObj> lstobj = this.GetlstClassifi().Where(s => s.ParentClassificationId == parent  && s.IsEnabled == 1).
                Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();
            foreach (SelectListObj objclass in lstobj)
            {
                mylist.Add(objclass);
                List<SelectListObj> childP = this.GetlstClassifi().Where(s => s.ParentClassificationId == objclass.value).
                                 Select(p => new SelectListObj { value = p.ClassificationId, text = p.ClassificationNM }).ToList();

                foreach (SelectListObj objChild in childP)
                {
                    objChild.text = "  --" + objChild.text;
                    mylist.Add(objChild);
                }
            }
            return mylist;
        }


        public Classification GetObjClasscifiByFriendlyUrl(string FriendlyUrl)
        {
            try
            {
                Classification Obj = db.Classifications.FirstOrDefault(c => c.FriendlyURL == FriendlyUrl);
                return Obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Classification GetObjClasscifiById(int id)
        {
            try
            {
                Classification Obj = db.Classifications.FirstOrDefault(c => c.ClassificationId == id);
                return Obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetNameObjClasscifiById(int? id)
        {
            try
            {
                if(id==null)
                    return "";
                string Obj = db.Classifications.FirstOrDefault(c => c.ClassificationId == id).ClassificationNM;
                return Obj;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<int> DeleteClass(int? Id)
        {
            Classification ObjClass = db.Classifications.FirstOrDefault(c => c.ClassificationId == Id);
            db.Classifications.Remove(ObjClass);
            return await db.SaveChangesAsync();
        }

        public async Task<int> ChangeOrderByDe(int idchange, int? _ParentClass, int _Schemeid)// Tăng DisplayOrder
        {
            Classification objClassChange = new Classification();
            Classification objClassNext = new Classification();
            objClassChange = db.Classifications.FirstOrDefault(c => c.ClassificationId == idchange);
            if( objClassChange.DisplayOrder==0)
            {
                int _tmp = db.Classifications.Where(c => c.ClassificationSchemeId == _Schemeid && c.ParentClassificationId == null).Max(c => c.DisplayOrder).Value;
                objClassChange.DisplayOrder = _tmp + 1;
                db.Entry(objClassChange).State = EntityState.Modified;
                return await db.SaveChangesAsync();

            }
           

            if (_ParentClass != null)
            {
                int _MaxClassDisplay = db.Classifications.Where(c => c.ClassificationSchemeId == _Schemeid && c.ParentClassificationId == _ParentClass).Max(c => c.DisplayOrder).Value;
                if (db.Classifications.Where(c => c.ClassificationSchemeId == _Schemeid && c.ParentClassificationId == _ParentClass && c.DisplayOrder == objClassChange.DisplayOrder).Count() > 1)
                {
                    objClassChange.DisplayOrder = _MaxClassDisplay + 1;
                    db.Entry(objClassChange).State = EntityState.Modified;
                    return await db.SaveChangesAsync();
                }
                
                
                if (objClassChange.DisplayOrder == _MaxClassDisplay)
                {
                    if (db.Classifications.Where(c => c.ClassificationSchemeId == _Schemeid && c.ParentClassificationId == _ParentClass && c.DisplayOrder==_MaxClassDisplay).Count()>1)
                    {
                        objClassChange.DisplayOrder = _MaxClassDisplay + 1;
                        db.Entry(objClassChange).State = EntityState.Modified;
                        return await db.SaveChangesAsync();
                    }
                }
                objClassNext = db.Classifications.Where(c => c.ParentClassificationId == _ParentClass && c.DisplayOrder > objClassChange.DisplayOrder).OrderBy(s => s.DisplayOrder).FirstOrDefault();
            }
            else
            {
                int _MaxClassDisplay = db.Classifications.Where(c => c.ClassificationSchemeId == _Schemeid && c.ParentClassificationId == null).Max(c => c.DisplayOrder).Value;
                if (db.Classifications.Where(c => c.ClassificationSchemeId == _Schemeid && c.ParentClassificationId == _ParentClass && c.DisplayOrder == objClassChange.DisplayOrder).Count() > 1)
                {
                    objClassChange.DisplayOrder = _MaxClassDisplay + 1;
                    db.Entry(objClassChange).State = EntityState.Modified;
                    return await db.SaveChangesAsync();
                }
                if (objClassChange.DisplayOrder == _MaxClassDisplay)
                {
                    if (db.Classifications.Where(c => c.ClassificationSchemeId == _Schemeid && c.ParentClassificationId == null && c.DisplayOrder == _MaxClassDisplay).Count() > 1)
                    {
                        objClassChange.DisplayOrder = _MaxClassDisplay + 1;
                        db.Entry(objClassChange).State = EntityState.Modified;
                        return await db.SaveChangesAsync();
                    }
                }
                objClassNext = db.Classifications.Where(c => c.ClassificationSchemeId == _Schemeid && c.ParentClassificationId == null && c.DisplayOrder > objClassChange.DisplayOrder).OrderBy(s => s.DisplayOrder).FirstOrDefault();
            }
            if (objClassNext.DisplayOrder.HasValue)
            {
                int _tmp = objClassNext.DisplayOrder.Value;
                objClassNext.DisplayOrder = objClassChange.DisplayOrder;
                objClassChange.DisplayOrder = _tmp;
            }
            db.Entry(objClassChange).State = EntityState.Modified;
            db.Entry(objClassNext).State = EntityState.Modified;
            return await db.SaveChangesAsync();
        }
        #endregion Stop BackEnd
        public IEnumerable GetClassifiListForSelectlist(int SchemeiD)
        {
            try
            {
                var lstobj = (from c in this.GetlstClassifi().Where(s => s.ClassificationSchemeId == SchemeiD)
                              select new { c.ClassificationId, c.ClassificationNM }).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region FRONTEND
        public List<Classification> GetMainMenuList(int? ParentId, int? Scheme)
        {
            List<Classification> Result = new List<Classification>();
            if (ParentId.HasValue)
            {
                Result = this.GetlstClassifiByParentId(ParentId.Value).ToList();
            }
            else
            {
                Result = this.GetlstClassifi().Where(s => s.ParentClassificationId == null
                    && s.ClassificationSchemeId == Scheme && s.IsEnabled == (int)EnumCore.StateType.enable
                   ).OrderBy(s => s.DisplayOrder).ToList();
            }
            return Result;

        }

        public List<Classification> GetMainMenuList(int? ParentId)
        {
            List<Classification> Result = new List<Classification>();
            List<Classification> Result2 = new List<Classification>();
            if (ParentId.HasValue)
            {
                Result = this.GetlstClassifiByParentId(ParentId.Value).Where(s=>s.IsEnabled== (int)EnumCore.StateType.enable).ToList();
            }
            else
            {
                Result = this.GetlstClassifi().Where(s => s.ParentClassificationId == null
                    && s.ClassificationSchemeId == (int)EnumCore.ClassificationScheme.san_pham && s.IsEnabled == (int)EnumCore.StateType.enable
                   ).OrderBy(s => s.DisplayOrder).ToList();

                //Result2 = this.GetlstClassifi().Where(s => s.ParentClassificationId == null
                //  &&  s.ClassificationSchemeId == (int)EnumCore.ClassificationScheme.display_type && s.IsEnabled==(int)EnumCore.StateType.Enable
                // ).OrderBy(s => s.DisplayOrder).ToList();
                //Result = Result.Concat(Result2).ToList();
            }
            return Result;

        }


        public List<BlockChildProductMenu> GetM2ProductList(int? ParentId)
        {
            List<BlockChildProductMenu> Result = new List<BlockChildProductMenu>();
            List<BlockClassMenu> lstM2 = this.GetlstClassifiByParentId(ParentId.Value).Where(s=>s.IsEnabled==(int)EnumCore.StateType.enable).Select(p => new BlockClassMenu 
                            { ClassId = p.ClassificationId, ClassName = p.ClassificationNM,ClassFriendly=p.FriendlyURL,ParentClassId=p.ParentClassificationId.Value }).ToList();
            foreach (BlockClassMenu _item in lstM2)
            {
                BlockChildProductMenu tmp = new BlockChildProductMenu();
                tmp.Parent = _item;
                tmp.lstChild = this.GetlstClassifiByParentId(_item.ClassId).Where(s => s.IsEnabled == (int)EnumCore.StateType.enable).Select(p => new BlockClassMenu 
                                { ClassId = p.ClassificationId, ClassName = p.ClassificationNM,ClassFriendly=p.FriendlyURL,ParentClassId=p.ParentClassificationId.Value }).Take(5).ToList();
                Result.Add(tmp);
            }
            return Result;
        }


        public List<SubMenuViewModels> GetSubMenuViewModels()
        {
            List<SubMenuViewModels> model = new List<SubMenuViewModels>();
            List<Classification> ParentListMenu = this.GetMainMenuList(null);
            foreach (Classification item in ParentListMenu)
            {
                SubMenuViewModels MenuItem = new SubMenuViewModels();
                MenuItem.Parent = item;
                MenuItem.lstChild = this.GetMainMenuList(item.ClassificationId);
                model.Add(MenuItem);
            }
            return model;
        }

     

        public string GetsttringClassName(int id)
        {
            return db.Classifications.Single(s => s.ClassificationId == id).ClassificationNM;
        
        }

        #endregion ENDFRONTEND

    }
}
