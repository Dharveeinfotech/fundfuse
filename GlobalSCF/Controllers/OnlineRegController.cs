
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;
using TMP.DAL;
using System.Data;
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;
using System.Web.Configuration;
using System.Net;
using System.IO;
using System.IO.Compression;
using session = TMP.Infrastructure.Web.SessionManager;

namespace TMP.Controllers
{
    public class OnlineRegController : Controller
    {
        #region Object

        ClsCustomerMaster _clsCust = new ClsCustomerMaster();
        ClsCountryMaster _clsContr = new ClsCountryMaster();
        ClsCompanyTypeMaster _clsCompType = new ClsCompanyTypeMaster();
        ClsIndustryMaster _clsInds = new ClsIndustryMaster();
        ClsDesignationMaster _clsDesig = new ClsDesignationMaster();
        ClsUserMaster _clsUser = new ClsUserMaster();
        Function fn = new Function();

        #endregion

        #region Client Common Action

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string id = "")
        {
            if (Request["Register"] == "Register")
            {
                return RedirectToAction("Welcome", "OnlineReg");
            }
            else if (Request["Login"] == "Login")
            {
                return RedirectToAction("Login", "mUserMasters");
            }

            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Welcome(string id = "")
        {
            if (Request["supplier"] == "supplier")
            {
                return RedirectToAction("UserAccount", "OnlineReg", new { CustomerTypeID = CONT.SupplierCustomerTypeID, ProgramType = CONT.FType });
            }
            //else if (Request["obligorF"] == "obligorF")
            //{
            //    return RedirectToAction("UserAccount", "OnlineReg", new { CustomerTypeID = CONT.ObligorCustomerTypeID, ProgramType = CONT.FType });
            //}
            //else if (Request["obligorR"] == "obligorR")
            //{
            //    return RedirectToAction("UserAccount", "OnlineReg", new { CustomerTypeID = CONT.ObligorCustomerTypeID, ProgramType = CONT.RType });
            //}
            else if (Request["obligor_buyer"] == "obligor_buyer")
            {
                return RedirectToAction("UserAccount", "OnlineReg", new { CustomerTypeID = CONT.BothObligorAndBuyerTypeID });
            }
            else if (Request["funder"] == "funder")
            {
                return RedirectToAction("UserAccount", "OnlineReg", new { CustomerTypeID = CONT.FunderCustomerTypeID });
            }
            return View();
        }
        public ActionResult UserAccount(int CustomerTempID = 0, int CustomerTypeID = 0, string ProgramType = "")
        {
            var ConfirmDate = _clsCust.ServerDateTime().FirstOrDefault();
            CustomerTempModel _objModel = new CustomerTempModel();
            _objModel.CustomerTempID = CustomerTempID;
            _objModel.CustomerTypeID = CustomerTypeID;
            _objModel.ProgramType = ProgramType;
            if (CustomerTempID > 0)
            {
                _objModel = _clsCust.CustomerMasterTemp_ListAll(_objModel).FirstOrDefault();
                _objModel.CustomerTypeID = _objModel.CustomerTypeID;
                _objModel.ProgramType = _objModel.ProgramType;
                if (_objModel.OtherIndustry == "") { _objModel.OtherSubIndustry_Sub = _objModel.OtherSubIndustry; }
            }
            FillCountryCombo();
            FillCompanyTypeCombo();
            FillIndustryCombo();
            FillDesignationCombo();
            FillProgramTypeCombo();
            FillGenderCombo();
            _objModel.ConfirmDate = ConfirmDate.UpdateDate;
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult UserAccount(CustomerTempModel _objModel)
        {
            try
            {
                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                if (_objModel.OtherIndustry == null) { _objModel.OtherSubIndustry = _objModel.OtherSubIndustry_Sub; }
                if (_objModel.CustomerTempID > 0)
                {
                    _clsCust.CustomerMasterTemp_Update(_objModel);
                }
                else
                {
                    _objModel.CustomerTempID = _clsCust.CustomerMasterTemp_Add(_objModel);
                }
                _clsCust.Tras.Commit();
                _clsCust.Conn.Close();
                string EmailTemplate = "";
                string ThankYou = "";
                if (_objModel.CustomerTypeID == CONT.SupplierCustomerTypeID) { EmailTemplate = CONT.SupplierEmail_1st; ThankYou = CONT.SupplierThanku_1st; }
                else if (_objModel.CustomerTypeID == CONT.ObligorCustomerTypeID) { EmailTemplate = CONT.ObligorEmail_1st; ThankYou = CONT.ObligorThanku_1st; }
                else if (_objModel.CustomerTypeID == CONT.FunderCustomerTypeID) { EmailTemplate = CONT.FunderEmail_1st; ThankYou = CONT.FunderThanku_1st; }
                else if (_objModel.CustomerTypeID == CONT.BuyerCustomerTypeID) { EmailTemplate = CONT.BuyerEmail_1st; ThankYou = CONT.BuyerThanku_1st; }
                else if (_objModel.CustomerTypeID == CONT.BothObligorAndBuyerTypeID) { EmailTemplate = CONT.ObligorAndBuyerEmail_1st; ThankYou = CONT.ObligorAndBuyerThanku_1st; }
                sendNotification(_objModel.CustomerTempID, EmailTemplate);
                return RedirectToAction("ThankYou", "OnlineReg", new { Tid = _objModel.CustomerTempID, temp_name = ThankYou });
            }

            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            FillCountryCombo();
            FillCompanyTypeCombo();
            FillIndustryCombo();
            FillDesignationCombo();
            FillProgramTypeCombo();
            FillGenderCombo();
            return View(_objModel);
        }
        public ActionResult ThankYou(int Tid = 0, string temp_name = "", int CustomerID = 0)
        {

            //temp_name = CONT.BuyerThanku_2st;
            if (CustomerID > 0)
            {
                ViewBag.ThankYouMsg = GetThankYouMsgText(CustomerID, temp_name);
            }
            else
            {
                ViewBag.ThankYouMsg = GetThankYouMsg(Tid, temp_name);
            }
            return View();
        }
        public ActionResult Connecting()
        {
            CustomerTempModel _objModel = new CustomerTempModel();
            try
            {
                session.ClearSession();

                string url = Convert.ToString(HttpContext.Request.Url);
                _objModel.ActivationLink = url;

                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();

                _objModel.CustomerTempID = _clsCust.CustomerMasterTemp_GetActivateLink(_objModel);
                _clsCust.Tras.Commit();
                _clsCust.Conn.Close();

            }

            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult Connecting(CustomerTempModel _objModel)
        {
            try
            {
                if (_objModel.CustomerTempID > 0)
                {
                    if (Convert.ToString(WebConfigurationManager.AppSettings["SendSMS"]).ToLower() == "true")
                    {
                        sendSMS(_objModel.CustomerTempID, CONT.SMS_1st);
                    }
                    else
                    {
                        sendNotification(_objModel.CustomerTempID, CONT.Email_SMS_1st);
                    }
                }
                return RedirectToAction("ActivationCode", "OnlineReg", new { Tid = _objModel.CustomerTempID });
            }

            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return View(_objModel);
        }

        public ActionResult ActivationCode(int Tid = 0, string Resend = "")
        {
            CustomerTempModel _objModel = new CustomerTempModel();
            _objModel.CustomerTempID = Tid;

            if (_objModel.CustomerTempID > 0)
            {
                var data = _clsCust.CustomerMasterTemp_ListAll(_objModel).FirstOrDefault();
                _objModel.CustomerTypeID = data.CustomerTypeID;
                _objModel.ProgramType = data.ProgramType;
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult ActivationCode(CustomerTempModel _objModel)
        {
            try
            {
                if (_objModel.CustomerTempID > 0)
                {

                    if (Request["ACTIVATE"] != null)
                    {

                        if (_objModel.CustomerTempID > 0)
                        {
                            var ServerDate = _clsCust.ServerDateTime().FirstOrDefault();
                            _clsCust.Conn = ClsAppDatabase.GetCon();
                            if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                            _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                            _objModel.ServerDate = ServerDate.UpdateDate;
                            _clsCust.CustomerMasterTemp_ActivateCode(_objModel);
                            _clsCust.Tras.Commit();
                            _clsCust.Conn.Close();
                        }
                        string EmailTemplate = "";
                        string ThankYou = "";
                        if (_objModel.CustomerTypeID == CONT.SupplierCustomerTypeID) { EmailTemplate = CONT.SupplierEmail_2st; ThankYou = CONT.SupplierThanku_2st; }
                        else if (_objModel.CustomerTypeID == CONT.ObligorCustomerTypeID) { EmailTemplate = CONT.ObligorEmail_2st; ThankYou = CONT.ObligorThanku_2st; }
                        else if (_objModel.CustomerTypeID == CONT.FunderCustomerTypeID) { EmailTemplate = CONT.FunderEmail_2st; ThankYou = CONT.FunderThanku_2st; }
                        else if (_objModel.CustomerTypeID == CONT.BuyerCustomerTypeID) { EmailTemplate = CONT.BuyerEmail_2st; ThankYou = CONT.BuyerThanku_2st; }
                        else if (_objModel.CustomerTypeID == CONT.BothObligorAndBuyerTypeID) { EmailTemplate = CONT.ObligorAndBuyerEmail_2st; ThankYou = CONT.ObligorAndBuyerThanku_2st; }
                        sendNotification(_objModel.CustomerTempID, EmailTemplate);
                        return RedirectToAction("ThankYou", "OnlineReg", new { Tid = _objModel.CustomerTempID, temp_name = ThankYou });
                    }
                    else if (Request["RESEND"] != null)
                    {
                        if (_objModel.CustomerTempID > 0)
                        {
                            _clsCust.Conn = ClsAppDatabase.GetCon();
                            if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                            _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                            _clsCust.CustomerMasterTemp_UpdateActivationCode(_objModel);
                            _clsCust.Tras.Commit();
                            _clsCust.Conn.Close();
                        }

                        if (Convert.ToString(WebConfigurationManager.AppSettings["SendSMS"]).ToLower() == "true")
                        {
                            sendSMS(_objModel.CustomerTempID, CONT.SMS_1st);
                        }
                        else
                        {
                            sendNotification(_objModel.CustomerTempID, CONT.Email_SMS_1st);
                        }
                        return RedirectToAction("ActivationCode", "OnlineReg", new { Tid = _objModel.CustomerTempID, Resend = "Yes" });
                    }
                    else if (Request["EDIT"] != null)
                    {
                        var CheckCount = _clsCust.CustomerMasterTemp_ListAll(_objModel).FirstOrDefault();
                        if (CheckCount != null)
                        {
                            return RedirectToAction("UserAccount", "OnlineReg", new { CustomerTempID = _objModel.CustomerTempID });
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }

            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return View(_objModel);
        }
        public ActionResult RegistrationAgree(int CustomerID = 0, string currentStatus = "", string AuthorizedUser = "", string ProgramType = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ViewBag.RegistrationAgree = GetThankYouMsg(0, CONT.RegistrationAgree);
                mCustomerMasterModel _objModel = new mCustomerMasterModel();
                _objModel.AuthorizedUser = AuthorizedUser;
                _objModel.CustomerID = CustomerID;
                _objModel.Status = currentStatus;
                _objModel.ProgramType = ProgramType;
                Session["hideMsg"] = true;
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }

        }
        [HttpPost]
        public ActionResult RegistrationAgree(mCustomerMasterModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (Request["Agree"] != null)
                    {
                        _clsCust.Conn = ClsAppDatabase.GetCon();
                        if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                        _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                        if (_objModel.CustomerID > 0)
                        {
                            _clsCust.UserMaster_UpdateAgree(_objModel);
                        }
                        _clsCust.Tras.Commit();
                        _clsCust.Conn.Close();
                        int intCustTypeID = Convert.ToInt32(Session["onlineCustomerTypeID"]);
                        Session["IsAgree1st"] = true;
                        if (_objModel.AuthorizedUser == "Y")
                        {
                            Session["hideMsg"] = false;
                            if (intCustTypeID == CONT.SupplierCustomerTypeID)
                            {
                                return RedirectToAction("SupplierAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = _objModel.Status });
                            }
                            if (intCustTypeID == CONT.ObligorCustomerTypeID || intCustTypeID == CONT.BuyerCustomerTypeID || intCustTypeID == CONT.BothObligorAndBuyerTypeID)
                            {
                                return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = _objModel.Status, ProgramType = _objModel.ProgramType });
                            }
                            if (intCustTypeID == CONT.FunderCustomerTypeID)
                            {
                                return RedirectToAction("FunderAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = _objModel.Status });
                            }
                        }
                        else
                        {
                            Session["hideMsg"] = false;
                            if (_objModel.Status == CONT.RQ)
                            {
                                return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerID = _objModel.CustomerID });
                            }
                            else
                            {
                                if (Convert.ToBoolean(Session["IsMenuPanelHide"]))
                                {
                                    return RedirectToAction("BlankDashboard", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("CMNDashboard", "Home");
                                }
                               
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "mUserMasters");
                    }

                }

                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
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
        public ActionResult ObligorAgreement(int UserID = 0, string currentStatus = "", string ProgramType = "", bool IsIndiCmplt = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {

                UserSignDetailModel _objModel = new UserSignDetailModel();
                CustomerReviewModel _objRevModel = new CustomerReviewModel();
                _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                if (ProgramType == CONT.BothProgramType)
                    _objRevModel.ProgramType = CONT.RType;
                var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                if (CheckCount != null)
                {
                    ViewBag.Detail = CheckCount;
                }
                _objModel.UserID = UserID;
                _objModel.Status = currentStatus;

                _objModel.ProgramType = ProgramType;
                _objModel.CustomerID = _objRevModel.CustomerID;
                _objModel.IsIndiCmplt = IsIndiCmplt;
                ViewBag.ProgramType = ProgramType;
                ViewBag.intCustTypeID = Convert.ToInt32(Session["onlineCustomerTypeID"]);
                UserSignDetailModel _objUserSignDetails = new UserSignDetailModel();
                _objUserSignDetails.Status = CONT.NA; _objUserSignDetails.UserID = UserID;

                #region Get data from sign details
                _objUserSignDetails.DocumentID = CONT.Indicative_Term_Sheet_DocumentID;
                var IndicativeData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (IndicativeData != null)
                {
                    _objModel.Feedback1 = IndicativeData.Feedback; ViewBag.btncls_1 = false;
                }
                else { ViewBag.btncls_1 = true; }

                _objUserSignDetails.DocumentID = CONT.Master_Murabaha_Agreement_DocumentID;
                var MurabahaData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (MurabahaData != null)
                {
                    _objModel.Feedback2 = MurabahaData.Feedback; ViewBag.btncls_2 = false;
                }
                else { ViewBag.btncls_2 = true; }

                _objUserSignDetails.DocumentID = CONT.Agency_Agreement_DocumentID;
                var AgencyData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (AgencyData != null)
                {
                    _objModel.Feedback3 = AgencyData.Feedback; ViewBag.btncls_3 = false;
                }
                else { ViewBag.btncls_3 = true; }

                _objUserSignDetails.DocumentID = CONT.Factoring_Agreement_DocumentID;
                var FactAgrmntData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (FactAgrmntData != null)
                {
                    _objModel.Feedback4 = FactAgrmntData.Feedback; ViewBag.btncls_4 = false;
                }
                else { ViewBag.btncls_4 = true; }
                #endregion

                if (ViewBag.btncls_1 == false && ViewBag.btncls_2 == false && ViewBag.btncls_3 == false)
                {
                    var IsMenuPanelHide = _clsCust.UserSignDetail_MenuPanelHide(_objUserSignDetails).FirstOrDefault();
                    if (IsMenuPanelHide != null)
                    {
                        Session["IsMenuPanelHide"] = IsMenuPanelHide.IsMenuPanelHide;
                        ViewBag.IsMenuPanelHide = IsMenuPanelHide.IsMenuPanelHide;
                    }
                }

                if (ViewBag.btncls_1 == false && ViewBag.btncls_2 == false && ViewBag.btncls_3 == false && ViewBag.btncls_4 == false)
                {
                    var IsMenuPanelHide = _clsCust.UserSignDetail_MenuPanelHide(_objUserSignDetails).FirstOrDefault();
                    if (IsMenuPanelHide != null)
                    {
                        Session["IsMenuPanelHide"] = IsMenuPanelHide.IsMenuPanelHide;
                        ViewBag.IsMenuPanelHide = IsMenuPanelHide.IsMenuPanelHide;
                    }
                }

                if (ViewBag.btncls_4 == false)
                {
                    if (ProgramType == CONT.FType)
                    {
                        var IsMenuPanelHide = _clsCust.UserSignDetail_MenuPanelHide(_objUserSignDetails).FirstOrDefault();
                        if (IsMenuPanelHide != null)
                        {
                            Session["IsMenuPanelHide"] = IsMenuPanelHide.IsMenuPanelHide;
                            ViewBag.IsMenuPanelHide = IsMenuPanelHide.IsMenuPanelHide;
                            ViewBag.OnlyObligor = 1;
                        }
                    }
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult ObligorAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE1"] != null)
                        _objModel.Status = CONT.NA;

                    else if (Request["btn_RETURN1"] != null)
                        _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Indicative_Term_Sheet_DocumentID;
                        _objModel.Feedback = _objModel.Feedback1;
                        _clsCust.UserSignDetail_Add(_objModel);
                        _objModel.DocumentID = CONT.Master_Murabaha_Agreement_DocumentID;
                        _clsCust.UserSignDetail_Add(_objModel);
                        _objModel.DocumentID = CONT.Agency_Agreement_DocumentID;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["btn_APPROVE1"] != null)
                    {
                        UserSendEmail();
                        return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, ProgramType = _objModel.ProgramType, IsIndiCmplt = true });
                    }

                    else if (Request["btn_RETURN1"] != null)
                        return RedirectToAction("BlankDashboard", "Home");
                }
                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerReviewModel _objRevModel = new CustomerReviewModel();
                    _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                    if (CheckCount != null)
                        ViewBag.Detail = CheckCount;
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult MuradahaAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE2"] != null)
                        _objModel.Status = CONT.NA;

                    else if (Request["btn_RETURN2"] != null)
                        _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Master_Murabaha_Agreement_DocumentID;
                        _objModel.Feedback = _objModel.Feedback2;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["btn_APPROVE2"] != null)
                    {
                        var CheckData = _clsCust.UserSignDetail_VerifySendLOA(_objModel).FirstOrDefault();
                        if (CheckData != null)
                        {
                            if (CheckData.IsSendLOA == true)
                            {
                                if (_objModel.ProgramType == CONT.RType || _objModel.ProgramType == CONT.BothProgramType)
                                {
                                    CustomerSupplierModel _objSppModel = new CustomerSupplierModel();
                                    _objSppModel.CustomerID = _objModel.CustomerID;
                                    var countSupplier = _clsCust.CustomerSupplierDetail_ListAll(_objSppModel).ToList();
                                    foreach (var item in countSupplier)
                                    {
                                        sendNotification(item.CustomerSuppID, CONT.SupplierLink_Email_4th);
                                    }
                                }
                            }
                        }
                        UserSendEmail();
                        return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, ProgramType = _objModel.ProgramType, IsIndiCmplt = true });
                    }

                    else if (Request["btn_RETURN2"] != null)
                        return RedirectToAction("BlankDashboard", "Home");
                }
                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerReviewModel _objRevModel = new CustomerReviewModel();
                    _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                    if (CheckCount != null)
                        ViewBag.Detail = CheckCount;
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult AgencyAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE3"] != null)
                        _objModel.Status = CONT.NA;

                    else if (Request["btn_RETURN3"] != null)
                        _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Agency_Agreement_DocumentID;
                        _objModel.Feedback = _objModel.Feedback3;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["btn_APPROVE3"] != null)
                    {
                        var CheckData = _clsCust.UserSignDetail_VerifySendLOA(_objModel).FirstOrDefault();
                        if (CheckData != null)
                        {
                            if (CheckData.IsSendLOA == true)
                            {
                                if (_objModel.ProgramType == CONT.RType || _objModel.ProgramType == CONT.BothProgramType)
                                {
                                    CustomerSupplierModel _objSppModel = new CustomerSupplierModel();
                                    _objSppModel.CustomerID = _objModel.CustomerID;
                                    var countSupplier = _clsCust.CustomerSupplierDetail_ListAll(_objSppModel).ToList();
                                    foreach (var item in countSupplier)
                                    {
                                        sendNotification(item.CustomerSuppID, CONT.SupplierLink_Email_4th);
                                    }
                                }
                            }
                        }
                        UserSendEmail();

                        return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, ProgramType = _objModel.ProgramType, IsIndiCmplt = true });
                    }

                    else if (Request["btn_RETURN3"] != null)
                        return RedirectToAction("BlankDashboard", "Home");
                }
                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerReviewModel _objRevModel = new CustomerReviewModel();
                    _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                    if (CheckCount != null)
                        ViewBag.Detail = CheckCount;
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        [HttpPost]
        public ActionResult FactoringAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE4"] != null)
                        _objModel.Status = CONT.NA;

                    else if (Request["btn_RETURN4"] != null)
                        _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Factoring_Agreement_DocumentID;
                        _objModel.Feedback = _objModel.Feedback4;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();


                    if (Request["btn_APPROVE4"] != null)
                    {
                        var CheckData = _clsCust.UserSignDetail_VerifySendLOA(_objModel).FirstOrDefault();
                        if (CheckData != null)
                        {
                            if (CheckData.IsSendLOA == true)
                            {
                                if (_objModel.ProgramType == CONT.RType || _objModel.ProgramType == CONT.BothProgramType)
                                {
                                    CustomerSupplierModel _objSppModel = new CustomerSupplierModel();
                                    _objSppModel.CustomerID = _objModel.CustomerID;
                                    var countSupplier = _clsCust.CustomerSupplierDetail_ListAll(_objSppModel).ToList();
                                    foreach (var item in countSupplier)
                                    {
                                        sendNotification(item.CustomerSuppID, CONT.SupplierLink_Email_4th);
                                    }
                                }
                            }
                        }
                        UserSendEmail();

                        return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, ProgramType = _objModel.ProgramType, IsIndiCmplt = true });
                    }

                    //UserSendEmail();
                    //return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, ProgramType = _objModel.ProgramType, IsIndiCmplt = true });




                    //if (_objModel.ProgramType == CONT.BothProgramType)
                    //    return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, ProgramType = _objModel.ProgramType, IsIndiCmplt = true });
                    //else
                    //{
                    //    UserSignDetailModel _objUserSignDetails = new UserSignDetailModel();
                    //    _objUserSignDetails.Status = CONT.NA; _objUserSignDetails.UserID = _objModel.UserID;
                    //    _objUserSignDetails.DocumentID = CONT.Factoring_Agreement_DocumentID;
                    //    var FactAgrmntData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                    //    if (FactAgrmntData != null)
                    //    {
                    //        var IsMenuPanelHide = _clsCust.UserSignDetail_MenuPanelHide(_objUserSignDetails).FirstOrDefault();
                    //        if (IsMenuPanelHide != null)
                    //        {
                    //            Session["IsMenuPanelHide"] = IsMenuPanelHide.IsMenuPanelHide;
                    //            ViewBag.IsMenuPanelHide = IsMenuPanelHide.IsMenuPanelHide;
                    //            ViewBag.OnlyObligor = 1;
                    //        }
                    //    }
                    //    else
                    //        return RedirectToAction("CMNDashboard", "Home");
                    //}
                }
                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerReviewModel _objRevModel = new CustomerReviewModel();
                    _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                    if (CheckCount != null)
                        ViewBag.Detail = CheckCount;
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult SupplierAgreement(int UserID = 0, string currentStatus = "", bool IsIndiCmplt = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {

                UserSignDetailModel _objModel = new UserSignDetailModel();
                CustomerReviewModel _objRevModel = new CustomerReviewModel();
                _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                if (CheckCount != null)
                {
                    ViewBag.Detail = CheckCount;
                }
                _objModel.UserID = UserID;
                _objModel.Status = currentStatus;
                _objModel.IsIndiCmplt = IsIndiCmplt;
                _objModel.CustomerID = _objRevModel.CustomerID;


                UserSignDetailModel _objUserSignDetails = new UserSignDetailModel();
                _objUserSignDetails.Status = CONT.NA; _objUserSignDetails.UserID = UserID;

                #region Get data from sign details
                _objUserSignDetails.DocumentID = CONT.Indicative_Term_Sheet_DocumentID;
                var IndicativeData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (IndicativeData != null)
                {
                    _objModel.Feedback1 = IndicativeData.Feedback; ViewBag.btncls_1 = false; ViewBag.btncls_2 = false; ViewBag.btncls_3 = false;
                }

                _objUserSignDetails.DocumentID = CONT.Master_PurchaseAgreement_DocumentID;
                var MurabahaData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (MurabahaData != null)
                {
                    _objModel.Feedback2 = MurabahaData.Feedback; ViewBag.btncls_2 = false;
                }

                _objUserSignDetails.DocumentID = CONT.Agency_Agreement_DocumentID;
                var AgencyData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (AgencyData != null)
                {
                    _objModel.Feedback3 = AgencyData.Feedback; ViewBag.btncls_3 = false;
                }
                #endregion

                if (ViewBag.btncls_1 == false && ViewBag.btncls_2 == false && ViewBag.btncls_3 == false)
                {
                    var IsMenuPanelHide = _clsCust.UserSignDetail_MenuPanelHide(_objUserSignDetails).FirstOrDefault();
                    if (IsMenuPanelHide != null)
                    {
                        Session["IsMenuPanelHide"] = IsMenuPanelHide.IsMenuPanelHide;
                        ViewBag.IsMenuPanelHide = IsMenuPanelHide.IsMenuPanelHide;
                    }
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult SupplierAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE1"] != null)
                        _objModel.Status = CONT.NA;

                    //else if (Request["btn_RETURN1"] != null)
                    //    _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Indicative_Term_Sheet_DocumentID; 
                        _objModel.Feedback = _objModel.Feedback1;
                        _clsCust.UserSignDetail_Add(_objModel);
                        _objModel.DocumentID = CONT.Agency_Agreement_DocumentID;
                        _clsCust.UserSignDetail_Add(_objModel);
                        _objModel.DocumentID = CONT.Master_PurchaseAgreement_DocumentID;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["btn_APPROVE1"] != null)
                    {
                        UserSendEmail();
                        return RedirectToAction("SupplierAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, IsIndiCmplt = true });
                    }

                    else if (Request["btn_RETURN1"] != null)
                        return RedirectToAction("BlankDashboard", "Home");
                }
                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerReviewModel _objRevModel = new CustomerReviewModel();
                    _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                    if (CheckCount != null)
                    {
                        ViewBag.Detail = CheckCount;
                    }
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        [HttpPost]
        public ActionResult PurchaseAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE2"] != null)
                        _objModel.Status = CONT.NA;

                    else if (Request["btn_RETURN2"] != null)
                        _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Master_PurchaseAgreement_DocumentID;
                        _objModel.Feedback = _objModel.Feedback2;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["btn_APPROVE2"] != null)
                    {
                        UserSendEmail();
                        return RedirectToAction("SupplierAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, IsIndiCmplt = true });
                    }

                    else if (Request["btn_RETURN2"] != null)
                        return RedirectToAction("BlankDashboard", "Home");
                }
                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerReviewModel _objRevModel = new CustomerReviewModel();
                    _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                    if (CheckCount != null)
                        ViewBag.Detail = CheckCount;
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult SupllierAgencyAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE3"] != null)
                        _objModel.Status = CONT.NA;

                    else if (Request["btn_RETURN3"] != null)
                        _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Agency_Agreement_DocumentID;
                        _objModel.Feedback = _objModel.Feedback3;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["btn_APPROVE3"] != null)
                    {
                        UserSendEmail();
                        return RedirectToAction("SupplierAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus, IsIndiCmplt = true });
                    }

                    else if (Request["btn_RETURN3"] != null)
                        return RedirectToAction("BlankDashboard", "Home");
                }
                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerReviewModel _objRevModel = new CustomerReviewModel();
                    _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                    if (CheckCount != null)
                        ViewBag.Detail = CheckCount;
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult FunderAgreement(int UserID = 0, string currentStatus = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {

                UserSignDetailModel _objModel = new UserSignDetailModel();
                _objModel.UserID = UserID;
                _objModel.Status = currentStatus;
                _objModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);

                UserSignDetailModel _objUserSignDetails = new UserSignDetailModel();
                _objUserSignDetails.Status = CONT.NA; _objUserSignDetails.UserID = UserID;

                #region Get data from sign details
                _objUserSignDetails.DocumentID = CONT.Master_Wakala_Agreement_DocumentID;
                var IndicativeData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (IndicativeData != null)
                {
                    _objModel.Feedback1 = IndicativeData.Feedback; ViewBag.btncls_1 = false;
                }

                _objUserSignDetails.DocumentID = CONT.Master_Assignment_Agreement_DocumentID;
                var MurabahaData = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (MurabahaData != null)
                {
                    _objModel.Feedback2 = MurabahaData.Feedback; ViewBag.btncls_2 = false;
                }
                #endregion

                if (ViewBag.btncls_1 == false && ViewBag.btncls_2 == false)
                {
                    var IsMenuPanelHide = _clsCust.UserSignDetail_MenuPanelHide(_objUserSignDetails).FirstOrDefault();
                    if (IsMenuPanelHide != null)
                    {
                        Session["IsMenuPanelHide"] = IsMenuPanelHide.IsMenuPanelHide;
                        ViewBag.IsMenuPanelHide = IsMenuPanelHide.IsMenuPanelHide;
                    }
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult FunderAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE1"] != null)
                        _objModel.Status = CONT.NA;

                    //else if (Request["btn_RETURN1"] != null)
                    //    _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Master_Wakala_Agreement_DocumentID;
                        _objModel.Feedback = _objModel.Feedback1;
                        _clsCust.UserSignDetail_Add(_objModel);
                        _objModel.DocumentID = CONT.Master_Assignment_Agreement_DocumentID;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["btn_APPROVE1"] != null)
                    {
                        UserSendEmail();
                        return RedirectToAction("FunderAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus });
                    }

                    //else if (Request["btn_RETURN1"] != null)
                    //    return RedirectToAction("BlankDashboard", "Home");
                }

                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
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
        public ActionResult AssignmentAgreement(UserSignDetailModel _objModel, FormCollection obj)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCust.Conn = ClsAppDatabase.GetCon();
                    if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                    _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                    string currentStatus = _objModel.Status;
                    if (Request["btn_APPROVE2"] != null)
                        _objModel.Status = CONT.NA;

                    else if (Request["btn_RETURN2"] != null)
                        _objModel.Status = CONT.NM;
                    _objModel.Type = "A";
                    _objModel.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);
                    if (_objModel.UserID > 0)
                    {
                        _objModel.DocumentID = CONT.Master_Assignment_Agreement_DocumentID;
                        _objModel.Feedback = _objModel.Feedback3;
                        _clsCust.UserSignDetail_Add(_objModel);
                    }
                    _clsCust.Tras.Commit();
                    _clsCust.Conn.Close();
                    if (Request["btn_APPROVE2"] != null)
                    {
                        UserSendEmail();
                        return RedirectToAction("FunderAgreement", "OnlineReg", new { UserID = _objModel.UserID, currentStatus = currentStatus });
                    }

                    else if (Request["btn_RETURN2"] != null)
                        return RedirectToAction("BlankDashboard", "Home");
                }
                catch (Exception ex)
                {
                    _clsCust.Conn.Close();
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerReviewModel _objRevModel = new CustomerReviewModel();
                    _objRevModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    var CheckCount = _clsCust.CustomerReviewDetailHistory_ListAllBind(_objRevModel).FirstOrDefault();
                    if (CheckCount != null)
                        ViewBag.Detail = CheckCount;
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult LetterofAwareness(int sp47c457s4 = 0, string st0579dg8s = "", int cu99d447ee = 0)
        {            
            int CustomerSuppID = sp47c457s4;
            string currentStatus = st0579dg8s;
            int CustomerID = cu99d447ee;
            
            UserSignDetailModel _objModel = new UserSignDetailModel();

            //ViewBag.DisplayMsg = 0;
            //_objModel.CustomerID = 31;
            try
            {
                _objModel.CustomerSuppID = CustomerSuppID;
                _objModel.Status = currentStatus;
                _objModel.CustomerID = CustomerID;

                var AcceptLov = _clsCust.UserSignDetail_AcceptLOA(_objModel).FirstOrDefault();
                if (AcceptLov != null)
                {
                    if (AcceptLov.IsAcceptLOA)
                    {
                        ViewBag.DisplayMsg = 1;
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
        [HttpPost]
        public ActionResult LetterofAwareness(UserSignDetailModel _objModel, FormCollection obj)
        {
            try
            {
                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                if (Request["btn_APPROVE"] != null)
                    _objModel.Status = CONT.NA;

                else if (Request["btn_RETURN3"] != null)
                    _objModel.Status = CONT.NM;
                _objModel.Type = "E";
                if (_objModel.CustomerSuppID > 0)
                {
                    _objModel.DocumentID = CONT.Letter_Awareness_DocumentID;
                    _objModel.ReferenceID = _objModel.CustomerSuppID;
                    _clsCust.UserSignDetail_Add(_objModel);
                }
                _clsCust.Tras.Commit();
                _clsCust.Conn.Close();

                return RedirectToAction("ThankYou", "OnlineReg", new { temp_name = CONT.LetterofAwareness_Thankyou, CustomerID = _objModel.CustomerID });
                //return RedirectToAction("Login", "mUserMasters");
            }

            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return View(_objModel);
        }
        public ActionResult FunderLetterofAwareness(int ExporterID = 0, string currentStatus = "")
        {
            UserSignDetailModel _objModel = new UserSignDetailModel();
            _objModel.CustomerExporterID = ExporterID;
            _objModel.Status = currentStatus;
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult FunderLetterofAwareness(UserSignDetailModel _objModel, FormCollection obj)
        {
            try
            {
                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();
                if (Request["btn_APPROVE"] != null)
                    _objModel.Status = CONT.NA;

                else if (Request["btn_RETURN3"] != null)
                    _objModel.Status = CONT.NM;
                _objModel.Type = "P";
                if (_objModel.CustomerExporterID > 0)
                {
                    _objModel.CustomerID = _objModel.CustomerExporterID;
                    _objModel.DocumentID = CONT.Payment_Acknowledgment_DocumentID;
                    _objModel.ReferenceID = _objModel.CustomerExporterID;
                    _clsCust.UserSignDetail_Add(_objModel);
                }
                _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                return RedirectToAction("ThankYou", "OnlineReg", new { temp_name = CONT.Exporter_LtrofAwrns_Thankyou, CustomerID = _objModel.CustomerID });
            }

            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return View(_objModel);
        }
        #endregion

        #region E-Mudhra Integration
        public ActionResult eMudhra(int UserSignID = 0, Boolean IsCloseWindow = false)
        {
            UserSignDetailModel _ObjModel = new UserSignDetailModel();
            if (IsCloseWindow == false)
            {
                try
                {
                    if (UserSignID > 0)
                    {
                        _ObjModel.UserSignID = UserSignID;
                        var GetData = _clsCust.UserSignDetail_ListAll(_ObjModel).FirstOrDefault();

                        if (GetData != null)
                        {
                            _ObjModel.OrderNo = GetData.OrderNo; _ObjModel.HtmlTemplateID = GetData.HtmlTemplateID;
                            var GetSignLocationData = _clsCust.SignLocMaster_ListAll(_ObjModel).FirstOrDefault();
                            if (GetSignLocationData != null)
                            {
                                ViewBag.PageNumber = GetSignLocationData.PageNo;
                                ViewBag.CustomizeCoordinates = GetSignLocationData.Location;
                            }
                            string RndmNo = Guid.NewGuid().ToString().Substring(0, 4);
                            string strRefNumbr = UserSignID + "_" + GetData.CustomerCode + "_" + RndmNo;
                            string FileUploadPath = Server.MapPath("~\\Files\\UploadSignDocs\\" + GetData.AttachName);

                            Byte[] bytes = System.IO.File.ReadAllBytes(FileUploadPath);
                            String Str64Stringfile = Convert.ToBase64String(bytes);

                            ViewBag.File = Str64Stringfile;
                            ViewBag.Name = GetData.ContactFullName;
                            ViewBag.ReferenceNumber = strRefNumbr;
                            ViewBag.AuthToken = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["eMudhra_AuthToken"]);
                            ViewBag.ReturnURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["eMudhra_ReturnURL"]);
                            ViewBag.CancelURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["eMudhra_CancelURL"]);
                            ViewBag.ErrorURL = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["eMudhra_ErrorURL"]);
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
                ViewBag.IsCloseWindow = IsCloseWindow;
            }
            return View(_ObjModel);
        }
        [HttpPost]
        public PartialViewResult ReturnURL(FormCollection frm)
        {
            try
            {
                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();

                string Returnvalue = Request.Form["Returnvalue"];
                string Referencenumber = Request.Form["Referencenumber"];
                string FileType = Request.Form["FileType"];
                string Transactionnumber = Request.Form["Transactionnumber"];
                string ReturnStatus = Request.Form["ReturnStatus"];
                string ErrorMessage = Request.Form["ErrorMessage"];

                var strSplitNo = Referencenumber.Split('_');
                int intUserSignID = Convert.ToInt32(strSplitNo[0]);
                String strCustomerCode = Convert.ToString(strSplitNo[1]);


                UserSignDetailModel _objUserSignDetails = new UserSignDetailModel();
                _objUserSignDetails.UserSignID = intUserSignID;
                var GetUserSignDetails = _clsCust.UserSignDetail_ListAll(_objUserSignDetails).FirstOrDefault();
                if (GetUserSignDetails != null)
                {
                    byte[] bytes = Convert.FromBase64String(Returnvalue);
                    string SourcePath = Server.MapPath(@"~\\Files\\UploadSignDocs\\");
                    string DestinationPath = Server.MapPath(@"~\\Files\\SignDocsBackup\\");
                    string UserFilePath = Server.MapPath(@"~\\Files\\DownloadSignDocs\\");
                    string RndmNo = Guid.NewGuid().ToString().Substring(0, 4);

                    String strMainFileName = GetUserSignDetails.AttachName;
                    String strUserFileName = intUserSignID + "_" + GetUserSignDetails.ContactFullName + "_" + GetUserSignDetails.HtmlName + ".pdf";

                    System.IO.File.Copy(SourcePath + strMainFileName, DestinationPath + RndmNo + "_" + strMainFileName, true);
                    if (System.IO.File.Exists(SourcePath + strMainFileName))
                        System.IO.File.Delete(SourcePath + strMainFileName);

                    #region Save Main File
                    System.IO.FileStream stream = new FileStream(SourcePath + strMainFileName, FileMode.CreateNew);
                    System.IO.BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(bytes, 0, bytes.Length);
                    writer.Close();
                    #endregion

                    #region Save User File
                    System.IO.FileStream StreamUser = new FileStream(UserFilePath + strUserFileName, FileMode.CreateNew);
                    System.IO.BinaryWriter WriterUser = new BinaryWriter(StreamUser);
                    WriterUser.Write(bytes, 0, bytes.Length);
                    WriterUser.Close();
                    #endregion
                }

                #region Save Responce In Database
                UserSignDetailModel _objModel = new UserSignDetailModel();
                _objModel.UserSignID = intUserSignID;
                _objModel.RespTranNo = Transactionnumber;
                _objModel.RespStatus = ReturnStatus;
                _objModel.RespMessage = ErrorMessage;
                _objModel.RespValue = Returnvalue;
                _objModel.Status = CONT.NS;

                _clsCust.UserSignDetail_UpdateResponse(_objModel);
                _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                #endregion

            }
            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult CancelURL(FormCollection frm)
        {
            try
            {
                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();

                string Returnvalue = Request.Form["Returnvalue"];
                string Referencenumber = Request.Form["Referencenumber"];
                string FileType = Request.Form["FileType"];
                string Transactionnumber = Request.Form["Transactionnumber"];
                string ReturnStatus = Request.Form["ReturnStatus"];
                string ErrorMessage = Request.Form["ErrorMessage"];

                var strSplitNo = Referencenumber.Split('_');
                int intUserSignID = Convert.ToInt32(strSplitNo[0]);
                String strCustomerCode = Convert.ToString(strSplitNo[1]);

                #region Save Responce In Database
                UserSignDetailModel _objModel = new UserSignDetailModel();
                _objModel.UserSignID = intUserSignID;
                _objModel.RespTranNo = Transactionnumber;
                _objModel.RespStatus = ReturnStatus;
                _objModel.RespMessage = ErrorMessage;
                _objModel.RespValue = Returnvalue;
                _objModel.Status = CONT.NC;

                //_objModel.Status = CONT.NS;

                _clsCust.UserSignDetail_UpdateResponse(_objModel);
                _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return PartialView();
        }
        [HttpPost]
        public PartialViewResult ErrorURL(FormCollection frm)
        {
            try
            {
                _clsCust.Conn = ClsAppDatabase.GetCon();
                if (_clsCust.Conn.State == ConnectionState.Closed) { _clsCust.Conn.Open(); } else { _clsCust.Conn.Close(); _clsCust.Conn.Open(); }
                _clsCust.Tras = _clsCust.Conn.BeginTransaction();

                string Returnvalue = Request.Form["Returnvalue"];
                string Referencenumber = Request.Form["Referencenumber"];
                string FileType = Request.Form["FileType"];
                string Transactionnumber = Request.Form["Transactionnumber"];
                string ReturnStatus = Request.Form["ReturnStatus"];
                string ErrorMessage = Request.Form["ErrorMessage"];

                var strSplitNo = Referencenumber.Split('_');
                int intUserSignID = Convert.ToInt32(strSplitNo[0]);
                String strCustomerCode = Convert.ToString(strSplitNo[1]);

                #region Save Responce In Database
                UserSignDetailModel _objModel = new UserSignDetailModel();
                _objModel.UserSignID = intUserSignID;
                _objModel.RespTranNo = Transactionnumber;
                _objModel.RespStatus = ReturnStatus;
                _objModel.RespMessage = ErrorMessage;
                _objModel.RespValue = Returnvalue;
                _objModel.Status = CONT.NE;

                _clsCust.UserSignDetail_UpdateResponse(_objModel);
                _clsCust.Tras.Commit(); _clsCust.Conn.Close();
                #endregion
            }
            catch (Exception ex)
            {
                _clsCust.Conn.Close();
                string errorMessage = fn.CreateErrorMessage(ex);
                fn.LogFileWrite(errorMessage);
                if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return PartialView();
        }
        [HttpGet]
        public JsonResult eMudhraSignature(int UserSignID)
        {
            UserSignDetailModel _ObjModel = new UserSignDetailModel();
            _ObjModel.UserSignID = 35;
            var result = _clsCust.UserSignDetail_ListAll(_ObjModel).FirstOrDefault();
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Local Function
        protected void sendNotification(int Tid, string tempName = "")
        {
            string sub = ""; int UserID = 0;
            DataTable DT = new DataTable();
            if (tempName == CONT.SupplierLink_Email_4th)
            {
                CustomerSupplierModel _objSppModel = new CustomerSupplierModel();
                _objSppModel.CustomerSuppID = Tid;
                DT = _clsCust.CustomerSupplierDetail_ListAllTable(_objSppModel);
            }
            else
            {
                CustomerTempModel _model = new CustomerTempModel();
                _model.CustomerTempID = Tid;
                DT = _clsCust.CustomerMasterTemp_DataTable(_model);
                UserID = Convert.ToInt32(DT.Rows[0]["UserID"]);
            }
            string toMail = Convert.ToString(DT.Rows[0]["EmailID"]);
            fn.GetMasterData(tempName, DT, toMail, sub, "", UserID);
        }
        protected void sendSMS(int Tid, string tempName = "")
        {
            DataTable DT = new DataTable();
            CustomerTempModel _model = new CustomerTempModel();
            _model.CustomerTempID = Tid;
            DT = _clsCust.CustomerMasterTemp_DataTable(_model);
            string MobileNo = Convert.ToString(DT.Rows[0]["MobileNo"]);
            fn.GetSMS(tempName, DT, MobileNo);
        }
        protected string GetThankYouMsg(int Tid, string tempName)
        {
            string msg = string.Empty;
            DataTable DT = new DataTable();
            CustomerTempModel _model = new CustomerTempModel();
            _model.CustomerTempID = Tid;
            DT = _clsCust.CustomerMasterTemp_DataTable(_model);
            msg = fn.GetTempMsg(tempName, DT);
            return msg;
        }
        protected string GetThankYouMsgText(int CustomerID, string tempName)
        {
            string msg = string.Empty;
            DataTable DT = new DataTable();
            CustomerMasterModel _model = new CustomerMasterModel();
            _model.CustomerID = CustomerID;
            DT = _clsCust.CustomerMaster_DataTable(_model);
            msg = fn.GetTempMsg(tempName, DT);
            return msg;
        }
        public void FillProgramTypeCombo()
        {
            //ViewBag.DDLProgramTypeID = new List<SelectListItem> { new SelectListItem() { Text = "Receivables Finance", Value = "F" },
            //                                                      new SelectListItem() { Text = "Payables Finance", Value = "R" },
            //                                                      new SelectListItem() { Text = "Receivables Finance & Payables Finance", Value = "B" } };

            ViewBag.DDLProgramTypeID = new List<SelectListItem> { new SelectListItem() { Text = "Receivables Finance", Value = "F" },
                                                                  new SelectListItem() { Text = "Payables Finance", Value = "R" }};
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
        [HttpPost]
        public JsonResult GetSubIndustryData(int IndustryID)
        {
            List<CountryMaster> IndustryMaster_ListAll = _clsInds.IndustryMaster_ListAll(0, IndustryID, "", -1, false, "").ToList();
            return Json(IndustryMaster_ListAll, JsonRequestBehavior.AllowGet);
        }
        protected void UserSendEmail()
        {
            UserMaster_ListAll_Result _objUserMaster = new UserMaster_ListAll_Result();
            _objUserMaster.UserID = Convert.ToInt32(Session["UserID"]);
            _objUserMaster = _clsCust.UserSingDetail_SendUserMail(_objUserMaster).FirstOrDefault();

            if (_objUserMaster.IsSendUserMail)
            {
                _objUserMaster.UserID = 0;
                _objUserMaster.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                _objUserMaster.IsActive = true;
                var UserData = _clsUser.UserMaster_ListAll(_objUserMaster).ToList();
                if (UserData != null)
                {
                    foreach (var item in UserData)
                    {
                        if (item.IsLoginMailSend == 0)
                        {
                            sendNotificationToUser(item.UserID, CONT.SupplierEmail_2st);
                        }
                    }
                }
            }
        }
        protected void sendNotificationToUser(int Tid, string tempName = "")
        {
            ClsCustomerMaster _ClsCustomerMaster = new ClsCustomerMaster();
            string sub = "";
            string toMail = "";
            int UserID = 0;
            DataTable DT = new DataTable();
            UserMaster_ListAll_Result _objModel = new UserMaster_ListAll_Result();
            _objModel.UserID = Tid;
            _objModel.IsActive = true;
            DT = _ClsCustomerMaster.UserMaster_ListAllTable(_objModel);
            toMail = Convert.ToString(DT.Rows[0]["EmailID"]);
            UserID = Convert.ToInt32(DT.Rows[0]["UserID"]);
            fn.GetMasterData(tempName, DT, toMail, sub, "", UserID);
        }
        public void FillGenderCombo()
        {
            ViewBag.DDLGenderID = new List<SelectListItem> { new SelectListItem() { Text = "Male", Value = "Male" },
                                                     new SelectListItem() { Text = "Female", Value = "Female" } };
        }
        #endregion

        [HttpPost]
        public ActionResult GeneratePDF(CustomerMasterModel _objModel)
        {
            var data = "";
            try
            {
                _objModel = _clsCust.CustomerMasterHistory_ListAllBind(_objModel).FirstOrDefault();
                string filename = _objModel.CustomerCode + ".pdf";
                string FilePath = Server.MapPath("\\Files\\PDF\\" + filename + "");
                string RequestUrl = Convert.ToString(WebConfigurationManager.AppSettings["PDFpageUrl"]) + _objModel.CustomerID + "";
                SaveHttpResponseAsFile(RequestUrl, FilePath);
                _objModel.filename = filename;
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
        public ActionResult PrintPDFSave(int CustomerID = 0)
        {
            CustomerMasterModel _objModel = new CustomerMasterModel();
            CustomerInfoModel _objinfoModel = new CustomerInfoModel();
            _objModel.CustomerID = CustomerID;
            _objModel = _clsCust.CustomerMasterHistory_ListAllBind(_objModel).FirstOrDefault();

            _objinfoModel.CustomerID = CustomerID;
            _objinfoModel = _clsCust.CustomerInfoDetailHistory_ListAllBind(_objinfoModel).FirstOrDefault();

            _objinfoModel.CustomerID = CustomerID;
            ViewBag.CreditDetails = _clsCust.CustomerBankCreditDetailHistory_ListAllBind(_objinfoModel);

            CustomerBankAccDetail _objBankModel = new CustomerBankAccDetail();
            _objBankModel.CustomerID = CustomerID;
            ViewBag.BankAccDetail = _clsCust.CustomerBankAccDetailHistory_ListAllBind(_objBankModel);

            CustomerShareHolderModel _objShareModel = new CustomerShareHolderModel();
            _objShareModel.CustomerID = CustomerID;
            _objShareModel.CustType = "A,D";
            ViewBag.AuthUser = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objShareModel);

            _objShareModel.CustType = "S";
            ViewBag.ShareIndi = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objShareModel);

            _objShareModel.CustType = "C";
            ViewBag.ShareComp = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objShareModel);


            _objShareModel.CustType = "B";
            _objShareModel.ParentID = -1;
            ViewBag.ShareBene = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objShareModel);

            UserMaster_ListAll_Result _objUserMaster = new UserMaster_ListAll_Result();
            _objUserMaster.CustomerID = CustomerID;
            ViewBag.UserDetail = _clsUser.UserMasterHistory_ListAllBind(_objUserMaster).ToList();

            CustomerSupplierModel _objSupplier = new CustomerSupplierModel();
            _objSupplier.CustomerID = CustomerID;
            ViewBag.Supplier = _clsCust.CustomerSupplierDetailHistory_ListAllBind(_objSupplier);

            _objSupplier.CustomerID = CustomerID;
            _objSupplier.IsCustomerSupp = true;
            ViewBag.ExpoBankDetail = _clsCust.ExpoCustomerBankAccDetailHistory_ListAllBind(_objSupplier);

            var tupleModel = new Tuple<CustomerMasterModel, CustomerInfoModel>(_objModel, _objinfoModel);

            return new Rotativa.MVC.ViewAsPdf("PrintPDFSave", tupleModel);
            //return View(tupleModel);
        }
        public static void SaveHttpResponseAsFile(string RequestUrl, string FilePath)
        {
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(RequestUrl);
                httpRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                httpRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (System.Net.WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                        response = (HttpWebResponse)ex.Response;
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    Stream FinalStream = responseStream;
                    if (response.ContentEncoding.ToLower().Contains("gzip"))
                        FinalStream = new GZipStream(FinalStream, CompressionMode.Decompress);
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))
                        FinalStream = new DeflateStream(FinalStream, CompressionMode.Decompress);

                    using (var fileStream = System.IO.File.Create(FilePath))
                    {
                        FinalStream.CopyTo(fileStream);
                    }

                    response.Close();
                    FinalStream.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}