using DataModel.DataEntity;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public IQueryable<Microsite> GetlstMicroSite()
        {
            return db.Microsites;
        }
        public IQueryable<Microsite> GetlstMicroSite(long? id, long? CreatUserId, long? CataType, long? Type)
        {
            IQueryable<Microsite> lstMicrosites = db.Microsites;
            if (id.HasValue)
                lstMicrosites = lstMicrosites.Where(s => s.Id == id);
            if (CreatUserId.HasValue)
                lstMicrosites = lstMicrosites.Where(s => s.CreatUserId == CreatUserId);
            if (CataType.HasValue)
                lstMicrosites = lstMicrosites.Where(s => s.CataType == CataType);
            if (Type.HasValue)
                lstMicrosites = lstMicrosites.Where(s => s.Type == Type);
            return lstMicrosites;
        }

        public List<Microsite> GetTopNewMicrosite()
        {
            return db.Microsites.OrderByDescending(x => x.Id).Where(x=>db.Products.Count(p=>p.MicrositeID == x.Id && p.StateId ==(int)EnumCore.StateType.cho_phep) > 0).Take(12).ToList();
        }

        public Microsite GetObjMicrositeByID(long id)
        {

            try
            {
                Microsite ObjMicrosites = db.Microsites.Single(s => s.Id == id);
                return ObjMicrosites;
            }
            catch
            {
                return null;
            }

        }
        
        public Microsite GetobjMicrositeByEmailUser(string email)
        {
            return db.Microsites.SingleOrDefault(x => x.Email == email);
        }
        public Microsite GetobjMicrositeByUserId(long id)
        {
            return db.Microsites.Where(x => x.CreatUserId == id).FirstOrDefault();
        }

        public async Task<int> AddObjMicrositeAsync(Microsite ObjClass)
        {
            try
            {

                db.Microsites.Add(ObjClass);
                return await db.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public int AddObjMicrosite(Microsite ObjClass)
        {
            try
            {

                db.Microsites.Add(ObjClass);
                return db.SaveChanges();

            }
            catch (Exception e)
            {
                return (int)EnumCore.Result.action_false;
            }

        }
        public async Task<int> UpdateMicrositeAsync(Microsite ObjClass)
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

        public long CountProductOfMicrosite(long idMicro)
        {
            return db.Products.Count(x=>x.MicrositeID == idMicro && x.StateId == (int)EnumCore.StateType.cho_phep);
        }
        public long CountProductOfMicroshoptDiscount(long idMicro)
        {
            return db.Products.Count(x => x.MicrositeID == idMicro && x.NewPrice > 0 && x.StateId == (int)EnumCore.StateType.cho_phep);
        }
        public long CountNewsMicroshop(long idMicro)
        {
            return db.ContentItems.Count(x=>x.MicrositeID == idMicro && x.StateId == (int)EnumCore.StateType.cho_phep);
        }

        public long CountOrderMicroshop(long idMicro)
        {
            return db.OrderProducts.Count(x=>db.Products.FirstOrDefault(p=>p.ProductId == x.ProductId && p.MicrositeID == idMicro)!=null);
        }

        public long CountProductInMenuOfMicroshop(long idCateMicro)
        {
            return db.Products.Where(x=>x.CateMicrositeID == idCateMicro && x.StateId == (int)EnumCore.StateType.cho_phep).Count();
        }

        public long CountLikeMicroshop(long idMicroshop)
        {
            return db.CountLikes.Where(x=>x.IdMicrosite == idMicroshop && x.TypeId == (int)EnumCore.Classification.dem_luot_thich_gh).Count();
        }

        public long CheckUserLikeMicroshop(long idUser, long idMicroshop)
        {
            return db.CountLikes.Where(x => x.IdMicrosite == idMicroshop && x.IdUser == idUser && x.TypeId == (int)EnumCore.Classification.dem_luot_thich_gh).Count();
        }

        public int DeleteCountLikeMicroshop(long idUser, long idMicroshop)
        {
            CountLike count = db.CountLikes.SingleOrDefault(x => x.IdMicrosite == idMicroshop && x.IdUser == idUser && x.TypeId == (int)EnumCore.Classification.dem_luot_thich_gh);
            count.IdMicrosite = null;
            count.IdUser = null;
            count.DateLike = null;
            count.TypeId = null;
            return db.SaveChanges();
        }
        public int AddCountLikeMicroshop(CountLike count)
        {
            CountLike check = db.CountLikes.FirstOrDefault(x=>x.IdMicrosite == null && x.IdProduct == null && x.IdUser == null && x.DateLike == null && x.TypeId == null);
            if (check != null)
            {
                check.IdMicrosite = count.IdMicrosite;
                check.IdUser = count.IdUser;
                check.DateLike = count.DateLike;
                check.TypeId = (int)EnumCore.Classification.dem_luot_thich_gh;
            }
            else
            {
                db.CountLikes.Add(count);
            }
            
            return db.SaveChanges();
        }

        public List<CountLike> GetLstIdUserAreInvite(List<long> IdMic)
        {
            return db.CountLikes.Where(x => IdMic.Contains(x.IdMicrosite ?? 0) && x.TypeId == (int)EnumCore.Classification.moi_user_xem_gh).ToList();
        }
    }
}
