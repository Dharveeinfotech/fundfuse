
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TMP.Models;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace TMP.DAL
{
    public class ClsPageMaster
    {
        ConString db = new ConString();        
        public List<PageMaster_ListAll_Result> PageMaster_ListAll(Nullable<int> pPageID, Nullable<int> pMenuID, string pPageName, Nullable<short> pIsActive, string pStatus)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("PageMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pPageID", SqlDbType.Int, pPageID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pPageName", SqlDbType.VarChar, pPageName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<PageMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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