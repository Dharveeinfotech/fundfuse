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
    public class ClsMenuRights
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public int MenuRights_Update(Nullable<int> pMenuRightsID, Nullable<int> pMenuID, Nullable<int> pRoleID, Nullable<bool> pIsMaker, Nullable<bool> pIsChecker, Nullable<bool> pIsApprover, Nullable<bool> pIsView, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult;
            SqlCommand cmd = ClsAppDatabase.GetSPName("MenuRights_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pMenuRightsID", SqlDbType.Int, pMenuRightsID);
            ClsAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMaker", SqlDbType.Bit, pIsMaker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsChecker", SqlDbType.Bit, pIsChecker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsApprover", SqlDbType.Bit, pIsApprover);
            ClsAppDatabase.AddInParameter(cmd, "@pIsView", SqlDbType.Bit, pIsView);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        public int MenuRightsProcessHistory_Add(ObjectParameter pMenuRightsProcessHistoryID, Nullable<int> pMenuRightsID, Nullable<int> pMenuID, Nullable<int> pRoleID, Nullable<bool> pIsMaker, Nullable<bool> pIsChecker, Nullable<bool> pIsApprover, Nullable<bool> pIsView, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult;
            SqlCommand cmd = ClsAppDatabase.GetSPName("MenuRightsProcessHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pMenuRightsProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pMenuRightsID", SqlDbType.Int, pMenuRightsID);
            ClsAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMaker", SqlDbType.Bit, pIsMaker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsChecker", SqlDbType.Bit, pIsChecker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsApprover", SqlDbType.Bit, pIsApprover);
            ClsAppDatabase.AddInParameter(cmd, "@pIsView", SqlDbType.Bit, pIsView);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt32(cmd.Parameters["@pMenuRightsProcessHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int MenuRightsProcessHistory_ProcessStatus(Nullable<int> pMenuRightsProcessHistoryID, Nullable<int> pMenuRightsID, Nullable<int> pProcessBy, string pProcessIP, string pStatus, string pProcessRemark)
        {
            int blnResult;
            SqlCommand cmd = ClsAppDatabase.GetSPName("MenuRightsProcessHistory_ProcessStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pMenuRightsProcessHistoryID", SqlDbType.Int, pMenuRightsProcessHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pMenuRightsID", SqlDbType.Int, pMenuRightsID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        public List<MenuRights> MenuRights_ListAll(Nullable<int> pMenuRightsID, Nullable<int> pMenuID, Nullable<int> pRoleID,
        string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("MenuRights_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuRightsID", SqlDbType.Int, pMenuRightsID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<MenuRights>(dataReader as DbDataReader).ToList();
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
        public List<MenuRights> MenuRights_ListAllNew(Nullable<int> pMenuRightsID, Nullable<int> pMenuID, Nullable<int> pRoleID,
        string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("MenuRights_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuRightsID", SqlDbType.Int, pMenuRightsID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<MenuRights>(dataReader as DbDataReader).ToList();
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
        public List<MenuRights> MenuRightsProcessHistory_ListAllBind(Nullable<int> pMenuRightsProcessHistoryID, int? proleid,
        Nullable<int> pMenuRightsID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("MenuRightsProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuRightsProcessHistoryID", SqlDbType.Int, pMenuRightsProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuRightsID", SqlDbType.Int, pMenuRightsID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, proleid);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<MenuRights>(dataReader as DbDataReader).ToList();
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