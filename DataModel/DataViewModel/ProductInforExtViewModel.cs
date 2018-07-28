using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;

namespace DataModel.DataViewModel
{
    public class ProductInforExtViewModel
    {
         public ProductInforExt _ModelObj { get; set; }
        public ProductInforExtViewModel()
        {
            _ModelObj = new ProductInforExt();
        }
        public ProductInforExtViewModel(ProductInforExt model)
        {
            _ModelObj = model;
        }

        public long id
        {
            get { return _ModelObj.id; }
            set { _ModelObj.id = value; }
        }

        public Nullable<long> ProductId
        {
            get { return _ModelObj.ProductId; }
            set { _ModelObj.ProductId = value; }
        }

        public Nullable<int> Type
        {
            get { return _ModelObj.Type; }
            set { _ModelObj.Type = value; }
        }

        public Nullable<int> Size
        {
            get { return _ModelObj.Size; }
            set { _ModelObj.Size = value; }
        }

        public Nullable<int> Color
        {
            get { return _ModelObj.Color; }
            set { _ModelObj.Color = value; }
        }

        public string SKU
        {
            get { return _ModelObj.SKU; }
            set { _ModelObj.SKU = value; }
        }

        public string ColorName
        {
            get { return _ModelObj.ColorName; }
            set { _ModelObj.ColorName = value; }
        }

        public string SizeName
        {
            get { return _ModelObj.SizeName; }
            set { _ModelObj.SizeName = value; }
        }


    }
}
