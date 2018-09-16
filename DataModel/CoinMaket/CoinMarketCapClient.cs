using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DataModel.CoinmaketEntity;
using DataModel.CoinmaketEnum;
using DataModel.Extension;
using DataModel.DataViewModel;
using DataModel.DataStore;
using DataModel.DataEntity;




namespace DataModel.CoinMaket
{




    /// <summary>
    /// Coin Market Cap Api Client.



    public class CoinMarketCapClient : ICoinMarketCapClient, IDisposable
    {


      


        bool _isDisposed;
        readonly string _uri = "https://pro-api.coinmarketcap.com/";
        readonly string _privateToken = "7181b157-2b8d-4b6d-be73-dbc791f1de3f";

        readonly HttpClient _client;

        static CoinMarketCapClient s_instance;

        #region ==================================================== Constructors ==========================================================
        /// <summary>
        /// Retrieves the an instance of the CoinMarketCapClient.
        /// </summary>
        /// <returns>CoinMarketCapClient instance.</returns>
        public static CoinMarketCapClient GetInstance() => s_instance = s_instance ?? new CoinMarketCapClient();

        /// <summary>
        /// Retrieves the an instance of the CoinMarketCapClient.
        /// </summary>
        /// <param name="httpClientHandler">Custom HTTP client handler. Can be used to define proxy settigs</param>
        /// <returns>CoinMarketCapClient instance.</returns>
        public static CoinMarketCapClient GetInstance(HttpClientHandler httpClientHandler) => 
            s_instance = s_instance ?? new CoinMarketCapClient(httpClientHandler);

        /// <summary>
        /// Initializes a new instance of the CoinMarketCapClient class.
        /// </summary>
        /// <param name="httpClientHandler">Custom HTTP client handler. Can be used to define proxy settigs.</param>
        public CoinMarketCapClient(HttpClientHandler httpClientHandler)
        {
            if (httpClientHandler != null)
            {
                this._client = new HttpClient(httpClientHandler, true)
                {
                    BaseAddress = new Uri(_uri)
                  
                  };
                this._client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY",_privateToken);
               
            }
        }

		/// <summary>
		/// Initializes a new instance of the CoinMarketCapClient class.
		/// </summary>
		public CoinMarketCapClient()
            : this(new HttpClientHandler())
        {
        }


        #endregion

        #region ==================================================== GetTickerList ====================================================
        /// <summary>
        /// Retrieves a list of Tickers.
        /// </summary>
        /// <param name="limit">Limit the amount of Tickers.</param>
        /// <param name="convert">Convert the crypto volumes to the given Fiat currency.</param>
        /// <returns>Returns the ticker list with their volumes.</returns>
        public async Task<List<TickerEntity>> GetTickerListAsync(int limit,ConvertEnum convert)
        {
            var uri = new StringBuilder("/v1/ticker/?");
            uri.Append($"limit={limit}&");
            uri.Append($"convert={convert.ToString()}");
            var response = await _client.GetStringAsync(uri.ToString());
            var obj = JsonConvert.DeserializeObject<List<TickerEntity>>(response);
            return obj;
        }

        public async Task<CoinMarketDataRespon> GetListCyptoItemAsync()
        {
            var uri = new StringBuilder("/v1/cryptocurrency/map");
            string response = await _client.GetStringAsync(uri.ToString());

            //  JObject jObject = JObject.Parse(response);
            // var json = JsonConvert.SerializeObject(response);
            //  JToken status = jObject["status"];
            //   JToken data = jObject["data"];
            //  CoinMarketResponStatus users = status.ToObject<CoinMarketResponStatus>();
            //   List<CyproEntity> cyptoArray = data.ToObject<List<CyproEntity>>();
            // CoinMarketResponStatus obj = JsonConvert.DeserializeObject<CoinMarketResponStatus>(status.ToString());
            //  List<CyproEntity> obj2 = JsonConvert.DeserializeObject<List<CyproEntity>>(data.ToString());

            CoinMarketDataRespon obj3 = JsonConvert.DeserializeObject<CoinMarketDataRespon>(response);

            return obj3;
        }

