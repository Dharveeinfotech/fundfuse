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
    public class ClsLanguageMaster
    {
        ConString db = new ConString();
        public List<CountryMaster> LanguageMaster_ListAll(Nullable<int> pLanguageID, string pLanguageName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("LanguageMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pLanguageID", SqlDbType.Int, pLanguageID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pLanguageName", SqlDbType.VarChar, pLanguageName);
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
        public int LanguageMaster_Add(Nullable<int> pLanguageID, string pLanguageName, Nullable<int> pIsDefault, Nullable<int> pCreateBy, string pCreateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("LanguageMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pLanguageID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pLanguageName", SqlDbType.VarChar, pLanguageName);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDefault", SqlDbType.Int, pIsDefault);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pLanguageID"].Value);
            cmd.Connection.Close();
            cmd.Dispose();
            return blnResult;
        }
        public int LanguageMaster_Update(int pLanguageID, string pLanguageName, Nullable<int> pIsDefault, int pUpdateBy, string pUpdateIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("LanguageMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pLanguageID", SqlDbType.Int, pLanguageID);
            ClsAppDatabase.AddInParameter(cmd, "@pLanguageName", SqlDbType.VarChar, pLanguageName);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDefault", SqlDbType.VarChar, pIsDefault);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            blnResult = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Dispose();            
            return blnResult;
        }
    }
}