using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.DAL
{
    public class ClsAppDatabase
    {
        public static string strConnection = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["constring"].ConnectionString.ToString();
        private static SqlConnection mSqlCon = null;


        internal static bool ConnectionOpen()
        {
            bool blnReturn = false;
            try
            {
                if (mSqlCon == null || mSqlCon.State == System.Data.ConnectionState.Closed)
                {
                    mSqlCon = new SqlConnection(strConnection);
                    //mSqlCon.Open();
                    blnReturn = true;
                }
                else
                {
                    blnReturn = true;
                }
                return blnReturn;
            }
            catch (Exception e)
            {
                String stree = e.Message;
            }
            return blnReturn;
        }

        internal static SqlConnection GetCon()
        {

             ConnectionOpen();
            return mSqlCon;
        }
        //internal static SqlConnection ConClose()
        //{
           
        //}

        internal static SqlCommand GetSPName(string ProceName)
        {
            SqlCommand cmdReturn = new SqlCommand(ProceName);
            cmdReturn.Connection = GetCon();
            cmdReturn.CommandType = CommandType.StoredProcedure;
            return cmdReturn;
        }
    
        internal static SqlParameter AddInParameter(SqlCommand sqlCMD, string strPrName, SqlDbType prDBType, object prValue)
        {
            return AddInParameter(sqlCMD, strPrName, prDBType, -1, prValue);
        }

        internal static SqlParameter AddInParameter(SqlCommand sqlCMD, string strPrName, SqlDbType prDBType, int prSize, object prValue)
        {
            SqlParameter parm = null;
            try
            {
                parm = new SqlParameter(strPrName, prDBType, prSize);
                parm.Direction = ParameterDirection.Input;
                parm.Value = prValue;
                sqlCMD.Parameters.Add(parm);
            }
            catch (Exception e)
            {
                String strEE = e.Message;
                parm = null;
            }
            return parm;
        }

        internal static SqlParameter AddOutParameter(SqlCommand cmd, string strPrName, SqlDbType sqlDbType)
        {
            return AddOutParameter(cmd, strPrName, sqlDbType, -1);
        }

        internal static SqlParameter AddOutParameter(SqlCommand cmd, string strPrName, SqlDbType sqlDbType, int size)
        {
            SqlParameter parm = null;
            try
            {
                parm = new SqlParameter(strPrName, sqlDbType, size);
                parm.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(parm);
            }
            catch (Exception ex)
            {
                parm = null;
                throw ex;
            }
            return parm;
        }

        internal static object AddInParameter(SqlCommand cmd, string p, SqlDbType sqlDbType)
        {
            throw new NotImplementedException();
        }

        internal static DataTableReader GetDataTableReader(SqlCommand cmdSql, DataSet dt)
        {
            DataTableReader dtrReturn = null;            
            if (dt.Tables[0] != null)
            {
                dtrReturn = dt.Tables[0].CreateDataReader();
                dt.Dispose();
            }
            return dtrReturn;
        }

        
    }
}