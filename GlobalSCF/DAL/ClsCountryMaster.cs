using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMP.Models;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using static TMP.Models.CountryMaster;

namespace TMP.DAL
{
    public class ClsCountryMaster
    {
        ConString db = new ConString();
        public SqlTransaction tras { get; set; }
        public SqlConnection conn { get; set; }
        Function FN = new Function();
        public List<CountryMaster> CountryMaster_ListAll(Nullable<int> pCountryID, string pCountryName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue, Nullable<short> pIsNationality)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CountryMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCountryName", SqlDbType.VarChar, pCountryName);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.SmallInt, pIsActive);            
            ClsEntityAppDatabase.AddInParameter(cmd, "@pIsNationality", SqlDbType.SmallInt, pIsNationality);
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
        public int CountryMaster_Add(Nullable<int> pCountryID, string pCountryCode, string pCountryName, Nullable<bool> pIsNationality, Nullable<int> pCreateBy, string pCreateIP,
                                     int RiskLevel, string Sanction)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CountryMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCountryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryCode", SqlDbType.Char, pCountryCode);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryName", SqlDbType.VarChar, pCountryName);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNationality", SqlDbType.Int, pIsNationality);
            ClsAppDatabase.AddInParameter(cmd, "@pRiskLevel", SqlDbType.Int, RiskLevel);
            ClsAppDatabase.AddInParameter(cmd, "@pSanction", SqlDbType.VarChar, Sanction);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, pCreateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, pCreateIP);
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCountryID"].Value);
            cmd.Dispose();
            return blnResult;
        }
        public int CountryMaster_Update(int pCountryID, string pCountryCode, string pCountryName, Nullable<bool> pIsNationality, int pUpdateBy, string pUpdateIP,
                                        int RiskLevel, string Sanction)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CountryMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryCode", SqlDbType.VarChar, pCountryCode);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryName", SqlDbType.VarChar, pCountryName);
            ClsAppDatabase.AddInParameter(cmd, "@pIsNationality", SqlDbType.Int, pIsNationality);
            ClsAppDatabase.AddInParameter(cmd, "@pRiskLevel", SqlDbType.Int, RiskLevel);
            ClsAppDatabase.AddInParameter(cmd, "@pSanction", SqlDbType.VarChar, Sanction);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, pUpdateBy);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, pUpdateIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public int CountryMaster_Delete(Nullable<int> pCountryID, Nullable<int> pDeleteBy, string pDeleteIP)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CountryMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, pCountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, pDeleteBy);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, pDeleteIP);
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Dispose();
            return blnResult;
        }
        public List<CountryMaster> SystemPerameter_ListAll()
        {
            //return db.Database.SqlQuery<CountryMaster>(" exec SystemParameter_ListAll ").ToList();
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("SystemParameter_ListAll");
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
        public void SystemPeraUpdate(int SystemPerameterID, int ProcessTranDay, string WebSiteURL, int pFundRequireDay, int pLoginLockoutAttempt,
           int pLoginLockoutTimeFrameMin, int pPwdExpiryDay, int pLastPwdHistoryDay, int pSiteIdleSessionMin, int UpdateBy, string UpdateIp, string SukukIssuanceName,
           Boolean IsMaintenance, string MaintenanceMsg, string TMPWebSite, string TMPEmail, string TMPTeam, string TMPPlatform)
        {
            SqlCommand cmd = ClsAppDatabase.GetSPName("SystemParameter_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pSystemParamterID", SqlDbType.Int, SystemPerameterID);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginLockoutAttempt", SqlDbType.Int, pLoginLockoutAttempt);
            ClsAppDatabase.AddInParameter(cmd, "@pLoginLockoutTimeFrameMin", SqlDbType.Int, pLoginLockoutTimeFrameMin);
            ClsAppDatabase.AddInParameter(cmd, "@pPwdExpiryDay", SqlDbType.Int, pPwdExpiryDay);
            ClsAppDatabase.AddInParameter(cmd, "@pLastPwdHistoryDay", SqlDbType.Int, pLastPwdHistoryDay);
            ClsAppDatabase.AddInParameter(cmd, "@pSiteIdleSessionMin", SqlDbType.Int, pSiteIdleSessionMin);
            ClsAppDatabase.AddInParameter(cmd, "@pWebSiteURL", SqlDbType.VarChar, WebSiteURL);
            ClsAppDatabase.AddInParameter(cmd, "@pIsMaintenance", SqlDbType.Bit, IsMaintenance);
            ClsAppDatabase.AddInParameter(cmd, "@pMaintenanceMsg", SqlDbType.VarChar, MaintenanceMsg);
            ClsAppDatabase.AddInParameter(cmd, "@pProcessTranDay", SqlDbType.Int, ProcessTranDay);
            ClsAppDatabase.AddInParameter(cmd, "@pFundRequireDay", SqlDbType.Int, pFundRequireDay);
            ClsAppDatabase.AddInParameter(cmd, "@pTMPWebSite", SqlDbType.VarChar, TMPWebSite);
            ClsAppDatabase.AddInParameter(cmd, "@pTMPEmail", SqlDbType.VarChar, TMPEmail);
            ClsAppDatabase.AddInParameter(cmd, "@pTMPTeam", SqlDbType.VarChar, TMPTeam);
            ClsAppDatabase.AddInParameter(cmd, "@pTMPPlatform", SqlDbType.VarChar, TMPPlatform);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
        }

        #region Transfer Charge Master
        public List<CountryMaster> TransChargeMaster_ListAll(CountryMaster _ObjModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("TransChargeMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pTransChargeID", SqlDbType.Int, _ObjModel.TransChargeID == null ? 0 : _ObjModel.TransChargeID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _ObjModel.CountryID == null ? 0 : _ObjModel.CountryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _ObjModel.CurrencyID == null ? 0 : _ObjModel.CurrencyID);
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
        public int TransChargeMaster_Add(CountryMaster _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("TransChargeMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pTransChargeID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pConvCurrencyID", SqlDbType.Int, _objModel.ConvCurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pFeePer", SqlDbType.Decimal, _objModel.FeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeAmt", SqlDbType.Decimal, _objModel.FeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pTransChargeID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int TransChargeMaster_Update(CountryMaster _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("TransChargeMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pTransChargeID", SqlDbType.Int, _objModel.TransChargeID);
            ClsAppDatabase.AddInParameter(cmd, "@pCountryID", SqlDbType.Int, _objModel.CountryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCurrencyID", SqlDbType.Int, _objModel.CurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pConvCurrencyID", SqlDbType.Int, _objModel.ConvCurrencyID);
            ClsAppDatabase.AddInParameter(cmd, "@pFeePer", SqlDbType.Decimal, _objModel.FeePer);
            ClsAppDatabase.AddInParameter(cmd, "@pFeeAmt", SqlDbType.Decimal, _objModel.FeeAmt);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int TransChargeMaster_Delete(CountryMaster _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("TransChargeMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pTransChargeID", SqlDbType.Int, _objModel.TransChargeID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            blnResult = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        #endregion

        #region Screening Config Master

        public int CategoryMaster_Add(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryMaster_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCategoryID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryName", SqlDbType.VarChar, _objModel.CategoryName == null ? "" : _objModel.CategoryName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pLowFromRate", SqlDbType.VarChar, _objModel.LowFromRate == null ? "" : _objModel.LowFromRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pLowToRate", SqlDbType.VarChar, _objModel.LowToRate == null ? "" : _objModel.LowToRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pModFromRate", SqlDbType.VarChar, _objModel.ModFromRate == null ? "" : _objModel.ModFromRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pModToRate", SqlDbType.VarChar, _objModel.ModToRate == null ? "" : _objModel.ModToRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pHighFromRate", SqlDbType.VarChar, _objModel.HighFromRate == null ? "" : _objModel.HighFromRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pHighToRate", SqlDbType.VarChar, _objModel.HighToRate == null ? "" : _objModel.HighToRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pVHighFromRate", SqlDbType.VarChar, _objModel.VHighFromRate == null ? "" : _objModel.VHighFromRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pVHighToRate", SqlDbType.VarChar, _objModel.VHighToRate == null ? "" : _objModel.VHighToRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRemark", SqlDbType.VarChar, _objModel.Remark == null ? "" : _objModel.Remark.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCategoryID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CategoryMaster_Update(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryMaster_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryName", SqlDbType.VarChar, _objModel.CategoryName == null ? "" : _objModel.CategoryName.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pLowFromRate", SqlDbType.VarChar, _objModel.LowFromRate == null ? "" : _objModel.LowFromRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pLowToRate", SqlDbType.VarChar, _objModel.LowToRate == null ? "" : _objModel.LowToRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pModFromRate", SqlDbType.VarChar, _objModel.ModFromRate == null ? "" : _objModel.ModFromRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pModToRate", SqlDbType.VarChar, _objModel.ModToRate == null ? "" : _objModel.ModToRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pHighFromRate", SqlDbType.VarChar, _objModel.HighFromRate == null ? "" : _objModel.HighFromRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pHighToRate", SqlDbType.VarChar, _objModel.HighToRate == null ? "" : _objModel.HighToRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pVHighFromRate", SqlDbType.VarChar, _objModel.VHighFromRate == null ? "" : _objModel.VHighFromRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pVHighToRate", SqlDbType.VarChar, _objModel.VHighToRate == null ? "" : _objModel.VHighToRate.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pRemark", SqlDbType.VarChar, _objModel.Remark == null ? "" : _objModel.Remark.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, _objModel.IsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CategoryMaster_Delete(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryMaster_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public List<ScreeningConfig> CategoryMaster_ListAll(ScreeningConfig _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CategoryMaster_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<ScreeningConfig>(dataReader as DbDataReader).ToList();
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
        public List<ScreeningConfig> CategoryRiskDetail_ListAll(ScreeningConfig _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CategoryRiskDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCategoryRiskID", SqlDbType.Int, _objModel.CategoryRiskID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<ScreeningConfig>(dataReader as DbDataReader).ToList();
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
        public List<ScreeningConfig> CategoryDetail_ListAll(ScreeningConfig _objModel)
        {
            DbCommand cmd = ClsEntityAppDatabase.GetSPName("CategoryDetail_ListAll");
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCategoryDetID", SqlDbType.Int, _objModel.CategoryDetID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            ClsEntityAppDatabase.AddInParameter(cmd, "@pParentCategoryDetID", SqlDbType.Int, _objModel.ParentCategoryDetID);
            try
            {
                using (var dataReader = cmd.ExecuteReader())
                {
                    return ((IObjectContextAdapter)db).ObjectContext.Translate<ScreeningConfig>(dataReader as DbDataReader).ToList();
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

        public int CategoryRiskDetail_Add(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryRiskDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCategoryRiskID", SqlDbType.Int, _objModel.CategoryRiskID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pRiskLevel", SqlDbType.VarChar, _objModel.RiskLevel == null ? "" : _objModel.RiskLevel.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pFromRating", SqlDbType.VarChar, _objModel.FromRating == null ? "" : _objModel.FromRating.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pToRating", SqlDbType.VarChar, _objModel.ToRating == null ? "" : _objModel.ToRating.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCategoryRiskID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CategoryRiskDetail_Update(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryRiskDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryRiskID", SqlDbType.Int, _objModel.CategoryRiskID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pRiskLevel", SqlDbType.VarChar, _objModel.RiskLevel == null ? "" : _objModel.RiskLevel.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pFromRating", SqlDbType.VarChar, _objModel.FromRating == null ? "" : _objModel.FromRating.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pToRating", SqlDbType.VarChar, _objModel.ToRating == null ? "" : _objModel.ToRating.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CategoryRiskDetail_Delete(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryRiskDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryRiskID", SqlDbType.Int, _objModel.CategoryRiskID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CategoryDetail_Add(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryDetail_Add");
            ClsAppDatabase.AddOutParameter(cmd, "@pCategoryDetID", SqlDbType.Int);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pParentCategoryDetID", SqlDbType.Int, _objModel.ParentCategoryDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pValue", SqlDbType.VarChar, _objModel.Value == null ? "" : _objModel.Value.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pYesRiskLevel", SqlDbType.VarChar, _objModel.YesRiskLevel == null ? "" : _objModel.YesRiskLevel.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pNoRiskLevel", SqlDbType.VarChar, _objModel.NoRiskLevel == null ? "" : _objModel.NoRiskLevel.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pOrderNo", SqlDbType.Int, _objModel.OrderNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIsOverride", SqlDbType.Bit, _objModel.IsOverride);
            ClsAppDatabase.AddInParameter(cmd, "@pYesComment", SqlDbType.VarChar, _objModel.YesComment == null ? "" : _objModel.YesComment.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pNoComment", SqlDbType.VarChar, _objModel.NoComment == null ? "" : _objModel.NoComment.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSelComment", SqlDbType.VarChar, _objModel.SelComment);
            ClsAppDatabase.AddInParameter(cmd, "@pOveRiskLevel", SqlDbType.VarChar, _objModel.OveRiskLevel);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pCreateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            blnResult = Convert.ToInt16(cmd.Parameters["@pCategoryDetID"].Value);
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CategoryDetail_Update(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryDetail_Update");
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryDetID", SqlDbType.Int, _objModel.CategoryDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryID", SqlDbType.Int, _objModel.CategoryID);
            ClsAppDatabase.AddInParameter(cmd, "@pParentCategoryDetID", SqlDbType.Int, _objModel.ParentCategoryDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pValue", SqlDbType.VarChar, _objModel.Value == null ? "" : _objModel.Value.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pYesRiskLevel", SqlDbType.VarChar, _objModel.YesRiskLevel == null ? "" : _objModel.YesRiskLevel.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pNoRiskLevel", SqlDbType.VarChar, _objModel.NoRiskLevel == null ? "" : _objModel.NoRiskLevel.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pOrderNo", SqlDbType.Int, _objModel.OrderNo);
            ClsAppDatabase.AddInParameter(cmd, "@pIsOverride", SqlDbType.Bit, _objModel.IsOverride);
            ClsAppDatabase.AddInParameter(cmd, "@pYesComment", SqlDbType.VarChar, _objModel.YesComment == null ? "" : _objModel.YesComment.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pNoComment", SqlDbType.VarChar, _objModel.NoComment == null ? "" : _objModel.NoComment.Trim());
            ClsAppDatabase.AddInParameter(cmd, "@pSelComment", SqlDbType.VarChar, _objModel.SelComment);
            ClsAppDatabase.AddInParameter(cmd, "@pOveRiskLevel", SqlDbType.VarChar, _objModel.OveRiskLevel);
            ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.Bit, _objModel.IsActive);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pUpdateIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }
        public int CategoryDetail_Delete(ScreeningConfig _objModel)
        {
            int blnResult = 0;
            SqlCommand cmd = ClsAppDatabase.GetSPName("CategoryDetail_Delete");
            ClsAppDatabase.AddInParameter(cmd, "@pCategoryDetID", SqlDbType.Int, _objModel.CategoryDetID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteBy", SqlDbType.Int, FN.LoggedUserID);
            ClsAppDatabase.AddInParameter(cmd, "@pDeleteIP", SqlDbType.VarChar, FN.GetSystemIP());
            cmd.Transaction = tras;
            int Row = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.Dispose();
            return blnResult;
        }

        #endregion
    }
}