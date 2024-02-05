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
    public class ClsCurrency
    {
        #region Local Variable
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        #endregion
        public List<CountryMaster> CurrencyMaster_ListAll(Nullable<int> pCurrencyID, string pCurrencyCode, string pCurrencyName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {  
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CurrencyMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, pCurrencyID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCurrencyCode", SqlDbType.VarChar, pCurrencyCode);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCurrencyName", SqlDbType.VarChar, pCurrencyName);
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
        public int CurrencyMaster_Add(Nullable<int> pCurrencyID, string pCurrencyCode, string pCurrencyName, string pCurrencySymbol, string pProcessRemark, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CurrencyMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCurrencyID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyCode", SqlDbType.Char, pCurrencyCode);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyName", SqlDbType.VarChar, pCurrencyName);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencySymbol", SqlDbType.VarChar, pCurrencySymbol);            
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCurrencyID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int CurrencyMaster_Update(int pCurrencyID, string pCurrencyCode, string pCurrencyName, string pCurrencySymbol, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CurrencyMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, pCurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyCode", SqlDbType.VarChar, pCurrencyCode);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyName", SqlDbType.VarChar, pCurrencyName);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencySymbol", SqlDbType.VarChar, pCurrencySymbol);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int CurrencyMaster_Delete(Nullable<int> pCurrencyID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CurrencyMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, pCurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}