using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TMP.Models
{
    public class OnlineUserRegModel
    {
        public int CustomerTempID { get; set; }
        public int CustomerEligID { get; set; }

        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string ContactLastName { get; set; }
        public string ContactMiddleName { get; set; }
        public int CustomerTypeID { get; set; }
        [Required]
        public int CompanyTypeID { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string EmailID { get; set; }

        //[Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? DOB { get; set; }
        [Required]
        public int NationalityID { get; set; }
        [Required]
        public int IncCountryID { get; set; }
        [Required]
        public int IndustryID { get; set; }
        public string Website { get; set; }
        public string InteretedIn { get; set; }
        public string Other { get; set; }
        [Required]
        public int DesignationID { get; set; }
        [Required]
        public string WorkPhoneNo { get; set; }
        public string ActivationCode { get; set; }
        public string ActivationLink { get; set; }
        public string CreateIP { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteIP { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public DateTime ConfirmDate { get; set; }
        public DateTime ServerDate { get; set; }
    }
    public class mCustomerDocDetailModel
    {
        public int CustomerDocDetID { get; set; }
        public int CustomerID { get; set; }
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public bool IsReceived { get; set; }
        public bool IsOriginal { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Comment { get; set; }
        public string AttchFileName { get; set; }
        public string CustomerTypeAttachment { get; set; }
        public string DocumentUpload { get; set; }
        public string Status { get; set; }
        public string StatusMain { get; set; }
        public int CreateBy { get; set; }
        public string CreateIP { get; set; }
        public int UpdateBy { get; set; }
        public string UpdateIP { get; set; }
        public int CustomerDocDetProcessHistoryID { get; set; }
        public string ProcessRemark { get; set; }
        public int ProcessBy { get; set; }
        public string ProcessIP { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteIP { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public List<CustomerTypeDocumentDetails_ListAll_Result> lstCustomerTypeDocumentList { get; set; }
        public List<mCustomerDocDetailModel> lstCustomerDocDetail { get; set; }
        public int CustomerAdmDetID { get; set; }
        public int CustomerAdmDetProcessHistoryID { get; set; }
        [Required]
        public string Clearance { get; set; }
        [Required]
        public string Compliance { get; set; }
        [Required]
        public string CompFeedBack { get; set; }
        [Required]
        public string Scoring { get; set; }
        [Required]
        public string InvMeeting { get; set; }
        [Required]
        public string AdmClearance { get; set; }
        [Required]
        public string AdmRef { get; set; }
        [Required]
        public string DocVerification { get; set; }
        public int CustomerTypeID { get; set; }
        public bool IsUserDoc { get; set; }
        public bool IsInvestor { get; set; }

        public int CustomerAuthSign { get; set; }
        public int CustomerBankAccDetail { get; set; }
        public int CustomerBriefDetail { get; set; }
        public int CustomerContactDetail { get; set; }
        public int CustomerDocDetail { get; set; }
        public int CustomerInvestmentDetail { get; set; }
        public int CustomerPastInvestmentDetail { get; set; }
        public int CustomerQuestion { get; set; }
        public int CustomerShareHoldDetail { get; set; }
        public int CustomerAdminProcess { get; set; }
    }
    public class mCustomerBankAccBriefModel
    {
        public int CustomerBankAccDetProcessHistoryID { get; set; }
        public int CustomerBankAccDetID { get; set; }
        public int CustomerID { get; set; }
        [Required]
        public int CurrencyID { get; set; }
        [Required]
        public int BankID { get; set; }
        [Required]
        public int BankCountryID { get; set; }
        [Required]
        public string BranchName { get; set; }
        [Required]
        public string AccountName { get; set; }
        public string CurrencyCode { get; set; }
        public string BankName { get; set; }
        public string AccountDetail { get; set; }
        public string CountryName { get; set; }
        [Required]
        public string AccountNo { get; set; }
        [Required]
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string IFSCCode { get; set; }
        public string Status { get; set; }
        public string StatusMain { get; set; }
        public string ProcessRemark { get; set; }
        public int ProcessBy { get; set; }
        public string ProcessIP { get; set; }
        public int CreateBy { get; set; }
        public string CreateIP { get; set; }
        public int UpdateBy { get; set; }
        public string UpdateIP { get; set; }
        public int CustomerBriefProcessHistoryID { get; set; }
        public int CustomerBriefID { get; set; }
        public int IncCurrencyID { get; set; }
        public string IncCurrencyCode { get; set; }
        public int EducationID { get; set; }
        public int EmpTypeID { get; set; }
        public string EducationName { get; set; }
        public string EmpTypeName { get; set; }
        [Required]
        public string CurrEmployer { get; set; }
        [Required]
        public string Designation { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string MaritalStatus { get; set; }
        [Required]
        public Decimal MonthlyInc { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteIP { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public int PreCurrencyID { get; set; }
        public int ConvCurrencyID { get; set; }
        public decimal ConvRate { get; set; }
        public decimal FeeAmt { get; set; }
        public string ConvCurrencyCode { get; set; }
        public string PreCurrencyCode { get; set; }
    }
    public class mCustomerMasterModel
    {
        public int UserID { get; set; }
        public int CustomerProcessHistoryID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public bool IsMyAccount { get; set; }
        public int CustomerTempID { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string FullName { get; set; }
        [Required]
        public string ContactName { get; set; }
        public string ContactMiddleName { get; set; }
        public string ContacFullName { get; set; }
        [Required]
        public string ContactLastName { get; set; }
        public int CustomerTypeID { get; set; }
        public string CustomerTypeName { get; set; }
        public int CompanyTypeID { get; set; }
        public string CompanyTypeName { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string EmailID { get; set; }
        //[Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? DOB { get; set; }
        [Required]
        public int NationalityID { get; set; }
        public string NationalityName { get; set; }
        public string IncCountryName { get; set; }
        public string CountryName { get; set; }
        public string TradingName { get; set; }
        [Required]
        public string TradeLicNo { get; set; }

        [Required]
        public int IncCountryID { get; set; }
        //[Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? IncDate { get; set; }
        //[Required]        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? LicExpDate { get; set; }
        public string EntityType { get; set; }
        [Required]
        public string IssuingAuth { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public int CityID { get; set; }
        public int GetCityID { get; set; }
        public string CityName { get; set; }
        public int StateID { get; set; }
        public string StateName { get; set; }
        [Required]
        public int CountryID { get; set; }
        [Required]
        public string TelNo { get; set; }
        public string Website { get; set; }
        [Required]
        public string POBoxNo { get; set; }
        public string TradeLicUpd { get; set; }
        public string MemAssUpd { get; set; }
        public string Status { get; set; }
        public string StatusMain { get; set; }
        public string ProcessRemark { get; set; }
        public int ProcessBy { get; set; }
        public string ProcessIP { get; set; }
        public int CreateBy { get; set; }
        public string CreateIP { get; set; }
        public int UpdateBy { get; set; }
        public string UpdateIP { get; set; }
        public int CustomerAuthSignDetProcessHistoryID { get; set; }
        public int CustomerAuthSignID { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PassportNo { get; set; }
        [Required]
        public int IssuanceID { get; set; }
        public string IssuanceName { get; set; }
        //[Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        //public DateTime? IssuanceDate { get; set; }
        public DateTime IssuanceDate { get; set; }
        //[Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime ExpiryDate { get; set; }
        public string PassportUpd { get; set; }
        public string NatIdenUpd { get; set; }
        [Required]
        public int ResCountryID { get; set; }
        public string ResCountryName { get; set; }
        [Required]
        public string POBox { get; set; }
        public int DeleteBy { get; set; }
        public string DeleteIP { get; set; }
        public bool IsKeywordSearch { get; set; }
        public string Keywordvalue { get; set; }
        public int IsAdminRegister { get; set; }
        public int CustomerShareHoldDetProcessHistoryID { get; set; }
        public int CustomerShareHoldDetID { get; set; }
        public int CustomerCompanyShareHoldDetID { get; set; }
        public int CustomerChildShareHoldDetID { get; set; }

        [Required]
        public string ShareFirstName { get; set; }
        public string ShareMiddleName { get; set; }
        [Required]
        public string ShareLastName { get; set; }
        [Required]
        public Decimal SharePer { get; set; }
        [Required]
        public Decimal CompSharePer { get; set; }
        public Decimal childSharePer { get; set; }
        [Required]
        public int ShareNatCountryID { get; set; }
        public string ShareCountryName { get; set; }
        [Required]
        public string SharePassportNo { get; set; }
        [Required]
        public int PassportCountryID { get; set; }
        public string PassportCountryName { get; set; }
        //[Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime PassportIssueDate { get; set; }
        //[Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime PassportExpDate { get; set; }
        public string SharePassportUpd { get; set; }
        public string ShareNatIdenUpd { get; set; }

        [Required]
        public string ShareCompanyName { get; set; }
        [Required]
        public string ShareTradeLicNo { get; set; }
        public string ShareTradeLicUpd { get; set; }
        public bool IsCompany { get; set; }
        public int ParentID { get; set; }
        public bool IsChild { get; set; }
        public int Duplicate { get; set; }
        public string StatusDisplay { get; set; }
        public string AuthorizedUser { get; set; }
        public string ProgramType { get; set; }

    }
    public class UserSignDetailModel
    {
        public int UserSignID { get; set; }
        public bool IsAcceptLOA { get; set; }
        public int UserID { get; set; }
        public string Type { get; set; }
        public int ReferenceID { get; set; }
        public int DocumentID { get; set; }
        public int HtmlTemplateID { get; set; }
        public int OrderNo { get; set; }
        public string Feedback { get; set; }
        public string Feedback1 { get; set; }
        public string Feedback2 { get; set; }
        public string Feedback3 { get; set; }
        public string Feedback4 { get; set; }
        public string FileType { get; set; }
        public string RefNo { get; set; }
        public string RespTranNo { get; set; }
        public string RespStatus { get; set; }
        public string RespMessage { get; set; }
        public string RespValue { get; set; }
        public string AttachName { get; set; }
        public string Status { get; set; }
        public string ProgramType { get; set; }
        public bool IsSendLOA { get; set; }
        public int CustomerRevID { get; set; }
        public int CustomerID { get; set; }
        public bool IsIndiCmplt { get; set; }
        public int CurrencyID { get; set; }
        public string CurrencyCode { get; set; }
        public Decimal Limit { get; set; }
        public int Tenor { get; set; }
        public Decimal ProfitRate { get; set; }
        public Decimal ProcessFee { get; set; }
        public Decimal SecDeptPer { get; set; }
        public Decimal SecDeptAmt { get; set; }
        public Decimal BrokerFee { get; set; }
        public Decimal ArrangFeePer { get; set; }
        public Decimal ArrangFeeAmt { get; set; }
        public Decimal OtherFee { get; set; }
        public Decimal PenaltyRate { get; set; }
        public string UpdDiligRpt { get; set; }
        public string UpdCredRpt { get; set; }
        public int CustomerRevHistoryID { get; set; }
        public int CustomerTypeID { get; set; }
        public string ProcessRemark { get; set; }
        public Decimal BaseRate { get; set; }
        public Decimal ProfMarginRate { get; set; }
        public Decimal DiscRate { get; set; }
        public Decimal HoldbackPer { get; set; }
        public Decimal TranFeePer { get; set; }
        public bool IsMenuPanelHide { get; set; }
        public int CustomerSuppID { get; set; }
        public int CustomerExporterID { get; set; }
        public string filename { get; set; }
        public string ContactFullName { get; set; }
        public string CustomerCode { get; set; }
        public string HtmlName { get; set; }
        public string PageNo { get; set; }
        public string Location { get; set; }
    }
}