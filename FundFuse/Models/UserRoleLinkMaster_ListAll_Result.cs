

namespace TMP.Models
{
    using System;
    
    public partial class UserRoleLinkMaster_ListAll_Result
    {
        public int UserRoleLinkID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
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
        public string FirstName { get; set; }
        public string LoginName { get; set; }
        public string RoleName { get; set; }
        public string StatusDesc { get; set; }
        public int UserRoleLinkHistoryID { get; set; }
        public string ProcessIP { get; set; }
        public string LastName { get; set; }
    }
}
