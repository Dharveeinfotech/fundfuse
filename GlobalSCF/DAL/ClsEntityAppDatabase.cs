using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.DAL
{
    public class ClsEntityAppDatabase
    {
        internal static DbCommand GetSPName(string ProceName)
        {
            ConString Con = new ConString();
            DbCommand cmdReturn = Con.Database.Connection.CreateCommand();
            if (Con.Database.Connection.State == ConnectionState.Closed)
                Con.Database.Connection.Open();
            cmdReturn.Connection = Con.Database.Connection;
            cmdReturn.CommandType = CommandType.StoredProcedure;
            cmdReturn.CommandText = ProceName;
            return cmdReturn;
        }
        internal static SqlParameter AddInParameter(DbCommand sqlCMD, string strPrName, SqlDbType prDBType, object prValue)
        {
            return AddInParameter(sqlCMD, strPrName, prDBType, -1, prValue);
        }
        internal static SqlParameter AddInParameter(DbCommand sqlCMD, string strPrName, SqlDbType prDBType, int prSize, object prValue)
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
        internal static SqlParameter AddOutParameter(DbCommand cmd, string strPrName, SqlDbType sqlDbType)
        {
            return AddOutParameter(cmd, strPrName, sqlDbType, -1);
        }
        internal static SqlParameter AddOutParameter(DbCommand cmd, string strPrName, SqlDbType sqlDbType, int size)
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
        internal static SqlParameter AddReturnParameter(DbCommand cmd, string strPrName, SqlDbType sqlDbType)
        {
            return AddReturnParameter(cmd, strPrName, sqlDbType, -1);
        }
        internal static SqlParameter AddReturnParameter(DbCommand cmd, string strPrName, SqlDbType sqlDbType, int size)
        {
            SqlParameter parm = null;
            try
            {
                parm = new SqlParameter(strPrName, sqlDbType, size);
                parm.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(parm);
            }
            catch (Exception ex)
            {
                parm = null;
                throw ex;
            }
            return parm;
        }
        internal static object AddInParameter(DbCommand cmd, string p, SqlDbType sqlDbType)
        {
            throw new NotImplementedException();
        }
        internal static DataTableReader GetDataTableReader(DbCommand cmdSql, DataSet dt)
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