//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TMP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class mRoleMaster
    {
        public int RoleID { get; set; }
        [Required]
        public string RoleName { get; set; }     
        public string RoleDesc { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
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
        public int RoleProcessHistoryID { get; set; }      
        public string ProcessIP { get; set; }
        public string StatusDesc { get; set; }
        public string StatusUserDesc { get; set; }
        public string Checked { get; set; }
        public string Approved { get; set; }
        public string MenuType { get; set; }
        public bool IsChild { get; set; }        
        public bool IsMaker { get; set; }
        public bool IsMakerRole { get; set; }
        public bool IsChecker { get; set; }
        public bool IsCheckerRole { get; set; }
        public bool IsApprover { get; set; }
        public bool IsApproverRole { get; set; }
        public bool IsViewer { get; set; }
        public bool IsViewerRole { get; set; }
        public bool IsView { get; set; }
        public List<MenuMaster> lstMenuMasterAdd { get; set; }
        public string CustomerTypeIDs { get; set; }
        public string GetCustomerTypeIDs { get; set; }

        //public List<mRoleMaster> lstRoleMaster { get; set; }
        //public List<mRoleMaster> lstRoleMasterlstallbind { get; set; }
        //public List<mRoleMaster> lstMenuRights { get; set; }
        //public List<mRoleMaster> lstMenuRightslstallbind { get; set; }
        //public List<mRoleMaster> lstRoleMasterListBind { get; set; }
        //public List<mRoleMaster> lstMenuRightsListBind { get; set; }

        public List<RoleMaster_ListAll_Result> lstRoleMaster { get; set; }
        public List<RoleMasterProcessHistory_ListAllBind_Result> lstRoleMasterlstallbind { get; set; }
        public List<MenuRights> lstMenuRights { get; set; }
        public List<MenuRights> lstMenuRightslstallbind { get; set; }
        public List<RoleMasterProcessHistory_ListAllBind_Result> lstRoleMasterListBind { get; set; }
        public List<MenuRights> lstMenuRightsListBind { get; set; }

        public class RoleMaster_ListAll_Result
        {
            public int RoleID { get; set; }
            public string RoleName { get; set; }
            public string RoleDesc { get; set; }
            public bool IsActive { get; set; }
            public string Status { get; set; }
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
            public int RoleProcessHistoryID { get; set; }
            public string ProcessIP { get; set; }
            public string StatusDesc { get; set; }
            public string StatusUserDesc { get; set; }
            public string Checked { get; set; }
            public string Approved { get; set; }
            public string CustomerTypeIDs { get; set; }
            public string GetCustomerTypeIDs { get; set; }
        }

        public class RoleMasterProcessHistory_ListAllBind_Result
        {
            public int RoleID { get; set; }
            [Required]
            public string RoleName { get; set; }
            public string RoleDesc { get; set; }
            public bool IsActive { get; set; }
            public string Status { get; set; }
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
            public int RoleProcessHistoryID { get; set; }
            public string ProcessIP { get; set; }
            public string StatusDesc { get; set; }
            public string StatusUserDesc { get; set; }
            public string Checked { get; set; }
            public string Approved { get; set; }
            public string CustomerTypeIDs { get; set; }
            public string GetCustomerTypeIDs { get; set; }
        }
    }
}
