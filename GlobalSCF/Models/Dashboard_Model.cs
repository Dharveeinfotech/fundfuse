using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Dashboard_Model
    {
       
        //RegisteredUsers
        public int RObligorsBuyersSuppliers { get; set; }
        public int RExporter { get; set; }
        public int RFunder { get; set; }
        public int RServiceProvider { get; set; }
        public int RInvestor { get; set; }
        public int RObligor { get; set; }
        public int RBuyer { get; set; }
        public int RSupplier { get; set; }

        public int RObligors { get; set; }

        public int RBuyers { get; set; }

        public int RSuppliers { get; set; }
        public int RTotal { get; set; }

        //IncompleteRegistrations
        public int IRInCompleteCustomer { get; set; }
        public int IRInCompleteProgramConfiguration { get; set; }
        public int IRTotal { get; set; }

        //ObligorsBuyersSuppliers

        public int OBSObligorsBuyersSuppliers { get; set; }
        public int OBSExporter { get; set; }
        public int OBSFunder { get; set; }
        public int OBSServiceProvider { get; set; }
        public int OBSInvestor { get; set; }
        public int OBSTotal { get; set; }

        //ReminderDetail
        public List<Dashboard_Model> AllNotifications { get; set; }

        public int TradeLicence { get; set; }
        public int Other { get; set; }
        public int UserPassport { get; set; }
        public int UserEmirateID { get; set; }

        public int ReminderTotal { get; set; }

        public string ChgRemark { get; set; }

        public string LoginName { get; set; }


        public System.DateTime ChangeDate { get; set; }

        public string Notifications1 { get; set; }

        public string Notifications2 { get; set; }

        public string Notifications3 { get; set; }

        public string TagName { get; set; }

        public string file { get; set; }

        //Queued for Amendments
        public int CustomerInformationFile { get; set; }
        public int ProgramConfiguration { get; set; }

        public int TotalQueuedforAmendments { get; set; }

        public string DocumentName { get; set; }

        public int Total { get; set; }

        public int MainTotal { get; set; }

        public List<Dashboard_Model> ReminderDetail { get; set; }



        ///////////////////////////////AdminDashBoard_CompleteCustomerReg////////AdminDashBoard_InCompleteCustomerReg///////////////////////
        public string CompanyName { get; set; }

        public string CompanyShortName { get; set; }

        public string TradeLicenceNo { get; set; }

        public string CustomerTypeName { get; set; }
        public string StatusUserDesc { get; set; }
        public string ExpiredStatus { get; set; }
        public string CompanyTypeName { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }


        /////////////////////AdminDashBoard_InCompleteProgramConfig/////////////////////////////////////

        public string ProgramConfigType { get; set; }
        public string ProgramConfigCode { get; set; }
        public string ProgramConfigDesc { get; set; }
        public string FromCIFNumber { get; set; }
        public string FromCompanyName { get; set; }
        public string FromCustomerType { get; set; }

        public string ToCIFNumber { get; set; }
        public string ToCompanyName { get; set; }
        public string ToCustomerType { get; set; }

        /////////////////////AdminDashBoard_CompleteUserReg/////////////////////////////////////

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        //public string LoginName { get; set; }
        public string CustomerCode { get; set; }
        public string ContactFullName { get; set; }
        public string CIFNumber { get; set; }
        //  public string CompanyName { get; set; }
        public string EmailID { get; set; }
        public int IsAdminRegister { get; set; }
        public string MobileNo { get; set; }
        public string StaffID { get; set; }
        public string DesignationName { get; set; }

        //  public string CreateBy { get; set; }

        ///////////////////AdminDashBoard_ReminderDetail/////////////////

        public bool IsOriginal { get; set; }

        public bool IsReceived { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime LicenceExpDate { get; set; }
    }
    public class DATDashboard_Model
    {
        public int InvoiceApproval { get; set; }
        public int AssignmentFromObligor { get; set; }
        public int FundRequestToTI { get; set; }
        public int CommodityExchangeRem { get; set; }
        public int PaymentInstructions { get; set; }
        public int TaskReminderTotal { get; set; }
        public int ObligorApproval { get; set; }
        public int DATApproval { get; set; }
        public int FundsFromTI { get; set; }
        public int CommodityExchangePen { get; set; }
        public int PendingPayment { get; set; }
        public int PendingSettlement { get; set; }
        public int PendingTransactionTotal { get; set; }
        public int FundsDisbursed { get; set; }
        public int SettledTransactions { get; set; }
        public int Defaulted { get; set; }
        public int Completed { get; set; }
        public int FactoringTransactionTotal { get; set; }
    }
}