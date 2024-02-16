using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMP.Models;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsTemplate
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public List<CountryMaster> Template_ListAll(Nullable<int> pTemplateID, string pName)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("Template_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int, pTemplateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pName", SqlDbType.VarChar, pName);
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
        public int Template_Add(Nullable<int> pTemplateID, string pName, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("Template_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pTemplateID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pName", SqlDbType.VarChar, pName);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pTemplateID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int Template_Update(int pTemplateID, string pName, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("Template_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int, pTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pName", SqlDbType.VarChar, pName);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int Template_Delete(Nullable<int> pTemplateID)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("Template_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pTemplateID", SqlDbType.Int, pTemplateID);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public List<CountryMaster> HtmlDynamicText_ListAll(Nullable<int> pDynamicTextID, string pDynamicTextName, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("HtmlDynamicText_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDynamicTextID", SqlDbType.Int, pDynamicTextID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDynamicTextName", SqlDbType.VarChar, pDynamicTextName);
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
        public int HtmlDynamicTemplateField_Add(Nullable<int> pDynamicTextID, string pDynamicTextName, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HtmlDynamicTemplateField_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pDynamicTextID ", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pDynamicTextName", SqlDbType.VarChar, pDynamicTextName);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int HtmlDynamicTemplateField_Update(int pDynamicTextID, string pDynamicTextName, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HtmlDynamicTemplateField_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pDynamicTextID", SqlDbType.Int, pDynamicTextID);
            ClsAppDatabase.AddInParameter(cmd, "@pDynamicTextName", SqlDbType.VarChar, pDynamicTextName);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int mHtmlDynamicTemplateField_Delete(Nullable<int> pDynamicTextID)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HtmlDynamicTemplateField_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pDynamicTextID", SqlDbType.Int, pDynamicTextID);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}