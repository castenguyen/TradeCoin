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
    public class DisplayTypeViewModels
    {
        public DisplayContent Mainmodel { get; set; }
        public DisplayTypeViewModels()
        {
            Mainmodel = new DisplayContent();
        }
        public DisplayTypeViewModels(DisplayContent model)
        {
            Mainmodel = model;
        }
        public string ContentName { get; set; }

    }
    public class CreateDisplayTypeViewModel
    {
        [Required]
        [Display(Name = "Sản Phẩm")]
        public long ProductId { get; set; }

        [Required]
        [Display(Name = "Bài Viết")]
        public long ContentId { get; set; }

        [Required]
        [Display(Name = "Banner")]
        public long BannerId { get; set; }

        [Required]
        [Display(Name = "Vùng Hiển thị")]
        public int DisplayTypeId { get; set; }

        [Required]
        [Display(Name = "Đối Tượng")]
        public int ObjTypeId { get; set; }

        [Required]
        [Display(Name = "Thời gian")]
        public string Datetime { get; set; }


        public SelectList LstDisplayType { get; set; }
        public SelectList LstProduct { get; set; }
        public SelectList LstContent { get; set; }
        public SelectList LstBanner { get; set; }
        public List<DisplayTypeViewModels> LstDisplayContent { get; set; }

     
    }


}
