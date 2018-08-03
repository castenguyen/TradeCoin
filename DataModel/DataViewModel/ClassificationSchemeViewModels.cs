using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataModel.DataEntity;
namespace DataModel.DataViewModel
{
    public class ClassificationSchemeViewModels
    {

        public ClassificationScheme _ModelObj { get; set; }
        public ClassificationSchemeViewModels()
        {
            _ModelObj = new ClassificationScheme();

        }
        public ClassificationSchemeViewModels(ClassificationScheme model)
        {
            _ModelObj = model;

        }


        public int ClassificationSchemeId
        {
            get { return _ModelObj.ClassificationSchemeId; }
            set { _ModelObj.ClassificationSchemeId = value; }
        }

        [Required]
        [Display(Name = "ClassificationSchemeNM")]
        public string ClassificationSchemeNM
        {
            get { return _ModelObj.ClassificationSchemeNM; }
            set { _ModelObj.ClassificationSchemeNM = value; }
        }

        [Required]
        [Display(Name = "ClassificationSchemeDesc")]
        public string ClassificationSchemeDesc
        {
            get { return _ModelObj.ClassificationSchemeDesc; }
            set { _ModelObj.ClassificationSchemeDesc = value; }
        }



        public Nullable<System.DateTime> CrtdDT
        {
            get { return _ModelObj.CrtdDT; }
            set { _ModelObj.CrtdDT = value; }
        }
        public Nullable<long> CrtdUID
        {
            get { return _ModelObj.CrtdUID; }
            set { _ModelObj.CrtdUID = value; }
        }
        public Nullable<System.DateTime> LstModDT
        {
            get { return _ModelObj.LstModDT; }
            set { _ModelObj.LstModDT = value; }
        }
        public Nullable<long> LstModUID
        {
            get { return _ModelObj.LstModUID; }
            set { _ModelObj.LstModUID = value; }
        }

        public Nullable<byte> IsHeirarchy
        {
            get { return _ModelObj.IsHeirarchy; }
            set { _ModelObj.IsHeirarchy = value; }
        }


        [Required]
        [Display(Name = "SchemeCD")]
        public string SchemeCD
        {
            get { return _ModelObj.SchemeCD; }
            set { _ModelObj.SchemeCD = value; }
        }

        [Required]
        [Display(Name = "ShortNM")]
        public string ShortNM
        {
            get { return _ModelObj.ShortNM; }
            set { _ModelObj.ShortNM = value; }
        }

        [Required]
        [Display(Name = "ShortNM")]
        public Nullable<byte> IsSystem
        {
            get { return _ModelObj.IsSystem; }
            set { _ModelObj.IsSystem = value; }
        }

        [Display(Name = "IsSystem")]
        public bool IsSystemVM { get; set; }

        [Display(Name = "IsSystem")]
        public int? ParentClassificationSchemeId { get; set; }

        [Display(Name = "ParentList")]
        public SelectList ParentList { get; set; }


    }
}
