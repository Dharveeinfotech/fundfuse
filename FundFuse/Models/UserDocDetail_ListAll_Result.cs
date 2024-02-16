
namespace TMP.Models
{
    using System;
    
    public partial class UserDocDetail_ListAll_Result
    {
        public int UserDocDetID { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string DocName { get; set; }
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
        public string StatusDesc { get; set; }
        public int UserDocDetProcessHistoryID { get; set; }
        public string ProcessIP { get; set; }
    }
}
