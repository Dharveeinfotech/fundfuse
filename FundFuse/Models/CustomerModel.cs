using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class CustomerTempModel
    {
        public int CustomerTempID { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string TradingName { get; set; }
        [Required]
        public string ContactFName { get; set; }
        public string ContactMName { get; set; }
        [Required]
        public string ContactLName { get; set; }
        public int CustomerTypeID { get; set; }
        public string ProgramType { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string EmailID { get; set; }
        public int NationalityID { get; set; }
        public int IndustryID { get; set; }
        public string OtherIndustry { get; set; }
        public int SubIndustryID { get; set; }
        public string SubIndustryName { get; set; }
        public string OtherSubIndustry { get; set; }
        public string OtherSubIndustry_Sub { get; set; }
        public string OtherCity { get; set; }
        [Required]
        public string Gender { get; set; }
        public int LocCountryID { get; set; }
        public string Website { get; set; }
        public int DesignationID { get; set; }
        public int CityID { get; set; }
        public string OtherDesignation { get; set; }
        [Required]
        public string TelNo { get; set; }
        public string ActivationCode { get; set; }
        public string ActivationLink { get; set; }
        public DateTime ConfirmDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int CustomerID { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public DateTime ServerDate { get; set; }
        public int CustomerEligID { get; set; }
        public string ContactFullName { get; set; }
        public int CompanyTypeID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? DOB { get; set; }
        [Required]
        public int IncCountryID { get; set; }
        public string InteretedIn { get; set; }
        public string Other { get; set; }
        [Required]
        public string WorkPhoneNo { get; set; }
        public string CreateIP { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteIP { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public string Address { get; set; }
        public string POBox { get; set; }
    }
    public class CustomerMasterModel
    {
        public int UserID { get; set; }
        public int CustomerID { get; set; }
        public int CustomerTempID { get; set; }
        public string CompanyName { get; set; }
        public string Program { get; set; }
        public string LoginName { get; set; }
        public string ActionLevel { get; set; }
        public DateTime ProcessDate { get; set; }
        public string TradingName { get; set; }
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
        public string OtherSubIndustry_Sub { get; set; }
        public string Website { get; set; }
        public int DesignationID { get; set; }
        public string OtherDesignation { get; set; }
        public string TelNo { get; set; }
        public int CustomerHistoryID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerTypeName { get; set; }
        public string Status { get; set; }
        public int GetCityID { get; set; }
        public string ProcessRemark { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public string ContactFullName { get; set; }
        public int IsAdminRegister { get; set; }
        public string StatusDisplay { get; set; }
        public string NationalityName { get; set; }
        public string IndustryName { get; set; }
        public string SubIndustryName { get; set; }
        public string DesignationName { get; set; }
        public string DocumentName { get; set; }
        public string DocumentUpload { get; set; }
        public bool IsCompPubList { get; set; }
        public string CompPubList { get; set; }
        public string Admin { get; set; }
        public string ProgramCode { get; set; }

        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string POBox { get; set; }
        public string OtherCity { get; set; }
        [Required]
        public string Gender { get; set; }
        public string LocCountryName { get; set; }
        public int LocCountryID { get; set; }
        public string _currentUser { get; set; }
        public bool IsViewer { get; set; }
        public bool IsMaker { get; set; }
        public bool IsCompScr { get; set; }
        public bool IsCreRevChecker { get; set; }
        public bool IsCreRevApprover { get; set; }
        public bool IsOpTeam { get; set; }
        public string filename { get; set; }
        public bool IsNew { get; set; }
        public bool IsCheckScreening { get; set; }
        public bool IsScreenCompleted { get; set; }
    }
    public class CustomerInfoModel
    {
        public int CustomerInfoID { get; set; }
        public int CustomerID { get; set; }
        public int CompanyTypeID { get; set; }
        public string OtherCompanyType { get; set; }
        public string CompanyTypeName { get; set; }
        [Required]
        public string TradeLicNo { get; set; }
        public int IncCountryID { get; set; }
        public string IncCountryName { get; set; }

        public DateTime IncDate { get; set; }

        public DateTime LicExpDate { get; set; }
        [Required]
        public string IssuingAuth { get; set; }
        [Required]
        public string AbtBusiness { get; set; }
        [Required]
        public string BusActivity { get; set; }
        [Required]
        public string EmpCount { get; set; }
        [Required]
        public Decimal AnnTurnOver { get; set; }
        [Required]
        public Decimal SaleVolume { get; set; }
        [Required]
        public Decimal NetProfit { get; set; }
        [Required]
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
        public string OtherCity { get; set; }
        public string TelNo { get; set; }
        public string Website { get; set; }
        public string ObligorIDs { get; set; }
        public string[] CustomerObligors_List { get; set; }
        public string UpdTradeLic { get; set; }
        public string UpdBusProfile { get; set; }
        public string UpdAuditFin { get; set; }
        public string UpdInHouseFin { get; set; }
        public string UpdAgingRpt { get; set; }
        public string UpdDiligRpt { get; set; }
        public string UpdCredRpt { get; set; }
        public string Status { get; set; }
        public int CustomerInfoHistoryID { get; set; }
        public string ProcessRemark { get; set; }
        public bool IsNoRequired { get; set; }
        public int CustomerTypeID { get; set; }
        public string CompPubList { get; set; }
        public string ObligorNames { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string POBox { get; set; }
        public int TranCnt { get; set; }
        public Decimal TotTranAmt { get; set; }
        public Decimal TotSetAmt { get; set; }
        public Decimal TotOutStand { get; set; }
        public string ProgramTypeDesc { get; set; }

        public int InvoiceID { get; set; }
        public int SupplierID { get; set; }
        public int BuyerID { get; set; }
        public int ObligorID { get; set; }
        public string _currentUser { get; set; }
        public string ProgramType { get; set; }
        public string CompanyName { get; set; }
        public int CustomerCreditID { get; set; }
        public string FacType { get; set; }
        public Decimal FacLimit { get; set; }
        public Decimal FacUtilize { get; set; }
        public int CustomerCreditHistoryID { get; set; }
        public int CreditBankAccID { get; set; }
        public bool IsCustomerSupp { get; set; }
        public string SupplierName { get; set; }
        public string CreditCurrencyCode { get; set; }
        public int CreditCurrencyID { get; set; }
        public int CreditBankID { get; set; }
        public string CreditBankName { get; set; }
        public int CustomerBankAccID { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string MstWebsite { get; set; }
    }
    public class CustomerDocModel
    {
        public List<CustomerTypeDocumentDetails_ListAll_Result> lstCustomerTypeDocumentList { get; set; }
        public List<CustomerDocModel> lstCustomerDocDetail { get; set; }
        public int CustomerDocID { get; set; }
        public int CustomerID { get; set; }
        public int DocumentID { get; set; }
        public string Attachment { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsUpload { get; set; }
        public bool IsOGS { get; set; }
        public string OrganisationName { get; set; }
        public int OrgCountryID { get; set; }
        public int IndCountryID { get; set; }
        public string resultId { get; set; }
        public string statusId { get; set; }
        public string riskId { get; set; }
        public string reasonId { get; set; }
        public string caseSystemId { get; set; }
        public string ContactFullName { get; set; }
        public string DocumentName { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public int CustomerDocHistoryID { get; set; }
        public string ProcessRemark { get; set; }
        public int CustomerTypeID { get; set; }
        public int IsUserDocInt { get; set; }
        public int IsInvestorInt { get; set; }
        public bool IsUserDoc { get; set; }
        public bool IsInvestor { get; set; }
        public bool IsNoRequired { get; set; }

        public int CustomerBankAccDetail { get; set; }
        public int CustomerBankCreditDetail { get; set; }
        public int CustomerDocDetail { get; set; }
        public int CustomerInfoDetail { get; set; }
        public int CustomerReviewDetail { get; set; }
        public int CustomerShareHoldDetail { get; set; }
        public int CustomerShareHoldDetailAuth { get; set; }
        public int CustomerSupplierDetail { get; set; }
        public int CustomerObligorDetail { get; set; }
        public int CustomerVerifyDetail { get; set; }
        public string ProgramType { get; set; }
        public string Admin { get; set; }
        public bool SendToCredit { get; set; }


        public bool rdo_CLIENTY { get; set; }
        public bool rdo_CLIENTN { get; set; }
  
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string Gender { get; set; }
        public string Type { get; set; }
        public string ScrStatus { get; set; }
        public int ScreeningID { get; set; }
        #region Client Clasification
        public int CustomerClassId { get; set; }
        public bool cbYes { get; set; }
        public bool cbNo { get; set; }
        public bool IsRC { get; set; }
        public bool IsPC { get; set; }
        public bool IsDP { get; set; }
        public bool IsDP1 { get; set; }
        public bool IsDP2 { get; set; }
        public bool IsDP3 { get; set; }
        public bool IsDP4 { get; set; }
        public bool IsDP5 { get; set; }
        public bool IsDP6 { get; set; }
        public bool IsDP7 { get; set; }
        public bool IsDP71 { get; set; }
        public bool IsDP72 { get; set; }
        public bool IsDP73 { get; set; }
        public bool IsDP8 { get; set; }
        public bool IsDP9 { get; set; }
        public bool IsDP10 { get; set; }
        public bool IsDP11 { get; set; }
        public bool IsSB { get; set; }
        public bool IsSB1 { get; set; }
        public bool IsSB11 { get; set; }
        public bool IsSB12 { get; set; }
        public bool IsSB2 { get; set; }
        public bool IsSB21 { get; set; }
        public bool IsAP { get; set; }
        public bool IsAP1 { get; set; }
        public bool IsAP11 { get; set; }
        public string APIndName11 { get; set; }
        public bool IsAP12 { get; set; }
        public string APIndName12 { get; set; }
        public bool IsAP2 { get; set; }
        public bool IsAP21 { get; set; }
        public string APIndName21 { get; set; }
        public bool IsAP22 { get; set; }
        public string APIndName22 { get; set; }
        public bool IsMC { get; set; }
        public bool IsMC1 { get; set; }
        public bool IsMC2 { get; set; }
        public string Category { get; set; }

        public bool rdo_CLIENT { get; set; }
        public bool rdo_BUSINESSPARTNER { get; set; }
        public bool rdo_RELATEDPARTY { get; set; }
        public bool rdo_SERVICEPROVIDER { get; set; }

        public bool IsCheckScreening { get; set; }
        public bool IsOPTeam { get; set; }

        #endregion
    }
    public class CustomerShareHolderModel
    {
        public bool IsSelectSign { get; set; }
        public string AuthSignType { get; set; }
        public int CustomerShareHoldID { get; set; }
        public int UserID { get; set; }
        public int CustomerBankAccID { get; set; }
        public int BankID { get; set; }
        public int CustomerID { get; set; }
        public int PlatformID { get; set; }
        public int CustomerTypeID { get; set; }
        public int ParentID { get; set; }
        public string CustType { get; set; }

        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string RoleNames { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public Decimal SharePer { get; set; }
        public int DesignationID { get; set; }
        public string OtherDesignation { get; set; }
        public string DesignationName { get; set; }
        public string ContactFullName { get; set; }
        public string Name { get; set; }
        public string CustTypeDesc { get; set; }
        public int NatCountryID { get; set; }
        
        public string NatCountryName { get; set; }
        public string NatCountryCode { get; set; }
        public string LocCountryCode { get; set; }

        public int RegiCountryID { get; set; }
        public string RegiCountryName { get; set; }
        public string RegiCountryCode { get; set; }

        [Required]
        public string PassNo { get; set; }
        public DateTime PassIssueDate { get; set; }
        public DateTime PassExpDate { get; set; }
        public int PassCountryID { get; set; }
        public string PassCountryName { get; set; }
        [Required]
        public string TradeLicNo { get; set; }
        public string UpdPassport { get; set; }
        public string UpdNatIden { get; set; }
        public string UpdTradeLic { get; set; }
        public string UpdPOA { get; set; }
        public string Status { get; set; }
        [Required]
        public string Gender { get; set; }
        public string ShareGender { get; set; }
        public int LocCountryID { get; set; }
        public int ShareLocCountryID { get; set; }
        public string LocCountryName { get; set; }
        public string RoleIDs { get; set; }
        public string GetRoleIDs { get; set; }
        public int CustomerShareHoldHistoryID { get; set; }
        public string ProcessRemark { get; set; }
        public bool IsNoRequired { get; set; }
        public int ChildCnt { get; set; }

        public int AuthCustomerShareHoldID { get; set; }
        [Required]
        public string AuthFirstName { get; set; }
        public string AuthMiddleName { get; set; }
        [Required]
        public string AuthLastName { get; set; }
        public int AuthDesignationID { get; set; }
        [Required]
        public string AuthPassNo { get; set; }
        public string AuthUpdPassport { get; set; }
        public string AuthUpdPOA { get; set; }
        public int AuthNatCountryID { get; set; }
        [Required]
        public string EmailID { get; set; }
        [Required]
        public string MobileNo { get; set; }
        public int CompCustomerShareHoldID { get; set; }
        [Required]
        public Decimal CompSharePer { get; set; }
        public int ShareCustomerShareHoldID { get; set; }
        [Required]
        public string ShareFirstName { get; set; }
        public string ShareMiddleName { get; set; }
        [Required]
        public string ShareLastName { get; set; }
        public int ShareDesignationID { get; set; }
        [Required]
        public string SharePassNo { get; set; }
        public string ShareUpdPassport { get; set; }
        public string ShareUpdNatIden { get; set; }
        public string caseSystemId { get; set; }
        public int ShareNatCountryID { get; set; }
        public Decimal childSharePer { get; set; }
        public DateTime SharePassIssueDate { get; set; }
        public DateTime SharePassExpDate { get; set; }
        public int SharePassCountryID { get; set; }
        public string ShareCompanyName { get; set; }
        public string CompPubList { get; set; }

    }
    public class CustomerBankAccDetail
    {
        public int CustomerBankAccID { get; set; }
        public int CustomerID { get; set; }
        public int CustomerSuppID { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyID { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string AccountNo { get; set; }
        public string CustomerAccountNo { get; set; }
        public int BankCountryID { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string IBAN { get; set; }
        public string CountryName { get; set; }
        public string Swift { get; set; }
        public string IFSC { get; set; }
        public string Status { get; set; }
        public int CustomerBankAccHistoryID { get; set; }
        public string ProcessRemark { get; set; }
        public bool IsNoRequired { get; set; }
        public int CustomerCreditID { get; set; }
        public string FacType { get; set; }
        public Decimal FacLimit { get; set; }
        public Decimal FacUtilize { get; set; }
        public int CustomerCreditHistoryID { get; set; }
        public int CreditBankAccID { get; set; }
        public bool IsCustomerSupp { get; set; }
        public string SupplierName { get; set; }
        public int CustomerTypeID { get; set; }
        public string ProgramType { get; set; }
        public string CreditCurrencyCode { get; set; }
        public int CreditCurrencyID { get; set; }
        public int CreditBankID { get; set; }
        public string CreditBankName { get; set; }
    }
    public class CustomerSupplierModel
    {
        public int CustomerSuppID { get; set; }
        public int CustomerID { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string ContactFName { get; set; }
        public string ContactMName { get; set; }
        [Required]
        public string ContactLName { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string EmailID { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string OtherIndustry { get; set; }
        public string OtherSubIndustry { get; set; }
        public string OtherCity { get; set; }
        public string OtherDesignation { get; set; }
        public string ProgramType { get; set; }

        [Required]
        public string TradeLicNo { get; set; }
        public int IncCountryID { get; set; }
        public string IncCountryName { get; set; }
        public DateTime IncDate { get; set; }
        public DateTime LicExpDate { get; set; }
        public int IndustryID { get; set; }
        public int SubIndustryID { get; set; }
        public string IndustryName { get; set; }
        public string SubIndustryName { get; set; }
        [Required]
        public string InvTerm { get; set; }
        public string OtherInvTerm { get; set; }
        public Decimal InvAmt { get; set; }
        public int CountryID { get; set; }
        public int CityID { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string POBox { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        public string TelNo { get; set; }
        public string Status { get; set; }
        public bool IsNoRequired { get; set; }

        public int CustomerSuppHistoryID { get; set; }
        public string ProcessRemark { get; set; }

        public int CustomerBankAccID { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyID { get; set; }
        public int BankID { get; set; }
        public string BankName { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string AccountNo { get; set; }
        public int BankCountryID { get; set; }
        [Required]
        public string BankAddress { get; set; }
        [Required]
        public string IBAN { get; set; }
        [Required]
        public int BankCurrencyID { get; set; }
        public string Swift { get; set; }
        public string IFSC { get; set; }
        public int CustomerBankAccHistoryID { get; set; }
        public bool IsCustomerSupp { get; set; }
        public int CustomerTypeID { get; set; }
        public string SupplierName { get; set; }
        public string ContactFullName { get; set; }
    }
    public class CustomerReviewModel
    {
        public int CustomerRevID { get; set; }
        public int FCustomerRevID { get; set; }
        public int CustomerID { get; set; }
        public int CurrencyID { get; set; }
        public int FCurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public string FCurrencyCode { get; set; }
        [Required]
        public Decimal Limit { get; set; }
        [Required]
        public Decimal FLimit { get; set; }
        [Required]
        public int Tenor { get; set; }
        [Required]
        public int FTenor { get; set; }
        public Decimal ProfitRate { get; set; }
        public Decimal ProcessFee { get; set; }
        public Decimal SecDeptPer { get; set; }
        public Decimal SecDeptAmt { get; set; }
        public Decimal BrokerFee { get; set; }
        public Decimal ArrangFeePer { get; set; }
        public Decimal ArrangFeeAmt { get; set; }
        public Decimal OtherFee { get; set; }
        public Decimal PenaltyRate { get; set; }
        public string Feedback { get; set; }
        public string UpdDiligRpt { get; set; }
        public string UpdCredRpt { get; set; }
        public string Status { get; set; }
        public int CustomerRevHistoryID { get; set; }
        public int CustomerTypeID { get; set; }
        public string ProcessRemark { get; set; }
        public Decimal BaseRate { get; set; }
        public Decimal ProfMarginRate { get; set; }
        public Decimal DiscRate { get; set; }
        public Decimal HoldbackPer { get; set; }
        public Decimal TranFeePer { get; set; }
        public string ProgramType { get; set; }
        public string Admin { get; set; }
        public bool IsViewer { get; set; }
        public bool IsMaker { get; set; }
        public bool IsCompScr { get; set; }
        public bool IsCreRevChecker { get; set; }
        public bool IsCreRevApprover { get; set; }
        public bool IsOpTeam { get; set; }
        public string ProfitTypeDesc { get; set; }
        public string ContactFullName { get; set; }
    }
    public class CustomerVerifyModel
    {
        public string CustomerCode { get; set; }
        public int IsLoginMailSend { get; set; }
        public string CompanyName { get; set; }
        public string ContactFullName { get; set; }
        public int CustomerVerID { get; set; }
        public int CustomerID { get; set; }
        public int ExtTenor { get; set; }
        public bool IsComplete { get; set; }
        public string CompleteBy { get; set; }
        public DateTime CompleteDate { get; set; }
        public bool IsVerified { get; set; }
        public string DocumentIDs { get; set; }
        public string OtherDoc { get; set; }
        public string CustomerShareHoldIDs { get; set; }
        public string[] CustomerShareHoldIDs_List { get; set; }
        public string[] DocumentIDs_List { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public int CustomerVerHistoryID { get; set; }
        public string ProgramCode { get; set; }
        public string AuthSignType { get; set; }
        public string ProfitType { get; set; }
        public string ExtDateType { get; set; }
        public int CustomerTypeID { get; set; }
        [Required]
        public string Complete { get; set; }
        public int UserID { get; set; }
        public string Admin { get; set; }
        public string Feedback { get; set; }
        public string ProcessRemark { get; set; }
        public bool IsInsurance { get; set; }
        public string PolicyNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpDate { get; set; }
        public string Attach { get; set; }
        public string _currentStatus { get; set; }
        public int OrderNo { get; set; }
    }

}