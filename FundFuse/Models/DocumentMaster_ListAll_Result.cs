

namespace TMP.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DocumentMaster_ListAll_Result
    {
        public int DocumentID { get; set; }
        [Required]
        public string DocumentName { get; set; }

        
        [DataType(DataType.MultilineText)]
        public string DocumentDesc { get; set; }
        public string StatusUserDesc { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string Attachment { get; set; }
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
        public ICollection<CustomerMaster_ListAll_Result> lstCustomerTypeList { get; set; }
        public ICollection<CustomerTypeDocumentDetails_ListAll_Result> lstCustomerTypeIDList { get; set; }
        public ICollection<CustomerTypeDocumentDetails_ListAll_Result> lstCustomerTypeIDListHis { get; set; }
        public List<DocumentMaster_ListAll_Result> lstmDocumentMasterList { get; set; }
        public List<DocumentMaster_ListAll_Result> lstmDocumentMasterListHis { get; set; }

        [Display(Name = "Is User Document")]
        public bool IsUserDoc { get; set; }
        public bool IsInvestor { get; set; }
        public int DocumentProcessHistoryID { get; set; }
        public string ProcessIP { get; set; }
        public string FirstName { get; set; }
    }
}
