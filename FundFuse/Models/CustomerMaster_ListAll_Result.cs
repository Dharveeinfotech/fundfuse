

namespace TMP.Models
{
    using System;
    using System.Collections.Generic;
    public partial class CustomerMaster_ListAll_Result
    {
        public string Attachment { get; set; }
        public string CompanyName { get; set; }
        public int CustomerTypeID { get; set; }
        public int CustomerID { get; set; }        
        public string CompanyShortName { get; set; }
        public string TradeLicenceNo { get; set; }
        public System.DateTime LicenceExpDate { get; set; }
        public System.DateTime IncDate { get; set; }
        public int IndustryID { get; set; }
        public string IndustryName { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
        public int CommandID { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateIP { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UpdateIP { get; set; }
        public string StatusDesc { get; set; }
        public int SubIndustryID { get; set; }
        public int IncCountryID { get; set; }
        public byte CompanyTypeID { get; set; }
        public string SubIndustryName { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CustomerTypeName { get; set; }
        public string CompanyTypeName { get; set; }
        public string Status { get; set; }
        public int CustomerContDetID { get; set; }
        public string ChgRemark { get; set; }
        public string ProcessRemark { get; set; }
        public bool IsInvestor { get; set; }
        public string UserName { get; set; }
        
        public string CompanyLogo { get; set; }
        public string CompanyBrief { get; set; }
        public List<CustomerMaster_ListAll_Result> lstCustomerMaster { get; set; }

        public int CustomerProcessHistoryID { get; set; }
     
        public int ProcessBy { get; set; }
        public System.DateTime ProcessDate { get; set; }
        public string ProcessIP { get; set; }
        public string FirstName { get; set; }

    }
}
