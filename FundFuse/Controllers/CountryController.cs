using TMP.DAL;
using TMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using System.Data;
using static TMP.Models.CountryMaster;

namespace TMP.Controllers
{
    public class CountryController : Controller
    {
        #region Local Variable
        ClsTemplate TemplateObj = new ClsTemplate();
        ConString con = new ConString();
        Function fn = new Function();
        CountryMaster countrymaster = new CountryMaster();
        CountryMaster companymaster = new CountryMaster();
        ClsCityMaster city_mater = new ClsCityMaster();
        ClsIndustryMaster indusrty_master = new ClsIndustryMaster();
        ClsCompanyTypeMaster company_type = new ClsCompanyTypeMaster();
        ClsDesignationMaster designation_type = new ClsDesignationMaster();
        ClsBankMaster bank_master = new ClsBankMaster();
        ClsLanguageMaster language_master = new ClsLanguageMaster();
        ClsCurrency currencymaster = new ClsCurrency();
        ClsInsuranceProviderMaster insurane_master = new ClsInsuranceProviderMaster();
        ClsFAQ faq = new ClsFAQ();
        ClsCountryMaster country_master = new ClsCountryMaster();
        private int txtCity;
        #endregion
        public ActionResult Index(string msg = "", string a = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string MenuId = Convert.ToString(Session["MenuID"]);
                int ParentMenuID = 0;
                int.TryParse(MenuId, out ParentMenuID);
                var UserRight = fn.CheckUserRight("", "Maker", ParentMenuID);
                if (UserRight != null)
                {
                    ClsCountryMaster cm = new ClsCountryMaster();
                    ClsCityMaster cm2 = new ClsCityMaster();
                    ClsIndustryMaster indusrty_master = new ClsIndustryMaster();
                    ClsCompanyTypeMaster company_type = new ClsCompanyTypeMaster();
                    ClsDesignationMaster designation_type = new ClsDesignationMaster();
                    ClsLanguageMaster language_master = new ClsLanguageMaster();
                    ClsInsuranceProviderMaster insurane_master = new ClsInsuranceProviderMaster();
                    CountryMaster cm3 = new CountryMaster();
                    CountryMaster cm4 = new CountryMaster();

                    if (Request.QueryString["ErrorMessage"] != null && Request.QueryString["ErrorMessage"] != "")
                    {
                        ViewBag.ErrorMsg = Request.QueryString["ErrorMessage"].ToString();
                    }
                    if (Request.QueryString["ErrorMessagec"] != null && Request.QueryString["ErrorMessagec"] != "")
                    {
                        ViewBag.ErrorMsgc = Request.QueryString["ErrorMessagec"].ToString();
                    }
                    if (Request.QueryString["ErrorMessagecity"] != null && Request.QueryString["ErrorMessagecity"] != "")
                    {
                        ViewBag.ErrorMsgcity = Request.QueryString["ErrorMessagecity"].ToString();
                    }
                    if (Request.QueryString["ErrorMessageind"] != null && Request.QueryString["ErrorMessagecind"] != "")
                    {
                        ViewBag.ErrorMsgcind = Request.QueryString["ErrorMessageind"].ToString();
                    }
                    if (Request.QueryString["ErrorMessagebank"] != null && Request.QueryString["ErrorMessagecbank"] != "")
                    {
                        ViewBag.ErrorMsgbank = Request.QueryString["ErrorMessagebank"].ToString();
                    }
                    if (Request.QueryString["ErrorMessageind"] != null && Request.QueryString["ErrorMessageind"] != "")
                    {
                        ViewBag.ErrorMsgind = Request.QueryString["ErrorMessageind"].ToString();
                    }
                    if (Request.QueryString["ErrorMessagetype"] != null && Request.QueryString["ErrorMessagetype"] != "")
                    {
                        ViewBag.ErrorMsgtype = Request.QueryString["ErrorMessagetype"].ToString();
                    }
                    if (Request.QueryString["ErrorMessageins"] != null && Request.QueryString["ErrorMessageins"] != "")
                    {
                        ViewBag.ErrorMsgins = Request.QueryString["ErrorMessageins"].ToString();
                    }
                    if (Request.QueryString["ErrorMessagedes"] != null && Request.QueryString["ErrorMessagedes"] != "")
                    {
                        ViewBag.ErrorMsgdes = Request.QueryString["ErrorMessagedes"].ToString();
                    }
                    if (Request.QueryString["ErrorMessagecountry"] != null && Request.QueryString["ErrorMessagecountry"] != "")
                    {
                        ViewBag.ErrorMsgcountry = Request.QueryString["ErrorMessagecountry"].ToString();
                    }
                    try
                    {
                        ViewBag.CountryID = new SelectList(cm.CountryMaster_ListAll(0, "", -1, "", false, "", -1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                        ViewBag.cityID = new SelectList(cm2.CityMaster_ListAll(0, 0, "AA", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityCode", "CityName");
                        ViewBag.CityCode = new SelectList(cm2.CityMaster_ListAll(0, 0, "AA", 1, "", false, "").OrderBy(x => x.CityCode).ToList(), "CityID", "CityCode");
                        ViewBag.IndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.SubIndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.SubIndustryID1 = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.CountryID2 = new SelectList(cm.CountryMaster_ListAll(0, "", -1, "", false, "", -1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                        ViewBag.CompanyTypeID = new SelectList(company_type.CompanyTypeMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.CompanyTypeName).ToList(), "CompanyTypeID", "CompanyTypeName");
                        ViewBag.DesignationID = new SelectList(designation_type.DesignationMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                        ViewBag.BankID = new SelectList(bank_master.BankMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.BankName).ToList(), "BankID", "BankName");
                        ViewBag.LanguageID = new SelectList(language_master.LanguageMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.LanguageName).ToList(), "LanguageID", "LanguageName");
                        ViewBag.CurrencyID = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, "").OrderBy(x => x.CurrencyName).ToList(), "CurrencyID", "CurrencyName");
                        ViewBag.CurrencyCode = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, "").OrderBy(x => x.CurrencyCode).ToList(), "CurrencyID", "CurrencyCode");
                        ViewBag.InsuranceProviderID = new SelectList(insurane_master.InsuranceProviderMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.InsuranceProviderID).ToList(), "InsuranceProviderID", "InsuranceProviderName");
                        ViewBag.TemplateID = new SelectList(TemplateObj.Template_ListAll(0, "").OrderBy(x => x.Name).ToList(), "TemplateID", "Name");
                        ViewBag.DynamicTextID = new SelectList(TemplateObj.HtmlDynamicText_ListAll(0, "", false, "").OrderBy(x => x.DynamicTextName).ToList(), "DynamicTextID", "DynamicTextName");

                        if (!string.IsNullOrEmpty(msg))
                        {
                            if (msg == "1" && a == "designation1")
                            { ViewData["designation1"] = "Added successfully"; }
                            else if (msg == "2" && a == "designation1")
                            { ViewData["designation1"] = "Updated successfully"; }
                            else if (msg == "3" && a == "designation1")
                            { ViewData["designation1"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "UPDATE")
                            { ViewData["UPDATE"] = "Added successfully"; }
                            else if (msg == "2" && a == "UPDATE")
                            { ViewData["UPDATE"] = "Updated successfully"; }
                            else if (msg == "3" && a == "UPDATE")
                            { ViewData["UPDATE"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "company1")
                            { ViewData["company1"] = "Added successfully"; }
                            else if (msg == "2" && a == "company1")
                            { ViewData["company1"] = "Updated successfully"; }
                            else if (msg == "3" && a == "company1")
                            { ViewData["company1"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "companytype1")
                            { ViewData["companytype1"] = "Added successfully"; }
                            else if (msg == "2" && a == "companytype1")
                            { ViewData["companytype1"] = "Updated successfully"; }
                            else if (msg == "3" && a == "companytype1")
                            { ViewData["companytype1"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "indusrty2")
                            { ViewData["indusrty2"] = "Added successfully"; }
                            else if (msg == "2" && a == "indusrty2")
                            { ViewData["indusrty2"] = "Updated successfully"; }
                            else if (msg == "3" && a == "indusrty2")
                            { ViewData["indusrty2"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "insurance1")
                            { ViewData["insurance1"] = "Added successfully"; }
                            else if (msg == "2" && a == "insurance1")
                            { ViewData["insurance1"] = "Updated successfully"; }
                            else if (msg == "3" && a == "insurance1")
                            { ViewData["insurance1"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "launguage")
                            { ViewData["launguage"] = "Added successfully"; }
                            else if (msg == "2" && a == "launguage")
                            { ViewData["launguage"] = "Updated successfully"; }
                            else if (msg == "1" && a == "indusrty")
                            { ViewData["indusrty"] = "Added successfully"; }
                            else if (msg == "2" && a == "indusrty")
                            { ViewData["indusrty"] = "Updated successfully"; }
                            else if (msg == "3" && a == "indusrty")
                            { ViewData["indusrty"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "bank")
                            { ViewData["bank"] = "Added successfully"; }
                            else if (msg == "2" && a == "bank")
                            { ViewData["bank"] = "Updated successfully"; }
                            else if (msg == "3" && a == "bank")
                            { ViewData["bank"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "btncurrency")
                            { ViewData["btncurrency"] = "Added successfully"; }
                            else if (msg == "2" && a == "btncurrency")
                            { ViewData["btncurrency"] = "Updated successfully"; }
                            else if (msg == "3" && a == "btncurrency")
                            { ViewData["btncurrency"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "btnTemplate")
                            { ViewData["btnTemplate"] = "Added successfully"; }
                            else if (msg == "2" && a == "btnTemplate")
                            { ViewData["btnTemplate"] = "Updated successfully"; }
                            else if (msg == "3" && a == "btnTemplate")
                            { ViewData["btnTemplate"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "btnDynamicTemplate")
                            { ViewData["btnDynamicTemplate"] = "Added successfully"; }
                            else if (msg == "2" && a == "btnDynamicTemplate")
                            { ViewData["btnDynamicTemplate"] = "Updated successfully"; }
                            else if (msg == "3" && a == "btnDynamicTemplate")
                            { ViewData["btnDynamicTemplate"] = "Deleted successfully"; }
                            else if (msg == "1" && a == "countrymsg")
                            { ViewData["countrymsg"] = "Added successfully"; }
                            else if (msg == "2" && a == "countrymsg")
                            { ViewData["countrymsg"] = "Updated successfully"; }
                            else if (msg == "3" && a == "countrymsg")
                            { ViewData["countrymsg"] = "Deleted successfully"; }
                        }
                        return View(cm4);
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = fn.CreateErrorMessage(ex);
                        fn.LogFileWrite(errorMessage);
                        ViewBag.CountryID2 = new SelectList(cm.CountryMaster_ListAll(0, "", -1, "", false, "", -1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                        ViewBag.IndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.SubIndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.SubIndustryID1 = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.CompanyTypeID = new SelectList(company_type.CompanyTypeMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.CompanyTypeName).ToList(), "CompanyTypeID", "CompanyTypeName");
                        ViewBag.DesignationID = new SelectList(designation_type.DesignationMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                        ViewBag.BankID = new SelectList(bank_master.BankMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.BankName).ToList(), "BankID", "BankName");
                        ViewBag.LanguageID = new SelectList(language_master.LanguageMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.LanguageName).ToList(), "LanguageID", "LanguageName");
                        ViewBag.CurrencyID = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, "").OrderBy(x => x.CurrencyName).ToList(), "CurrencyID", "CurrencyName");
                        ViewBag.CurrencyCode = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, "").OrderBy(x => x.CurrencyCode).ToList(), "CurrencyID", "CurrencyCode");
                        ViewBag.TemplateID = new SelectList(TemplateObj.Template_ListAll(0, "").OrderBy(x => x.Name).ToList(), "TemplateID", "Name");
                        ViewBag.DynamicTextID = new SelectList(TemplateObj.HtmlDynamicText_ListAll(0, "", false, "").OrderBy(x => x.DynamicTextName).ToList(), "DynamicTextID", "DynamicTextName");
                        return View();
                    }
                }
                else
                { return RedirectToAction("NoaccessPage", "MasterPage"); }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        #region Transfer Charge Master
        public ActionResult TransferChargeMaster(int TransChargeID = 0, int DeleteID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CountryMaster _ObjModel = new CountryMaster();
                FillCountryCombo();
                FillCurrencyCombo();

                if (TransChargeID != 0)
                {
                    _ObjModel.TransChargeID = TransChargeID;
                    var GetData = country_master.TransChargeMaster_ListAll(_ObjModel).FirstOrDefault();
                    if (GetData != null)
                    {
                        _ObjModel.TransChargeID = GetData.TransChargeID; _ObjModel.CountryID = GetData.CountryID;
                        _ObjModel.CurrencyID = GetData.CurrencyID; _ObjModel.CurrencyCode = GetData.CurrencyCode;
                        _ObjModel.ConvCurrencyID = GetData.ConvCurrencyID; _ObjModel.ConvCurrencyCode = GetData.ConvCurrencyCode;
                        _ObjModel.CountryName = GetData.CountryName; _ObjModel.FeePer = GetData.FeePer;

                        _ObjModel.FeeAmt = GetData.FeeAmt;
                        return View(_ObjModel);
                    }
                }
                if (DeleteID != 0)
                {
                    country_master.conn = ClsAppDatabase.GetCon();
                    if (country_master.conn.State == ConnectionState.Closed)
                    { country_master.conn.Open(); }
                    else { country_master.conn.Close(); country_master.conn.Open(); }
                    country_master.tras = country_master.conn.BeginTransaction();

                    _ObjModel.TransChargeID = DeleteID;
                    int Result = country_master.TransChargeMaster_Delete(_ObjModel);
                    country_master.tras.Commit();
                    country_master.conn.Close();
                    return RedirectToAction("TransferChargeMaster", "Country");
                }
                return View(_ObjModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult TransferChargeMaster(CountryMaster _ObjModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    country_master.conn = ClsAppDatabase.GetCon();
                    if (country_master.conn.State == ConnectionState.Closed)
                    { country_master.conn.Open(); }
                    else { country_master.conn.Close(); country_master.conn.Open(); }
                    country_master.tras = country_master.conn.BeginTransaction();
                    if (_ObjModel.TransChargeID == 0)
                    {
                        int Result = country_master.TransChargeMaster_Add(_ObjModel);
                        country_master.tras.Commit();
                        country_master.conn.Close();

                        return RedirectToAction("TransferChargeMaster", "Country");
                    }
                    else
                    {
                        int Result = country_master.TransChargeMaster_Update(_ObjModel);
                        country_master.tras.Commit();
                        country_master.conn.Close();
                        return RedirectToAction("TransferChargeMaster", "Country");
                    }
                }
                catch (Exception ex)
                {
                    if (country_master.conn != null)
                    { country_master.conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo();
                FillCurrencyCombo();
                return View(_ObjModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult GridTransferCharge()
        {
            var Data = country_master.TransChargeMaster_ListAll(countrymaster);
            return PartialView(Data);
        }
        public void FillCountryCombo()
        {
            ClsCountryMaster _ClsCountryMaster = new ClsCountryMaster();
            var DDLCountryIDData = _ClsCountryMaster.CountryMaster_ListAll(0, "", 1, "", false, "", -1).OrderBy(x => x.CountryName).ToList();
            if (DDLCountryIDData != null && DDLCountryIDData.Count > 0)
                ViewBag.DDLCountryID = DDLCountryIDData;
            else
                ViewBag.DDLCountryID = new List<SelectListItem> { };
        }
        public void FillCurrencyCombo()
        {
            ClsCurrency _ClsCurrency = new ClsCurrency();
            var DDLCurrencyData = _ClsCurrency.CurrencyMaster_ListAll(0, "", "", 1, "", false, "").OrderBy(x => x.CurrencyCode).ToList();
            if (DDLCurrencyData != null && DDLCurrencyData.Count > 0)
                ViewBag.DDLCurrencyID = DDLCurrencyData;
            else
                ViewBag.DDLCurrencyID = new List<SelectListItem> { };
        }
        #endregion
        public ActionResult SystemPerameter(int id = 1)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string MenuId = Convert.ToString(Session["MenuID"]);
                int ParentMenuID = 0;
                int.TryParse(MenuId, out ParentMenuID);
                var UserRight = fn.CheckUserRight("", "Maker", ParentMenuID);
                if (UserRight != null)
                {
                    var Data = country_master.SystemPerameter_ListAll().FirstOrDefault();
                    return View(Data);
                }
                else
                { return RedirectToAction("NoaccessPage", "MasterPage"); }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        public ActionResult SystemPerameter(CountryMaster model, FormCollection frm)
        {
            int SystemParamterID = Convert.ToInt32(frm["SystemParamterID"]);
            string WebSiteURL = frm["WebSiteURL"];
            int ProcessTranDay = Convert.ToInt32(frm["ProcessTranDay"]);
            int fund1 = Convert.ToInt32(frm["fund"]);
            int loginattempt = Convert.ToInt32(frm["loginattempt"]);
            int loginlock = Convert.ToInt32(frm["loginlock"]);
            int psdexpday = Convert.ToInt32(frm["psdexpday"]);
            int lastpwdepxy = Convert.ToInt32(frm["lastpwdepxy"]);
            int siteidle = Convert.ToInt32(frm["siteidle"]);
            string SukukIssuanceName = "";

            bool IsMaintenance = model.IsMaintenance;
            string MaintenanceMsg = frm["MaintenanceMsg"];


            string TMPWebSite = frm["TMPWebSite"];
            string TMPEmail = frm["TMPEmail"];
            string TMPTeam = frm["TMPTeam"];
            string TMPPlatform = frm["TMPPlatform"];

            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    var Data = country_master.SystemPerameter_ListAll().FirstOrDefault();
                    country_master.conn = ClsAppDatabase.GetCon();
                    if (country_master.conn.State == System.Data.ConnectionState.Closed)
                    { country_master.conn.Open(); }
                    else { country_master.conn.Close(); country_master.conn.Open(); }
                    country_master.tras = country_master.conn.BeginTransaction();
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    Function fn = new Function();
                    country_master.SystemPeraUpdate(SystemParamterID, ProcessTranDay, WebSiteURL, fund1, loginattempt, loginlock, psdexpday, lastpwdepxy,
                        siteidle, 1, "192.168.1.1", SukukIssuanceName, IsMaintenance, MaintenanceMsg,
                        TMPWebSite, TMPEmail, TMPTeam, TMPPlatform);
                    country_master.tras.Commit();
                    country_master.conn.Close();
                    var GetData = country_master.SystemPerameter_ListAll().FirstOrDefault();
                    ViewData["SuccessMessage"] = "Updated Succesfully";
                    return View(GetData);
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    country_master.tras.Rollback();
                    country_master.conn.Close();
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    ModelState.AddModelError("", ex.InnerException.Message);
                    return View(model);
                }
            }
            else
                return RedirectToAction("Login", "mUserMasters");
        }

        [HttpPost]
        public ActionResult GetCountryCode(int iCountryID)
        {
            List<CountryMaster> CountryMaster_ListAll = country_master.CountryMaster_ListAll(iCountryID, "", -1, "", false, "", -1).ToList();
            return Json(CountryMaster_ListAll, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string GetCountryCodeString(int iCountryID)
        {
            string CountryCode = "";
            List<CountryMaster> CountryMaster_ListAll = country_master.CountryMaster_ListAll(iCountryID, "", -1, "", false, "", -1).ToList();
            CountryCode = CountryMaster_ListAll[0].CountryCode + "," + CountryMaster_ListAll[0].IsNationality
                            + "," + CountryMaster_ListAll[0].RiskLevel + "," + CountryMaster_ListAll[0].Sanction;
            return CountryCode;
        }

        [HttpPost]
        public JsonResult GetCityData(int iCountryID)
        {
            var result = city_mater.CityMaster_ListAll(0, iCountryID, "", -1, "", false, "").ToList();
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult Getindustrydata(int iIndustryID)
        {
            var Data = indusrty_master.IndustryMaster_ListAll(iIndustryID, 0, "", 1, false, "").ToList();
            List<CountryMaster> IndustryMaster_ListAll = indusrty_master.IndustryMaster_ListAll(0, iIndustryID, "", -1, false, "").ToList();
            return Json(IndustryMaster_ListAll, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FAQ()
        {
            try
            {
                string[] LoginStatus = fn.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    ViewBag.fq = faq.FaqMaster_ListAll(0, 0, 1, "", false, "").ToList();
                    return View(ViewBag.fq);
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                string ErrorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(ErrorMessage);
            }
            return View();
        }
        public ActionResult CreateFAQ(int faqId = 0, int faqdId = 0)
        {
            try
            {
                string[] LoginStatus = fn.Checkcredentials();
                FAQ FQ = new FAQ();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (faqId > 0)
                    {
                        FQ = faq.FaqMaster_ListAll(faqId, 0, 1, "", false, "").FirstOrDefault();
                        BindFAQDropDown(FQ.CustomerTypeID);
                        return View(FQ);
                    }
                    else if (faqdId > 0)
                    {
                        faq.conn = ClsAppDatabase.GetCon();
                        if (faq.conn.State == ConnectionState.Closed)
                        { faq.conn.Open(); }
                        else { faq.conn.Close(); faq.conn.Open(); }
                        faq.tras = faq.conn.BeginTransaction();

                        faq.FaqMaster_Delete(faqdId);

                        faq.tras.Commit();
                        faq.conn.Close();

                        return RedirectToAction("FAQ");
                    }
                    if (faqId == 0 && faqdId == 0)
                    { BindFAQDropDown(0); }
                    return View(FQ);
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                faq.tras.Rollback(); faq.conn.Close();
                string ErrorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(ErrorMessage);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(FormCollection frm, CountryMaster result, string Action)
        {
            ClsCountryMaster cm = new ClsCountryMaster();
            ClsCityMaster cm2 = new ClsCityMaster();
            string CityName = frm["textid"];
            string city = frm[txtCity];
            string CityCode = frm["textid1"];
            int CityID = Convert.ToInt32(frm["CityID"]);
            int CountryID = Convert.ToInt32(frm["CountryID2"]);

            city_mater.conn = ClsAppDatabase.GetCon();
            if (city_mater.conn.State == ConnectionState.Closed)
            { city_mater.conn.Open(); }
            else { city_mater.conn.Close(); city_mater.conn.Open(); }
            city_mater.tras = city_mater.conn.BeginTransaction();
            try
            {
                if (CityID == null || CityID == 0)
                {
                    var Data = city_mater.Citymaster_add(CityID, CityCode, CityName, CountryID, 1, fn.GetSystemIP());
                    ViewBag.CountryID = new SelectList(cm.CountryMaster_ListAll(0, "", -1, "", false, "", -1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                    ViewBag.cityID = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    ViewBag.CityCode = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    city_mater.tras.Commit();
                    city_mater.conn.Close();
                    return RedirectToAction("Index", new { msg = "1", a = "UPDATE" });
                }
                else if (Request["btncountryvalDelete"] != null)
                {
                    var DeletedCity = city_mater.CityMaster_Delete(CityID, 1, fn.GetSystemIP());
                    ViewBag.CountryID = new SelectList(cm.CountryMaster_ListAll(0, "", 1, "", false, "", 1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                    ViewBag.cityID = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    ViewBag.CityCode = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    city_mater.tras.Commit();
                    city_mater.conn.Close();
                    return RedirectToAction("Index", new { msg = "3", a = "UPDATE" });
                }
                else
                {
                    var Data = city_mater.Citymaster_Update(CityID, CityCode, CityName, CountryID, 1, fn.GetSystemIP());
                    ViewBag.CountryID = new SelectList(cm.CountryMaster_ListAll(0, "", 1, "", false, "", 1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                    ViewBag.cityID = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    ViewBag.CityCode = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    city_mater.tras.Commit();
                    city_mater.conn.Close();
                    return RedirectToAction("Index", new { msg = "2", a = "UPDATE" });
                }
            }
            catch (Exception ex)
            {
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (city_mater.conn.State == ConnectionState.Open)
                {
                    city_mater.tras.Rollback();
                    city_mater.conn.Close();
                }
                ViewBag.ErrorMsgcity = "";
                string strErrorMsg = "";
                if (ex.InnerException == null)
                    strErrorMsg = ex.Message;
                else
                    strErrorMsg = ex.InnerException.Message;

                ViewBag.ErrorMsg = strErrorMsg;
                return RedirectToAction("Index", "Country", new { ErrorMessagecity = strErrorMsg });
            }
        }
        [HttpPost]
        public ActionResult AddCountry(FormCollection frm, CountryMaster result, string Action)
        {
            ClsCountryMaster cm = new ClsCountryMaster();
            ConString con = new ConString();
            ClsCityMaster cm2 = new ClsCityMaster();
            ClsStateMaster state_master = new ClsStateMaster();
            string CountryCode = frm["textidcon"];
            string CountryName = frm["textidconN"];
            int RiskLevel = Convert.ToInt16(frm["txtRiskLevel"]);
            string Sanction = frm["txtSanction"];

            int CountryID = 0;
            int.TryParse(Convert.ToString(frm["CountryID"]), out CountryID);
            country_master.conn = ClsAppDatabase.GetCon();
            if (country_master.conn.State == ConnectionState.Closed)
            { country_master.conn.Open(); }
            else { country_master.conn.Close(); country_master.conn.Open(); }
            country_master.tras = country_master.conn.BeginTransaction();
            bool IsNationality;
            if (frm.AllKeys.Contains("chkIsNAt"))
            { IsNationality = true; }
            else
            { IsNationality = false; }
            try
            {
                if (CountryID == null || CountryID == 0)
                {
                    int intCountryID = country_master.CountryMaster_Add(CountryID, CountryCode, CountryName, IsNationality, 1, fn.GetSystemIP(), RiskLevel, Sanction);
                    country_master.tras.Commit();
                    country_master.conn.Close();

                    ViewBag.CountryID = new SelectList(cm.CountryMaster_ListAll(0, "", -1, "", false, "", -1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                    ViewBag.cityID = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    ViewBag.CountryCode = new SelectList(cm.CountryMaster_ListAll(0, "", -1, "", false, "", -1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");

                    return RedirectToAction("Index", new { msg = "1", a = "countrymsg" });
                }
                else if (Request["btncountryDelete"] != null)
                {
                    int DeletedCountry = country_master.CountryMaster_Delete(CountryID, 1, fn.GetSystemIP());
                    country_master.tras.Commit();
                    country_master.conn.Close();

                    ViewBag.CountryID = new SelectList(cm.CountryMaster_ListAll(0, "", 1, "", false, "", 1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                    ViewBag.cityID = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    ViewBag.CountryCode = new SelectList(cm.CountryMaster_ListAll(0, "", -1, "", false, "", -1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");

                    return RedirectToAction("Index", new { msg = "3", a = "countrymsg" });
                }
                else
                {
                    int intCountryID = country_master.CountryMaster_Update(CountryID, CountryCode, CountryName, IsNationality, 1, fn.GetSystemIP(), RiskLevel, Sanction);
                    country_master.tras.Commit();
                    country_master.conn.Close();

                    ViewBag.CountryID = new SelectList(cm.CountryMaster_ListAll(0, "", 1, "", false, "", 1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");
                    ViewBag.cityID = new SelectList(cm2.CityMaster_ListAll(0, 0, "", 1, "", false, "").OrderBy(x => x.CityName).ToList(), "CityID", "CityName");
                    ViewBag.CountryCode = new SelectList(cm.CountryMaster_ListAll(0, "", -1, "", false, "", -1).OrderBy(x => x.CountryName).ToList(), "CountryID", "CountryName");

                    return RedirectToAction("Index", new { msg = "2", a = "countrymsg" });
                }
            }
            catch (Exception ex)
            {
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (country_master.conn.State == ConnectionState.Open)
                {
                    country_master.tras.Rollback();
                    country_master.conn.Close();
                }
                ViewBag.ErrorMsgcountry = "";
                string strErrorMsg = "";
                if (ex.InnerException == null)
                    strErrorMsg = ex.Message;
                else
                    strErrorMsg = ex.InnerException.Message;

                ViewBag.ErrorMsg = strErrorMsg;
                return RedirectToAction("Index", "Country", new { ErrorMessagecountry = strErrorMsg });
            }
        }
        [HttpPost]

        public ActionResult Industry(FormCollection frm, CountryMaster result, string Action)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                int IndustryID = 0;
                int.TryParse(Convert.ToString(frm["IndustryID"]), out IndustryID);
                string IndustryName = frm["industry"];
                indusrty_master.conn = ClsAppDatabase.GetCon();
                if (indusrty_master.conn.State == ConnectionState.Closed)
                { indusrty_master.conn.Open(); }
                else { indusrty_master.conn.Close(); indusrty_master.conn.Open(); }
                indusrty_master.tras = indusrty_master.conn.BeginTransaction();
                try
                {
                    if (IndustryID == 0)
                    {
                        var Data1 = indusrty_master.industry_add(IndustryID, 0, IndustryName, "", "", UserId, fn.GetSystemIP());
                        ViewBag.IndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 1, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        indusrty_master.tras.Commit();
                        indusrty_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "indusrty2" });
                    }
                    else if (Request["btnindufromDelete"] != null)
                    {
                        var Data1 = indusrty_master.IndustryMaster_Delete(IndustryID, UserId, fn.GetSystemIP());
                        ViewBag.IndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 1, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        indusrty_master.tras.Commit();
                        indusrty_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "indusrty2" });

                    }
                    else
                    {
                        var data = indusrty_master.IndustryMaster_ListAll(IndustryID, -1, "", -1, false, "").ToList();
                        var Data1 = indusrty_master.industry_update(IndustryID, 0, IndustryName, data[0].IndustryDesc, data[0].ClassificationNo, UserId, fn.GetSystemIP());
                        ViewBag.IndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 1, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        indusrty_master.tras.Commit();
                        indusrty_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "indusrty2" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (indusrty_master.conn.State == ConnectionState.Open)
                    {
                        indusrty_master.tras.Rollback();
                        indusrty_master.conn.Close();
                    }
                    ViewBag.ErrorMsgind = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;
                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Index", "Country", new { ErrorMessageind = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult SubIndustry(FormCollection frm, CountryMaster result, string Action)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);

                int IndustryID = Convert.ToInt32(frm["SubIndustryID"]);
                int parentIndustryID = Convert.ToInt32(frm["SubIndustryID1"]);
                string IndustryName = frm["industrynew"];
                indusrty_master.conn = ClsAppDatabase.GetCon();
                if (indusrty_master.conn.State == ConnectionState.Closed)
                { indusrty_master.conn.Open(); }
                else { indusrty_master.conn.Close(); indusrty_master.conn.Open(); }
                indusrty_master.tras = indusrty_master.conn.BeginTransaction();
                try
                {
                    if (IndustryID == 0)
                    {
                        var Data1 = indusrty_master.industry_add(IndustryID, parentIndustryID, IndustryName, "", "", UserId, fn.GetSystemIP());
                        ViewBag.SubIndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.SubIndustryID1 = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");

                        indusrty_master.tras.Commit();
                        indusrty_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "indusrty" });
                    }
                    else if (Request["btnsubformDelete"] != null)
                    {
                        var Data1 = indusrty_master.IndustryMaster_Delete(IndustryID, UserId, fn.GetSystemIP());
                        ViewBag.SubIndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.SubIndustryID1 = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        indusrty_master.tras.Commit();
                        indusrty_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "indusrty" });
                    }
                    else
                    {
                        var data = indusrty_master.IndustryMaster_ListAll(IndustryID, -1, "", -1, false, "").ToList();
                        var Data1 = indusrty_master.industry_update(IndustryID, parentIndustryID, IndustryName, data[0].IndustryDesc, data[0].ClassificationNo, UserId, fn.GetSystemIP());
                        ViewBag.SubIndustryID = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        ViewBag.SubIndustryID1 = new SelectList(indusrty_master.IndustryMaster_ListAll(0, 0, "", -1, false, "").OrderBy(x => x.IndustryName).ToList(), "IndustryID", "IndustryName");
                        indusrty_master.tras.Commit();
                        indusrty_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "indusrty" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (indusrty_master.conn.State == ConnectionState.Open)
                    {
                        indusrty_master.tras.Rollback();
                        indusrty_master.conn.Close();
                    }
                    ex.ToString();
                    return RedirectToAction("Index", "Country", new { ErrorMessageind = ex.ToString() });
                    return View();
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        public ActionResult CompanyType(FormCollection frm, CountryMaster result, string Action)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);

                int CompanyTypeID = 0;
                int.TryParse(Convert.ToString(frm["CompanyTypeID"]), out CompanyTypeID);
                string CompanyName = frm["companytype"];

                company_type.conn = ClsAppDatabase.GetCon();
                if (company_type.conn.State == ConnectionState.Closed)
                { company_type.conn.Open(); }
                else { company_type.conn.Close(); company_type.conn.Open(); }
                company_type.tras = company_type.conn.BeginTransaction();
                try
                {
                    if (CompanyTypeID == 0)
                    {
                        var Data1 = company_type.Companytypemaster_add(CompanyTypeID, CompanyName, UserId, fn.GetSystemIP());
                        ViewBag.CompanyTypeID = new SelectList(company_type.CompanyTypeMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.CompanyTypeName).ToList(), "CompanyTypeID", "CompanyTypeName");
                        company_type.tras.Commit();
                        company_type.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "companytype1" });
                    }
                    else if (Request["btncomptypeDelete"] != null)
                    {
                        var Data1 = company_type.CompanyTypeMaster_Delete(CompanyTypeID, UserId, fn.GetSystemIP());
                        ViewBag.CompanyTypeID = new SelectList(company_type.CompanyTypeMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.CompanyTypeName).ToList(), "CompanyTypeID", "CompanyTypeName");
                        company_type.tras.Commit();
                        company_type.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "companytype1" });
                    }
                    else
                    {
                        var Data1 = company_type.Companytypemaster_Update(CompanyTypeID, CompanyName, UserId, fn.GetSystemIP());
                        ViewBag.CompanyTypeID = new SelectList(company_type.CompanyTypeMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.CompanyTypeName).ToList(), "CompanyTypeID", "CompanyTypeName");
                        company_type.tras.Commit();
                        company_type.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "companytype1" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (company_type.conn.State == ConnectionState.Open)
                    {
                        company_type.tras.Rollback();
                        company_type.conn.Close();
                    }
                    ViewBag.ErrorMsgtype = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;

                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Index", "Country", new { ErrorMessagetype = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult DesignationType(FormCollection frm, CountryMaster result)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                int DesignationID = 0;
                int.TryParse(Convert.ToString(frm["DesignationID"]), out DesignationID);
                string DesignationName = frm["designation"];
                designation_type.conn = ClsAppDatabase.GetCon();
                if (designation_type.conn.State == ConnectionState.Closed)
                { designation_type.conn.Open(); }
                else { designation_type.conn.Close(); designation_type.conn.Open(); }
                designation_type.tras = designation_type.conn.BeginTransaction();
                try
                {
                    if (DesignationID == 0)
                    {
                        var Data1 = designation_type.DesignationMaster_Add(DesignationID, DesignationName, 1, "192.168.1.1");
                        ViewBag.DesignationID = new SelectList(designation_type.DesignationMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                        designation_type.tras.Commit();
                        designation_type.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "designation1" });
                    }
                    else if (Request["btndesignationDelete"] != null)
                    {
                        var Data1 = designation_type.DesignationMaster_Delete(DesignationID, UserId, fn.GetSystemIP());
                        ViewBag.DesignationID = new SelectList(designation_type.DesignationMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                        designation_type.tras.Commit();
                        designation_type.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "designation1" });
                    }
                    else
                    {
                        var Data1 = designation_type.DesignationMaster_Update(DesignationID, DesignationName, 1, "192.168.1.1");
                        ViewBag.DesignationID = new SelectList(designation_type.DesignationMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                        designation_type.tras.Commit();
                        designation_type.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "designation1" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (designation_type.conn.State == ConnectionState.Open)
                    {
                        designation_type.tras.Rollback();
                        designation_type.conn.Close();
                    }
                    ViewBag.ErrorMsgdes = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;

                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Index", "Country", new { ErrorMessagedes = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult Bank(FormCollection frm, CountryMaster result)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsBankMaster bankmaster = new ClsBankMaster();
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                int BankID = 0;
                int.TryParse(Convert.ToString(frm["BankID"]), out BankID);
                string BankName = frm["bankname"];
                bankmaster.conn = ClsAppDatabase.GetCon();
                if (bankmaster.conn.State == ConnectionState.Closed)
                { bankmaster.conn.Open(); }
                else { bankmaster.conn.Close(); bankmaster.conn.Open(); }
                bankmaster.tras = bankmaster.conn.BeginTransaction();
                try
                {
                    if (BankID == 0)
                    {
                        var Data1 = bankmaster.BankMaster_Add(BankID, BankName, "", "", "", 0, 0, 0, "", "", "", UserId, fn.GetSystemIP());
                        ViewBag.BankID = new SelectList(bankmaster.BankMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.BankName).ToList(), "BankID", "BankName");
                        bankmaster.tras.Commit();
                        bankmaster.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "bank" });
                    }
                    else if (Request["btnbankformDelete"] != null)
                    {
                        var Data1 = bankmaster.BankMaster_Delete(BankID, UserId, fn.GetSystemIP());
                        ViewBag.BankID = new SelectList(bankmaster.BankMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.BankName).ToList(), "BankID", "BankName");
                        bankmaster.tras.Commit();
                        bankmaster.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "bank" });
                    }
                    else
                    {
                        var data = bankmaster.BankMaster_ListAll(BankID, "", 0, 0, 0, -1, "", false, "").ToList();
                        var Data1 = bankmaster.BankMaster_Update(BankID, BankName, data[0].Address1, data[0].Address2, data[0].Address3, data[0].CityID, data[0].StateID, data[0].CountryID, data[0].ContactPerson, data[0].EmailID, data[0].MobileNo, UserId, fn.GetSystemIP());
                        ViewBag.BankID = new SelectList(bankmaster.BankMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.BankName).ToList(), "BankID", "BankName");
                        bankmaster.tras.Commit();
                        bankmaster.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "bank" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (bankmaster.conn.State == ConnectionState.Open)
                    {
                        bankmaster.tras.Rollback();
                        bankmaster.conn.Close();
                    }
                    ViewBag.ErrorMsgbank = "";
                    string strErrorMsg = "";

                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;

                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Index", "Country", new { ErrorMessagebank = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult Launguage(FormCollection frm, CountryMaster result)
        {
            int LanguageID = 0;
            int.TryParse(Convert.ToString(frm["LanguageID"]), out LanguageID);
            string LanguageName = frm["launguagename"];
            try
            {
                if (LanguageID == 0)
                {
                    var Data1 = language_master.LanguageMaster_Add(LanguageID, LanguageName, 1, 1, fn.GetSystemIP());
                    ViewBag.LanguageID = new SelectList(language_master.LanguageMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.LanguageName).ToList(), "LanguageID", "LanguageName");
                    return RedirectToAction("Index", new { msg = "1", a = "launguage" });
                }
                else
                {
                    var Data1 = language_master.LanguageMaster_Update(LanguageID, LanguageName, 1, 1, fn.GetSystemIP());
                    ViewBag.LanguageID = new SelectList(language_master.LanguageMaster_ListAll(0, "", -1, "", false, "").OrderBy(x => x.LanguageName).ToList(), "LanguageID", "LanguageName");
                    return RedirectToAction("Index", new { msg = "2", a = "launguage" });
                }
            }
            catch (Exception ex)
            {
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                ex.ToString();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Currency(FormCollection frm, CountryMaster result, string Action)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                currencymaster.conn = ClsAppDatabase.GetCon();
                if (currencymaster.conn.State == ConnectionState.Closed)
                { currencymaster.conn.Open(); }
                else { currencymaster.conn.Close(); currencymaster.conn.Open(); }
                currencymaster.tras = currencymaster.conn.BeginTransaction();
                try
                {
                    int CurrencyID = 0;
                    int.TryParse(Convert.ToString(frm["CurrencyID"]), out CurrencyID);
                    string CurrencyName = frm["CurrencyName"];
                    string currencycode = frm["CurrencyCode1"].Trim();

                    if (CurrencyID == 0)
                    {
                        var Data1 = currencymaster.CurrencyMaster_Add(CurrencyID, currencycode, CurrencyName, "", "", UserId, fn.GetSystemIP());
                        ViewBag.CurrencyID = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, ""), "CurrencyID", "CurrencyName");
                        ViewBag.CurrencyCode = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, ""), "CurrencyID", "CurrencyCode");
                        currencymaster.tras.Commit();
                        currencymaster.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "btncurrency" });
                    }
                    else if (Request["btncurrnyformDelete"] != null)
                    {
                        var Data1 = currencymaster.CurrencyMaster_Delete(CurrencyID, UserId, fn.GetSystemIP());
                        ViewBag.CurrencyID = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, ""), "CurrencyID", "CurrencyName");
                        ViewBag.CurrencyCode = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, ""), "CurrencyID", "CurrencyCode");
                        currencymaster.tras.Commit();
                        currencymaster.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "btncurrency" });
                    }
                    else
                    {
                        var Data1 = currencymaster.CurrencyMaster_Update(CurrencyID, currencycode, CurrencyName, "", UserId, fn.GetSystemIP());
                        ViewBag.CurrencyID = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, ""), "CurrencyID", "CurrencyName");
                        ViewBag.CurrencyCode = new SelectList(currencymaster.CurrencyMaster_ListAll(0, "", "", -1, "", false, ""), "CurrencyID", "CurrencyCode");
                        currencymaster.tras.Commit();
                        currencymaster.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "btncurrency" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (currencymaster.conn.State == ConnectionState.Open)
                    {
                        currencymaster.tras.Rollback();
                        currencymaster.conn.Close();
                    }
                    ViewBag.ErrorMsg = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;

                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Index", "Country", new { ErrorMessage = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult Insurrance(FormCollection frm, CountryMaster result)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                int InsuranceProviderID = 0;
                int.TryParse(Convert.ToString(frm["InsuranceProviderID"]), out InsuranceProviderID);
                string insuranceProviderName = frm["insurance"];

                insurane_master.conn = ClsAppDatabase.GetCon();
                if (insurane_master.conn.State == ConnectionState.Closed)
                { insurane_master.conn.Open(); }
                else { insurane_master.conn.Close(); insurane_master.conn.Open(); }
                insurane_master.tras = insurane_master.conn.BeginTransaction();
                try
                {
                    if (InsuranceProviderID == 0)
                    {
                        var Data1 = insurane_master.InsuranceProviderMaster_Add(InsuranceProviderID, insuranceProviderName, "", "", "", 0, 0, 0, "", "", "", "", "", UserId, fn.GetSystemIP());
                        ViewBag.InsuranceProviderID = new SelectList(insurane_master.InsuranceProviderMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.InsuranceProviderID).ToList(), "InsuranceProviderID", "InsuranceProviderName");
                        insurane_master.tras.Commit();
                        insurane_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "insurance1" });
                    }
                    else if (Request["btninsformDelete"] != null)
                    {
                        var Data1 = insurane_master.InsuranceProviderMaster_Delete(InsuranceProviderID, UserId, fn.GetSystemIP());
                        ViewBag.InsuranceProviderID = new SelectList(insurane_master.InsuranceProviderMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.InsuranceProviderID).ToList(), "InsuranceProviderID", "InsuranceProviderName");
                        insurane_master.tras.Commit();
                        insurane_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "insurance1" });
                    }
                    else
                    {
                        var data = insurane_master.InsuranceProviderMaster_ListAll(InsuranceProviderID, "", 0, 0, 0, -1, "", false, "").ToList();
                        var Data1 = insurane_master.InsuranceProviderMaster_Update(InsuranceProviderID, insuranceProviderName, data[0].Address1, data[0].Address2, data[0].Address3, data[0].CityID, data[0].StateID, data[0].CountryID, data[0].ContactName, data[0].TelNo, data[0].EmailID, data[0].MobileNo, data[0].WebSite, UserId, fn.GetSystemIP());
                        ViewBag.InsuranceProviderID = new SelectList(insurane_master.InsuranceProviderMaster_ListAll(0, "", 0, 0, 0, -1, "", false, "").OrderBy(x => x.InsuranceProviderID).ToList(), "InsuranceProviderID", "InsuranceProviderName");
                        insurane_master.tras.Commit();
                        insurane_master.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "insurance1" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (insurane_master.conn.State == ConnectionState.Open)
                    {
                        insurane_master.tras.Rollback();
                        insurane_master.conn.Close();
                    }
                    ViewBag.ErrorMsgin = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;
                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Index", "Country", new { ErrorMessagein = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [AllowAnonymous]
        public string CheckCityCode(string input)
        {
            if (input != null && input != "")
            {
                var CityCode = city_mater.CityMaster_ListAll(0, 0, "", 1, "", false, "").FirstOrDefault();
                var CompanyName = "";

                if (CompanyName == null)
                { return "Available"; }
            }
            return "";
        }
        public ActionResult GetCityCode(int iCityID)
        {
            List<CountryMaster> CityMaster_ListAll = city_mater.CityMaster_ListAll(iCityID, 0, "", -1, "", false, "").ToList();
            return Json(CityMaster_ListAll, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Template(FormCollection frm, CountryMaster result, string Action)
        {
            ClsTemplate Template = new ClsTemplate();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                int TemplateID = 0;
                int.TryParse(Convert.ToString(frm["TemplateID"]), out TemplateID);
                string Name = frm["Name"];
                Template.conn = ClsAppDatabase.GetCon();
                if (Template.conn.State == ConnectionState.Closed)
                { Template.conn.Open(); }
                else { Template.conn.Close(); Template.conn.Open(); }
                Template.tras = Template.conn.BeginTransaction();
                try
                {
                    if (TemplateID == 0)
                    {
                        var Data1 = Template.Template_Add(TemplateID, Name, UserId, fn.GetSystemIP());
                        ViewBag.TemplateID = new SelectList(TemplateObj.Template_ListAll(0, "").OrderBy(x => x.Name).ToList(), "TemplateID", "Name");
                        Template.tras.Commit();
                        Template.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "btnTemplate" });
                    }
                    else if (Request["TemplateFormDelete"] != null)
                    {
                        var Data1 = Template.Template_Delete(TemplateID);
                        ViewBag.InsuranceProviderID = new SelectList(Template.Template_ListAll(0, "").OrderBy(x => x.TemplateID).ToList(), "TemplateID", "Name");
                        Template.tras.Commit();
                        Template.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "btnTemplate" });
                    }
                    else
                    {
                        var Data1 = Template.Template_Update(TemplateID, Name, UserId, fn.GetSystemIP());
                        ViewBag.TemplateID = new SelectList(TemplateObj.Template_ListAll(0, "").OrderBy(x => x.Name).ToList(), "TemplateID", "Name");
                        Template.tras.Commit();
                        Template.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "btnTemplate" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (Template.conn.State == ConnectionState.Open)
                    {
                        Template.tras.Rollback();
                        Template.conn.Close();
                    }
                    ViewBag.ErrorMsgin = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;
                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Index", "Country", new { ErrorMessagein = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult DynamicText(FormCollection frm, CountryMaster result, string Action)
        {
            ClsTemplate Template = new ClsTemplate();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                int DynamicTextID = 0;
                int.TryParse(Convert.ToString(frm["DynamicTextID"]), out DynamicTextID);
                string DynamicTextName = frm["DynamicTextName"];
                Template.conn = ClsAppDatabase.GetCon();
                if (Template.conn.State == ConnectionState.Closed)
                { Template.conn.Open(); }
                else { Template.conn.Close(); Template.conn.Open(); }
                Template.tras = Template.conn.BeginTransaction();
                try
                {
                    if (DynamicTextID == 0)
                    {
                        var Data1 = Template.HtmlDynamicTemplateField_Add(DynamicTextID, DynamicTextName, UserId, fn.GetSystemIP());
                        ViewBag.DynamicTextID = new SelectList(TemplateObj.HtmlDynamicText_ListAll(0, "", false, "").OrderBy(x => x.DynamicTextName).ToList(), "DynamicTextID", "DynamicTextName");
                        Template.tras.Commit();
                        Template.conn.Close();
                        return RedirectToAction("Index", new { msg = "1", a = "btnDynamicTemplate" });
                    }
                    else if (Request["DynamicTextTemplateFormDelete"] != null)
                    {
                        var Data1 = Template.mHtmlDynamicTemplateField_Delete(DynamicTextID);
                        ViewBag.DynamicTextID = new SelectList(TemplateObj.HtmlDynamicText_ListAll(0, "", false, "").OrderBy(x => x.DynamicTextName).ToList(), "DynamicTextID", "DynamicTextName");
                        Template.tras.Commit();
                        Template.conn.Close();
                        return RedirectToAction("Index", new { msg = "3", a = "btnDynamicTemplate" });
                    }
                    else
                    {
                        var Data1 = Template.HtmlDynamicTemplateField_Update(DynamicTextID, DynamicTextName, UserId, fn.GetSystemIP());
                        ViewBag.DynamicTextID = new SelectList(TemplateObj.HtmlDynamicText_ListAll(0, "", false, "").OrderBy(x => x.DynamicTextName).ToList(), "DynamicTextID", "DynamicTextName");
                        Template.tras.Commit();
                        Template.conn.Close();
                        return RedirectToAction("Index", new { msg = "2", a = "btnDynamicTemplate" });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (Template.conn.State == ConnectionState.Open)
                    {
                        Template.tras.Rollback();
                        Template.conn.Close();
                    }
                    ViewBag.ErrorMsgin = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;
                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Index", "Country", new { ErrorMessagein = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult CreateEmail(int EmailConfigID = 0, int DeleteID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (Request.QueryString["ErrorMessagein"] != null && Request.QueryString["ErrorMessagein"] != "")
                { ViewBag.ErrorMsg = Request.QueryString["ErrorMessagein"].ToString(); }
                CountryMaster result = new CountryMaster();
                ClsEmailConfiguration obj = new ClsEmailConfiguration();
                if (EmailConfigID != 0)
                {
                    var data = obj.EmailConfiguration_ListAll(EmailConfigID, "", false, "").FirstOrDefault();
                    return View(data);
                }
                if (DeleteID != 0)
                {
                    obj.conn = ClsAppDatabase.GetCon();
                    if (obj.conn.State == ConnectionState.Closed)
                    { obj.conn.Open(); }
                    else { obj.conn.Close(); obj.conn.Open(); }
                    obj.tras = obj.conn.BeginTransaction();

                    var data = obj.EmailConfiguration_Delete(DeleteID);
                    obj.tras.Commit();
                    obj.conn.Close();
                    return RedirectToAction("CreateEmail", "Country", new { EmailConfigID = 0, DeleteID = 0 });
                }
                return View(result);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public string CreateEmailDelete(int EmailConfigID = 0, int DeleteID = 0)
        {
            string resp = "";
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CountryMaster result = new CountryMaster();
                ClsEmailConfiguration obj = new ClsEmailConfiguration();
                try
                {
                    int LoginUserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out LoginUserId);
                    if (DeleteID != 0)
                    {
                        obj.conn = ClsAppDatabase.GetCon();
                        if (obj.conn.State == ConnectionState.Closed)
                        { obj.conn.Open(); }
                        else { obj.conn.Close(); obj.conn.Open(); }
                        obj.tras = obj.conn.BeginTransaction();

                        var data = obj.EmailConfiguration_Delete(DeleteID);
                        obj.tras.Commit();
                        obj.conn.Close();
                        resp = "Successfully";
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    obj.tras.Rollback();
                    obj.conn.Close();
                    resp = "Error";
                }
                return resp;
            }
            else
            { return resp; }
        }

        [HttpPost]
        public ActionResult CreateEmail(FormCollection frm, CountryMaster emd, int idEdit = 0, int EmailConfigID = 0, int idDelete = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            ClsEmailConfiguration obj = new ClsEmailConfiguration();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    int.TryParse(Convert.ToString(frm["EmailConfigID"]), out EmailConfigID);
                    string smtpname = frm["smtpname"];
                    string username = frm["username"];
                    string psd = frm["psd"];
                    string fromemailid = frm["fromemailid"];
                    string ccemailid = frm["ccemailid"];
                    string bccemailid = frm["bccemailid"];
                    int inport = Convert.ToInt32(frm["inport"]);
                    int outport = Convert.ToInt32(frm["outport"]);

                    obj.conn = ClsAppDatabase.GetCon();
                    if (obj.conn.State == ConnectionState.Closed)
                    { obj.conn.Open(); }
                    else { obj.conn.Close(); obj.conn.Open(); }
                    obj.tras = obj.conn.BeginTransaction();
                    if (EmailConfigID == 0)
                    {
                        var data = obj.EmailConfiguration_Add(EmailConfigID, smtpname, username, psd, fromemailid, ccemailid, bccemailid, "", inport, outport, 1, fn.GetSystemIP());
                        obj.tras.Commit();
                        obj.conn.Close();

                        return RedirectToAction("CreateEmail", "Country", new { EmailConfigID, idEdit });
                    }
                    else
                    {
                        var data = obj.EmailConfiguration_Update(EmailConfigID, smtpname, username, psd, fromemailid, ccemailid, bccemailid, "", inport, outport, 1, fn.GetSystemIP());
                        obj.tras.Commit();
                        obj.conn.Close();

                        return RedirectToAction("CreateEmail", "Country");
                    }

                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);

                    if (obj.conn.State == ConnectionState.Open)
                    {
                        obj.tras.Rollback();
                        obj.conn.Close();
                    }
                    ViewBag.ErrorMsgin = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;
                    ViewBag.ErrorMsgin = strErrorMsg;
                    return RedirectToAction("CreateEmail", "Country", new { ErrorMessagein = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult Edit(int EmailConfigID)
        { return RedirectToAction("CreateEmail", "Country"); }
        public ActionResult EmailDetail(CountryMaster emd)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsEmailConfiguration obj = new ClsEmailConfiguration();
                var data = obj.EmailConfiguration_ListAll(0, "", false, "").ToList();
                return PartialView(data);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public string SMSConfigDelete(int SMSConfigID = 0, int DeleteID = 0)
        {
            string resp = "";
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsSMSConfig obj = new ClsSMSConfig();
                try
                {
                    int LoginUserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out LoginUserId);
                    if (DeleteID != 0)
                    {
                        obj.conn = ClsAppDatabase.GetCon();
                        if (obj.conn.State == ConnectionState.Closed)
                        { obj.conn.Open(); }
                        else { obj.conn.Close(); obj.conn.Open(); }
                        obj.tras = obj.conn.BeginTransaction();

                        var data = obj.SMSConfiguration_Delete(DeleteID);
                        obj.tras.Commit();
                        obj.conn.Close();

                        resp = "Successfully";
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    obj.tras.Rollback();
                    obj.conn.Close();
                    resp = "Error";
                }
                return resp;
            }
            else
            { return resp; }
        }
        public ActionResult SMSConfig(int SMSConfigID = 0, int DeleteID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (Request.QueryString["ErrorMessagein"] != null && Request.QueryString["ErrorMessagein"] != "")
                { ViewBag.ErrorMsg = Request.QueryString["ErrorMessagein"].ToString(); }
                CountryMaster result = new CountryMaster();
                ClsSMSConfig obj = new ClsSMSConfig();
                if (SMSConfigID != 0)
                {
                    var data = obj.SMSConfiguration_ListAll(SMSConfigID, false, "").FirstOrDefault();
                    return View(data);
                }
                return View(result);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult SMSConfig(FormCollection frm, CountryMaster emd, int idEdit = 0, int SMSConfigID = 0, int idDelete = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            ClsSMSConfig obj = new ClsSMSConfig();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    int.TryParse(Convert.ToString(frm["SMSConfigID"]), out SMSConfigID);
                    string url = frm["url"];
                    string username = frm["username"];
                    string psd = frm["psd"];
                    obj.conn = ClsAppDatabase.GetCon();
                    if (obj.conn.State == ConnectionState.Closed)
                    { obj.conn.Open(); }
                    else { obj.conn.Close(); obj.conn.Open(); }
                    obj.tras = obj.conn.BeginTransaction();
                    if (SMSConfigID == 0)
                    {
                        var data = obj.SMSConfiguration_Add(SMSConfigID, url, username, psd, 1, fn.GetSystemIP());
                        obj.tras.Commit();
                        obj.conn.Close();

                        return RedirectToAction("SMSConfig", "Country", new { SMSConfigID, idEdit });
                    }
                    else
                    {
                        var data = obj.SMSConfiguration_Update(SMSConfigID, url, username, psd, 1, fn.GetSystemIP());
                        obj.tras.Commit();
                        obj.conn.Close();

                        return RedirectToAction("SMSConfig", "Country");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);

                    if (obj.conn.State == ConnectionState.Open)
                    {
                        obj.tras.Rollback();
                        obj.conn.Close();
                    }
                    ViewBag.ErrorMsgin = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;
                    ViewBag.ErrorMsgin = strErrorMsg;
                    return RedirectToAction("SMSConfig", "Country", new { ErrorMessagein = strErrorMsg });
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult SMSDetail(CountryMaster emd)
        {
            ClsSMSConfig obj = new ClsSMSConfig();
            var data = obj.SMSConfiguration_ListAll(0, false, "").ToList();
            return PartialView(data);
        }
        [HttpPost]
        public ActionResult CreateFAQ(FormCollection frm, CountryMaster fq)
        {
            try
            {
                string[] LoginStatus = fn.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    bool isActive = String.IsNullOrEmpty(Request["IsActive"]) ? false : true;

                    faq.conn = ClsAppDatabase.GetCon();
                    if (faq.conn.State == ConnectionState.Closed)
                    { faq.conn.Open(); }
                    else { faq.conn.Close(); faq.conn.Open(); }
                    faq.tras = faq.conn.BeginTransaction();
                    if (fq.FaqID == 0)
                    {
                        int faqId = 0;
                        faq.FaqMaster_Add(faqId, fq.CustomerTypeID, fq.Questions, fq.Answer, 1, fn.GetSystemIP());
                        faq.tras.Commit();
                        faq.conn.Close();
                    }
                    else
                    {
                        faq.FaqMaster_Update(fq.FaqID, fq.CustomerTypeID, fq.Questions, fq.Answer, isActive, 1, fn.GetSystemIP());
                        faq.tras.Commit();
                        faq.conn.Close();
                    }

                    return RedirectToAction("FAQ");
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
            }
            catch (Exception ex)
            {
                BindFAQDropDown(fq.CustomerTypeID);
                ViewBag.ErrorMessage = ex.Message;
                faq.tras.Rollback(); faq.conn.Close();
                string ErrorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(ErrorMessage);
                return View();
            }
        }
        public void BindFAQDropDown(int FAQID)
        {
            List<SelectListItem> item = new List<SelectListItem>();
            item.Add(new SelectListItem { Text = "All", Value = "0" });
            item.Add(new SelectListItem { Text = "Buyers", Value = "4" });
            item.Add(new SelectListItem { Text = "Funder", Value = "7" });
            item.Add(new SelectListItem { Text = "Exporter", Value = "6" });
            item.Add(new SelectListItem { Text = "Investor", Value = "9" });
            item.Add(new SelectListItem { Text = "Obligors", Value = "3" });
            item.Add(new SelectListItem { Text = "Admin", Value = "8" });
            item.Add(new SelectListItem { Text = "Suppliers", Value = "5" });
            if (FAQID == 0)
            { ViewBag.CustomerTypeID = new SelectList((item).ToList(), "Value", "Text"); }
            else
            { ViewBag.CustomerTypeID = new SelectList((item).ToList(), "Value", "Text", FAQID); }
        }
        public ActionResult CreateHolidayMaster(int HolidayID = 0, int DeleteID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (Request.QueryString["ErrorMessagein"] != null && Request.QueryString["ErrorMessagein"] != "")
                { ViewBag.ErrorMsg = Request.QueryString["ErrorMessagein"].ToString(); }
                CountryMaster result = new CountryMaster();
                ClsHolidayMaster obj = new ClsHolidayMaster();
                if (HolidayID != 0)
                {
                    var data = obj.HolidayMaster_ListAll(HolidayID).FirstOrDefault();
                    return View(data);
                }
                if (DeleteID != 0)
                {
                    obj.conn = ClsAppDatabase.GetCon();
                    if (obj.conn.State == ConnectionState.Closed)
                    { obj.conn.Open(); }
                    else { obj.conn.Close(); obj.conn.Open(); }
                    obj.tras = obj.conn.BeginTransaction();

                    var data = obj.HolidayMaster_Delete(DeleteID);
                    obj.tras.Commit();
                    obj.conn.Close();
                    return RedirectToAction("CreateHolidayMaster", "Country", new { HolidayID = 0, DeleteID = 0 });
                }
                return View(result);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public string CreateHolidayMasterDelete(int HolidayID = 0, int DeleteID = 0)
        {
            string resp = "";
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CountryMaster result = new CountryMaster();
                ClsHolidayMaster obj = new ClsHolidayMaster();
                try
                {
                    int LoginUserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out LoginUserId);
                    if (DeleteID != 0)
                    {
                        obj.conn = ClsAppDatabase.GetCon();
                        if (obj.conn.State == ConnectionState.Closed)
                        { obj.conn.Open(); }
                        else { obj.conn.Close(); obj.conn.Open(); }
                        obj.tras = obj.conn.BeginTransaction();

                        var data = obj.HolidayMaster_Delete(DeleteID);
                        obj.tras.Commit();
                        obj.conn.Close();

                        resp = "Successfully";
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    obj.tras.Rollback();
                    obj.conn.Close();
                    resp = "Error";
                }
                return resp;
            }
            else
            { return resp; }
        }

        [HttpPost]
        public ActionResult CreateHolidayMaster(FormCollection frm, CountryMaster emd, int idEdit = 0, int HolidayID = 0, int idDelete = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            ClsHolidayMaster obj = new ClsHolidayMaster();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    int.TryParse(Convert.ToString(frm["HolidayID"]), out HolidayID);
                    string HolidayName = frm["HolidayName"];
                    DateTime FromHolidayDate = Convert.ToDateTime(frm["FromHolidayDate"]);
                    DateTime ToHolidayDate = Convert.ToDateTime(frm["ToHolidayDate"]);

                    obj.conn = ClsAppDatabase.GetCon();
                    if (obj.conn.State == ConnectionState.Closed)
                    { obj.conn.Open(); }
                    else { obj.conn.Close(); obj.conn.Open(); }
                    obj.tras = obj.conn.BeginTransaction();
                    if (HolidayID == 0)
                    {
                        var data = obj.HolidayMaster_Add(HolidayID, HolidayName, FromHolidayDate, ToHolidayDate, 1, fn.GetSystemIP());
                        obj.tras.Commit();
                        obj.conn.Close();

                        return RedirectToAction("CreateHolidayMaster", "Country", new { HolidayID, idEdit });
                    }
                    else
                    {
                        var data = obj.HolidayMaster_Update(HolidayID, HolidayName, FromHolidayDate, ToHolidayDate, 1, fn.GetSystemIP());
                        obj.tras.Commit();
                        obj.conn.Close();
                        return RedirectToAction("CreateHolidayMaster", "Country");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);

                    if (obj.conn.State == ConnectionState.Open)
                    {
                        obj.tras.Rollback();
                        obj.conn.Close();
                    }
                    ViewBag.ErrorMsgin = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;
                    ViewBag.ErrorMsgin = strErrorMsg;
                    return RedirectToAction("CreateEmail", "Country", new { ErrorMessagein = strErrorMsg });
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult HolidayDetail(CountryMaster model)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsHolidayMaster obj = new ClsHolidayMaster();
                var data = obj.HolidayMaster_ListAll(0).ToList();
                return PartialView(data);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult ScreeningConfig(int CategoryID = 0, int dID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ScreeningConfig _objModel = new ScreeningConfig();
                if (CategoryID > 0)
                {
                    _objModel.CategoryID = CategoryID;
                    _objModel = country_master.CategoryMaster_ListAll(_objModel).FirstOrDefault();
                }
                else if (dID > 0)
                {
                    country_master.conn = ClsAppDatabase.GetCon();
                    if (country_master.conn.State == ConnectionState.Closed)
                    { country_master.conn.Open(); }
                    else { country_master.conn.Close(); country_master.conn.Open(); }
                    country_master.tras = country_master.conn.BeginTransaction();

                    _objModel.CategoryID = dID;
                    country_master.CategoryMaster_Delete(_objModel);

                    country_master.tras.Commit(); country_master.conn.Close();

                    return RedirectToAction("ScreeningConfig", "Country");

                }
                return View(_objModel);
            }
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult ScreeningConfig(FormCollection frm, ScreeningConfig _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {

                try
                {
                    country_master.conn = ClsAppDatabase.GetCon();
                    if (country_master.conn.State == ConnectionState.Closed)
                    { country_master.conn.Open(); }
                    else { country_master.conn.Close(); country_master.conn.Open(); }
                    country_master.tras = country_master.conn.BeginTransaction();

                    if (Request["catAdd"] != null)
                    {
                        if (_objModel.CategoryID > 0)
                        {
                            country_master.CategoryMaster_Update(_objModel);
                        }
                        else
                        {
                            _objModel.CategoryID = country_master.CategoryMaster_Add(_objModel);
                        }
                        country_master.tras.Commit(); country_master.conn.Close();

                        return RedirectToAction("ScreeningConfig", "Country");
                    }
                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View(_objModel);
            }
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        public PartialViewResult GridScreeningConfig()
        {
            ScreeningConfig _objModel = new ScreeningConfig();
            var Data = country_master.CategoryMaster_ListAll(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult GridCategoryDetail()
        {
            ScreeningConfig _objModel = new ScreeningConfig();
            var Data = country_master.CategoryMaster_ListAll(_objModel);
            _objModel.ParentCategoryDetID = -1;
            ViewBag.child = country_master.CategoryDetail_ListAll(_objModel);
            return PartialView(Data);
        }
        public ActionResult addCategoryChild(int CategoryID = 0, int CategoryDetID = 0, string Edit = "", string Delete = "")
        {
            try
            {
                ScreeningConfig _objModel = new ScreeningConfig();
                _objModel.CategoryID = CategoryID;
                _objModel.CategoryDetID = CategoryDetID;
                _objModel.EditRecord = Edit;
                if (Edit != "")
                {
                    _objModel.ParentCategoryDetID = -1;
                    _objModel = country_master.CategoryDetail_ListAll(_objModel).FirstOrDefault();
                    _objModel.EditRecord = Edit;
                }
                if (Delete != "")
                {
                    country_master.conn = ClsAppDatabase.GetCon();
                    if (country_master.conn.State == ConnectionState.Closed)
                    { country_master.conn.Open(); }
                    else { country_master.conn.Close(); country_master.conn.Open(); }
                    country_master.tras = country_master.conn.BeginTransaction();
                    country_master.CategoryDetail_Delete(_objModel);
                    country_master.tras.Commit(); country_master.conn.Close();

                }
                FillRiskCombo();
                return PartialView(_objModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult addCategoryChildPopUpPost(ScreeningConfig _objModel)
        {
            var data = "";
            try
            {
                country_master.conn = ClsAppDatabase.GetCon();
                if (country_master.conn.State == ConnectionState.Closed)
                { country_master.conn.Open(); }
                else { country_master.conn.Close(); country_master.conn.Open(); }
                country_master.tras = country_master.conn.BeginTransaction();
                if (_objModel.EditRecord == "Edit")
                {
                    country_master.CategoryDetail_Update(_objModel);
                }
                else
                {
                    _objModel.ParentCategoryDetID = _objModel.CategoryDetID;
                    country_master.CategoryDetail_Add(_objModel);
                }
                country_master.tras.Commit(); country_master.conn.Close();
                return Json(_objModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                country_master.conn.Close();
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                data = ex.Message.ToString();
                return Json(new { error = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public void FillRiskCombo()
        {
            ViewBag.DDLRiskID = new List<SelectListItem> { new SelectListItem() { Text = "Law", Value = "L" },
                                                     new SelectListItem() { Text = "Moderate", Value = "M" },
                                                     new SelectListItem() { Text = "High", Value = "H" },
                                                     new SelectListItem() { Text = "Very High", Value = "V" },
                                                     new SelectListItem() { Text = "Yes", Value = "Y" },
                                                     new SelectListItem() { Text = "No", Value = "N" }};

            ViewBag.DDLSelComment = new List<SelectListItem> { new SelectListItem() { Text = "No Comment", Value = "O" },
                                                     new SelectListItem() { Text = "Yes", Value = "Y" },
                                                     new SelectListItem() { Text = "No", Value = "N" }};
        }
    }
}