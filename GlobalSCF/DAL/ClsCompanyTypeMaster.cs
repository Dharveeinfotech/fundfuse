using TMP.Models;
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
    public class ClsCompanyTypeMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public List<CountryMaster> CompanyTypeMaster_ListAll(Nullable<int> pCompanyTypeID, string pCompanyTypeName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CompanyTypeMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCompanyTypeID", SqlDbType.Int, pCompanyTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCompanyTypeName", SqlDbType.VarChar, pCompanyTypeName);
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
        public int Companytypemaster_add(Nullable<int> @pCompanyTypeID, string pCompanyName, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CompanyTypeMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCompanyTypeID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyTypeName", SqlDbType.VarChar, pCompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCompanyTypeID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int Companytypemaster_Update(int pCompanyTypeID, string pCompanyTypeName, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CompanyTypeMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyTypeID", SqlDbType.Int, pCompanyTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyTypeName", SqlDbType.VarChar, pCompanyTypeName);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int CompanyTypeMaster_Delete(Nullable<int> pCompanyTypeID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CompanyTypeMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyTypeID", SqlDbType.Int, pCompanyTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}
