using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using TMP.Models;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace TMP.DAL
{
    public class ClsEmailConfiguration
    {
        ConString db = new ConString();
        Function FN = new Function();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public int EmailConfiguration_Add(int pEmailConfigID, string pSmtpName,
        string pUserName, string pPwd, string pFromEmailID, string pCCEmailID, string pBCCEmailID, string pDisplayName, int pInPort, int pOutPort, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("EmailConfiguration_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pEmailConfigID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pSmtpName", SqlDbType.VarChar, pSmtpName);
            ClsAppDatabase.AddInParameter(cmd, "@pUserName", SqlDbType.VarChar, pUserName);
            ClsAppDatabase.AddInParameter(cmd, "@pPwd", SqlDbType.VarChar, pPwd);
            ClsAppDatabase.AddInParameter(cmd, "@pFromEmailID", SqlDbType.VarChar, pFromEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pCCEmailID", SqlDbType.VarChar, pCCEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pBCCEmailID", SqlDbType.VarChar, pBCCEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pDisplayName", SqlDbType.VarChar, pDisplayName);
            ClsAppDatabase.AddInParameter(cmd, "@pInPort", SqlDbType.Int, pInPort);
            ClsAppDatabase.AddInParameter(cmd, "@pOutPort", SqlDbType.Int, pOutPort);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pEmailConfigID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<CountryMaster> EmailConfiguration_ListAll(
        Nullable<int> pEmailConfigID, string pSmtpName, bool pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("EmailConfiguration_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pEmailConfigID", SqlDbType.Int, pEmailConfigID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pSmtpName", SqlDbType.VarChar, pSmtpName);            
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
        public int EmailConfiguration_Delete(Nullable<int> pEmailConfigID)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("EmailConfiguration_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pEmailConfigID", SqlDbType.Int, pEmailConfigID);

            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;

        }
        public int EmailConfiguration_Update(int pEmailConfigID, string pSmtpName,
        string pUserName, string pPwd, string pFromEmailID, string pCCEmailID, string pBCCEmailID, string pDisplayName, int pInPort, int pOutPort, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("EmailConfiguration_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pEmailConfigID", SqlDbType.Int, pEmailConfigID);
            ClsAppDatabase.AddInParameter(cmd, "@pSmtpName", SqlDbType.VarChar, pSmtpName);
            ClsAppDatabase.AddInParameter(cmd, "@pUserName", SqlDbType.VarChar, pUserName);
            ClsAppDatabase.AddInParameter(cmd, "@pPwd", SqlDbType.VarChar, pPwd);
            ClsAppDatabase.AddInParameter(cmd, "@pFromEmailID", SqlDbType.VarChar, pFromEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pCCEmailID", SqlDbType.VarChar, pCCEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pBCCEmailID", SqlDbType.VarChar, pBCCEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pDisplayName", SqlDbType.VarChar, pDisplayName);
            ClsAppDatabase.AddInParameter(cmd, "@pInPort", SqlDbType.Int, pInPort);
            ClsAppDatabase.AddInParameter(cmd, "@pOutPort", SqlDbType.Int, pOutPort);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;

            blnResult = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pEmailConfigID"].Value);
            cmd.Dispose();
            return blnResult;

        }
        public int EmailSMSLogDetail_Add(ObjectParameter pLogID, Nullable<int> pHtmlTemplateID, Nullable<int> pCustomerID, int UserID,
        string pSendFrom, string pSendTo, string pHtmlText)
        {
            if (UserID == 0)
            {
                UserID = FN.LoggedUserID;
            }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("EmailSMSLogDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pLogID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, pHtmlTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, pCustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pSendFrom", SqlDbType.VarChar, pSendFrom);
            ClsAppDatabase.AddInParameter(cmd, "@pSendTo", SqlDbType.VarChar, pSendTo);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlText", SqlDbType.VarChar, pHtmlText);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pLogID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int EmailSMSLogDetail_Update(Nullable<int> pLogID, Boolean pIsSuccess, string pResponseText)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("EmailSMSLogDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pLogID", SqlDbType.Int, pLogID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSuccess", SqlDbType.Bit, pIsSuccess);
            ClsAppDatabase.AddInParameter(cmd, "@pResponseText", SqlDbType.VarChar, pResponseText);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}