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
    public class ContentTagViewModels
    {

        public ContentTag _ModelObj { get; set; }
        public ContentTagViewModels()
        {
            _ModelObj = new ContentTag();

        }
        public ContentTagViewModels(ContentTag model)
        {
            _ModelObj = model;

        }

        public long ObjcontentId { get; set; }
        public long TagId { get; set; }
        public Nullable<System.DateTime> CrtdDT { get; set; }
        public long ObjTypeId { get; set; }

        public virtual Tag Tag { get; set; }


    }
    public class TagViewModels
    {
          public Tag _MainModel { get; set; }
          public TagViewModels()
        {
            _MainModel = new Tag();

        }
          public TagViewModels(Tag model)
        {
            _MainModel = model;

        }
          [Required]
          [Display(Name = "TagId")]
          public long TagId
          {
              get { return _MainModel.TagId; }
              set { _MainModel.TagId = value; }
          }

          [Required]
          [MaxLength(20)]
          [Display(Name = "TagNM")]
          public string TagNM
          {
              get { return _MainModel.TagNM; }
              set { _MainModel.TagNM = value; }
          }

          [Display(Name = "TagList")]
          public List<Tag> TagList { get; set; }

    
    }

}
