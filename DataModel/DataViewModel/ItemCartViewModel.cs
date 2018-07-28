using DataModel.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataViewModel
{
    public class ItemCartViewModel
    {
        public int Quantity { get; set; }

        public Product Product { get; set; }
        

        public double TotalPrice
        {
            get { return Quantity * (Product.NewPrice > 0 ? Product.NewPrice : Product.OldPrice).Value; }
        }
    }
}
