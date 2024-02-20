using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using TMP.Models;
using System.Data.Entity.Core.Objects;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsFAQ
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public List<FAQ> FaqMaster_ListAll(Nullable<int> pFaqID, Nullable<int> pCustomerTypeID, Nullable<short> pIsActive, 
        string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("FaqMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pFaqID", SqlDbType.Int, pFaqID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<FAQ>(dataReader as DbDataReader).ToList();
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
        public int FaqMaster_Add(Nullable<int> pFaqID, Nullable<int> pCustomerTypeID, string pQuestions, string pAnswer, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("FaqMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pFaqID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pQuestions", SqlDbType.VarChar, pQuestions);
            ClsAppDatabase.AddInParameter(cmd, "@pAnswer", SqlDbType.VarChar, pAnswer);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pFaqID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int FaqMaster_Update(Nullable<int> pFaqID, Nullable<int> pCustomerTypeID, string pQuestions, string pAnswer, bool pIsActive, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("FaqMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pFaqID", SqlDbType.Int, pFaqID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pQuestions", SqlDbType.VarChar, pQuestions);
            ClsAppDatabase.AddInParameter(cmd, "@pAnswer", SqlDbType.VarChar, pAnswer);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, pIsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int FaqMaster_Delete(Nullable<int> pFaqID)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("FaqMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pFaqID", SqlDbType.Int, pFaqID);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}