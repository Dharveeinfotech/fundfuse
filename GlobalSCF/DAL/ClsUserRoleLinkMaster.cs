using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsUserRoleLinkMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public int UserRoleLinkMaster_Add(ObjectParameter pUserRoleLinkID, Nullable<int> pUserID, Nullable<int> pRoleID, string pStatus, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserRoleLinkMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserRoleLinkID"].Value);
            cmd.Dispose();
            return blnResult;
        }

        public int UserRoleLinkMasterHistory_Add(ObjectParameter pUserRoleLinkHistoryID, Nullable<int> pUserRoleLinkID, Nullable<int> pUserID, 
            Nullable<int> pRoleID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserRoleLinkMasterHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserRoleLinkHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, pUserRoleLinkID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark == null ? "" : pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserRoleLinkHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }

        
        public List<UserRoleLinkMaster_ListAll_Result> UserRoleLinkMaster_ListAll(Nullable<int> pUserRoleLinkID, Nullable<int> pUserID, Nullable<int> 
            pRoleID, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserRoleLinkMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, pUserRoleLinkID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserRoleLinkMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int UserRoleLinkMaster_Update(Nullable<int> pUserRoleLinkID, Nullable<int> pUserID, Nullable<int> pRoleID, string Status, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserRoleLinkMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, pUserRoleLinkID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, Status == null ? "" : Status);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        public int UserRoleLinkMaster_UpdateNotRequired(Nullable<int> pUserRoleLinkID, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserRoleLinkMaster_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, pUserRoleLinkID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        public int UserRoleLinkMaster_Delete(Nullable<int> pUserRoleLinkID, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserRoleLinkMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, pUserRoleLinkID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        public List<UserRoleLinkMaster_ListAll_Result> UserRoleLinkMasterHistory_ListAllBind(Nullable<int> pUserRoleLinkHistoryID, Nullable<int> pUserRoleLinkID, int pUserID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserRoleLinkMasterHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserRoleLinkHistoryID", SqlDbType.Int, pUserRoleLinkHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, pUserRoleLinkID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserRoleLinkMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int UserRoleLinkMasterHistory_ProcessStatus(Nullable<int> pUserRoleLinkHistoryID, Nullable<int> pUserRoleLinkID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserRoleLinkMasterHistory_ProcessStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pUserRoleLinkHistoryID", SqlDbType.Int, pUserRoleLinkHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, pUserRoleLinkID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark == null ? "" : pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
    }
}