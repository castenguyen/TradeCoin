using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
namespace DataModel.DataViewModel
{
    public class StoreContainsProductViewModel
    {
        public Microsite Microste { get; set; }

        public MediaContent MediaContent { get; set; }

        public List<Product> LstProduct { get; set; }
    }
}
