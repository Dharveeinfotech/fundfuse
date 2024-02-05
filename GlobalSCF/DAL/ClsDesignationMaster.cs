
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
    public class ClsDesignationMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public List<DesignationMaster_ListAll_Result> DesignationMaster_ListAll(Nullable<int> pDesignationID, string pDesignationName,
            Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("DesignationMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, pDesignationID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDesignationName", SqlDbType.VarChar, pDesignationName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
                     
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<DesignationMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int DesignationMaster_Add(Nullable<int> DesignationID, string pDesignationName, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("DesignationMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pDesignationID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationName", SqlDbType.VarChar, pDesignationName);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pDesignationID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int DesignationMaster_Update(int pDesignationID, string @pDesignationName, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("DesignationMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, pDesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationName", SqlDbType.VarChar, @pDesignationName);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int DesignationMaster_Delete(Nullable<int> pDesignationID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("DesignationMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, pDesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}