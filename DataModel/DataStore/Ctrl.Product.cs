using DataModel.DataEntity;
using DataModel.DataViewModel;
using DataModel.Extension;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        #region Begin BackEnd
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjContentItem"></param>
        /// <returns></returns>
        public async Task<int> CreateProduct(Product ObjProduct)
        {
            try
            {
                db.Products.Add(ObjProduct);
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {

                return (int)EnumCore.Result.action_false;
            }
        }

        public async Task<int> DeleteProductByObj(Product ObjProduct)
        {
            try
            {
                ObjProduct.StateId = (int)EnumCore.StateType.da_xoa;
                //db.Products.Remove(ObjProduct);
                db.Entry(ObjProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjContentItem"></param>
        /// <returns></returns>
        public async Task<int> UpdateProduct(Product ObjProduct)
        {
            try
            {
                db.Entry(ObjProduct).State = EntityState.Modified;
                int rs = await db.SaveChangesAsync();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception ex)
            {
                return (int)EnumCore.Result.action_false;
            }
        }
        /// <summary>
        /// insert 1 nội dung liên quan vào DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        public IQueryable<Product> GetlstProduct()
        {
            var lstProduct = db.Products;
            return lstProduct;
        }

        public List<Product> GetlstProductByMicrositeId(long id)
        {
            var lstProduct = db.Products.Where(x => x.MicrositeID == id).OrderByDescending(x => x.ProductId).Take(6).ToList();
            return lstProduct;
        }

        public IList<Product> GetlstProductHome(long CategoryId, int Limit)
        {
            var lstProduct = db.Database.SqlQuery<Product>("exec GetProductHome @IdCategory, @Limit", new SqlParameter("@IdCategory", CategoryId), new SqlParameter("@Limit", Limit)).ToList();
            return lstProduct;
        }

        public IEnumerable GetProductList()
        {
            try
            {
                List<Product> lstobj = this.GetlstProduct().ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Product> GetlstProductByCataId(int cata)
        {
            try
            {
                List<Product> lstobj = this.GetlstProduct().Where(p => p.CategoryId == cata && p.StateId!=(int)EnumCore.StateType.da_xoa).OrderByDescending(s => s.ProductId).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }






        public Product GetObjProductById(long id)
        {
            try
            {
                Product Obj = db.Products.FirstOrDefault(c => c.ProductId == id);
                return Obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Product GetObjProductForMicrosite(long idMicrosite)
        {
            return db.Products.Where(x => x.MicrositeID == idMicrosite && x.StateId == (int)EnumCore.StateType.cho_phep).OrderByDescending(x => x.CrtdDT).FirstOrDefault();
        }

        public Product GetObjProductByIdNoTracking(long id)
        {
            try
            {
                Product Obj = db.Products.FirstOrDefault(c => c.ProductId == id);
                db.Entry<Product>(Obj).State = EntityState.Detached;
                return Obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Lấy List nội dung liên quan ở đây là ContentItem liên quan với ContentItem
        /// trả về sẽ là 1 list ContentItem
        /// </summary>
        /// <param name="id">id của contenitem đưa vào</param>
        /// <returns></returns>
        /// 
        public Product GetObjProductByFriendlyURL(string FriendlyURL)
        {
            try
            {
                Product Obj = db.Products.FirstOrDefault(c => c.FriendlyURL == FriendlyURL);
                return Obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion Stop BackEnd

        /// <summary>
        /// Tim san pham
        /// </summary>
        /// <param name="name">tu khoa tim kiem</param>
        /// <param name="skip">bo qua bao nhieu san pham</param>
        /// <param name="take">lay bao nhieu san pham</param>
        /// <param name="statusId">trang thai san pham</param>
        /// <param name="idCate">id cua danh muc</param>
        /// <param name="MicrositeID">id cua gian hang</param>
        /// <returns></returns>
        public List<Product> SearchProduct(string name, int skip, int take, int statusId, int idCate, int MicrositeID = 0)
        {
            var lstProduct = db.Database.SqlQuery<Product>("exec SearchProductByCate @keyword, @stateId, @idCate, @MicrositeID, @skip, @take",
                 new SqlParameter("@keyword", name),
                 new SqlParameter("@stateId", statusId),
                 new SqlParameter("@idCate", idCate),
                 new SqlParameter("@MicrositeID", MicrositeID),
                 new SqlParameter("@skip", skip),
                 new SqlParameter("@take", take)
                 ).ToList();
            return lstProduct;
        }

        public List<Product> GetlstProductByCataIdHasNum(int cata, int num)
        {
            try
            {
                List<Product> lstobj = this.GetlstProduct().Where(p => p.CategoryId == cata && p.StateId != (int)EnumCore.StateType.da_xoa).OrderByDescending(s => s.ProductId).Take(num).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Product> GetlstProductByUserIdHasNum(long UserId, int num)
        {
            try
            {
                List<Product> lstobj = this.GetlstProduct().Where(p => p.CrtdUserId == UserId).OrderByDescending(s => s.ProductId).Take(num).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable GetProductListForSelectlist()
        {
            try
            {
                var lstobj = (from c in this.GetlstProduct() select new { c.ProductId, c.ProductName }).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Product> GetlstProductByCataId(int cata, int num)
        {
            try
            {
                List<Product> lstobj = this.GetlstProduct().Where(p => p.CategoryId == cata && p.StateId != (int)EnumCore.StateType.da_xoa).OrderByDescending(s => s.ProductId).Take(num).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<Product> GetlstProductByParentCateId(int parentcate, int skip, int num, int StateId)
        {
            try
            {
                List<Product> lstobj = this.db.Database.SqlQuery<Product>("exec GetProductByParentCate @ParentCate, @index, @NumOfCate, @StateId",
                            new SqlParameter("@ParentCate", parentcate),
                            new SqlParameter("@index", skip),
                            new SqlParameter("@NumOfCate", num),
                            new SqlParameter("@StateId", StateId)
                         ).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Lấy sản phẩm của các loại như sp theo xu hướng, sp giảm giá nhiều,
        /// </summary>
        /// <param name="parentcate">Id danh mụcDanh mục cha</param>
        /// <param name="takeInACate">Số lượng muốn lấy sản phẩm ở danh mục con</param>
        /// <param name="takeAll">Lấy ra sản phẩm từ các danh mục con đã được order by theo yêu cầu</param>
        /// <returns></returns>
        public List<Product> GetlstProductByParentCateAllType(int parentcate, int takeInACate, int takeAll, int StateId)
        {
            try
            {
                List<Product> lstobj = this.db.Database.SqlQuery<Product>("exec GetProductByParentCate @ParentCate, @index, @NumOfCate, @StateId",
                           new SqlParameter("@ParentCate", parentcate),
                           new SqlParameter("@index", takeInACate),
                           new SqlParameter("@NumOfCate", takeAll),
                           new SqlParameter("@StateId", StateId)
                        ).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Product> GetlstProductByParentCateIdEntity(int parentcate, int num)
        {
            try
            {
                List<Product> model = new List<Product>();
                if (this.GetlstClassifi().Where(s => s.ParentClassificationId == parentcate).Count() > 0)
                {
                    int[] lstchildid = this.GetlstClassifi().Where(s => s.ParentClassificationId == parentcate).Select(s => s.ClassificationId).ToArray();
                    lstchildid = lstchildid.Concat(new[] { parentcate }).ToArray();
                    model = this.GetlstProduct().Where(i => lstchildid.Contains(i.CategoryId.Value)).Where(s => s.StateId == (int)EnumCore.StateType.cho_phep).OrderByDescending(s => s.CrtdDT).Take(num).ToList();
                }
                else {
                    model = this.GetlstProduct().Where(s => s.CategoryId == parentcate).OrderByDescending(s => s.CrtdDT).Take(num).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Product> GetlstPromotionProductByCateId(int ProductCateId)
        {
            try
            {
                List<Product> lstobj = this.db.Database.SqlQuery<Product>("exec GetlstPromotionProductByCateId @CateId",
                            new SqlParameter("@CateId", ProductCateId)).Take(4).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<Product> GetlstPromotionProductByPromotionId(int PromotionId)
        {
            try
            {
                List<Product> lstobj = this.db.Database.SqlQuery<Product>("exec GetlstPromotionProductByPromotionId @PromotionId",
                            new SqlParameter("@PromotionId", PromotionId)).ToList();
                return lstobj;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public List<ProductItemViewModel> GetListNewsProduct(int num)
        {
            List<ProductItemViewModel> LstItemNewProduct = new List<ProductItemViewModel>();//lấy  san pham mới
            List<Product> tmpn = this.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep)
                                                .OrderByDescending(s => s.CrtdDT).Take(num).ToList();
            foreach (Product item in tmpn)
            {
                ProductItemViewModel tmp2 = new ProductItemViewModel();
                tmp2.ObiProduct = item;
                tmp2.Obicate = item.ObjCatelogry;
                LstItemNewProduct.Add(tmp2);
            }
            return LstItemNewProduct;
        
        }

        public List<ProductItemViewModel> GetListDayProduct(int num)
        {
            List<ProductItemViewModel> LstItemNewProduct = new List<ProductItemViewModel>();//lấy  san pham danh mục hoat dong
            List<Product> tmpn = this.GetlstProduct().Where(s => s.CategoryId== 11882)
                                                .OrderByDescending(s => s.CrtdDT).Take(num).ToList();
            foreach (Product item in tmpn)
            {
                ProductItemViewModel tmp2 = new ProductItemViewModel();
                tmp2.ObiProduct = item;
                tmp2.Obicate = item.ObjCatelogry;
                LstItemNewProduct.Add(tmp2);
            }
            return LstItemNewProduct;

        }



        public List<ProductItemViewModel> GetListViewProduct(int num)
        {
            List<ProductItemViewModel> LstItemNewProduct = new List<ProductItemViewModel>();//lấy  san xem nhieu
            List<Product> tmpn = this.GetlstProduct().Where(s => s.StateId == (int)EnumCore.StateType.cho_phep)
                                                .OrderByDescending(s => s.ViewCount).Take(num).ToList();
            foreach (Product item in tmpn)
            {
                ProductItemViewModel tmp2 = new ProductItemViewModel();
                tmp2.ObiProduct = item;
                tmp2.Obicate = item.ObjCatelogry;
                LstItemNewProduct.Add(tmp2);
            }
            return LstItemNewProduct;

        }


        public int SaveFavoriteProductByUser(CountLike count)
        {
            CountLike check = db.CountLikes.FirstOrDefault(x => x.IdMicrosite == null && x.IdProduct == null && x.IdUser == null && x.DateLike == null && x.TypeId == null);
            if (check != null)
            {
                check.IdProduct = count.IdProduct;
                check.IdUser = count.IdUser;
                check.DateLike = count.DateLike;
                check.TypeId = (int)EnumCore.Classification.dem_luot_thich_sp;
            }
            else
            {
                db.CountLikes.Add(count);
            }
            return db.SaveChanges();
        }

        public long CheckUserSaveFavoriteProduct(long idUser, long idProduct)
        {
            return db.CountLikes.Where(x => x.IdProduct == idProduct && x.IdUser == idUser && x.TypeId == (int)EnumCore.Classification.dem_luot_thich_sp).Count();
        }

        public long CountSaveFavoriteProductOfUser(long idUser)
        {
            return db.CountLikes.Where(x => x.IdUser == idUser && x.IdMicrosite == null && x.IdProduct != null && x.TypeId == (int)EnumCore.Classification.dem_luot_thich_sp).Count();
        }

        public List<Product> GetlstProductSaveFavoriteByUser(long idUser)
        {
            List<CountLike> lstLike = db.CountLikes.Where(x => x.IdUser == idUser && x.IdMicrosite == null && x.IdProduct != null && x.TypeId == (int)EnumCore.Classification.dem_luot_thich_sp).ToList();
            List<Product> lstProduct = new List<Product>();
            lstLike.ForEach(x=>{
                Product product = db.Products.SingleOrDefault(p => p.ProductId == x.IdProduct);
                if (product != null)
                {
                    lstProduct.Add(product);
                }
            });
            return lstProduct;
        }

        public long CountLikeProduct(long idProduct)
        {
            return db.CountLikes.Where(x=>x.IdProduct == idProduct && x.TypeId == (int)EnumCore.Classification.dem_luot_thich_sp).Count();
        }

        public long DeleteSaveFavoriteProductOfUser(long idUser, long idProduct)
        {
            CountLike count = db.CountLikes.SingleOrDefault(x => x.IdProduct == idProduct && x.IdUser == idUser && x.TypeId == (int)EnumCore.Classification.dem_luot_thich_sp);
            if (count!=null)
            {
                count.IdProduct = null;
                count.IdUser = null;
                count.DateLike = null;
                count.TypeId = null;
                int success = db.SaveChanges();
                return success;
            }
            return 0;
        }



        public HomeListProductViewModel GetLstHomeProductByCataId(int CataId, int Num)
        {
            HomeListProductViewModel model = new HomeListProductViewModel();
            model.CataObj = this.GetObjClasscifiById(CataId);
            int[] tmpcate = this.GetlstClassifiByParentId(CataId).Select(s => s.ClassificationId).ToArray();
            if (tmpcate.Length >= 1)
            {
                model.lstProduct = this.GetlstProduct().Where(s => tmpcate.Contains(s.CategoryId.Value)
              && s.StateId != (int)EnumCore.StateType.da_xoa).OrderByDescending(s => s.ProductId).Take(Num).ToList();
            }
            else
            {

                model.lstProduct = this.GetlstProduct().Where(s => s.CategoryId== CataId 
                    && s.StateId != (int)EnumCore.StateType.da_xoa).OrderByDescending(s => s.ProductId).Take(Num).ToList();
            }
          
            return model;
        }
    }
}
