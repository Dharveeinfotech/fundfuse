
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
    public class ClsDocumentMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public int DocumentMaster_Add(ObjectParameter pDocumentID, string pDocumentName,
             string pDocumentDesc, Nullable<bool> pIsUserDoc, Nullable<int> pCreateBy, string pCreateIP, Nullable<bool> pIsInvestor)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("DocumentMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pDocumentID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentName", SqlDbType.VarChar, pDocumentName);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentDesc", SqlDbType.VarChar, pDocumentDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pIsUserDoc", SqlDbType.Bit, pIsUserDoc);
            ClsAppDatabase.AddInParameter(cmd, "@pIsInvestor", SqlDbType.Bit, pIsInvestor); 
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pDocumentID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<DocumentMaster_ListAll_Result> DocumentMaster_ListAll(Nullable<int> pDocumentID, string pDocumentName, Nullable<short> pIsUserDoc, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("DocumentMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentName", SqlDbType.VarChar, pDocumentName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsUserDoc", SqlDbType.SmallInt, pIsUserDoc);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<DocumentMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int DocumentMaster_Update(Nullable<int> pDocumentID, string pDocumentName,
        string pDocumentDesc, Nullable<bool> pIsUserDoc, Nullable<int> pUpdateBy, string pUpdateIP, Nullable<bool> pIsInvestor)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("DocumentMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentName", SqlDbType.VarChar, pDocumentName);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentDesc", SqlDbType.VarChar, pDocumentDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pIsUserDoc", SqlDbType.Bit, pIsUserDoc);
            ClsAppDatabase.AddInParameter(cmd, "@pIsInvestor", SqlDbType.Bit, pIsInvestor);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;

        }

        public int DocumentMasterProcessHistory_Add(ObjectParameter pDocumentProcessHistoryID,
             Nullable<int> pDocumentID, string pDocumentName, string pDocumentDesc, Nullable<bool> pIsUserDoc,
             Nullable<bool> pIsActive, string pStatus, string pProcessRemark, Nullable<int> pProcessBy,
             string pProcessIP, Nullable<bool> pIsInvestor)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("DocumentMasterProcessHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pDocumentProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentName", SqlDbType.VarChar, pDocumentName);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentDesc", SqlDbType.VarChar, pDocumentDesc);
            ClsAppDatabase.AddInParameter(cmd, "@pIsUserDoc", SqlDbType.Bit, pIsUserDoc);
            ClsAppDatabase.AddInParameter(cmd, "@pIsInvestor", SqlDbType.Bit, pIsInvestor);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, pIsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pDocumentProcessHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }

        public List<DocumentMaster_ListAll_Result> DocumentMasterProcessHistory_ListAllBind(Nullable<int> pDocumentProcessHistoryID, Nullable<int> pDocumentID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("DocumentMasterProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentProcessHistoryID", SqlDbType.Int, pDocumentProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<DocumentMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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

        public List<DocumentMaster_ListAll_Result> DocumentMasterProcessHistory_ListAllBind1(Nullable<int> pDocumentProcessHistoryID, Nullable<int> pDocumentID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("DocumentMasterProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentProcessHistoryID", SqlDbType.Int, pDocumentProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<DocumentMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int DocumentMasterProcessHistory_ProcessStatus(Nullable<int> pDocumentProcessHistoryID, Nullable<int>
           pDocumentID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("DocumentMasterProcessHistory_ProcessStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentProcessHistoryID", SqlDbType.Int, pDocumentProcessHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }

        public int CustomerTypeDocumentDetails_Add(ObjectParameter pCustomerTypeDocDetID, Nullable<int> pCustomerTypeID, Nullable<int> pDocumentID, 
            string pStatus, Nullable<int> pCreateBy, string pCreateIP, string pAttachment)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerTypeDocumentDetails_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pAttachment", SqlDbType.VarChar, pAttachment);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerTypeDocDetID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerTypeDocumentDetails_Update(Nullable<int> pCustomerTypeDocDetID, Nullable<int> pCustomerTypeID, Nullable<int> pDocumentID, 
            Nullable<int> pUpdateBy, string pUpdateIP, string pAttachment)
        {
            int blnResult = 0;

            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerTypeDocumentDetails_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, pCustomerTypeDocDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pAttachment", SqlDbType.VarChar, pAttachment);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }

        public int CustomerTypeDocumentDetailsProcessHistory_ProcessStatus(Nullable<int> pCustomerTypeDocProcessHistoryID,
           Nullable<int> pCustomerTypeDocDetID, string pStatus, string pProcessRemark, Nullable<int> pProcessBy, string pProcessIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerTypeDocumentDetailsProcessHistory_ProcessStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocProcessHistoryID", SqlDbType.Int, pCustomerTypeDocProcessHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, pCustomerTypeDocDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerTypeDocumentDetails_ListAll_Result> CustomerTypeDocumentDetailsProcessHistory_ListAllBind(Nullable<int> pCustomerTypeDocProcessHistoryID, Nullable<int> pCustomerTypeDocDetID, Nullable<int> pDocumentID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerTypeDocumentDetailsProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocProcessHistoryID", SqlDbType.Int, pCustomerTypeDocProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, pCustomerTypeDocDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<CustomerTypeDocumentDetails_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int CustomerTypeDocumentDetailsProcessHistory_Add(ObjectParameter pCustomerTypeDocProcessHistoryID, Nullable<int> pCustomerTypeDocDetID,
            Nullable<int> pCustomerTypeID, Nullable<int> pDocumentID, Nullable<bool> pIsActive, string pStatus, string pProcessRemark,
            Nullable<int> pProcessBy, string pProcessIP, string pAttachment)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerTypeDocumentDetailsProcessHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerTypeDocProcessHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, pCustomerTypeDocDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, pIsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pAttachment", SqlDbType.VarChar, pAttachment);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, pProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pProcessIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerTypeDocProcessHistoryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerTypeDocumentDetails_Delete(Nullable<int> pCustomerTypeDocDetID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerTypeDocumentDetails_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, pCustomerTypeDocDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerTypeDocumentDetails_UpdateNotRequired(Nullable<int> pCustomerTypeDocDetID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerTypeDocumentDetails_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, pCustomerTypeDocDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public List<DocumentMaster_ListAll_Result> DocumentMaster_ListAllRemarks(Nullable<int> pDocumentID)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("DocumentMaster_ListAllRemarks");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<DocumentMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<CustomerTypeDocumentDetails_ListAll_Result> CustomerTypeDocumentDetails_ListAll(Nullable<int> pCustomerTypeDocDetID, 
            Nullable<int> pCustomerTypeID, Nullable<int> pDocumentID, 
            Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, 
            string pKeywordvalue, Nullable<int> pIsUserDoc, Nullable<int> pIsInvestor)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerTypeDocumentDetails_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, pCustomerTypeDocDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsUserDoc", SqlDbType.SmallInt, pIsUserDoc);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsInvestor", SqlDbType.SmallInt, pIsInvestor);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<CustomerTypeDocumentDetails_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<CustomerTypeDocumentDetails_ListAll_Result> CustomerTypeDocumentDetailsProcessHistory_ListAllBind1(Nullable<int> pCustomerTypeDocProcessHistoryID, Nullable<int> pCustomerTypeDocDetID, Nullable<int> pDocumentID, string pStatus, Nullable<int> pProcessBy)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerTypeDocumentDetailsProcessHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocProcessHistoryID", SqlDbType.Int, pCustomerTypeDocProcessHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, pCustomerTypeDocDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, pProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<CustomerTypeDocumentDetails_ListAll_Result>(dataReader as DbDataReader).ToList();
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