using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class ServiceProviderModel
    {
        public int CustomerID { get; set; }
        public int GetCityID { get; set; }        
        public int CustomerTempID { get; set; }
        public string CompanyName { get; set; }
        public string CustomerTypeName { get; set; }
        public string CompanyTypeName { get; set; }        
        public string DesignationName { get; set; }        
        public string IncCountryName { get; set; }
        public string CountryName { get; set; }        
        public string StatusDisplay { get; set; }        
        public string TradingName { get; set; }
        public string ContactFullName { get; set; }        
        public string ContactFName { get; set; }
        public string ContactMName { get; set; }
        public string ContactLName { get; set; }
        public int CustomerTypeID { get; set; }
        public string CustomerTypeIDs { get; set; }        
        public string ProgramType { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public int NationalityID { get; set; }
        public int IndustryID { get; set; }
        public string OtherIndustry { get; set; }
        public int SubIndustryID { get; set; }
        public string OtherSubIndustry { get; set; }
        public string Website { get; set; }
        public int DesignationID { get; set; }
        public string OtherDesignation { get; set; }
        public string TelNo { get; set; }
        public string OtherCity { get; set; }
        [Required]
        public string Gender { get; set; }
        public string LocCountryName { get; set; }
        public int LocCountryID { get; set; }
        public string TelNoInfo { get; set; }
        public int CustomerHistoryID { get; set; }
        public string CustomerCode { get; set; }
        public string Status { get; set; }
        public string ProcessRemark { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public int CustomerInfoID { get; set; }
        public int CompanyTypeID { get; set; }
        public string OtherCompanyType { get; set; }
        
        public string TradeLicNo { get; set; }
        public int IncCountryID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? IncDate { get; set; }        
        public DateTime IncDate1 { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? LicExpDate { get; set; }
        public DateTime LicExpDate1 { get; set; }
        public string IssuingAuth { get; set; }
        public string AbtBusiness { get; set; }
        public string BusActivity { get; set; }
        public string EmpCount { get; set; }
        public Decimal AnnTurnOver { get; set; }
        public Decimal SaleVolume { get; set; }
        public Decimal NetProfit { get; set; }
        public string SaleInvTerms { get; set; }
        public string OtherSaleInvTerms { get; set; }        
        public bool IsCompPubList { get; set; }
        public string ExchangeName { get; set; }
        public string TickerCode { get; set; }
        public bool IsCompGrpPart { get; set; }
        public string GroupName { get; set; }
        public string GroupWebsite { get; set; }
        public bool IsCompFinServ { get; set; }
        public string RegulatorName { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int StateID { get; set; }        
        public string ObligorIDs { get; set; }
        public string UpdTradeLic { get; set; }
        public string UpdBusProfile { get; set; }
        public string UpdAuditFin { get; set; }
        public string UpdInHouseFin { get; set; }
        public string UpdAgingRpt { get; set; }
        public int CustomerInfoHistoryID { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string POBox { get; set; }





        #region User Master
        public int UserID { get; set; }
        public int UserHistoryID { get; set; }
        public string LoginName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ServLength { get; set; }
        public int DocumentID { get; set; }
        public string DocNo { get; set; }
        public string Attach { get; set; }
        public int CustomerShareHoldID { get; set; }
        public string Photo { get; set; }
        public string Password { get; set; }
        public DateTime PwdExpiryDate { get; set; }
        public bool IsAgree { get; set; }
        public bool IsActive { get; set; }      
        #endregion
    }
}