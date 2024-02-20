using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using TMP.Models;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsHolidayMaster
    {
       ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        public int HolidayMaster_Add(int pHolidayID, string pHolidayName,DateTime pFromHolidayDate, DateTime pToHolidayDate, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HolidayMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pHolidayID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pHolidayName", SqlDbType.VarChar,  pHolidayName);
            ClsAppDatabase.AddInParameter(cmd, "@pFromHolidayDate", SqlDbType.DateTime, pFromHolidayDate);
            ClsAppDatabase.AddInParameter(cmd, "@pToHolidayDate", SqlDbType.DateTime, pToHolidayDate);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pHolidayID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public List<CountryMaster> HolidayMaster_ListAll(Nullable<int> pHolidayID)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("HolidayMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pHolidayID", SqlDbType.Int, pHolidayID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<CountryMaster>(dataReader as DbDataReader).ToList();
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
        public int HolidayMaster_Delete(Nullable<int> pHolidayID)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HolidayMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pHolidayID", SqlDbType.Int, pHolidayID);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int HolidayMaster_Update(int pHolidayID, string pHolidayName,DateTime pFromHolidayDate, DateTime pToHolidayDate, Nullable<int> pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("HolidayMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pHolidayID", SqlDbType.Int, pHolidayID);
            ClsAppDatabase.AddInParameter(cmd, "@pHolidayName", SqlDbType.VarChar, pHolidayName);
            ClsAppDatabase.AddInParameter(cmd, "@pFromHolidayDate", SqlDbType.DateTime, pFromHolidayDate);
            ClsAppDatabase.AddInParameter(cmd, "@pToHolidayDate", SqlDbType.DateTime, pToHolidayDate);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int,  pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pHolidayID"].Value);
            cmd.Dispose();
            return blnResult;
        }
    }
}