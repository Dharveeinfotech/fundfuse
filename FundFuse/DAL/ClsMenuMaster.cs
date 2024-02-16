using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TMP.Models;

namespace TMP.DAL
{
    public class ClsMenuMaster
    {
        #region Local Variable
        ConString db = new ConString();
        public SqlTransaction Tras { get; set; }
        public SqlConnection Conn { get; set; }
        Function FN = new Function();
        #endregion
        public List<MenuMaster> MenuMaster_ListAll(Nullable<int> pMenuID, Nullable<short> pParentMenuID, string pMenuName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue, int pIsEnable)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("MenuMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, pMenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pParentMenuID", SqlDbType.SmallInt, pParentMenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuName", SqlDbType.VarChar, pMenuName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsEnable", SqlDbType.SmallInt, pIsEnable);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);            
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<MenuMaster>(dataReader as DbDataReader).ToList();
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

        #region Menu Layer
        public int MenuMaster_UpdateEnable(MenuMaster _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("MenuMaster_UpdateEnable");
            ClsAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, _objModel.MenuID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsEnable", SqlDbType.Bit, _objModel.IsEnable);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<MenuMaster> MenuMaster_ListAllMultiLayer(MenuMaster _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("MenuMaster_ListAllMultiLayer");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pMenuID", SqlDbType.Int, _objModel.MenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pParentMenuID", SqlDbType.SmallInt, _objModel.ParentMenuID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, _objModel.IsActive);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<MenuMaster>(dataReader as DbDataReader).ToList();
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
        #endregion
    }
}