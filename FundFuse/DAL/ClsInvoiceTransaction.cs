using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TMP.Models;
using System.Data.Entity.Core.Objects;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsInvoiceTransaction
    {
        #region Local Variable
        ConString CN = new ConString();
        public SqlTransaction Tras { get; set; }
        public SqlConnection Conn { get; set; }
        Function FN = new Function();
        #endregion

        #region Invoice Master
        public int InvoiceMaster_Add(InvoiceTransactionModel _objModel)
        {
            if (_objModel.DueDate == null) { _objModel.DueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.DueDate == Convert.ToDateTime("1/1/0001")) { _objModel.DueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.FundReqDate == null) { _objModel.FundReqDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.FundReqDate == Convert.ToDateTime("1/1/0001")) { _objModel.FundReqDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pTranRefNo", SqlDbType.VarChar, _objModel.TranRefNo == null ? "" : _objModel.TranRefNo);
            ClsAppDatabase.AddInParameter(cmd, "@pDueDate", SqlDbType.DateTime, _objModel.DueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pFundReqDate", SqlDbType.DateTime, _objModel.FundReqDate);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsAppDatabase.AddInParameter(cmd, "@pExporterID", SqlDbType.Int, _objModel.ExporterID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsObligorGen", SqlDbType.Bit, _objModel.IsObligorGen);
            ClsAppDatabase.AddInParameter(cmd, "@pTenor", SqlDbType.Int, _objModel.Tenor);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pInvoiceID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceMaster_Update(InvoiceTransactionModel _objModel)
        {
            if (_objModel.DueDate == null) { _objModel.DueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.DueDate == Convert.ToDateTime("1/1/0001")) { _objModel.DueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.FundReqDate == null) { _objModel.FundReqDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.FundReqDate == Convert.ToDateTime("1/1/0001")) { _objModel.FundReqDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pDueDate", SqlDbType.DateTime, _objModel.DueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pFundReqDate", SqlDbType.DateTime, _objModel.FundReqDate);
            ClsAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsAppDatabase.AddInParameter(cmd, "@pExporterID", SqlDbType.Int, _objModel.ExporterID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsObligorGen", SqlDbType.Bit, _objModel.IsObligorGen);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pTenor", SqlDbType.Int, _objModel.Tenor);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceMaster_UpdateAllStatus(InvoiceTransactionModel _objModel)
        {
            if (_objModel.ExtDueDate == null) { _objModel.ExtDueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExtDueDate == Convert.ToDateTime("1/1/0001")) { _objModel.ExtDueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.FundReqDate == null) { _objModel.FundReqDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.FundReqDate == Convert.ToDateTime("1/1/0001")) { _objModel.FundReqDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceMaster_UpdateAllStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSupplierConf", SqlDbType.Bit, _objModel.IsSupplierConf);
            ClsAppDatabase.AddInParameter(cmd, "@pIsObligorConf", SqlDbType.Bit, _objModel.IsObligorConf);
            ClsAppDatabase.AddInParameter(cmd, "@pIsFunderConf", SqlDbType.Bit, _objModel.IsFunderConf);
            ClsAppDatabase.AddInParameter(cmd, "@pIsFunderAgree", SqlDbType.Bit, _objModel.IsFunderAgree);
            ClsAppDatabase.AddInParameter(cmd, "@pIsInsurance", SqlDbType.Bit, _objModel.IsInsurance);
            ClsAppDatabase.AddInParameter(cmd, "@pIsBuyerConf", SqlDbType.Bit, _objModel.IsBuyerConf);
            ClsAppDatabase.AddInParameter(cmd, "@pIsInvOffer", SqlDbType.Bit, _objModel.IsInvOffer);
            ClsAppDatabase.AddInParameter(cmd, "@pFunderID", SqlDbType.Int, _objModel.FunderID);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceID", SqlDbType.Int, _objModel.InsuranceID);
            ClsAppDatabase.AddInParameter(cmd, "@pServiceProviderID", SqlDbType.Int, _objModel.ServiceProviderID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pExtDueDate", SqlDbType.DateTime, _objModel.ExtDueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pFundReqDate", SqlDbType.DateTime, _objModel.FundReqDate);
            ClsAppDatabase.AddInParameter(cmd, "@pFunProfitRate", SqlDbType.Decimal, _objModel.FunProfitRate);
            ClsAppDatabase.AddInParameter(cmd, "@pFunProfitAmt", SqlDbType.Decimal, _objModel.FunProfitAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceMaster_UpdateSettlement(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceMaster_UpdateSettlement");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceMaster_UpdatePaymentFees(InvoiceTransactionModel _objModel)
        {
            if (_objModel.PayProfitDate == Convert.ToDateTime("1/1/0001")) { _objModel.PayProfitDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PayProcessDate == Convert.ToDateTime("1/1/0001")) { _objModel.PayProcessDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceMaster_UpdatePaymentFees");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pPayBankID", SqlDbType.Int, _objModel.PayBankID);
            ClsAppDatabase.AddInParameter(cmd, "@pPayAccountNo", SqlDbType.VarChar, _objModel.PayAccountNo == null ? "" : _objModel.PayAccountNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPayCurrencyID", SqlDbType.Int, _objModel.PayCurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pPayProfitRefNo", SqlDbType.VarChar, _objModel.PayProfitRefNo == null ? "" : _objModel.PayProfitRefNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPayProfitDate", SqlDbType.DateTime, _objModel.PayProfitDate);
            ClsAppDatabase.AddInParameter(cmd, "@pPayProcessRefNo", SqlDbType.VarChar, _objModel.PayProcessRefNo == null ? "" : _objModel.PayProcessRefNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPayProcessDate", SqlDbType.DateTime, _objModel.PayProcessDate);
            ClsAppDatabase.AddInParameter(cmd, "@pPayCurrencyRate", SqlDbType.Decimal, _objModel.PayCurrencyRate);
            ClsAppDatabase.AddInParameter(cmd, "@pPayProfitAmt", SqlDbType.Decimal, _objModel.PayProfitAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pPayProcessFeeAmt", SqlDbType.Decimal, _objModel.PayProcessFeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoicePayDetail_Add(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoicePayDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInvoicePayID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pTypeDesc", SqlDbType.VarChar, _objModel.TypeDesc == null ? "" : _objModel.TypeDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pTypeStatus", SqlDbType.VarChar, _objModel.TypeStatus == null ? "" : _objModel.TypeStatus);
            if (_objModel.PayDate == null) { _objModel.PayDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PayDate == Convert.ToDateTime("1/1/0001")) { _objModel.PayDate = Convert.ToDateTime("01/01/1900"); }
            ClsAppDatabase.AddInParameter(cmd, "@pPayDate", SqlDbType.DateTime, _objModel.PayDate);
            ClsAppDatabase.AddInParameter(cmd, "@pRefNo", SqlDbType.VarChar, _objModel.RefNo == null ? "" : _objModel.RefNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAmt", SqlDbType.Decimal, _objModel.Amt);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pInvoicePayID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceMaster_UpdatePenSettlement(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceMaster_UpdatePenSettlement");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        public List<InvoiceTransactionModel> InvoiceMaster_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorBuyerID", SqlDbType.Int, _objModel.ObligorBuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFunderID", SqlDbType.Int, _objModel.FunderID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInsuranceID", SqlDbType.Int, _objModel.InsuranceID);
            if (_objModel.IsInsuranceVal == 0) { _objModel.IsInsuranceVal = -1; }
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsInsuranceVal", SqlDbType.SmallInt,_objModel.IsInsuranceVal);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsObligorGen", SqlDbType.Int, _objModel.ObligorGen);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFromTranDate", SqlDbType.VarChar, _objModel.FromTranDate == null ? "" : _objModel.FromTranDate);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pToTranDate", SqlDbType.VarChar, _objModel.ToTranDate == null ? "" : _objModel.ToTranDate);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, _objModel.CreateBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceMasterHistory_ListAllBind(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMasterHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFunderID", SqlDbType.Int, _objModel.FunderID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInsuranceID", SqlDbType.Int, _objModel.InsuranceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFromTranDate", SqlDbType.VarChar, _objModel.FromTranDate == null ? "" : _objModel.FromTranDate);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pToTranDate", SqlDbType.VarChar, _objModel.ToTranDate == null ? "" : _objModel.ToTranDate);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceMaster_UpdateCalculation(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_UpdateCalculation");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsUpdate", SqlDbType.Bit, _objModel.IsUpdate);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceMaster_BindCompany(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_BindCompany");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorBuyerID", SqlDbType.Int, _objModel.ObligorBuyerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceMaster_BindCurrency(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_BindCurrency");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorBuyerID", SqlDbType.Int, _objModel.ObligorBuyerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> CustomerAccDetail_GetByInvoice(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerAccDetail_GetByInvoice");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceMaster_ExtendedTenor(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_ExtendedTenor");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            if (_objModel.ExtDueDate == null) { _objModel.ExtDueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExtDueDate == Convert.ToDateTime("1/1/0001")) { _objModel.ExtDueDate = Convert.ToDateTime("01/01/1900"); }
            ClsEntityAppDatabase.AddInParameter(cmd, "@pExtDueDate", SqlDbType.DateTime, _objModel.ExtDueDate);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceMaster_FunProfitAmt(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_FunProfitAmt");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFunProfitRate", SqlDbType.Decimal, _objModel.FunProfitRate);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        public List<InvoiceTransactionModel> InvoiceMaster_ListAllSettlement(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_ListAllSettlement");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceMaster_ListAllPenSettlement(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_ListAllPenSettlement");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, _objModel.CreateBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        public DataTable InvoiceMaster_DueDateHoliday(DateTime DueDate)
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceMaster_DueDateHoliday");
            ClsAppDatabase.AddInParameter(cmd, "@pDueDate", SqlDbType.DateTime, DueDate);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.Transaction = Tras;
            ad.Fill(dtTable);
            cmd.Dispose();
            return dtTable;
        }
        #endregion

        #region Invoice Detail
        public int InvoiceDetail_Add(InvoiceTransactionModel _objModel)
        {
            if (_objModel.InvoiceDate == null) { _objModel.InvoiceDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.InvoiceDate == Convert.ToDateTime("1/1/0001")) { _objModel.InvoiceDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInvoiceDetID", SqlDbType.Int, _objModel.InvoiceDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceNo", SqlDbType.VarChar, _objModel.InvoiceNo == null ? "" : _objModel.InvoiceNo);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceDate", SqlDbType.DateTime, _objModel.InvoiceDate);
            ClsAppDatabase.AddInParameter(cmd, "@pAmt", SqlDbType.Decimal, _objModel.Amt);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pInvoiceDetID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceDetail_Update(InvoiceTransactionModel _objModel)
        {
            if (_objModel.InvoiceDate == null) { _objModel.InvoiceDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.InvoiceDate == Convert.ToDateTime("1/1/0001")) { _objModel.InvoiceDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceDetID", SqlDbType.Int, _objModel.InvoiceDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceNo", SqlDbType.VarChar, _objModel.InvoiceNo == null ? "" : _objModel.InvoiceNo);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceDate", SqlDbType.DateTime, _objModel.InvoiceDate);
            ClsAppDatabase.AddInParameter(cmd, "@pAmt", SqlDbType.Decimal, _objModel.Amt);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceDetail_Delete(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceDetID", SqlDbType.Int, _objModel.InvoiceDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<InvoiceTransactionModel> InvoiceDetail_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceDetID", SqlDbType.Int, _objModel.InvoiceDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceMasterHistory_Remarks(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMasterHistory_Remarks");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            //ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            //ClsEntityAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            //ClsEntityAppDatabase.AddInParameter(cmd, "@pFunderID", SqlDbType.Int, _objModel.FunderID);
            //ClsEntityAppDatabase.AddInParameter(cmd, "@pInsuranceID", SqlDbType.Int, _objModel.InsuranceID);
            //ClsEntityAppDatabase.AddInParameter(cmd, "@pServiceProviderID", SqlDbType.Int, _objModel.ServiceProviderID);
            //ClsEntityAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> CustomerAccDetail_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerAccDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoicePayDetail_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoicePayDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoicePayID", SqlDbType.Int, _objModel.InvoicePayID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status.Trim());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        #endregion

        #region Invoice Ins Detail
        public int InvoiceInsDetail_Add(InvoiceTransactionModel _objModel)
        {
            if (_objModel.IssueDate == Convert.ToDateTime("1/1/0001")) { _objModel.IssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.ExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceInsDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInvoiceInsID", SqlDbType.Int, _objModel.InvoiceInsID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceID", SqlDbType.Int, _objModel.InsuranceID);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceBrokerID", SqlDbType.Int, _objModel.InsuranceBrokerID);
            ClsAppDatabase.AddInParameter(cmd, "@pPolicyNo", SqlDbType.VarChar, _objModel.PolicyNo == null ? "" : _objModel.PolicyNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIssueDate", SqlDbType.DateTime, _objModel.IssueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pExpDate", SqlDbType.DateTime, _objModel.ExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pInsAmt", SqlDbType.Decimal, _objModel.InsAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pPremPer", SqlDbType.Decimal, _objModel.PremPer);
            ClsAppDatabase.AddInParameter(cmd, "@pPremAmt", SqlDbType.Decimal, _objModel.PremAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pInvoiceInsID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceInsDetail_Update(InvoiceTransactionModel _objModel)
        {
            if (_objModel.IssueDate == Convert.ToDateTime("1/1/0001")) { _objModel.IssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.ExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceInsDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceInsID", SqlDbType.Int, _objModel.InvoiceInsID);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceID", SqlDbType.Int, _objModel.InsuranceID);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceBrokerID", SqlDbType.Int, _objModel.InsuranceBrokerID);
            ClsAppDatabase.AddInParameter(cmd, "@pPolicyNo", SqlDbType.VarChar, _objModel.PolicyNo == null ? "" : _objModel.PolicyNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIssueDate", SqlDbType.DateTime, _objModel.IssueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pExpDate", SqlDbType.DateTime, _objModel.ExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pInsAmt", SqlDbType.Decimal, _objModel.InsAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pPremPer", SqlDbType.Decimal, _objModel.PremPer);
            ClsAppDatabase.AddInParameter(cmd, "@pPremAmt", SqlDbType.Decimal, _objModel.PremAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceInsDetail_Delete(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceInsDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceInsID", SqlDbType.Int, _objModel.InvoiceInsID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<InvoiceTransactionModel> InvoiceInsDetail_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceInsDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceInsID", SqlDbType.Int, _objModel.InvoiceInsID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInsuranceID", SqlDbType.Int, _objModel.InsuranceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsSelect", SqlDbType.Int, _objModel.IsSelect);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvoiceAmtDetail_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceAmtDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceAmtID", SqlDbType.Int, _objModel.InvoiceAmtID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public int InvoiceInsDetail_UpdateSelected(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceInsDetail_UpdateSelected");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceInsID", SqlDbType.Int, _objModel.InvoiceInsID);
            ClsAppDatabase.AddOutParameter(cmd, "@pInsuranceID", SqlDbType.Int, _objModel.InsuranceID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pInsuranceID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        #endregion

        #region Invoice Com Detail
        public int InvoiceComDetail_Add(InvoiceTransactionModel _objModel)
        {
            if (_objModel.TranDate == Convert.ToDateTime("1/1/0001")) { _objModel.TranDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceComDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInvoiceComID", SqlDbType.Int, _objModel.InvoiceComID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            ClsAppDatabase.AddInParameter(cmd, "@pComLevel", SqlDbType.VarChar, _objModel.ComLevel == null ? "" : _objModel.ComLevel);
            ClsAppDatabase.AddInParameter(cmd, "@pRefNo", SqlDbType.VarChar, _objModel.RefNo == null ? "" : _objModel.RefNo);
            ClsAppDatabase.AddInParameter(cmd, "@pTranDate", SqlDbType.DateTime, _objModel.TranDate);
            ClsAppDatabase.AddInParameter(cmd, "@pChargeAmt", SqlDbType.Decimal, _objModel.ChargeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pCertificateNo", SqlDbType.VarChar, _objModel.CertificateNo == null ? "" : _objModel.CertificateNo);
            ClsAppDatabase.AddInParameter(cmd, "@pQtyComm", SqlDbType.Decimal, _objModel.QtyComm);
            ClsAppDatabase.AddInParameter(cmd, "@pCommValue", SqlDbType.VarChar, _objModel.CommValue == null ? "" : _objModel.CommValue);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pInvoiceComID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvoiceComDetail_Update(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceComDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceComID", SqlDbType.Int, _objModel.InvoiceComID);
            ClsAppDatabase.AddInParameter(cmd, "@pComLevel", SqlDbType.VarChar, _objModel.ComLevel == null ? "" : _objModel.ComLevel);
            ClsAppDatabase.AddInParameter(cmd, "@pRefNo", SqlDbType.VarChar, _objModel.RefNo == null ? "" : _objModel.RefNo);
            ClsAppDatabase.AddInParameter(cmd, "@pTranDate", SqlDbType.DateTime, _objModel.TranDate);
            ClsAppDatabase.AddInParameter(cmd, "@pChargeAmt", SqlDbType.Decimal, _objModel.ChargeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pCertificateNo", SqlDbType.VarChar, _objModel.CertificateNo == null ? "" : _objModel.CertificateNo);
            ClsAppDatabase.AddInParameter(cmd, "@pQtyComm", SqlDbType.Decimal, _objModel.QtyComm);
            ClsAppDatabase.AddInParameter(cmd, "@pCommValue", SqlDbType.VarChar, _objModel.CommValue == null ? "" : _objModel.CommValue);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<InvoiceTransactionModel> InvoiceComDetail_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceComDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        #endregion

        #region invoice Temp Master
        public int InvTempMaster_Add(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvTempMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pInvTempID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvTempDetail_Add(InvoiceTransactionModel _objModel)
        {
            if (_objModel.DueDate == null) { _objModel.DueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.DueDate == Convert.ToDateTime("1/1/0001")) { _objModel.DueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.InvoiceDate == null) { _objModel.InvoiceDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.InvoiceDate == Convert.ToDateTime("1/1/0001")) { _objModel.InvoiceDate = Convert.ToDateTime("01/01/1900"); }

            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvTempDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInvTempDetID", SqlDbType.Int, _objModel.InvTempDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pDueDate", SqlDbType.DateTime, _objModel.DueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pTenor", SqlDbType.Int, _objModel.Tenor);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyCode", SqlDbType.VarChar, _objModel.CurrencyCode == null ? "" : _objModel.CurrencyCode.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pObligorCode", SqlDbType.VarChar, _objModel.ObligorCode == null ? "" : _objModel.ObligorCode.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pObligorName", SqlDbType.VarChar, _objModel.ObligorName == null ? "" : _objModel.ObligorName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSupplierCode", SqlDbType.VarChar, _objModel.SupplierCode == null ? "" : _objModel.SupplierCode.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSupplierName", SqlDbType.VarChar, _objModel.SupplierName == null ? "" : _objModel.SupplierName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pBuyerCode", SqlDbType.VarChar, _objModel.BuyerCode == null ? "" : _objModel.BuyerCode.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pBuyerName", SqlDbType.VarChar, _objModel.BuyerName == null ? "" : _objModel.BuyerName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pExporterCode", SqlDbType.VarChar, _objModel.ExporterCode == null ? "" : _objModel.ExporterCode.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pExporterName", SqlDbType.VarChar, _objModel.ExporterName == null ? "" : _objModel.ExporterName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceNo", SqlDbType.VarChar, _objModel.InvoiceNo == null ? "" : _objModel.InvoiceNo);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceDate", SqlDbType.DateTime, _objModel.InvoiceDate);
            ClsAppDatabase.AddInParameter(cmd, "@pAmt", SqlDbType.Decimal, _objModel.Amt);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pInvTempDetID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvTempDetail_Delete(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvTempDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pInvTempDetID", SqlDbType.Int, _objModel.InvTempDetID);
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvTempDetail_InvProcess(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvTempDetail_InvProcess");
            ClsAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvTempDetail_Update(InvoiceTransactionModel _objModel)
        {
            if (_objModel.DueDate == null) { _objModel.DueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.DueDate == Convert.ToDateTime("1/1/0001")) { _objModel.DueDate = Convert.ToDateTime("01/01/1900"); }

            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvTempDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pInvTempDetID", SqlDbType.Int, _objModel.InvTempDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pDueDate", SqlDbType.DateTime, _objModel.DueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pTenor", SqlDbType.Int, _objModel.Tenor);
            ClsAppDatabase.AddInParameter(cmd, "@pObligorName", SqlDbType.VarChar, _objModel.ObligorName == null ? "" : _objModel.ObligorName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSupplierName", SqlDbType.VarChar, _objModel.SupplierName == null ? "" : _objModel.SupplierName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pBuyerName", SqlDbType.VarChar, _objModel.BuyerName == null ? "" : _objModel.BuyerName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pExporterName", SqlDbType.VarChar, _objModel.ExporterName == null ? "" : _objModel.ExporterName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach.Trim());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        public int InvTempMaster_Delete(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvTempMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InvTempMaster_Generate(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvTempMaster_Generate");
            ClsAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }


        public List<InvoiceTransactionModel> InvTempMaster_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvTempMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvTempDetail_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvTempDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvTempDetID", SqlDbType.Int, _objModel.InvTempDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSeqNo ", SqlDbType.Int, _objModel.SeqNo);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsVerified", SqlDbType.SmallInt, _objModel.pIsVerified);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsDeleted", SqlDbType.SmallInt, _objModel.pIsDeleted);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> InvTempDetail_FileVerify(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvTempDetail_FileVerify");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pAttachs", SqlDbType.VarChar, _objModel.Attachs == null ? "" : _objModel.Attachs);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public int InvTempDetail_FileVerifyint(InvoiceTransactionModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvTempDetail_FileVerify");
            ClsAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pAttachs", SqlDbType.VarChar, _objModel.Attachs == null ? "" : _objModel.Attachs.Trim());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        public List<InvoiceTransactionModel> InvTempMaster_ListAllBind(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvTempMaster_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInvTempID", SqlDbType.Int, _objModel.InvTempID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }


        #endregion

        #region Send Template Notification Methods
        public string GetInvoiceMasterData(int InvoiceID, string HTMLText)
        {
            DataTable DT = new DataTable();
            string ViewSample = "";
            DT = InvoiceHTMLDetail(InvoiceID);
            foreach (DataRow dr in DT.Rows)
            {
                ViewSample = GetText(dr, HTMLText);
            }
            return ViewSample;
        }
        public DataTable InvoiceHTMLDetail(int intInvoiceNo, int intUserID = 0)
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceMaster_HTMLDetail");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, intInvoiceNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, intUserID);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.Transaction = Tras;
            ad.Fill(dtTable);
            cmd.Dispose();
            return dtTable;
        }
        public DataTable InvoiceDetail_ListAllDT(InvoiceTransactionModel _objModel)
        {
            DataTable dtTable = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("InvoiceDetail_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceDetID", SqlDbType.Int, _objModel.InvoiceDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, _objModel.InvoiceID);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.Transaction = Tras;
            ad.Fill(dtTable);
            cmd.Dispose();
            return dtTable;
        }
        public string GetText(DataRow dr, string msg)
        {
            string strSMS = msg;
            while (strSMS.Contains("{"))
            {
                string ColumnName = strSMS.Substring(strSMS.IndexOf("{") + 1, strSMS.IndexOf("}") - strSMS.IndexOf("{") - 1);
                var a = ColumnName;
                ColumnName = RemoveBetwee(ColumnName, '<', '>');
                ColumnName = ColumnName.Replace("&nbsp;", " ");
                strSMS = strSMS.Replace(a, ColumnName);
                strSMS = strSMS.Replace("{" + ColumnName + "}", GetPlainText(dr[ColumnName], dr.Table.Columns[ColumnName].DataType.ToString()).Trim());
            }
            return strSMS;
        }
        string RemoveBetwee(string s, char begin, char end)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }
        public string GetPlainText(object obj, string Type)
        {
            if (Type.Equals("System.DateTime"))
            {
                return Convert.ToDateTime(obj.ToString()).Date.ToString("dd-MMM-yyyy");
            }

            if (Type.Equals("System.Decimal"))
            {
                return Convert.ToDecimal(obj.ToString()).ToString("#,##0.00");
            }
            return obj.ToString();
        }
        #endregion

        #region Dashboard
        //public DataSet CustomerDashboardTable(InvoiceTransactionModel _objModel)
        //{
        //    DataSet ds = new DataSet();
        //    SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerDashboard");
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
        //    SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //    ad.Fill(ds);
        //    cmd.Connection.Close();
        //    cmd.Dispose();
        //    return ds;
        //}
        public DataSet CustomerDashboardTable(InvoiceTransactionModel _objModel)
        {
            DataSet DS = new DataSet();
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerDashboard");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            DbDataAdapter DataAdpt = DbProviderFactories.GetFactory("System.Data.SqlClient").CreateDataAdapter();
            try
            {
                DataAdpt.SelectCommand = cmd;
                DataAdpt.Fill(DS);
                return DS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataAdpt.Dispose();
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        public List<InvoiceTransactionModel> UserRole_GetDetail(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserRole_GetDetail");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> CustomerObligorDetail_ListAll(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerObligorDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSupplierID", SqlDbType.Int, _objModel.SupplierID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorID", SqlDbType.Int, _objModel.ObligorID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBuyerID", SqlDbType.Int, _objModel.BuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pObligorBuyerID", SqlDbType.Int, _objModel.ObligorBuyerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCompanyID", SqlDbType.Int, _objModel.CompanyID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }

        #endregion

        #region Deshboard Portfolio
        public List<InvoiceTransactionModel> CustomerPortFolio(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerPortFolio");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCurrencyId", SqlDbType.Int, _objModel.CurrencyID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        public List<InvoiceTransactionModel> Dashboard_BindCurrency(InvoiceTransactionModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("Dashboard_BindCurrency");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InvoiceTransactionModel>(dataReader as DbDataReader).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        #endregion
    }
}