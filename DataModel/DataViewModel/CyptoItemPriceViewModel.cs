using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;
using PagedList;
namespace DataModel.DataViewModel
{
    public class CyptoItemPriceViewModel
    {
        public CyptoItemPrice _MainObj { get; set; }
        public CyptoItemPriceViewModel()
        {
            _MainObj = new CyptoItemPrice();

        }
        public CyptoItemPriceViewModel(CyptoItemPrice model)
        {
            _MainObj = model;

        }
        public long CyptoItemPriceId
        {
            get { return _MainObj.CyptoItemPriceId; }
            set { _MainObj.CyptoItemPriceId = value; }
        }
        public System.DateTime CyptoItemPriceUpdate
        {
            get { return _MainObj.CyptoItemPriceUpdate; }
            set { _MainObj.CyptoItemPriceUpdate = value; }
        }
        public long id
        {
            get { return _MainObj.id; }
            set { _MainObj.id = value; }
        }
        public string name
        {
            get { return _MainObj.name; }
            set { _MainObj.name = value; }
        }
        public string symbol
        {
            get { return _MainObj.symbol; }
            set { _MainObj.symbol = value; }
        }
        public string slug
        {
            get { return _MainObj.slug; }
            set { _MainObj.slug = value; }
        }
        public bool is_active
        {
            get { return _MainObj.is_active; }
            set { _MainObj.is_active = value; }
        }
        public Nullable<System.DateTime> first_historical_data
        {
            get { return _MainObj.first_historical_data; }
            set { _MainObj.first_historical_data = value; }
        }
        public Nullable<System.DateTime> last_historical_data
        {
            get { return _MainObj.last_historical_data; }
            set { _MainObj.last_historical_data = value; }
        }
        public Nullable<int> cmc_rank
        {
            get { return _MainObj.cmc_rank; }
            set { _MainObj.cmc_rank = value; }
        }
        public Nullable<int> num_market_pairs
        {
            get { return _MainObj.num_market_pairs; }
            set { _MainObj.num_market_pairs = value; }
        }
        public Nullable<long> circulating_supply
        {
            get { return _MainObj.circulating_supply; }
            set { _MainObj.circulating_supply = value; }
        }
        public Nullable<long> total_supply
        {
            get { return _MainObj.total_supply; }
            set { _MainObj.total_supply = value; }
        }
        public Nullable<long> max_supply
        {
            get { return _MainObj.max_supply; }
            set { _MainObj.max_supply = value; }
        }
        public System.DateTime last_updated
        {
            get { return _MainObj.last_updated; }
            set { _MainObj.last_updated = value; }
        }
        public double USD_price
        {
            get { return _MainObj.USD_price; }
            set { _MainObj.USD_price = value; }
        }
        public Nullable<double> USD_volume_24h
        {
            get { return _MainObj.USD_volume_24h; }
            set { _MainObj.USD_volume_24h = value; }
        }
        public Nullable<double> USD_percent_change_1h
        {
            get { return _MainObj.USD_percent_change_1h; }
            set { _MainObj.USD_percent_change_1h = value; }
        }
        public Nullable<double> USD_percent_change_7d
        {
            get { return _MainObj.USD_percent_change_7d; }
            set { _MainObj.USD_percent_change_7d = value; }
        }
        public Nullable<double> USD_market_cap
        {
            get { return _MainObj.USD_market_cap; }
            set { _MainObj.USD_market_cap = value; }
        }
        public Nullable<double> BTC_price
        {
            get { return _MainObj.BTC_price; }
            set { _MainObj.BTC_price = value; }
        }
        public Nullable<double> BTC_volume_24h
        {
            get { return _MainObj.BTC_volume_24h; }
            set { _MainObj.BTC_volume_24h = value; }
        }
        public Nullable<double> BTC_percent_change_1h
        {
            get { return _MainObj.BTC_percent_change_1h; }
            set { _MainObj.BTC_percent_change_1h = value; }
        }
        public Nullable<double> BTC_percent_change_7d
        {
            get { return _MainObj.BTC_percent_change_7d; }
            set { _MainObj.BTC_percent_change_7d = value; }
        }
        public Nullable<double> BTC_market_cap
        {
            get { return _MainObj.BTC_market_cap; }
            set { _MainObj.BTC_market_cap = value; }
        }
    }
}
