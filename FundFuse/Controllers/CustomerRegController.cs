using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;
using TMP.DAL;
using System.Data;
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;
using System.IO;
using System.Web.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TMP.Controllers
{
    public class CustomerRegController : Controller
    {
        #region Object

        ClsCustomerMaster _clsCust = new ClsCustomerMaster();
        ClsCountryMaster _clsContr = new ClsCountryMaster();
        ClsCompanyTypeMaster _clsCompType = new ClsCompanyTypeMaster();
        ClsIndustryMaster _clsInds = new ClsIndustryMaster();
        ClsDesignationMaster _clsDesig = new ClsDesignationMaster();
        ClsCurrency _clsCurry = new ClsCurrency();
        ClsCommon _clsComm = new ClsCommon();
        ClsBankMaster _clsBank = new ClsBankMaster();
        ClsUserMaster _clsUser = new ClsUserMaster();
        Function fn = new Function();
        UserMaster_ListAll_Result _objUserMaster = new UserMaster_ListAll_Result();
        UserRoleLinkMaster_ListAll_Result _objRolelink = new UserRoleLinkMaster_ListAll_Result();
        ServiceProviderModel _ObjServiceProvider = new ServiceProviderModel();
        ClsServiceProvider _ClsServiceProvider = new ClsServiceProvider();

        public string currentStatus
        {
            get
            {
                if (Request.QueryString["currentStatus"] != null && Request.QueryString["currentStatus"] != "")
                {
                    return Request.QueryString["currentStatus"].ToString();
                }
                return "";
            }
        }

        string apikey = System.Configuration.ConfigurationManager.AppSettings["apikey"];
        string apisecret = System.Configuration.ConfigurationManager.AppSettings["apisecret"];
        string group_id = System.Configuration.ConfigurationManager.AppSettings["group_id"];
        string gatewayhost = System.Configuration.ConfigurationManager.AppSettings["gatewayhost"];
        string gatewayurl = System.Configuration.ConfigurationManager.AppSettings["gatewayurl"];
        string Getrequestendpoint = System.Configuration.ConfigurationManager.AppSettings["requestendpoint"];
        int PlatformID = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["PlatformID"]);
        ClsThomsonReuters _ClsThomsonReuters = new ClsThomsonReuters();
        ThomsonReutersModel _ThomsonReutersModel = new ThomsonReutersModel();
        string requestendpoint = "";
        #endregion

        #region Client

        // GET: CustomerReg
        public ActionResult EditProfile()
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                _RightsNoaccessPage();
                CustomerMasterModel _objModel = new CustomerMasterModel();
                try
                {

                    int intCustomerID = Convert.ToInt32(Session["CustomerID"]);
                    int intCustomerType = Convert.ToInt32(Session["onlineCustomerTypeID"]);

                    if (intCustomerType == CONT.BuyerCustomerTypeID || intCustomerType == CONT.BothObligorAndBuyerTypeID || intCustomerType == CONT.ObligorCustomerTypeID)
                    {
                        _objModel.CustomerTypeIDs = CONT.BuyerCustomerTypeID + "," + CONT.BothObligorAndBuyerTypeID + "," + CONT.ObligorCustomerTypeID;
                    }
                    else
                    {
                        _objModel.CustomerTypeID = intCustomerType;
                    }

                    _objModel.CustomerID = intCustomerID;
                    var Data = _clsCust.CustomerMaster_ListAll(_objModel).FirstOrDefault();
                    if (Data != null)
                    {
                        if (Data.Status == CONT.RQ || Data.Status == CONT.NM || Data.Status == CONT.NA || Data.Status == CONT.UA || Data.Status == CONT.UM)
                        {
                            if (intCustomerType == CONT.ServiceProviderCustomerTypeID)
                            {
                                return RedirectToAction("CompanyInfo", "ServiceProvider", new { CustomerID = intCustomerID, IsEditProfile = true });
                            }
                            else if (intCustomerType == CONT.InsuranceProviderTypeID || intCustomerType == CONT.InsuranceBrokerTypeID)
                            {
                                return RedirectToAction("CompanyInfo", "InsuranceProvider", new { CustomerID = intCustomerID, IsEditProfile = true });
                            }
                            else
                            {
                                return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerID = intCustomerID, CustomerTypeID = intCustomerType, IsEditProfile = true });
                            }

                        }
                        else
                        {
                            if (intCustomerType == CONT.ServiceProviderCustomerTypeID)
                            {
                                return RedirectToAction("MakerDetails", "ServiceProvider", new { CustomerID = intCustomerID });
                            }
                            else if (intCustomerType == CONT.InsuranceProviderTypeID || intCustomerType == CONT.InsuranceBrokerTypeID)
                            {
                                return RedirectToAction("CompanyInfoView", "InsuranceProvider", new { CustomerID = intCustomerID, CustomerTypeID = intCustomerType, IsEditProfile = true });
                            }
                            else
                            {
                                return RedirectToAction("CompanyInfoDetails", "CustomerReg", new { CustomerID = intCustomerID, currentStatus = Data.Status, IsEditProfile = true });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
            return View();
        }
        public ActionResult CompanyInfo(int CustomerID = 0, int CustomerTypeID = 0, string ProgramType = "", bool IsEditProfile = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                _RightsNoaccessPage();
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                {
                    Session["IsEditProfile"] = true;
                }
                else { Session["IsEditProfile"] = false; }
                CustomerMasterModel _objModel = new CustomerMasterModel();
                try
                {
                    _objModel.CustomerID = CustomerID;
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.Status = "";
                    if (_objModel.CustomerID > 0)
                    {
                        _objModel = _clsCust.CustomerMasterHistory_ListAllBind(_objModel).FirstOrDefault();
                        _objModel.GetCityID = _objModel.CityID;
                        Session["ProgramType"] = _objModel.ProgramType;
                        if (_objModel.OtherIndustry == "") { _objModel.OtherSubIndustry_Sub = _objModel.OtherSubIndustry; }
                    }

                    FillProgramTypeCombo(); FillCountryCombo();
                    FillCompanyTypeCombo(); FillIndustryCombo();
                    FillDesignationCombo(); FillGenderCombo();
                    return View(_objModel);
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo();
                FillCompanyTypeCombo();
                FillIndustryCombo();
                FillDesignationCombo();
                FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult CompanyInfo(CustomerMasterModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    if (_objModel.OtherIndustry == null) { _objModel.OtherSubIndustry = _objModel.OtherSubIndustry_Sub; }
                    if (_objModel.CustomerID > 0)
                    {
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                        {
                            _objModel.Status = CONT.UR; _clsCust.CustomerMasterHistory_Add(_objModel);
                        }
                        else
                        { _clsCust.CustomerMaster_Update(_objModel); }
                    }
                    else
                    { _objModel.CustomerID = _clsCust.CustomerMaster_Add(_objModel); }
                    _clsCust.Tras.Commit(); _clsCust.Conn.Close();

                    if (_objModel.CustomerTypeID > 0)
                    {
                        if (_objModel.CustomerTypeID == CONT.ObligorCustomerTypeID)
                            Session["ProgramType"] = "F";
                        else if (_objModel.CustomerTypeID == CONT.BuyerCustomerTypeID)
                            Session["ProgramType"] = "R";
                        else if (_objModel.CustomerTypeID == CONT.BothObligorAndBuyerTypeID)
                            Session["ProgramType"] = "B";
                    }

                    if (Request["Save"] != null)
                        return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerID = _objModel.CustomerID, CustomerTypeID = _objModel.CustomerTypeID });
                    else if (Request["NextSave"] != null)
                        return RedirectToAction("CompanyMoreInfo", "CustomerReg", new { CustomerID = _objModel.CustomerID, CustomerTypeID = _objModel.CustomerTypeID });
                }
                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillProgramTypeCombo();
                FillCountryCombo();
                FillCompanyTypeCombo();
                FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        public ActionResult CompanyMoreInfo(int CustomerID = 0, int CustomerTypeID = 0, string ProgramType = "", int Ccid = 0, int Cdid = 0, bool IsEditProfile = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                { Session["IsEditProfile"] = true; }
                else { Session["IsEditProfile"] = false; }
                CustomerInfoModel _objModel = new CustomerInfoModel();
                try
                {
                    ViewBag.Global_CompanyTypeName = System.Configuration.ConfigurationManager.AppSettings["CompanyTypeName"];
                    _objModel.CustomerID = CustomerID;
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.Status = "";
                    var CheckCount = _clsCust.CustomerInfoDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    if (CheckCount != null)
                    {
                        _objModel = CheckCount;
                    }
                    if (Ccid > 0)
                    {
                        _objModel.Status = "";
                        _objModel.CustomerCreditID = Ccid;
                        var Data = _clsCust.CustomerBankCreditDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        _objModel.CreditBankID = Data.BankID; _objModel.Status = Data.Status;
                        _objModel.CreditCurrencyID = Data.CurrencyID;
                        _objModel.FacType = Data.FacType; _objModel.FacLimit = Data.FacLimit; _objModel.FacUtilize = Data.FacUtilize;
                    }
                    else if (Cdid > 0)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        _objModel.CustomerCreditID = Cdid;
                        _clsCust.CustomerBankCreditDetail_Delete(_objModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        return RedirectToAction("CompanyMoreInfo", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.CustomerTypeIDs = Convert.ToString(CONT.ObligorCustomerTypeID) + ',' + Convert.ToString(CONT.BothObligorAndBuyerTypeID);
                    _objCustModel.Status = CONT.ddstatus;
                    ViewBag.CustomerObligors_List = new SelectList(_clsCust.CustomerMaster_ListAll(_objCustModel).OrderBy(x => x.CompanyName).ToList(), "CustomerID", "CompanyName", _objModel.ObligorIDs);
                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillSalesInvoiceTermsCombo();
                FillCountryCombo();
                FillCompanyTypeCombo();
                FillCurrencyCombo(); FillBankCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }

        }
        [HttpPost]
        public ActionResult CompanyMoreInfo(CustomerInfoModel _objModel, HttpPostedFileBase UpdTradeLic, HttpPostedFileBase UpdBusProfile,
        HttpPostedFileBase UpdAuditFin, HttpPostedFileBase UpdInHouseFin)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    if (Request["CreditSave"] != null || Request["CreditNextSave"] != null)
                    {
                        _objModel.CustomerBankAccID = _objModel.CreditBankAccID;
                        _objModel.BankID = _objModel.CreditBankID;
                        _objModel.CurrencyID = _objModel.CreditCurrencyID;
                        if (_objModel.CustomerCreditID > 0)
                        {
                            if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                            {
                                _objModel.Status = CONT.UR;
                                _clsCust.CustomerBankCreditDetailHistory_Add(_objModel);
                            }
                            else
                            {
                                _clsCust.CustomerBankCreditDetail_Update(_objModel);
                            }
                        }
                        else
                        {
                            _objModel.Status = CONT.RQ;
                            _clsCust.CustomerBankCreditDetail_Add(_objModel);
                        }
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                    }

                    else if (Request["Save"] != null || Request["NextSave"] != null)
                    {
                        string newfilenm1 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdTradeLic != null)
                        {
                            if (UpdTradeLic.FileName != null)
                            {
                                _objModel.UpdTradeLic = newfilenm1 + UpdTradeLic.FileName.ToString();
                                UpdTradeLic.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdTradeLic));
                            }
                        }
                        string newfilenm2 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdBusProfile != null)
                        {
                            if (UpdBusProfile.FileName != null)
                            {
                                _objModel.UpdBusProfile = newfilenm2 + UpdBusProfile.FileName.ToString();
                                UpdBusProfile.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdBusProfile));
                            }
                        }
                        string newfilenm3 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdAuditFin != null)
                        {
                            if (UpdAuditFin.FileName != null)
                            {
                                _objModel.UpdAuditFin = newfilenm3 + UpdAuditFin.FileName.ToString();
                                UpdAuditFin.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdAuditFin));
                            }
                        }
                        string newfilenm4 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdInHouseFin != null)
                        {
                            if (UpdInHouseFin.FileName != null)
                            {
                                _objModel.UpdInHouseFin = newfilenm4 + UpdInHouseFin.FileName.ToString();
                                UpdInHouseFin.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdInHouseFin));
                            }
                        }

                        if (Request["cb1"] == "cb1") { _objModel.IsCompGrpPart = true; }
                        if (Request["cb3"] == "cb3" || _objModel.CompanyTypeID == 1) { _objModel.IsCompPubList = true; }
                        if (Request["cb5"] == "cb5") { _objModel.IsCompFinServ = true; }

                        var Obligorsresult = string.Empty;
                        if (_objModel.CustomerObligors_List != null)
                        {
                            Obligorsresult = string.Join(",", _objModel.CustomerObligors_List);
                        }
                        _objModel.ObligorIDs = Convert.ToString(Obligorsresult);

                        if (_objModel.CustomerInfoID > 0)
                        {
                            if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                            {
                                _objModel.Status = CONT.UR;
                                _clsCust.CustomerInfoDetailHistory_Add(_objModel);
                            }
                            else
                            {
                                _clsCust.CustomerInfoDetail_Update(_objModel);
                            }

                        }
                        else
                        {
                            _objModel.Status = CONT.RQ;
                            _objModel.CustomerInfoID = _clsCust.CustomerInfoDetail_Add(_objModel);
                        }
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                    }
                    if (Request["Save"] != null || Request["CreditSave"] != null)
                    {
                        return RedirectToAction("CompanyMoreInfo", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                    else if (Request["NextSave"] != null || Request["CreditNextSave"] != null)
                    {
                        return RedirectToAction("AuthSignDetails", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }


                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    ViewBag.Global_CompanyTypeName = System.Configuration.ConfigurationManager.AppSettings["CompanyTypeName"];

                    _objModel.CustomerID = _objModel.CustomerID;
                    _objModel.CustomerTypeID = _objModel.CustomerID;
                    _objModel.Status = "";
                    var CheckCount = _clsCust.CustomerInfoDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    if (CheckCount != null)
                    {
                        _objModel = CheckCount;
                    }

                }
                CustomerMasterModel _objCustModel = new CustomerMasterModel();
                _objCustModel.CustomerTypeID = CONT.ObligorCustomerTypeID;
                _objCustModel.Status = CONT.ddstatus;
                ViewBag.CustomerObligors_List = new SelectList(_clsCust.CustomerMaster_ListAll(_objCustModel).OrderBy(x => x.CompanyName).ToList(), "CustomerID", "CompanyName", _objModel.ObligorIDs);
                FillCountryCombo();
                FillCompanyTypeCombo(); FillCurrencyCombo();
                FillSalesInvoiceTermsCombo(); FillBankCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult AuthSignDetails(int CustomerID = 0, int CDid = 0, int CAid = 0, int did = 0, string CustType = "", bool IsEditProfile = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                { Session["IsEditProfile"] = true; }
                else { Session["IsEditProfile"] = false; }
                CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
                try
                {
                    _objModel.CustomerID = CustomerID;
                    if (_objModel.CustomerID > 0)
                    {
                        _objModel.CustType = "D,A";
                        var authCount = _clsCust.CustomerShareHoldDetail_ListAll(_objModel).ToList();
                        if (authCount.Count > 0)
                            ViewBag.AuthCount = authCount;
                    }
                    if (CDid > 0)
                    {
                        _objModel.CustomerShareHoldID = CDid;
                        _objModel.CustType = CustType; ViewBag.CustType = CustType;
                        _objModel = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        _objModel.GetRoleIDs = _objModel.RoleIDs;
                    }

                    else if (did > 0)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        _objModel.CustomerShareHoldID = did;
                        _clsCust.CustomerShareHoldDetail_Delete(_objModel);
                        _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                        return RedirectToAction("AuthSignDetails", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                #region Role Combo
                CustomerMasterModel _CustModel = new CustomerMasterModel();
                _CustModel.CustomerID = _objModel.CustomerID;
                var GetCustomerTypeID = _clsCust.CustomerMaster_ListAll(_CustModel).FirstOrDefault();
                FillRoleCombo(GetCustomerTypeID.CustomerTypeID);
                #endregion

                FillCountryCombo(); FillDesignationCombo(); FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult AuthSignDetails(CustomerShareHolderModel _objModel, HttpPostedFileBase UpdPassport, HttpPostedFileBase UpdNatIden,
        HttpPostedFileBase AuthUpdPassport, HttpPostedFileBase UpdPOA, FormCollection frm)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (Request["Save"] != null || Request["AuthNextSave"] != null)
                    {
                        string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdPassport != null)
                        {
                            if (UpdPassport.FileName != null)
                            {
                                _objModel.UpdPassport = newfilenm + UpdPassport.FileName.ToString();
                                UpdPassport.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdPassport));
                            }
                        }
                        string newfilenm1 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdNatIden != null)
                        {
                            if (UpdNatIden.FileName != null)
                            {
                                _objModel.UpdNatIden = newfilenm1 + UpdNatIden.FileName.ToString();
                                UpdNatIden.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdNatIden));
                            }
                        }
                        string newfilenm2 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdPOA != null)
                        {
                            if (UpdPOA.FileName != null)
                            {
                                _objModel.UpdPOA = newfilenm2 + UpdPOA.FileName.ToString();
                                UpdPOA.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdPOA));
                            }
                        }

                        string strYes = Convert.ToString(frm["ChkYes"]);
                        if (strYes == "ChkYes")
                            _objModel.CustType = "A";
                        else
                            _objModel.CustType = "D";
                    }

                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();




                    string selRoles = Request.Form["RoleIDs"];

                    if (!string.IsNullOrEmpty(selRoles))
                    {
                        _objModel.RoleIDs = selRoles;
                    }

                    if (_objModel.CustomerShareHoldID > 0)
                    {
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                        {
                            _objModel.Status = CONT.UR;
                            _clsCust.CustomerShareHoldDetailHistory_Add(_objModel);
                        }
                        else
                        {
                            _clsCust.CustomerShareHoldDetail_Update(_objModel);
                        }
                    }
                    else
                    {
                        _objModel.Status = CONT.RQ;
                        _clsCust.CustomerShareHoldDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["Save"] != null)
                    {
                        return RedirectToAction("AuthSignDetails", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                    else if (Request["AuthNextSave"] != null)
                    {
                        CustomerMasterModel _objCustModel = new CustomerMasterModel();
                        _objCustModel.CustomerID = _objModel.CustomerID;
                        _objCustModel = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                        if (_objCustModel.CompPubList == "N")
                        {
                            return RedirectToAction("Shareholders", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }
                        else
                        {
                            return RedirectToAction("BankAccountDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }
                    }
                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                #region Role Combo
                CustomerMasterModel _CustModel = new CustomerMasterModel();
                _CustModel.CustomerID = _objModel.CustomerID;
                var GetCustomerTypeID = _clsCust.CustomerMaster_ListAll(_CustModel).FirstOrDefault();
                FillRoleCombo(GetCustomerTypeID.CustomerTypeID);
                #endregion
                FillCountryCombo(); FillDesignationCombo(); FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult AuthSignDetailsPopUp(int CustomerShareHoldID)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            try
            {
                if (CustomerShareHoldID > 0)
                {
                    _objModel.CustomerShareHoldID = CustomerShareHoldID;
                    _objModel.Status = CONT.NA + ',' + CONT.UA;
                    ViewBag.AuthSign = _clsCust.CustomerShareHoldDetailHistory_ListAll(_objModel).FirstOrDefault();

                    _objModel.Status = "";
                    _objModel = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    _objModel.GetRoleIDs = _objModel.RoleIDs;
                    if (ViewBag.AuthSign == null) { ViewBag.AuthSign = _objModel; }
                }
                else
                {
                    Response.Write("User not exist."); Response.End();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null)
                    ViewBag.ErrorMesssage = ex.Message;
                else
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            return PartialView(_objModel);
        }
        public PartialViewResult GridDirectorDetails(int CustomerID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.CustType = "A,D";
            var Data = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult GridAuthSignDetails(int CustomerID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.CustType = "A,D";
            var Data = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }

        public ActionResult Shareholders(int CustomerID = 0, int CIid = 0, int CCid = 0, int CBid = 0, int did = 0, string compIndi = "", bool IsEditProfile = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                { Session["IsEditProfile"] = true; }
                else { Session["IsEditProfile"] = false; }
                CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
                try
                {
                    if (compIndi != "")
                    {
                        ViewBag.compIndi = compIndi;
                    }
                    _objModel.CustomerID = CustomerID;
                    if (CIid > 0)
                    {
                        _objModel.CustomerShareHoldID = CIid;
                        _objModel = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        ViewBag.compIndi = "Indi";
                    }
                    else if (CCid > 0)
                    {
                        _objModel.CustomerShareHoldID = CCid;
                        _objModel = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        _objModel.CompCustomerShareHoldID = CCid;
                        _objModel.CompSharePer = _objModel.SharePer;
                        _objModel.RegiCountryID = _objModel.NatCountryID;
                        _objModel.CustomerShareHoldID = 0;
                        _objModel.SharePer = 0; _objModel.ParentID = CCid;
                        if (CBid > 0)
                        {
                            ViewBag.childID = CBid;
                        }
                        ViewBag.compIndi = "Comp";
                    }

                    else if (did > 0)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        _objModel.CustomerShareHoldID = did;
                        _clsCust.CustomerShareHoldDetail_Delete(_objModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        return RedirectToAction("Shareholders", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                }
                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                _objModel.CustType = "D,A";
                ViewBag.GetAuthSignCombo = new SelectList(_clsCust.CustomerShareHoldDetail_ListAll(_objModel).OrderBy(x => x.FirstName).ToList(), "CustomerShareHoldID", "ContactFullName", _objModel.CustomerShareHoldID);
                FillCountryCombo(); FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        [HttpPost]
        public ActionResult Shareholders(CustomerShareHolderModel _objModel, HttpPostedFileBase UpdPassport, HttpPostedFileBase UpdNatIden,
        HttpPostedFileBase ShareUpdPassport, HttpPostedFileBase ShareUpdNatIden, HttpPostedFileBase UpdTradeLic, FormCollection frm, ControllerContext controllerContext)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    if (Request["Save"] != null || Request["NextSave"] != null)
                    {
                        string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdPassport != null)
                        {
                            if (UpdPassport.FileName != null)
                            {
                                _objModel.UpdPassport = newfilenm + UpdPassport.FileName.ToString();
                                UpdPassport.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdPassport));
                            }
                        }
                        string newfilenm1 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdNatIden != null)
                        {
                            if (UpdNatIden.FileName != null)
                            {
                                _objModel.UpdNatIden = newfilenm1 + UpdNatIden.FileName.ToString();
                                UpdNatIden.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdNatIden));
                            }
                        }
                        _objModel.CustType = "S";
                    }
                    else if (Request["parSave"] != null || Request["parSaveNext"] != null)
                    {

                        string newfilenm2 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdTradeLic != null)
                        {
                            if (UpdTradeLic.FileName != null)
                            {
                                _objModel.UpdTradeLic = newfilenm2 + UpdTradeLic.FileName.ToString();
                                UpdTradeLic.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdTradeLic));
                            }
                        }
                        if (_objModel.ParentID == 0)
                        {
                            _objModel.SharePer = _objModel.CompSharePer;
                            _objModel.Status = CONT.RQ;
                            _objModel.CustType = "C";
                            _objModel.NatCountryID = _objModel.RegiCountryID;
                            _objModel.CompanyName = _objModel.ShareCompanyName;

                            int parentId = _clsCust.CustomerShareHoldDetail_Add(_objModel);
                            _objModel.ParentID = parentId;
                        }
                        int count = Convert.ToInt32(frm["hdncount"]);
                        for (int i = 0; i < count; i++)
                        {
                            _objModel.FirstName = Convert.ToString(frm["ShareFirstName_" + i + ""]);
                            if (_objModel.FirstName != null && _objModel.FirstName != "")
                            {

                                HttpFileCollectionBase files = Request.Files;
                                for (int z = 0; z < files.Count; z++)
                                {
                                    string GetAllkeysUploadFile = files.AllKeys[z];

                                    if (GetAllkeysUploadFile.Equals("ShareUpdPassport_" + i + ""))
                                    {
                                        var fileName = "";
                                        string gu = Guid.NewGuid().ToString().Substring(0, 12);
                                        HttpPostedFileBase file = files[z];
                                        if (file != null && file.ContentLength > 0)
                                        {
                                            fileName = gu + Path.GetFileName(file.FileName);
                                            file.SaveAs(Server.MapPath("\\Files\\Upload\\" + fileName));
                                            _objModel.UpdPassport = fileName;
                                        }
                                    }
                                    if (GetAllkeysUploadFile.Equals("ShareUpdNatIden_" + i + ""))
                                    {
                                        var fileName = "";
                                        string gu = Guid.NewGuid().ToString().Substring(0, 12);
                                        HttpPostedFileBase file = files[z];
                                        if (file != null && file.ContentLength > 0)
                                        {
                                            fileName = gu + Path.GetFileName(file.FileName);
                                            file.SaveAs(Server.MapPath("\\Files\\Upload\\" + fileName));
                                            _objModel.UpdNatIden = fileName;
                                        }
                                    }
                                }

                                _objModel.CustType = "B";
                                _objModel.CustomerShareHoldID = _objModel.ShareCustomerShareHoldID;

                                _objModel.FirstName = Convert.ToString(frm["ShareFirstName_" + i + ""]);
                                _objModel.MiddleName = Convert.ToString(frm["ShareMiddleName_" + i + ""]);
                                _objModel.LastName = Convert.ToString(frm["ShareLastName_" + i + ""]);
                                _objModel.NatCountryID = Convert.ToInt32(frm["ShareNatCountryID_" + i + ""]);
                                _objModel.PassNo = Convert.ToString(frm["SharePassNo_" + i + ""]);
                                _objModel.PassIssueDate = Convert.ToDateTime(frm["SharePassIssueDate_" + i + ""]);
                                _objModel.PassCountryID = Convert.ToInt32(frm["SharePassCountryID_" + i + ""]);
                                _objModel.PassExpDate = Convert.ToDateTime(frm["SharePassExpDate_" + i + ""]);
                                _objModel.SharePer = Convert.ToInt32(frm["childSharePer_" + i + ""]);

                                _objModel.Gender = Convert.ToString(frm["ShareGender_" + i + ""]);
                                _objModel.LocCountryID = Convert.ToInt32(frm["ShareLocCountryID_" + i + ""]);
                                _clsCust.CustomerShareHoldDetail_Add(_objModel);

                            }
                        }
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();

                        if (Request["parSave"] != null)
                        {
                            return RedirectToAction("Shareholders", "CustomerReg", new { CustomerID = _objModel.CustomerID, compIndi = "Comp" });
                        }
                        else if (Request["parSaveNext"] != null)
                        {
                            return RedirectToAction("BankAccountDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }
                    }
                    else if (Request["ComSaveAdd"] != null || Request["ComNextSave"] != null)
                    {
                        string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdTradeLic != null)
                        {
                            if (UpdTradeLic.FileName != null)
                            {
                                _objModel.UpdTradeLic = newfilenm + UpdTradeLic.FileName.ToString();
                                UpdTradeLic.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdTradeLic));
                            }
                        }
                        _objModel.SharePer = _objModel.CompSharePer;
                        _objModel.CustType = "C";
                        _objModel.CompanyName = _objModel.ShareCompanyName;
                        _objModel.NatCountryID = _objModel.RegiCountryID;
                        _objModel.CustomerShareHoldID = _objModel.CompCustomerShareHoldID;
                        _objModel.ParentID = 0;
                    }

                    if (_objModel.CustomerShareHoldID > 0)
                    {
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                        {
                            _objModel.Status = CONT.UR;
                            _clsCust.CustomerShareHoldDetailHistory_Add(_objModel);
                        }
                        else
                        {
                            _clsCust.CustomerShareHoldDetail_Update(_objModel);
                        }
                    }
                    else
                    {
                        _objModel.Status = CONT.RQ;
                        _clsCust.CustomerShareHoldDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();

                    if (Request["parSave"] != null)
                    {
                        return RedirectToAction("Shareholders", "CustomerReg", new { CustomerID = _objModel.CustomerID, compIndi = "Comp" });
                    }
                    else if (Request["NextSave"] != null || Request["ComNextSave"] != null)
                    {
                        return RedirectToAction("BankAccountDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                    else if (Request["Save"] != null)
                    {
                        return RedirectToAction("Shareholders", "CustomerReg", new { CustomerID = _objModel.CustomerID, compIndi = "Indi" });
                    }
                    else if (Request["ComSaveAdd"] != null)
                    {
                        return RedirectToAction("Shareholders", "CustomerReg", new { CustomerID = _objModel.CustomerID, compIndi = "Comp" });
                    }

                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillGenderCombo();
                _objModel.CustType = "D,A";
                ViewBag.GetAuthSignCombo = new SelectList(_clsCust.CustomerShareHoldDetail_ListAll(_objModel).OrderBy(x => x.FirstName).ToList(), "CustomerShareHoldID", "ContactFullName", _objModel.CustomerShareHoldID);
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult GridShareholders(int CustomerID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.CustType = "S";
            var Data = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult GridShareholdersBeneficial(int CustomerID = 0, int ParentID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.ParentID = ParentID;
            _objModel.CustType = "B";
            var Data = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult GridShareholdersCompany(int CustomerID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.CustType = "C";
            var Data = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult GridShareholdersCompIndiPop(int CustomerID = 0, int ParentID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = "";
            _objModel.ParentID = ParentID;
            _objModel.CustType = "B";
            var Data = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        [HttpPost]
        public JsonResult GetAuthSignDetails(int CustomerAuthSignID)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerShareHoldID = CustomerAuthSignID;
            _objModel.CustType = "D,A";
            var result = _clsCust.CustomerShareHoldDetail_ListAll(_objModel);
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ShareholdersBeneficial(int CustomerID = 0, int ParentID = 0, int CBid = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            FillCountryCombo();
            if (CBid > 0)
            {
                _objModel.CustomerShareHoldID = CBid;
                _objModel.ParentID = -1;
                _objModel = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();

                _objModel.ShareCustomerShareHoldID = _objModel.CustomerShareHoldID;
                _objModel.ShareFirstName = _objModel.FirstName; _objModel.ShareMiddleName = _objModel.MiddleName;
                _objModel.ShareLastName = _objModel.LastName; _objModel.ShareNatCountryID = _objModel.NatCountryID;
                _objModel.SharePassNo = _objModel.PassNo; _objModel.ShareUpdPassport = _objModel.UpdPassport;
                _objModel.ShareUpdNatIden = _objModel.UpdNatIden; _objModel.SharePassIssueDate = _objModel.PassIssueDate;
                _objModel.SharePassCountryID = _objModel.PassCountryID;
                _objModel.SharePassExpDate = _objModel.PassExpDate; _objModel.childSharePer = _objModel.SharePer;

                _objModel.FirstName = ""; _objModel.MiddleName = "";
                _objModel.LastName = ""; _objModel.NatCountryID = 0;
                _objModel.PassNo = ""; _objModel.UpdPassport = "";
                _objModel.UpdNatIden = ""; _objModel.PassIssueDate = Convert.ToDateTime("01/01/1900");
                _objModel.PassExpDate = Convert.ToDateTime("01/01/1900");
                _objModel.CustomerShareHoldID = 0;
                _objModel.SharePer = 0;
            }
            return PartialView(_objModel);
        }

        public ActionResult ShareholdersBeneficialPopUp(int CBid = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            FillCountryCombo(); FillGenderCombo();
            if (CBid > 0)
            {
                _objModel.ShareCustomerShareHoldID = _objModel.CustomerShareHoldID = CBid;
                _objModel.ParentID = -1;
                _objModel = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                _objModel.ShareCustomerShareHoldID = _objModel.CustomerShareHoldID = CBid;
                _objModel.SharePassCountryID = _objModel.PassCountryID; _objModel.ShareNatCountryID = _objModel.NatCountryID;
                _objModel.ShareGender = _objModel.Gender;
                _objModel.ShareLocCountryID = _objModel.LocCountryID;

            }
            return PartialView(_objModel);
        }
        [HttpPost]
        public ActionResult ShareholdersBeneficialPopUpPost(CustomerShareHolderModel _objModel, HttpPostedFileBase ShareUpdNatIden1, HttpPostedFileBase ShareUpdPassport1)
        {
            var data = "";
            try
            {
                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();

                string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                if (ShareUpdNatIden1 != null)
                {
                    if (ShareUpdNatIden1.FileName != null)
                    {
                        _objModel.ShareUpdNatIden = newfilenm + ShareUpdNatIden1.FileName.ToString();
                        ShareUpdNatIden1.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.ShareUpdNatIden));
                    }
                }
                string newfilenm1 = Guid.NewGuid().ToString().Substring(0, 12);
                if (ShareUpdPassport1 != null)
                {
                    if (ShareUpdPassport1.FileName != null)
                    {
                        _objModel.ShareUpdPassport = newfilenm1 + ShareUpdPassport1.FileName.ToString();
                        ShareUpdPassport1.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.ShareUpdPassport));
                    }
                }

                _objModel.CustType = "B";
                _objModel.CustomerShareHoldID = _objModel.ShareCustomerShareHoldID;
                _objModel.FirstName = _objModel.ShareFirstName; _objModel.MiddleName = _objModel.ShareMiddleName;
                _objModel.LastName = _objModel.ShareLastName; _objModel.NatCountryID = _objModel.ShareNatCountryID;
                _objModel.PassNo = _objModel.SharePassNo; _objModel.UpdPassport = _objModel.ShareUpdPassport;
                _objModel.UpdNatIden = _objModel.ShareUpdNatIden; _objModel.PassIssueDate = _objModel.SharePassIssueDate;
                _objModel.PassCountryID = _objModel.SharePassCountryID;
                _objModel.Gender = _objModel.ShareGender;
                _objModel.LocCountryID = _objModel.ShareLocCountryID;
                _objModel.PassExpDate = _objModel.SharePassExpDate; _objModel.SharePer = _objModel.childSharePer;

                if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                {
                    _objModel.Status = CONT.UR;
                    _clsCust.CustomerShareHoldDetailHistory_Add(_objModel);
                }
                else
                {
                    _clsCust.CustomerShareHoldDetail_Update(_objModel);
                }
                _clsCust.Tras.Commit();
                _clsCust.Conn.Close();
                return Json(_objModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                data = ex.Message.ToString();
                return Json(new { error = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult BankAccountDetail(int CustomerID = 0, int Cbid = 0, int did = 0, int Ccid = 0, int Cdid = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerBankAccDetail _objModel = new CustomerBankAccDetail();
                try
                {
                    _objModel.CustomerID = CustomerID;

                    if (Cbid > 0)
                    {
                        _objModel.CustomerBankAccID = Cbid;
                        _objModel = _clsCust.CustomerBankAccDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    }
                    else if (did > 0)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        _objModel.CustomerBankAccID = did;
                        _clsCust.CustomerBankAccDetail_Delete(_objModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        return RedirectToAction("BankAccountDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }

                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillCurrencyCombo(); FillBankCombo();

                return View(_objModel);

            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult BankAccountDetail(CustomerBankAccDetail _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    if (Request["BankSave"] != null || Request["BankSaveNext"] != null)
                    {

                        if (_objModel.CustomerBankAccID > 0)
                        {
                            if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                            {
                                _objModel.Status = CONT.UR;
                                _clsCust.CustomerBankAccDetailHistory_Add(_objModel);
                            }
                            else
                            {
                                _clsCust.CustomerBankAccDetail_Update(_objModel);
                            }
                        }
                        else
                        {
                            _objModel.Status = CONT.RQ;
                            _clsCust.CustomerBankAccDetail_Add(_objModel);
                        }
                    }

                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["BankSave"] != null)
                    {
                        return RedirectToAction("BankAccountDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                    else if (Request["NextSave"] != null || Request["BankSaveNext"] != null)
                    {
                        CustomerMasterModel _objCustModel = new CustomerMasterModel();
                        _objCustModel.CustomerID = _objModel.CustomerID;
                        _objCustModel = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                        if ((_objCustModel.CustomerTypeID == CONT.BuyerCustomerTypeID && _objCustModel.ProgramType == CONT.RType) ||
                            (_objCustModel.CustomerTypeID == CONT.BothObligorAndBuyerTypeID && _objCustModel.ProgramType == CONT.BothProgramType))
                        {
                            return RedirectToAction("SupplierDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }
                        else
                        {
                            return RedirectToAction("CreateUser", "CustomerReg", new { CustomerID = _objModel.CustomerID, CustomerTypeID = _objCustModel.CustomerTypeID });

                        }
                    }
                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillCurrencyCombo(); FillBankCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult GridBankDetail(int CustomerID = 0)
        {
            CustomerBankAccDetail _objModel = new CustomerBankAccDetail();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            var Data = _clsCust.CustomerBankAccDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public JsonResult GetBankData(int CustomerBankAccID = 0)
        {
            CustomerBankAccDetail _objModel = new CustomerBankAccDetail();
            _objModel.CustomerBankAccID = CustomerBankAccID;
            List<CustomerBankAccDetail> BankDetail_ListAll = _clsCust.CustomerBankAccDetail_ListAll(_objModel).ToList();
            return Json(BankDetail_ListAll, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult GridCreditBankDetail(int CustomerID = 0)
        {
            CustomerInfoModel _objModel = new CustomerInfoModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            var Data = _clsCust.CustomerBankCreditDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public ActionResult SupplierDetail(int CustomerID = 0, int CSid = 0, int did = 0, int Ebid = 0, int Edid = 0, bool IsEditProfile = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                { Session["IsEditProfile"] = true; }
                else { Session["IsEditProfile"] = false; }
                CustomerSupplierModel _objModel = new CustomerSupplierModel();
                string _currentStatus = "";
                try
                {
                    _objModel.CustomerID = CustomerID;
                    _objModel.ProgramType = Convert.ToString(Session["ProgramType"]);
                    if (CSid > 0)
                    {
                        _objModel.CustomerSuppID = CSid;
                        _objModel = _clsCust.CustomerSupplierDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        _currentStatus = _objModel.Status;
                    }
                    else if (did > 0)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        _objModel.CustomerSuppID = did;
                        _clsCust.CustomerSupplierDetail_Delete(_objModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        return RedirectToAction("SupplierDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                    if (Ebid > 0)
                    {
                        _objModel.CustomerBankAccID = Ebid;
                        _objModel.IsCustomerSupp = true;
                        _objModel.Status = "";
                        var Data = _clsCust.ExpoCustomerBankAccDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        _objModel.BankCurrencyID = _objModel.CurrencyID; _objModel.AccountName = Data.AccountName; _objModel.BankName = Data.BankName;
                        _objModel.AccountNo = Data.AccountNo; _objModel.IBAN = Data.IBAN; _objModel.BankCountryID = Data.BankCountryID;
                        _objModel.BankAddress = Data.Address; _objModel.Swift = Data.Swift; _objModel.IFSC = Data.IFSC; _objModel.BankID = Data.BankID;
                        _objModel.Status = _currentStatus;
                    }
                    else if (Edid > 0)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        _objModel.CustomerBankAccID = Edid;
                        _clsCust.ExpoCustomerBankAccDetail_Delete(_objModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        return RedirectToAction("SupplierDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID, CSid = _objModel.CustomerSuppID });
                    }
                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillDesignationCombo(); FillIndustryCombo(); FillCurrencyCombo(); FillBankCombo();
                FillSalesInvoiceTermsCombo();
                return View(_objModel);

            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult SupplierDetail(CustomerSupplierModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();

                    if (Request["SupplierSave"] != null || Request["NextSave"] != null)
                    {
                        if (_objModel.CustomerSuppID > 0)
                        {
                            if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                            {
                                _objModel.Status = CONT.UR;
                                _clsCust.CustomerSupplierDetailHistory_Add(_objModel);
                                _clsCust.ExpoCustomerBankAccDetailHistory_Add(_objModel);
                            }
                            else
                            {
                                _clsCust.CustomerSupplierDetail_Update(_objModel);
                                _clsCust.ExpoCustomerBankAccDetail_Update(_objModel);
                            }
                        }
                        else
                        {
                            _objModel.Status = CONT.RQ;
                            _objModel.CustomerSuppID = _clsCust.CustomerSupplierDetail_Add(_objModel);
                            _clsCust.ExpoCustomerBankAccDetail_Add(_objModel);
                        }
                    }
                    else if (Request["NewSupplierSave"] != null || Request["NewNextSave"] != null)
                    {
                        if (_objModel.CustomerSuppID > 0)
                        {
                            if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                            {
                                _objModel.Status = CONT.UR;
                                _clsCust.CustomerSupplierDetailHistory_Add(_objModel);
                            }
                            else
                            {
                                _clsCust.CustomerSupplierDetail_Update(_objModel);
                            }
                        }
                        else
                        {
                            _objModel.Status = CONT.RQ;
                            _clsCust.CustomerSupplierDetail_Add(_objModel);
                        }
                        _objModel.CustomerSuppID = 0;
                    }

                    else if (Request["bankSave"] != null)
                    {
                        if (_objModel.CustomerBankAccID > 0)
                        {
                            if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                            {
                                _objModel.Status = CONT.UR;
                                _clsCust.ExpoCustomerBankAccDetailHistory_Add(_objModel);
                            }
                            else
                            {
                                _clsCust.ExpoCustomerBankAccDetail_Update(_objModel);
                            }
                        }
                        else
                        {
                            _objModel.Status = CONT.RQ;
                            _clsCust.ExpoCustomerBankAccDetail_Add(_objModel);
                        }
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["SupplierSave"] != null || Request["bankSave"] != null || Request["NewSupplierSave"] != null)
                    {
                        return RedirectToAction("SupplierDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID, CSid = _objModel.CustomerSuppID });
                    }
                    else if (Request["NewNextSave"] != null || Request["NextSave"] != null)
                    {
                        return RedirectToAction("CreateUser", "CustomerReg", new { CustomerID = _objModel.CustomerID, CustomerTypeID = _objModel.CustomerTypeID });

                    }
                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillDesignationCombo(); FillIndustryCombo(); FillCurrencyCombo(); FillBankCombo();
                FillSalesInvoiceTermsCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult GridSupplierDetail(int CustomerID = 0)
        {
            CustomerSupplierModel _objModel = new CustomerSupplierModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            var Data = _clsCust.CustomerSupplierDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult SupplierDetailPop(int CustomerID = 0, int CustomerSuppID = 0, bool IsBank = false)
        {
            CustomerSupplierModel _objModel = new CustomerSupplierModel();
            _objModel.Status = CONT.NA + ',' + CONT.UA;
            _objModel.CustomerID = CustomerID;
            _objModel.CustomerSuppID = CustomerSuppID;
            ViewBag.Supplier = _clsCust.CustomerSupplierDetailHistory_ListAll(_objModel).FirstOrDefault();

            _objModel.Status = "";
            _objModel.CustomerID = CustomerID;
            _objModel.CustomerSuppID = CustomerSuppID;
            _objModel = _clsCust.CustomerSupplierDetailHistory_ListAllBind(_objModel).FirstOrDefault();

            if (ViewBag.Supplier == null) { ViewBag.Supplier = _objModel; }
            if (IsBank)
            {
                _objModel.IsCustomerSupp = true;
                ViewBag.ExpoBank = _clsCust.ExpoCustomerBankAccDetailHistory_ListAllBind(_objModel);
            }
            return PartialView(_objModel);
        }
        public ActionResult ExporterBankAccountDetail(int CustomerID = 0, int Cbid = 0, int did = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerBankAccDetail _objModel = new CustomerBankAccDetail();
                try
                {
                    _objModel.CustomerID = CustomerID;

                    if (Cbid > 0)
                    {
                        _objModel.CustomerBankAccID = Cbid;
                        _objModel.IsCustomerSupp = true;
                        _objModel = _clsCust.CustomerBankAccDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    }
                    else if (did > 0)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        _objModel.CustomerBankAccID = did;
                        _clsCust.CustomerBankAccDetail_Delete(_objModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        return RedirectToAction("ExporterBankAccountDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }

                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillCurrencyCombo(); FillBankCombo();
                CustomerSupplierModel _objSupModel = new CustomerSupplierModel();
                _objSupModel.CustomerID = CustomerID;
                ViewBag.CustomerSuppID = new SelectList(_clsCust.CustomerSupplierDetail_ListAll(_objSupModel).OrderBy(x => x.CustomerSuppID).ToList(), "CustomerSuppID", "CompanyName", _objModel.CustomerSuppID);
                return View(_objModel);

            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult ExporterBankAccountDetail(CustomerBankAccDetail _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    if (_objModel.CustomerBankAccID > 0)
                    {
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                        {
                            _objModel.Status = CONT.UR;
                            _clsCust.CustomerBankAccDetailHistory_Add(_objModel);
                        }
                        else
                        {
                            _clsCust.CustomerBankAccDetail_Update(_objModel);
                        }
                    }
                    else
                    {
                        _objModel.Status = CONT.RQ;
                        _clsCust.CustomerBankAccDetail_Add(_objModel);
                    }

                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["Save"] != null)
                    {
                        return RedirectToAction("ExporterBankAccountDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                    else if (Request["NextSave"] != null)
                    {
                        return RedirectToAction("Documents", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillCurrencyCombo(); FillBankCombo();
                CustomerSupplierModel _objSupModel = new CustomerSupplierModel();
                ViewBag.CustomerSuppID = new SelectList(_clsCust.CustomerSupplierDetail_ListAll(_objSupModel).OrderBy(x => x.CustomerSuppID).ToList(), "CustomerSuppID", "CompanyName", _objModel.CustomerSuppID);
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult GridExporterBankDetail(int CustomerID = 0, int CustomerSuppID = 0)
        {
            CustomerBankAccDetail _objModel = new CustomerBankAccDetail();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.IsCustomerSupp = true;
            _objModel.CustomerSuppID = CustomerSuppID;
            var Data = _clsCust.CustomerBankAccDetailHistory_ListAllBind(_objModel);
            ViewBag.ProgramType = Convert.ToString(Session["ProgramType"]);
            return PartialView(Data);
        }

        public ActionResult CreateUser(int CustomerID = 0, int CustomerTypeID = 0, int EditID = 0, int DeleteID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    #region Role Combo
                    CustomerMasterModel _CustModel = new CustomerMasterModel();
                    _CustModel.CustomerID = CustomerID;
                    var GetCustomerTypeID = _clsCust.CustomerMaster_ListAll(_CustModel).FirstOrDefault();
                    FillRoleCombo(GetCustomerTypeID.CustomerTypeID);
                    #endregion
                    string strRoleIDs = "";
                    FillCustomerCombo(); FillDocumentCombo();
                    FillDesignationCombo(); FillGenderCombo(); FillCountryCombo();

                    if (EditID > 0)
                    {
                        _objUserMaster.UserID = EditID; _objRolelink.UserID = EditID; _objUserMaster.CustomerID = CustomerID;
                        _objUserMaster.IsActive = true;
                        _objUserMaster = _clsUser.UserMasterHistory_ListAllBind(_objUserMaster).FirstOrDefault();
                        var Data = _clsUser.UserRoleLinkMaster_ListAll(_objRolelink).ToList();

                        if (Data != null)
                        {
                            for (int i = 0; i < Data.Count; i++)
                            {
                                strRoleIDs = strRoleIDs + Data[i].RoleID + ",";
                            }
                            if (strRoleIDs.Length > 0)
                                strRoleIDs = strRoleIDs.Remove(strRoleIDs.Length - 1);
                        }
                        _objUserMaster.GetRoleIDs = strRoleIDs;
                    }
                    if (DeleteID > 0)
                    {
                        _clsUser.conn = ClsAppDatabase.GetCon();
                        if (_clsUser.conn.State == ConnectionState.Closed) _clsUser.conn.Open();
                        else
                        { _clsUser.conn.Close(); _clsUser.conn.Open(); }
                        _clsUser.tras = _clsUser.conn.BeginTransaction();

                        _objUserMaster.UserID = DeleteID; _objUserMaster.IsActive = true;
                        var GetUserStatus = _clsUser.UserMaster_ListAll(_objUserMaster).FirstOrDefault();
                        string status = GetUserStatus.Status;

                        Boolean IsRequird = false; _objUserMaster.IsNoRequired = IsRequird;
                        if (status == CONT.NA || status == CONT.UM || status == CONT.UA)
                            _objUserMaster.IsNoRequired = true;

                        _objUserMaster.UserID = DeleteID;
                        int intDeleteID = _clsUser.UserMaster_Delete(_objUserMaster);
                        _clsUser.tras.Commit(); _clsUser.conn.Close();

                        return RedirectToAction("CreateUser", "CustomerReg", new { CustomerID = CustomerID, CustomerTypeID = CustomerTypeID });
                    }

                    _objUserMaster.CustomerID = CustomerID; _objUserMaster.CustomerTypeID = CustomerTypeID;
                    _ObjServiceProvider.CustomerID = CustomerID; _ObjServiceProvider.CustomerTypeID = CustomerTypeID;
                    var GetCompanyName = _ClsServiceProvider.CustomerMaster_ListAll(_ObjServiceProvider).FirstOrDefault();

                    _objUserMaster.CompanyName = GetCompanyName.CompanyName;
                    return View(_objUserMaster);
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult CreateUser(UserMaster_ListAll_Result mUserMaster, HttpPostedFileBase Attach, FormCollection frm)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string strRoleIDs = "";
                if (Request["RoleIDs"] != null) { strRoleIDs = Request["RoleIDs"]; }

                try
                {
                    _clsUser.conn = ClsAppDatabase.GetCon(); if (_clsUser.conn.State == ConnectionState.Closed) { _clsUser.conn.Open(); }
                    else { _clsUser.conn.Close(); _clsUser.conn.Open(); }
                    _clsUser.tras = _clsUser.conn.BeginTransaction();

                    string newfilenm = Guid.NewGuid().ToString().Substring(0, 5);
                    if (Attach != null)
                    {
                        if (Attach.FileName != null)
                        {
                            mUserMaster.Attach = newfilenm + Attach.FileName.ToString(); Attach.SaveAs(Server.MapPath("\\Files\\Upload\\" + mUserMaster.Attach));
                        }
                    }

                    if (mUserMaster.UserID > 0)
                    {
                        if (mUserMaster.Status == CONT.NA || mUserMaster.Status == CONT.UR || mUserMaster.Status == CONT.UA)
                        {
                            mUserMaster.Status = CONT.UR;
                            _clsUser.UserMasterHistory_Add(mUserMaster); RoleMaster_AddDelete(mUserMaster.UserID, CONT.UR);
                        }
                        else
                        {
                            mUserMaster.Status = CONT.RQ;
                            _clsUser.UserMaster_Update(mUserMaster); RoleMaster_AddDelete(mUserMaster.UserID, CONT.RQ);
                        }
                    }
                    else
                    {
                        mUserMaster.Status = CONT.RQ;
                        mUserMaster.UserID = _clsUser.UserMaster_Add(mUserMaster);
                        if (mUserMaster.UserID > 0)
                            RolLinkMaster_Add(mUserMaster.UserID, CONT.RQ);
                    }
                    _clsUser.tras.Commit(); _clsUser.conn.Close();

                    mUserMaster.RoleIDs = strRoleIDs;
                    if (Request["createsave"] != null)
                        return RedirectToAction("CreateUser", "CustomerReg", new { CustomerID = mUserMaster.CustomerID, CustomerTypeID = mUserMaster.CustomerTypeID });
                    else if (Request["UserNextSave"] != null)
                    {
                        if (Convert.ToBoolean(Session["IsEditProfile"]) == true)
                        {
                            return RedirectToAction("Documents", "CustomerReg", new { CustomerID = mUserMaster.CustomerID });
                        }

                        if (mUserMaster.Status == CONT.RQ || mUserMaster.Status == CONT.NR || mUserMaster.Status == CONT.ND)
                        {
                            return RedirectToAction("Documents", "CustomerReg", new { CustomerID = mUserMaster.CustomerID });
                        }
                        else
                        {
                            return RedirectToAction("ComplianceScreening", "CustomerReg", new { CustomerID = mUserMaster.CustomerID, currentStatus = mUserMaster.Status, CustomerTypeID = mUserMaster.CustomerTypeID, Admin = "Y" });
                        }
                    }
                    //return RedirectToAction("Documents", "CustomerReg", new { CustomerID = mUserMaster.CustomerID });
                }
                catch (Exception ex)
                {
                    _clsUser.tras.Rollback(); _clsUser.conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                #region Role Combo
                CustomerMasterModel _CustModel = new CustomerMasterModel();
                _CustModel.CustomerID = mUserMaster.CustomerID;
                var GetCustomerTypeID = _clsCust.CustomerMaster_ListAll(_CustModel).FirstOrDefault();
                FillRoleCombo(GetCustomerTypeID.CustomerTypeID);
                #endregion
                FillCustomerCombo(); FillDocumentCombo();
                FillDesignationCombo(); FillGenderCombo(); FillCountryCombo();
                return View(mUserMaster);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public void RoleMaster_AddDelete(int UserID, string ProStaus)
        {
            if (UserID != 0)
            {
                int LoginUserId = 0;
                int.TryParse(Session["UserId"].ToString(), out LoginUserId);
                string selRoles = Request.Form["RoleIDs"];
                if (!string.IsNullOrEmpty(selRoles))
                {
                    #region Delete Added Role                    
                    _objRolelink.UserID = UserID;
                    var GetData = _clsUser.UserRoleLinkMaster_ListAll(_objRolelink).ToList();
                    if (GetData != null && GetData.Count > 0)
                    {
                        for (int i = 0; i < GetData.Count; i++)
                        {
                            _objRolelink.UserRoleLinkID = GetData[i].UserRoleLinkID;
                            _clsUser.UserRoleLinkMaster_Delete(_objRolelink);
                        }
                    }
                    #endregion

                    selRoles = selRoles.TrimEnd(',');
                    string[] selrol = selRoles.Split(',');

                    for (int i = 0; i < selrol.Length; i++)
                    {
                        int RolID = 0; int.TryParse(selrol[i].ToString(), out RolID);
                        int getid = 0;
                        _objRolelink.UserID = UserID; _objRolelink.RoleID = RolID; _objRolelink.Status = ProStaus;
                        getid = _clsUser.UserRoleLinkMaster_Add(_objRolelink);
                    }
                }
            }
        }
        public void RolLinkMaster_Add(int UserID, string ProStaus)
        {
            if (UserID != 0)
            {
                string selRoles = Request.Form["RoleIDs"];
                if (!string.IsNullOrEmpty(selRoles))
                {
                    selRoles = selRoles.TrimEnd(',');
                    string[] selrol = selRoles.Split(',');

                    for (int i = 0; i < selrol.Length; i++)
                    {
                        int RolID = 0; int.TryParse(selrol[i].ToString(), out RolID);
                        int getid = 0;
                        _objRolelink.UserID = UserID; _objRolelink.RoleID = RolID; _objRolelink.Status = ProStaus;
                        getid = _clsUser.UserRoleLinkMaster_Add(_objRolelink);
                    }
                }
            }
        }
        public PartialViewResult GridUserMaster(int CustomerID = 0)
        {
            _objUserMaster.CustomerID = CustomerID; _objUserMaster.Status = CONT.mkstatus;
            var Data = _clsUser.UserMasterHistory_ListAllBind(_objUserMaster).ToList();
            return PartialView(Data);
        }
        public ActionResult ViewUserMaster(int CustomerID = 0, int CustomerTypeID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (CustomerID > 0)
                    {
                        _objUserMaster.CustomerID = CustomerID; _objUserMaster.Status = CONT.NA;
                        ViewBag.UserData = _clsUser.UserMasterHistory_ListAll(_objUserMaster).ToList();

                        _objUserMaster.CustomerID = CustomerID; _objUserMaster.Status = currentStatus;
                        var Data = _clsUser.UserMasterHistory_ListAllBind(_objUserMaster).ToList();
                        ViewBag.UserMasterData = Data;
                        ViewBag.CustomerTypeID = CustomerTypeID;
                        ViewBag.CustomerID = CustomerID;
                        _objUserMaster.CustomerID = CustomerID;
                        _objUserMaster.CustomerTypeID = CustomerTypeID;
                        if (ViewBag.UserData.Count == 0) { ViewBag.UserData = ViewBag.UserMasterData; }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                ViewBag.CustomerTypeID = CustomerTypeID;
                ViewBag.CustomerID = CustomerID;
                _objUserMaster.CustomerID = CustomerID;
                _objUserMaster.CustomerTypeID = CustomerTypeID;
                return View(_objUserMaster);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }


        public ActionResult Documents(int CustomerID = 0)
        {
            if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
            {
                ViewBag.ErrorMesssage = Request.QueryString["ErrorMsg"].ToString();
            }
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                //string MenuId = Convert.ToString(Session["MenuID"]);
                //int ParentMenuID = 0;
                //int.TryParse(MenuId, out ParentMenuID);
                //var UserRight = fn.CheckUserRight("", "", ParentMenuID);
                //if (UserRight != null)
                //{
                //    ViewBag.UserRight = UserRight.MenuName;
                //}
                //ViewBag.UserRight = "Maker";

                CustomerDocModel _objModel = new CustomerDocModel();
                CustomerTypeDocumentDetails_ListAll_Result _objSubModel = new CustomerTypeDocumentDetails_ListAll_Result();
                CustomerMasterModel _objCustModel = new CustomerMasterModel();

                _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();

                _objCustModel.CustomerID = CustomerID;
                var Data = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                if (Data != null)
                {
                    _objSubModel.CustomerTypeID = Data.CustomerTypeID;
                    _objModel.CustomerTypeID = Data.CustomerTypeID;
                    ViewBag.IsAdminRegister = Data.IsAdminRegister;
                }
                _objSubModel.IsActive = true;
                try
                {
                    _objModel.CustomerID = CustomerID;
                    _objModel.IsUserDoc = _objSubModel.IsUserDoc = false;
                    _objModel.IsInvestor = _objSubModel.IsInvestor = true;
                    _objSubModel.Status = CONT.ddstatus;
                    _objModel.lstCustomerTypeDocumentList = _clsCust.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();
                    if (_objModel.CustomerID > 0)
                    {

                        _objModel.IsUserDocInt = -1;
                        _objModel.IsInvestorInt = -1;
                        var attachList = _clsCust.CustomerDocDetail_ListAll(_objModel).ToList();

                        if (attachList.Count == 0)
                        {
                            _objModel.lstCustomerTypeDocumentList = _clsCust.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();
                        }
                        else
                        {

                            _objModel.lstCustomerDocDetail = _clsCust.CustomerDocDetailHistory_ListAllBind(_objModel).ToList();
                            _objModel.lstCustomerTypeDocumentList = _clsCust.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();

                            for (int i = 0; i < _objModel.lstCustomerDocDetail.Count; i++)
                            {
                                if (_objModel.lstCustomerDocDetail[i].Attachment != "")
                                {
                                    Session["hdAttchFileName_" + _objModel.lstCustomerDocDetail[i].DocumentID] = _objModel.lstCustomerDocDetail[i].Attachment;
                                }
                            }
                            _objModel.Status = attachList[0].Status;

                        }

                    }
                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    _objModel.lstCustomerTypeDocumentList = _clsCust.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();

                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult Documents(CustomerDocModel _objModel, FormCollection frm, ControllerContext controllerContext)
        {
            _clsCust.Conn = ClsAppDatabase.GetCon();
            if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
            _clsCust.Tras = _clsCust.Conn.BeginTransaction();
            _objModel.IsInvestorInt = -1;
            _objModel.IsUserDocInt = -1;
            var attaDocList = _clsCust.CustomerDocDetail_ListAll(_objModel).ToList();
            try
            {
                if (Request["Submit"] != null)
                {
                    var CustGetCount = _clsCust.CustomerMaster_GetCount(_objModel).FirstOrDefault();

                    if (CustGetCount != null)
                    {

                        if (CustGetCount.CustomerInfoDetail == 0)
                        {
                            return RedirectToAction("CompanyMoreInfo", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }
                        else if (CustGetCount.CustomerShareHoldDetailAuth == 0)
                        {
                            return RedirectToAction("AuthSignDetails", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }
                        //else if (CustGetCount.CustomerShareHoldDetail == 0)
                        //{
                        //    return RedirectToAction("Shareholders", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        //}
                        else if (CustGetCount.CustomerBankAccDetail == 0)
                        {
                            return RedirectToAction("BankAccountDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }

                        else if (CustGetCount.CustomerSupplierDetail == 0 && ((CustGetCount.CustomerTypeID == CONT.BuyerCustomerTypeID && CustGetCount.ProgramType == CONT.RType) ||
                            (CustGetCount.CustomerTypeID == CONT.BothObligorAndBuyerTypeID && CustGetCount.ProgramType == CONT.BothProgramType)))
                        {
                            return RedirectToAction("SupplierDetail", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }
                        else if (CustGetCount.CustomerDocDetail == 0)
                        {
                            return RedirectToAction("Documents", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                        }
                    }
                    if (Request["cb1"] == "cb1")
                    {
                        var CheckScreening = _clsCust.CustomerMaster_CheckScreening(_objModel).FirstOrDefault();
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                        {
                            if (CheckScreening.IsCheckScreening)
                            {
                                _objModel.Status = CONT.UR;
                            }
                            else
                            {
                                _objModel.Status = CONT.UV;
                            }
                        }
                        else
                        {
                            if (CheckScreening.IsOPTeam)
                            {
                                if (CheckScreening.IsCheckScreening)
                                {
                                    _objModel.Status = CONT.NR;
                                }
                                else
                                {
                                    _objModel.Status = CONT.NV;
                                }
                            }
                            else
                            {
                                _objModel.Status = CONT.NR;
                            }

                        }
                        _clsCust.CustomerMaster_UpdateIsAgree(_objModel);
                        _clsCust.CustomerMaster_UpdateAllStatus(_objModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA || _objModel.Status == CONT.UP)
                        {
                            sendNotification(_objModel.CustomerID, CONT.Email_3rd);
                            sendNotification(_objModel.CustomerID, CONT.Supplier_Email_3rd);
                            return RedirectToAction("CMNDashboard", "Home");
                        }
                        else
                        {
                            sendNotification(_objModel.CustomerID, CONT.Email_3rd);
                            sendNotification(_objModel.CustomerID, CONT.Supplier_Email_3rd);
                            return RedirectToAction("ThankYou", "OnlineReg", new { CustomerID = _objModel.CustomerID, temp_name = CONT.Thanku_3rd });
                        }

                    }
                    else
                    {
                        var CheckScreening = _clsCust.CustomerMaster_CheckScreening(_objModel).FirstOrDefault();
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                        {

                            if (CheckScreening.IsCheckScreening)
                            {
                                _objModel.Status = CONT.UR;
                            }
                            else
                            {
                                _objModel.Status = CONT.UV;
                            }
                        }
                        else
                        {
                            if (CheckScreening.IsOPTeam)
                            {
                                if (CheckScreening.IsCheckScreening)
                                {
                                    _objModel.Status = CONT.NR;
                                }
                                else
                                {
                                    _objModel.Status = CONT.NV;
                                }
                            }

                            else
                            {
                                _objModel.Status = CONT.NR;
                            }
                        }
                        _clsCust.CustomerMaster_UpdateAllStatus(_objModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        #region Screening
                        //if (_objModel.Status == CONT.NR || _objModel.Status == CONT.UR || _objModel.Status == CONT.NH || _objModel.Status == CONT.UH)
                        //{
                        //    #region Screening Record
                        //    CustomerShareHolderModel _objCustShareHolder = new CustomerShareHolderModel();
                        //    _objCustShareHolder.CustomerID = _objModel.CustomerID;
                        //    _objCustShareHolder.PlatformID = PlatformID;
                        //    var CountScreeningData = _ClsThomsonReuters.ScreeningCustomer_ListAll(_objCustShareHolder).ToList();
                        //    foreach (var item in CountScreeningData)
                        //    {
                        //        Int16 intCustomerShareHoldID = 0;

                        //        _ThomsonReutersModel.PlatformID = PlatformID;
                        //        _ThomsonReutersModel.CustomerID = item.CustomerID;
                        //        _ThomsonReutersModel.CustomerShareHoldID = item.CustomerShareHoldID;
                        //        _ThomsonReutersModel.UserID = item.UserID;
                        //        _ThomsonReutersModel.CustomerBankAccID = item.CustomerBankAccID;
                        //        _ThomsonReutersModel.BankID = item.BankID;

                        //        _ThomsonReutersModel.CustType = item.CustType;
                        //        _ThomsonReutersModel.FirstName = item.FirstName;
                        //        _ThomsonReutersModel.MiddleName = item.MiddleName;
                        //        _ThomsonReutersModel.LastName = item.LastName;
                        //        _ThomsonReutersModel.CountryID = item.NatCountryID;
                        //        _ThomsonReutersModel.LocCountryID = item.LocCountryID;
                        //        _ThomsonReutersModel.LocCountryCode = item.LocCountryCode;
                        //        _ThomsonReutersModel.NatCountryCode = item.NatCountryCode;
                        //        _ThomsonReutersModel.Gender = item.Gender;
                        //        _ThomsonReutersModel.IsCompany = false;
                        //        _ThomsonReutersModel.CompanyName = "";

                        //        if (item.CustType == "C" || item.CustType == "M" || item.CustType == "N" || item.CustType == "T" || item.CustType == "K")
                        //        {
                        //            _ThomsonReutersModel.IsCompany = true;
                        //            _ThomsonReutersModel.CountryID = item.NatCountryID;
                        //            _ThomsonReutersModel.CompanyName = item.Name;
                        //        }

                        //        var Verifycase = _ClsThomsonReuters.ScreeningMaster_VerifycaseID(_ThomsonReutersModel).FirstOrDefault();
                        //        if (Verifycase != null)
                        //        {
                        //            string CaseID = ""; string strcaseSystemId = "";

                        //            #region Step 1
                        //            bool IsNew = Verifycase.IsNew;
                        //            if (IsNew)
                        //            {
                        //                ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
                        //                clsTRLog.Conn = ClsAppDatabase.GetCon();
                        //                if (clsTRLog.Conn.State == ConnectionState.Closed) { clsTRLog.Conn.Open(); }
                        //                else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
                        //                clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

                        //                CaseID = clsTRLog.ScreeningMaster_Add(_ThomsonReutersModel);

                        //                clsTRLog.Tras.Commit(); clsTRLog.Conn.Close();

                        //                caseIdentifiers(CaseID, _objModel.CustomerID, intCustomerShareHoldID);
                        //            }
                        //            #endregion

                        //            #region Step 2
                        //            if (Verifycase.caseSystemId == "")
                        //            {
                        //                if (CaseID != "")
                        //                    strcaseSystemId = caseSystemId(CaseID, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode);
                        //                else
                        //                    strcaseSystemId = caseSystemId(Verifycase.caseId, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode);
                        //            }
                        //            #endregion

                        //            #region Step 3
                        //            if (!Verifycase.IsScreening)
                        //            {
                        //                if (Verifycase.caseSystemId != "")
                        //                {
                        //                    screeningRequest(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                }
                        //                else
                        //                {
                        //                    if (strcaseSystemId != null && strcaseSystemId != "")
                        //                    {
                        //                        if (CaseID != "")
                        //                            screeningRequest(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                        else
                        //                            screeningRequest(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                    }
                        //                }
                        //            }
                        //            #endregion

                        //            #region Step 4
                        //            if (!Verifycase.IsResolved)
                        //            {
                        //                if (Verifycase.caseSystemId != "")
                        //                {

                        //                    GetScreeningResults(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                        //                    #region Is Delete Result And Re-Searching Logic
                        //                    if (ViewBag.IsDeleteResult == true)
                        //                    {
                        //                        Verifycase = _ClsThomsonReuters.ScreeningMaster_VerifycaseID(_ThomsonReutersModel).FirstOrDefault();
                        //                        if (Verifycase != null)
                        //                        {
                        //                            CaseID = ""; strcaseSystemId = "";

                        //                            #region Step 1
                        //                            IsNew = Verifycase.IsNew;
                        //                            if (IsNew)
                        //                            {
                        //                                ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
                        //                                clsTRLog.Conn = ClsAppDatabase.GetCon();
                        //                                if (clsTRLog.Conn.State == ConnectionState.Closed) { clsTRLog.Conn.Open(); }
                        //                                else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
                        //                                clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

                        //                                CaseID = clsTRLog.ScreeningMaster_Add(_ThomsonReutersModel);
                        //                                clsTRLog.Tras.Commit(); clsTRLog.Conn.Close();
                        //                                caseIdentifiers(CaseID, _objModel.CustomerID, intCustomerShareHoldID);
                        //                            }
                        //                            #endregion

                        //                            #region Step 2
                        //                            if (Verifycase.caseSystemId == "")
                        //                            {
                        //                                if (CaseID != "")
                        //                                    strcaseSystemId = caseSystemId(CaseID, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode, true);
                        //                                else
                        //                                    strcaseSystemId = caseSystemId(Verifycase.caseId, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode, true);
                        //                            }
                        //                            #endregion

                        //                            #region Step 3
                        //                            if (!Verifycase.IsScreening)
                        //                            {
                        //                                if (Verifycase.caseSystemId != "")
                        //                                {
                        //                                    screeningRequest(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                                }
                        //                                else
                        //                                {
                        //                                    if (strcaseSystemId != null && strcaseSystemId != "")
                        //                                    {
                        //                                        if (CaseID != "")
                        //                                            screeningRequest(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                                        else
                        //                                            screeningRequest(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                                    }
                        //                                }
                        //                            }
                        //                            #endregion

                        //                            #region Step 4
                        //                            if (!Verifycase.IsResolved)
                        //                            {
                        //                                if (Verifycase.caseSystemId != "")
                        //                                {

                        //                                    GetScreeningResults(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);

                        //                                    //EnableOngoingScreeningByUser(Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                        //                                    _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                        //                                    if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                        //                                    { _ClsThomsonReuters.Conn.Open(); }
                        //                                    else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                        //                                    _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                        //                                    _ThomsonReutersModel.caseSystemId = Verifycase.caseSystemId;
                        //                                    _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                        //                                    _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                        //                                }
                        //                                else
                        //                                {
                        //                                    if (strcaseSystemId != null && strcaseSystemId != "")
                        //                                    {
                        //                                        if (CaseID != "")
                        //                                            GetScreeningResults(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);
                        //                                        else
                        //                                            GetScreeningResults(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);
                        //                                        //EnableOngoingScreeningByUser(VerifyCaseSystemID.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                        //                                        _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                        //                                        if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                        //                                        { _ClsThomsonReuters.Conn.Open(); }
                        //                                        else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                        //                                        _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                        //                                        _ThomsonReutersModel.caseSystemId = strcaseSystemId;
                        //                                        _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                        //                                        _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                        //                                    }
                        //                                }
                        //                            }
                        //                            #endregion
                        //                        }
                        //                    }
                        //                    #endregion
                        //                    //EnableOngoingScreeningByUser(Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                        //                    _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                        //                    if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                        //                    { _ClsThomsonReuters.Conn.Open(); }
                        //                    else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                        //                    _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                        //                    _ThomsonReutersModel.caseSystemId = Verifycase.caseSystemId;
                        //                    _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                        //                    _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                        //                }
                        //                else
                        //                {
                        //                    if (strcaseSystemId != null && strcaseSystemId != "")
                        //                    {
                        //                        if (CaseID != "")
                        //                            GetScreeningResults(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                        else
                        //                            GetScreeningResults(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                        //EnableOngoingScreeningByUser(VerifyCaseSystemID.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                        //                        #region Is Delete Result And Re-Searching Logic
                        //                        if (ViewBag.IsDeleteResult == true)
                        //                        {
                        //                            Verifycase = _ClsThomsonReuters.ScreeningMaster_VerifycaseID(_ThomsonReutersModel).FirstOrDefault();
                        //                            if (Verifycase != null)
                        //                            {
                        //                                CaseID = ""; strcaseSystemId = "";

                        //                                #region Step 1
                        //                                IsNew = Verifycase.IsNew;
                        //                                if (IsNew)
                        //                                {
                        //                                    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
                        //                                    clsTRLog.Conn = ClsAppDatabase.GetCon();
                        //                                    if (clsTRLog.Conn.State == ConnectionState.Closed) { clsTRLog.Conn.Open(); }
                        //                                    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
                        //                                    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

                        //                                    CaseID = clsTRLog.ScreeningMaster_Add(_ThomsonReutersModel);
                        //                                    clsTRLog.Tras.Commit(); clsTRLog.Conn.Close();
                        //                                    caseIdentifiers(CaseID, _objModel.CustomerID, intCustomerShareHoldID);
                        //                                }
                        //                                #endregion

                        //                                #region Step 2
                        //                                if (Verifycase.caseSystemId == "")
                        //                                {
                        //                                    if (CaseID != "")
                        //                                        strcaseSystemId = caseSystemId(CaseID, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode, true);
                        //                                    else
                        //                                        strcaseSystemId = caseSystemId(Verifycase.caseId, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode, true);
                        //                                }
                        //                                #endregion

                        //                                #region Step 3
                        //                                if (!Verifycase.IsScreening)
                        //                                {
                        //                                    if (Verifycase.caseSystemId != "")
                        //                                    {
                        //                                        screeningRequest(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        if (strcaseSystemId != null && strcaseSystemId != "")
                        //                                        {
                        //                                            if (CaseID != "")
                        //                                                screeningRequest(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                                            else
                        //                                                screeningRequest(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                        //                                        }
                        //                                    }
                        //                                }
                        //                                #endregion

                        //                                #region Step 4
                        //                                if (!Verifycase.IsResolved)
                        //                                {
                        //                                    if (Verifycase.caseSystemId != "")
                        //                                    {

                        //                                        GetScreeningResults(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);

                        //                                        //EnableOngoingScreeningByUser(Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                        //                                        _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                        //                                        if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                        //                                        { _ClsThomsonReuters.Conn.Open(); }
                        //                                        else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                        //                                        _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                        //                                        _ThomsonReutersModel.caseSystemId = Verifycase.caseSystemId;
                        //                                        _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                        //                                        _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        if (strcaseSystemId != null && strcaseSystemId != "")
                        //                                        {
                        //                                            if (CaseID != "")
                        //                                                GetScreeningResults(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);
                        //                                            else
                        //                                                GetScreeningResults(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);
                        //                                            //EnableOngoingScreeningByUser(VerifyCaseSystemID.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                        //                                            _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                        //                                            if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                        //                                            { _ClsThomsonReuters.Conn.Open(); }
                        //                                            else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                        //                                            _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                        //                                            _ThomsonReutersModel.caseSystemId = strcaseSystemId;
                        //                                            _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                        //                                            _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                        //                                        }
                        //                                    }
                        //                                }
                        //                                #endregion
                        //                            }
                        //                        }
                        //                        #endregion

                        //                        _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                        //                        if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                        //                        { _ClsThomsonReuters.Conn.Open(); }
                        //                        else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                        //                        _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                        //                        _ThomsonReutersModel.caseSystemId = strcaseSystemId;
                        //                        _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                        //                        _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                        //                    }
                        //                }
                        //            }
                        //            #endregion
                        //        }
                        //    }

                        //    #endregion
                        //}
                        #endregion
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                }

                if (attaDocList.Count == 0)
                {
                    int intMenuCount = Convert.ToInt32(Request.Form["intMenuCount"]);
                    for (int i = 0; i < intMenuCount; i++)
                    {
                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            //DocumentID , HttpPostedFileBase[] files
                            string DocumentIDChar = Convert.ToString(frm["item.DocumentID"]);
                            var ID2 = DocumentIDChar.Split(',');
                            int DocumentID;
                            DocumentID = Convert.ToInt32(ID2[i]);
                            if (GetAllkeysStringValue.Equals("chk_" + DocumentID + ""))
                            {
                                string DocumentNoChar = Convert.ToString(frm["DocumentNo"]);
                                string DocumentNo = "";
                                if (DocumentNoChar != null)
                                {
                                    var ID5 = DocumentNoChar.Split(',');
                                    DocumentNo = Convert.ToString(ID5[i]);
                                }
                                var path = "";
                                HttpFileCollectionBase files = Request.Files;
                                var fileName = "";
                                string gu = Guid.NewGuid().ToString().Substring(0, 12);
                                string FileLocation = "";
                                for (int z = 0; z < files.Count; z++)
                                {
                                    string GetAllkeysUploadFile = files.AllKeys[z];

                                    if (GetAllkeysUploadFile.Equals("fileCantrol_" + DocumentID + ""))
                                    {
                                        HttpPostedFileBase file = files[z];
                                        if (file != null && file.ContentLength > 0)
                                        {
                                            fileName = System.IO.Path.GetFileName(file.FileName);
                                            FileLocation = "1_" + DocumentID + "_" + fileName;
                                            path = Path.Combine(Server.MapPath("~\\Files\\Upload\\"), FileLocation);
                                            file.SaveAs(path);
                                        }
                                    }
                                }
                                _objModel.DocumentID = DocumentID; _objModel.Attachment = FileLocation;
                                _objModel.Status = CONT.RQ;
                                _clsCust.CustomerDocDetail_Add(_objModel);
                            }
                        }
                    }
                }
                else
                {
                    int intMenuCount = Convert.ToInt32(Request.Form["intMenuCount"]);
                    for (int i = 0; i < intMenuCount; i++)
                    {
                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];

                            string DocumentIDChar = Convert.ToString(frm["item1.DocumentID"] + "," + frm["item.DocumentID"]);
                            var ID2 = DocumentIDChar.Split(',');
                            int DocumentID;

                            DocumentID = Convert.ToInt32(ID2[i]);
                            if (GetAllkeysStringValue.Equals("chk_" + DocumentID + ""))
                            {
                                //Document No
                                string DocumentNoChar = Convert.ToString(frm["item1.DocumentNo"]);
                                string DocumentNo = "";
                                if (DocumentNoChar != null)
                                {
                                    var ID5 = DocumentNoChar.Split(',');
                                    DocumentNo = Convert.ToString(ID5[i]);
                                }

                                int CustomerDocID = 0;
                                for (int p = 0; p < attaDocList.Count; p++)
                                {

                                    CustomerDocModel obj = (CustomerDocModel)attaDocList[p];
                                    if (DocumentID == obj.DocumentID)
                                    {
                                        CustomerDocID = obj.CustomerDocID;
                                    }
                                }
                                //Comment


                                //Guid gu = Guid.NewGuid();
                                string gu = Guid.NewGuid().ToString().Substring(0, 12);
                                HttpFileCollectionBase files = Request.Files;
                                var fileName = "";
                                if (Session["hdAttchFileName_" + DocumentID] != null && Convert.ToString(Session["hdAttchFileName_" + DocumentID]) != "")
                                {
                                    fileName = Convert.ToString(Session["hdAttchFileName_" + DocumentID]);
                                }
                                for (int z = 0; z < files.Count; z++)
                                {
                                    string GetAllkeysUploadFile = files.AllKeys[z];

                                    if (GetAllkeysUploadFile.Equals("fileCantrol_" + DocumentID + ""))
                                    {
                                        HttpPostedFileBase file = files[z];

                                        if (file != null && file.ContentLength > 0)
                                        {
                                            fileName = gu + Path.GetFileName(file.FileName);
                                            file.SaveAs(Server.MapPath("\\Files\\Upload\\" + fileName));

                                        }
                                    }
                                }

                                _objModel.DocumentID = DocumentID; _objModel.Attachment = fileName; _objModel.CustomerDocID = CustomerDocID;
                                if (Convert.ToInt32(CustomerDocID) > 0)
                                {
                                    //_objModel.Status = CONT.RQ;
                                    if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                                    {
                                        _objModel.Status = CONT.UR;
                                        _clsCust.CustomerDocDetailHistory_Add(_objModel);
                                    }
                                    else
                                    {
                                        _clsCust.CustomerDocDetail_Update(_objModel);
                                    }

                                }
                                else
                                {
                                    if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                                    {
                                        _objModel.Status = CONT.UR;
                                        _clsCust.CustomerDocDetail_Add(_objModel);
                                    }
                                    else
                                    {
                                        _objModel.Status = CONT.RQ;
                                        _clsCust.CustomerDocDetail_Add(_objModel);
                                    }


                                }

                            }
                        }
                    }
                }

                _clsCust.Tras.Commit();
                _clsCust.Conn.Close();
                return RedirectToAction("Documents", "CustomerReg", new { CustomerID = _objModel.CustomerID });
            }
            catch (Exception ex)
            {
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (_clsCust.Conn.State == ConnectionState.Open)
                {
                    _clsCust.Tras.Rollback();
                    _clsCust.Conn.Close();
                }
                string strErrorMsg = "";
                if (ex.InnerException == null)
                    strErrorMsg = ex.Message;
                else
                    strErrorMsg = ex.InnerException.Message;

                CustomerTypeDocumentDetails_ListAll_Result _objSubModel = new CustomerTypeDocumentDetails_ListAll_Result();
                _objSubModel.CustomerTypeID = _objModel.CustomerTypeID;

                _objModel.lstCustomerTypeDocumentList = _clsCust.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();

                CustomerMasterModel _objCustModel = new CustomerMasterModel();
                _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();

                return RedirectToAction("Documents", "CustomerReg", new { CustomerID = _objModel.CustomerID, ErrorMsg = strErrorMsg });
            }

        }
        #endregion

        #region Admin
        public ActionResult MakerIndex(string Status = "", int CustomerTypeID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                Session["IsEditProfile"] = false;
                CustomerMasterModel _objModel = new CustomerMasterModel();
                try
                {
                    if (CustomerTypeID == CONT.BuyerCustomerTypeID || CustomerTypeID == CONT.BothObligorAndBuyerTypeID || CustomerTypeID == CONT.ObligorCustomerTypeID)
                    {
                        _objModel.CustomerTypeIDs = CONT.BuyerCustomerTypeID + "," + CONT.BothObligorAndBuyerTypeID + "," + CONT.ObligorCustomerTypeID;
                        _objModel.Status = Status;
                    }
                    else
                    {
                        _objModel.CustomerTypeID = CustomerTypeID; _objModel.Status = Status;
                    }

                    var Data = _clsCust.CustomerMaster_ListAll(_objModel);
                    ViewBag.MakerIndex = _objModel;
                    return View(Data);
                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult MakerIndex(CustomerMasterModel _objModel)
        {
            if (Request["Maker"] == "Maker")
            {
                if (_objModel.CustomerTypeID == CONT.ObligorCustomerTypeID)
                {
                    return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerTypeID = _objModel.CustomerTypeID });
                }
                else if (_objModel.CustomerTypeID == CONT.SupplierCustomerTypeID)
                {
                    return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerTypeID = _objModel.CustomerTypeID, ProgramType = CONT.FType });
                }
                else
                {
                    return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerTypeID = _objModel.CustomerTypeID });
                }
            }
            return View();
        }
        public ActionResult Index(string Status = "", int CustomerTypeID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                Session["IsEditProfile"] = false;
                CustomerMasterModel _objModel = new CustomerMasterModel();
                try
                {
                    if (CustomerTypeID == CONT.BuyerCustomerTypeID || CustomerTypeID == CONT.BothObligorAndBuyerTypeID || CustomerTypeID == CONT.ObligorCustomerTypeID)
                    {
                        _objModel.CustomerTypeIDs = CONT.BuyerCustomerTypeID + "," + CONT.BothObligorAndBuyerTypeID + "," + CONT.ObligorCustomerTypeID;
                        _objModel.Status = Status;
                    }
                    else
                    {
                        _objModel.CustomerTypeID = CustomerTypeID; _objModel.Status = Status;
                    }

                    var Data = _clsCust.CustomerMasterHistory_ListAllBind(_objModel);
                    return View(Data);
                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }


        }
        public ActionResult CompanyInfoDetails(int CustomerID = 0, bool IsEditProfile = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                {
                    Session["IsEditProfile"] = true;
                }
                else { Session["IsEditProfile"] = false; }

                CustomerMasterModel _objModel = new CustomerMasterModel();
                _objModel.CustomerID = CustomerID;
                _objModel.Status = CONT.NA + ',' + CONT.UA;
                ViewBag.CustCheck = _clsCust.CustomerMasterHistory_ListAll(_objModel).FirstOrDefault();

                _objModel.Status = currentStatus;
                _objModel = _clsCust.CustomerMasterHistory_ListAllBind(_objModel).FirstOrDefault();

                if (ViewBag.CustCheck == null) { ViewBag.CustCheck = _objModel; }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult CompanyInfoDetails(CustomerMasterModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    if (Request["Next"] != null)
                    {
                        return RedirectToAction("CompanyMoreInfoDetails", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status });
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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult CompanyMoreInfoDetails(int CustomerID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                ViewBag.Global_CompanyTypeName = System.Configuration.ConfigurationManager.AppSettings["CompanyTypeName"];
                CustomerInfoModel _objModel = new CustomerInfoModel();
                _objModel.CustomerID = CustomerID;
                _objModel.Status = CONT.NA + ',' + CONT.UA;
                ViewBag.MoreCheck = _clsCust.CustomerInfoDetailHistory_ListAll(_objModel).FirstOrDefault();

                _objModel.Status = currentStatus;
                _objModel = _clsCust.CustomerInfoDetailHistory_ListAllBind(_objModel).FirstOrDefault();

                _objModel.Status = CONT.NA;
                ViewBag.CreditDetailCheck = _clsCust.CustomerBankCreditDetailHistory_ListAll(_objModel);

                _objModel.Status = currentStatus;
                ViewBag.CreditDetailGrid = _clsCust.CustomerBankCreditDetailHistory_ListAllBind(_objModel);

                if (ViewBag.CreditDetailCheck.Count == 0) { ViewBag.CreditDetailCheck = ViewBag.CreditDetailGrid; }
                if (ViewBag.MoreCheck == null) { ViewBag.MoreCheck = _objModel; }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }

        }
        [HttpPost]
        public ActionResult CompanyMoreInfoDetails(CustomerMasterModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    if (Request["Next"] != null)
                    {
                        return RedirectToAction("CommonDetails", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status });
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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult CommonDetails(int CustomerID = 0, bool IsEditProfile = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                { Session["IsEditProfile"] = true; }
                else { Session["IsEditProfile"] = false; }
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }

                CustomerShareHolderModel _objModel = new CustomerShareHolderModel();

                try
                {
                    _objModel.CustomerID = CustomerID;
                    if (_objModel.CustomerID > 0)
                    {
                        CustomerMasterModel _objCustModel = new CustomerMasterModel();
                        _objCustModel.CustomerID = CustomerID;
                        _objCustModel = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                        _objModel.CompPubList = _objCustModel.CompPubList;

                        _objModel.CustomerID = CustomerID;
                        _objModel.CustType = "D,A";
                        _objModel.Status = CONT.NA;
                        ViewBag.DirAuthSignCheck = _clsCust.CustomerShareHoldDetailHistory_ListAll(_objModel);

                        _objModel.Status = currentStatus;
                        ViewBag.DirAuthSignGrid = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);


                        _objModel.CustType = "S";
                        _objModel.Status = CONT.NA;
                        ViewBag.SharHoldCheck = _clsCust.CustomerShareHoldDetailHistory_ListAll(_objModel);

                        _objModel.Status = currentStatus;
                        ViewBag.SharHoldGrid = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                        _objModel.CustType = "C";
                        _objModel.Status = CONT.NA;
                        ViewBag.CompSharHoldCheck = _clsCust.CustomerShareHoldDetailHistory_ListAll(_objModel);

                        _objModel.Status = currentStatus;
                        ViewBag.CompSharHoldGrid = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                        if (ViewBag.DirAuthSignCheck.Count == 0) { ViewBag.DirAuthSignCheck = ViewBag.DirAuthSignGrid; }
                        if (ViewBag.SharHoldCheck.Count == 0) { ViewBag.SharHoldCheck = ViewBag.SharHoldGrid; }
                        if (ViewBag.CompSharHoldCheck.Count == 0) { ViewBag.CompSharHoldCheck = ViewBag.CompSharHoldGrid; }
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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult CommonDetails(CustomerShareHolderModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    if (Request["Next"] != null)
                    {
                        return RedirectToAction("BankAccountDetailsList", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status });
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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult BankAccountDetailsList(int CustomerID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }

                CustomerBankAccDetail _objModel = new CustomerBankAccDetail();
                try
                {
                    _objModel.CustomerID = CustomerID;
                    if (_objModel.CustomerID > 0)
                    {
                        _objModel.CustomerID = CustomerID;
                        _objModel.Status = CONT.NA;
                        ViewBag.BankDetailCheck = _clsCust.CustomerBankAccDetailHistory_ListAll(_objModel);

                        _objModel.Status = currentStatus;
                        ViewBag.BankDetailGrid = _clsCust.CustomerBankAccDetailHistory_ListAllBind(_objModel);

                        if (ViewBag.BankDetailCheck.Count == 0) { ViewBag.BankDetailCheck = ViewBag.BankDetailGrid; }

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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult BankAccountDetailsList(CustomerBankAccDetail _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    if (Request["Next"] != null)
                    {
                        if (_objModel.CustomerID > 0)
                        {
                            CustomerMasterModel _objCustModel = new CustomerMasterModel();
                            _objCustModel.CustomerID = _objModel.CustomerID;
                            _objCustModel = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                            if ((_objCustModel.CustomerTypeID == CONT.BuyerCustomerTypeID && _objCustModel.ProgramType == CONT.RType) ||
                                (_objCustModel.CustomerTypeID == CONT.BothObligorAndBuyerTypeID && _objCustModel.ProgramType == CONT.BothProgramType))
                            {
                                return RedirectToAction("SupplierDetailList", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status });
                            }
                            else
                            {
                                return RedirectToAction("ViewUserMaster", "CustomerReg", new { CustomerID = _objModel.CustomerID, CustomerTypeID = _objCustModel.CustomerTypeID, currentStatus = _objModel.Status });
                                //return RedirectToAction("DocumentsDetails", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status });
                            }
                        }

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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult SupplierDetailList(int CustomerID = 0, bool IsEditProfile = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                { Session["IsEditProfile"] = true; }
                else { Session["IsEditProfile"] = false; }

                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }

                CustomerSupplierModel _objModel = new CustomerSupplierModel();
                try
                {
                    _objModel.CustomerID = CustomerID;
                    if (_objModel.CustomerID > 0)
                    {
                        _objModel.CustomerID = CustomerID;
                        _objModel.Status = CONT.NA;
                        ViewBag.SupplierDetailCheck = _clsCust.CustomerSupplierDetailHistory_ListAll(_objModel);

                        _objModel.Status = currentStatus;
                        var GetData = _clsCust.CustomerSupplierDetailHistory_ListAllBind(_objModel);
                        ViewBag.SupplierDetailGrid = GetData;
                        if (ViewBag.SupplierDetailGrid.Count != 0)
                        {
                            ViewBag.ProgramType = GetData[0].ProgramType;
                        }
                        CustomerBankAccDetail _objBankModel = new CustomerBankAccDetail();
                        _objBankModel.CustomerID = CustomerID;
                        _objBankModel.IsCustomerSupp = true;
                        _objBankModel.Status = CONT.NA;
                        ViewBag.BankDetailCheck = _clsCust.CustomerBankAccDetailHistory_ListAll(_objBankModel);

                        _objBankModel.Status = currentStatus;
                        ViewBag.BankDetailGrid = _clsCust.CustomerBankAccDetailHistory_ListAllBind(_objBankModel);

                        if (ViewBag.BankDetailCheck.Count == 0) { ViewBag.BankDetailCheck = ViewBag.BankDetailGrid; }
                        if (ViewBag.SupplierDetailCheck.Count == 0) { ViewBag.SupplierDetailCheck = ViewBag.SupplierDetailGrid; }
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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult SupplierDetailList(CustomerSupplierModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    if (Request["Next"] != null)
                    {
                        return RedirectToAction("ViewUserMaster", "CustomerReg", new { CustomerID = _objModel.CustomerID, CustomerTypeID = _objModel.CustomerTypeID, currentStatus = _objModel.Status });
                        //return RedirectToAction("DocumentsDetails", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status });

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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult DocumentsDetails(int CustomerID = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }

                CustomerDocModel _objModel = new CustomerDocModel();
                try
                {

                    _objModel.Status = currentStatus;
                    _objModel.CustomerID = CustomerID;
                    _objModel.IsUserDocInt = -1;
                    _objModel.IsInvestorInt = 1;
                    _objModel.Status = CONT.NA;
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();

                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();

                    _objCustModel.CustomerID = CustomerID;
                    var Data = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                    if (Data != null)
                    {
                        _objModel.CustomerTypeID = Data.CustomerTypeID;
                        _objModel.ProgramType = Data.ProgramType;
                    }
                    ViewBag.DocCheck = _clsCust.CustomerDocDetailHistory_ListAll(_objModel).ToList();

                    _objModel.Status = currentStatus;
                    var attachList = _clsCust.CustomerDocDetailHistory_ListAllBind(_objModel).ToList();
                    ViewBag.attachList = attachList;


                    _objCustModel.CustomerID = CustomerID;
                    _objCustModel.Status = currentStatus;
                    var userAttach = _clsCust.CustomerMaster_BindAllDocument(_objCustModel);
                    ViewBag.userAttach = userAttach;
                }

                catch (Exception ex)
                {

                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                if (ViewBag.DocCheck.Count == 0) { ViewBag.DocCheck = ViewBag.attachList; }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult DocumentsDetails(CustomerDocModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    if (Request["Next"] != null)
                    {
                        CustomerMasterModel _objCustModel = new CustomerMasterModel();
                        _objCustModel.CustomerID = _objModel.CustomerID;
                        _objCustModel = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();

                        _objModel.Status = _objCustModel.Status;
                        return RedirectToAction("ComplianceScreening", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, SendToCredit = true, MenuOrderNo = 1 });
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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        //#region Thomson Reuters Integration
        //public void caseIdentifiers(string caseId, int CustomerID, int CustomerShareHoldID)
        //{
        //    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    clsTRLog.Conn = ClsAppDatabase.GetCon();
        //    if (clsTRLog.Conn.State == ConnectionState.Closed)
        //    { clsTRLog.Conn.Open(); }
        //    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

        //    try
        //    {
        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "caseIdentifiers?caseId=" + caseId;

        //        _objModel.CustomerID = CustomerID;
        //        _objModel.CustomerShareHoldID = CustomerShareHoldID;
        //        _objModel.LogType = CONT.CaseIDVerificationRequest;
        //        _objModel.caseId = caseId;
        //        _objModel.Request = requestendpoint;
        //        _objModel.LogID = clsTRLog.APILogDetail_Add(_objModel);

        //        var dataToSign = "(request-target): head " + gatewayurl +
        //                         "caseIdentifiers" + "\n" +
        //                         "host: " + gatewayhost + "\n" +
        //                         "date: " + date;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);

        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date\",signature=\"" + hmac + "\"";

        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "HEAD";
        //        WebReq.Headers.Add("authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.Date = dateValue;

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        _objModel.Response = WebResp.StatusCode.ToString();
        //        _objModel.IsSuccess = true;
        //        clsTRLog.APILogDetail_Update(_objModel);

        //        _objModel.caseId = caseId;
        //        clsTRLog.ScreeningMaster_Update(_objModel);

        //        clsTRLog.Tras.Commit();
        //        clsTRLog.Conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsTRLog.Conn = ClsAppDatabase.GetCon();
        //        if (clsTRLog.Conn.State == ConnectionState.Open)
        //        {
        //            _objModel.Response = "";
        //            _objModel.IsSuccess = true;
        //            clsTRLog.APILogDetail_Update(_objModel);

        //            _objModel.PlatformID = PlatformID;

        //            //clsTRLog.ScreeningMaster_Add(_objModel);
        //            clsTRLog.ScreeningMaster_Update(_objModel);

        //            clsTRLog.Tras.Commit();
        //            clsTRLog.Conn.Close();
        //        }
        //    }
        //}
        //public string caseSystemId(string caseId, string ContactFullName, int CustomerID, int CustomerShareHoldID, string NatCountryCode, string CustType,
        //                            string Gender = "", string LocationCountryCode = "", Boolean IsUnspecified = false)
        //{
        //    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    clsTRLog.Conn = ClsAppDatabase.GetCon();
        //    if (clsTRLog.Conn.State == ConnectionState.Closed)
        //    { clsTRLog.Conn.Open(); }
        //    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();
        //    string RtncaseSystemId = "";
        //    try
        //    {
        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "cases";

        //        _objModel.CustomerID = CustomerID;
        //        _objModel.CustomerShareHoldID = CustomerShareHoldID;
        //        _objModel.LogType = CONT.caseSystemIDReturnRequest;
        //        _objModel.caseId = caseId;
        //        _objModel.Request = requestendpoint;
        //        _objModel.LogID = clsTRLog.APILogDetail_Add(_objModel);
        //        string postData = "";
        //        string SecondaryFields = "";


        //        string strNatCountryCode = "\"" + NatCountryCode + "\"";
        //        string _pSFCT_6 = @"{""typeId"":""SFCT_6"",""value"":" + strNatCountryCode + "}";

        //        string strGender = "\"" + Gender + "\"";
        //        string _pSFCT_1 = @"{""typeId"":""SFCT_1"",""value"":" + strGender + "}";

        //        string strLocationCountryCode = "\"" + LocationCountryCode + "\"";
        //        string _pSFCT_3 = @"{""typeId"":""SFCT_3"",""value"":" + strLocationCountryCode + "}";

        //        string strNatCountryCode_Ind = "\"" + NatCountryCode + "\"";
        //        string _pSFCT_5 = @"{""typeId"":""SFCT_5"",""value"":" + strNatCountryCode_Ind + "}";

        //        if (IsUnspecified == true)
        //        {
        //            postData = "{\"entityType\":\"UNSPECIFIED\",\"caseId\":\"" + caseId + "\",\"groupId\":\"" + group_id + "\",\"providerTypes\":[\"WATCHLIST\"],\"name\":\"" + ContactFullName + "\"}";
        //        }
        //        else if (CustType == "C" || CustType == "M" || CustType == "N" || CustType == "T" || CustType == "K")
        //        {
        //            SecondaryFields = _pSFCT_6;
        //            postData = "{\"secondaryFields\":[" + SecondaryFields + "],\"entityType\":\"ORGANISATION\",\"caseId\":\"" + caseId + "\",\"groupId\":\"" + group_id + "\",\"providerTypes\":[\"WATCHLIST\"],\"name\":\"" + ContactFullName + "\"}";
        //        }
        //        else
        //        {
        //            if (Gender != "" && LocationCountryCode != "")
        //                SecondaryFields = _pSFCT_1 + "," + _pSFCT_3 + "," + _pSFCT_5;
        //            else if (Gender != "" && LocationCountryCode == "")
        //                SecondaryFields = _pSFCT_1 + "," + _pSFCT_5;
        //            else if (Gender == "" && LocationCountryCode != "")
        //                SecondaryFields = _pSFCT_3 + "," + _pSFCT_5;
        //            else
        //                SecondaryFields = _pSFCT_5;

        //            postData = "{\"secondaryFields\":[" + SecondaryFields + "],\"entityType\":\"INDIVIDUAL\",\"caseId\":\"" + caseId + "\",\"groupId\":\"" + group_id + "\",\"providerTypes\":[\"WATCHLIST\"],\"name\":\"" + ContactFullName + "\"}";
        //            //postData = "{\"secondaryFields\":[" + SecondaryFields + "],\"entityType\":\"INDIVIDUAL\",\"groupId\":\"" + group_id + "\",\"providerTypes\":[\"WATCHLIST\"],\"name\":\"" + ContactFullName + "\"}";
        //        }

        //        fn.LogFileWrite("Post Data Log Start : " + postData);
        //        string msg = postData;
        //        UTF8Encoding encoding = new UTF8Encoding();
        //        byte[] byte1 = encoding.GetBytes(postData);
        //        string dataToSign = "(request-target): post " + gatewayurl + "cases\n" +
        //                            "host: " + gatewayhost + "\n" +
        //                            "date: " + date + "\n" +
        //                            "content-type: " + "application/json" + "\n" +
        //                            "content-length: " + byte1.Length + "\n" +
        //                            msg;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);
        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date content-type content-length\",signature=\"" + hmac + "\"";

        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "POST";
        //        WebReq.Headers.Add("Authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.ContentLength = msg.Length;
        //        WebReq.Date = dateValue;
        //        WebReq.ContentType = "application/json";
        //        WebReq.ContentLength = byte1.Length;

        //        Stream newStream = WebReq.GetRequestStream();
        //        newStream.Write(byte1, 0, byte1.Length);

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        string StrStatusCode = WebResp.StatusCode.ToString();
        //        Stream Answer = WebResp.GetResponseStream();
        //        StreamReader _Answer = new StreamReader(Answer);
        //        string jsontxt = _Answer.ReadToEnd();

        //        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsontxt);
        //        var f = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);

        //        JObject o = JObject.Parse(f);

        //        _objModel.caseId = caseId;
        //        _objModel.caseSystemId = (string)o["caseSystemId"];
        //        clsTRLog.ScreeningMaster_Update(_objModel);

        //        _objModel.Response = StrStatusCode;
        //        _objModel.IsSuccess = true;
        //        clsTRLog.APILogDetail_Update(_objModel);

        //        clsTRLog.Tras.Commit();
        //        clsTRLog.Conn.Close();

        //        return RtncaseSystemId = (string)o["caseSystemId"];
        //    }
        //    catch (Exception ex)
        //    {
        //        //string ss = ex.InnerException.ToString();
        //        //clsTRLog.Conn = ClsAppDatabase.GetCon();
        //        //if (clsTRLog.Conn.State == ConnectionState.Open)
        //        //{
        //        //    clsTRLog.Tras.Commit();
        //        //    clsTRLog.Conn.Close();
        //        //}
        //        clsTRLog.Conn = ClsAppDatabase.GetCon();
        //        if (clsTRLog.Conn.State == ConnectionState.Open)
        //        {
        //            clsTRLog.Tras.Rollback();
        //            clsTRLog.Conn.Close();
        //        }

        //        string errorMessage = fn.CreateErrorMessage(ex);
        //        fn.LogFileWrite(errorMessage);
        //        if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //    }
        //    return RtncaseSystemId = "";
        //}
        //public void screeningRequest(string caseId, string caseSystemId, int CustomerID, int CustomerShareHoldID)
        //{
        //    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    clsTRLog.Conn = ClsAppDatabase.GetCon();
        //    if (clsTRLog.Conn.State == ConnectionState.Closed)
        //    { clsTRLog.Conn.Open(); }
        //    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();
        //    try
        //    {

        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "cases/" + caseSystemId + "/screeningRequest";

        //        _objModel.CustomerID = CustomerID;
        //        _objModel.CustomerShareHoldID = CustomerShareHoldID;
        //        _objModel.LogType = CONT.ScreeningRequestJson;
        //        _objModel.caseId = caseId;
        //        _objModel.caseSystemId = caseSystemId;
        //        _objModel.Request = requestendpoint;
        //        _objModel.LogID = clsTRLog.APILogDetail_Add(_objModel);

        //        string dataToSign = "(request-target): post " + gatewayurl + "cases/" + caseSystemId + "/screeningRequest\n" +
        //                            "host: " + gatewayhost + "\n" +
        //                            "date: " + date + "\n" +
        //                            "content-type: " + "application/json" + "\n" +
        //                            "content-length: " + 0;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);
        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date content-type content-length\",signature=\"" + hmac + "\"";


        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "POST";
        //        WebReq.Headers.Add("Authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.ContentLength = 0;
        //        WebReq.Date = dateValue;
        //        WebReq.ContentType = "application/json";
        //        WebReq.ContentLength = 0;

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        _objModel.Response = WebResp.StatusCode.ToString();
        //        _objModel.IsSuccess = true;
        //        clsTRLog.APILogDetail_Update(_objModel);

        //        clsTRLog.Tras.Commit();
        //        clsTRLog.Conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsTRLog.Conn = ClsAppDatabase.GetCon();
        //        if (clsTRLog.Conn.State == ConnectionState.Open)
        //        {
        //            clsTRLog.Tras.Rollback();
        //            clsTRLog.Conn.Close();
        //        }
        //        string errorMessage = fn.CreateErrorMessage(ex);
        //        fn.LogFileWrite(errorMessage);
        //        if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //        throw ex;
        //    }
        //}
        //public void GetScreeningResults(string caseId, string caseSystemId, int CustomerID, int CustomerShareHoldID, Boolean IsUnspecified = false)
        //{
        //    System.Threading.Thread.Sleep(500);
        //    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    clsTRLog.Conn = ClsAppDatabase.GetCon();
        //    if (clsTRLog.Conn.State == ConnectionState.Closed)
        //    { clsTRLog.Conn.Open(); }
        //    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();
        //    try
        //    {
        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "cases/" + caseSystemId + "/results";

        //        _objModel.CustomerID = CustomerID;
        //        _objModel.CustomerShareHoldID = CustomerShareHoldID;
        //        _objModel.LogType = CONT.ScreeningResults;
        //        _objModel.caseId = caseId;
        //        _objModel.caseSystemId = caseSystemId;
        //        _objModel.Request = requestendpoint;
        //        _objModel.LogID = clsTRLog.APILogDetail_Add(_objModel);

        //        string dataToSign = "(request-target): get " + gatewayurl + "cases/" +
        //                            caseSystemId + "/results" + "\n" +
        //                            "host: " + gatewayhost + "\n" +
        //                            "date: " + date;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);
        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date\",signature=\"" + hmac + "\"";

        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "GET";
        //        WebReq.Headers.Add("Authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.Date = dateValue;

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        _objModel.Response = WebResp.StatusCode.ToString();
        //        _objModel.IsSuccess = true;
        //        clsTRLog.APILogDetail_Update(_objModel);

        //        clsTRLog.Tras.Commit();
        //        clsTRLog.Conn.Close();

        //        Stream Answer = WebResp.GetResponseStream();
        //        StreamReader _Answer = new StreamReader(Answer);
        //        string jsontxt = _Answer.ReadToEnd();

        //        var result = JsonConvert.DeserializeObject<List<Models.ThomsonReutersModel.RootObject>>(jsontxt);
        //        //ViewBag.Result = JsonConvert.DeserializeObject<List<Models.ThomsonReutersModel.RootObject>>(jsontxt);
        //        ViewBag.IsDeleteResult = false;

        //        fn.LogFileWrite("jsontxt : " + jsontxt);
        //        fn.LogFileWrite("Case System Id is : " + caseSystemId);

        //        fn.LogFileWrite("Total Record Count Log Start..." + "Total Record Count " + result.Count);
        //        if (result != null && result.Count > 0)
        //        {
        //            fn.LogFileWrite("Total Record Count " + result.Count + ", for caseSystemId = " + caseSystemId);
        //            for (int i = 0; i < result.Count; i++)
        //            {
        //                if (result[i].matchStrength != "STRONG" && result[i].matchStrength != "EXACT")
        //                    //if (result[i].matchStrength != "EXACT")
        //                    SystemResolution(caseId, caseSystemId, CustomerID, CustomerShareHoldID, result[i].resultId);

        //            }
        //        }
        //        else if (result != null && result.Count == 0)
        //        {
        //            ViewBag.IsDeleteResult = true;

        //            if (IsUnspecified == false)
        //            {
        //                clsTRLog.Conn = ClsAppDatabase.GetCon();
        //                if (clsTRLog.Conn.State == ConnectionState.Closed)
        //                { clsTRLog.Conn.Open(); }
        //                else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //                clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

        //                _objModel.caseSystemId = caseSystemId;
        //                clsTRLog.ScreeningMaster_UpdateDeleted(_objModel);
        //                clsTRLog.Tras.Commit(); clsTRLog.Conn.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        clsTRLog.Conn = ClsAppDatabase.GetCon();
        //        if (clsTRLog.Conn.State == ConnectionState.Open)
        //        {
        //            clsTRLog.Tras.Rollback();
        //            clsTRLog.Conn.Close();
        //        }
        //        string errorMessage = fn.CreateErrorMessage(ex);
        //        fn.LogFileWrite(errorMessage);
        //        if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //        throw ex;
        //    }
        //}
        //public void SystemResolution(string caseId, string caseSystemId, int CustomerID, int CustomerShareHoldID, string presultIds)
        //{
        //    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    clsTRLog.Conn = ClsAppDatabase.GetCon();
        //    if (clsTRLog.Conn.State == ConnectionState.Closed)
        //    { clsTRLog.Conn.Open(); }
        //    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();
        //    try
        //    {

        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "cases/" + caseSystemId + "/results/resolution";

        //        _objModel.CustomerID = CustomerID;
        //        _objModel.CustomerShareHoldID = CustomerShareHoldID;
        //        _objModel.LogType = CONT.SystemResolution;
        //        _objModel.caseId = caseId;
        //        _objModel.caseSystemId = caseSystemId;
        //        _objModel.Request = requestendpoint;
        //        _objModel.LogID = clsTRLog.APILogDetail_Add(_objModel);

        //        string resultIds = "\"" + presultIds + "\"";
        //        string statusId = "\"" + System.Configuration.ConfigurationManager.AppSettings["statusId"] + "\"";
        //        string riskId = "\"" + System.Configuration.ConfigurationManager.AppSettings["riskId"] + "\"";
        //        string reasonId = "\"" + System.Configuration.ConfigurationManager.AppSettings["reasonId"] + "\"";
        //        string resolutionRemark = "\"" + System.Configuration.ConfigurationManager.AppSettings["resolutionRemark"] + "\"";

        //        string postData = "{\"resultIds\":[" + resultIds + "],\"statusId\":" + statusId + ",\"riskId\":" + riskId + ",\"reasonId\":" + reasonId + ",\"resolutionRemark\":" + resolutionRemark + "}";

        //        string msg = postData;
        //        UTF8Encoding encoding = new UTF8Encoding();
        //        byte[] byte1 = encoding.GetBytes(postData);
        //        string dataToSign = "(request-target): put " + gatewayurl + "cases/" + caseSystemId + "/results/resolution\n" +
        //                            "host: " + gatewayhost + "\n" +
        //                            "date: " + date + "\n" +
        //                            "content-type: " + "application/json" + "\n" +
        //                            "content-length: " + byte1.Length + "\n" +
        //                            msg;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);
        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date content-type content-length\",signature=\"" + hmac + "\"";

        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "PUT";
        //        WebReq.Headers.Add("Authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.ContentLength = msg.Length;
        //        WebReq.Date = dateValue;
        //        WebReq.ContentType = "application/json";
        //        WebReq.ContentLength = byte1.Length;

        //        Stream newStream = WebReq.GetRequestStream();
        //        newStream.Write(byte1, 0, byte1.Length);

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        _objModel.Response = WebResp.StatusCode.ToString();
        //        _objModel.IsSuccess = true;
        //        clsTRLog.APILogDetail_Update(_objModel);

        //        clsTRLog.Tras.Commit();
        //        clsTRLog.Conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsTRLog.Conn = ClsAppDatabase.GetCon();
        //        if (clsTRLog.Conn.State == ConnectionState.Open)
        //        {
        //            clsTRLog.Tras.Rollback();
        //            clsTRLog.Conn.Close();
        //        }
        //        string errorMessage = fn.CreateErrorMessage(ex);
        //        fn.LogFileWrite(errorMessage);
        //        if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //        throw ex;
        //    }
        //}

        //public string GetScreeningResultsByUser(string caseSystemId = "")
        //{
        //    string strReturn = "";
        //    string jsontxt = "";
        //    try
        //    {
        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "cases/" + caseSystemId + "/results";

        //        string dataToSign = "(request-target): get " + gatewayurl + "cases/" +
        //                            caseSystemId + "/results" + "\n" +
        //                            "host: " + gatewayhost + "\n" +
        //                            "date: " + date;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);
        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date\",signature=\"" + hmac + "\"";

        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "GET";
        //        WebReq.Headers.Add("Authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.Date = dateValue;

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        Stream Answer = WebResp.GetResponseStream();
        //        StreamReader _Answer = new StreamReader(Answer);
        //        jsontxt = _Answer.ReadToEnd();

        //        //var result = JsonConvert.DeserializeObject<List<Models.ThomsonReutersModel.RootObject>>(jsontxt);
        //        ViewBag.Result = JsonConvert.DeserializeObject<List<Models.ThomsonReutersModel.RootObject>>(jsontxt).OrderBy(x => x.resultId).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        string errorMessage = fn.CreateErrorMessage(ex);
        //        fn.LogFileWrite(errorMessage);
        //        fn.LogFileWrite("caseSystemId :" + caseSystemId);
        //        if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //        throw ex;
        //    }
        //    return strReturn = jsontxt;
        //}
        //public void SystemResolutionByUser(string caseSystemId, int CustomerID, string presultIds, string pstatusId, string priskId, string preasonId, string presolutionRemark)
        //{
        //    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    clsTRLog.Conn = ClsAppDatabase.GetCon();
        //    if (clsTRLog.Conn.State == ConnectionState.Closed)
        //    { clsTRLog.Conn.Open(); }
        //    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();
        //    try
        //    {
        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "cases/" + caseSystemId + "/results/resolution";

        //        _objModel.CustomerID = CustomerID;
        //        _objModel.LogType = CONT.SystemResolutionByUser;
        //        _objModel.caseSystemId = caseSystemId;
        //        _objModel.Request = requestendpoint;

        //        _objModel.resultId = presultIds;
        //        _objModel.statusId = pstatusId;
        //        _objModel.riskId = priskId;
        //        _objModel.reasonId = preasonId;
        //        _objModel.resolutionRemark = presolutionRemark;
        //        _objModel.presolutionDate = DateTime.Now;
        //        _objModel.LogID = clsTRLog.APILogDetail_Add(_objModel);

        //        string resultIds = "\"" + presultIds + "\"";
        //        string statusId = "\"" + pstatusId + "\"";
        //        string riskId = "\"" + priskId + "\"";
        //        string reasonId = "\"" + preasonId + "\"";
        //        string resolutionRemark = "\"" + presolutionRemark + "\"";

        //        string postData = "{\"resultIds\":[" + resultIds + "],\"statusId\":" + statusId + ",\"riskId\":" + riskId + ",\"reasonId\":" + reasonId + ",\"resolutionRemark\":" + resolutionRemark + "}";

        //        string msg = postData;
        //        UTF8Encoding encoding = new UTF8Encoding();
        //        byte[] byte1 = encoding.GetBytes(postData);
        //        string dataToSign = "(request-target): put " + gatewayurl + "cases/" + caseSystemId + "/results/resolution\n" +
        //                            "host: " + gatewayhost + "\n" +
        //                            "date: " + date + "\n" +
        //                            "content-type: " + "application/json" + "\n" +
        //                            "content-length: " + byte1.Length + "\n" +
        //                            msg;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);
        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date content-type content-length\",signature=\"" + hmac + "\"";

        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "PUT";
        //        WebReq.Headers.Add("Authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.ContentLength = msg.Length;
        //        WebReq.Date = dateValue;
        //        WebReq.ContentType = "application/json";
        //        WebReq.ContentLength = byte1.Length;

        //        Stream newStream = WebReq.GetRequestStream();
        //        newStream.Write(byte1, 0, byte1.Length);

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        _objModel.Response = WebResp.StatusCode.ToString();
        //        _objModel.IsSuccess = true;
        //        clsTRLog.APILogDetail_Update(_objModel);

        //        clsTRLog.ScreeningDetail_Add(_objModel);

        //        clsTRLog.Tras.Commit();
        //        clsTRLog.Conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsTRLog.Tras.Rollback();
        //        clsTRLog.Conn.Close();

        //        string errorMessage = fn.CreateErrorMessage(ex);
        //        fn.LogFileWrite(errorMessage);
        //        if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //        throw ex;
        //    }
        //}
        //public void EnableOngoingScreeningByUser(string caseSystemId, int CustomerID, int CustomerShareHoldID)
        //{
        //    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    clsTRLog.Conn = ClsAppDatabase.GetCon();
        //    if (clsTRLog.Conn.State == ConnectionState.Closed)
        //    { clsTRLog.Conn.Open(); }
        //    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();
        //    try
        //    {
        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "cases/" + caseSystemId + "/ongoingScreening";

        //        _objModel.CustomerID = CustomerID;
        //        _objModel.CustomerShareHoldID = CustomerShareHoldID;
        //        _objModel.LogType = CONT.OngoingScreeningByUser;
        //        _objModel.caseSystemId = caseSystemId;
        //        _objModel.Request = requestendpoint;

        //        _objModel.LogID = clsTRLog.APILogDetail_Add(_objModel);

        //        string dataToSign = "(request-target): put " + gatewayurl + "cases/" + caseSystemId + "/ongoingScreening\n" +
        //                            "host: " + gatewayhost + "\n" +
        //                            "date: " + date + "\n" +
        //                            "content-type: " + "application/json" + "\n" +
        //                            "content-length: " + 0;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);
        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date content-type content-length\",signature=\"" + hmac + "\"";

        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "PUT";
        //        WebReq.Headers.Add("Authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.ContentLength = 0;
        //        WebReq.Date = dateValue;
        //        WebReq.ContentType = "application/json";
        //        WebReq.ContentLength = 0;

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        _objModel.Response = WebResp.StatusCode.ToString();
        //        _objModel.IsSuccess = true;
        //        clsTRLog.APILogDetail_Update(_objModel);

        //        _objModel.IsOGS = true;
        //        clsTRLog.ScreeningMaster_UpdateOGS(_objModel);

        //        clsTRLog.Tras.Commit();
        //        clsTRLog.Conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsTRLog.Tras.Rollback();
        //        clsTRLog.Conn.Close();

        //        string errorMessage = fn.CreateErrorMessage(ex);
        //        fn.LogFileWrite(errorMessage);
        //        if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //        throw ex;
        //    }
        //}
        //public void DisableOngoingScreeningByUser(string caseSystemId, int CustomerID)
        //{
        //    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    clsTRLog.Conn = ClsAppDatabase.GetCon();
        //    if (clsTRLog.Conn.State == ConnectionState.Closed)
        //    { clsTRLog.Conn.Open(); }
        //    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
        //    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();
        //    try
        //    {
        //        DateTime dateValue = DateTime.UtcNow;
        //        string date = dateValue.ToString("R");
        //        requestendpoint = "";
        //        requestendpoint = Getrequestendpoint + "cases/" + caseSystemId + "/ongoingScreening";

        //        _objModel.CustomerID = CustomerID;
        //        _objModel.LogType = CONT.OngoingScreeningByUser;
        //        _objModel.caseSystemId = caseSystemId;
        //        _objModel.Request = requestendpoint;

        //        _objModel.LogID = clsTRLog.APILogDetail_Add(_objModel);

        //        string dataToSign = "(request-target): delete " + gatewayurl + "cases/" + caseSystemId + "/ongoingScreening\n" +
        //                            "host: " + gatewayhost + "\n" +
        //                            "date: " + date + "\n" +
        //                            "content-type: " + "application/json" + "\n" +
        //                            "content-length: " + 0;

        //        string hmac = generateAuthHeader(dataToSign, apisecret);
        //        string authorisation = "Signature keyId=\"" + apikey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date content-type content-length\",signature=\"" + hmac + "\"";

        //        HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(requestendpoint);
        //        WebReq.Method = "DELETE";
        //        WebReq.Headers.Add("Authorization", authorisation);
        //        WebReq.Headers.Add("Cache-Control", "no-cache");
        //        WebReq.ContentLength = 0;
        //        WebReq.Date = dateValue;
        //        WebReq.ContentType = "application/json";
        //        WebReq.ContentLength = 0;

        //        HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
        //        _objModel.Response = WebResp.StatusCode.ToString();
        //        _objModel.IsSuccess = true;
        //        clsTRLog.APILogDetail_Update(_objModel);

        //        _objModel.IsOGS = false;
        //        clsTRLog.ScreeningMaster_UpdateOGS(_objModel);

        //        clsTRLog.Tras.Commit();
        //        clsTRLog.Conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsTRLog.Tras.Rollback();
        //        clsTRLog.Conn.Close();

        //        string errorMessage = fn.CreateErrorMessage(ex);
        //        fn.LogFileWrite(errorMessage);
        //        if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //        throw ex;
        //    }
        //}
        //public static string generateAuthHeader(string dataToSign, string apisecret)
        //{
        //    byte[] secretKey = Encoding.UTF8.GetBytes(apisecret);
        //    HMACSHA256 hmac = new HMACSHA256(secretKey);
        //    hmac.Initialize();

        //    byte[] bytes = Encoding.UTF8.GetBytes(dataToSign);
        //    byte[] rawHmac = hmac.ComputeHash(bytes);

        //    return (Convert.ToBase64String(rawHmac));
        //}

        //public void ApproveButtonValidation(int CustomerID)
        //{
        //    CustomerShareHolderModel _objValidation = new CustomerShareHolderModel();
        //    CustomerDocModel _obCustomerDocModel = new CustomerDocModel();
        //    _objValidation.PlatformID = PlatformID;
        //    _objValidation.CustomerID = CustomerID;
        //    var GetComData = _ClsThomsonReuters.ScreeningCustomer_ListAll(_objValidation).ToList();
        //    bool isStopLoop = false;
        //    ViewBag.btnApproveVisible = true;
        //    ViewBag.btnClassificationVisible = true;

        //    if (GetComData != null && GetComData.Count > 0)
        //    {
        //        foreach (var item in GetComData)
        //        {
        //            if (item.caseSystemId != "" && isStopLoop == false)
        //            {
        //                System.Threading.Thread.Sleep(500);
        //                string strData = GetScreeningResultsByUser(item.caseSystemId);
        //                if (strData != "")
        //                {
        //                    var result = JsonConvert.DeserializeObject<List<Models.ThomsonReutersModel.RootObject>>(strData).Where(x => x.matchStrength == "STRONG" || x.matchStrength == "EXACT").ToList();
        //                    if (result != null && result.Count > 0)
        //                    {
        //                        for (int i = 0; i < result.Count; i++)
        //                        {
        //                            if (result[i].resolution != null && result[i].resolution.reasonId == null)
        //                            {
        //                                ViewBag.btnApproveVisible = false;
        //                                isStopLoop = true;
        //                                break;
        //                            }
        //                            else if (result[i].resolution == null)
        //                            {
        //                                ViewBag.btnApproveVisible = false;
        //                                isStopLoop = true;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //if (isStopLoop == false)
        //    //{
        //    _obCustomerDocModel.CustomerID = CustomerID;
        //    var GetData = _clsCust.CustomerClassDetail_ListAll(_obCustomerDocModel).FirstOrDefault();
        //    if (GetData == null)
        //    {
        //        ViewBag.btnClassificationVisible = false;

        //    }


        //    //if (GetData != null)
        //    //{
        //    ScreeningConfig _objModel = new ScreeningConfig();
        //    _objModel.CustomerID = CustomerID;
        //    var CustomerRiskData = _clsCust.CustomerRiskDetail_ListAll(_objModel).ToList();
        //    if (CustomerRiskData == null)
        //        ViewBag.btnApproveVisible = false;
        //    //}
        //    //}
        //}
        //public void FillStatusCombo()
        //{
        //    ViewBag.DDLStatusID = new List<SelectListItem> {
        //    new SelectListItem() { Text = "Positive", Value = "0a3687cf-5fe4-105f-9813-f982000009f4" },
        //    new SelectListItem() { Text = "Possible", Value = "0a3687cf-5fe4-105f-9813-f982000009f9" },
        //    new SelectListItem() { Text = "False", Value = "0a3687cf-5fe4-105f-9813-f982000009ff" },
        //    new SelectListItem() { Text = "Unspecified", Value = "0a3687cf-5fe4-105f-9813-f98200000a02" }};
        //}
        //public void FillRiskLevelCombo()
        //{
        //    ViewBag.DDLRiskLevelID = new List<SelectListItem> {
        //    new SelectListItem() { Text = "Unknown", Value = "0a3687cf-5fe4-105f-9813-f982000009f3" },
        //    new SelectListItem() { Text = "High", Value = "0a3687cf-5fe4-105f-9813-f982000009f0" },
        //    new SelectListItem() { Text = "Medium", Value = "0a3687cf-5fe4-105f-9813-f982000009f1" },
        //    new SelectListItem() { Text = "Low", Value = "0a3687cf-5fe4-105f-9813-f982000009f2" }};
        //}
        //public void FillReasonCombo()
        //{
        //    ViewBag.DDLReasonID = new List<SelectListItem> {
        //    new SelectListItem() { Text = "No Match", Value = "0a3687cf-5fe4-105f-9813-f982000009ee" },
        //    new SelectListItem() { Text = "Unknown", Value = "0a3687cf-5fe4-105f-9813-f982000009ef" },
        //    new SelectListItem() { Text = "Full Match", Value = "0a3687cf-5fe4-105f-9813-f982000009ec" },
        //    new SelectListItem() { Text = "Partial Match", Value = "0a3687cf-5fe4-105f-9813-f982000009ed" } };
        //}

        //#region Thomson Common

        //public void FillAuthoriseUserCombo(int CustomerID)
        //{
        //    CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
        //    _objModel.PlatformID = PlatformID; _objModel.CustomerID = CustomerID;
        //    var DDLAuthoUserData = _ClsThomsonReuters.ScreeningCustomer_ListAll(_objModel).Where(x => x.caseSystemId != "").ToList();
        //    if (DDLAuthoUserData != null && DDLAuthoUserData.Count > 0)
        //        ViewBag.DDLAuthoUserID = DDLAuthoUserData;
        //    else
        //        ViewBag.DDLAuthoUserID = new List<SelectListItem> { };
        //}
        //public void FillGenderCombo()
        //{
        //    ViewBag.DDLGenderID = new List<SelectListItem> { new SelectListItem() { Text = "Male", Value = "Male" },
        //                                             new SelectListItem() { Text = "Female", Value = "Female" } };
        //}

        //public PartialViewResult GridOrganisationDetails()
        //{
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    ClsThomsonReuters _ClsThomsonReuters = new ClsThomsonReuters();
        //    _objModel.intIsCompany = 1;
        //    var Data = _ClsThomsonReuters.ScreeningMaster_ListAll(_objModel).OrderByDescending(x => x.SCID);
        //    return PartialView(Data);
        //}
        //public PartialViewResult GridIndividualDetails()
        //{
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    ClsThomsonReuters _ClsThomsonReuters = new ClsThomsonReuters();
        //    _objModel.intIsCompany = 0;
        //    var Data = _ClsThomsonReuters.ScreeningMaster_ListAll(_objModel).OrderByDescending(x => x.SCID);
        //    return PartialView(Data);
        //}
        //public void FillAllUserCombo()
        //{
        //    ThomsonReutersModel _objModel = new ThomsonReutersModel();
        //    _objModel.PlatformID = PlatformID;
        //    _objModel.intIsCompany = -1;
        //    var DDLAuthoUserData = _ClsThomsonReuters.ScreeningMaster_ListAll(_objModel).Where(x => x.caseSystemId != "").ToList();
        //    if (DDLAuthoUserData != null && DDLAuthoUserData.Count > 0)
        //        ViewBag.DDLAuthoUserID = DDLAuthoUserData;
        //    else
        //        ViewBag.DDLAuthoUserID = new List<SelectListItem> { };
        //}
        //#endregion

        //#endregion
        public void FillAuthoriseUserCombo(int CustomerID)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.PlatformID = PlatformID; _objModel.CustomerID = CustomerID;
            var DDLAuthoUserData = _ClsThomsonReuters.ScreeningCustomer_ListAll(_objModel).Where(x => x.caseSystemId != "").ToList();
            if (DDLAuthoUserData != null && DDLAuthoUserData.Count > 0)
                ViewBag.DDLAuthoUserID = DDLAuthoUserData;
            else
                ViewBag.DDLAuthoUserID = new List<SelectListItem> { };
        }
        public void FillGenderCombo()
        {
            ViewBag.DDLGenderID = new List<SelectListItem> { new SelectListItem() { Text = "Male", Value = "Male" },
                                                     new SelectListItem() { Text = "Female", Value = "Female" } };
        }

        public ActionResult ScreenDataEntry(int TabIndex = 1)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    FillCountryCombo(); FillGenderCombo();
                    ViewBag.SelectedTab = TabIndex;
                    return View(_ThomsonReutersModel);
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ScreenDataEntry(ThomsonReutersModel _objModel, FormCollection frm)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsThomsonReuters ClsThomsonReuters = new ClsThomsonReuters();
                try
                {
                    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
                    clsTRLog.Conn = ClsAppDatabase.GetCon();
                    if (clsTRLog.Conn.State == ConnectionState.Closed)
                    { clsTRLog.Conn.Open(); }
                    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
                    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

                    #region Thomson Reuters Integration
                    //if (Request["btnTR_Org_Add"] != null)
                    //{
                    //    _objModel.IsCompany = true;
                    //    _objModel.CountryID = _objModel.OrgCountryID;
                    //    string CaseID = clsTRLog.ScreeningMaster_Add(_objModel);
                    //    clsTRLog.Tras.Commit();
                    //    clsTRLog.Conn.Close();

                    //    _objModel.caseId = CaseID; _objModel.intIsCompany = -1;
                    //    var GetValue = clsTRLog.ScreeningMaster_ListAll(_objModel).FirstOrDefault();
                    //    if (GetValue != null)
                    //    {
                    //        #region Step 1
                    //        caseIdentifiers(CaseID, 0, 0);
                    //        #endregion

                    //        #region Step 2
                    //        string strcaseSystemId = caseSystemId(CaseID, GetValue.CompanyName, 0, 0, GetValue.CountryCode, "M");
                    //        #endregion

                    //        #region Step 3
                    //        if (strcaseSystemId != null && strcaseSystemId != "")
                    //            screeningRequest(CaseID, strcaseSystemId, 0, 0);
                    //        #endregion

                    //        #region Step 4
                    //        if (strcaseSystemId != null && strcaseSystemId != "")
                    //        {
                    //            GetScreeningResults(CaseID, strcaseSystemId, 0, 0);

                    //            //EnableOngoingScreeningByUser(strcaseSystemId, 0, 0);

                    //            _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                    //            if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                    //            { _ClsThomsonReuters.Conn.Open(); }
                    //            else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                    //            _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                    //            _ThomsonReutersModel.caseSystemId = strcaseSystemId;
                    //            _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                    //            _ClsThomsonReuters.Tras.Commit();
                    //            _ClsThomsonReuters.Conn.Close();
                    //        }
                    //        #endregion
                    //    }
                    //    return RedirectToAction("ScreenDataEntry", "CustomerReg", new { TabIndex = 1 });
                    //}

                    //else if (Request["btnTR_Ind_Add"] != null)
                    //{
                    //    _objModel.IsCompany = false;
                    //    _objModel.CountryID = _objModel.IndCountryID;
                    //    string CaseID = clsTRLog.ScreeningMaster_Add(_objModel);
                    //    clsTRLog.Tras.Commit();
                    //    clsTRLog.Conn.Close();

                    //    _objModel.caseId = CaseID; _objModel.intIsCompany = -1;
                    //    var GetValue = clsTRLog.ScreeningMaster_ListAll(_objModel).FirstOrDefault();
                    //    if (GetValue != null)
                    //    {
                    //        #region Step 1
                    //        caseIdentifiers(CaseID, 0, 0);
                    //        #endregion

                    //        #region Step 2
                    //        string strcaseSystemId = caseSystemId(CaseID, GetValue.ContactFullName, 0, 0, GetValue.CountryCode, "", GetValue.Gender, GetValue.LocCountryCode);
                    //        #endregion

                    //        #region Step 3
                    //        if (strcaseSystemId != null && strcaseSystemId != "")
                    //            screeningRequest(CaseID, strcaseSystemId, 0, 0);
                    //        #endregion

                    //        #region Step 4
                    //        if (strcaseSystemId != null && strcaseSystemId != "")
                    //        {
                    //            GetScreeningResults(CaseID, strcaseSystemId, 0, 0);

                    //            //EnableOngoingScreeningByUser(strcaseSystemId, 0, 0);

                    //            _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                    //            if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                    //            { _ClsThomsonReuters.Conn.Open(); }
                    //            else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                    //            _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                    //            _ThomsonReutersModel.caseSystemId = strcaseSystemId;
                    //            _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                    //            _ClsThomsonReuters.Tras.Commit();
                    //            _ClsThomsonReuters.Conn.Close();
                    //        }
                    //        #endregion
                    //    }
                    //    return RedirectToAction("ScreenDataEntry", "CustomerReg", new { TabIndex = 2 });
                    //}
                    #endregion
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    FillCountryCombo(); FillGenderCombo();
                }
                FillCountryCombo(); FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult ScreeningResolution(bool IsAuthoData = false, string AuthcaseSystemId = "", int TabIndex = 1)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ThomsonReutersModel _objModel = new ThomsonReutersModel();
                try
                {
                    #region Thomson Reuters Integration
                    //FillAllUserCombo();
                    //FillStatusCombo(); FillRiskLevelCombo(); FillReasonCombo();
                    //if (IsAuthoData == true && AuthcaseSystemId != "")
                    //{
                    //    GetScreeningResultsByUser(AuthcaseSystemId);
                    //    _objModel.caseSystemId = AuthcaseSystemId;

                    //    _ThomsonReutersModel.caseSystemId = AuthcaseSystemId;
                    //    var ScreeningData = _ClsThomsonReuters.ScreeningDetail_ListAllcaseSystemId(_ThomsonReutersModel).OrderBy(x => x.resultId).ToList();
                    //    if (ScreeningData != null && ScreeningData.Count > 0)
                    //    {
                    //        ViewBag.ScreeningData = ScreeningData;
                    //    }
                    //    return View(_objModel);
                    //}
                    #endregion
                    return View(_objModel);
                }

                catch (Exception ex)
                {
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                //FillAllUserCombo();
                //FillStatusCombo(); FillRiskLevelCombo(); FillReasonCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult ScreeningResolution(ThomsonReutersModel _objModel, FormCollection frm)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsThomsonReuters ClsThomsonReuters = new ClsThomsonReuters();
                try
                {
                    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
                    clsTRLog.Conn = ClsAppDatabase.GetCon();
                    if (clsTRLog.Conn.State == ConnectionState.Closed)
                    { clsTRLog.Conn.Open(); }
                    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
                    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

                    #region Thomson Reuters Integration
                    //if (Request["btnTR_Display"] != null)
                    //{
                    //    return RedirectToAction("ScreeningResolution", "CustomerReg", new { IsAuthoData = true, AuthcaseSystemId = _objModel.caseSystemId });
                    //}
                    //else if (Request["btnTR_Submit"] != null)
                    //{
                    //    for (int j = 0; j < frm.Count; j++)
                    //    {
                    //        #region Update Resolution
                    //        string GetresultId = "";
                    //        string GetAllkeysStringValue = frm.AllKeys[j];
                    //        if (GetAllkeysStringValue.Contains("chk_"))
                    //        {
                    //            var RID = GetAllkeysStringValue.Split('_');
                    //            GetresultId = Convert.ToString(RID[1]);
                    //            string statusId = ""; string riskId = ""; string reasonId = ""; string resolutionRemark = "";

                    //            string strColmnStatus = "statusId_" + GetresultId;
                    //            statusId = Convert.ToString(frm[strColmnStatus]);

                    //            string strColmnriskId = "riskId_" + GetresultId;
                    //            riskId = Convert.ToString(frm[strColmnriskId]);

                    //            string strColmnreasonId = "reasonId_" + GetresultId;
                    //            reasonId = Convert.ToString(frm[strColmnreasonId]);

                    //            string strColmnresolutionRemark = "resolutionRemark_" + GetresultId;
                    //            resolutionRemark = Convert.ToString(frm[strColmnresolutionRemark]);

                    //            SystemResolutionByUser(_objModel.caseSystemId, _objModel.CustomerID, GetresultId, statusId, riskId, reasonId, resolutionRemark);
                    //        }
                    //        #endregion
                    //    }
                    //    return RedirectToAction("ScreeningResolution", "CustomerReg", new { IsAuthoData = true, AuthcaseSystemId = _objModel.caseSystemId });
                    //}
                    #endregion
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                //FillAllUserCombo();
                //FillStatusCombo(); FillRiskLevelCombo(); FillReasonCombo(); FillCountryCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult ComplianceScreening(int CustomerID = 0, int CustomerTypeID = 0, string Admin = "", bool IsAuthoData = false,
                                                string AuthcaseSystemId = "", bool SendToCredit = false, int MenuOrderNo = 0)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerDocModel _objModel = new CustomerDocModel();
                CustomerDocModel _objModelScreen = new CustomerDocModel();
                try
                {
                    _objModel.CustomerID = CustomerID; _objModelScreen.CustomerID = CustomerID;
                    _objModel.Status = currentStatus;
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.Admin = Admin;
                    _objModel.SendToCredit = SendToCredit;
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();

                    _objCustModel.CustomerID = _objModel.CustomerID;
                    _objCustModel = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                    _objModel.ProgramType = _objCustModel.ProgramType;
                    ViewBag.MenuOrderNo = MenuOrderNo;
                    ViewBag.IsScreenCompleted = _objCustModel.IsScreenCompleted;
                    var Data = _clsCust.CustomerScreening_ListAll(_objModel).ToList();
                    if (Data.Count > 0)
                    {
                        ViewBag.GetScreening = Data;
                        ViewBag.ScrStatus = 'N';
                    }
                    else
                    {
                        ViewBag.GetScreening = _clsCust.CustomerMaster_GetScreening(_objModelScreen).ToList();
                        ViewBag.ScrStatus = 'Y';
                    }
                    
                    #region Client Classification
                    _objModel.cbNo = true;
                    var GetData = _clsCust.CustomerClassDetail_ListAll(_objModel).FirstOrDefault();
                    if (GetData != null && GetData.CustomerID > 0)
                    {
                        _objModel = GetData;
                        if (GetData.IsRC == true)
                            _objModel.cbYes = true;
                        else
                            _objModel.cbNo = true;
                    }

                    _objModel.CustomerID = CustomerID;
                    _objModel.Status = currentStatus;
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.Admin = Admin;
                    _objModel.SendToCredit = SendToCredit;
                    #endregion

                    #region Thomson Reuters Integration

                    //FillAuthoriseUserCombo(CustomerID);
                    //FillStatusCombo(); FillRiskLevelCombo(); FillReasonCombo();
                    //if (_objModel.Status == CONT.NR || _objModel.Status == CONT.UR || _objModel.Status == CONT.NH || _objModel.Status == CONT.UH)
                    //{
                    //    #region Approve Button Validation
                    //    if (IsAuthoData == true)
                    //        ApproveButtonValidation(CustomerID);
                    //    else if (MenuOrderNo == 2)
                    //        ApproveButtonValidation(CustomerID);
                    //    #endregion
                    //}

                    //if (IsAuthoData == true && AuthcaseSystemId != "")
                    //{
                    //    GetScreeningResultsByUser(AuthcaseSystemId);
                    //    _objModel.caseSystemId = AuthcaseSystemId;

                    //    _ThomsonReutersModel.caseSystemId = AuthcaseSystemId;
                    //    var ScreeningData = _ClsThomsonReuters.ScreeningDetail_ListAllcaseSystemId(_ThomsonReutersModel).OrderBy(x => x.resultId).ToList();
                    //    if (ScreeningData != null && ScreeningData.Count > 0)
                    //    {
                    //        ViewBag.ScreeningData = ScreeningData;
                    //    }
                    //    return View(_objModel);
                    //}

                    //if (_objModel.Status == CONT.NR || _objModel.Status == CONT.UR || _objModel.Status == CONT.NH || _objModel.Status == CONT.UH)
                    //{
                    //    if (MenuOrderNo == 1)
                    //    {
                    //        #region Screening Record
                    //        CustomerShareHolderModel _objCustShareHolder = new CustomerShareHolderModel();
                    //        _objCustShareHolder.CustomerID = _objModel.CustomerID;
                    //        _objCustShareHolder.PlatformID = PlatformID;
                    //        var CountScreeningData = _ClsThomsonReuters.ScreeningCustomer_ListAll(_objCustShareHolder).ToList();
                    //        foreach (var item in CountScreeningData)
                    //        {
                    //            Int16 intCustomerShareHoldID = 0;

                    //            _ThomsonReutersModel.PlatformID = PlatformID;
                    //            _ThomsonReutersModel.CustomerID = item.CustomerID;
                    //            _ThomsonReutersModel.CustomerShareHoldID = item.CustomerShareHoldID;
                    //            _ThomsonReutersModel.UserID = item.UserID;
                    //            _ThomsonReutersModel.CustomerBankAccID = item.CustomerBankAccID;
                    //            _ThomsonReutersModel.BankID = item.BankID;

                    //            _ThomsonReutersModel.CustType = item.CustType;
                    //            _ThomsonReutersModel.FirstName = item.FirstName;
                    //            _ThomsonReutersModel.MiddleName = item.MiddleName;
                    //            _ThomsonReutersModel.LastName = item.LastName;
                    //            _ThomsonReutersModel.CountryID = item.NatCountryID;
                    //            _ThomsonReutersModel.LocCountryID = item.LocCountryID;
                    //            _ThomsonReutersModel.LocCountryCode = item.LocCountryCode;
                    //            _ThomsonReutersModel.NatCountryCode = item.NatCountryCode;
                    //            _ThomsonReutersModel.Gender = item.Gender;
                    //            _ThomsonReutersModel.IsCompany = false;
                    //            _ThomsonReutersModel.CompanyName = "";

                    //            if (item.CustType == "C" || item.CustType == "M" || item.CustType == "N" || item.CustType == "T" || item.CustType == "K")
                    //            {
                    //                _ThomsonReutersModel.IsCompany = true;
                    //                _ThomsonReutersModel.CountryID = item.NatCountryID;
                    //                _ThomsonReutersModel.CompanyName = item.Name;
                    //            }

                    //            var Verifycase = _ClsThomsonReuters.ScreeningMaster_VerifycaseID(_ThomsonReutersModel).FirstOrDefault();
                    //            if (Verifycase != null)
                    //            {
                    //                string CaseID = ""; string strcaseSystemId = "";

                    //                #region Step 1
                    //                bool IsNew = Verifycase.IsNew;
                    //                if (IsNew)
                    //                {
                    //                    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
                    //                    clsTRLog.Conn = ClsAppDatabase.GetCon();
                    //                    if (clsTRLog.Conn.State == ConnectionState.Closed) { clsTRLog.Conn.Open(); }
                    //                    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
                    //                    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

                    //                    CaseID = clsTRLog.ScreeningMaster_Add(_ThomsonReutersModel);

                    //                    clsTRLog.Tras.Commit(); clsTRLog.Conn.Close();

                    //                    caseIdentifiers(CaseID, _objModel.CustomerID, intCustomerShareHoldID);
                    //                }
                    //                #endregion

                    //                #region Step 2
                    //                if (Verifycase.caseSystemId == "")
                    //                {
                    //                    if (CaseID != "")
                    //                        strcaseSystemId = caseSystemId(CaseID, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode);
                    //                    else
                    //                        strcaseSystemId = caseSystemId(Verifycase.caseId, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode);
                    //                }
                    //                #endregion

                    //                #region Step 3
                    //                if (!Verifycase.IsScreening)
                    //                {
                    //                    if (Verifycase.caseSystemId != "")
                    //                    {
                    //                        screeningRequest(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                    }
                    //                    else
                    //                    {
                    //                        if (strcaseSystemId != null && strcaseSystemId != "")
                    //                        {
                    //                            if (CaseID != "")
                    //                                screeningRequest(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                            else
                    //                                screeningRequest(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                        }
                    //                    }
                    //                }
                    //                #endregion

                    //                #region Step 4
                    //                if (!Verifycase.IsResolved)
                    //                {
                    //                    if (Verifycase.caseSystemId != "")
                    //                    {

                    //                        GetScreeningResults(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                    //                        #region Is Delete Result And Re-Searching Logic
                    //                        if (ViewBag.IsDeleteResult == true)
                    //                        {
                    //                            Verifycase = _ClsThomsonReuters.ScreeningMaster_VerifycaseID(_ThomsonReutersModel).FirstOrDefault();
                    //                            if (Verifycase != null)
                    //                            {
                    //                                CaseID = ""; strcaseSystemId = "";

                    //                                #region Step 1
                    //                                IsNew = Verifycase.IsNew;
                    //                                if (IsNew)
                    //                                {
                    //                                    ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
                    //                                    clsTRLog.Conn = ClsAppDatabase.GetCon();
                    //                                    if (clsTRLog.Conn.State == ConnectionState.Closed) { clsTRLog.Conn.Open(); }
                    //                                    else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
                    //                                    clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

                    //                                    CaseID = clsTRLog.ScreeningMaster_Add(_ThomsonReutersModel);
                    //                                    clsTRLog.Tras.Commit(); clsTRLog.Conn.Close();
                    //                                    caseIdentifiers(CaseID, _objModel.CustomerID, intCustomerShareHoldID);
                    //                                }
                    //                                #endregion

                    //                                #region Step 2
                    //                                if (Verifycase.caseSystemId == "")
                    //                                {
                    //                                    if (CaseID != "")
                    //                                        strcaseSystemId = caseSystemId(CaseID, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode);
                    //                                    else
                    //                                        strcaseSystemId = caseSystemId(Verifycase.caseId, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode);
                    //                                }
                    //                                #endregion

                    //                                #region Step 3
                    //                                if (!Verifycase.IsScreening)
                    //                                {
                    //                                    if (Verifycase.caseSystemId != "")
                    //                                    {
                    //                                        screeningRequest(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        if (strcaseSystemId != null && strcaseSystemId != "")
                    //                                        {
                    //                                            if (CaseID != "")
                    //                                                screeningRequest(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                                            else
                    //                                                screeningRequest(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                                        }
                    //                                    }
                    //                                }
                    //                                #endregion

                    //                                #region Step 4
                    //                                if (!Verifycase.IsResolved)
                    //                                {
                    //                                    if (Verifycase.caseSystemId != "")
                    //                                    {

                    //                                        GetScreeningResults(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);

                    //                                        //EnableOngoingScreeningByUser(Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                    //                                        _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                    //                                        if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                    //                                        { _ClsThomsonReuters.Conn.Open(); }
                    //                                        else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                    //                                        _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                    //                                        _ThomsonReutersModel.caseSystemId = Verifycase.caseSystemId;
                    //                                        _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                    //                                        _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        if (strcaseSystemId != null && strcaseSystemId != "")
                    //                                        {
                    //                                            if (CaseID != "")
                    //                                                GetScreeningResults(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);
                    //                                            else
                    //                                                GetScreeningResults(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);
                    //                                            //EnableOngoingScreeningByUser(VerifyCaseSystemID.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                    //                                            _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                    //                                            if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                    //                                            { _ClsThomsonReuters.Conn.Open(); }
                    //                                            else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                    //                                            _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                    //                                            _ThomsonReutersModel.caseSystemId = strcaseSystemId;
                    //                                            _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                    //                                            _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                    //                                        }
                    //                                    }
                    //                                }
                    //                                #endregion
                    //                            }
                    //                        }
                    //                        #endregion


                    //                        //EnableOngoingScreeningByUser(Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                    //                        _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                    //                        if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                    //                        { _ClsThomsonReuters.Conn.Open(); }
                    //                        else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                    //                        _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                    //                        _ThomsonReutersModel.caseSystemId = Verifycase.caseSystemId;
                    //                        _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                    //                        _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                    //                    }
                    //                    else
                    //                    {
                    //                        if (strcaseSystemId != null && strcaseSystemId != "")
                    //                        {
                    //                            if (CaseID != "")
                    //                                GetScreeningResults(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                            else
                    //                                GetScreeningResults(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                            //EnableOngoingScreeningByUser(VerifyCaseSystemID.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                    //                            #region Is Delete Result And Re-Searching Logic
                    //                            if (ViewBag.IsDeleteResult == true)
                    //                            {
                    //                                Verifycase = _ClsThomsonReuters.ScreeningMaster_VerifycaseID(_ThomsonReutersModel).FirstOrDefault();
                    //                                if (Verifycase != null)
                    //                                {
                    //                                    CaseID = ""; strcaseSystemId = "";

                    //                                    #region Step 1
                    //                                    IsNew = Verifycase.IsNew;
                    //                                    if (IsNew)
                    //                                    {
                    //                                        ClsThomsonReuters clsTRLog = new ClsThomsonReuters();
                    //                                        clsTRLog.Conn = ClsAppDatabase.GetCon();
                    //                                        if (clsTRLog.Conn.State == ConnectionState.Closed) { clsTRLog.Conn.Open(); }
                    //                                        else { clsTRLog.Conn.Close(); clsTRLog.Conn.Open(); }
                    //                                        clsTRLog.Tras = clsTRLog.Conn.BeginTransaction();

                    //                                        CaseID = clsTRLog.ScreeningMaster_Add(_ThomsonReutersModel);
                    //                                        clsTRLog.Tras.Commit(); clsTRLog.Conn.Close();
                    //                                        caseIdentifiers(CaseID, _objModel.CustomerID, intCustomerShareHoldID);
                    //                                    }
                    //                                    #endregion

                    //                                    #region Step 2
                    //                                    if (Verifycase.caseSystemId == "")
                    //                                    {
                    //                                        if (CaseID != "")
                    //                                            strcaseSystemId = caseSystemId(CaseID, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode, true);
                    //                                        else
                    //                                            strcaseSystemId = caseSystemId(Verifycase.caseId, item.Name, _objModel.CustomerID, intCustomerShareHoldID, item.NatCountryCode, item.CustType, item.Gender, item.LocCountryCode, true);
                    //                                    }
                    //                                    #endregion

                    //                                    #region Step 3
                    //                                    if (!Verifycase.IsScreening)
                    //                                    {
                    //                                        if (Verifycase.caseSystemId != "")
                    //                                        {
                    //                                            screeningRequest(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                                        }
                    //                                        else
                    //                                        {
                    //                                            if (strcaseSystemId != null && strcaseSystemId != "")
                    //                                            {
                    //                                                if (CaseID != "")
                    //                                                    screeningRequest(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                                                else
                    //                                                    screeningRequest(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID);
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                    #endregion

                    //                                    #region Step 4
                    //                                    if (!Verifycase.IsResolved)
                    //                                    {
                    //                                        if (Verifycase.caseSystemId != "")
                    //                                        {

                    //                                            GetScreeningResults(Verifycase.caseId, Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);

                    //                                            //EnableOngoingScreeningByUser(Verifycase.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                    //                                            _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                    //                                            if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                    //                                            { _ClsThomsonReuters.Conn.Open(); }
                    //                                            else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                    //                                            _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                    //                                            _ThomsonReutersModel.caseSystemId = Verifycase.caseSystemId;
                    //                                            _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                    //                                            _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                    //                                        }
                    //                                        else
                    //                                        {
                    //                                            if (strcaseSystemId != null && strcaseSystemId != "")
                    //                                            {
                    //                                                if (CaseID != "")
                    //                                                    GetScreeningResults(CaseID, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);
                    //                                                else
                    //                                                    GetScreeningResults(Verifycase.caseId, strcaseSystemId, _objModel.CustomerID, intCustomerShareHoldID, true);
                    //                                                //EnableOngoingScreeningByUser(VerifyCaseSystemID.caseSystemId, _objModel.CustomerID, intCustomerShareHoldID);

                    //                                                _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                    //                                                if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                    //                                                { _ClsThomsonReuters.Conn.Open(); }
                    //                                                else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                    //                                                _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                    //                                                _ThomsonReutersModel.caseSystemId = strcaseSystemId;
                    //                                                _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                    //                                                _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                    #endregion
                    //                                }
                    //                            }
                    //                            #endregion


                    //                            _ClsThomsonReuters.Conn = ClsAppDatabase.GetCon();
                    //                            if (_ClsThomsonReuters.Conn.State == ConnectionState.Closed)
                    //                            { _ClsThomsonReuters.Conn.Open(); }
                    //                            else { _ClsThomsonReuters.Conn.Close(); _ClsThomsonReuters.Conn.Open(); }
                    //                            _ClsThomsonReuters.Tras = _ClsThomsonReuters.Conn.BeginTransaction();

                    //                            _ThomsonReutersModel.caseSystemId = strcaseSystemId;
                    //                            _ClsThomsonReuters.ScreeningMaster_UpdateResolved(_ThomsonReutersModel);

                    //                            _ClsThomsonReuters.Tras.Commit(); _ClsThomsonReuters.Conn.Close();
                    //                        }
                    //                    }
                    //                }
                    //                #endregion
                    //            }
                    //        }
                    //        if (IsAuthoData == false)
                    //        {
                    //            ApproveButtonValidation(CustomerID);
                    //            FillAuthoriseUserCombo(CustomerID);
                    //        }
                    //        #endregion
                    //    }
                    //}

                    //#region Page Load
                    //if (IsAuthoData == false && AuthcaseSystemId == "")
                    //{
                    //    CustomerShareHolderModel _objLoadData = new CustomerShareHolderModel();
                    //    _objLoadData.PlatformID = PlatformID;
                    //    _objLoadData.CustomerID = CustomerID;
                    //    var GetCompanyData = _ClsThomsonReuters.ScreeningCustomer_ListAll(_objLoadData).ToList();
                    //    if (GetCompanyData != null && GetCompanyData.Count > 0)
                    //    {
                    //        foreach (var item in GetCompanyData)
                    //        {
                    //            if (item.CustType == "M" && item.caseSystemId != "")
                    //            {
                    //                //_ThomsonReutersModel.caseSystemId = item.caseSystemId;
                    //                //var ScreeningData = _ClsThomsonReuters.ScreeningDetail_ListAllcaseSystemId(_ThomsonReutersModel).OrderBy(x => x.resultId).ToList();
                    //                //if (ScreeningData != null && ScreeningData.Count > 0)
                    //                //{
                    //                //    ViewBag.ScreeningData = ScreeningData;
                    //                //    ViewBag.Result = "";
                    //                //}

                    //                GetScreeningResultsByUser(item.caseSystemId);
                    //                _objModel.caseSystemId = item.caseSystemId;

                    //                _ThomsonReutersModel.caseSystemId = item.caseSystemId;
                    //                var ScreeningData = _ClsThomsonReuters.ScreeningDetail_ListAllcaseSystemId(_ThomsonReutersModel).OrderBy(x => x.resultId).ToList();
                    //                if (ScreeningData != null && ScreeningData.Count > 0)
                    //                {
                    //                    ViewBag.ScreeningData = ScreeningData;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    //#endregion
                    #endregion

                    return View(_objModel);
                }

                catch (Exception ex)
                {
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillAuthoriseUserCombo(CustomerID);
                //FillStatusCombo(); FillRiskLevelCombo(); FillReasonCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult ComplianceScreening(CustomerDocModel _objModel, FormCollection frm)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    #region Thomson Reuters Integration
                    if (Request["btnTR_Display"] != null)
                    {
                        return RedirectToAction("ComplianceScreening", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, IsAuthoData = true, AuthcaseSystemId = _objModel.caseSystemId, Admin = _objModel.Admin, MenuOrderNo = 1 });
                    }
                    else if (Request["btnScreening_Next"] != null)
                    {
                        return RedirectToAction("ComplianceScreening", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, IsAuthoData = true, AuthcaseSystemId = _objModel.caseSystemId, Admin = _objModel.Admin, MenuOrderNo = 2 });
                    }
                    else if (Request["btnClassification_Next"] != null)
                    {
                        return RedirectToAction("CustomerRisk", "CustomerReg", new { CustomerID = _objModel.CustomerID, Admin = _objModel.Admin });
                    }

                    else if (Request["btnTR_Submit"] != null)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        var Data = _clsCust.CustomerMaster_GetScreening(_objModel).ToList();
                        for (int i = 0; i < Data.Count; i++)
                        {
                            int intID = i+1;
                            for (int j = 0; j < frm.Count; j++)
                            {
                                string GetAllkeysStringValue = frm.AllKeys[j];
                                if (GetAllkeysStringValue.Equals("ScrStatus_" + intID + ""))
                                {
                                    _objModel.ScrStatus = Convert.ToString(frm["ScrStatus_" + intID + ""]);
                                    _objModel.Name= Data[i].Name;
                                    _objModel.Type = Data[i].Type;
                                    _objModel.Gender = Data[i].Gender;
                                    _objModel.CountryName = Data[i].CountryName;
                                    _clsCust.CustomerScreening_Add(_objModel);
                                }
                                #region Update Resolution
                                //string GetresultId = "";
                                //string GetAllkeysStringValue = frm.AllKeys[j];
                                //if (GetAllkeysStringValue.Contains("chk_"))
                                //{
                                //    var RID = GetAllkeysStringValue.Split('_');
                                //    GetresultId = Convert.ToString(RID[1]);
                                //    string statusId = ""; string riskId = ""; string reasonId = ""; string resolutionRemark = "";

                                //    string strColmnStatus = "statusId_" + GetresultId;
                                //    statusId = Convert.ToString(frm[strColmnStatus]);

                                //    string strColmnriskId = "riskId_" + GetresultId;
                                //    riskId = Convert.ToString(frm[strColmnriskId]);

                                //    string strColmnreasonId = "reasonId_" + GetresultId;
                                //    reasonId = Convert.ToString(frm[strColmnreasonId]);

                                //    string strColmnresolutionRemark = "resolutionRemark_" + GetresultId;
                                //    resolutionRemark = Convert.ToString(frm[strColmnresolutionRemark]);

                                //    SystemResolutionByUser(_objModel.caseSystemId, _objModel.CustomerID, GetresultId, statusId, riskId, reasonId, resolutionRemark);

                                //    //string IsOGS = Convert.ToString(frm["IsOGS"]);

                                //    //if (IsOGS != null)
                                //    //{
                                //    //    EnableOngoingScreeningByUser(_objModel.caseSystemId, _objModel.CustomerID);
                                //    //}
                                //    //else
                                //    //{
                                //    //    DisableOngoingScreeningByUser(_objModel.caseSystemId, _objModel.CustomerID);
                                //    //}
                                //}
                                #endregion
                            }
                        }
                        _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                        //return RedirectToAction("ComplianceScreening", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, IsAuthoData = true, AuthcaseSystemId = _objModel.caseSystemId, Admin = _objModel.Admin, MenuOrderNo = 1 });
                        return RedirectToAction("ComplianceScreening", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, IsAuthoData = true, AuthcaseSystemId = _objModel.caseSystemId, Admin = _objModel.Admin, MenuOrderNo = 2 });
                    }
                    else if (Request["btn_Classification_Submit"] != null)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();

                        var GetData = _clsCust.CustomerClassDetail_ListAll(_objModel).FirstOrDefault();

                        if (_objModel.cbYes == true)
                            _objModel.IsRC = true;
                        else
                            _objModel.IsRC = false;

                        if (Request.Form["rdo_CLIENTY"] != null)
                            _objModel.Category = "Client";
                        else if (Request.Form["rdo_BUSINESSPARTNER"] != null)
                            _objModel.Category = "Business Partner";
                        else if (Request.Form["rdo_RELATEDPARTY"] != null)
                            _objModel.Category = "Related Party";
                        else if (Request.Form["rdo_SERVICEPROVIDER"] != null)
                            _objModel.Category = "Service Provider";

                        if (GetData != null && GetData.CustomerID > 0)
                        {
                            _objModel.CustomerClassId = GetData.CustomerClassId;
                            _clsCust.CustomerClassDetail_Update(_objModel);
                            _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                        }
                        else
                        {
                            _clsCust.CustomerClassDetail_Add(_objModel);
                            _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                        }

                        //return RedirectToAction("CustomerRisk", "CustomerReg", new { CustomerID = _objModel.CustomerID, Admin = _objModel.Admin });
                        return RedirectToAction("ComplianceScreening", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, IsAuthoData = true, AuthcaseSystemId = _objModel.caseSystemId, Admin = _objModel.Admin, MenuOrderNo = 2 });
                    }
                    #endregion

                    else if (Request["btn_Classification_Print"] != null)
                    {
                        return RedirectToAction("PrintClientClassification", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }

                    else if (Request["onhold"] != null)
                    {
                        if (_objModel.Status == CONT.UR || _objModel.Status == CONT.UH) { _objModel.Status = CONT.UH; }
                        else { _objModel.Status = CONT.NH; }
                    }

                    else if (Request["Approve"] != null)
                    {
                        if (_objModel.CustomerTypeID == CONT.FunderCustomerTypeID)
                        {
                            if (_objModel.Status == CONT.UR || _objModel.Status == CONT.UH) { _objModel.Status = CONT.UV; }
                            else { _objModel.Status = CONT.NV; }
                        }
                        else
                        {
                            if (_objModel.Status == CONT.UR || _objModel.Status == CONT.UH) { _objModel.Status = CONT.UP; }
                            else
                            {
                                var CheckScreening = _clsCust.CustomerMaster_CheckScreening(_objModel).FirstOrDefault();
                                if (CheckScreening.IsOPTeam)
                                {
                                    _objModel.Status = CONT.NV;
                                }
                                else
                                {
                                    _objModel.Status = CONT.NP;
                                }
                            }

                        }
                    }

                    else if (Request["Reject"] != null)
                    {
                        if (_objModel.Status == CONT.UR || _objModel.Status == CONT.UH) { _objModel.Status = CONT.UT; }
                        else { _objModel.Status = CONT.NT; }
                    }
                    else if (Request["Next"] != null)
                    {
                        if ((_objModel.Status == CONT.NP || _objModel.Status == CONT.NK) || ((_objModel.Status == CONT.UK || _objModel.Status == CONT.NA || _objModel.Status == CONT.UA) && _objModel.SendToCredit == true))
                        {
                            return RedirectToAction("CreditReview", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, ProgramType = _objModel.ProgramType });
                        }

                        else
                        {
                            if (_objModel.CustomerTypeID == CONT.FunderCustomerTypeID)
                            {
                                return RedirectToAction("AccountActivation", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID });
                            }
                            else
                            {
                                return RedirectToAction("CreditReviewApprove", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, ProgramType = _objModel.ProgramType, Admin = _objModel.Admin });
                            }
                        }
                    }
                    else if (Request["Return"] != null)
                    {
                        _objModel.Status = CONT.ND;
                    }
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    _clsCust.CustomerMaster_UpdateAllStatus(_objModel);
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    return RedirectToAction("CMNDashboard", "Home");
                }

                catch (Exception ex)
                {
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillAuthoriseUserCombo(_objModel.CustomerID);
                //FillStatusCombo(); FillRiskLevelCombo(); FillReasonCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult PrintClientClassification(int CustomerID = 0)
        {
            try
            {
                CustomerDocModel _objModel = new CustomerDocModel();
                _objModel.CustomerID = CustomerID;
                _objModel.cbNo = true;
                var GetData = _clsCust.CustomerClassDetail_ListAll(_objModel).FirstOrDefault();
                if (GetData != null && GetData.CustomerID > 0)
                {
                    _objModel = GetData;
                    if (GetData.IsRC == true)
                        _objModel.cbYes = true;
                    else
                        _objModel.cbNo = true;
                }
                return new Rotativa.MVC.ViewAsPdf("PrintClientClassification", _objModel);
            }
            catch (Exception ex)
            { throw ex; }
        }
        public ActionResult PrintRiskCategorization(int CustomerID = 0)
        {
            try
            {
                ScreeningConfig _objModel = new ScreeningConfig();
                _objModel.CustomerID = CustomerID;

                var Data = _clsCust.CustomerRiskDetail_ListAll(_objModel).ToList();
                ViewBag.Category = _clsContr.CategoryMaster_ListAll(_objModel).ToList();
                ViewBag.Country = _clsCust.CustomerRiskDetail_GetCountryRisk(_objModel);
                ViewBag.Cust_Result = _clsCust.CustomerRiskDetail_Result(_objModel);
                ViewBag.Cust_Result1 = _clsCust.CustomerRiskDetail_Result1(_objModel);
                return new Rotativa.MVC.ViewAsPdf("PrintRiskCategorization", Data);
            }
            catch (Exception ex)
            { throw ex; }
        }
        public ActionResult CustomerRisk(int CustomerID = 0, string Admin = "", bool SendToCredit = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    //ApproveButtonValidation(CustomerID);
                    ScreeningConfig _objModel = new ScreeningConfig();
                    ViewBag.CustomerID = _objModel.CustomerID = CustomerID;
                    var Data = _clsCust.CustomerRiskDetail_ListAll(_objModel).ToList();
                    ViewBag.Category = _clsContr.CategoryMaster_ListAll(_objModel).ToList();
                    ViewBag.Country = _clsCust.CustomerRiskDetail_GetCountryRisk(_objModel);
                    ViewBag.Cust_Result = _clsCust.CustomerRiskDetail_Result(_objModel);
                    ViewBag.Cust_Result1 = _clsCust.CustomerRiskDetail_Result1(_objModel);
                    ViewBag.Admin = Admin; ViewBag.SendToCredit = SendToCredit;
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.CustomerID = CustomerID;
                    _objCustModel = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                    ViewBag.IsScreenCompleted = _objCustModel.IsScreenCompleted;

                    CustomerDocModel _CustomerDocModel = new CustomerDocModel();
                    _CustomerDocModel.cbNo = true;
                    _CustomerDocModel.CustomerID = CustomerID;
                    var GetClassDetailData = _clsCust.CustomerClassDetail_ListAll(_CustomerDocModel).FirstOrDefault();
                    if (GetClassDetailData != null)
                    {
                        ViewBag.CustomerClassId = GetClassDetailData.CustomerClassId;
                    }

                    //if (ViewBag.btnApproveVisible == false || GetClassDetailData ==null)
                    //{
                    //    return RedirectToAction("ComplianceScreening", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, Admin = _objModel.Admin, MenuOrderNo = 2 });
                    //}

                    ViewBag.CustomerMaster = _clsCust.CustomerMasterHistory_ListAllBind(_objCustModel).FirstOrDefault();

                    return View(Data);
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }

        }
        [HttpPost]
        public ActionResult CustomerRisk(FormCollection frm, ScreeningConfig _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    CustomerDocModel _CustomerDocModel = new CustomerDocModel();
                    _CustomerDocModel.CustomerTypeID = _objModel.CustomerTypeID;
                    _CustomerDocModel.CustomerID = _objModel.CustomerID;

                    // if (Request["btn_Result_Print"] != null)
                    //{
                    //    return RedirectToAction("PrintRiskCategorization", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    //}

                    if (Request["Submit"] != null)
                    {
                        int intTmpCategoryDetID = _objModel.CategoryDetID;
                        _objModel.CategoryDetID = 0;
                        var Data = _clsCust.CustomerRiskDetail_ListAll(_objModel).ToList();
                        _objModel.CategoryDetID = intTmpCategoryDetID;
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        if (_objModel.CustomerRiskID == 0)
                            _clsCust.CustomerRiskDetail_AddCountryRisk(_objModel);
                        else
                            _clsCust.CustomerRiskDetail_UpdateCountryRisk(_objModel);

                        for (int i = 0; i < Data.Count; i++)
                        {
                            int intCatDelID = Data[i].CategoryDetID;

                            for (int j = 0; j < frm.Count; j++)
                            {
                                string GetAllkeysStringValue = frm.AllKeys[j];
                                if (GetAllkeysStringValue.Equals("selTy_" + intCatDelID + ""))
                                {
                                    _objModel.CategoryDetID = intCatDelID;
                                    _objModel.SelType = Convert.ToString(frm["selTy_" + intCatDelID + ""]);
                                    _objModel.CustomerRiskID = Convert.ToInt32(frm["RiskID_" + intCatDelID + ""]);
                                    if (Convert.ToString(frm["Y_SelectType_" + intCatDelID + ""]) != "" && Convert.ToString(frm["Y_SelectType_" + intCatDelID + ""]) != "N")
                                        _objModel.SelTypeYN = "Y";
                                    else
                                        _objModel.SelTypeYN = "N";


                                    #region Check Is Override Or Not
                                    if (Convert.ToString(frm["Y_SelectType_" + intCatDelID + ""]) != "" && Convert.ToString(frm["Y_SelectType_" + intCatDelID + ""]) != "N")
                                    {
                                        ClsCountryMaster _ClsCountryMaster = new ClsCountryMaster();
                                        _objModel.CategoryDetID = intCatDelID; _objModel.CategoryID = 0; _objModel.ParentCategoryDetID = 0;
                                        var GetCatDetailsData = _ClsCountryMaster.CategoryDetail_ListAll(_objModel).FirstOrDefault();
                                        if (GetCatDetailsData != null)
                                        {
                                            if (GetCatDetailsData.IsOverride)
                                            {
                                                _objModel.SelType = GetCatDetailsData.OveRiskLevel;
                                            }
                                        }
                                    }

                                    #endregion


                                    if (_objModel.CustomerRiskID > 0)
                                    {
                                        _clsCust.CustomerRiskDetail_Update(_objModel);
                                        #region Check Customer Is PEP Or Not
                                        if (intCatDelID == 1)
                                        {
                                            _objModel.CategoryDetID = 1;
                                            var GetData = _clsCust.CustomerRiskDetail_ListAll(_objModel).FirstOrDefault();
                                            if (GetData != null)
                                            {
                                                if (GetData.SelType == "N")
                                                {
                                                    _objModel.CategoryDetID = 2;
                                                    var GetData_Two = _clsCust.CustomerRiskDetail_ListAll(_objModel).FirstOrDefault();
                                                    if (GetData_Two != null)
                                                    {
                                                        _objModel.CustomerRiskID = 0;
                                                        _objModel.CustomerRiskID = GetData_Two.CustomerRiskID; _objModel.CategoryDetID = 2; _objModel.SelType = "N";
                                                        _clsCust.CustomerRiskDetail_Update(_objModel);
                                                    }

                                                    _objModel.CategoryDetID = 3;
                                                    var GetData_Three = _clsCust.CustomerRiskDetail_ListAll(_objModel).FirstOrDefault();
                                                    if (GetData_Three != null)
                                                    {
                                                        _objModel.CustomerRiskID = 0;
                                                        _objModel.CustomerRiskID = GetData_Three.CustomerRiskID; _objModel.CategoryDetID = 3; _objModel.SelType = "N";
                                                        _clsCust.CustomerRiskDetail_Update(_objModel);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        _clsCust.CustomerRiskDetail_Add(_objModel);

                                        #region Check Customer Is PEP Or Not
                                        if (intCatDelID == 1)
                                        {
                                            _objModel.CategoryDetID = 1;
                                            var GetData = _clsCust.CustomerRiskDetail_ListAll(_objModel).FirstOrDefault();
                                            if (GetData != null)
                                            {
                                                if (GetData.SelType == "N")
                                                {
                                                    _objModel.CategoryDetID = 2;
                                                    var GetData_Two = _clsCust.CustomerRiskDetail_ListAll(_objModel).FirstOrDefault();
                                                    if (GetData_Two == null)
                                                    {
                                                        _objModel.CategoryDetID = 2; _objModel.SelType = "N";
                                                        _clsCust.CustomerRiskDetail_Add(_objModel);
                                                    }
                                                    else
                                                    {
                                                        _objModel.CategoryRiskID = GetData_Two.CustomerRiskID; _objModel.CategoryDetID = 2; _objModel.SelType = "N";
                                                        _clsCust.CustomerRiskDetail_Update(_objModel);
                                                    }

                                                    _objModel.CategoryDetID = 3;
                                                    var GetData_Three = _clsCust.CustomerRiskDetail_ListAll(_objModel).FirstOrDefault();
                                                    if (GetData_Three == null)
                                                    {
                                                        _objModel.CategoryDetID = 3; _objModel.SelType = "N";
                                                        _clsCust.CustomerRiskDetail_Add(_objModel);
                                                    }
                                                    else
                                                    {
                                                        _objModel.CategoryRiskID = GetData_Three.CustomerRiskID; _objModel.CategoryDetID = 3; _objModel.SelType = "N";
                                                        _clsCust.CustomerRiskDetail_Update(_objModel);
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }

                        _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                        return RedirectToAction("CustomerRisk", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                    }
                    else if (Request["Next"] != null)
                    {
                        if ((_objModel.Status == CONT.NP || _objModel.Status == CONT.NK || _objModel.Status == CONT.NL) || ((_objModel.Status == CONT.UK || _objModel.Status == CONT.NA || _objModel.Status == CONT.UA) && _objModel.SendToCredit == true))
                        {
                            return RedirectToAction("CreditReview", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, ProgramType = _objModel.ProgramType });
                        }
                        else
                        {
                            if (_objModel.CustomerTypeID == CONT.FunderCustomerTypeID)
                            {
                                return RedirectToAction("AccountActivation", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID });
                            }
                            else
                            {
                                return RedirectToAction("CreditReviewApprove", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, ProgramType = _objModel.ProgramType, Admin = _objModel.Admin });
                            }
                        }

                        //if ((_objModel.Status == CONT.NP || _objModel.Status == CONT.NK) || ((_objModel.Status == CONT.UK || _objModel.Status == CONT.NA || _objModel.Status == CONT.UA) && _objModel.SendToCredit == true))
                        //{
                        //    return RedirectToAction("CreditReview", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID });
                        //}
                        //else
                        //{
                        //    return RedirectToAction("CreditReviewApprove", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, Admin = _objModel.Admin });
                        //}
                    }

                    #region Approve Code
                    if (Request["Approve"] != null || Request["Reject"] != null || Request["Return"] != null
                            || Request["btnSrMgmtChkoff"] != null || Request["btnSrMgmtReject"] != null || Request["btnSrMgmtApprove"] != null)
                    {
                        string strProcessRemark = Request["ProcessRemark"];
                        _CustomerDocModel.ProcessRemark = strProcessRemark;
                        if (Request["Approve"] != null)
                        {
                            if (_CustomerDocModel.CustomerTypeID == CONT.FunderCustomerTypeID)
                            {
                                if (_CustomerDocModel.Status == CONT.UR || _CustomerDocModel.Status == CONT.UH) { _CustomerDocModel.Status = CONT.UV; }
                                else { _CustomerDocModel.Status = CONT.NV; }
                            }
                            else
                            {
                                if (_CustomerDocModel.Status == CONT.UR || _CustomerDocModel.Status == CONT.UH) { _CustomerDocModel.Status = CONT.UP; }
                                else
                                {
                                    var CheckScreening = _clsCust.CustomerMaster_CheckScreening(_CustomerDocModel).FirstOrDefault();
                                    if (CheckScreening.IsOPTeam)
                                    {
                                        _CustomerDocModel.Status = CONT.NV;
                                    }
                                    else
                                    {
                                        _CustomerDocModel.Status = CONT.NP;
                                    }
                                }

                            }
                        }
                        else if (Request["Reject"] != null)
                        {
                            if (_CustomerDocModel.Status == CONT.UR || _CustomerDocModel.Status == CONT.UH) { _CustomerDocModel.Status = CONT.UT; }
                            else { _CustomerDocModel.Status = CONT.NT; }
                        }
                        else if (Request["Return"] != null)
                        {
                            _CustomerDocModel.Status = CONT.ND;
                        }
                        else if (Request["btnSrMgmtChkoff"] != null)
                        {
                            if (_CustomerDocModel.CustomerTypeID == CONT.FunderCustomerTypeID)
                            {
                                _CustomerDocModel.Status = CONT.NV;
                            }
                            else
                            {
                                _CustomerDocModel.Status = CONT.NL;
                            }

                        }

                        else if (Request["btnSrMgmtReject"] != null)
                        {
                            _CustomerDocModel.Status = CONT.NB;
                        }

                        else if (Request["btnSrMgmtApprove"] != null)
                        {
                            _CustomerDocModel.Status = CONT.NL;
                        }

                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        _clsCust.CustomerMaster_UpdateAllStatus(_CustomerDocModel);
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        return RedirectToAction("CMNDashboard", "Home");
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult CreditReview(int CustomerID = 0, int CustomerTypeID = 0, string ProgramType = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerReviewModel _objModel = new CustomerReviewModel();
                try
                {
                    bool Rights = _RightsNoaccessPage();
                    if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    _objModel.CustomerID = CustomerID;
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.ProgramType = ProgramType;

                    if (CustomerTypeID == CONT.BothObligorAndBuyerTypeID)
                    {
                        _objModel.ProgramType = CONT.RType;
                    }
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    if (CheckCount != null)
                    {
                        _objModel = CheckCount;
                    }
                    if (CustomerTypeID == CONT.BothObligorAndBuyerTypeID)
                    {
                        CustomerReviewModel _objNewModel = new CustomerReviewModel();
                        _objNewModel.ProgramType = CONT.FType; _objNewModel.CustomerID = CustomerID;
                        var FCheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objNewModel).FirstOrDefault();
                        if (FCheckCount != null)
                        {
                            _objModel.FCustomerRevID = FCheckCount.CustomerRevID; _objModel.FCurrencyID = FCheckCount.CurrencyID; _objModel.FCurrencyCode = FCheckCount.CurrencyCode; _objModel.FLimit = FCheckCount.Limit; _objModel.FTenor = FCheckCount.Tenor;
                        }
                    }

                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();

                    _objCustModel.CustomerID = _objModel.CustomerID;
                    var GetRemarksData = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                    if (GetRemarksData != null)
                        _objModel.ProcessRemark = GetRemarksData.ProcessRemark;
                    else
                        _objModel.ProcessRemark = "";

                    FillCurrencyCombo();
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.ProgramType = ProgramType;
                    _objModel.Status = currentStatus;
                    return View(_objModel);
                }

                catch (Exception ex)
                {
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult CreditReview(CustomerReviewModel _objModel, HttpPostedFileBase UpdDiligRpt, HttpPostedFileBase UpdCredRpt)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {

                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                try
                {
                    string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                    if (UpdDiligRpt != null)
                    {
                        if (UpdDiligRpt.FileName != null)
                        {
                            _objModel.UpdDiligRpt = newfilenm + UpdDiligRpt.FileName.ToString();
                            UpdDiligRpt.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdDiligRpt));
                        }
                    }
                    string newfilenm1 = Guid.NewGuid().ToString().Substring(0, 12);
                    if (UpdCredRpt != null)
                    {
                        if (UpdCredRpt.FileName != null)
                        {
                            _objModel.UpdCredRpt = newfilenm1 + UpdCredRpt.FileName.ToString();
                            UpdCredRpt.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdCredRpt));
                        }
                    }

                    if (_objModel.CustomerTypeID == CONT.BothObligorAndBuyerTypeID)
                    {
                        if (_objModel.CustomerRevID > 0 && _objModel.FCustomerRevID > 0)
                        {
                            if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA || _objModel.Status == CONT.UP || _objModel.Status == CONT.UM)
                            {
                                _objModel.Status = CONT.UC;
                                _objModel.ProgramType = CONT.RType;
                                _clsCust.CustomerReviewDetailHistory_Add(_objModel);


                                CustomerReviewModel _objNewModel = new CustomerReviewModel();
                                _objNewModel.ProgramType = CONT.FType;
                                _objNewModel.Status = CONT.NC; _objNewModel.CustomerID = _objModel.CustomerID; _objNewModel.CustomerRevID = _objModel.FCustomerRevID;
                                _objNewModel.CurrencyID = _objModel.FCurrencyID; _objNewModel.Limit = _objModel.FLimit; _objNewModel.Tenor = _objModel.FTenor;
                                _clsCust.CustomerReviewDetailHistory_Add(_objNewModel);
                            }
                            else
                            {
                                _objModel.Status = CONT.NC;
                                _objModel.ProgramType = CONT.RType;
                                _clsCust.CustomerReviewDetail_Update(_objModel);

                                CustomerReviewModel _objNewModel = new CustomerReviewModel();
                                _objNewModel.ProgramType = CONT.FType;
                                _objNewModel.Status = CONT.NC; _objNewModel.CustomerID = _objModel.CustomerID; _objNewModel.CustomerRevID = _objModel.FCustomerRevID;
                                _objNewModel.CurrencyID = _objModel.FCurrencyID; _objNewModel.Limit = _objModel.FLimit; _objNewModel.Tenor = _objModel.FTenor;
                                _clsCust.CustomerReviewDetail_Update(_objNewModel);
                            }
                        }
                        else
                        {
                            _objModel.ProgramType = CONT.RType;
                            _objModel.Status = CONT.NC;
                            _clsCust.CustomerReviewDetail_Add(_objModel);

                            CustomerReviewModel _objNewModel = new CustomerReviewModel();
                            _objNewModel.ProgramType = CONT.FType;
                            _objNewModel.Status = CONT.NC; _objNewModel.CustomerID = _objModel.CustomerID;
                            _objNewModel.CurrencyID = _objModel.FCurrencyID; _objNewModel.Limit = _objModel.FLimit; _objNewModel.Tenor = _objModel.FTenor;
                            _clsCust.CustomerReviewDetail_Add(_objNewModel);
                        }
                    }
                    else
                    {

                        if (_objModel.CustomerRevID > 0)
                        {
                            if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA || _objModel.Status == CONT.UP || _objModel.Status == CONT.UM)
                            {
                                _objModel.Status = CONT.UC;
                                _clsCust.CustomerReviewDetailHistory_Add(_objModel);
                            }
                            else
                            {
                                _objModel.Status = CONT.NC;
                                _clsCust.CustomerReviewDetail_Update(_objModel);
                            }
                        }
                        else
                        {
                            _objModel.Status = CONT.NC;
                            _clsCust.CustomerReviewDetail_Add(_objModel);
                        }
                    }

                    CustomerDocModel _objDocModel = new CustomerDocModel();
                    _objDocModel.CustomerID = _objModel.CustomerID;
                    _objDocModel.Status = _objModel.Status;
                    _objDocModel.ProcessRemark = _objModel.Feedback;
                    _clsCust.CustomerMaster_UpdateAllStatus(_objDocModel);
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    return RedirectToAction("CMNDashboard", "Home");
                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerMasterModel _objCustModel1 = new CustomerMasterModel();
                    _objCustModel1.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel1).FirstOrDefault();
                }
                FillCurrencyCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult CreditReviewApprove(int CustomerID = 0, int CustomerTypeID = 0, string ProgramType = "", string Admin = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }

                CustomerReviewModel _objModel = new CustomerReviewModel();
                try
                {
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.ProgramType = ProgramType;
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();

                    _objModel.CustomerID = CustomerID;
                    if (CustomerTypeID == CONT.BothObligorAndBuyerTypeID)
                    {
                        _objModel.ProgramType = CONT.RType;
                    }
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    if (CheckCount != null)
                    {
                        _objModel = CheckCount;
                    }

                    if (CustomerTypeID == CONT.BothObligorAndBuyerTypeID)
                    {
                        CustomerReviewModel _objNewModel = new CustomerReviewModel();
                        _objNewModel.ProgramType = CONT.FType; _objNewModel.CustomerID = CustomerID;
                        var FCheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objNewModel).FirstOrDefault();
                        if (FCheckCount != null)
                        {
                            _objModel.FCustomerRevID = FCheckCount.CustomerRevID; _objModel.FCurrencyID = FCheckCount.CurrencyID; _objModel.FCurrencyCode = FCheckCount.CurrencyCode; _objModel.FLimit = FCheckCount.Limit; _objModel.FTenor = FCheckCount.Tenor;
                        }
                    }
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.ProgramType = ProgramType;
                    _objModel.Status = currentStatus;
                    _objModel.Admin = Admin;
                    return View(_objModel);
                }

                catch (Exception ex)
                {
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult CreditReviewApprove(CustomerReviewModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {

                try
                {
                    CustomerDocModel _objDocModel = new CustomerDocModel();

                    if (Request["Return"] != null)
                    {
                        if (_objModel.Status == CONT.UC) { _objModel.Status = CONT.UM; }
                        else { _objModel.Status = CONT.NK; }
                    }
                    else if (Request["Approve"] != null)
                    {
                        if (_objModel.Status == CONT.UC) { _objModel.Status = CONT.UA; }
                        else { _objModel.Status = CONT.NV; }
                    }
                    else if (Request["Reject"] != null)
                    {
                        if (_objModel.Status == CONT.UC) { _objModel.Status = CONT.UE; }
                        else { _objModel.Status = CONT.NE; }
                    }
                    else if (Request["Next"] != null)
                    {
                        return RedirectToAction("AccountActivation", "CustomerReg", new { CustomerID = _objModel.CustomerID, currentStatus = _objModel.Status, CustomerTypeID = _objModel.CustomerTypeID, Admin = _objModel.Admin });
                    }

                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    _objDocModel.CustomerID = _objModel.CustomerID;
                    _objDocModel.Status = _objModel.Status;
                    _objDocModel.ProcessRemark = _objModel.ProcessRemark;
                    _clsCust.CustomerMaster_UpdateAllStatus(_objDocModel);
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    return RedirectToAction("CMNDashboard", "Home");
                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel).FirstOrDefault();
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult AccountActivation(int CustomerID = 0, int CustomerTypeID = 0, string Admin = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                bool Rights = _RightsNoaccessPage();
                if (Rights == false) { return RedirectToAction("NoaccessPage", "MasterPage"); }

                CustomerVerifyModel _objModel = new CustomerVerifyModel();
                try
                {
                    CustomerMasterModel _objCustModel1 = new CustomerMasterModel();
                    _objCustModel1.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel1).FirstOrDefault();

                    _objModel.CustomerID = CustomerID;
                    _objModel.CustomerTypeID = CustomerTypeID;
                    var CheckCount = _clsCust.CustomerVerifyDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    if (CheckCount != null)
                    {
                        _objModel = CheckCount;
                    }
                    CustomerReviewModel _objModelReview = new CustomerReviewModel();
                    _objModelReview.CustomerID = CustomerID;
                    var GetProcessRemarks = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objModelReview).FirstOrDefault();
                    if (GetProcessRemarks != null)
                        _objModel.ProcessRemark = GetProcessRemarks.ProcessRemark;
                    else
                        _objModel.ProcessRemark = "";


                    if (CustomerTypeID == CONT.FunderCustomerTypeID)
                    {
                        CustomerMasterModel _objCustModel = new CustomerMasterModel();
                        _objCustModel.CustomerID = _objModel.CustomerID;
                        var GetRemarksData = _clsCust.CustomerMaster_ListAll(_objCustModel).FirstOrDefault();
                        if (GetRemarksData != null)
                            _objModel.ProcessRemark = GetRemarksData.ProcessRemark;
                        else
                            _objModel.ProcessRemark = "";
                    }



                    CustomerShareHolderModel _objShareModel = new CustomerShareHolderModel();
                    _objShareModel.CustomerID = CustomerID;
                    _objShareModel.CustType = "A";
                    //ViewBag.CustomerShareHoldIDs_List = new SelectList(_clsCust.CustomerShareHoldDetail_ListAll(_objShareModel).OrderBy(x => x.FirstName).ToList(), "CustomerShareHoldID", "FirstName", _objModel.CustomerShareHoldIDs);
                    ViewBag.CustomerShareHoldList = _clsCust.CustomerShareHoldDetail_ListAll(_objShareModel).OrderBy(x => x.FirstName).ToList();
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.Status = currentStatus;
                    _objModel.Admin = Admin;
                    _objModel._currentStatus = currentStatus;
                    FillDocCombo(CustomerTypeID); FillUserCombo(); FillProfitTypeCombo();
                    return View(_objModel);
                }

                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult AccountActivation(CustomerVerifyModel _objModel, HttpPostedFileBase Attach, FormCollection frm)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (Request["Next"] != null)
                {
                    return RedirectToAction("Documents", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                }
                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                try
                {
                    string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                    if (Attach != null)
                    {
                        if (Attach.FileName != null)
                        {
                            _objModel.Attach = newfilenm + Attach.FileName.ToString();
                            Attach.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.Attach));
                        }
                    }
                    if (_objModel.Complete == "Yes")
                        _objModel.IsComplete = true;
                    if (Request["cb2"] == "cb2")
                    {
                        _objModel.IsVerified = true;
                        //    if (Request["rdoJ"] != null) _objModel.AuthSignType = "J"; else _objModel.AuthSignType = "S";
                    }
                    if (Request["cb3"] == "cb3")
                    {
                        _objModel.IsInsurance = true;
                    }

                    _objModel.ExtDateType = Convert.ToString(Request.Form["Extensionradio"]);

                    var ShareHoldIDsresult = string.Empty;
                    if (_objModel.CustomerShareHoldIDs_List != null)
                    {
                        ShareHoldIDsresult = string.Join(",", _objModel.CustomerShareHoldIDs_List);
                    }
                    _objModel.CustomerShareHoldIDs = Convert.ToString(ShareHoldIDsresult);

                    var DocumentIDssresult = string.Empty;
                    if (_objModel.DocumentIDs_List != null)
                    {
                        DocumentIDssresult = string.Join(",", _objModel.DocumentIDs_List);
                    }
                    _objModel.DocumentIDs = Convert.ToString(DocumentIDssresult);

                    string _status = "";
                    if (Request["onhold"] != null)
                    {
                        if (_objModel.Status == CONT.UV || _objModel.Status == CONT.UO || _objModel.Status == CONT.UP) { _status = CONT.UO; }
                        else { _status = CONT.NO; }
                    }
                    else if (Request["Activate"] != null)
                    {
                        if (_objModel.Status == CONT.UV || _objModel.Status == CONT.UO || _objModel.Status == CONT.UP) { _status = CONT.UA; }
                        else { _status = CONT.NA; }
                    }
                    else if (Request["Reject"] != null)
                    {
                        if (_objModel.Status == CONT.UV || _objModel.Status == CONT.UO || _objModel.Status == CONT.UP) { _status = CONT.UJ; }
                        else { _status = CONT.NJ; }
                    }
                    else if (Request["Return"] != null)
                    {
                        _status = CONT.ND;
                    }
                    if (_objModel.CustomerVerID > 0)
                    {
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA || _objModel.Status == CONT.UV || _objModel.Status == CONT.UP)
                        {
                            _objModel.Status = _status;
                            _clsCust.CustomerVerifyDetailHistory_Add(_objModel);
                        }
                        else
                        {
                            _objModel.Status = _status;
                            _clsCust.CustomerVerifyDetail_Update(_objModel);
                        }
                    }
                    else
                    {
                        _objModel.Status = _status;
                        _clsCust.CustomerVerifyDetail_Add(_objModel);
                    }

                    #region Share Holder Sign
                    CustomerShareHolderModel _objShareModel = new CustomerShareHolderModel();
                    _objShareModel.CustomerID = _objModel.CustomerID;
                    _objShareModel.CustType = "A";
                    var Data = _clsCust.CustomerShareHoldDetail_ListAll(_objShareModel).OrderBy(x => x.FirstName).ToList();
                    ViewBag.CustomerShareHoldList = Data;

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int CustomerShareHoldID = Data[i].CustomerShareHoldID;

                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("AuthSignType_" + CustomerShareHoldID + ""))
                            {
                                _objShareModel.CustomerShareHoldID = CustomerShareHoldID;
                                _objShareModel.IsSelectSign = true;
                                _objShareModel.AuthSignType = Convert.ToString(frm["AuthSignType_" + CustomerShareHoldID + ""]);
                                _clsCust.CustomerShareHoldDetail_UpdateAuthSignType(_objShareModel);
                            }
                        }
                    }
                    #endregion

                    CustomerDocModel _objDocModel = new CustomerDocModel();
                    _objDocModel.CustomerID = _objModel.CustomerID;
                    _objDocModel.Status = _status;
                    _objDocModel.ProcessRemark = _objModel.Feedback;
                    _clsCust.CustomerMaster_UpdateAllStatus(_objDocModel);

                    _clsCust.Tras.Commit(); _clsCust.Conn.Close();

                    if (_status == CONT.NA || _status == CONT.UA)
                    {
                        var countAuth = _clsCust.UserMaster_AuthorizedListAll(_objModel).ToList();
                        foreach (var item in countAuth)
                        {
                            if (item.IsLoginMailSend == 0)
                                sendNotificationAuthrize(item.UserID, CONT.SupplierEmail_2st);
                        }
                    }
                    return RedirectToAction("CMNDashboard", "Home");
                }

                catch (Exception ex)
                {
                    if (_clsCust.Conn.State == ConnectionState.Open)
                    { _clsCust.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerMasterModel _objCustModel1 = new CustomerMasterModel();
                    _objCustModel1.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsCust.UserRole_GetDetail(_objCustModel1).FirstOrDefault();
                    _objModel.Status = _objModel._currentStatus;
                }

                FillDocCombo(_objModel.CustomerTypeID); FillUserCombo(); FillProfitTypeCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult GridMultiProgram(int CustomerID = 0)
        {
            CustomerVerifyModel _objModel = new CustomerVerifyModel();
            _objModel.CustomerID = CustomerID;
            var Data = _clsCust.CustomerVerifyDetail_GenerateProgram(_objModel);
            return PartialView(Data);
        }

        [HttpGet]
        public JsonResult GetProgramCode(int CustomerID)
        {
            CustomerMasterModel _objCustomer = new CustomerMasterModel();
            _objCustomer.CustomerID = CustomerID;
            var result = _clsCust.CustomerVerifyDetail_GenerateProgram(_objCustomer).ToList();
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public PartialViewResult RemarksPopup(int CustomerID = 0)
        {
            CustomerMasterModel _objCustomer = new CustomerMasterModel();
            _objCustomer.CustomerID = CustomerID;
            var result = _clsCust.CustomerMasterHistory_Remarks(_objCustomer).ToList();
            return PartialView(result);
        }
        #endregion

        #region function
        public void FillSalesInvoiceTermsCombo()
        {
            ViewBag.DDLSalesInvoiceTerms = new List<SelectListItem> { new SelectListItem() { Text = "Less than 30 days", Value = "Less than 30 days" },
                                                                  new SelectListItem() { Text = "30-60 days", Value = "30-60 days" },
                                                                  new SelectListItem() { Text = "60-90 days", Value = "60-90 days" },
                                                                  new SelectListItem() { Text = "90-120 days", Value = "90-120 days" },
                                                                  new SelectListItem() { Text = "Above 120 days", Value = "Above 120 days" },
                                                                  new SelectListItem() { Text = "Payment in Advance (PIA)", Value = "Payment in Advance (PIA)" },
                                                                  new SelectListItem() { Text = "Letters of Credit (LC)", Value = "Letters of Credit (LC)" },
                                                                  new SelectListItem() { Text = "Documentary Credit (COD)", Value = "Documentary Credit (COD)" },
                                                                  new SelectListItem() { Text = "Open Account", Value = "Open Account" },
                                                                  new SelectListItem() { Text = "Other", Value = "Other" } };
        }
        public void FillProgramTypeCombo()
        {
            ViewBag.DDLProgramTypeID = new List<SelectListItem> { new SelectListItem() { Text = "Receivables Finance", Value = "F" },
                                                                  new SelectListItem() { Text = "Payables Finance", Value = "R" } };
            //ViewBag.DDLProgramTypeID = new List<SelectListItem> { new SelectListItem() { Text = "Factoring", Value = "F" },
            //                                                      new SelectListItem() { Text = "Payables Finance", Value = "R" },
            //                                                      new SelectListItem() { Text = "Factoring & Payables Finance", Value = "B" } };
        }
        public void FillCountryCombo()
        {
            var DDLCountryIDData = _clsContr.CountryMaster_ListAll(0, "", 1, "", false, "", -1).OrderBy(x => x.CountryName).ToList();
            if (DDLCountryIDData != null && DDLCountryIDData.Count > 0)
                ViewBag.DDLCountryID = DDLCountryIDData;
            else
                ViewBag.DDLCountryID = new List<SelectListItem> { };
        }
        public void FillCompanyTypeCombo()
        {
            var DDLCompanyTypeData = _clsCompType.CompanyTypeMaster_ListAll(0, "", 1, "", false, "").OrderBy(x => x.CompanyTypeName).ToList();
            if (DDLCompanyTypeData != null && DDLCompanyTypeData.Count > 0)
                ViewBag.DDLCompanyTypeID = DDLCompanyTypeData;
            else
                ViewBag.DDLCompanyTypeID = new List<SelectListItem> { };
        }
        public void FillIndustryCombo()
        {
            var DDLIndustryData = _clsInds.IndustryMaster_ListAll(0, 0, "", 1, false, "").OrderBy(x => x.IndustryName).ToList();
            if (DDLIndustryData != null && DDLIndustryData.Count > 0)
                ViewBag.DDLIndustryID = DDLIndustryData;
            else
                ViewBag.DDLIndustryID = new List<SelectListItem> { };
        }
        public void FillDesignationCombo()
        {
            var DDLDesignationData = _clsDesig.DesignationMaster_ListAll(0, "", 1, "", false, "").OrderBy(x => x.DesignationName).ToList();
            if (DDLDesignationData != null && DDLDesignationData.Count > 0)
                ViewBag.DDLDesignationID = DDLDesignationData;
            else
                ViewBag.DDLDesignationID = new List<SelectListItem> { };
        }
        public void FillCurrencyCombo()
        {
            var DDLCurrencyData = _clsCurry.CurrencyMaster_ListAll(0, "", "", 1, "", false, "").OrderBy(x => x.CurrencyCode).ToList();
            if (DDLCurrencyData != null && DDLCurrencyData.Count > 0)
                ViewBag.DDLCurrencyID = DDLCurrencyData;
            else
                ViewBag.DDLCurrencyID = new List<SelectListItem> { };
        }
        public void FillBankCombo()
        {
            var DDLBankData = _clsBank.BankMaster_ListAll(0, "", 0, 0, 0, 1, "", false, "").OrderBy(x => x.BankName).ToList();
            if (DDLBankData != null && DDLBankData.Count > 0)
                ViewBag.DDLBankID = DDLBankData;
            else
                ViewBag.DDLBankID = new List<SelectListItem> { };
        }
        public void FillDocCombo(int CustomerTypeID = 0)
        {
            CustomerTypeDocumentDetails_ListAll_Result _objDocModel = new CustomerTypeDocumentDetails_ListAll_Result();
            _objDocModel.CustomerTypeID = CustomerTypeID;
            _objDocModel.IsActive = true;
            _objDocModel.Status = CONT.ddstatus;
            var DDLDocData = _clsCust.CustomerTypeDocumentDetails_ListAll(_objDocModel).OrderBy(x => x.DocumentName).ToList();
            if (DDLDocData != null && DDLDocData.Count > 0)
                ViewBag.DDLDocID = DDLDocData;
            else
                ViewBag.DDLDocID = new List<SelectListItem> { };
        }
        public void FillUserCombo(int CustomerTypeID = 0)
        {
            CustomerMasterModel _objModel = new CustomerMasterModel();
            _objModel.CustomerTypeID = 1;
            var DDLUserData = _clsCust.UserMaster_CustomerTypeListAll(_objModel).OrderBy(x => x.ContactFullName).ToList();
            if (DDLUserData != null && DDLUserData.Count > 0)
                ViewBag.DDLUserID = DDLUserData;
            else
                ViewBag.DDLUserID = new List<SelectListItem> { };
        }
        protected void sendNotification(int Tid, string tempName = "")
        {
            string sub = "";
            string toMail = "";
            DataTable DT = new DataTable();
            CustomerMasterModel _objModel = new CustomerMasterModel();
            _objModel.CustomerID = Tid;
            DT = _clsCust.CustomerMaster_ListAllTable(_objModel);
            if (tempName == CONT.Email_3rd)
            {
                toMail = Convert.ToString(WebConfigurationManager.AppSettings["AdminSupportEmail"]);
            }
            else
            {
                toMail = Convert.ToString(DT.Rows[0]["EmailID"]);
            }
            fn.GetMasterData(tempName, DT, toMail, sub);
        }
        protected void sendNotificationAuthrize(int Tid, string tempName = "")
        {
            string sub = "";
            string toMail = "";
            int UserID = 0;
            DataTable DT = new DataTable();
            UserMaster_ListAll_Result _objModel = new UserMaster_ListAll_Result();
            _objModel.UserID = Tid;
            _objModel.IsActive = true;
            DT = _clsCust.UserMaster_ListAllTable(_objModel);
            toMail = Convert.ToString(DT.Rows[0]["EmailID"]);
            UserID = Convert.ToInt32(DT.Rows[0]["UserID"]);
            fn.GetMasterData(tempName, DT, toMail, sub, "", UserID);
        }
        public void FillCustomerCombo()
        {
            //ClsCustomerMaster _CustomerMaster = new ClsCustomerMaster();
            //var DDLCustomerIDData = _CustomerMaster.UserMaster_CustomerBindDetails(0, 0, CONT.ddstatus).OrderBy(x => x.CompanyName).ToList();
            //if (DDLCustomerIDData != null && DDLCustomerIDData.Count > 0)
            //    ViewBag.DDLCustomerID = DDLCustomerIDData;
            //else
            //    ViewBag.DDLCustomerID = new List<SelectListItem> { };
        }
        public void FillRoleCombo(int CustomerTypeID = 0)
        {
            int intCustType = 0;
            ClsRoleMaster dbR = new ClsRoleMaster();
            UserMaster_ListAll_Result obj = new UserMaster_ListAll_Result();
            if (CustomerTypeID > 0)
                intCustType = CustomerTypeID;
            else
                intCustType = Convert.ToInt32(Session["onlineCustomerTypeID"]);
            var DDLRoleData = dbR.RoleMaster_ListAll(0, "", 1, CONT.ddstatus, false, "", intCustType).OrderBy(x => x.RoleName).ToList();
            if (DDLRoleData != null && DDLRoleData.Count > 0)
                ViewBag.DDLRoleID = DDLRoleData;
            else
                ViewBag.DDLRoleID = new List<SelectListItem> { };
        }
        public void FillDocumentCombo()
        {
            ClsDocumentMaster dbDM = new ClsDocumentMaster();
            var DDLDocumentData = dbDM.DocumentMaster_ListAll(0, "", 1, 1, CONT.ddstatus, false, "").OrderBy(x => x.DocumentName).ToList();
            if (DDLDocumentData != null && DDLDocumentData.Count > 0)
                ViewBag.DDLDocumentID = DDLDocumentData;
            else
                ViewBag.DDLDocumentID = new List<SelectListItem> { };
        }
        public bool _RightsNoaccessPage()
        {
            string MenuId = Convert.ToString(Session["MenuID"]);
            int ParentMenuID = 0;
            int.TryParse(MenuId, out ParentMenuID);
            var UserRight = fn.CheckUserRight("", "", ParentMenuID);
            if (UserRight != null)
            {
                return true;
            }
            else
            { return true; }
        }
        public void FillProfitTypeCombo()
        {
            ViewBag.DDLProfitTypeID = new List<SelectListItem> {
            new SelectListItem() { Text = "Annualized (p.a.) - 360 days", Value = "A" },
            new SelectListItem() { Text = "Annualized (p.a.) - 365 days", Value = "B" },
            new SelectListItem() { Text = "Fixed rate", Value = "F" } };
        }

        [HttpPost]
        public JsonResult GetCountryData()
        {
            var result = _clsContr.CountryMaster_ListAll(0, "", 1, "", false, "", -1).OrderBy(x => x.CountryName).ToList();
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
    }
}