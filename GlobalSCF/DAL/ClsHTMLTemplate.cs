
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
    public class ClsHTMLTemplate
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public int HTMLTemplate_Add(ObjectParameter pHtmlTemplateID, string pHtmlName, string pHtmlText, string pSubject, Nullable<int> pTemplateID,
        Nullable<bool> pIsSMS, Nullable<bool> pIsMemo, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HTMLTemplate_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlName", SqlDbType.VarChar, pHtmlName);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlText", SqlDbType.NVarChar, pHtmlText);
            ClsAppDatabase.AddInParameter(cmd, "@pSubject", SqlDbType.VarChar, pSubject);
            ClsAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int, pTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSMS", SqlDbType.Bit, pIsSMS);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMemo", SqlDbType.Bit, pIsMemo);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pHtmlTemplateID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public virtual int HTMLTemplate_Update(Nullable<int> pHtmlTemplateID, string pHtmlName, string pHtmlText, string pSubject, Nullable<int> pTemplateID,
        Nullable<bool> pIsSMS, Nullable<bool> pIsMemo, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HTMLTemplate_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, pHtmlTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlName", SqlDbType.VarChar, pHtmlName);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlText", SqlDbType.NVarChar, pHtmlText);
            ClsAppDatabase.AddInParameter(cmd, "@pSubject", SqlDbType.VarChar, pSubject);
            ClsAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int, pTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSMS", SqlDbType.Bit, pIsSMS);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMemo", SqlDbType.Bit, pIsMemo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pHtmlTemplateID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<HTMLTemplate_ListAll_Result> HTMLTemplate_ListAll(Nullable<int> pHtmlTemplateID, string pHtmlName, Nullable<short> pIsActive,
        string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue, Nullable<int> @pTemplateID)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("HTMLTemplate_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, pHtmlTemplateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pHtmlName", SqlDbType.VarChar, pHtmlName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int, pTemplateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<HTMLTemplate_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int HTMLTemplateProcessHistory_ProcessStatus(Nullable<int> pHtmlTemplateProcessHistoryID, Nullable<int>
        pHtmlTemplateID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HTMLTemplateProcessHistory_ProcessStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlTemplateProcessHistoryID", SqlDbType.Int, pHtmlTemplateProcessHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, pHtmlTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pHtmlTemplateID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int HTMLTemplateProcessHistory_Add(ObjectParameter pHtmlTemplateProcessHistoryID,
        Nullable<int> pHtmlTemplateID, string pHtmlName, string pHtmlText, string pSubject, Nullable<int> pTemplateID, Nullable<bool> pIsSMS, Nullable<bool> pIsMemo,
        Nullable<bool> pIsActive, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HTMLTemplateProcessHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pHtmlTemplateProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, pHtmlTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlName", SqlDbType.NVarChar, pHtmlName);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlText", SqlDbType.NVarChar, pHtmlText);
            ClsAppDatabase.AddInParameter(cmd, "@pSubject", SqlDbType.VarChar, pSubject);
            ClsAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int, pTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSMS", SqlDbType.Bit, pIsSMS);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMemo", SqlDbType.Bit, pIsMemo);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, pIsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pHtmlTemplateProcessHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public DataSet GetDataSetUser(Nullable<int> pUserID, Nullable<int> pCustomerID, string pLoginName, string pMobileNo, string pEmailID,
        Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {

            DataSet dsAc = new DataSet();
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, pCustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, pLoginName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, pMobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);            
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            if (cmd.Connection.State == ConnectionState.Closed)
            {
                cmd.Connection.Open();
            }
            else
            {
                cmd.Connection.Close();
                cmd.Connection.Open();
            }
            ad.Fill(dsAc);
            cmd.Connection.Close();
            cmd.Dispose();
            return dsAc;
        }
        public List<HTMLTemplate_ListAll_Result> HtmlDynamicText_ListAll(Nullable<int> pDynamicTextID, string pDynamicTextName, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("HtmlDynamicText_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDynamicTextID", SqlDbType.Int, pDynamicTextID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDynamicTextName", SqlDbType.VarChar, pDynamicTextName);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<HTMLTemplate_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<HTMLTemplate_ListAll_Result> HTMLTemplate_ListAllRemarks(Nullable<int> pHtmlTemplateID)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("HTMLTemplate_ListAllRemarks");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, pHtmlTemplateID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<HTMLTemplate_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<HTMLTemplate_ListAll_Result> HTMLTemplateProcessHistory_ListAllBind_Result(Nullable<int> pHtmlTemplateProcessHistoryID, Nullable<int> pHtmlTemplateID, Nullable<int>
        pTemplateID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("HTMLTemplateProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pHtmlTemplateProcessHistoryID", SqlDbType.Int, pHtmlTemplateProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, pHtmlTemplateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int, pTemplateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<HTMLTemplate_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<HTMLTemplate_ListAll_Result> Template_ListAll_Result(Nullable<int> pTemplateID, string pName)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("Template_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int,pTemplateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pName", SqlDbType.VarChar, pName);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<HTMLTemplate_ListAll_Result>(dataReader as DbDataReader).ToList();
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