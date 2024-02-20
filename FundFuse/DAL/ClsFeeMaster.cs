using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace TMP.DAL
{
    public class ClsFeeMaster
    {
        #region Local Variable
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        #endregion
        public int FeeMaster_Add(ObjectParameter pFeeId, string pFeeName, Nullable<bool> pIsFix, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("FeeMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pFeeId", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeName", SqlDbType.VarChar, pFeeName);
            ClsAppDatabase.AddInParameter(cmd, "@pIsFix", SqlDbType.Bit, pIsFix);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);

            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pFeeId"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<FeeMaster_ListAll_Result> FeeMaster_ListAll(Nullable<int> pFeeID, string pFeeName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FeeMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeName", SqlDbType.VarChar, pFeeName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FeeMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int FeeMaster_Update(Nullable<int> pFeeID, string pFeeName, Nullable<bool> pIsFix, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("FeeMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeName", SqlDbType.VarChar, pFeeName);
            ClsAppDatabase.AddInParameter(cmd, "@pIsFix", SqlDbType.Bit, pIsFix);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);

            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int FeeMasterProcessHistory_Add(ObjectParameter pFeeProcessHistoryID, Nullable<int> pFeeID,
        string pFeeName, Nullable<bool> pIsFix, Nullable<bool> pIsActive, string pStatus, string pProcessRemark,
        Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("FeeMasterProcessHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pFeeProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeName", SqlDbType.VarChar, pFeeName);
            ClsAppDatabase.AddInParameter(cmd, "@pIsFix", SqlDbType.Bit, pIsFix);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark == null ? "" : pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);

            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pFeeProcessHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<FeeMasterProcessHistory_ListAllBind_Result> FeeMasterProcessHistory_ListAllBind(Nullable<int> pFeeProcessHistoryID, Nullable<int> pFeeID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FeeMasterProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeProcessHistoryID", SqlDbType.Int, pFeeProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FeeMasterProcessHistory_ListAllBind_Result>(dataReader as DbDataReader).ToList();
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
        public List<FeeMaster_ListAll_Result> FeeMasterProcessHistory_ListAllBind2(Nullable<int> pFeeProcessHistoryID, Nullable<int> pFeeID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FeeMasterProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeProcessHistoryID", SqlDbType.Int, pFeeProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FeeMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<FeeMaster_ListAll_Model> FeeMasterProcessHistory_ListAllBind1(Nullable<int> pFeeProcessHistoryID, Nullable<int> pFeeID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FeeMasterProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeProcessHistoryID", SqlDbType.Int, pFeeProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FeeMaster_ListAll_Model>(dataReader as DbDataReader).ToList();
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
        public int FeeMasterProcessHistory_ProcessStatus(Nullable<int> pFeeProcessHistoryID,
        Nullable<int> pFeeID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy,
        string pProcessIP)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("FeeMasterProcessHistory_ProcessStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pFeeProcessHistoryID", SqlDbType.Int, pFeeProcessHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int FeeDetail_Add(ObjectParameter pFeeDetId, Nullable<int> pFeeId, Nullable<int> pCustomerTypeId, Nullable<decimal> pFeePer, Nullable<decimal> pFeeAmt, string pStatus,
        Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("FeeDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pFeeDetId", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeId", SqlDbType.Int, pFeeId);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeId", SqlDbType.Int, pCustomerTypeId);
            ClsAppDatabase.AddInParameter(cmd, "@pFeePer", SqlDbType.Decimal, pFeePer == null ? 0 : pFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeAmt", SqlDbType.Money, pFeeAmt == null ? 0 : pFeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);

            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pFeeDetId"].Value);
            cmd.Dispose();

            return blnResult;
        }
        public List<FeeDetail_ListAll_Result> FeeDetail_ListAll(Nullable<int> pFeeDetId, Nullable<int> pFeeID, Nullable<int> pCustomerTypeId, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FeeDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeDetId", SqlDbType.Int, pFeeDetId);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeId", SqlDbType.Int, pCustomerTypeId);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FeeDetail_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int FeeDetail_Update(Nullable<int> pFeeDetId, Nullable<int> pFeeID, Nullable<int> pCustomerTypeId,
        Nullable<decimal> pFeePer, Nullable<decimal> pFeeAmt, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("FeeDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pFeeDetId", SqlDbType.Int, pFeeDetId);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeId", SqlDbType.Int, pCustomerTypeId);
            ClsAppDatabase.AddInParameter(cmd, "@pFeePer", SqlDbType.Decimal, pFeePer == null ? 0 : pFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeAmt", SqlDbType.Money, pFeeAmt == null ? 0 : pFeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);

            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int FeeDetailProcessHistory_Add(ObjectParameter pFeeDetProcessHistoryID, Nullable<int> pFeeDetID, Nullable<int> pFeeID, Nullable<int> pCustomerTypeId,
        Nullable<decimal> pFeePer, Nullable<decimal> pFeeAmt, Nullable<bool> pIsActive, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("FeeDetailProcessHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pFeeDetProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeDetID", SqlDbType.Int, pFeeDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeId", SqlDbType.Int, pCustomerTypeId);
            ClsAppDatabase.AddInParameter(cmd, "@pFeePer", SqlDbType.Decimal, pFeePer == null ? 0 : pFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeAmt", SqlDbType.Money, pFeeAmt == null ? 0 : pFeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);

            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pFeeDetProcessHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<FeeDetail_ListAll_Result> FeeDetailProcessHistory_ListAllBind1(Nullable<int> pFeeDetProcessHistoryID, Nullable<int> pFeeId, Nullable<int> pFeeDetID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FeeDetailProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeDetProcessHistoryID", SqlDbType.Int, pFeeDetProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeId);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeDetID", SqlDbType.Int, pFeeDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FeeDetail_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<FeeDetailProcessHistory_ListAllBind_Result> FeeDetailProcessHistory_ListAllBind(Nullable<int> pFeeDetProcessHistoryID, Nullable<int> pFeeId, Nullable<int> pFeeDetID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FeeDetailProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeDetProcessHistoryID", SqlDbType.Int, pFeeDetProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeID", SqlDbType.Int, pFeeId);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeDetID", SqlDbType.Int, pFeeDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FeeDetailProcessHistory_ListAllBind_Result>(dataReader as DbDataReader).ToList();
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
        public List<FeeDetailProcessHistory_ListAllBind_Result> FeeMaster_ListAllRemarks(Nullable<int> pFeeId)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FeeMaster_ListAllRemarks");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFeeId", SqlDbType.Int, pFeeId);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FeeDetailProcessHistory_ListAllBind_Result>(dataReader as DbDataReader).ToList();
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