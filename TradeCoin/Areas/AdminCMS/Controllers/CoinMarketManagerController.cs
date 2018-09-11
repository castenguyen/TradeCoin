using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.CoinMaket;
using DataModel.CoinmaketEntity;
using DataModel.CoinmaketEnum;
using DataModel.DataEntity;
using CMSPROJECT.Areas.AdminCMS.Core;


namespace CMSPROJECT.Areas.AdminCMS.Controllers
{
    public class CoinMarketManagerController : CoreBackEnd
    {
        readonly CoinMarketCapClient _client;

        public CoinMarketManagerController()
        {
            _client = CoinMarketCapClient.GetInstance();
            //  _client = new CoinMarketCapClient();
        }

        public async Task<ActionResult> GetAndUpcateTableCyptoItem()
        {
            CoinMarketDataRespon coinmaketresult = await _client.GetListCyptoItemAsync();
            foreach (CyproEntity Item in coinmaketresult.data)
            {
                CyptoItem objCyptoItem = new CyptoItem();
                objCyptoItem.id = Item.id;
                objCyptoItem.name = Item.name;
                objCyptoItem.symbol = Item.symbol;
                objCyptoItem.slug = Item.slug;
                if (Item.is_active == 1)
                {
                    objCyptoItem.is_active = true;
                }
                else
                {
                    objCyptoItem.is_active = false;
                }

                try
                {
                    objCyptoItem.first_historical_data = DateTime.Parse(Item.first_historical_data);
                }
                catch
                {
                    objCyptoItem.first_historical_data = DateTime.Now;
                }

                try
                {
                    objCyptoItem.last_historical_data = DateTime.Parse(Item.last_historical_data);
                }
                catch
                {
                    objCyptoItem.last_historical_data = DateTime.Now;
                }


                await cms_db.CreateCyptoItem(objCyptoItem);
            }
            return View();
        }


        public async Task<ActionResult> GetPriceOfCyptoItem()
        {
            string lstCyptoid = "1,1027";
            IDictionary<string, CyproPriceEntity> coinmaketresult = await _client.GetListPriceCyptoItemAsync(lstCyptoid);
            foreach (CyproPriceEntity Item in coinmaketresult.Values)
            {
                CyptoItemPrice objCyptoItemPrice = new CyptoItemPrice();
                objCyptoItemPrice.id = Item.id;
                objCyptoItemPrice.name = Item.name;
                objCyptoItemPrice.symbol = Item.symbol;
                objCyptoItemPrice.slug = Item.slug;
                objCyptoItemPrice.CyptoItemPriceUpdate = DateTime.Now;
                objCyptoItemPrice.is_active = true;
                objCyptoItemPrice.cmc_rank = Item.cmc_rank;
                objCyptoItemPrice.num_market_pairs = Item.num_market_pairs;
                objCyptoItemPrice.circulating_supply = Item.circulating_supply;
                objCyptoItemPrice.total_supply = Item.total_supply;
                objCyptoItemPrice.max_supply = Item.max_supply ?? 0 ;
                try
                {
                    objCyptoItemPrice.last_updated = DateTime.Parse(Item.last_updated);
                }
                catch
                {
                    objCyptoItemPrice.last_updated = DateTime.Now;
                }

                 PriceEntity USD = Item.quote.Values.First();
                objCyptoItemPrice.USD_price = USD.price;
                objCyptoItemPrice.USD_volume_24h = USD.volume_24h;
                objCyptoItemPrice.USD_percent_change_1h = USD.percent_change_1h;
                objCyptoItemPrice.USD_percent_change_7d = USD.percent_change_7d;
                objCyptoItemPrice.USD_market_cap = USD.market_cap;
                await cms_db.CreateCyptoItemPrice(objCyptoItemPrice);
            }
            return View();
        }




