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

        public IQueryable<MarketItem> GetlstMarketItem()
        {
            var lstMarketItem = db.MarketItems;
            return lstMarketItem;
        }


        public string GetMarketName(long id)
        {
            string MarketName = db.MarketItems.Where(s => s.Marketid == id).Select(s => s.MarketName).First();
            return MarketName;
        }

    }
}
