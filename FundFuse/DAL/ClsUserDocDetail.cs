using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Entity.Core.Objects;

namespace TMP.DAL
{
    public class ClsUserDocDetail
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public int UserDocDetail_Add(ObjectParameter pUserDocDetID, Nullable<int> pUserID, Nullable<int> pDocumentID, string pDocName, string pStatus, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserDocDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserDocDetID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocName", SqlDbType.VarChar, pDocName);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.Char, pStatus == null ? "" : pStatus);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserDocDetID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int UserDocDetail_Update(Nullable<int> pUserDocDetID, Nullable<int> pUserID, Nullable<int> pDocumentID, string pDocName, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserDocDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pUserDocDetID", SqlDbType.Int, pUserDocDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, pUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, pDocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocName", SqlDbType.VarChar, pDocName);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Dispose();
            blnResult = Row;
            return blnResult;
        }
        //public IEnumerable<UserDocDetail_ListAll_Result> UserDocDetailProcessHistory_ListAllBind(Nullable<int> pUserDocDetProcessHistoryID, Nullable<int> pUserDocDetID, Nullable<int> pUserID, Nullable<int> pDocumentID, string pStatus, Nullable<int> pProcessBy)
        //{
        //    return db.Database.SqlQuery<UserDocDetail_ListAll_Result>(" exec UserDocDetailProcessHistory_ListAllBind @pUserDocDetProcessHistoryID={0}," +
        //    "@pUserDocDetID={1}," +
        //    "@pUserID={2}," +
        //    "@pDocumentID={3}," +
        //    "@pStatus={4}," +
        //    "@pProcessBy={5}",
        //                    pUserDocDetProcessHistoryID, pUserDocDetID, pUserID, pDocumentID, pStatus, pProcessBy).ToList();
        //}        
    }
}