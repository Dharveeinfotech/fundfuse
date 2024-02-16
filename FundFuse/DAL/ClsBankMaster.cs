using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

using System.Data.Entity.Core.Objects;
using TMP.Models;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsBankMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        Function fn = new Function();
        public List<CountryMaster> BankMaster_ListAll(Nullable<int> pBankID, string pBankName,
        Nullable<int> pCityID, Nullable<int> pStateID, Nullable<int> pCountryID, Nullable<short> pIsActive,
        string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("BankMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, pBankID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBankName", SqlDbType.VarChar, pBankName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, pCityID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            
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
        public int BankMaster_Add(Nullable<int> pBankID, string pBankName, string pAddress1, string pAddress2, string pAddress3, Nullable<int> CityID,
            Nullable<int> pStateID, Nullable<int> pCountryID, string pContactPerson, string pEmailID, string pMobileNo, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("BankMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pBankID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pBankName", SqlDbType.VarChar, pBankName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress1", SqlDbType.VarChar, pAddress1);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress2", SqlDbType.VarChar, pAddress2);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress3", SqlDbType.VarChar, pAddress3);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pContactPerson", SqlDbType.Char, pContactPerson);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.Char, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.Char, pMobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, fn.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pBankID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int BankMaster_Update(Nullable<int> pBankID, string pBankName, string pAddress1, string pAddress2, string pAddress3, Nullable<int> CityID,
            Nullable<int> pStateID, Nullable<int> pCountryID, string @pContactPerson, string pEmailID, string pMobileNo, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("BankMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, pBankID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankName", SqlDbType.VarChar, pBankName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress1", SqlDbType.VarChar, pAddress1);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress2", SqlDbType.VarChar, pAddress2);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress3", SqlDbType.VarChar, pAddress3);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pContactPerson", SqlDbType.Char, pContactPerson);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.Char, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.Char, pMobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, fn.GetSystemIP());
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int BankMaster_Delete(Nullable<int> pBankID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("BankMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, pBankID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}