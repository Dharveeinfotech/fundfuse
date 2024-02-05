using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class OSBDashboard
    {
        public int intBReminderTotal { get; set; }
        public int intBVolumeTotal { get; set; }
        public int intBAuthoTotal { get; set; }
        public decimal intBCreditTotal { get; set; }

        public int intOReminderTotal { get; set; }
        public int intOVolumeTotal { get; set; }
        public int intOAuthoTotal { get; set; }
        public decimal intOCreditTotal { get; set; }

        public int intSReminderTotal { get; set; }
        public int intSVolumeTotal { get; set; }
        public int intSAuthoTotal { get; set; }
        public decimal intSCreditTotal { get; set; }


        public int DocumentRenewal { get; set; }
        public int SettlementDue { get; set; }
        public int DocumentRenewal1 { get; set; }
        public int SettlementDue1 { get; set; }
        public int DocumentRenewal2 { get; set; }
        public int SettlementDue2 { get; set; }
        public DateTime SettlementDueDate { get; set; }
        public int NumberOfBuyerLinkedToExporter { get; set; }
        public int NumberOfExporterLinkedToBuyer { get; set; }
        public int NumberOfObligorsLinkedToSupplier { get; set; }
        public int NumberOfSupplierLinkedToObligors { get; set; }
        public int TransactionSubmitted { get; set; }
        public int TransactionsApprove { get; set; }
        public int TransactionsRejected { get; set; }
        public string TransactionsAmount { get; set; }
        public int TransactionSubmitted1 { get; set; }
        public int TransactionsApprove1 { get; set; }
        public int TransactionsRejected1 { get; set; }
        public string TransactionsAmount1 { get; set; }
        public int NoOfAssignments { get; set; }
        public int AcceptedAssignments { get; set; }
        public int RejectedAssignment { get; set; }
        public string TotalAssignmentAmount { get; set; }
        public bool Maker { get; set; }
        public bool Checker { get; set; }
        public int Maker1 { get; set; }
        public int Checker1 { get; set; }
        public int Maker2 { get; set; }
        public int Checker2 { get; set; }
        public int Maker3 { get; set; }
        public int Checker3 { get; set; }
        public decimal TotalLimit { get; set; }
        public decimal UtilizedLimit { get; set; }
        public decimal BalanceLimit { get; set; }
        public decimal TotalLimit1 { get; set; }
        public decimal UtilizedLimit1 { get; set; }
        public decimal BalanceLimit1 { get; set; }
        public decimal TotalLimit2 { get; set; }
        public decimal UtilizedLimit2 { get; set; }
        public decimal BalanceLimit2 { get; set; }


        public string TagName { get; set; }
        public string CompanyName { get; set; }
        public string DocumentName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsReceived { get; set; }
        public bool IsOriginal { get; set; }


        public string TranRefNo { get; set; }
        public string FromCompanyName { get; set; }
        public DateTime TranDate { get; set; }
        public DateTime DueDate { get; set; }


        public string ProgramConfigCode { get; set; }
        public string FromCIFNumber { get; set; }

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
        public int TotalTaskReminder { get; set; }
        public int RFTransactions { get; set; }
        public int PendingTransactionsTotal { get; set; }
        public int ApprovalOfferBuyer { get; set; }
        public int TIApproval { get; set; }

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

        public string file { get; set; }

        //Queued for Amendments
        public int CustomerInformationFile { get; set; }
        public int ProgramConfiguration { get; set; }

        public int TotalQueuedforAmendments { get; set; }

        public int Total { get; set; }

        public int MainTotal { get; set; }

        public List<Dashboard_Model> ReminderDetail { get; set; }



        ///////////////////////////////AdminDashBoard_CompleteCustomerReg////////AdminDashBoard_InCompleteCustomerReg///////////////////////

        public string CompanyShortName { get; set; }

        public string TradeLicenceNo { get; set; }

        public string CustomerTypeName { get; set; }

        public string CompanyTypeName { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateBy { get; set; }


        /////////////////////AdminDashBoard_InCompleteProgramConfig/////////////////////////////////////

        public string ProgramConfigType { get; set; }
        public string FromCustomerType { get; set; }

        public string ToCIFNumber { get; set; }
        public string ToCompanyName { get; set; }
        public string ToCustomerType { get; set; }

        /////////////////////AdminDashBoard_CompleteUserReg/////////////////////////////////////

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string CustomerCode { get; set; }
        public string CIFNumber { get; set; }
        //  public string CompanyName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string StaffID { get; set; }
        public string DesignationName { get; set; }
        public string ServicePeriod { get; set; }
        public string MenuName { get; set; }


        ///////////////////////////Ti DashBoard//////////////////////////////////////////////////
        public int RFFundsDisbursed { get; set; }
        public int RFSettledTransactions { get; set; }
        public int RFDefaulted { get; set; }
        public int RFCompleted { get; set; }
        public int RFTotal { get; set; }


        public int FFundsDisbursed { get; set; }
        public int RFPendingTransactionsTotal { get; set; }


        public int FSettledTransactions { get; set; }
        public int FTransactionRequests { get; set; }
        public int FTransactionsRejected { get; set; }
        public int FTransactionsApprove { get; set; }
        public int FVolumesTillDate { get; set; }
        public decimal ObligorUtilizedLimit { get; set; }

        public decimal BuyerUtilizedLimit { get; set; }

        public decimal SupplierUtilizedLimit { get; set; }
        public decimal FunderUtilizedLimit { get; set; }
        public decimal InsuranceUtilizedLimit { get; set; }





        public decimal ObligorTotalLimit { get; set; }

        public decimal BuyerTotalLimit { get; set; }

        public decimal SupplierTotalLimit { get; set; }
        public decimal FunderTotalLimit { get; set; }
        public decimal InsuranceTotalLimit { get; set; }

        public decimal rObligorUtilizedLimit { get; set; }

        public decimal rBuyerUtilizedLimit { get; set; }

        public decimal rSupplierUtilizedLimit { get; set; }
        public decimal rFunderUtilizedLimit { get; set; }
        public decimal rInsuranceUtilizedLimit { get; set; }





        public decimal rObligorTotalLimit { get; set; }

        public decimal rBuyerTotalLimit { get; set; }

        public decimal rSupplierTotalLimit { get; set; }
        public decimal rFunderTotalLimit { get; set; }
        public decimal rInsuranceTotalLimit { get; set; }





        public int FDefaulted { get; set; }
        public int FCompleted { get; set; }
        public int FTotal { get; set; }

        public int RFInvoiceApproval { get; set; }
        public int RFApprovalOfferBuyer { get; set; }
        public int RFTIApproval { get; set; }
        public int RFCommodity { get; set; }
        public int RFFundFromTI { get; set; }

        public int RFPendingDisbursements { get; set; }

        public int RFPendingSettlement { get; set; }
        public int RFTransactionRequests { get; set; }
        public int RFTransactionsApprove { get; set; }
        public int RFTransactionsRejected { get; set; }
        public int RFVolumesTillDate { get; set; }





        public int RFPendingTranTotal { get; set; }


        public int FInvoiceApproval { get; set; }
        public int FPendingSettlement { get; set; }
        public int FPendingDisbursements { get; set; }

        public int FCommodity { get; set; }
        public int FPendingTransactionsTotal { get; set; }

        public int FPendingTranTotal { get; set; }


        public int FAssignmentFromObligor { get; set; }
        public int FApproveObligor { get; set; }
        public int FApproveDAT { get; set; }
        public int FFundReqToTI { get; set; }
        public int FFundFromTI { get; set; }





    }
}