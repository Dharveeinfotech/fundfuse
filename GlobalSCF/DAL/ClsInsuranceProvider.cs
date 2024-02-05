using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TMP.Models;

namespace TMP.DAL
{
    public class ClsInsuranceProvider
    {
        #region Local Variable
        ConString CN = new ConString();
        public SqlTransaction Tras { get; set; }
        public SqlConnection Conn { get; set; }
        Function FN = new Function();
        #endregion

        #region Company Information Methods
        public int InsuranceProviderMaster_Add(InsuranceProvider _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InsuranceProviderMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderName", SqlDbType.VarChar, _objModel.InsuranceProviderName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress1", SqlDbType.VarChar, _objModel.Address1);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress2", SqlDbType.VarChar, _objModel.Address2);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress3", SqlDbType.VarChar, _objModel.Address3);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, _objModel.StateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pContactName", SqlDbType.VarChar, _objModel.ContactName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pWebSite", SqlDbType.VarChar, _objModel.WebSite);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pInsuranceProviderID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InsuranceProviderMaster_Update(InsuranceProvider _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InsuranceProviderMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int, _objModel.InsuranceProviderID);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderName", SqlDbType.VarChar, _objModel.InsuranceProviderName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress1", SqlDbType.VarChar, _objModel.Address1);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress2", SqlDbType.VarChar, _objModel.Address2);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress3", SqlDbType.VarChar, _objModel.Address3);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, _objModel.StateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pContactName", SqlDbType.VarChar, _objModel.ContactName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pWebSite", SqlDbType.VarChar, _objModel.WebSite);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InsuranceProviderMaster_Delete(InsuranceProvider _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InsuranceProviderMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int, _objModel.InsuranceProviderID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int InsuranceProviderMasterProcessHistory_Add(InsuranceProvider _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InsuranceProviderMasterProcessHistory_Add");
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int, _objModel.InsuranceProviderID);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceCode", SqlDbType.VarChar, _objModel.InsuranceCode);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderName", SqlDbType.VarChar, _objModel.InsuranceProviderName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress1", SqlDbType.VarChar, _objModel.Address1);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress2", SqlDbType.VarChar, _objModel.Address2);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress3", SqlDbType.VarChar, _objModel.Address3);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, _objModel.StateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pContactName", SqlDbType.VarChar, _objModel.ContactName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pWebSite", SqlDbType.VarChar, _objModel.WebSite);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pInsuranceProviderProcessHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<InsuranceProvider> InsuranceProviderMaster_ListAll(InsuranceProvider _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InsuranceProviderMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int, _objModel.InsuranceProviderID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInsuranceProviderName", SqlDbType.VarChar, _objModel.InsuranceProviderName == null ? "" : _objModel.InsuranceProviderName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, _objModel.StateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, _objModel.IsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<InsuranceProvider>(dataReader as DbDataReader).ToList();
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