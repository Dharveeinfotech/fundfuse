using TMP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data.Entity.Core.Objects;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace TMP.DAL
{
    public class ClsMenuRoleRights
    {
        ConString db = new ConString();
        public List<MenuRoleRights> MenuRoleRights_ListAll(string pRoleIDs,int pMenuID,int pParentMenuID, string pStatus)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("MenuRoleRights_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRoleIDs", SqlDbType.VarChar, pRoleIDs);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pParentMenuID", SqlDbType.Int, pParentMenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<MenuRoleRights>(dataReader as DbDataReader).ToList();
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