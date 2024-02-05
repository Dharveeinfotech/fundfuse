
namespace TMP.Models
{
    using System;
    
    public partial class PageMaster_ListAll_Result
    {
        public int PageID { get; set; }
        public int MenuID { get; set; }
        public string PageName { get; set; }
        public string PageURL { get; set; }
        //public bool IsActive { get; set; }
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
        public string MenuName { get; set; }
        public int PageProcessHistoryID { get; set; }
        public string ProcessIP { get; set; }
    }
}
