using DataModel.DataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataViewModel
{
    public class DetailNewsViewModel
    {
        public ContentItem MainNew { get; set; }
        public Classification CataObj { get; set; }
        public List<ContentItem> lstSameContentItem { get; set; }
    }
    public class HomeListNewViewModel
    {
        public Classification CataObj { get; set; }
        public List<ContentItem> lstNews { get; set; }
    }
    public class IndexViewModel
    {
        public List<HomeListProductViewModel> HomeProduct { get; set; }
        public List<ProductItemViewModel> ListNewProduct { get; set; }
        public List<ProductItemViewModel> ListViewProduct { get; set; }
        public List<ContentItem> lstNews { get; set; }
        public List<ContentItem> lstIdeas { get; set; }
        public List<ContentItem> lstParner { get; set; }
        public List<ContentItem> lstWhy { get; set; }

        // public List<HomeSliderPartialViewModel> HomeSlider { get; set; }
    }
    public class HomeListProductViewModel
    {
        public Classification CataObj { get; set; }
        public List<Product> lstProduct { get; set; }
    }

    public class ProductItemViewModel
    {
        public Product ObiProduct { get; set; }
        public Classification Obicate { get; set; }
    }
    public class RightSliderBarViewModel
    {
        public List<SubMenuViewModels> CataList { get; set; }
        public List<ProductItemViewModel> LstProductViewMore { get; set; }
        public List<ProductItemViewModel> LstProductNews { get; set; }
        public List<ProductItemViewModel> ListDayProduct { get; set; }
        public List<ContentItem> lstPolixyNews { get; set; }
        public List<ContentItem> lstIdea { get; set; }
        public List<HomeSliderPartialViewModel> HomeSlider { get; set; }

    }

    public class ListNewViewModel
    {
        public Classification CataObj { get; set; }
        public List<ContentItem> lstNews { get; set; }
    }
    public class DetailNewViewModel
    {
        public ContentItem MainNews { get; set; }
        public Classification CataObj { get; set; }
        public List<ContentItem> lstSameNews { get; set; }

    }
}
