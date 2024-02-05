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
    public class ClsIndustryMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }  
        public SqlConnection conn { get; set; }
        public List<CountryMaster> IndustryMaster_ListAll(Nullable<int> pIndustryID, Nullable<int> pParentIndustryID, string pIndustryName, Nullable<short> pIsActive, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("IndustryMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, pIndustryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pParentIndustryID", SqlDbType.Int, pParentIndustryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIndustryName", SqlDbType.VarChar, pIndustryName);
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
        public int industry_add(Nullable<int> IndustryID, int pParentIndustryID, string pIndustryName, string pIndustryDesc, string pClassificationNo, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("IndustryMaster_Add ");
            ClsAppDatabase.AddOutParameter(cmd, "@pIndustryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pParentIndustryID", SqlDbType.Int, pParentIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryName", SqlDbType.VarChar, pIndustryName);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryDesc", SqlDbType.VarChar, pIndustryDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pClassificationNo", SqlDbType.VarChar, pClassificationNo);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int,  pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pIndustryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int industry_update(Nullable<int> IndustryID, int pParentIndustryID, string pIndustryName, string pIndustryDesc, string pClassificationNo, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("IndustryMaster_Update ");
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pParentIndustryID", SqlDbType.Int, pParentIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryName", SqlDbType.VarChar, pIndustryName);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryDesc", SqlDbType.VarChar, pIndustryDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pClassificationNo", SqlDbType.VarChar, pClassificationNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int IndustryMaster_Delete(Nullable<int> pIndustryID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("IndustryMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, pIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}