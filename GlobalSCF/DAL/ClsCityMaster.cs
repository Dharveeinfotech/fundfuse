using TMP.Models;
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
    public class ClsCityMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        //public SqlCommand cmd { get; set; }
        public SqlConnection conn { get; set; }
        public List<CountryMaster> CityMaster_ListAll(Nullable<int> pCityID, Nullable<int> pStateID, string pCityName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CityMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, pCityID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCityName", SqlDbType.VarChar, pCityName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
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
        public int Citymaster_add(Nullable<int> CityID, string pCityCode, string pCityName, Nullable<int> pStateID, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CityMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCityID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCityCode", SqlDbType.Char, pCityCode);
            ClsAppDatabase.AddInParameter(cmd, "@pCityName", SqlDbType.VarChar, pCityName);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCityID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int Citymaster_Update(int pCityID, string pCityCode, string pCityName, Nullable<int> pStateID, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CityMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, pCityID);
            ClsAppDatabase.AddInParameter(cmd, "@pCityCode", SqlDbType.VarChar, pCityCode);
            ClsAppDatabase.AddInParameter(cmd, "@pCityName", SqlDbType.VarChar, pCityName);
            ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, pStateID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int CityMaster_Delete(Nullable<int> pCityID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CityMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, pCityID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
    }
}