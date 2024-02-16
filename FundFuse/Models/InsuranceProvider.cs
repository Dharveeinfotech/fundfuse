using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class InsuranceProvider
    {
        public int InsuranceProviderID { get; set; }
        public string InsuranceProviderName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int CityID { get; set; }
        public int StateID { get; set; }
        public int CountryID { get; set; }
        public string ContactName { get; set; }
        public string CountryName { get; set; }        
        public string MobileNo { get; set; }
        public string TelNo { get; set; }
        public string EmailID { get; set; }
        public string WebSite { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? IncDate { get; set; }            
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? LicExpDate { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public int InsuranceProviderProcessHistoryID { get; set; }      
        public string InsuranceCode { get; set; }      
    }
}