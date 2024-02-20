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
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;

namespace TMP.DAL
{
    public class ClsServiceProvider
    {
        #region Local Variable
        ConString CN = new ConString();
        public SqlTransaction Tras { get; set; }
        public SqlConnection Conn { get; set; }
        Function FN = new Function();
        #endregion

        #region Customer Master
        public int CustomerMaster_Add(ServiceProviderModel _objModel)
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
            if (_objModel.CustomerTypeID == CONT.ServiceProviderCustomerTypeID ||
                      _objModel.CustomerTypeID == CONT.InsuranceProviderTypeID ||
                      _objModel.CustomerTypeID == CONT.InsuranceBrokerTypeID)
            {
                ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, 0);
                ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, "");
            }
            else
            {
                ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
                ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            }
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
        public int CustomerMaster_Update(ServiceProviderModel _objModel)
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
            if (_objModel.CustomerTypeID == CONT.ServiceProviderCustomerTypeID ||
               _objModel.CustomerTypeID == CONT.InsuranceProviderTypeID ||
               _objModel.CustomerTypeID == CONT.InsuranceBrokerTypeID)
            {
                ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, 0);
                ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, "");
            }
            else
            {
                ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
                ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            }
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
        public int CustomerMaster_UpdateIsAgree(ServiceProviderModel _objModel)
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
        public int CustomerMasterHistory_Add(ServiceProviderModel _objModel)
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
            if (_objModel.CustomerTypeID == CONT.ServiceProviderCustomerTypeID ||
                  _objModel.CustomerTypeID == CONT.InsuranceProviderTypeID ||
                  _objModel.CustomerTypeID == CONT.InsuranceBrokerTypeID)
            {
                ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, 0);
                ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, "");
            }
            else
            {
                ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.Int, _objModel.CityID);
                ClsAppDatabase.AddInParameter(cmd, "@pOtherCity", SqlDbType.VarChar, _objModel.OtherCity == null ? "" : _objModel.OtherCity);
            }
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
        public int CustomerMaster_UpdateAllStatus(ServiceProviderModel _objModel)
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
        public List<ServiceProviderModel> CustomerMaster_ListAll(ServiceProviderModel _objModel)
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
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ServiceProviderModel>(dataReader as DbDataReader).ToList();
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
        public List<ServiceProviderModel> CustomerMasterHistory_ListAll(ServiceProviderModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMasterHistory_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerHistoryID", SqlDbType.Int, _objModel.CustomerHistoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCompanyName", SqlDbType.VarChar, _objModel.CompanyName == null ? "" : _objModel.CompanyName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerTypeID", SqlDbType.Int, _objModel.CustomerTypeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ServiceProviderModel>(dataReader as DbDataReader).ToList();
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
        public List<ServiceProviderModel> CustomerMasterHistory_ListAllBind(ServiceProviderModel _objModel)
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
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ServiceProviderModel>(dataReader as DbDataReader).ToList();
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
        public List<ServiceProviderModel> CustomerMaster_BindAllDocument(ServiceProviderModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerMaster_BindAllDocument");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ServiceProviderModel>(dataReader as DbDataReader).ToList();
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

        #region Customer Info Detail
        public int CustomerInfoDetail_Add(ServiceProviderModel _objModel)
        {
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
            ClsAppDatabase.AddInParameter(cmd, "@pTelNo", SqlDbType.VarChar, _objModel.TelNoInfo == null ? "" : _objModel.TelNoInfo);
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
        public int CustomerInfoDetailHistory_Add(ServiceProviderModel _objModel)
        {
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
        public int CustomerInfoDetail_Delete(ServiceProviderModel _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CustomerInfoDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = Tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CustomerInfoDetail_Update(ServiceProviderModel _objModel)
        {
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
        public int CustomerInfoDetail_UpdateNotRequired(ServiceProviderModel _objModel)
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
        public List<ServiceProviderModel> CustomerInfoDetail_ListAll(ServiceProviderModel _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CustomerInfoDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerInfoID", SqlDbType.Int, _objModel.CustomerInfoID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ServiceProviderModel>(dataReader as DbDataReader).ToList();
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
        public List<ServiceProviderModel> CustomerInfoDetailHistory_ListAll(ServiceProviderModel _objModel)
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
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ServiceProviderModel>(dataReader as DbDataReader).ToList();
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
        public List<ServiceProviderModel> CustomerInfoDetailHistory_ListAllBind(ServiceProviderModel _objModel)
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
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ServiceProviderModel>(dataReader as DbDataReader).ToList();
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

        #region User Master
        //public int UserMaster_Add(ServiceProviderModel _objModel)
        //{
        //    int blnResult = 0;
        //    SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_Add");
        //    ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, _objModel.LoginName == null ? "" : _objModel.LoginName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pGender", SqlDbType.VarChar, _objModel.Gender == null ? "" : _objModel.Gender);
        //    ClsAppDatabase.AddInParameter(cmd, "@pLocCountryID", SqlDbType.Int, _objModel.LocCountryID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, _objModel.ServLength == null ? "" : _objModel.ServLength);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, _objModel.Photo == null ? "" : _objModel.Photo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pPassword", SqlDbType.VarChar, _objModel.Password == null ? "" : _objModel.Password);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
        //    cmd.Transaction = Tras;
        //    int Row = cmd.ExecuteNonQuery();
        //    cmd.Parameters.Clear();
        //    cmd.Dispose();
        //    return blnResult;
        //}
        //public int UserMaster_Update(ServiceProviderModel _objModel)
        //{
        //    int blnResult = 0;
        //    SqlCommand cmd = ClsAppDatabase.GetSPName("UserMaster_Update");
        //    ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, _objModel.LoginName == null ? "" : _objModel.LoginName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, _objModel.ServLength == null ? "" : _objModel.ServLength);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, _objModel.Photo == null ? "" : _objModel.Photo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
        //    cmd.Transaction = Tras;
        //    int Row = cmd.ExecuteNonQuery();
        //    cmd.Parameters.Clear();
        //    cmd.Dispose();
        //    return blnResult;
        //}
        //public int UserMasterHistory_Add(ServiceProviderModel _objModel)
        //{
        //    int blnResult = 0;
        //    SqlCommand cmd = ClsAppDatabase.GetSPName("UserMasterHistory_Add");
        //    ClsAppDatabase.AddInParameter(cmd, "@pUserHistoryID", SqlDbType.Int, _objModel.UserHistoryID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pUserID", SqlDbType.Int, _objModel.UserID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pFirstName", SqlDbType.VarChar, _objModel.FirstName == null ? "" : _objModel.FirstName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pMiddleName", SqlDbType.VarChar, _objModel.MiddleName == null ? "" : _objModel.MiddleName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pLastName", SqlDbType.VarChar, _objModel.LastName == null ? "" : _objModel.LastName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pLoginName", SqlDbType.VarChar, _objModel.LoginName == null ? "" : _objModel.LoginName);
        //    ClsAppDatabase.AddInParameter(cmd, "@pPassword", SqlDbType.VarChar, _objModel.Password == null ? "" : _objModel.Password);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerID", SqlDbType.Int, _objModel.CustomerID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pEmailID", SqlDbType.VarChar, _objModel.EmailID == null ? "" : _objModel.EmailID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pMobileNo", SqlDbType.VarChar, _objModel.MobileNo == null ? "" : _objModel.MobileNo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pServLength", SqlDbType.VarChar, _objModel.ServLength == null ? "" : _objModel.ServLength);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDesignationID", SqlDbType.Int, _objModel.DesignationID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pOtherDesignation", SqlDbType.VarChar, _objModel.OtherDesignation == null ? "" : _objModel.OtherDesignation);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDocumentID", SqlDbType.Int, _objModel.DocumentID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pDocNo", SqlDbType.VarChar, _objModel.DocNo == null ? "" : _objModel.DocNo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pAttach", SqlDbType.VarChar, _objModel.Attach == null ? "" : _objModel.Attach);
        //    ClsAppDatabase.AddInParameter(cmd, "@pCustomerShareHoldID", SqlDbType.Int, _objModel.CustomerShareHoldID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pPhoto", SqlDbType.VarChar, _objModel.Photo == null ? "" : _objModel.Photo);
        //    ClsAppDatabase.AddInParameter(cmd, "@pPwdExpiryDate", SqlDbType.DateTime, _objModel.PwdExpiryDate);
        //    ClsAppDatabase.AddInParameter(cmd, "@pIsAgree", SqlDbType.Bit, _objModel.IsAgree);
        //    ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, _objModel.IsActive);
        //    ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, _objModel.Status == null ? "" : _objModel.Status);
        //    ClsAppDatabase.AddInParameter(cmd, "@pProcessRemark", SqlDbType.VarChar, _objModel.ProcessRemark == null ? "" : _objModel.ProcessRemark);
        //    ClsAppDatabase.AddInParameter(cmd, "@pProcessBy", SqlDbType.Int, FN.LoggedUserID);
        //    ClsAppDatabase.AddInParameter(cmd, "@pProcessIP", SqlDbType.VarChar, FN.GetSystemIP());
        //    cmd.Transaction = Tras;
        //    int Row = cmd.ExecuteNonQuery();
        //    cmd.Parameters.Clear();
        //    cmd.Dispose();
        //    return blnResult;
        //}
        #endregion
        public List<ServiceProviderModel> CityMaster_BindCityName(ServiceProviderModel _model)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CityMaster_BindCityName");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.Int, _model.StateID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)CN).ObjectContext.Translate<ServiceProviderModel>(dataReader as DbDataReader).ToList();
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