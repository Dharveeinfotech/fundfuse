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
    public class ClsContactUs
    {
        #region Local Variable
        ConString CN = new ConString();
        public SqlTransaction Tras { get; set; }
        public SqlConnection Conn { get; set; }
        Function FN = new Function();
        #endregion
        public int ContactDetails_Add(ContactUsModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("ContactDetails_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pContactID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.NVarChar, _objModel.FirstName == null ? "" : _objModel.FirstName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.NVarChar, _objModel.LastName == null ? "" : _objModel.LastName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.NVarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.NVarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.NVarChar, _objModel.EmailID == null ? "" : _objModel.EmailID.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.NVarChar, _objModel.Website == null ? "" : _objModel.Website.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pContactDesc", SqlDbType.NVarChar, _objModel.ContactDesc == null ? "" : _objModel.ContactDesc.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyID", SqlDbType.Int, _objModel.CompanyID);
            ClsAppDatabase.AddInParameter(cmd, "@pLanguageID", SqlDbType.Int, _objModel.LanguageID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.NVarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pContactID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<ContactUsModel> ContactDetails_ListAll(ContactUsModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("ContactDetails_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pContactID", SqlDbType.Int, _objModel.ContactID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ContactUsModel>(dataReader as DbDataReader).ToList();
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
        public DataTable ContactDetails_ListAllTable(ContactUsModel _objModel)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("ContactDetails_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pContactID", SqlDbType.Int, _objModel.ContactID);           
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            cmd.Connection.Close();
            cmd.Dispose();
            return dt;
        }
    }
}