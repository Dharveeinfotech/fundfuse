using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using TMP.Models;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsSMSConfig
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public List<CountryMaster> SMSConfiguration_ListAll(Nullable<int> pSMSConfigID, bool pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("SMSConfiguration_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSMSConfigID", SqlDbType.Int, pSMSConfigID);            
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
        public int SMSConfiguration_Add(int pSMSConfigID, string pURL,string pUserName, string pPwd, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("SMSConfiguration_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pSMSConfigID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pURL", SqlDbType.VarChar, pURL);
            ClsAppDatabase.AddInParameter(cmd, "@pUserName", SqlDbType.VarChar, pUserName);
            ClsAppDatabase.AddInParameter(cmd, "@pPwd", SqlDbType.VarChar, pPwd);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pSMSConfigID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int SMSConfiguration_Delete(Nullable<int> pSMSConfigID)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("SMSConfiguration_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pSMSConfigID", SqlDbType.Int, pSMSConfigID);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int SMSConfiguration_Update(int pSMSConfigID, string pURL,string pUserName, string pPwd, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("SMSConfiguration_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pSMSConfigID", SqlDbType.Int, pSMSConfigID);
            ClsAppDatabase.AddInParameter(cmd, "@pURL", SqlDbType.VarChar,  pURL);
            ClsAppDatabase.AddInParameter(cmd, "@pUserName", SqlDbType.VarChar, pUserName);
            ClsAppDatabase.AddInParameter(cmd, "@pPwd", SqlDbType.VarChar, pPwd);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pSMSConfigID"].Value);
            cmd.Dispose();
            return blnResult;
        }
    }
}