        public async Task<IDictionary<string, CyproPriceEntity>> GetListPriceCyptoItemAsync(string listType )
        {
          
             var uri = new StringBuilder("/v1/cryptocurrency/quotes/latest?");
            uri.Append($"id={listType}");
            string response = await _client.GetStringAsync(uri.ToString());
            JObject jObject = JObject.Parse(response);
            JToken status = jObject["status"];
            JToken data = jObject["data"];
            IDictionary<string, CyproPriceEntity> obj3 = JsonConvert.DeserializeObject<IDictionary<string,CyproPriceEntity>>(data.ToString());

            return obj3;
        }


     



        /// <summary>
        /// Retrieves a list of Tickers.
        /// </summary>
        /// <param name="limit">Limit the amount of Tickers.</param>
        /// <returns>Returns the ticker list with their volumes in USD.</returns>
        public async Task<List<TickerEntity>> GetTickerListAsync(int limit) => 
            await this.GetTickerListAsync(limit, ConvertEnum.USD).ConfigureAwait(false);

        /// <summary>
        /// Retrieves a list of Tickers.
        /// </summary>
        /// <param name="convert">Convert the crypto volumes to the given Fiat currency.</param>
        /// <returns>Returns all available tickers with their volumes.</returns>
        public async Task<List<TickerEntity>> GetTickerListAsync(ConvertEnum convert) => 
            await this.GetTickerListAsync(0, convert).ConfigureAwait(false);

        /// <summary>
        /// Retrieves a list of Tickers
        /// </summary>
        /// <returns>Returns all available tickers with their volumes in USD.</returns>
        public async Task<List<TickerEntity>> GetTickerListAsync() => 
            await this.GetTickerListAsync(0, ConvertEnum.USD).ConfigureAwait(false);



        #endregion

        #region ==================================================== GetTicker ====================================================
        /// <summary>
        /// Retrieves the Ticker for given cryptoCurrency value.
        /// </summary>
        /// <param name="cryptoCurrency">The Ticker name of the desired cryptoCurrency.</param>
        /// <param name="convert">Convert the crypto volumes to the given Fiat currency.</param>
        /// <returns>Returns the ticker.</returns>
        public async Task<TickerEntity> GetTickerAsync(string cryptoCurrency, ConvertEnum convert)
        {
            StringBuilder uri = new StringBuilder();
            uri.Append($"/v1/ticker/{cryptoCurrency}/?");
            uri.Append(ConvertEnum.USD != convert ? $"convert={convert.ToString()}" : "");

            var response = await _client.GetStringAsync(uri.ToString());
            var obj = JsonConvert.DeserializeObject<List<TickerEntity>>(response);
            return obj.First();
        }

        /// <summary>
        /// Retrieves the Ticker for given cryptoCurrency value.
        /// </summary>
        /// <param name="cryptoCurrency">The Ticker name of the desired cryptoCurrency.</param>
        /// <returns>Returns the ticker in USD.</returns>
        public async Task<TickerEntity> GetTickerAsync(string cryptoCurrency) => 
            await GetTickerAsync(cryptoCurrency,ConvertEnum.USD).ConfigureAwait(false);

        #endregion

        #region ==================================================== GetlGlobalData ====================================================
        /// <summary>
        /// Retrieves the global market cap for crypto currencies.
        /// </summary>
        /// <param name="convert">Convert the crypto volumes to the given Fiat currency.</param>
        /// <returns>A GlobalDataEntity with the requested information in the given currency.</returns>
        public async Task<GlobalDataEntity> GetGlobalDataAsync(ConvertEnum convert)
        {
            StringBuilder uri = new StringBuilder($"/v1/global/?");
            uri.Append(ConvertEnum.USD != convert ? $"convert={convert.ToString()}" : "");
            var response = await _client.GetStringAsync(uri.ToString());
            var obj = JsonConvert.DeserializeObject<GlobalDataEntity>(response);
            return obj;
        }

        /// <summary>
        /// Retrieves the global market cap for crypto currencies.
        /// </summary>
        /// <returns>A GlobalDataEntity with the requested information in USD.</returns>
        public async Task<GlobalDataEntity> GetGlobalDataAsync() =>
            await GetGlobalDataAsync(ConvertEnum.USD).ConfigureAwait(false);
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to
        /// release only unmanaged resources.</param>
        internal virtual void Dispose(bool disposing)
		{
			if (!this._isDisposed)
			{
				if (disposing)
				{
					this._client?.Dispose();
				}
				this._isDisposed = true;
			}
		}

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        /// <seealso cref="M:System.IDisposable.Dispose()"/>
        public void Dispose() => this.Dispose(true);
    }
}
