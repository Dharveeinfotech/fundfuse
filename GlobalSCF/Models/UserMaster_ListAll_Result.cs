namespace TMP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class UserMaster_ListAll_Result
    {
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string LoginName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int CustomerID { get; set; }
        public int CustomerShareHoldID { get; set; }
        public string RoleIDs { get; set; }
        public string GetRoleIDs { get; set; }
        
        public int CustomerTempID { get; set; }
        public int CustomerTypeID { get; set; }        
        public string CompanyName { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string OtherDesignation { get; set; }
        public string ServLength { get; set; }
        
        [Required]
        public string EmailID { get; set; }
        public string StaffID { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string Gender { get; set; }
        public string LocCountryName { get; set; }
        public int LocCountryID { get; set; }
        public string StatusUserDesc { get; set; }
        public bool IsActive { get; set; }
        public string ServicePeriod { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSMS { get; set; }
        public string Status { get; set; }
        public string StatusDesc { get; set; }
        public int ProcessBy { get; set; }
        public System.DateTime ProcessDate { get; set; }
        public string ProcessRemark { get; set; }
        public int CommandID { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateIP { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateIP { get; set; }
        public int IdType { get; set; }
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string IdNumber { get; set; }
        public List<UserRoleLinkMaster_ListAll_Result> lstUserRoleLinkMaster_ListAll_Result { get; set; }
        public List<MenuRights> lstMenuRights { get; set; }
        public UserRights_Result lstUserRight { get; set; }
        public List<UserRoleLinkMaster_ListAll_Result> lstUserRoleLink { get; set; }
        public bool IsDelete { get; set; }
        public string Photo { get; set; }
        public DateTime PwdExpiryDate { get; set; }
        public bool IsAgree { get; set; }
        public bool IsVerified { get; set; }
        public string Email { get; set; }
        public string ActivationLink { get; set; }
        [Required]
        public string OTP { get; set; }
        public bool RememberMe { get; set; }
        public Nullable<bool> AlreadyLogin { get; set; }
        public int UserHistoryID { get; set; }
        public string ProcessIP { get; set; }

        #region User Identity Detail Property
        public int UserIdenID { get; set; }
       
        public string DocNo { get; set; }
        public string Attach { get; set; }
     
        public bool IsNoRequired { get; set; }
        public int UserIdenHistoryID { get; set; }
        public int IsLoginMailSend { get; set; }
        #endregion

        [Required]
        [DataType(DataType.Password)]
        [StringLength(10, ErrorMessage = "The maximun length should be 10 characters long.")]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The maximun length should be 10 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [StringLength(10, ErrorMessage = "The maximun length should be 10 characters long.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string OPTNumber { get; set; }
        public bool Iagree { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public bool IsSendUserMail { get; set; }
        public string RoleNames { get; set; }
        public string CurrentSessionID { get; set; }
        public string Request { get; set; }
        public string LoginDevice { get; set; }
        public string SelectedRole { get; set; }
    }
    public class mUserDeptDetail
    {
        public int UserDeptDetID { get; set; }
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
        public string Status { get; set; }
        public int CreateBy { get; set; }
        public string CreateIP { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public int UpdateBy { get; set; }
        public string UpdateIP { get; set; }
        public int UserDeptDetHistoryID { get; set; }
        public string ProcessRemark { get; set; }
        public int ProcessBy { get; set; }
        public string ProcessIP { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteIP { get; set; }
        public bool IsActive { get; set; }
        public string DepartmentName { get; set; }
    }
    public class UserLoginHistory_ListAll_Result
    {
        public int UserLoginHistoryID { get; set; }
        public int UserID { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public System.DateTime LoginDateTime { get; set; }
        public bool IsResetPwd { get; set; }
        public string OTP { get; set; }
    }
}
