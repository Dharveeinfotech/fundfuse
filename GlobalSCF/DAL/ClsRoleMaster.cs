using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using TMP.DAL;
using static TMP.Models.mRoleMaster;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace TMP.DAL
{
    public class ClsRoleMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        Function FN = new Function();
        SqlCommand cmd { get; set; }
        public int RoleMaster_Add(ObjectParameter pRoleID, string pRoleName, string pRoleDesc, Nullable<int> pCreateBy, string pCreateIP, string CustomerTypeIDs = "")
        {
            int blnResult = 0;
            cmd = ClsAppDatabase.GetSPName("RoleMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pRoleID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleName", SqlDbType.VarChar, pRoleName == null ? "" : pRoleName);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleDesc", SqlDbType.VarChar, pRoleDesc == null ? "" : pRoleDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, CustomerTypeIDs == null ? "" : CustomerTypeIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pRoleID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int RoleMaster_AddNew(mRoleMaster _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("RoleMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pRoleID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleName", SqlDbType.VarChar, _objModel.RoleName == null ? "" : _objModel.RoleName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRoleDesc", SqlDbType.VarChar, _objModel.RoleDesc == null ? "" : _objModel.RoleDesc.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, _objModel.CustomerTypeIDs == null ? "" : _objModel.CustomerTypeIDs.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsMaker", SqlDbType.Bit, _objModel.IsMaker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsChecker", SqlDbType.Bit, _objModel.IsChecker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsApprover", SqlDbType.Bit, _objModel.IsApprover);
            ClsAppDatabase.AddInParameter(cmd, "@pIsViewer", SqlDbType.Bit, _objModel.IsViewer);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pRoleID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int RoleMaster_Update(Nullable<int> pRoleID, string pRoleName, string pRoleDesc, Nullable<int> pUpdateBy, string pUpdateIP, string CustomerTypeIDs = "")
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("RoleMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleName", SqlDbType.VarChar, pRoleName);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleDesc", SqlDbType.VarChar, pRoleDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, CustomerTypeIDs == null ? "" : CustomerTypeIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int RoleMaster_UpdateNew(mRoleMaster _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("RoleMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, _objModel.RoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleName", SqlDbType.VarChar, _objModel.RoleName == null ? "" : _objModel.RoleName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRoleDesc", SqlDbType.VarChar, _objModel.RoleDesc == null ? "" : _objModel.RoleDesc.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, _objModel.CustomerTypeIDs == null ? "" : _objModel.CustomerTypeIDs.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsMaker", SqlDbType.Bit, _objModel.IsMaker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsChecker", SqlDbType.Bit, _objModel.IsChecker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsApprover", SqlDbType.Bit, _objModel.IsApprover);
            ClsAppDatabase.AddInParameter(cmd, "@pIsViewer", SqlDbType.Bit, _objModel.IsViewer);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int RoleMasterProcessHistory_Add(ObjectParameter pRoleProcessHistoryID, Nullable<int> pRoleID, string pRoleName, string pRoleDesc,
       Nullable<bool> pIsActive, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP, string CustomerTypeIDs = "")
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("RoleMasterProcessHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pRoleProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleName", SqlDbType.VarChar, pRoleName);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleDesc", SqlDbType.VarChar, pRoleDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, CustomerTypeIDs == null ? "" : CustomerTypeIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, pIsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pRoleProcessHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int RoleMasterProcessHistory_AddNew(mRoleMaster _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("RoleMasterProcessHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pRoleProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, _objModel.RoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleName", SqlDbType.VarChar, _objModel.RoleName == null ? "" : _objModel.RoleName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRoleDesc", SqlDbType.VarChar, _objModel.RoleDesc == null ? "" : _objModel.RoleDesc.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, _objModel.CustomerTypeIDs == null ? "" : _objModel.CustomerTypeIDs.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsMaker", SqlDbType.Bit, _objModel.IsMaker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsChecker", SqlDbType.Bit, _objModel.IsChecker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsApprover", SqlDbType.Bit, _objModel.IsApprover);
            ClsAppDatabase.AddInParameter(cmd, "@pIsViewer", SqlDbType.Bit, _objModel.IsViewer);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, _objModel.IsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pRoleProcessHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int MenuRights_Add(ObjectParameter pMenuRightsID, Nullable<int> pMenuID, Nullable<int> pRoleID, Nullable<bool> pIsMaker, Nullable<bool> pIsChecker, Nullable<bool> pIsApprover, Nullable<bool> pIsView, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult;
            SqlCommand cmd = ClsAppDatabase.GetSPName("MenuRights_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pMenuRightsID", SqlDbType.Int, 4);
            ClsAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMaker", SqlDbType.Bit, pIsMaker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsChecker", SqlDbType.Bit, pIsChecker);
            ClsAppDatabase.AddInParameter(cmd, "@pIsApprover", SqlDbType.Bit, pIsApprover);
            ClsAppDatabase.AddInParameter(cmd, "@pIsView", SqlDbType.Bit, pIsView);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pMenuRightsID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<RoleMaster_ListAll_Result> RoleMaster_ListAll(Nullable<int> pRoleID, string pRoleName, Nullable<short> pIsActive, string pStatus, 
        Nullable<bool> pIsKeywordSearch, string pKeywordvalue, int CustomerTypeID = 0)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("RoleMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleName", SqlDbType.VarChar, pRoleName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, CustomerTypeID); 
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<RoleMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<RoleMasterProcessHistory_ListAllBind_Result> RoleMaster_ListAllNew(Nullable<int> pRoleID, string pRoleName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("RoleMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleName", SqlDbType.VarChar, pRoleName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<RoleMasterProcessHistory_ListAllBind_Result>(dataReader as DbDataReader).ToList();
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
       

       

        public List<RoleMasterProcessHistory_ListAllBind_Result> RoleMasterProcessHistory_ListAllBind(Nullable<int> pRoleProcessHistoryID, Nullable<int> pRoleID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("RoleMasterProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleProcessHistoryID", SqlDbType.Int, pRoleProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<RoleMasterProcessHistory_ListAllBind_Result>(dataReader as DbDataReader).ToList();
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
        public int RoleMasterProcessHistory_ProcessStatus(Nullable<int> pRoleProcessHistoryID,
        Nullable<int> pRoleID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("RoleMasterProcessHistory_ProcessStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pRoleProcessHistoryID", SqlDbType.Int, pRoleProcessHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null?"": pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public List<mRoleMaster> RoleMaster_ListAllRemarks(Nullable<int> pRoleID)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("RoleMaster_ListAllRemarks");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<mRoleMaster>(dataReader as DbDataReader).ToList();
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
        public int RoleMaster_UpdateAllStatus(mRoleMaster _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("RoleMaster_UpdateAllStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, _objModel.RoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

    }
}