using DataModel.DataEntity;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace DataModel.DataViewModel
{
  public class CyptoItemViewModel
    {
        public CyptoItem _MainObj { get; set; }

        public CyptoItemViewModel()
        {
            _MainObj = new CyptoItem();

        }
        public CyptoItemViewModel(CyptoItem model)
        {
            _MainObj = model;

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
        public System.DateTime first_historical_data
        {
            get { return _MainObj.first_historical_data; }
            set { _MainObj.first_historical_data = value; }
        }
        public System.DateTime last_historical_data
        {
            get { return _MainObj.last_historical_data; }
            set { _MainObj.last_historical_data = value; }
        }




    }


    public class IndexCyptoManager
    {


        public IPagedList<CyptoItem> lstMainCypto { get; set; }
        public int page { get; set; }
        public int status { get; set; }
        public string letter { get; set; }

    }

    public class IndexCyptoPriceManager
    {


        public IPagedList<CyptoItemPrice> lstMainCypto { get; set; }
        public int page { get; set; }
        public int status { get; set; }
        public string letter { get; set; }

    }

}
