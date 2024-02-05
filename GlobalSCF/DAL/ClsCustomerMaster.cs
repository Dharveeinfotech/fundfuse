
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
    public class ClsCustomerMaster
    {
        #region Local Variable
        ConString CN = new ConString();
        public SqlTransaction Tras { get; set; }
        public SqlConnection Conn { get; set; }
        Function FN = new Function();
        #endregion
        int ProcessBy = 0;

        #region Customer Temp Master
        public int CustomerMasterTemp_Add(CustomerTempModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMasterTemp_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerTempID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pTradingName", SqlDbType.VarChar, _objModel.TradingName == null ? "" : _objModel.TradingName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactFName", SqlDbType.VarChar, _objModel.ContactFName == null ? "" : _objModel.ContactFName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactMName", SqlDbType.VarChar, _objModel.ContactMName == null ? "" : _objModel.ContactMName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactLName", SqlDbType.VarChar, _objModel.ContactLName == null ? "" : _objModel.ContactLName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pNationalityID", SqlDbType.Int, _objModel.NationalityID);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, _objModel.IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherIndustry", SqlDbType.VarChar, _objModel.OtherIndustry == null ? "" : _objModel.OtherIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pSubIndustryID", SqlDbType.Int, _objModel.SubIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSubIndustry", SqlDbType.VarChar, _objModel.OtherSubIndustry == null ? "" : _objModel.OtherSubIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pActivationCode", SqlDbType.VarChar, _objModel.ActivationCode == null ? "" : _objModel.ActivationCode);
            ClsAppDatabase.AddInParameter(cmd, "@pActivationLink", SqlDbType.VarChar, _objModel.ActivationLink == null ? "" : _objModel.ActivationLink);
            ClsAppDatabase.AddInParameter(cmd, "@pConfirmDate", SqlDbType.DateTime, _objModel.ConfirmDate);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerTempID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerMasterTemp_Update(CustomerTempModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMasterTemp_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTempID", SqlDbType.Int, _objModel.CustomerTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pTradingName", SqlDbType.VarChar, _objModel.TradingName == null ? "" : _objModel.TradingName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactFName", SqlDbType.VarChar, _objModel.ContactFName == null ? "" : _objModel.ContactFName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactMName", SqlDbType.VarChar, _objModel.ContactMName == null ? "" : _objModel.ContactMName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactLName", SqlDbType.VarChar, _objModel.ContactLName == null ? "" : _objModel.ContactLName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pNationalityID", SqlDbType.Int, _objModel.NationalityID);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, _objModel.IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherIndustry", SqlDbType.VarChar, _objModel.OtherIndustry == null ? "" : _objModel.OtherIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pSubIndustryID", SqlDbType.Int, _objModel.SubIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSubIndustry", SqlDbType.VarChar, _objModel.OtherSubIndustry == null ? "" : _objModel.OtherSubIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pActivationCode", SqlDbType.VarChar, _objModel.ActivationCode == null ? "" : _objModel.ActivationCode);
            ClsAppDatabase.AddInParameter(cmd, "@pActivationLink", SqlDbType.VarChar, _objModel.ActivationLink == null ? "" : _objModel.ActivationLink);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerMasterTemp_UpdateActivationCode(CustomerTempModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMasterTemp_UpdateActivationCode");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTempID", SqlDbType.VarChar, _objModel.CustomerTempID);
            ClsAppDatabase.AddOutParameter(cmd, "@pActivationCode", SqlDbType.VarChar);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerMasterTemp_ActivateCode(CustomerTempModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMasterTemp_ActivateCode");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTempID", SqlDbType.Int, _objModel.CustomerTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pActivationCode", SqlDbType.VarChar, _objModel.ActivationCode == null ? "" : _objModel.ActivationCode);
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerID", SqlDbType.Int);
            ClsAppDatabase.AddOutParameter(cmd, "@pLoginName", SqlDbType.VarChar);
            ClsAppDatabase.AddOutParameter(cmd, "@pPassword", SqlDbType.VarChar);
            ClsAppDatabase.AddInParameter(cmd, "@pServerDate", SqlDbType.DateTime, _objModel.ServerDate);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerTempModel> CustomerMasterTemp_ListAll(CustomerTempModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMasterTemp_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTempID", SqlDbType.Int, _objModel.CustomerTempID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerTempModel>(dataReader as DbDataReader).ToList();
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
        public int CustomerMasterTemp_GetActivateLink(CustomerTempModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMasterTemp_GetActivateLink");
            ClsAppDatabase.AddInParameter(cmd, "@pActivationLink", SqlDbType.VarChar, _objModel.ActivationLink);
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerTempID", SqlDbType.Int);
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerTempID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public DataTable CustomerMasterTemp_DataTable(CustomerTempModel _objModel)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMasterTemp_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTempID", SqlDbType.Int, _objModel.CustomerTempID);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            cmd.Connection.Close();
            cmd.Dispose();
            return dt;
        }

        public DataTable CustomerMaster_DataTable(CustomerMasterModel _objModel)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, _objModel.CustomerTypeIDs == null ? "" : _objModel.CustomerTypeIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            cmd.Connection.Close();
            cmd.Dispose();
            return dt;
        }
        #endregion

        #region Customer Master
        public int CustomerMaster_Add(CustomerMasterModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTempID", SqlDbType.Int, _objModel.CustomerTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pTradingName", SqlDbType.VarChar, _objModel.TradingName == null ? "" : _objModel.TradingName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactFName", SqlDbType.VarChar, _objModel.ContactFName == null ? "" : _objModel.ContactFName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactMName", SqlDbType.VarChar, _objModel.ContactMName == null ? "" : _objModel.ContactMName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactLName", SqlDbType.VarChar, _objModel.ContactLName == null ? "" : _objModel.ContactLName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pNationalityID", SqlDbType.Int, _objModel.NationalityID);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, _objModel.IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherIndustry", SqlDbType.VarChar, _objModel.OtherIndustry == null ? "" : _objModel.OtherIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pSubIndustryID", SqlDbType.Int, _objModel.SubIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSubIndustry", SqlDbType.VarChar, _objModel.OtherSubIndustry == null ? "" : _objModel.OtherSubIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerMaster_Update(CustomerMasterModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTempID", SqlDbType.Int, _objModel.CustomerTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pTradingName", SqlDbType.VarChar, _objModel.TradingName == null ? "" : _objModel.TradingName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactFName", SqlDbType.VarChar, _objModel.ContactFName == null ? "" : _objModel.ContactFName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactMName", SqlDbType.VarChar, _objModel.ContactMName == null ? "" : _objModel.ContactMName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactLName", SqlDbType.VarChar, _objModel.ContactLName == null ? "" : _objModel.ContactLName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pNationalityID", SqlDbType.Int, _objModel.NationalityID);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, _objModel.IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherIndustry", SqlDbType.VarChar, _objModel.OtherIndustry == null ? "" : _objModel.OtherIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pSubIndustryID", SqlDbType.Int, _objModel.SubIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSubIndustry", SqlDbType.VarChar, _objModel.OtherSubIndustry == null ? "" : _objModel.OtherSubIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerMaster_UpdateIsAgree(CustomerMasterModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_UpdateIsAgree");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerMasterHistory_Add(CustomerMasterModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMasterHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTempID", SqlDbType.Int, _objModel.CustomerTempID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerCode", SqlDbType.VarChar, _objModel.CustomerCode == null ? "" : _objModel.CustomerCode);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pTradingName", SqlDbType.VarChar, _objModel.TradingName == null ? "" : _objModel.TradingName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactFName", SqlDbType.VarChar, _objModel.ContactFName == null ? "" : _objModel.ContactFName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactMName", SqlDbType.VarChar, _objModel.ContactMName == null ? "" : _objModel.ContactMName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactLName", SqlDbType.VarChar, _objModel.ContactLName == null ? "" : _objModel.ContactLName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pNationalityID", SqlDbType.Int, _objModel.NationalityID);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, _objModel.IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherIndustry", SqlDbType.VarChar, _objModel.OtherIndustry == null ? "" : _objModel.OtherIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pSubIndustryID", SqlDbType.Int, _objModel.SubIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSubIndustry", SqlDbType.VarChar, _objModel.OtherSubIndustry == null ? "" : _objModel.OtherSubIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerMaster_UpdateAllStatus(CustomerMasterModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_UpdateAllStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerMasterModel> CustomerMaster_ListAll(CustomerMasterModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, _objModel.CustomerTypeIDs == null ? "" : _objModel.CustomerTypeIDs);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMasterModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerMasterModel> CustomerMasterHistory_ListAll(CustomerMasterModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMasterHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerHistoryID", SqlDbType.Int, _objModel.CustomerHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, _objModel.CustomerTypeIDs == null ? "" : _objModel.CustomerTypeIDs);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMasterModel>(dataReader as DbDataReader).ToList();
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

        public List<CustomerMasterModel> CustomerMasterHistory_ListAllBind(CustomerMasterModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMasterHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerHistoryID", SqlDbType.Int, _objModel.CustomerHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, _objModel.CustomerTypeIDs == null ? "" : _objModel.CustomerTypeIDs);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMasterModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerMasterModel> CustomerMaster_BindAllDocument(CustomerMasterModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMaster_BindAllDocument");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMasterModel>(dataReader as DbDataReader).ToList();
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
        public DataTable CustomerMaster_ListAllTable(CustomerMasterModel _objModel)
        {
            DataTable dt = new DataTable();
            if (_objModel.CompanyName == null) { _objModel.CompanyName = ""; }
            if (_objModel.Status == null) { _objModel.Status = ""; }
            if (_objModel.Keywordvalue == null) { _objModel.Keywordvalue = ""; }
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerTypeIDs", SqlDbType.VarChar, _objModel.CustomerTypeIDs == null ? "" : _objModel.CustomerTypeIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            cmd.Connection.Close();
            cmd.Dispose();
            return dt;
        }
        public List<CustomerMasterModel> UserMaster_CustomerTypeListAll(CustomerMasterModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMaster_CustomerTypeListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMasterModel>(dataReader as DbDataReader).ToList();
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
        public DataSet CustomerMaster_BindTemplate_DataSet(CustomerMasterModel _objModel)
        {
            DataSet DS = new DataSet();
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMaster_BindTemplate");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            DbDataAdapter DataAdpt = DbProviderFactories.GetFactory("System.Data.SqlClient").CreateDataAdapter();
            try
            {
                DataAdpt.SelectCommand = cmd;
                DataAdpt.Fill(DS);
                return DS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DataAdpt.Dispose();
                cmd.Connection.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
            }
        }
        #endregion

        #region Customer Info Detail
        public int CustomerInfoDetail_Add(CustomerInfoModel _objModel)
        {
            if (_objModel.IncDate == null) { _objModel.IncDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IncDate == Convert.ToDateTime("1/1/0001")) { _objModel.IncDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.LicExpDate == null) { _objModel.LicExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.LicExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.LicExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerInfoDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerInfoID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyTypeID", SqlDbType.Int, _objModel.CompanyTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCompanyType", SqlDbType.VarChar, _objModel.OtherCompanyType == null ? "" : _objModel.OtherCompanyType);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIncCountryID", SqlDbType.Int, _objModel.IncCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIncDate", SqlDbType.DateTime, _objModel.IncDate);
            ClsAppDatabase.AddInParameter(cmd, "@pLicExpDate", SqlDbType.DateTime, _objModel.LicExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIssuingAuth", SqlDbType.VarChar, _objModel.IssuingAuth == null ? "" : _objModel.IssuingAuth);
            ClsAppDatabase.AddInParameter(cmd, "@pAbtBusiness", SqlDbType.VarChar, _objModel.AbtBusiness == null ? "" : _objModel.AbtBusiness);
            ClsAppDatabase.AddInParameter(cmd, "@pBusActivity", SqlDbType.VarChar, _objModel.BusActivity == null ? "" : _objModel.BusActivity);
            ClsAppDatabase.AddInParameter(cmd, "@pEmpCount", SqlDbType.VarChar, _objModel.EmpCount == null ? "" : _objModel.EmpCount);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pAnnTurnOver", SqlDbType.Decimal, _objModel.AnnTurnOver);
            ClsAppDatabase.AddInParameter(cmd, "@pSaleVolume", SqlDbType.Decimal, _objModel.SaleVolume);
            ClsAppDatabase.AddInParameter(cmd, "@pNetProfit", SqlDbType.Decimal, _objModel.NetProfit);
            ClsAppDatabase.AddInParameter(cmd, "@pSaleInvTerms", SqlDbType.VarChar, _objModel.SaleInvTerms == null ? "" : _objModel.SaleInvTerms);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSaleInvTerms", SqlDbType.VarChar, _objModel.OtherSaleInvTerms == null ? "" : _objModel.OtherSaleInvTerms);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompPubList", SqlDbType.Bit, _objModel.IsCompPubList);
            ClsAppDatabase.AddInParameter(cmd, "@pExchangeName", SqlDbType.VarChar, _objModel.ExchangeName == null ? "" : _objModel.ExchangeName);
            ClsAppDatabase.AddInParameter(cmd, "@pTickerCode", SqlDbType.VarChar, _objModel.TickerCode == null ? "" : _objModel.TickerCode);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompGrpPart", SqlDbType.Bit, _objModel.IsCompGrpPart);
            ClsAppDatabase.AddInParameter(cmd, "@pGroupName", SqlDbType.VarChar, _objModel.GroupName == null ? "" : _objModel.GroupName);
            ClsAppDatabase.AddInParameter(cmd, "@pGroupWebsite", SqlDbType.VarChar, _objModel.GroupWebsite == null ? "" : _objModel.GroupWebsite);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompFinServ", SqlDbType.Bit, _objModel.IsCompFinServ);
            ClsAppDatabase.AddInParameter(cmd, "@pRegulatorName", SqlDbType.VarChar, _objModel.RegulatorName == null ? "" : _objModel.RegulatorName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pObligorIDs", SqlDbType.VarChar, _objModel.ObligorIDs == null ? "" : _objModel.ObligorIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdTradeLic", SqlDbType.VarChar, _objModel.UpdTradeLic == null ? "" : _objModel.UpdTradeLic);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdBusProfile", SqlDbType.VarChar, _objModel.UpdBusProfile == null ? "" : _objModel.UpdBusProfile);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdAuditFin", SqlDbType.VarChar, _objModel.UpdAuditFin == null ? "" : _objModel.UpdAuditFin);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdInHouseFin", SqlDbType.VarChar, _objModel.UpdInHouseFin == null ? "" : _objModel.UpdInHouseFin);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdAgingRpt", SqlDbType.VarChar, _objModel.UpdAgingRpt == null ? "" : _objModel.UpdAgingRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerInfoID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerInfoDetailHistory_Add(CustomerInfoModel _objModel)
        {
            if (_objModel.IncDate == null) { _objModel.IncDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IncDate == Convert.ToDateTime("1/1/0001")) { _objModel.IncDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.LicExpDate == null) { _objModel.LicExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.LicExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.LicExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerInfoDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerInfoHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyTypeID", SqlDbType.Int, _objModel.CompanyTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCompanyType", SqlDbType.VarChar, _objModel.OtherCompanyType == null ? "" : _objModel.OtherCompanyType);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIncCountryID", SqlDbType.Int, _objModel.IncCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIncDate", SqlDbType.DateTime, _objModel.IncDate);
            ClsAppDatabase.AddInParameter(cmd, "@pLicExpDate", SqlDbType.DateTime, _objModel.LicExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIssuingAuth", SqlDbType.VarChar, _objModel.IssuingAuth == null ? "" : _objModel.IssuingAuth);
            ClsAppDatabase.AddInParameter(cmd, "@pAbtBusiness", SqlDbType.VarChar, _objModel.AbtBusiness == null ? "" : _objModel.AbtBusiness);
            ClsAppDatabase.AddInParameter(cmd, "@pBusActivity", SqlDbType.VarChar, _objModel.BusActivity == null ? "" : _objModel.BusActivity);
            ClsAppDatabase.AddInParameter(cmd, "@pEmpCount", SqlDbType.VarChar, _objModel.EmpCount == null ? "" : _objModel.EmpCount);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pAnnTurnOver", SqlDbType.Decimal, _objModel.AnnTurnOver);
            ClsAppDatabase.AddInParameter(cmd, "@pSaleVolume", SqlDbType.Decimal, _objModel.SaleVolume);
            ClsAppDatabase.AddInParameter(cmd, "@pNetProfit", SqlDbType.Decimal, _objModel.NetProfit);
            ClsAppDatabase.AddInParameter(cmd, "@pSaleInvTerms", SqlDbType.VarChar, _objModel.SaleInvTerms == null ? "" : _objModel.SaleInvTerms);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSaleInvTerms", SqlDbType.VarChar, _objModel.OtherSaleInvTerms == null ? "" : _objModel.OtherSaleInvTerms);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompPubList", SqlDbType.Bit, _objModel.IsCompPubList);
            ClsAppDatabase.AddInParameter(cmd, "@pExchangeName", SqlDbType.VarChar, _objModel.ExchangeName == null ? "" : _objModel.ExchangeName);
            ClsAppDatabase.AddInParameter(cmd, "@pTickerCode", SqlDbType.VarChar, _objModel.TickerCode == null ? "" : _objModel.TickerCode);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompGrpPart", SqlDbType.Bit, _objModel.IsCompGrpPart);
            ClsAppDatabase.AddInParameter(cmd, "@pGroupName", SqlDbType.VarChar, _objModel.GroupName == null ? "" : _objModel.GroupName);
            ClsAppDatabase.AddInParameter(cmd, "@pGroupWebsite", SqlDbType.VarChar, _objModel.GroupWebsite == null ? "" : _objModel.GroupWebsite);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompFinServ", SqlDbType.Bit, _objModel.IsCompFinServ);
            ClsAppDatabase.AddInParameter(cmd, "@pRegulatorName", SqlDbType.VarChar, _objModel.RegulatorName == null ? "" : _objModel.RegulatorName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pObligorIDs", SqlDbType.VarChar, _objModel.ObligorIDs == null ? "" : _objModel.ObligorIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdTradeLic", SqlDbType.VarChar, _objModel.UpdTradeLic == null ? "" : _objModel.UpdTradeLic);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdBusProfile", SqlDbType.VarChar, _objModel.UpdBusProfile == null ? "" : _objModel.UpdBusProfile);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdAuditFin", SqlDbType.VarChar, _objModel.UpdAuditFin == null ? "" : _objModel.UpdAuditFin);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdInHouseFin", SqlDbType.VarChar, _objModel.UpdInHouseFin == null ? "" : _objModel.UpdInHouseFin);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdAgingRpt", SqlDbType.VarChar, _objModel.UpdAgingRpt == null ? "" : _objModel.UpdAgingRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerInfoHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerInfoDetail_Delete(CustomerInfoModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerInfoDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerInfoDetail_Update(CustomerInfoModel _objModel)
        {
            if (_objModel.IncDate == null) { _objModel.IncDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IncDate == Convert.ToDateTime("1/1/0001")) { _objModel.IncDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.LicExpDate == null) { _objModel.LicExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.LicExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.LicExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerInfoDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyTypeID", SqlDbType.Int, _objModel.CompanyTypeID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCompanyType", SqlDbType.VarChar, _objModel.OtherCompanyType == null ? "" : _objModel.OtherCompanyType);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIncCountryID", SqlDbType.Int, _objModel.IncCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIncDate", SqlDbType.DateTime, _objModel.IncDate);
            ClsAppDatabase.AddInParameter(cmd, "@pLicExpDate", SqlDbType.DateTime, _objModel.LicExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIssuingAuth", SqlDbType.VarChar, _objModel.IssuingAuth == null ? "" : _objModel.IssuingAuth);
            ClsAppDatabase.AddInParameter(cmd, "@pAbtBusiness", SqlDbType.VarChar, _objModel.AbtBusiness == null ? "" : _objModel.AbtBusiness);
            ClsAppDatabase.AddInParameter(cmd, "@pBusActivity", SqlDbType.VarChar, _objModel.BusActivity == null ? "" : _objModel.BusActivity);
            ClsAppDatabase.AddInParameter(cmd, "@pEmpCount", SqlDbType.VarChar, _objModel.EmpCount == null ? "" : _objModel.EmpCount);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pAnnTurnOver", SqlDbType.Decimal, _objModel.AnnTurnOver);
            ClsAppDatabase.AddInParameter(cmd, "@pSaleVolume", SqlDbType.Decimal, _objModel.SaleVolume);
            ClsAppDatabase.AddInParameter(cmd, "@pNetProfit", SqlDbType.Decimal, _objModel.NetProfit);
            ClsAppDatabase.AddInParameter(cmd, "@pSaleInvTerms", SqlDbType.VarChar, _objModel.SaleInvTerms == null ? "" : _objModel.SaleInvTerms);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSaleInvTerms", SqlDbType.VarChar, _objModel.OtherSaleInvTerms == null ? "" : _objModel.OtherSaleInvTerms);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompPubList", SqlDbType.Bit, _objModel.IsCompPubList);
            ClsAppDatabase.AddInParameter(cmd, "@pExchangeName", SqlDbType.VarChar, _objModel.ExchangeName == null ? "" : _objModel.ExchangeName);
            ClsAppDatabase.AddInParameter(cmd, "@pTickerCode", SqlDbType.VarChar, _objModel.TickerCode == null ? "" : _objModel.TickerCode);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompGrpPart", SqlDbType.Bit, _objModel.IsCompGrpPart);
            ClsAppDatabase.AddInParameter(cmd, "@pGroupName", SqlDbType.VarChar, _objModel.GroupName == null ? "" : _objModel.GroupName);
            ClsAppDatabase.AddInParameter(cmd, "@pGroupWebsite", SqlDbType.VarChar, _objModel.GroupWebsite == null ? "" : _objModel.GroupWebsite);
            ClsAppDatabase.AddInParameter(cmd, "@pIsCompFinServ", SqlDbType.Bit, _objModel.IsCompFinServ);
            ClsAppDatabase.AddInParameter(cmd, "@pRegulatorName", SqlDbType.VarChar, _objModel.RegulatorName == null ? "" : _objModel.RegulatorName);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pObligorIDs", SqlDbType.VarChar, _objModel.ObligorIDs == null ? "" : _objModel.ObligorIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdTradeLic", SqlDbType.VarChar, _objModel.UpdTradeLic == null ? "" : _objModel.UpdTradeLic);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdBusProfile", SqlDbType.VarChar, _objModel.UpdBusProfile == null ? "" : _objModel.UpdBusProfile);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdAuditFin", SqlDbType.VarChar, _objModel.UpdAuditFin == null ? "" : _objModel.UpdAuditFin);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdInHouseFin", SqlDbType.VarChar, _objModel.UpdInHouseFin == null ? "" : _objModel.UpdInHouseFin);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdAgingRpt", SqlDbType.VarChar, _objModel.UpdAgingRpt == null ? "" : _objModel.UpdAgingRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerInfoDetail_UpdateNotRequired(CustomerInfoModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerInfoDetail_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerInfoModel> CustomerInfoDetail_ListAll(CustomerInfoModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerInfoDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerInfoModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerInfoModel> CustomerInfoDetailHistory_ListAll(CustomerInfoModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerInfoDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerInfoHistoryID", SqlDbType.Int, _objModel.CustomerInfoHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerInfoModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerInfoModel> CustomerInfoDetailHistory_ListAllBind(CustomerInfoModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerInfoDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerInfoHistoryID", SqlDbType.Int, _objModel.CustomerInfoHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerInfoModel>(dataReader as DbDataReader).ToList();
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

        #region Customer Doc Detail
        public int CustomerDocDetail_Add(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerDocDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerDocID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsUpload", SqlDbType.Bit, _objModel.IsUpload);
            ClsAppDatabase.AddInParameter(cmd, "@pAttachment", SqlDbType.VarChar, _objModel.Attachment == null ? "" : _objModel.Attachment);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerDocID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerDocDetail_Update(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerDocDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerDocID", SqlDbType.Int, _objModel.CustomerDocID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsUpload", SqlDbType.Bit, _objModel.IsUpload);
            ClsAppDatabase.AddInParameter(cmd, "@pAttachment", SqlDbType.VarChar, _objModel.Attachment == null ? "" : _objModel.Attachment);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerDocDetail_Delete(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerDocDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerDocID", SqlDbType.Int, _objModel.CustomerDocID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerDocDetail_UpdateNotRequired(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerDocDetail_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerDocID", SqlDbType.Int, _objModel.CustomerDocID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerDocDetailHistory_Add(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerDocDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerDocHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerDocID", SqlDbType.Int, _objModel.CustomerDocID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsUpload", SqlDbType.Bit, _objModel.IsUpload);
            ClsAppDatabase.AddInParameter(cmd, "@pAttachment", SqlDbType.VarChar, _objModel.Attachment == null ? "" : _objModel.Attachment);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerDocHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerDocModel> CustomerDocDetail_ListAll(CustomerDocModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerDocDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerDocID", SqlDbType.Int, _objModel.CustomerDocID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsUserDoc", SqlDbType.SmallInt, _objModel.IsUserDocInt);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsInvestor", SqlDbType.SmallInt, _objModel.IsInvestorInt);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerDocModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerDocModel> CustomerDocDetailHistory_ListAll(CustomerDocModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerDocDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerDocHistoryID", SqlDbType.Int, _objModel.CustomerDocHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerDocID", SqlDbType.Int, _objModel.CustomerDocID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerDocModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerDocModel> CustomerDocDetailHistory_ListAllBind(CustomerDocModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerDocDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerDocHistoryID", SqlDbType.Int, _objModel.CustomerDocHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerDocID", SqlDbType.Int, _objModel.CustomerDocID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsUserDoc", SqlDbType.SmallInt, _objModel.IsUserDocInt);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsInvestor", SqlDbType.SmallInt, _objModel.IsInvestorInt);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerDocModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerTypeDocumentDetails_ListAll_Result> CustomerTypeDocumentDetails_ListAll(CustomerTypeDocumentDetails_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerTypeDocumentDetails_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeDocDetID", SqlDbType.Int, _objModel.CustomerTypeDocDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, _objModel.IsActive);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsUserDoc", SqlDbType.SmallInt, _objModel.IsUserDoc);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsInvestor", SqlDbType.SmallInt, _objModel.IsInvestor);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerTypeDocumentDetails_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int CustomerMaster_UpdateIsAgree(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_UpdateIsAgree");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerMaster_UpdateAllStatus(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_UpdateAllStatus");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerDocModel> CustomerMaster_GetCount(CustomerDocModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMaster_GetCount");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerDocModel>(dataReader as DbDataReader).ToList();
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
        //public List<CustomerDocModel> CustomerMaster_CheckScreening(CustomerDocModel _objModel)
        //{
        //    bool blnResult = false;
        //    SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerMaster_CheckScreening");
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
        //    ClsAppDatabase.AddOutParameter(cmd, "@pIsCheckScreening", SqlDbType.Int);
        //    cmd.Transaction = Tras;
        //    int Row = cmd.ExecuteNonQuery();
        //    blnResult = Convert.ToBoolean(cmd.Parameters["@pIsCheckScreening"].Value);
        //    cmd.Parameters.Clear();
        //    cmd.Dispose();
        //    return blnResult;
        //}
        public List<CustomerDocModel> CustomerMaster_CheckScreening(CustomerDocModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMaster_CheckScreening");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerDocModel>(dataReader as DbDataReader).ToList();
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

        #region Customer Share Holder Detail
        public int CustomerShareHoldDetail_Add(CustomerShareHolderModel _objModel)
        {
            if (_objModel.PassIssueDate == null) { _objModel.PassIssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassIssueDate == Convert.ToDateTime("1/1/0001")) { _objModel.PassIssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassExpDate == null) { _objModel.PassExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.PassExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerShareHoldDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pParentID", SqlDbType.Int, _objModel.ParentID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.Char, _objModel.CustType == null ? "" : _objModel.CustType);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
            ClsAppDatabase.AddInParameter(cmd, "@pSharePer", SqlDbType.Decimal, _objModel.SharePer);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pNatCountryID", SqlDbType.Int, _objModel.NatCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pPassNo", SqlDbType.VarChar, _objModel.PassNo == null ? "" : _objModel.PassNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPassIssueDate", SqlDbType.DateTime, _objModel.PassIssueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pPassExpDate", SqlDbType.DateTime, _objModel.PassExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pPassCountryID", SqlDbType.Int, _objModel.PassCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdPassport", SqlDbType.VarChar, _objModel.UpdPassport == null ? "" : _objModel.UpdPassport);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdNatIden", SqlDbType.VarChar, _objModel.UpdNatIden == null ? "" : _objModel.UpdNatIden);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdTradeLic", SqlDbType.VarChar, _objModel.UpdTradeLic == null ? "" : _objModel.UpdTradeLic);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdPOA", SqlDbType.VarChar, _objModel.UpdPOA == null ? "" : _objModel.UpdPOA);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleIDs", SqlDbType.VarChar, _objModel.RoleIDs == null ? "" : _objModel.RoleIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerShareHoldID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerShareHoldDetail_Update(CustomerShareHolderModel _objModel)
        {
            if (_objModel.PassIssueDate == null) { _objModel.PassIssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassIssueDate == Convert.ToDateTime("1/1/0001")) { _objModel.PassIssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassExpDate == null) { _objModel.PassExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.PassExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerShareHoldDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pParentID", SqlDbType.Int, _objModel.ParentID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.Char, _objModel.CustType == null ? "" : _objModel.CustType);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
            ClsAppDatabase.AddInParameter(cmd, "@pSharePer", SqlDbType.Decimal, _objModel.SharePer);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pNatCountryID", SqlDbType.Int, _objModel.NatCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pPassNo", SqlDbType.VarChar, _objModel.PassNo == null ? "" : _objModel.PassNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPassIssueDate", SqlDbType.DateTime, _objModel.PassIssueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pPassExpDate", SqlDbType.DateTime, _objModel.PassExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pPassCountryID", SqlDbType.Int, _objModel.PassCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdPassport", SqlDbType.VarChar, _objModel.UpdPassport == null ? "" : _objModel.UpdPassport);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdNatIden", SqlDbType.VarChar, _objModel.UpdNatIden == null ? "" : _objModel.UpdNatIden);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdTradeLic", SqlDbType.VarChar, _objModel.UpdTradeLic == null ? "" : _objModel.UpdTradeLic);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdPOA", SqlDbType.VarChar, _objModel.UpdPOA == null ? "" : _objModel.UpdPOA);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleIDs", SqlDbType.VarChar, _objModel.RoleIDs == null ? "" : _objModel.RoleIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerShareHoldDetail_Delete(CustomerShareHolderModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerShareHoldDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerShareHoldDetail_UpdateNotRequired(CustomerShareHolderModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerShareHoldDetail_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerShareHoldDetailHistory_Add(CustomerShareHolderModel _objModel)
        {
            if (_objModel.PassIssueDate == null) { _objModel.PassIssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassIssueDate == Convert.ToDateTime("1/1/0001")) { _objModel.PassIssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassExpDate == null) { _objModel.PassExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.PassExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.PassExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerShareHoldDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerShareHoldHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pParentID", SqlDbType.Int, _objModel.ParentID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.Char, _objModel.CustType == null ? "" : _objModel.CustType);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
            ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
            ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
            ClsAppDatabase.AddInParameter(cmd, "@pSharePer", SqlDbType.Decimal, _objModel.SharePer);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pNatCountryID", SqlDbType.Int, _objModel.NatCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pPassNo", SqlDbType.VarChar, _objModel.PassNo == null ? "" : _objModel.PassNo);
            ClsAppDatabase.AddInParameter(cmd, "@pPassIssueDate", SqlDbType.DateTime, _objModel.PassIssueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pPassExpDate", SqlDbType.DateTime, _objModel.PassExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pPassCountryID", SqlDbType.Int, _objModel.PassCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdPassport", SqlDbType.VarChar, _objModel.UpdPassport == null ? "" : _objModel.UpdPassport);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdNatIden", SqlDbType.VarChar, _objModel.UpdNatIden == null ? "" : _objModel.UpdNatIden);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdTradeLic", SqlDbType.VarChar, _objModel.UpdTradeLic == null ? "" : _objModel.UpdTradeLic);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdPOA", SqlDbType.VarChar, _objModel.UpdPOA == null ? "" : _objModel.UpdPOA);
            ClsAppDatabase.AddInParameter(cmd, "@pRoleIDs", SqlDbType.VarChar, _objModel.RoleIDs == null ? "" : _objModel.RoleIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerShareHoldHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerShareHolderModel> CustomerShareHoldDetail_ListAll(CustomerShareHolderModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerShareHoldDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.VarChar, _objModel.CustType == null ? "" : _objModel.CustType);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pParentID", SqlDbType.Int, _objModel.ParentID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerShareHolderModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerShareHolderModel> CustomerShareHoldDetailHistory_ListAll(CustomerShareHolderModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerShareHoldDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldHistoryID", SqlDbType.Int, _objModel.CustomerShareHoldHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pParentID", SqlDbType.Int, _objModel.ParentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.VarChar, _objModel.CustType == null ? "" : _objModel.CustType);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, 0);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerShareHolderModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerShareHolderModel> CustomerShareHoldDetailHistory_ListAllBind(CustomerShareHolderModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerShareHoldDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldHistoryID", SqlDbType.Int, _objModel.CustomerShareHoldHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pParentID", SqlDbType.Int, _objModel.ParentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustType", SqlDbType.VarChar, _objModel.CustType == null ? "" : _objModel.CustType);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, ProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerShareHolderModel>(dataReader as DbDataReader).ToList();
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

        #region Customer Bank Acc Detail
        public int CustomerBankAccDetail_Add(CustomerBankAccDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountName", SqlDbType.VarChar, _objModel.AccountName == null ? "" : _objModel.AccountName);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountNo", SqlDbType.VarChar, _objModel.AccountNo == null ? "" : _objModel.AccountNo);
            ClsAppDatabase.AddInParameter(cmd, "@pBankCountryID", SqlDbType.Int, _objModel.BankCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pIBAN", SqlDbType.VarChar, _objModel.IBAN == null ? "" : _objModel.IBAN);
            ClsAppDatabase.AddInParameter(cmd, "@pSwift", SqlDbType.VarChar, _objModel.Swift == null ? "" : _objModel.Swift);
            ClsAppDatabase.AddInParameter(cmd, "@pIFSC", SqlDbType.VarChar, _objModel.IFSC == null ? "" : _objModel.IFSC);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerBankAccID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerBankAccDetail_Update(CustomerBankAccDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountName", SqlDbType.VarChar, _objModel.AccountName == null ? "" : _objModel.AccountName);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountNo", SqlDbType.VarChar, _objModel.AccountNo == null ? "" : _objModel.AccountNo);
            ClsAppDatabase.AddInParameter(cmd, "@pBankCountryID", SqlDbType.Int, _objModel.BankCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pIBAN", SqlDbType.VarChar, _objModel.IBAN == null ? "" : _objModel.IBAN);
            ClsAppDatabase.AddInParameter(cmd, "@pSwift", SqlDbType.VarChar, _objModel.Swift == null ? "" : _objModel.Swift);
            ClsAppDatabase.AddInParameter(cmd, "@pIFSC", SqlDbType.VarChar, _objModel.IFSC == null ? "" : _objModel.IFSC);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerBankAccDetail_Delete(CustomerBankAccDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerBankAccDetail_UpdateNotRequired(CustomerBankAccDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetail_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerBankAccDetailHistory_Add(CustomerBankAccDetail _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerBankAccHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountName", SqlDbType.VarChar, _objModel.AccountName == null ? "" : _objModel.AccountName);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountNo", SqlDbType.VarChar, _objModel.AccountNo == null ? "" : _objModel.AccountNo);
            ClsAppDatabase.AddInParameter(cmd, "@pBankCountryID", SqlDbType.Int, _objModel.BankCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pIBAN", SqlDbType.VarChar, _objModel.IBAN == null ? "" : _objModel.IBAN);
            ClsAppDatabase.AddInParameter(cmd, "@pSwift", SqlDbType.VarChar, _objModel.Swift == null ? "" : _objModel.Swift);
            ClsAppDatabase.AddInParameter(cmd, "@pIFSC", SqlDbType.VarChar, _objModel.IFSC == null ? "" : _objModel.IFSC);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerBankAccHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerBankAccDetail> CustomerBankAccDetail_ListAll(CustomerBankAccDetail _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerBankAccDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID == 0 ? -1 : _objModel.CustomerSuppID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsCustomerSupp", SqlDbType.Bit, _objModel.IsCustomerSupp);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerBankAccDetail>(dataReader as DbDataReader).ToList();
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
        public List<CustomerBankAccDetail> CustomerBankAccDetailHistory_ListAll(CustomerBankAccDetail _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerBankAccDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerBankAccHistoryID", SqlDbType.Int, _objModel.CustomerBankAccHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID == 0 ? -1 : _objModel.CustomerSuppID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsCustomerSupp", SqlDbType.Bit, _objModel.IsCustomerSupp);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, ProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerBankAccDetail>(dataReader as DbDataReader).ToList();
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
        public List<CustomerBankAccDetail> CustomerBankAccDetailHistory_ListAllBind(CustomerBankAccDetail _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerBankAccDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerBankAccHistoryID", SqlDbType.Int, _objModel.CustomerBankAccHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID == 0 ? -1 : _objModel.CustomerSuppID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsCustomerSupp", SqlDbType.Bit, _objModel.IsCustomerSupp);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, ProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerBankAccDetail>(dataReader as DbDataReader).ToList();
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
        public int CustomerBankCreditDetail_Add(CustomerInfoModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankCreditDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerCreditID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pFacType", SqlDbType.VarChar, _objModel.FacType == null ? "" : _objModel.FacType);
            ClsAppDatabase.AddInParameter(cmd, "@pFacLimit", SqlDbType.Decimal, _objModel.FacLimit);
            ClsAppDatabase.AddInParameter(cmd, "@pFacUtilize", SqlDbType.Decimal, _objModel.FacUtilize);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerCreditID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerBankCreditDetail_Update(CustomerInfoModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankCreditDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerCreditID", SqlDbType.Int, _objModel.CustomerCreditID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pFacType", SqlDbType.VarChar, _objModel.FacType == null ? "" : _objModel.FacType);
            ClsAppDatabase.AddInParameter(cmd, "@pFacLimit", SqlDbType.Decimal, _objModel.FacLimit);
            ClsAppDatabase.AddInParameter(cmd, "@pFacUtilize", SqlDbType.Decimal, _objModel.FacUtilize);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerBankCreditDetail_Delete(CustomerInfoModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankCreditDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerCreditID", SqlDbType.Int, _objModel.CustomerCreditID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Int, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerBankCreditDetailHistory_Add(CustomerInfoModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankCreditDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerCreditHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerCreditID", SqlDbType.Int, _objModel.CustomerCreditID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pFacType", SqlDbType.VarChar, _objModel.FacType == null ? "" : _objModel.FacType);
            ClsAppDatabase.AddInParameter(cmd, "@pFacLimit", SqlDbType.Decimal, _objModel.FacLimit);
            ClsAppDatabase.AddInParameter(cmd, "@pFacUtilize", SqlDbType.Decimal, _objModel.FacUtilize);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerCreditHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerInfoModel> CustomerBankCreditDetail_ListAll(CustomerInfoModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerBankCreditDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerCreditID", SqlDbType.Int, _objModel.CustomerCreditID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerInfoModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerInfoModel> CustomerBankCreditDetailHistory_ListAll(CustomerInfoModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerBankCreditDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerCreditHistoryID", SqlDbType.Int, _objModel.CustomerCreditHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerCreditID", SqlDbType.Int, _objModel.CustomerCreditID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerInfoModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerInfoModel> CustomerBankCreditDetailHistory_ListAllBind(CustomerInfoModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerBankCreditDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerCreditHistoryID", SqlDbType.Int, _objModel.CustomerCreditHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerCreditID", SqlDbType.Int, _objModel.CustomerCreditID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerInfoModel>(dataReader as DbDataReader).ToList();
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

        #region Customer Supplier Detail
        public int CustomerSupplierDetail_Add(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerSupplierDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerSuppID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactFName", SqlDbType.VarChar, _objModel.ContactFName == null ? "" : _objModel.ContactFName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactMName", SqlDbType.VarChar, _objModel.ContactMName == null ? "" : _objModel.ContactMName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactLName", SqlDbType.VarChar, _objModel.ContactLName == null ? "" : _objModel.ContactLName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIncCountryID", SqlDbType.Int, _objModel.IncCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIncDate", SqlDbType.DateTime, _objModel.IncDate);
            ClsAppDatabase.AddInParameter(cmd, "@pLicExpDate", SqlDbType.DateTime, _objModel.LicExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, _objModel.IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherIndustry", SqlDbType.VarChar, _objModel.OtherIndustry == null ? "" : _objModel.OtherIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pSubIndustryID", SqlDbType.Int, _objModel.SubIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSubIndustry", SqlDbType.VarChar, _objModel.OtherSubIndustry == null ? "" : _objModel.OtherSubIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pInvTerm", SqlDbType.VarChar, _objModel.InvTerm == null ? "" : _objModel.InvTerm);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherInvTerm", SqlDbType.VarChar, _objModel.OtherInvTerm == null ? "" : _objModel.OtherInvTerm);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvAmt", SqlDbType.Decimal, _objModel.InvAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerSuppID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerSupplierDetail_Update(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerSupplierDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactFName", SqlDbType.VarChar, _objModel.ContactFName == null ? "" : _objModel.ContactFName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactMName", SqlDbType.VarChar, _objModel.ContactMName == null ? "" : _objModel.ContactMName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactLName", SqlDbType.VarChar, _objModel.ContactLName == null ? "" : _objModel.ContactLName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIncCountryID", SqlDbType.Int, _objModel.IncCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIncDate", SqlDbType.DateTime, _objModel.IncDate);
            ClsAppDatabase.AddInParameter(cmd, "@pLicExpDate", SqlDbType.DateTime, _objModel.LicExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, _objModel.IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherIndustry", SqlDbType.VarChar, _objModel.OtherIndustry == null ? "" : _objModel.OtherIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pSubIndustryID", SqlDbType.Int, _objModel.SubIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSubIndustry", SqlDbType.VarChar, _objModel.OtherSubIndustry == null ? "" : _objModel.OtherSubIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pInvTerm", SqlDbType.VarChar, _objModel.InvTerm == null ? "" : _objModel.InvTerm);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherInvTerm", SqlDbType.VarChar, _objModel.OtherInvTerm == null ? "" : _objModel.OtherInvTerm);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvAmt", SqlDbType.Decimal, _objModel.InvAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerSupplierDetail_Delete(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerSupplierDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerSupplierDetail_UpdateNotRequired(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerSupplierDetail_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerSupplierDetailHistory_Add(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerSupplierDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerSuppHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactFName", SqlDbType.VarChar, _objModel.ContactFName == null ? "" : _objModel.ContactFName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactMName", SqlDbType.VarChar, _objModel.ContactMName == null ? "" : _objModel.ContactMName);
            ClsAppDatabase.AddInParameter(cmd, "@pContactLName", SqlDbType.VarChar, _objModel.ContactLName == null ? "" : _objModel.ContactLName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
            ClsAppDatabase.AddInParameter(cmd, "@pTradeLicNo", SqlDbType.VarChar, _objModel.TradeLicNo == null ? "" : _objModel.TradeLicNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIncCountryID", SqlDbType.Int, _objModel.IncCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pIncDate", SqlDbType.DateTime, _objModel.IncDate);
            ClsAppDatabase.AddInParameter(cmd, "@pLicExpDate", SqlDbType.DateTime, _objModel.LicExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIndustryID", SqlDbType.Int, _objModel.IndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherIndustry", SqlDbType.VarChar, _objModel.OtherIndustry == null ? "" : _objModel.OtherIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pSubIndustryID", SqlDbType.Int, _objModel.SubIndustryID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherSubIndustry", SqlDbType.VarChar, _objModel.OtherSubIndustry == null ? "" : _objModel.OtherSubIndustry);
            ClsAppDatabase.AddInParameter(cmd, "@pInvTerm", SqlDbType.VarChar, _objModel.InvTerm == null ? "" : _objModel.InvTerm);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherInvTerm", SqlDbType.VarChar, _objModel.OtherInvTerm == null ? "" : _objModel.OtherInvTerm);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pInvAmt", SqlDbType.Decimal, _objModel.InvAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.Address == null ? "" : _objModel.Address);
            ClsAppDatabase.AddInParameter(cmd, "@pPOBox", SqlDbType.VarChar, _objModel.POBox == null ? "" : _objModel.POBox);
            ClsAppDatabase.AddInParameter(cmd, "@pWebsite", SqlDbType.VarChar, _objModel.Website == null ? "" : _objModel.Website);
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNo == null ? "" : _objModel.TelNo);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerSuppHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerSupplierModel> CustomerSupplierDetailHistory_ListAllBind(CustomerSupplierModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerSupplierDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppHistoryID", SqlDbType.Int, _objModel.CustomerSuppHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerSupplierModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerSupplierModel> CustomerSupplierDetailHistory_ListAll(CustomerSupplierModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerSupplierDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppHistoryID", SqlDbType.Int, _objModel.CustomerSuppHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerSupplierModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerSupplierModel> CustomerSupplierDetail_ListAll(CustomerSupplierModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerSupplierDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerSupplierModel>(dataReader as DbDataReader).ToList();
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
        public DataTable CustomerSupplierDetail_ListAllTable(CustomerSupplierModel _objModel)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerSupplierDetail_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);            
            ad.Fill(dt);
            cmd.Connection.Close();
            cmd.Dispose();
            return dt;
        }

        #endregion

        #region Exporter Bank Acc Detail
        public int ExpoCustomerBankAccDetail_Add(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.BankCurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountName", SqlDbType.VarChar, _objModel.AccountName == null ? "" : _objModel.AccountName);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountNo", SqlDbType.VarChar, _objModel.AccountNo == null ? "" : _objModel.AccountNo);
            ClsAppDatabase.AddInParameter(cmd, "@pBankCountryID", SqlDbType.Int, _objModel.BankCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.BankAddress == null ? "" : _objModel.BankAddress);
            ClsAppDatabase.AddInParameter(cmd, "@pIBAN", SqlDbType.VarChar, _objModel.IBAN == null ? "" : _objModel.IBAN);
            ClsAppDatabase.AddInParameter(cmd, "@pSwift", SqlDbType.VarChar, _objModel.Swift == null ? "" : _objModel.Swift);
            ClsAppDatabase.AddInParameter(cmd, "@pIFSC", SqlDbType.VarChar, _objModel.IFSC == null ? "" : _objModel.IFSC);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerBankAccID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ExpoCustomerBankAccDetail_Update(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.BankCurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountName", SqlDbType.VarChar, _objModel.AccountName == null ? "" : _objModel.AccountName);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountNo", SqlDbType.VarChar, _objModel.AccountNo == null ? "" : _objModel.AccountNo);
            ClsAppDatabase.AddInParameter(cmd, "@pBankCountryID", SqlDbType.Int, _objModel.BankCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.BankAddress == null ? "" : _objModel.BankAddress);
            ClsAppDatabase.AddInParameter(cmd, "@pIBAN", SqlDbType.VarChar, _objModel.IBAN == null ? "" : _objModel.IBAN);
            ClsAppDatabase.AddInParameter(cmd, "@pSwift", SqlDbType.VarChar, _objModel.Swift == null ? "" : _objModel.Swift);
            ClsAppDatabase.AddInParameter(cmd, "@pIFSC", SqlDbType.VarChar, _objModel.IFSC == null ? "" : _objModel.IFSC);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ExpoCustomerBankAccDetail_Delete(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNoRequired", SqlDbType.Bit, _objModel.IsNoRequired);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ExpoCustomerBankAccDetail_UpdateNotRequired(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetail_UpdateNotRequired");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int ExpoCustomerBankAccDetailHistory_Add(CustomerSupplierModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerBankAccDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerBankAccHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.BankCurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pBankID", SqlDbType.Int, _objModel.BankID);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountName", SqlDbType.VarChar, _objModel.AccountName == null ? "" : _objModel.AccountName);
            ClsAppDatabase.AddInParameter(cmd, "@pAccountNo", SqlDbType.VarChar, _objModel.AccountNo == null ? "" : _objModel.AccountNo);
            ClsAppDatabase.AddInParameter(cmd, "@pBankCountryID", SqlDbType.Int, _objModel.BankCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pAddress", SqlDbType.VarChar, _objModel.BankAddress == null ? "" : _objModel.BankAddress);
            ClsAppDatabase.AddInParameter(cmd, "@pIBAN", SqlDbType.VarChar, _objModel.IBAN == null ? "" : _objModel.IBAN);
            ClsAppDatabase.AddInParameter(cmd, "@pSwift", SqlDbType.VarChar, _objModel.Swift == null ? "" : _objModel.Swift);
            ClsAppDatabase.AddInParameter(cmd, "@pIFSC", SqlDbType.VarChar, _objModel.IFSC == null ? "" : _objModel.IFSC);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerBankAccHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        public List<CustomerSupplierModel> ExpoCustomerBankAccDetailHistory_ListAllBind(CustomerSupplierModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerBankAccDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerBankAccHistoryID", SqlDbType.Int, _objModel.CustomerBankAccHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerBankAccID", SqlDbType.Int, _objModel.CustomerBankAccID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID == 0 ? -1 : _objModel.CustomerSuppID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsCustomerSupp", SqlDbType.Bit, _objModel.IsCustomerSupp);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, ProcessBy);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerSupplierModel>(dataReader as DbDataReader).ToList();
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

        #region Others
        public List<CustomerTempModel> ServerDateTime()
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("ServerDateTime");
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerTempModel>(dataReader as DbDataReader).ToList();
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
        public int UserMaster_UpdateAgree(mCustomerMasterModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_UpdateAgree");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, FN.UpdateByID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.UpdateByID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerMaster_ListAll_Result> UserMaster_CustomerBindDetails(Nullable<int> pCustomerID, Nullable<int> pCustomerTypeID, string pStatus)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMaster_CustomerBindDetails");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, pCustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, pCustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public int UserSignDetail_Add(UserSignDetailModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserSignDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pUserSignID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pType", SqlDbType.Char, _objModel.Type == null ? "" : _objModel.Type.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pReferenceID", SqlDbType.Int, _objModel.ReferenceID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, _objModel.HtmlTemplateID);
            ClsAppDatabase.AddInParameter(cmd, "@pFileType", SqlDbType.VarChar, "PDF");
            ClsAppDatabase.AddInParameter(cmd, "@pRefNo", SqlDbType.VarChar, _objModel.RefNo == null ? "" : _objModel.RefNo.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pAttachName", SqlDbType.VarChar, _objModel.AttachName == null ? "" : _objModel.AttachName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pOrderNo", SqlDbType.Int, _objModel.OrderNo);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pUserSignID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int UserSignDetail_Update(UserSignDetailModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserSignDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pUserSignID", SqlDbType.Int, _objModel.UserSignID);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsAppDatabase.AddInParameter(cmd, "@pFeedback", SqlDbType.VarChar, _objModel.Feedback == null ? "" : _objModel.Feedback.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<UserSignDetailModel> UserSignDetail_UpdateDetails(UserSignDetailModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserSignDetail_UpdateDetails");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserSignID", SqlDbType.Int, _objModel.UserSignID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pRefNo", SqlDbType.VarChar, _objModel.RefNo == null ? "" : _objModel.RefNo.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pAttachName", SqlDbType.VarChar, _objModel.AttachName == null ? "" : _objModel.AttachName.Trim());
            ClsEntityAppDatabase.AddInParameter(cmd, "@pOrderNo", SqlDbType.Int, _objModel.OrderNo);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<UserSignDetailModel>(dataReader as DbDataReader).ToList();
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
        public int UserSignDetail_UpdateResponse(UserSignDetailModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserSignDetail_UpdateResponse");
            ClsAppDatabase.AddInParameter(cmd, "@pUserSignID", SqlDbType.Int, _objModel.UserSignID);
            ClsAppDatabase.AddInParameter(cmd, "@pRespTranNo", SqlDbType.VarChar, _objModel.RespTranNo == null ? "" : _objModel.RespTranNo.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRespStatus", SqlDbType.VarChar, _objModel.RespStatus == null ? "" : _objModel.RespStatus.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRespMessage", SqlDbType.VarChar, _objModel.RespMessage == null ? "" : _objModel.RespMessage.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRespValue", SqlDbType.NVarChar, _objModel.RespValue == null ? "" : _objModel.RespValue.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<UserSignDetailModel> UserSignDetail_ListAll(UserSignDetailModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserSignDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserSignID", SqlDbType.Int, _objModel.UserSignID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, _objModel.HtmlTemplateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pReferenceID", SqlDbType.Int, _objModel.ReferenceID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status.Trim());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<UserSignDetailModel>(dataReader as DbDataReader).ToList();
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
        public List<UserSignDetailModel> UserSignDetail_VerifySendLOA(UserSignDetailModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserSignDetail_VerifySendLOA");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<UserSignDetailModel>(dataReader as DbDataReader).ToList();
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
        public List<UserSignDetailModel> UserSignDetail_AcceptLOA(UserSignDetailModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserSignDetail_AcceptLOA");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerSuppID", SqlDbType.Int, _objModel.CustomerSuppID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<UserSignDetailModel>(dataReader as DbDataReader).ToList();
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

        public List<UserSignDetailModel> UserSignDetail_MenuPanelHide(UserSignDetailModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserSignDetail_MenuPanelHide");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<UserSignDetailModel>(dataReader as DbDataReader).ToList();
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
        public List<UserSignDetailModel> SignLocMaster_ListAll(UserSignDetailModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("SignLocMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pHtmlTemplateID", SqlDbType.Int, _objModel.HtmlTemplateID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pOrderNo", SqlDbType.Int, _objModel.OrderNo);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<UserSignDetailModel>(dataReader as DbDataReader).ToList();
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

        public List<CustomerMaster_ListAll_Result> CustomerTypeMaster_ListAll(Nullable<int> pCustomerTypeID, string pCustomerTypeName, string pCustomerTypeCode, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
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
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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
        public List<CustomerMasterModel> CustomerVerifyDetail_GenerateProgram(CustomerMasterModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerVerifyDetail_GenerateProgram");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMasterModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerMasterModel> CustomerMasterHistory_Remarks(CustomerMasterModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMasterHistory_Remarks");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMasterModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerMasterModel> UserRole_GetDetail(CustomerMasterModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserRole_GetDetail");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerMasterModel>(dataReader as DbDataReader).ToList();
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

        public List<ScreeningConfig> CustomerRiskDetail_ListAll(ScreeningConfig _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerRiskDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCategoryDetID", SqlDbType.Int, _objModel.CategoryDetID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ScreeningConfig>(dataReader as DbDataReader).ToList();
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
        public List<ScreeningConfig> CustomerRiskDetail_GetCountryRisk(ScreeningConfig _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerRiskDetail_GetCountryRisk");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ScreeningConfig>(dataReader as DbDataReader).ToList();
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

        public int CustomerRiskDetail_Add(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerRiskDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerRiskID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryDetID", SqlDbType.Int, _objModel.CategoryDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pSelType", SqlDbType.Char, _objModel.SelType == null ? "" : _objModel.SelType.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSelTypeYN", SqlDbType.Char, _objModel.SelTypeYN == null ? "" : _objModel.SelTypeYN.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerRiskID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerRiskDetail_Update(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerRiskDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerRiskID", SqlDbType.Int, _objModel.CustomerRiskID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryDetID", SqlDbType.Int, _objModel.CategoryDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pSelType", SqlDbType.Char, _objModel.SelType == null ? "" : _objModel.SelType.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSelTypeYN", SqlDbType.Char, _objModel.SelTypeYN == null ? "" : _objModel.SelTypeYN.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerRiskDetail_AddCountryRisk(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerRiskDetail_AddCountryRisk");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerRiskID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryDetID", SqlDbType.Int, _objModel.CategoryDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pDueDiligence", SqlDbType.VarChar, _objModel.DueDiligence == null ? "" : _objModel.DueDiligence.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSanction", SqlDbType.VarChar, _objModel.Sanction == null ? "" : _objModel.Sanction.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCategory", SqlDbType.VarChar, _objModel.Category == null ? "" : _objModel.Category.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRemark", SqlDbType.VarChar, _objModel.Remark == null ? "" : _objModel.Remark.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerRiskDetail_UpdateCountryRisk(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerRiskDetail_UpdateCountryRisk");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerRiskID", SqlDbType.Int, _objModel.CustomerRiskID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryDetID", SqlDbType.Int, _objModel.CategoryDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pDueDiligence", SqlDbType.VarChar, _objModel.DueDiligence == null ? "" : _objModel.DueDiligence.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSanction", SqlDbType.VarChar, _objModel.Sanction == null ? "" : _objModel.Sanction.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCategory", SqlDbType.VarChar, _objModel.Category == null ? "" : _objModel.Category.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRemark", SqlDbType.VarChar, _objModel.Remark == null ? "" : _objModel.Remark.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        public List<ScreeningConfig> CustomerRiskDetail_Result(ScreeningConfig _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerRiskDetail_Result");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ScreeningConfig>(dataReader as DbDataReader).ToList();
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
        public List<ScreeningConfig> CustomerRiskDetail_Result1(ScreeningConfig _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerRiskDetail_Result1");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ScreeningConfig>(dataReader as DbDataReader).ToList();
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

        #region Customer Crdeit Review
        public int CustomerReviewDetail_Add(CustomerReviewModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerReviewDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerRevID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pLimit", SqlDbType.Decimal, _objModel.Limit);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pTenor", SqlDbType.Int, _objModel.Tenor);
            ClsAppDatabase.AddInParameter(cmd, "@pProfitRate", SqlDbType.Decimal, _objModel.ProfitRate);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessFee", SqlDbType.Decimal, _objModel.ProcessFee);
            ClsAppDatabase.AddInParameter(cmd, "@pSecDeptPer", SqlDbType.Decimal, _objModel.SecDeptPer);
            ClsAppDatabase.AddInParameter(cmd, "@pSecDeptAmt", SqlDbType.Decimal, _objModel.SecDeptAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pBrokerFee", SqlDbType.Decimal, _objModel.BrokerFee);
            ClsAppDatabase.AddInParameter(cmd, "@pArrangFeePer", SqlDbType.Decimal, _objModel.ArrangFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pArrangFeeAmt", SqlDbType.Decimal, _objModel.ArrangFeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherFee", SqlDbType.Decimal, _objModel.OtherFee);
            ClsAppDatabase.AddInParameter(cmd, "@pPenaltyRate", SqlDbType.Decimal, _objModel.PenaltyRate);
            ClsAppDatabase.AddInParameter(cmd, "@pBaseRate", SqlDbType.Decimal, _objModel.BaseRate);
            ClsAppDatabase.AddInParameter(cmd, "@pProfMarginRate", SqlDbType.Decimal, _objModel.ProfMarginRate);
            ClsAppDatabase.AddInParameter(cmd, "@pDiscRate", SqlDbType.Decimal, _objModel.DiscRate);
            ClsAppDatabase.AddInParameter(cmd, "@pHoldbackPer", SqlDbType.Decimal, _objModel.HoldbackPer);
            ClsAppDatabase.AddInParameter(cmd, "@pTranFeePer", SqlDbType.Decimal, _objModel.TranFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pFeedback", SqlDbType.VarChar, _objModel.Feedback == null ? "" : _objModel.Feedback);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdDiligRpt", SqlDbType.VarChar, _objModel.UpdDiligRpt == null ? "" : _objModel.UpdDiligRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdCredRpt", SqlDbType.VarChar, _objModel.UpdCredRpt == null ? "" : _objModel.UpdCredRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerRevID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerReviewDetail_Update(CustomerReviewModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerReviewDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerRevHistoryID", SqlDbType.Int, _objModel.CustomerRevHistoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerRevID", SqlDbType.Int, _objModel.CustomerRevID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pLimit", SqlDbType.Decimal, _objModel.Limit);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pTenor", SqlDbType.Int, _objModel.Tenor);
            ClsAppDatabase.AddInParameter(cmd, "@pProfitRate", SqlDbType.Decimal, _objModel.ProfitRate);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessFee", SqlDbType.Decimal, _objModel.ProcessFee);
            ClsAppDatabase.AddInParameter(cmd, "@pSecDeptPer", SqlDbType.Decimal, _objModel.SecDeptPer);
            ClsAppDatabase.AddInParameter(cmd, "@pSecDeptAmt", SqlDbType.Decimal, _objModel.SecDeptAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pBrokerFee", SqlDbType.Decimal, _objModel.BrokerFee);
            ClsAppDatabase.AddInParameter(cmd, "@pArrangFeePer", SqlDbType.Decimal, _objModel.ArrangFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pArrangFeeAmt", SqlDbType.Decimal, _objModel.ArrangFeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherFee", SqlDbType.Decimal, _objModel.OtherFee);
            ClsAppDatabase.AddInParameter(cmd, "@pPenaltyRate", SqlDbType.Decimal, _objModel.PenaltyRate);
            ClsAppDatabase.AddInParameter(cmd, "@pBaseRate", SqlDbType.Decimal, _objModel.BaseRate);
            ClsAppDatabase.AddInParameter(cmd, "@pProfMarginRate", SqlDbType.Decimal, _objModel.ProfMarginRate);
            ClsAppDatabase.AddInParameter(cmd, "@pDiscRate", SqlDbType.Decimal, _objModel.DiscRate);
            ClsAppDatabase.AddInParameter(cmd, "@pHoldbackPer", SqlDbType.Decimal, _objModel.HoldbackPer);
            ClsAppDatabase.AddInParameter(cmd, "@pTranFeePer", SqlDbType.Decimal, _objModel.TranFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pFeedback", SqlDbType.VarChar, _objModel.Feedback == null ? "" : _objModel.Feedback);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdDiligRpt", SqlDbType.VarChar, _objModel.UpdDiligRpt == null ? "" : _objModel.UpdDiligRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdCredRpt", SqlDbType.VarChar, _objModel.UpdCredRpt == null ? "" : _objModel.UpdCredRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerReviewDetailHistory_Add(CustomerReviewModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerReviewDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerRevHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerRevID", SqlDbType.Int, _objModel.CustomerRevID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pLimit", SqlDbType.Decimal, _objModel.Limit);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType);
            ClsAppDatabase.AddInParameter(cmd, "@pTenor", SqlDbType.Int, _objModel.Tenor);
            ClsAppDatabase.AddInParameter(cmd, "@pProfitRate", SqlDbType.Decimal, _objModel.ProfitRate);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessFee", SqlDbType.Decimal, _objModel.ProcessFee);
            ClsAppDatabase.AddInParameter(cmd, "@pSecDeptPer", SqlDbType.Decimal, _objModel.SecDeptPer);
            ClsAppDatabase.AddInParameter(cmd, "@pSecDeptAmt", SqlDbType.Decimal, _objModel.SecDeptAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pBrokerFee", SqlDbType.Decimal, _objModel.BrokerFee);
            ClsAppDatabase.AddInParameter(cmd, "@pArrangFeePer", SqlDbType.Decimal, _objModel.ArrangFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pArrangFeeAmt", SqlDbType.Decimal, _objModel.ArrangFeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherFee", SqlDbType.Decimal, _objModel.OtherFee);
            ClsAppDatabase.AddInParameter(cmd, "@pPenaltyRate", SqlDbType.Decimal, _objModel.PenaltyRate);
            ClsAppDatabase.AddInParameter(cmd, "@pBaseRate", SqlDbType.Decimal, _objModel.BaseRate);
            ClsAppDatabase.AddInParameter(cmd, "@pProfMarginRate", SqlDbType.Decimal, _objModel.ProfMarginRate);
            ClsAppDatabase.AddInParameter(cmd, "@pDiscRate", SqlDbType.Decimal, _objModel.DiscRate);
            ClsAppDatabase.AddInParameter(cmd, "@pHoldbackPer", SqlDbType.Decimal, _objModel.HoldbackPer);
            ClsAppDatabase.AddInParameter(cmd, "@pTranFeePer", SqlDbType.Decimal, _objModel.TranFeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pFeedback", SqlDbType.VarChar, _objModel.Feedback == null ? "" : _objModel.Feedback);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdDiligRpt", SqlDbType.VarChar, _objModel.UpdDiligRpt == null ? "" : _objModel.UpdDiligRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdCredRpt", SqlDbType.VarChar, _objModel.UpdCredRpt == null ? "" : _objModel.UpdCredRpt);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerRevHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerReviewModel> CustomerReviewDetail_ListAll(CustomerReviewModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerReviewDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerRevID", SqlDbType.Int, _objModel.CustomerRevID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerReviewModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerReviewModel> CustomerReviewDetailHistory_ListAll(CustomerReviewModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerReviewDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerRevHistoryID", SqlDbType.Int, _objModel.CustomerRevHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerRevID", SqlDbType.Int, _objModel.CustomerRevID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerReviewModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerReviewModel> CustomerReviewDetailHistory_ListAllBind(CustomerReviewModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerReviewDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerRevHistoryID", SqlDbType.Int, _objModel.CustomerRevHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerRevID", SqlDbType.Int, _objModel.CustomerRevID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pProgramType", SqlDbType.Char, _objModel.ProgramType == null ? "" : _objModel.ProgramType);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerReviewModel>(dataReader as DbDataReader).ToList();
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

        #region Customer Verify
        public int CustomerVerifyDetail_Add(CustomerVerifyModel _objModel)
        {
            if (_objModel.CompleteDate == null) { _objModel.CompleteDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.CompleteDate == Convert.ToDateTime("1/1/0001")) { _objModel.CompleteDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IssueDate == null) { _objModel.IssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IssueDate == Convert.ToDateTime("1/1/0001")) { _objModel.IssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExpDate == null) { _objModel.ExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.ExpDate = Convert.ToDateTime("01/01/1900"); }

            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerVerifyDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerVerID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsComplete", SqlDbType.Bit, _objModel.IsComplete);
            ClsAppDatabase.AddInParameter(cmd, "@pCompleteBy", SqlDbType.VarChar, _objModel.CompleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCompleteDate", SqlDbType.DateTime, _objModel.CompleteDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIsVerified", SqlDbType.Bit, _objModel.IsVerified);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentIDs", SqlDbType.VarChar, _objModel.DocumentIDs == null ? "" : _objModel.DocumentIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDoc", SqlDbType.VarChar, _objModel.OtherDoc == null ? "" : _objModel.OtherDoc);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldIDs", SqlDbType.VarChar, _objModel.CustomerShareHoldIDs == null ? "" : _objModel.CustomerShareHoldIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramCode", SqlDbType.VarChar, _objModel.ProgramCode == null ? "" : _objModel.ProgramCode);
            ClsAppDatabase.AddInParameter(cmd, "@pAuthSignType", SqlDbType.Char, _objModel.AuthSignType == null ? "" : _objModel.AuthSignType);
            ClsAppDatabase.AddInParameter(cmd, "@pProfitType", SqlDbType.Char, _objModel.ProfitType == null ? "" : _objModel.ProfitType);
            ClsAppDatabase.AddInParameter(cmd, "@pExtTenor", SqlDbType.Int, _objModel.ExtTenor);
            ClsAppDatabase.AddInParameter(cmd, "@pExtDateType", SqlDbType.Char, _objModel.ExtDateType == null ? "" : _objModel.ExtDateType);
            ClsAppDatabase.AddInParameter(cmd, "@pIsInsurance", SqlDbType.Bit, _objModel.IsInsurance);
            ClsAppDatabase.AddInParameter(cmd, "@pPolicyNo", SqlDbType.VarChar, _objModel.PolicyNo == null ? "" : _objModel.PolicyNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIssueDate", SqlDbType.DateTime, _objModel.IssueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pExpDate", SqlDbType.DateTime, _objModel.ExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Feedback == null ? "" : _objModel.Feedback);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerVerID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerVerifyDetail_Update(CustomerVerifyModel _objModel)
        {
            if (_objModel.CompleteDate == null) { _objModel.CompleteDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.CompleteDate == Convert.ToDateTime("1/1/0001")) { _objModel.CompleteDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IssueDate == null) { _objModel.IssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IssueDate == Convert.ToDateTime("1/1/0001")) { _objModel.IssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExpDate == null) { _objModel.ExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.ExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerVerifyDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerVerID", SqlDbType.Int, _objModel.CustomerVerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsComplete", SqlDbType.Bit, _objModel.IsComplete);
            ClsAppDatabase.AddInParameter(cmd, "@pCompleteBy", SqlDbType.VarChar, _objModel.CompleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCompleteDate", SqlDbType.DateTime, _objModel.CompleteDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIsVerified", SqlDbType.Bit, _objModel.IsVerified);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentIDs", SqlDbType.VarChar, _objModel.DocumentIDs == null ? "" : _objModel.DocumentIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDoc", SqlDbType.VarChar, _objModel.OtherDoc == null ? "" : _objModel.OtherDoc);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldIDs", SqlDbType.VarChar, _objModel.CustomerShareHoldIDs == null ? "" : _objModel.CustomerShareHoldIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramCode", SqlDbType.VarChar, _objModel.ProgramCode == null ? "" : _objModel.ProgramCode);
            ClsAppDatabase.AddInParameter(cmd, "@pAuthSignType", SqlDbType.Char, _objModel.AuthSignType == null ? "" : _objModel.AuthSignType);
            ClsAppDatabase.AddInParameter(cmd, "@pProfitType", SqlDbType.Char, _objModel.ProfitType == null ? "" : _objModel.ProfitType);
            ClsAppDatabase.AddInParameter(cmd, "@pExtTenor", SqlDbType.Int, _objModel.ExtTenor);
            ClsAppDatabase.AddInParameter(cmd, "@pExtDateType", SqlDbType.Char, _objModel.ExtDateType == null ? "" : _objModel.ExtDateType);
            ClsAppDatabase.AddInParameter(cmd, "@pIsInsurance", SqlDbType.Bit, _objModel.IsInsurance);
            ClsAppDatabase.AddInParameter(cmd, "@pPolicyNo", SqlDbType.VarChar, _objModel.PolicyNo == null ? "" : _objModel.PolicyNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIssueDate", SqlDbType.DateTime, _objModel.IssueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pExpDate", SqlDbType.DateTime, _objModel.ExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerVerifyDetailHistory_Add(CustomerVerifyModel _objModel)
        {
            if (_objModel.CompleteDate == null) { _objModel.CompleteDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.CompleteDate == Convert.ToDateTime("1/1/0001")) { _objModel.CompleteDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IssueDate == null) { _objModel.IssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.IssueDate == Convert.ToDateTime("1/1/0001")) { _objModel.IssueDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExpDate == null) { _objModel.ExpDate = Convert.ToDateTime("01/01/1900"); }
            if (_objModel.ExpDate == Convert.ToDateTime("1/1/0001")) { _objModel.ExpDate = Convert.ToDateTime("01/01/1900"); }
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerVerifyDetailHistory_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerVerHistoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerVerID", SqlDbType.Int, _objModel.CustomerVerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsComplete", SqlDbType.Bit, _objModel.IsComplete);
            ClsAppDatabase.AddInParameter(cmd, "@pCompleteBy", SqlDbType.VarChar, _objModel.CompleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCompleteDate", SqlDbType.DateTime, _objModel.CompleteDate);
            ClsAppDatabase.AddInParameter(cmd, "@pIsVerified", SqlDbType.Bit, _objModel.IsVerified);
            ClsAppDatabase.AddInParameter(cmd, "@pDocumentIDs", SqlDbType.VarChar, _objModel.DocumentIDs == null ? "" : _objModel.DocumentIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pOtherDoc", SqlDbType.VarChar, _objModel.OtherDoc == null ? "" : _objModel.OtherDoc);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldIDs", SqlDbType.VarChar, _objModel.CustomerShareHoldIDs == null ? "" : _objModel.CustomerShareHoldIDs);
            ClsAppDatabase.AddInParameter(cmd, "@pProgramCode", SqlDbType.VarChar, _objModel.ProgramCode == null ? "" : _objModel.ProgramCode);
            ClsAppDatabase.AddInParameter(cmd, "@pAuthSignType", SqlDbType.Char, _objModel.AuthSignType == null ? "" : _objModel.AuthSignType);
            ClsAppDatabase.AddInParameter(cmd, "@pProfitType", SqlDbType.Char, _objModel.ProfitType == null ? "" : _objModel.ProfitType);
            ClsAppDatabase.AddInParameter(cmd, "@pExtTenor", SqlDbType.Int, _objModel.ExtTenor);
            ClsAppDatabase.AddInParameter(cmd, "@pExtDateType", SqlDbType.Char, _objModel.ExtDateType == null ? "" : _objModel.ExtDateType);
            ClsAppDatabase.AddInParameter(cmd, "@pIsInsurance", SqlDbType.Bit, _objModel.IsInsurance);
            ClsAppDatabase.AddInParameter(cmd, "@pPolicyNo", SqlDbType.VarChar, _objModel.PolicyNo == null ? "" : _objModel.PolicyNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIssueDate", SqlDbType.DateTime, _objModel.IssueDate);
            ClsAppDatabase.AddInParameter(cmd, "@pExpDate", SqlDbType.DateTime, _objModel.ExpDate);
            ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerVerHistoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerShareHoldDetail_UpdateAuthSignType(CustomerShareHolderModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerShareHoldDetail_UpdateAuthSignType");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSelectSign", SqlDbType.Bit, _objModel.IsSelectSign);
            ClsAppDatabase.AddInParameter(cmd, "@pAuthSignType", SqlDbType.Char, _objModel.AuthSignType == null ? "" : _objModel.AuthSignType);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        public List<CustomerVerifyModel> CustomerVerifyDetail_ListAll(CustomerVerifyModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerVerifyDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerVerID", SqlDbType.Int, _objModel.CustomerVerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerVerifyModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerVerifyModel> CustomerVerifyDetailHistory_ListAll(CustomerVerifyModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerVerifyDetailHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerVerHistoryID", SqlDbType.Int, _objModel.CustomerVerHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerVerID", SqlDbType.Int, _objModel.CustomerVerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerVerifyModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerVerifyModel> CustomerVerifyDetailHistory_ListAllBind(CustomerVerifyModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerVerifyDetailHistory_ListAllBind");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerVerHistoryID", SqlDbType.Int, _objModel.CustomerVerHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerVerID", SqlDbType.Int, _objModel.CustomerVerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerVerifyModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerVerifyModel> UserMaster_AuthorizedListAll(CustomerVerifyModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserMaster_AuthorizedListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerVerifyModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerVerifyModel> CustomerVerifyDetail_GenerateProgram(CustomerVerifyModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerVerifyDetail_GenerateProgram");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            //ClsEntityAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            //ClsEntityAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerVerifyModel>(dataReader as DbDataReader).ToList();
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

        public DataTable UserMaster_AuthorizedListAllTable(CustomerVerifyModel _objModel)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_AuthorizedListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            cmd.Connection.Close();
            cmd.Dispose();
            return dt;
        }
        public DataTable UserMaster_ListAllTable(UserMaster_ListAll_Result _objModel)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_ListAll");
            ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, _objModel.LoginName == null ? "" : _objModel.LoginName);
            ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
            ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, _objModel.IsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(dt);
            cmd.Connection.Close();
            cmd.Dispose();
            return dt;
        }


        public List<UserMaster_ListAll_Result> UserSingDetail_SendUserMail(UserMaster_ListAll_Result _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("UserSingDetail_SendUserMail");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<UserMaster_ListAll_Result>(dataReader as DbDataReader).ToList();
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

        #region Client Classification
        public int CustomerClassDetail_Add(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerClassDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCustomerClassId", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategory", SqlDbType.VarChar, _objModel.Category == null ? "" : _objModel.Category.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsRC", SqlDbType.Bit, _objModel.IsRC);
            ClsAppDatabase.AddInParameter(cmd, "@pIsPC", SqlDbType.Bit, _objModel.IsPC);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP", SqlDbType.Bit, _objModel.IsDP);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP1", SqlDbType.Bit, _objModel.IsDP1);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP2", SqlDbType.Bit, _objModel.IsDP2);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP3", SqlDbType.Bit, _objModel.IsDP3);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP4", SqlDbType.Bit, _objModel.IsDP4);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP5", SqlDbType.Bit, _objModel.IsDP5);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP6", SqlDbType.Bit, _objModel.IsDP6);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP7", SqlDbType.Bit, _objModel.IsDP7);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP71", SqlDbType.Bit, _objModel.IsDP71);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP72", SqlDbType.Bit, _objModel.IsDP72);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP73", SqlDbType.Bit, _objModel.IsDP73);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP8", SqlDbType.Bit, _objModel.IsDP8);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP9", SqlDbType.Bit, _objModel.IsDP9);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP10", SqlDbType.Bit, _objModel.IsDP10);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP11", SqlDbType.Bit, _objModel.IsDP11);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB", SqlDbType.Bit, _objModel.IsSB);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB1", SqlDbType.Bit, _objModel.IsSB1);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB11", SqlDbType.Bit, _objModel.IsSB11);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB12", SqlDbType.Bit, _objModel.IsSB12);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB2", SqlDbType.Bit, _objModel.IsSB2);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB21", SqlDbType.Bit, _objModel.IsSB21);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP", SqlDbType.Bit, _objModel.IsAP);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP1", SqlDbType.Bit, _objModel.IsAP1);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP11", SqlDbType.Bit, _objModel.IsAP11);
            ClsAppDatabase.AddInParameter(cmd, "@pAPIndName11", SqlDbType.VarChar, _objModel.APIndName11 == null ? "" : _objModel.APIndName11.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP12", SqlDbType.Bit, _objModel.IsAP12);
            ClsAppDatabase.AddInParameter(cmd, "@pAPIndName12", SqlDbType.VarChar, _objModel.APIndName12 == null ? "" : _objModel.APIndName12.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP2", SqlDbType.Bit, _objModel.IsAP2);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP21", SqlDbType.Bit, _objModel.IsAP21);
            ClsAppDatabase.AddInParameter(cmd, "@pAPIndName21", SqlDbType.VarChar, _objModel.APIndName21 == null ? "" : _objModel.APIndName21.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP22", SqlDbType.Bit, _objModel.IsAP22);
            ClsAppDatabase.AddInParameter(cmd, "@pAPIndName22", SqlDbType.VarChar, _objModel.APIndName22 == null ? "" : _objModel.APIndName22.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsMC", SqlDbType.Bit, _objModel.IsMC);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMC1", SqlDbType.Bit, _objModel.IsMC1);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMC2", SqlDbType.Bit, _objModel.IsMC2);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCustomerClassId"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerClassDetail_Update(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerClassDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerClassId", SqlDbType.Int, _objModel.CustomerClassId);
            ClsAppDatabase.AddInParameter(cmd, "@pCategory", SqlDbType.VarChar, _objModel.Category == null ? "" : _objModel.Category.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsRC", SqlDbType.Bit, _objModel.IsRC);
            ClsAppDatabase.AddInParameter(cmd, "@pIsPC", SqlDbType.Bit, _objModel.IsPC);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP", SqlDbType.Bit, _objModel.IsDP);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP1", SqlDbType.Bit, _objModel.IsDP1);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP2", SqlDbType.Bit, _objModel.IsDP2);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP3", SqlDbType.Bit, _objModel.IsDP3);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP4", SqlDbType.Bit, _objModel.IsDP4);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP5", SqlDbType.Bit, _objModel.IsDP5);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP6", SqlDbType.Bit, _objModel.IsDP6);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP7", SqlDbType.Bit, _objModel.IsDP7);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP71", SqlDbType.Bit, _objModel.IsDP71);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP72", SqlDbType.Bit, _objModel.IsDP72);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP73", SqlDbType.Bit, _objModel.IsDP73);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP8", SqlDbType.Bit, _objModel.IsDP8);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP9", SqlDbType.Bit, _objModel.IsDP9);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP10", SqlDbType.Bit, _objModel.IsDP10);
            ClsAppDatabase.AddInParameter(cmd, "@pIsDP11", SqlDbType.Bit, _objModel.IsDP11);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB", SqlDbType.Bit, _objModel.IsSB);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB1", SqlDbType.Bit, _objModel.IsSB1);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB11", SqlDbType.Bit, _objModel.IsSB11);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB12", SqlDbType.Bit, _objModel.IsSB12);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB2", SqlDbType.Bit, _objModel.IsSB2);
            ClsAppDatabase.AddInParameter(cmd, "@pIsSB21", SqlDbType.Bit, _objModel.IsSB21);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP", SqlDbType.Bit, _objModel.IsAP);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP1", SqlDbType.Bit, _objModel.IsAP1);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP11", SqlDbType.Bit, _objModel.IsAP11);
            ClsAppDatabase.AddInParameter(cmd, "@pAPIndName11", SqlDbType.VarChar, _objModel.APIndName11 == null ? "" : _objModel.APIndName11.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP12", SqlDbType.Bit, _objModel.IsAP12);
            ClsAppDatabase.AddInParameter(cmd, "@pAPIndName12", SqlDbType.VarChar, _objModel.APIndName12 == null ? "" : _objModel.APIndName12.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP2", SqlDbType.Bit, _objModel.IsAP2);
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP21", SqlDbType.Bit, _objModel.IsAP21);
            ClsAppDatabase.AddInParameter(cmd, "@pAPIndName21", SqlDbType.VarChar, _objModel.APIndName21 == null ? "" : _objModel.APIndName21.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsAP22", SqlDbType.Bit, _objModel.IsAP22);
            ClsAppDatabase.AddInParameter(cmd, "@pAPIndName22", SqlDbType.VarChar, _objModel.APIndName22 == null ? "" : _objModel.APIndName22.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsMC", SqlDbType.Bit, _objModel.IsMC);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMC1", SqlDbType.Bit, _objModel.IsMC1);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMC2", SqlDbType.Bit, _objModel.IsMC2);
            ClsAppDatabase.AddInParameter(cmd, "@pComment", SqlDbType.VarChar, _objModel.Comment == null ? "" : _objModel.Comment.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<CustomerDocModel> CustomerClassDetail_ListAll(CustomerDocModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerClassDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerClassId", SqlDbType.Int, _objModel.CustomerClassId);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerDocModel>(dataReader as DbDataReader).ToList();
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
        public List<CustomerDocModel> CustomerMaster_GetScreening(CustomerDocModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMaster_GetScreening");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerDocModel>(dataReader as DbDataReader).ToList();
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

        public List<CustomerDocModel> CustomerScreening_ListAll(CustomerDocModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerScreening_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pScreeningID", SqlDbType.Int, _objModel.ScreeningID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<CustomerDocModel>(dataReader as DbDataReader).ToList();
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

        public int CustomerScreening_Add(CustomerDocModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerScreening_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pScreeningID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsAppDatabase.AddInParameter(cmd, "@pName", SqlDbType.VarChar, _objModel.Name == null ? "" : _objModel.Name.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCountryName", SqlDbType.VarChar, _objModel.CountryName == null ? "" : _objModel.CountryName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pType", SqlDbType.VarChar, _objModel.Type == null ? "" : _objModel.Type.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.ScrStatus == null ? "" : _objModel.ScrStatus.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        #endregion
    }
}