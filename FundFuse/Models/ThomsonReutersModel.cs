using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class ThomsonReutersModel
    {
        public int LogID { get; set; }
        public string LogType { get; set; }
        public string caseId { get; set; }
        public string caseSystemId { get; set; }
        public int SCID { get; set; }
        public string Request { get; set; }
        public int RequestBy { get; set; }
        public string RequestIP { get; set; }
        public string Response { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsNew { get; set; }
        public bool IsScreening { get; set; }
        public bool IsResolved { get; set; }
        public bool IsOGS { get; set; }
        public int PlatformID { get; set; }
        public int CustomerID { get; set; }
        public string CustType { get; set; }
        public int CustomerShareHoldID { get; set; }
        public int UserID { get; set; }
        public int BankID { get; set; }
        public int CustomerBankAccID { get; set; }
        public int SCDetID { get; set; }
        public string resultId { get; set; }
        public string statusId { get; set; }
        public string riskId { get; set; }
        public string reasonId { get; set; }
        public string ContactFullName { get; set; }
        public string BindFullName { get; set; }
        public string CountryCode { get; set; }
        public string resolutionRemark { get; set; }
        public DateTime presolutionDate { get; set; }
        public bool IsCompany { get; set; }
        public int intIsCompany { get; set; }
        public string CompanyName { get; set; }
        public string CountryName { get; set; }
        public int CountryID { get; set; }
        public int LocCountryID { get; set; }
        public string LoginName { get; set; }
        [Required]
        public string Gender { get; set; }
        public string LocCountryCode { get; set; }
        public string NatCountryCode { get; set; }
        public string LocCountryName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsUpload { get; set; }       
        public int OrgCountryID { get; set; }
        public int IndCountryID { get; set; }      
        public class Field
        {
            public string typeId { get; set; }
            public string value { get; set; }
            public object dateTimeValue { get; set; }
        }

        public class SecondaryFieldResult
        {
            public Field field { get; set; }
            public string typeId { get; set; }
            public string submittedValue { get; set; }
            public object submittedDateTimeValue { get; set; }
            public string matchedValue { get; set; }
            public object matchedDateTimeValue { get; set; }
            public string fieldResult { get; set; }
        }

        public class Resolution
        {
            public string statusId { get; set; }
            public object riskId { get; set; }
            public object reasonId { get; set; }
            public object resolutionRemark { get; set; }
            public object resolutionDate { get; set; }
        }

        public class ResultReview
        {
            public bool reviewRequired { get; set; }
            public DateTime reviewRequiredDate { get; set; }
            public object reviewRemark { get; set; }
            public object reviewDate { get; set; }
        }

        public class RootObject
        {
            public string resultId { get; set; }
            public string referenceId { get; set; }
            public string matchStrength { get; set; }
            public string matchedTerm { get; set; }
            public string submittedTerm { get; set; }
            public string matchedNameType { get; set; }
            public List<SecondaryFieldResult> secondaryFieldResults { get; set; }
            public List<string> sources { get; set; }
            public List<string> categories { get; set; }
            public DateTime creationDate { get; set; }
            public DateTime modificationDate { get; set; }
            public Resolution resolution { get; set; }
            public ResultReview resultReview { get; set; }
        }
        //public class Field
        //{
        //    public string typeId { get; set; }
        //    public string value { get; set; }
        //    public object dateTimeValue { get; set; }
        //}
        //public class SecondaryFieldResult
        //{
        //    public Field field { get; set; }
        //    public string typeId { get; set; }
        //    public string submittedValue { get; set; }
        //    public string submittedDateTimeValue { get; set; }
        //    public string matchedValue { get; set; }
        //    public object matchedDateTimeValue { get; set; }
        //    public string fieldResult { get; set; }
        //}
        //public class Resolution
        //{
        //    public string statusId { get; set; }
        //    public string riskId { get; set; }
        //    public string reasonId { get; set; }
        //    public string resolutionRemark { get; set; }
        //    public DateTime resolutionDate { get; set; }
        //}
        //public class ResultReview
        //{
        //    public bool reviewRequired { get; set; }
        //    public DateTime reviewRequiredDate { get; set; }
        //    public string reviewRemark { get; set; }
        //    public DateTime reviewDate { get; set; }
        //}
        //public class RootObject
        //{
        //    public string resultId { get; set; }
        //    public string referenceId { get; set; }
        //    public string matchStrength { get; set; }
        //    public string matchedTerm { get; set; }
        //    public string submittedTerm { get; set; }
        //    public string matchedNameType { get; set; }
        //    public List<SecondaryFieldResult> secondaryFieldResults { get; set; }
        //    public List<string> sources { get; set; }
        //    public List<string> categories { get; set; }
        //    public DateTime creationDate { get; set; }
        //    public DateTime modificationDate { get; set; }
        //    public Resolution resolution { get; set; }
        //    public ResultReview resultReview { get; set; }
        //}
    }
}