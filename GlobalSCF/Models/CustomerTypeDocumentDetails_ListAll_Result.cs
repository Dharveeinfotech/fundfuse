

namespace TMP.Models
{
    using System;
    using System.Collections.Generic;

    public partial class CustomerTypeDocumentDetails_ListAll_Result
    {   public int CustomerTypeDocDetID { get; set; }
        public int CustomerTypeID { get; set; }
        public int DocumentID { get; set; }
        public bool IsActive { get; set; }
        public bool IsUserDoc { get; set; }
        public bool IsInvestor { get; set; }
        public string Status { get; set; }
        public int ProcessBy { get; set; }
        public string Attachment { get; set; }
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
        public string DocumentName { get; set; }
        public string CustomerTypeName { get; set; }
        public string CustomerTypeCode { get; set; }
        public string DocumentDesc { get; set; }
        public string DocumentNo { get; set; }
        public System.DateTime RegDate { get; set; }
        public int IssueID { get; set; }
        public int CustomerTypeDocProcessHistoryID { get; set; }
        public List<CustomerTypeDocumentDetails_ListAll_Result> lstCustomerTypeDocumentList { get; set; }
        public List<CustomerTypeDocumentDetails_ListAll_Result> lstCustomerDocDetail { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        //public string Attachment { get; set; }
    }
}
