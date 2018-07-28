using DataModel.DataEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataModel.DataViewModel
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage ="Không được bỏ trống ô họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Không được bỏ ô trường địa chỉ")]
        public string UserAdress { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống")]
        [RegularExpression("^[0]{1}[1-9]{1}[0-9]{8,9}$",ErrorMessage ="Phải đúng số điện thoại Việt Nam")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống ô email")]
        [EmailAddress(ErrorMessage ="Phải đúng là định dạng email")]
        public string EMail { get; set; }
        
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống ô tỉnh/thành phố")]
        [RegularExpression("\\d+",ErrorMessage = "Không được bỏ trống ô tỉnh/thành phố")]
        public int IdProvice { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống ô quận/huyện")]
        [RegularExpression("\\d+", ErrorMessage = "Không được bỏ trống ô quận/huyện")]
        public int IdDistrict { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống ô phường/xã")]
        [RegularExpression("\\d+", ErrorMessage = "Không được bỏ trống ô phường/xã")]
        public int IdWard { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống phương thức vận chuyển")]
        [RegularExpression("\\d+", ErrorMessage = "Là số id của phương thức vận chuyển")]
        public int IdShippingMethod { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống phương thức thanh toán")]
        [RegularExpression("\\d+", ErrorMessage = "Là số id của phương thức thanh toán")]
        public int IdPaymentMethod { get; set; }

        public List<SelectListItem> ListItemCity { get; set; }
        public List<SelectListItem> ListItemDistrict { get; set; }
        public List<SelectListItem> ListItemWard { get; set; }
    }
}