        #region    ================================================    example  ====================================================
        public async Task<JsonResult> GetTickerList_NoParams_Success()
        {
            CoinMarketDataRespon ticker = await _client.GetListCyptoItemAsync();

            return Json(ticker, JsonRequestBehavior.AllowGet);

        }


        public async Task<JsonResult> GetTickerList_Limit10_Success()
        {
            List<TickerEntity> ticker = await _client.GetTickerListAsync(10);
            //Assert.IsNotNull(ticker);
            //Assert.AreEqual(ticker.Count, 10);
            // Assert.Greater(ticker.First().PriceUsd, 0);
            // Assert.Less(ticker.First().LastUpdated, DateTime.Now);

            return Json(ticker, JsonRequestBehavior.AllowGet);

        }


        public async Task<JsonResult> GetTickerList_Unlimited_Success()
        {
            List<TickerEntity> ticker = await _client.GetTickerListAsync(0);
            //  Assert.IsNotNull(ticker);
            //  Assert.Greater(ticker.Count, 100);
            // Assert.Greater(ticker.First().PriceUsd, 0);
            //  Assert.Less(ticker.First().LastUpdated, DateTime.Now);
            return Json(ticker, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetTickerList_Limit5ConvertEUR_Success()
        {
            List<TickerEntity> ticker = await _client.GetTickerListAsync(5, ConvertEnum.EUR);
            // Assert.IsNotNull(ticker);
            // Assert.AreEqual(ticker.Count, 5);
            // Assert.Greater(ticker.First().PriceOther[ConvertEnum.EUR], 0);
            //Assert.Less(ticker.First().LastUpdated, DateTime.Now);
            return Json(ticker, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetTicker_Bitcoin_Success()
        {
            TickerEntity ticker = await _client.GetTickerAsync("bitcoin");
            //  Assert.IsNotNull(ticker);
            // Assert.Greater(ticker.Name, "bitcoin");
            //  Assert.Greater(ticker.PriceOther[Enums.ConvertEnum.USD], 0);
            //  Assert.Less(ticker.LastUpdated, DateTime.Now);
            return Json(ticker, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetTicker_Ethereum_Success()
        {
            TickerEntity ticker = await _client.GetTickerAsync("ethereum");
            //   Assert.IsNotNull(ticker);
            // Assert.Greater(ticker.Name, "ethereum");
            //   Assert.Greater(ticker.PriceOther[Enums.ConvertEnum.USD], 0);
            //  Assert.Less(ticker.LastUpdated, DateTime.Now);
            return Json(ticker, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetTicker_Ripple_Success()
        {
            TickerEntity ticker = await _client.GetTickerAsync("ripple", ConvertEnum.USD);
            // Assert.IsNotNull(ticker);
            //Assert.Greater(ticker.Name, "ripple");
            //Assert.Greater(ticker.PriceOther[Enums.ConvertEnum.USD], 0);
            //Assert.Less(ticker.LastUpdated, DateTime.Now);
            return Json(ticker, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetGlobalDataAsync_Default_Success()
        {
            GlobalDataEntity globalData = await _client.GetGlobalDataAsync();
            //Assert.IsNotNull(globalData);
            //Assert.Greater(globalData.MarketCapUsd, 0);
            return Json(globalData, JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetTickerList_All_Success()
        {
            List<TickerEntity> ticker = await _client.GetTickerListAsync(0);
            //Assert.IsNotNull(ticker);
            //Assert.Less(ticker.First().LastUpdated, DateTime.Now);
            return Json(ticker, JsonRequestBehavior.AllowGet);

        }


        public async Task<JsonResult> GetTickerList_NamedParam_Success()
        {
            List<TickerEntity> ticker = await _client.GetTickerListAsync(limit: 0);
            //Assert.IsNotNull(ticker);
            //Assert.Less(ticker.First().LastUpdated, DateTime.Now);
            return Json(ticker, JsonRequestBehavior.AllowGet);
        }

        #endregion 


    }
}