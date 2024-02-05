using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class MenuRights
    {
        public int MenuRightsID { get; set; }
        public int MenuID { get; set; }
        public int RoleID { get; set; }
        public bool IsMaker { get; set; }
        public bool IsChecker { get; set; }
        public bool IsApprover { get; set; }
        public bool IsView { get; set; }
        public bool IsViewer { get; set; }


        public bool IsMakerRole { get; set; }
        public bool IsCheckerRole { get; set; }
        public bool IsApproverRole { get; set; }        
        public bool IsViewerRole { get; set; }


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
        public string MenuName { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public string StatusDesc { get; set; }
        public int ParentMenuID { get; set; }
        public int MenuRightsProcessHistoryID { get; set; }   
        public string ProcessIP { get; set; }
        public string MenuType { get; set; }
        public bool IsChild { get; set; }
        public string CustomerTypeIDs { get; set; }
        public string GetCustomerTypeIDs { get; set; }
    }
}