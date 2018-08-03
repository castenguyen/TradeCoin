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
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ManageUserViewModel
    {
        public User _User { get; set; }
        public ManageUserViewModel()
        {
            _User = new User();

        }
        public ManageUserViewModel(User model)
        {
            _User = model;

        }

        [Required]
        [Display(Name = "FullName")]
        public string FullName
        {
            get { return _User.FullName; }
            set { _User.FullName = value; }
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email
        {
            get { return _User.EMail; }
            set { _User.EMail = value; }
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<Userhist> Myhistory { get; set; }
    }

    public class LoginViewModel
    {
   

        [Required]
        [Display(Name = "Tên truy cập")]
        public string Email { get; set; }

    
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Không được bỏ trống email")]
        [EmailAddress(ErrorMessage = "Phải đúng định dãng email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống ô họ tên người đăng ký")]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống ô mật khẩu")]
        [RegularExpression("^[\\w]{2,100}$", ErrorMessage = "Mật khẩu chỉ chứa các ký tự chữ cái hoa thường và số")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống ô lặp lại mật khẩu")]
        [RegularExpression("^[\\w]{2,100}$", ErrorMessage = "Mật khẩu chỉ chứa các ký tự chữ cái hoa thường và số")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Chọn giới tính")]
        public int Gender { get; set; }

        public bool RegisterSendEmail { get; set; } //dang ky nhan ban tin Allunee

        public bool AcceptPolicyAllunee { get; set; } // dong y voi chinh sach cua allunee

        [RegularExpression("(^[0]{1}[1]{1}[0-9]{9}$)|(^[0]{1}[2-9]{1}[0-9]{8}$)", ErrorMessage = "Phải là số điện thoại Việt Nam")]
        public string Phone { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RoleViewModel
    {
        public Role _Role { get; set; }

        public RoleViewModel()
        {
            _Role = new Role();

        }
        public RoleViewModel(Role model)
        {
            _Role = model;

        }
        [Required]
        [Display(Name = "Name")]
        public long Id
        {
            get { return _Role.Id; }
            set { _Role.Id = value; }
        }

        [Required]
        [Display(Name = "Name")]
        public string Name
        {
            get { return _Role.Name; }
            set { _Role.Name = value; }
        }

        [Display(Name = "RoleDes")]
        public string RoleDes
        {
            get { return _Role.RoleDes; }
            set { _Role.RoleDes = value; }
        }

        [Display(Name = "IsGroup")]
        public bool IsGroup
        {
            get { return _Role.IsGroup; }
            set { _Role.IsGroup = value; }
        }
     


        public SelectList RoleList { get; set; }
        public List<Role> ListRole { get; set; }


        public long[] lstPermission { get; set; }
   
    }

    public class UserRoleViewModel
    {

        [Display(Name = "Danh sách Role")]
        public List<Role> LstRole { get; set; }

        [Display(Name = "Danh sách Role")]
        public string RoleName { get; set; }
        public int Page { get; set; }
        public string letter { get; set; }
        [Display(Name = "Danh sách User")]
        public long UserId { get; set; }

        public IPagedList<User> LstAllUser { get; set; }

    }

    //TRI - 20160707 This class is used to contains the user information, include a list of roles available
    public class UserAndRoles
    {
        [Required]
        [Display(Name = "Object User")]
        public User ObjUser { get; set; }

        [Required]
        [Display(Name = "Danh sách quyền hiện tại")]
        public IEnumerable<string> LstCurPermission { get; set; }

        [Display(Name = "Tất cả các quyền")]
        public SelectList LstAllPermission { get; set; }

        public string RoleName { get; set; }

        public SelectList LstPackages { get; set; }

        public string AlertMessage { get; set; }


        [Required]
        [Display(Name = "Danh sách loại tài khoản hiện tại")]
        public IEnumerable<string> LstCurUserType { get; set; }

        [Display(Name = "Tất cả các loại tài khoản")]
        public SelectList LstAllUserType { get; set; }

    }

    public class AlertPageViewModel
    {
        public string AlertString { get; set; }
        public string AlertLink { get; set; }
    }
    public class ProfileViewModel
    {
        public User _ModelObj { get; set; }
        public ProfileViewModel()
        {
            _ModelObj = new User();

        }
        public ProfileViewModel(User model)
        {
            _ModelObj = model;

        }

        public long Id
        {
            get { return _ModelObj.Id; }
            set { _ModelObj.Id = value; }
        }

        [Display(Name = "Login")]
        public string Login
        {
            get { return _ModelObj.Login; }
            set { _ModelObj.Login = value; }
        }

        [Display(Name = "EMail")]
        public string EMail
        {
            get { return _ModelObj.EMail; }
            set { _ModelObj.EMail = value; }
        }

        [Display(Name = "PhoneNumber")]
        public string PhoneNumber
        {
            get { return _ModelObj.PhoneNumber; }
            set { _ModelObj.PhoneNumber = value; }
        }

        [Display(Name = "FullName")]
        public string FullName
        {
            get { return _ModelObj.FullName; }
            set { _ModelObj.FullName = value; }
        }



        [Display(Name = "FolerName")]
        public string FolerName
        {
            get { return _ModelObj.FolerName; }
            set { _ModelObj.FolerName = value; }
        }

        [Display(Name = "GenderId")]
        public Nullable<int> GenderId
        {
            get { return _ModelObj.GenderId; }
            set { _ModelObj.GenderId = value; }
        }

        [Display(Name = "GenderName")]
        public string GenderName
        {
            get { return _ModelObj.GenderName; }
            set { _ModelObj.GenderName = value; }
        }

        [Display(Name = "Ngày Sinh")]
        public Nullable<System.DateTime> BirthDay
        {
            get { return _ModelObj.BirthDay; }
            set { _ModelObj.BirthDay = value; }
        }

     


        [Display(Name = "Giới Tính")]
        public SelectList GenderList { get; set; }

        [Display(Name = "Công Ty")]
        public SelectList CompanyList { get; set; }

        [Display(Name = "Công Ty")]
        public string ImgUrl { get; set; }
        [Display(Name = "Money")]
        public float Money { get; set; }

        public System.Web.HttpPostedFileBase MyFile { get; set; }

        public string CroppedImagePath { get; set; }

        //[Display(Name = "Hình đại diện")]
        //public long Default_files { get; set; }
    }

    public class ConfirmDeleteViewModel
    {
        public string controller { get; set; }
        public string action { get; set; }
        public long id { get; set; }
    }



    #region FrontEnd

    public class UserInfo
    {
        public string ConnectionId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string UserGroup { get; set; }
        public int freeflag { get; set; }
        //if tpflag==2 ==> User Admin
        //if tpflag==0 ==> User Member
        //if tpflag==1 ==> Admin
        public int tpflag { get; set; }
        public int waitflag { get; set; }
        public long UserID { get; set; }
        public long AdminID { get; set; }
        public int AdminCode { get; set; }

    }
    public class MessageInfo
    {
        public string UserName { get; set; }

        public string Message { get; set; }

        public string UserGroup { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string MsgDate { get; set; }
    }

    #endregion End frontend

}
