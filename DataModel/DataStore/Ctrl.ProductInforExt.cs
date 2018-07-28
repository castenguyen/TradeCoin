using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using DataModel.DataEntity;
using DataModel.Extension;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public int CreateProductInforExt(ProductInforExt model)
        {
            db.ProductInforExts.Add(model);
            return (int)EnumCore.Result.action_true;
        }

        public IQueryable<ProductInforExt> GetProductInforExt(long? ProductId, int? Type, int? color, int? size, string sku)
        {
            IQueryable<ProductInforExt> tmp = db.ProductInforExts;
            if (ProductId.HasValue)
                tmp = tmp.Where(s => s.ProductId == ProductId.Value);
            if (Type.HasValue)
                tmp = tmp.Where(s => s.Type == Type.Value);
            if (color.HasValue)
                tmp = tmp.Where(s => s.Color == color.Value);
            if (size.HasValue)
                tmp = tmp.Where(s => s.Size == size.Value);
            if (!String.IsNullOrEmpty(sku))
                tmp = tmp.Where(s => s.SKU == sku);
            return tmp;

        }

     
        public int DeleteProductInforExtByProductIdVsType(long ProductId, int Type)
        {
            List<ProductInforExt> lstProductInforExt = db.ProductInforExts.Where(s => s.ProductId.Value == ProductId && s.Type.Value == Type).ToList();
            foreach (ProductInforExt item in lstProductInforExt)
            {
                db.ProductInforExts.Remove(item);
                db.SaveChanges();
            }
            return (int)EnumCore.Result.action_true;
        }

    }
}
