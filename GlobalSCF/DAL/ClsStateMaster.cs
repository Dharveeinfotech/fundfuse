using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using System.Data.Entity.Core.Objects;

namespace TMP.DAL
{
    public class ClsStateMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public int StateMaster_Add(Nullable<int> pStateID, string pStateCode, string pStateName, Nullable<int> pCountryID, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("StateMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pStateID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pStateCode", SqlDbType.Char, pStateCode);
            ClsAppDatabase.AddInParameter(cmd, "@pStateName", SqlDbType.VarChar, pStateName);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int,  pCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pStateID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int StateMaster_Update(Nullable<int> pStateID, string pStateCode, string pStateName, Nullable<int> pCountryID, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("StateMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsAppDatabase.AddInParameter(cmd, "@pStateCode", SqlDbType.VarChar, pStateCode);
            ClsAppDatabase.AddInParameter(cmd, "@pStateName", SqlDbType.VarChar, pStateName);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int StateMaster_Delete(Nullable<int> pStateID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("StateMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int,  pStateID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}