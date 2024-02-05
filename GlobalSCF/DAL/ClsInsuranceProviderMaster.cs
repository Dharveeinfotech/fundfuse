using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsInsuranceProviderMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public List<CountryMaster> InsuranceProviderMaster_ListAll(Nullable<int> pInsuranceProviderID, string pInsuranceProviderName, Nullable<int> pCityID, Nullable<int> pStateID, Nullable<int> pCountryID, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InsuranceProviderMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int, pInsuranceProviderID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pInsuranceProviderName", SqlDbType.VarChar, pInsuranceProviderName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, pCityID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<CountryMaster>(dataReader as DbDataReader).ToList();
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
        public int InsuranceProviderMaster_Add(Nullable<int> pInsuranceProviderID, string pInsuranceProviderName, string pAddress1, string pAddress2, string pAddress3,
        Nullable<int> CityID, Nullable<int> pStateID, Nullable<int> pCountryID, string pContactName, string pTelNo, string pEmailID, string pMobileNo,
        string pWebSite, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InsuranceProviderMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderName", SqlDbType.VarChar, pInsuranceProviderName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress1", SqlDbType.VarChar, pAddress1);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress2", SqlDbType.VarChar, pAddress2);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress3", SqlDbType.VarChar, pAddress3);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pContactName", SqlDbType.VarChar, pContactName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, pMobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, pTelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.Char, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pWebSite", SqlDbType.VarChar, pWebSite);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pInsuranceProviderID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int InsuranceProviderMaster_Update(Nullable<int> pInsuranceProviderID, string pInsuranceProviderName, string pAddress1, string pAddress2,
        string pAddress3, Nullable<int> CityID, Nullable<int> pStateID, Nullable<int> pCountryID, string pContactName, string pTelNo, string pEmailID,
        string pMobileNo, string pWebSite, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InsuranceProviderMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int, pInsuranceProviderID);
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderName", SqlDbType.VarChar, pInsuranceProviderName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress1", SqlDbType.VarChar, pAddress1);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress2", SqlDbType.VarChar, pAddress2);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress3", SqlDbType.VarChar, pAddress3);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pContactName", SqlDbType.VarChar, pContactName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, pMobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, pTelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.Char, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pWebSite", SqlDbType.VarChar, pWebSite);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int InsuranceProviderMaster_Delete(Nullable<int> pInsuranceProviderID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("InsuranceProviderMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pInsuranceProviderID", SqlDbType.Int, pInsuranceProviderID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}