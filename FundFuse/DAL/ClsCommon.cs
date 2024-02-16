using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TMP.Models;

namespace TMP.DAL
{
    
    public class ClsCommon
    {
        ConString CN = new ConString();
        public SqlTransaction Tras { get; set; }
        public SqlConnection Conn { get; set; }
        Function FN = new Function();
        public List<CommonModel> EducationMaster_ListAll(int EduID, int IsActive)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("EducationMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pEduID", SqlDbType.Int, EduID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, IsActive);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CommonModel>(dataReader as DbDataReader).ToList();
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
        public List<CommonModel> EmployeeTypeMaster_ListAll(int EmpTypeID, int IsActive)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("EmployeeTypeMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pEmpTypeID", SqlDbType.Int, EmpTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, IsActive);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CommonModel>(dataReader as DbDataReader).ToList();
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