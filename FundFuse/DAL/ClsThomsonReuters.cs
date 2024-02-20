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
    public class ClsThomsonReuters
    {
        #region Local Variable
        ConString CN = new ConString();
        public SqlTransaction Tras { get; set; }
        public SqlConnection Conn { get; set; }
        Function FN = new Function();
        #endregion
        public int APILogDetail_Add(ThomsonReutersModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("APILogDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pLogID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pLogType", SqlDbType.VarChar, _objModel.LogType == null ? "" : _objModel.LogType.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pcaseId", SqlDbType.VarChar, _objModel.caseId == null ? "" : _objModel.caseId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSCID", SqlDbType.Int, _objModel.SCID);
            ClsAppDatabase.AddInParameter(cmd, "@pRequest", SqlDbType.NVarChar, _objModel.Request == null ? "" : _objModel.Request.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRequestBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pRequestIP", SqlDbType.VarChar, _objModel.RequestIP == null ? "" : _objModel.RequestIP.Trim());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pLogID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int APILogDetail_Update(ThomsonReutersModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("APILogDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pLogID", SqlDbType.Int, _objModel.LogID);
            ClsAppDatabase.AddInParameter(cmd, "@pResponse", SqlDbType.NVarChar, _objModel.Response == null ? "" : _objModel.Response.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsSuccess", SqlDbType.Bit, _objModel.IsSuccess);
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        //public int ScreeningMaster_Add(ThomsonReutersModel _objModel)
        //{
        //    int blnResult = 0;
        //    SqlCommand cmd = ClsAppDatabase.GetSPName("ScreeningMaster_Add");
        //    ClsAppDatabase.AddOutParameter(cmd, "@pSCID", SqlDbType.Int);
        //    ClsAppDatabase.AddInParameter(cmd, "@pPlatformID", SqlDbType.Int, _objModel.PlatformID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pcaseId", SqlDbType.VarChar, _objModel.caseId == null ? "" : _objModel.caseId.Trim());
        //    ClsAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
        //    ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
        //    cmd.Transaction = Tras;
        //    int Row = cmd.ExecuteNonQuery();
        //    blnResult = Convert.ToInt32(cmd.Parameters["@pSCID"].Value);
        //    cmd.Parameters.Clear();
        //    cmd.Dispose();
        //    return blnResult;
        //}

        public string ScreeningMaster_Add(ThomsonReutersModel _objModel)
        {
            string blnResult = "";
            SqlCommand cmd = ClsAppDatabase.GetSPName("ScreeningMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pSCID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pPlatformID", SqlDbType.Int, _objModel.PlatformID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.VarChar, _objModel.CustType == null ? "" : _objModel.CustType.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompany", SqlDbType.Bit, _objModel.IsCompany);
            ClsAppDatabase.AddOutParameter(cmd, "@pcaseId", SqlDbType.VarChar);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToString(cmd.Parameters["@pcaseId"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ScreeningDetail_Add(ThomsonReutersModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("ScreeningDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pSCDetID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pSCID", SqlDbType.Int, _objModel.SCID);
            ClsAppDatabase.AddInParameter(cmd, "@pcaseId", SqlDbType.VarChar, _objModel.caseId == null ? "" : _objModel.caseId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@presultId", SqlDbType.VarChar, _objModel.resultId == null ? "" : _objModel.resultId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pstatusId", SqlDbType.VarChar, _objModel.statusId == null ? "" : _objModel.statusId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@priskId", SqlDbType.VarChar, _objModel.riskId == null ? "" : _objModel.riskId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@preasonId", SqlDbType.VarChar, _objModel.reasonId == null ? "" : _objModel.reasonId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@presolutionRemark", SqlDbType.VarChar, _objModel.resolutionRemark == null ? "" : _objModel.resolutionRemark.Trim());
            if (_objModel.presolutionDate == null) { _objModel.presolutionDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.presolutionDate == Convert.ToDateTime("1/1/0001")) { _objModel.presolutionDate = Convert.ToDateTime("01/01/1900"); }
            ClsAppDatabase.AddInParameter(cmd, "@presolutionDate", SqlDbType.DateTime, _objModel.presolutionDate);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pSCDetID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ScreeningMaster_Update(ThomsonReutersModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("ScreeningMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pcaseId", SqlDbType.VarChar, _objModel.caseId == null ? "" : _objModel.caseId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ScreeningMaster_UpdateResolved(ThomsonReutersModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("ScreeningMaster_UpdateResolved");
            ClsAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ScreeningMaster_UpdateOGS(ThomsonReutersModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("ScreeningMaster_UpdateOGS");
            ClsAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsOGS", SqlDbType.Bit, _objModel.IsOGS);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ScreeningMaster_UpdateDeleted(ThomsonReutersModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("ScreeningMaster_UpdateDeleted");
            ClsAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<ThomsonReutersModel> ScreeningMaster_VerifycaseID(ThomsonReutersModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("ScreeningMaster_VerifycaseID");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pPlatformID", SqlDbType.Int, _objModel.PlatformID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.VarChar, _objModel.CustType == null ? "" : _objModel.CustType.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pLocCountryCode", SqlDbType.VarChar, _objModel.LocCountryCode == null ? "" : _objModel.LocCountryCode.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pNatCountryCode", SqlDbType.VarChar, _objModel.NatCountryCode == null ? "" : _objModel.NatCountryCode.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender.Trim());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ThomsonReutersModel>(dataReader as DbDataReader).ToList();
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

        public List<ThomsonReutersModel> ScreeningDetail_ListAll(ThomsonReutersModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("ScreeningDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pPlatformID", SqlDbType.Int, _objModel.PlatformID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ThomsonReutersModel>(dataReader as DbDataReader).ToList();
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
        public List<ThomsonReutersModel> ScreeningDetail_ListAllcaseSystemId(ThomsonReutersModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("ScreeningDetail_ListAllcaseSystemId");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ThomsonReutersModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerShareHolderModel> ScreeningCustomer_ListAll(CustomerShareHolderModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("ScreeningCustomer_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pPlatformID", SqlDbType.Int, _objModel.PlatformID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.VarChar, _objModel.CustType == null ? "" : _objModel.CustType.Trim());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerShareHolderModel>(dataReader as DbDataReader).ToList();
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
        public List<ThomsonReutersModel> ScreeningMaster_ListAll(ThomsonReutersModel _objModel)
        {            
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("ScreeningMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsCompany", SqlDbType.SmallInt, _objModel.intIsCompany);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pcaseSystemId", SqlDbType.VarChar, _objModel.caseSystemId == null ? "" : _objModel.caseSystemId.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pcaseId", SqlDbType.VarChar, _objModel.caseId == null ? "" : _objModel.caseId.Trim());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN)  .ObjectContext.Translate<ThomsonReutersModel>(dataReader as DbDataReader).ToList();
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
    }
}