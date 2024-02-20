using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class CountryMaster
    {
        private int _CountryID = 0;
        private string _CountryName = "";
        private string _status = "";
        private bool _IsKeywordSearch = false;
        private string _Keywordvalue = "";
        public int CountryID { get { return _CountryID; } set { _CountryID = value; } }
        public string CountryCode { get; set; }
        public bool IsMaintenance { get; set; }
        public string MaintenanceMsg { get; set; }
        public string TMPWebSite { get; set; }
        public string TMPEmail { get; set; }
        public string TMPTeam { get; set; }
        public string TMPPlatform { get; set; }
        public int TransChargeID { get; set; }
        public string CountryName { get { return _CountryName; } set { _CountryName = value; } }
        public bool IsActive { get; set; }
        public bool IsNationality { get; set; }
        public int RiskLevel { get; set; }
        public string Sanction { get; set; }
        public string Status { get { return _status; } set { _status = value; } }
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
        public bool IsKeywordSearch { get { return _IsKeywordSearch; } set { _IsKeywordSearch = value; } }
        public string Keywordvalue { get { return _Keywordvalue; } set { _Keywordvalue = value; } }
        public short Active { get; set; }
        public short Nationality { get; set; }
        ///////////////////////////////////AffiliateCustomerMaste//////////////////////////
        private string _DesignationName = "";
        public int AffCustomerID { get; set; }
        public string CompanyName { get; set; }
        public int CityID { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public int StateID { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public int IndustryID { get; set; }
        public int ParentIndustryID { get; set; }
        public string IndustryName { get; set; }
        public string IndustryDesc { get; set; }
        public string ParentIndustryName { get; set; }
        public byte CompanyTypeID { get; set; }
        public string CompanyTypeName { get; set; }

        private int _DesignationID = 0;
        public int DesignationID { get { return _DesignationID; } set { _DesignationID = value; } }
        public string DesignationName { get { return _DesignationName; } set { _DesignationName = value; } }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string ContactPerson { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }
        public bool IsDefault { get; set; }
        public decimal FeePer { get; set; }
        public decimal FeeAmt { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public int ConvCurrencyID { get; set; }
        public string ConvCurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public byte[] CurrencySymbol { get; set; }
        public int InsuranceProviderID { get; set; }
        public string InsuranceProviderName { get; set; }
        public string ContactName { get; set; }
        public string TelNo { get; set; }
        public string WebSite { get; set; }
        ///////////////////////////////////mTEMPLATE//////////////////////////
        public int TemplateID { get; set; }
        public string Name { get; set; }
        ///////////////////////////////////mHtmlDynamicTemplateField//////////////////////////
        public int DynamicTextID { get; set; }
        public string DynamicTextName { get; set; }
        public string textid1 { get; set; }
        public int FaqID { get; set; }
        public string CustomerTypeName { get; set; }
        public string Questions { get; set; }
        public string Answer { get; set; }
        public int CustomerTypeID { get; set; }
        public string CustomerType { get; set; }
        public string ClassificationNo { get; set; }
        public int SMSConfigID { get; set; }
        public string URL { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public int HolidayID { get; set; }
        public string HolidayName { get; set; }
        public System.DateTime FromHolidayDate { get; set; }
        public System.DateTime ToHolidayDate { get; set; }
        public int EmailConfigID { get; set; }
        public string SmtpName { get; set; }
        public string FromEmailID { get; set; }
        public string BCCEmailID { get; set; }
        public string CCEmailID { get; set; }
        public string DisplayName { get; set; }
        public int InPort { get; set; }
        public int OutPort { get; set; }
        public int SystemParamterID { get; set; }
        public int ProcessTranDay { get; set; }
        public int FundRequireDay { get; set; }
        public string WebSiteURL { get; set; }
        public string SukukIssuanceName { get; set; }
        public int LoginLockoutAttempt { get; set; }
        public int LoginLockoutTimeFrameMin { get; set; }
        public int PwdExpiryDay { get; set; }
        public int LastPwdHistoryDay { get; set; }
        public int SiteIdleSessionMin { get; set; }
        //public bool IsMaintenance { get; set; }
        //public string MaintenanceMsg { get; set; }
        public List<CountryMaster> faqls { get; set; }
        public class PdfUpload
        {
            public int PdfID { get; set; }
            public int CustomerTypeID { get; set; }
            public string CustomerTypeName { get; set; }
            public string Name { get; set; }
            public bool IsActive { get; set; }
            public string Status { get; set; }
            public int ProcessBy { get; set; }
            public DateTime ProcessDate { get; set; }
            public string ProcessRemark { get; set; }
            public int CommandID { get; set; }
            public int CreateBy { get; set; }
            public DateTime CreateDate { get; set; }
            public string CreateIP { get; set; }
            public int UpdateBy { get; set; }
            public DateTime UpdateDate { get; set; }
            public string UpdateIP { get; set; }
        }

       
    }
    public class ScreeningConfig
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
        public int CategoryRiskID { get; set; }
        public string RiskLevel { get; set; }
        public string FromRating { get; set; }
        public string ToRating { get; set; }
        public int CategoryDetID { get; set; }
        public int ParentCategoryDetID { get; set; }
        public string Value { get; set; }
        public string YesRiskLevel { get; set; }
        public string SelTypeYN { get; set; }
        public string NoRiskLevel { get; set; }
        public int OrderNo { get; set; }
        public string EditRecord { get; set; }
        public string LowFromRate { get; set; }
        public string LowToRate { get; set; }
        public string ModFromRate { get; set; }
        public string ModToRate { get; set; }
        public string HighFromRate { get; set; }
        public string HighToRate { get; set; }
        public string VHighFromRate { get; set; }
        public string VHighToRate { get; set; }
        public int CustomerRiskID { get; set; }
        public int CustomerID { get; set; }
        public string SelType { get; set; }
        public int LocCountryID { get; set; }
        public string Sanction { get; set; }
        public string Category { get; set; }
        public string LocCountryName { get; set; }
        public string DueDiligence { get; set; }
        public int Low { get; set; }
        public int Moderate { get; set; }
        public int High { get; set; }
        public int VeryHigh { get; set; }
        public int CustomerTypeID { get; set; }
        public string Status { get; set; }
        public bool SendToCredit { get; set; }
        public string Admin { get; set; }
        public int Rating { get; set; }
        public string ProcessRemark { get; set; }
        public bool IsOverride { get; set; }
        public string ProgramType { get; set; }
        public string YesComment { get; set; }
        public string NoComment { get; set; }
        public string SelComment { get; set; }
        public string OveRiskLevel { get; set; }
    }
}