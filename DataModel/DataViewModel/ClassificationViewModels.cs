using DataModel.DataEntity;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace DataModel.DataViewModel
{
    public class ClassifiIndexViewModels
    {
        public IPagedList<Classification> MainLst { get; set; }
        public int SchemeId { get; set; }
        public int classId { get; set; }
        public int page { get; set; }

    }

    public class ClassificationViewModels
    {
        public Classification _ModelObj { get; set; }
        public ClassificationViewModels()
        {
            _ModelObj = new Classification();

        }
        public ClassificationViewModels(Classification model)
        {
            _ModelObj = model;

        }

        public int ClassificationId
        {
            get { return _ModelObj.ClassificationId; }
            set { _ModelObj.ClassificationId = value; }
        }

        public Nullable<int> ParentClassificationId
        {
            get { return _ModelObj.ParentClassificationId; }
            set { _ModelObj.ParentClassificationId = value; }
        }

        [Required]
        [Display(Name = "ClassificationNM")]
        public string ClassificationNM
        {
            get { return _ModelObj.ClassificationNM; }
            set { _ModelObj.ClassificationNM = value; }
        }

        [Required]
        [Display(Name = "ClassificationDesc")]
        public string ClassificationDesc
        {
            get { return _ModelObj.ClassificationDesc; }
            set { _ModelObj.ClassificationDesc = value; }
        }


        [Display(Name = "ClassificationMetakeyword")]
        public string ClassificationMetakeyword
        {
            get { return _ModelObj.ClassificationMetakeyword; }
            set { _ModelObj.ClassificationMetakeyword = value; }
        }


        [Display(Name = "ClassificationMetaDes")]
        public string ClassificationMetaDes
        {
            get { return _ModelObj.ClassificationMetaDes; }
            set { _ModelObj.ClassificationMetaDes = value; }
        }

        [Required]
        [Display(Name = "ClassificationSchemeId")]
        public int ClassificationSchemeId
        {
            get { return _ModelObj.ClassificationSchemeId; }
            set { _ModelObj.ClassificationSchemeId = value; }
        }

        [Required]
        [MaxLength(20)]
        [Display(Name = "ClassificationCD")]
        public string ClassificationCD
        {
            get { return _ModelObj.ClassificationCD; }
            set { _ModelObj.ClassificationCD = value; }
        }

        [Required]
        [Display(Name = "FriendlyURL")]
        public string FriendlyURL
        {
            get { return _ModelObj.FriendlyURL; }
            set { _ModelObj.FriendlyURL = value; }
        }


        [Display(Name = "IsEnabled")]
        public byte IsEnabled
        {
            get
            {

                if (this.IsEnabledbool == true)
                {
                    _ModelObj.IsEnabled = 1;
                    return 1;
                }

                else
                {
                    _ModelObj.IsEnabled = 0;
                    return 0;
                };
            }
            set
            {
                if (this.IsEnabledbool == true)
                {
                    _ModelObj.IsEnabled = 1;
                }

                else
                {
                    _ModelObj.IsEnabled = 0;
                };
            }
        }

        [Display(Name = "IsEnabledbool")]
        public bool IsEnabledbool { get; set; }

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

        [Required]
        [Display(Name = "ShortNM")]
        public string ShortNM
        {
            get { return _ModelObj.ShortNM; }
            set { _ModelObj.ShortNM = value; }
        }
        public Nullable<int> DisplayOrder
        {
            get { return _ModelObj.DisplayOrder; }
            set { _ModelObj.DisplayOrder = value; }
        }


        [Display(Name = "ParentList")]
        public SelectList ParentList { get; set; }

        [Display(Name = "SchemeList")]
        public SelectList SchemeList { get; set; }

        [Required]
        [Display(Name = "Hình đại diện")]
        public long ImgdefaultId { get; set; }


    }

    public class SubMenuViewModels
    {
        public Classification Parent { get; set; }
        public List<Classification> lstChild { get; set; }
        public List<Product> lstPromotionProduct { get; set; }

    }

    public class BlockProductMenuViewModels
    {
        public BlockClassMenu Parent { get; set; }
        public List<BlockChildProductMenu> lstChild { get; set; }
        public List<Product> lstPromotionProduct { get; set; }
    }
    public class NavMenuViewModels
    {
        public List<BlockProductMenuViewModels> LstMenu { get; set; }
        public List<BlockClassMenu> LstPromotionMenu { get; set; }
    
    }

    public class BlockClassMenu
    {
        public string ClassName { get; set; }
        public int ParentClassId { get; set; }
        public int ClassId { get; set; }
        public string ClassFriendly { get; set; }

    }

    public class BlockChildProductMenu
    {
        public BlockClassMenu Parent { get; set; }
        public List<BlockClassMenu> lstChild { get; set; }
    }

    public class SelectListObj
    {
        public long value { get; set; }
        public string text { get; set; }

    }

    public class MainMenuViewModels
    {
        public ProfileViewModel User { get; set; }
        public List<SubMenuViewModels> ProductCate { get; set; }
        public List<SubMenuViewModels> NewCate { get; set; }

        public long CountSaveFavoriteProduct { get; set; }
    }
    public class PageInforDetail
    {
        public List<Classification> lstPageInfor { get; set; }
        public ContentItem PageContent { get; set; }
        public Classification PageTitle { get; set; }
    }

    public class BreadcrumbObj
    {
        public string href { get; set; }
        public string text { get; set; }
    }

}
