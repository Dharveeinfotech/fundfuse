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
    public class ClsCustomerTypeMaster
    {
        #region Local Variable
        ConString db = new ConString();
        #endregion
        public List<CustomerTypeMaster_ListAll_Result> CustomerTypeMaster_ListAll(Nullable<int> pCustomerTypeID, string pCustomerTypeName, string pCustomerTypeCode, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerTypeMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeName", SqlDbType.VarChar, pCustomerTypeName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeCode", SqlDbType.Char, pCustomerTypeCode);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<CustomerTypeMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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