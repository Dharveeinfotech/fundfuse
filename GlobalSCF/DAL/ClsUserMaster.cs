using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsUserMaster
    {
        #region Local Variable
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        Function FN = new Function();
        #endregion
        public int UserMaster_Add(ObjectParameter pUserID, ObjectParameter pLoginName, string pFirstName, string pMiddleName, string pLastName,
        Nullable<int> pCustomerID, Nullable<int> pDesignationID, string pServicePeriod, string pEmailID, string pMobileNo,
        string pStaffID, string pPhoto, Nullable<bool> pIsEmail, Nullable<bool> pIsSMS, Nullable<int> pCreateBy, string pCreateIP, string pServLength,
        Nullable<int> pDocumentID, string pDocNo, string pAttach, string Status, int pCustomerShareHoldID = 0, string OtherDesignation = "")
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserID", SqlDbType.Int);
            ClsAppDatabase.AddOutParameter(cmd, "@pLoginName", SqlDbType.VarChar);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, pFirstName);
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, pMiddleName == null ? "" : pMiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, pLastName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, pCustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, pMobileNo);
         
            ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, pServLength);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, pDesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, pDocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, pAttach == null ? "" : pAttach);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, pCustomerShareHoldID);

            ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, pPhoto);
            ClsAppDatabase.AddOutParameter(cmd, "@pPassword", SqlDbType.VarChar);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, Status == null ? "" : Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            int intStore = Convert.ToInt16(cmd.Parameters["@pUserID"].Value);
            pUserID.Value = intStore;
            string StringStore = Convert.ToString(cmd.Parameters["@pLoginName"].Value);
            pLoginName.Value = StringStore;
            cmd.Parameters.Clear();
            cmd.Dispose();
            blnResult = intStore;
            return blnResult;
        }

        public int UserDeptDetailHistory_Add(ObjectParameter pUserDeptDetHistoryID, Nullable<int> pUserDeptDetID, Nullable<int> pUserID,
          Nullable<int> pDepartmentID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserDeptDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserDeptDetHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserDeptDetID", SqlDbType.Int, pUserDeptDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDepartmentID", SqlDbType.Int, pDepartmentID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark == null ? "" : pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserDeptDetHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }

        public int UserMaster_ChangePassword(Nullable<int> pUserID, string pOldPassword, string pNewPassword, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_ChangePassword");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pOldPassword", SqlDbType.VarChar, pOldPassword);
            ClsAppDatabase.AddInParameter(cmd, "@pNewPassword", SqlDbType.VarChar, pNewPassword);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        public string UserMaster_ForgotPassword(string pEmailID, string pMobileNo, string pUpdateIP, ObjectParameter pPassword)
        {
            string blnResult = "";
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_ForgotPassword");
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, pMobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            ClsAppDatabase.AddOutParameter(cmd, "@pPassword", SqlDbType.VarChar);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToString(cmd.Parameters["@pPassword"].Value);
            pPassword.Value = blnResult;
            cmd.Dispose();
            return blnResult;
        }
        public int UserMaster_UpdateAgree(Nullable<int> pUserID, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_UpdateAgree");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        public List<UserMaster_ListAll_Result> UserMaster_ListAll(Nullable<int> pUserID, Nullable<int> pCustomerID, string pLoginName, string pMobileNo,
        string pEmailID, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, pCustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, pLoginName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, pMobileNo);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, pEmailID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int UserMaster_Update(Nullable<int> pUserID, string pFirstName, string pMiddleName, string pLastName, string pLoginName, Nullable<int> pCustomerID,
        Nullable<int> pDesignationID, string pServicePeriod, string pEmailID, string pMobileNo, string pStaffID, string pPhoto, Nullable<bool> pIsEmail,
        Nullable<bool> pIsSMS, Nullable<int> pUpdateBy, string pUpdateIP, string pServLength,
         Nullable<int> pDocumentID, string pDocNo, string pAttach, string Status, int pCustomerShareHoldID = 0, string OtherDesignation = "")
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, pFirstName);
            if (string.IsNullOrEmpty(pMiddleName)) { pMiddleName = ""; }
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, pMiddleName == null ? "" : pMiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, pLastName);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, pLoginName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, pCustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, pMobileNo);

            ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, pServLength);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, pDesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, pDocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, pAttach == null ? "" : pAttach);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, pCustomerShareHoldID);

            ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, pPhoto);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, Status == null ? "" : Status);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        public DataSet GetDataSet(string pLoginName, string pPassword)
        {
            DataSet dsAc = new DataSet();
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_VerifyUser");
            ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, pLoginName);
            ClsAppDatabase.AddInParameter(cmd, "@pPassword", SqlDbType.VarChar, pPassword);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            cmd.Transaction = tras;
            ad.Fill(dsAc);
            cmd.Dispose();
            return dsAc;
        }
        public DataSet GetAdminDashBoard()
        {
            DataSet dsAc = new DataSet();
            SqlCommand cmd = ClsAppDatabase.GetSPName("AdminDashBoard");
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            ad.Fill(dsAc);
            cmd.Connection.Close();
            cmd.Dispose();
            return dsAc;
        }
        public List<DATDashboard_Model> GetDATDashboard()
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("InvoiceMaster_HomeSerProv");
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<DATDashboard_Model>(dataReader as DbDataReader).ToList();
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
        public DataSet GetTaskDetail(int pRoleId)
        {
            DataSet dsAc = new DataSet();
            SqlCommand cmd = ClsAppDatabase.GetSPName("TaskDetail_User");
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, pRoleId);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            ad.Fill(dsAc);
            cmd.Connection.Close();
            cmd.Dispose();
            return dsAc;
        }
        public DataSet UserMaster_VerifyUser(string pLoginName, string pPassword)
        {
            return GetDataSet(pLoginName, pPassword);
        }
        public int UserMaster_ProfilePictureUpdate(string pUserID, string pPhoto, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_ProfilePictureUpdate");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, pPhoto);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Dispose();
            return blnResult;
        }
        public DataSet BuyerBoardResult(int CustomerID, int UserID)
        {
            DataSet dsAc = new DataSet();
            SqlCommand cmd = ClsAppDatabase.GetSPName("BuyerDashBoard");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, UserID);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            ad.Fill(dsAc);
            cmd.Connection.Close();
            cmd.Dispose();
            return dsAc;
        }
        public DataSet ObligorBoardResult(int CustomerID, int UserID)
        {
            DataSet dsAc = new DataSet();
            SqlCommand cmd = ClsAppDatabase.GetSPName("ObligorDashBoard");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, UserID);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            ad.Fill(dsAc);
            cmd.Connection.Close();
            cmd.Dispose();
            return dsAc;
        }
        public DataSet SupplierBoardResult(int CustomerID, int UserID)
        {
            DataSet dsAc = new DataSet();
            SqlCommand cmd = ClsAppDatabase.GetSPName("SupplierDashBoard");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, UserID);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            ad.Fill(dsAc);
            cmd.Connection.Close();
            cmd.Dispose();
            return dsAc;
        }
        public DataSet MenuRoleRights_ListAll_Result(string pRoleIDs, int pMenuID, int pParentMenuID, string pStatus)
        {
            DataSet dsAc = new DataSet();
            SqlCommand cmd = ClsAppDatabase.GetSPName("MenuRoleRights_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pRoleIDs", SqlDbType.VarChar, pRoleIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsAppDatabase.AddInParameter(cmd, "@pParentMenuID", SqlDbType.Int, pParentMenuID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            ad.Fill(dsAc);
            cmd.Connection.Close();
            cmd.Dispose();
            return dsAc;
        }
        public DataTable UserLoginHistory_PasswordHistory(int pUserID)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserLoginHistory_PasswordHistory");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            if (cmd.Connection.State == ConnectionState.Closed)
            { cmd.Connection.Open(); }
            else
            { cmd.Connection.Close(); cmd.Connection.Open(); }
            ad.Fill(dt);
            cmd.Connection.Close();
            cmd.Dispose();
            return dt;
        }
        public int UserLoginHistory_Add(ObjectParameter pUserLoginHistoryID, Nullable<int> pUserID, string pLoginName, string pPassword, string pOTPNumber, Nullable<bool> pIsSuccess, Nullable<bool> pIsResetPwd, string pMessage, string pLoginIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserLoginHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserLoginHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, pLoginName);
            ClsAppDatabase.AddInParameter(cmd, "@pPassword", SqlDbType.VarChar, pPassword);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSuccess", SqlDbType.Bit, pIsSuccess);
            ClsAppDatabase.AddInParameter(cmd, "@pIsResetPwd", SqlDbType.Bit, pIsResetPwd);
            ClsAppDatabase.AddInParameter(cmd, "@pOTP", SqlDbType.VarChar, pOTPNumber);
            ClsAppDatabase.AddInParameter(cmd, "@pMessage", SqlDbType.VarChar, pMessage);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginIP", SqlDbType.VarChar, pLoginIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserLoginHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<UserLoginHistory_ListAll_Result> UserLoginHistory_OTPNumber(Nullable<int> pUserID)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserLoginHistory_OTPNumber");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserLoginHistory_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int UserMasterHistory_Add(ObjectParameter pUserHistoryID, Nullable<int> pUserID, string pFirstName, string pMiddleName,
        string pLastName, string pLoginName, string pPassword, Nullable<int> pCustomerID, Nullable<int> pDesignationID, string pServicePeriod,
        string pEmailID, string pMobileNo, string pStaffID, string pPhoto, Nullable<bool> pIsEmail, Nullable<bool> pIsSMS, Nullable<bool> pIsActive,
        DateTime pPwdExpiryDate, Nullable<bool> pIsAgree, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP, string pServLength,
         Nullable<int> pDocumentID, string pDocNo, string pAttach, int pCustomerShareHoldID = 0, string OtherDesignation = "")
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMasterHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, pFirstName);
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, pMiddleName == null ? "" : pMiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, pLastName);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, pLoginName);
            ClsAppDatabase.AddInParameter(cmd, "@pPassword", SqlDbType.VarChar, pPassword);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, pCustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, pEmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, pMobileNo);

            ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, pServLength == null ? "" : pServLength);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, pDesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, pDocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, pAttach == null ? "" : pAttach);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, pCustomerShareHoldID);

            ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, pPhoto);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, pIsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pPwdExpiryDate", SqlDbType.DateTime, pPwdExpiryDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAgree", SqlDbType.Bit, pIsAgree);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark == null ? "" : pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<UserMaster_ListAll_Result> UserMasterHistory_ListAllBind(Nullable<int> pUserHistoryID, Nullable<int> pUserID, string pStatus, Nullable<int> pProcessBy, int CustomerID = 0)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMasterHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserHistoryID", SqlDbType.Int, pUserHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        //public int UserMasterHistory_ProcessStatus(Nullable<int> pUserHistoryID, Nullable<int> pUserID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        //{
        //    int blnResult = 0;
        //    SqlCommand cmd = ClsAppDatabase.GetSPName("UserMasterHistory_ProcessStatus");
        //    ClsAppDatabase.AddInParameter(cmd, "@pUserHistoryID", SqlDbType.Int, pUserHistoryID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);
        //    ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
        //    ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
        //    ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
        //    cmd.Transaction = tras;
        //    int Row = cmd.ExecuteNonQuery();
        //    cmd.Dispose();
        //    blnResult = Row;
        //    return blnResult;
        //}
        public List<UserMaster_ListAll_Result> UserMaster_ListAllRemarks(Nullable<int> pUserID)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMaster_ListAllRemarks");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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

        public int UserMaster_UpdateAllStatus(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_UpdateAllStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
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
        public int UserMaster_Delete(int UserID, bool IsNoRequired)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        public List<UserMaster_ListAll_Result> UserMaster_ListAllNew(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, _objModel.LoginName == null ? "" : _objModel.LoginName.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, _objModel.IsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status.Trim());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<UserMaster_ListAll_Result> UserMasterHistory_ListAllBindNew(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMasterHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserHistoryID", SqlDbType.Int, _objModel.UserHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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


        #region User Registration
        public int UserMaster_Add(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserID", SqlDbType.Int);
            ClsAppDatabase.AddOutParameter(cmd, "@pLoginName", SqlDbType.VarChar);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, _objModel.ServLength == null ? "" : _objModel.ServLength);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, _objModel.Photo == null ? "" : _objModel.Photo);
            ClsAppDatabase.AddInParameter(cmd, "@pPassword", SqlDbType.VarChar, _objModel.Password == null ? "" : _objModel.Password);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserID"].Value);
            string strLoginName = Convert.ToString(cmd.Parameters["@pLoginName"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int UserMasterHistory_Add(UserMaster_ListAll_Result _objModel)
        {
            if (_objModel.PwdExpiryDate == null) { _objModel.PwdExpiryDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMasterHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserHistoryID", SqlDbType.Int, _objModel.UserHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, _objModel.LoginName == null ? "" : _objModel.LoginName);
            ClsAppDatabase.AddInParameter(cmd, "@pPassword", SqlDbType.VarChar, _objModel.Password == null ? "" : _objModel.Password);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, _objModel.ServLength == null ? "" : _objModel.ServLength);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, _objModel.Photo == null ? "" : _objModel.Photo);
            ClsAppDatabase.AddInParameter(cmd, "@pPwdExpiryDate", SqlDbType.DateTime, _objModel.PwdExpiryDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAgree", SqlDbType.Bit, _objModel.IsAgree);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, _objModel.IsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());            
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int UserMaster_Update(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, _objModel.LoginName == null ? "" : _objModel.LoginName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, _objModel.ServLength == null ? "" : _objModel.ServLength);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, _objModel.Photo == null ? "" : _objModel.Photo);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserMaster_Delete(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserRoleLinkMaster_Add(UserRoleLinkMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserRoleLinkMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, _objModel.UserRoleLinkID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, _objModel.RoleID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserRoleLinkID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int UserRoleLinkMaster_Delete(UserRoleLinkMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserRoleLinkMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, _objModel.UserRoleLinkID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<UserMaster_ListAll_Result> UserMaster_ListAll(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, _objModel.LoginName == null ? "" : _objModel.LoginName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, _objModel.IsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<UserMaster_ListAll_Result> UserMasterHistory_ListAllBind(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMasterHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserHistoryID", SqlDbType.Int, _objModel.UserHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<UserMaster_ListAll_Result> UserMasterHistory_ListAll(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMasterHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserHistoryID", SqlDbType.Int, _objModel.UserHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<UserRoleLinkMaster_ListAll_Result> UserRoleLinkMaster_ListAll(UserRoleLinkMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserRoleLinkMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserRoleLinkID", SqlDbType.Int, _objModel.UserRoleLinkID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleID", SqlDbType.Int, _objModel.RoleID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
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
        public int UserSessionHistory_Add(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserSessionHistory_Add");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrentSessionID", SqlDbType.VarChar, _objModel.CurrentSessionID);
            ClsAppDatabase.AddInParameter(cmd, "@pRequest", SqlDbType.VarChar, _objModel.Request);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginDevice", SqlDbType.VarChar, _objModel.LoginDevice);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<UserMaster_ListAll_Result> UserSessionHistory_ListAll(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserSessionHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        #endregion

        #region mUserDeptDetail
        public int UserDeptDetail_Add(mUserDeptDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserDeptDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserDeptDetID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDepartmentID", SqlDbType.Int, _objModel.DepartmentID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserDeptDetID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserDeptDetail_Update(mUserDeptDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserDeptDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pUserDeptDetID", SqlDbType.Int, _objModel.UserDeptDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDepartmentID", SqlDbType.Int, _objModel.DepartmentID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserDeptDetail_Delete(mUserDeptDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserDeptDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pUserDeptDetID", SqlDbType.Int, _objModel.UserDeptDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserDeptDetail_UpdateNotRequired(mUserDeptDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserDeptDetail_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pUserDeptDetID", SqlDbType.Int, _objModel.UserDeptDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserDeptDetailHistory_ProcessStatus(mUserDeptDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserDeptDetailHistory_ProcessStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pUserDeptDetHistoryID", SqlDbType.Int, _objModel.UserDeptDetHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserDeptDetID", SqlDbType.Int, _objModel.UserDeptDetID);
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
        public List<mUserDeptDetail> DepartmentMaster_ListAll(mUserDeptDetail _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("DepartmentMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDepartmentID", SqlDbType.Int, _objModel.DepartmentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, _objModel.IsActive ? 1 : 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<mUserDeptDetail>(dataReader as DbDataReader).ToList();
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
        public List<mUserDeptDetail> UserDeptDetail_ListAll(mUserDeptDetail _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserDeptDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserDeptDetID", SqlDbType.Int, _objModel.UserDeptDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDepartmentID", SqlDbType.Int, _objModel.DepartmentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<mUserDeptDetail>(dataReader as DbDataReader).ToList();
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
        public List<mUserDeptDetail> UserDeptDetailHistory_ListAllBind(mUserDeptDetail _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserDeptDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserDeptDetHistoryID", SqlDbType.Int, _objModel.UserDeptDetHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserDeptDetID", SqlDbType.Int, _objModel.UserDeptDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDepartmentID", SqlDbType.Int, _objModel.DepartmentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<mUserDeptDetail>(dataReader as DbDataReader).ToList();
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
        #endregion

        #region User Identity Detail Methods
        public int UserIdentityDetail_Add(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserIdentityDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserIdenID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserIdenID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserIdentityDetail_Update(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserIdentityDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pUserIdenID", SqlDbType.Int, _objModel.UserIdenID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserIdentityDetail_Delete(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserIdentityDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pUserIdenID", SqlDbType.Int, _objModel.UserIdenID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserIdentityDetailHistory_Add(UserMaster_ListAll_Result _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserIdentityDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserIdenHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserIdenID", SqlDbType.Int, _objModel.UserIdenID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserIdenHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<UserMaster_ListAll_Result> UserIdentityDetail_ListAll(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserIdentityDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserIdenID", SqlDbType.Int, _objModel.UserIdenID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<UserMaster_ListAll_Result> UserIdentityDetailHistory_ListAll(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserIdentityDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserIdenHistoryID", SqlDbType.Int, _objModel.UserIdenHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserIdenID", SqlDbType.Int, _objModel.UserIdenID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<UserMaster_ListAll_Result> UserIdentityDetailHistory_ListAllBind(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserIdentityDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserIdenHistoryID", SqlDbType.Int, _objModel.UserIdenHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserIdenID", SqlDbType.Int, _objModel.UserIdenID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        #endregion
    }
}