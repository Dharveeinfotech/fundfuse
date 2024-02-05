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

namespace TMP.Controllers
{
    public class FactoringController : Controller
    {
        #region Object

        ClsCustomerMaster _clsCust = new ClsCustomerMaster();
        ClsCountryMaster _clsContr = new ClsCountryMaster();
        ClsCurrency _clsCurry = new ClsCurrency();
        ClsInvoiceTransaction _clsInvoi = new ClsInvoiceTransaction();
        Function fn = new Function();


        #endregion

        #region Supplier / Buyer
        // GET: Factoring
        public ActionResult Index(string ProgramType = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { ViewBag._currentUser = _currentUser; }
                    _objModel.Status = _currentStatus(ProgramType);
                    ViewBag.ProgramType = _objModel.ProgramType = ProgramType;
                    if (ProgramType == CONT.FType)
                        _objModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                    else if (ProgramType == CONT.RType)
                        _objModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);
                    var data = _clsInvoi.InvoiceMaster_ListAll(_objModel).ToList();
                    return View(data);
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
        public ActionResult UploadInvoice(int InvoiceID = 0, int InvoiceDetID = 0, int InvoiceEditID = 0, string ProgramType = "")
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BuyerCustomerTypeID ||
                        Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BothObligorAndBuyerTypeID)
                        ProgramType = CONT.RType;
                    else
                        ProgramType = CONT.FType;
                    _objModel.ProgramType = ProgramType;
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }
                    if (InvoiceID > 0 && InvoiceDetID > 0)
                    {
                        _clsInvoi.Conn = ClsAppDatabase.GetCon();
                        if (_clsInvoi.Conn.State == ConnectionState.Closed) { _clsInvoi.Conn.Open(); } else { _clsInvoi.Conn.Close(); _clsInvoi.Conn.Open(); }
                        _clsInvoi.Tras = _clsInvoi.Conn.BeginTransaction();
                        _objModel.InvoiceDetID = InvoiceDetID; _objModel.InvoiceID = InvoiceID;
                        _clsInvoi.InvoiceDetail_Delete(_objModel);
                        _clsInvoi.Tras.Commit();
                        _clsInvoi.Conn.Close();
                        return RedirectToAction("UploadInvoice", "Factoring", new { InvoiceID = _objModel.InvoiceID, ProgramType = ProgramType });
                    }

                    if (InvoiceID > 0)
                    {
                        _objModel.InvoiceID = InvoiceID;
                        _objModel = _clsInvoi.InvoiceMaster_ListAll(_objModel).FirstOrDefault();
                    }

                    if (InvoiceEditID > 0)
                    {
                        _objModel.InvoiceDetID = InvoiceEditID; _objModel.InvoiceID = InvoiceID;
                        var EditData = _clsInvoi.InvoiceDetail_ListAll(_objModel).FirstOrDefault();
                        _objModel.InvoiceNo = EditData.InvoiceNo; _objModel.InvoiceDate = EditData.InvoiceDate;
                        _objModel.Amt = EditData.Amt; _objModel.Attach = EditData.Attach;
                        _objModel.InvoiceDetID = InvoiceEditID;
                    }
                    FillCustomerCombo(_objModel.ProgramType);
                    FillCurrencyCombo(_objModel.ProgramType);
                    var SystemParameterData = _clsContr.SystemPerameter_ListAll().FirstOrDefault();
                    if (SystemParameterData != null)
                    {
                        ViewBag.ProcessTranDay = SystemParameterData.ProcessTranDay;
                    }
                    else
                    {
                        ViewBag.ProcessTranDay = 0;
                    }
                    return View(_objModel);
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    ViewBag.DDLCurrencyID = new List<SelectListItem> { }; ViewBag.DDLCustomerID = new List<SelectListItem> { };
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult UploadInvoice(InvoiceTransactionModel _objModel, HttpPostedFileBase Attach)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    _clsInvoi.Conn = ClsAppDatabase.GetCon();
                    if (_clsInvoi.Conn.State == ConnectionState.Closed) { _clsInvoi.Conn.Open(); } else { _clsInvoi.Conn.Close(); _clsInvoi.Conn.Open(); }
                    _clsInvoi.Tras = _clsInvoi.Conn.BeginTransaction();
                    if (_objModel.ProgramType == CONT.FType)
                        _objModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                    else
                        _objModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);

                    if (Request["Submit"] != null)
                    {
                        if (_objModel.ProgramType == CONT.FType)
                            _objModel.Status = CONT.SR;
                        else
                            _objModel.Status = CONT.BR;

                        _clsInvoi.InvoiceMaster_UpdateAllStatus(_objModel);
                        _clsInvoi.Tras.Commit(); _clsInvoi.Conn.Close();
                    }
                    else
                    {
                        string newfilenm1 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (Attach != null)
                        {
                            if (Attach.FileName != null)
                            {
                                _objModel.Attach = newfilenm1 + Attach.FileName.ToString();
                                Attach.SaveAs(Server.MapPath("\\Files\\Invoice\\" + _objModel.Attach));
                            }
                        }

                        if (_objModel.InvoiceID > 0)
                        {
                            _clsInvoi.InvoiceMaster_Update(_objModel);
                            if (_objModel.InvoiceDetID > 0)
                            { _clsInvoi.InvoiceDetail_Update(_objModel); }
                            else { _clsInvoi.InvoiceDetail_Add(_objModel); }
                        }
                        else
                        {
                            _objModel.InvoiceID = _clsInvoi.InvoiceMaster_Add(_objModel);
                            _clsInvoi.InvoiceDetail_Add(_objModel);
                        }
                        _clsInvoi.Tras.Commit(); _clsInvoi.Conn.Close();
                    }

                    if (Request["SaveAddNew"] != null)
                    {
                        return RedirectToAction("UploadInvoice", "Factoring", new { InvoiceID = _objModel.InvoiceID, ProgramType = _objModel.ProgramType });
                    }
                    else
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }

                }
                catch (Exception ex)
                {
                    if (_clsInvoi.Conn.State == ConnectionState.Open)
                    { _clsInvoi.Tras.Rollback(); _clsInvoi.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCustomerCombo(_objModel.ProgramType);
                FillCurrencyCombo(_objModel.ProgramType);
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult SupplierCheckerIndex(string ProgramType = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { ViewBag._currentUser = _currentUser; }
                    _objModel.Status = CONT.Supplier_Buyer_Offer;

                    if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.SupplierCustomerTypeID)
                    {
                        _objModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                        ViewBag.ProgramType = _objModel.ProgramType = CONT.FType;
                    }
                    else if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BuyerCustomerTypeID)
                    {
                        _objModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);
                        ViewBag.ProgramType = _objModel.ProgramType = CONT.RType;
                    }
                    else if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.PayableSupplierCustomerTypeID)
                    {
                        _objModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                        ViewBag.ProgramType = _objModel.ProgramType = CONT.RType;
                    }
                    else if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BothObligorAndBuyerTypeID)
                    {
                        _objModel.ObligorBuyerID = Convert.ToInt32(Session["CustomerID"]);
                        ViewBag.ProgramType = _objModel.ProgramType = CONT.BothProgramType;
                    }

                    var data = _clsInvoi.InvoiceMaster_ListAll(_objModel).ToList();
                    return View(data);
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
        public ActionResult SupplierChecker(int InvoiceID = 0, string ProgramType = "")
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }

                    if (InvoiceID > 0)
                    {
                        _objModel.InvoiceID = InvoiceID; _objModel.ProgramType = ProgramType;
                        _objModel = _clsInvoi.InvoiceMaster_ListAll(_objModel).FirstOrDefault();
                    }
                    if (_objModel.Status == CONT.CB || _objModel.Status == CONT.CE)
                    {
                        CustomerBankAccDetail _objCustModel = new CustomerBankAccDetail();
                        _objCustModel.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                        _objCustModel.Status = CONT.ddstatus;
                        ViewBag.CustomerBankAccID = new SelectList(_clsCust.CustomerBankAccDetail_ListAll(_objCustModel).OrderBy(x => x.BankName).ToList(), "CustomerBankAccID", "CustomerAccountNo", _objModel.CustomerBankAccID);
                        return View("_SupplierCheckerOffer", _objModel);
                    }
                    else
                    {
                        return View("SupplierChecker", _objModel);
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    ViewBag.CustomerBankAccID = new List<SelectListItem> { };
                    _objModel.InvoiceID = 0;
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult SupplierChecker(InvoiceTransactionModel _objModel)
        {

            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }

                    _clsInvoi.Conn = ClsAppDatabase.GetCon();
                    if (_clsInvoi.Conn.State == ConnectionState.Closed) { _clsInvoi.Conn.Open(); } else { _clsInvoi.Conn.Close(); _clsInvoi.Conn.Open(); }
                    _clsInvoi.Tras = _clsInvoi.Conn.BeginTransaction();

                    if (_objModel.ProgramType == CONT.FType)
                    {
                        _objModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                    }
                    else
                    {
                        _objModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);
                    }

                    if (Request["Submit"] != null)
                    {

                        if (_objModel.ProgramType == CONT.FType && _objModel.Status == CONT.CB)
                        { _objModel.Status = CONT.SB; }
                        else if (_objModel.ProgramType == CONT.RType && _objModel.Status == CONT.CE)
                        { _objModel.Status = CONT.BB; }
                        else
                        {
                            if (_objModel.ProgramType == CONT.FType)
                            {
                                _objModel.IsSupplierConf = true; _objModel.Status = CONT.SA;
                            }
                            else
                            {
                                if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.PayableSupplierCustomerTypeID)
                                {
                                    if (_objModel.Status =="SK1")
                                    {
                                        _objModel.IsBuyerConf = true; _objModel.Status = "SL1";
                                    }
                                    else if (_objModel.Status == "SL1")
                                    {
                                        _objModel.IsBuyerConf = true; _objModel.Status = CONT.BA;
                                    }
                                    else
                                    {
                                        _objModel.IsBuyerConf = true; _objModel.Status = "SK1";
                                    }
                                    
                                }
                                else
                                {
                                    _objModel.IsBuyerConf = true; _objModel.Status = "SK1"; //CONT.BA;
                                }
                            }
                        }
                    }
                    if (Request["Return"] != null)
                    {
                        if (_objModel.ProgramType == CONT.FType)
                            _objModel.Status = CONT.SP;
                        else
                            _objModel.Status = CONT.BP;
                    }
                    _clsInvoi.InvoiceMaster_UpdateAllStatus(_objModel);
                    _clsInvoi.Tras.Commit(); _clsInvoi.Conn.Close();
                    return RedirectToAction("CMNDashboard", "Home");
                }
                catch (Exception ex)
                {
                    if (_clsInvoi.Conn.State == ConnectionState.Open)
                    { _clsInvoi.Conn.Close(); }
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    CustomerBankAccDetail _objCustModel = new CustomerBankAccDetail();
                    _objCustModel.CustomerID = _objModel.SupplierID;
                    _objCustModel.Status = CONT.ddstatus;
                    ViewBag.CustomerBankAccID = new SelectList(_clsCust.CustomerBankAccDetail_ListAll(_objCustModel).OrderBy(x => x.BankName).ToList(), "CustomerBankAccID", "BankName", _objModel.CustomerBankAccID);
                }
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public string DueDateHoliday(DateTime DueDate)
        {
            string resp = "";
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    int LoginUserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out LoginUserId);
                    ClsInvoiceTransaction _ClsInvoiceTransaction = new ClsInvoiceTransaction();
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed)
                    { _ClsInvoiceTransaction.Conn.Open(); }
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    DataTable dt = _ClsInvoiceTransaction.InvoiceMaster_DueDateHoliday(DueDate);
                    if (dt != null && dt.Rows.Count > 0)
                    { resp = "Error"; }
                    else
                    { resp = "Successfully"; }
                    _ClsInvoiceTransaction.Tras.Commit();
                    _ClsInvoiceTransaction.Conn.Close();
                    return resp;
                }
                catch (Exception ex)
                {
                    string errorMessage = fn.CreateErrorMessage(ex);
                    fn.LogFileWrite(errorMessage);
                    resp = "Error";
                }
                return resp;
            }
            else
            {
                return resp;
            }
        }
        #endregion

        #region Obligor
        public ActionResult ObligorIndex()
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { ViewBag._currentUser = _currentUser; }
                    _objModel.Status = CONT.OA + ',' + CONT.OP;
                    _objModel.ProgramType = CONT.FType;
                    _objModel.ObligorID = Convert.ToInt32(Session["CustomerID"]);
                    var data = _clsInvoi.InvoiceMaster_ListAll(_objModel).ToList();
                    return View(data);
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
        public ActionResult ObligorMaker(int InvoiceID = 0)
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }
                    if (InvoiceID > 0)
                    {
                        _objModel.InvoiceID = InvoiceID;
                        _objModel.ProgramType = CONT.FType;
                        _objModel = _clsInvoi.InvoiceMaster_ListAll(_objModel).FirstOrDefault();
                    }
                    return View(_objModel);
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
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult ObligorMaker(InvoiceTransactionModel _objModel)
        {

            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }

                    _clsInvoi.Conn = ClsAppDatabase.GetCon();
                    if (_clsInvoi.Conn.State == ConnectionState.Closed) { _clsInvoi.Conn.Open(); } else { _clsInvoi.Conn.Close(); _clsInvoi.Conn.Open(); }
                    _clsInvoi.Tras = _clsInvoi.Conn.BeginTransaction();
                    _objModel.ProgramType = CONT.FType;
                    _objModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);

                    if (Request["Approve"] != null)
                    {
                        _objModel.Status = CONT.OA;
                    }
                    if (Request["Reject"] != null)
                    {
                        _objModel.Status = CONT.OP;
                    }
                    _clsInvoi.InvoiceMaster_UpdateAllStatus(_objModel);
                    _clsInvoi.Tras.Commit(); _clsInvoi.Conn.Close();
                    return RedirectToAction("CMNDashboard", "Home");
                }
                catch (Exception ex)
                {
                    if (_clsInvoi.Conn.State == ConnectionState.Open)
                    { _clsInvoi.Conn.Close(); }
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
        public ActionResult ObligorChecker(int InvoiceID = 0)
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }
                    if (InvoiceID > 0)
                    {
                        _objModel.InvoiceID = InvoiceID;
                        _objModel.ProgramType = CONT.FType;
                        _objModel = _clsInvoi.InvoiceMaster_ListAll(_objModel).FirstOrDefault();
                    }
                    return View(_objModel);
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
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult ObligorChecker(InvoiceTransactionModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }

                    _clsInvoi.Conn = ClsAppDatabase.GetCon();
                    if (_clsInvoi.Conn.State == ConnectionState.Closed) { _clsInvoi.Conn.Open(); } else { _clsInvoi.Conn.Close(); _clsInvoi.Conn.Open(); }
                    _clsInvoi.Tras = _clsInvoi.Conn.BeginTransaction();
                    _objModel.ProgramType = CONT.FType;
                    _objModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);

                    if (Request["Approve"] != null)
                    {
                        _objModel.IsObligorConf = true;
                        _objModel.Status = CONT.OB;
                    }
                    if (Request["Reject"] != null)
                    {
                        _objModel.Status = CONT.OR;
                    }
                    _clsInvoi.InvoiceMaster_UpdateAllStatus(_objModel);
                    _clsInvoi.Tras.Commit(); _clsInvoi.Conn.Close();
                    return RedirectToAction("CMNDashboard", "Home");
                }
                catch (Exception ex)
                {
                    if (_clsInvoi.Conn.State == ConnectionState.Open)
                    { _clsInvoi.Conn.Close(); }
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
        #endregion

        #region Funder
        public ActionResult FunderIndex(string ProgramType = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { ViewBag._currentUser = _currentUser; }
                    _objModel.Status = _currentStatus(ProgramType);
                    ViewBag.ProgramType = _objModel.ProgramType = ProgramType;
                    _objModel.FunderID = Convert.ToInt32(Session["CustomerID"]);
                    var data = _clsInvoi.InvoiceMaster_ListAll(_objModel).ToList();

                    return View(data);
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
        public ActionResult FundingPartnerObligors(string ProgramType = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objCustModel._currentUser = _currentUser; }
                    _objCustModel.CustomerTypeIDs = CONT.ObligorCustomerTypeID + "," + CONT.BuyerCustomerTypeID + "," + CONT.BothObligorAndBuyerTypeID;
                    _objCustModel.Status = CONT.ddstatus;
                    var data = _clsCust.CustomerMaster_ListAll(_objCustModel).ToList();
                    return View(data);
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
        public ActionResult FundingPartnerObligorDetails(int InvoiceID = 0, int ObligorID = 0, int SupplierID = 0, string ProgramType = "")
        {

            CustomerInfoModel _objCustModel = new CustomerInfoModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    _objCustModel.InvoiceID = InvoiceID;
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objCustModel._currentUser = _currentUser; }
                    if (ObligorID > 0)
                    {
                        _objCustModel.CustomerID = ObligorID;
                        _objCustModel = _clsCust.CustomerInfoDetail_ListAll(_objCustModel).FirstOrDefault();

                        _objModel.CustomerID = ObligorID;
                        ViewBag.ObligorGrid = _clsInvoi.CustomerAccDetail_ListAll(_objModel).ToList();
                        _objCustModel.SupplierID = SupplierID; _objCustModel.InvoiceID = InvoiceID; _objCustModel.ObligorID = ObligorID;
                        CustomerMasterModel _objCustDocModel = new CustomerMasterModel();
                        _objCustDocModel.CustomerID = ObligorID;
                        ViewBag.Document = _clsCust.CustomerMaster_BindAllDocument(_objCustDocModel).Where(x => x.DocumentName == "Memorandum of Association").FirstOrDefault();

                        CustomerDocModel _objCountModel = new CustomerDocModel();
                        _objCountModel.CustomerID = ObligorID;
                        ViewBag.CustGetCount = _clsCust.CustomerMaster_GetCount(_objCountModel).FirstOrDefault();
                    }

                    _objCustModel.ProgramType = ProgramType;
                    _objCustModel._currentUser = _currentUser;
                    return View(_objCustModel);
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
            return View(_objCustModel);
        }
        [HttpPost]
        public ActionResult FundingPartnerObligorDetails(CustomerInfoModel _objCustModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (_objCustModel.InvoiceID > 0)
                {
                    if (Request["Back"] != null)
                    {
                        //return RedirectToAction("FunderTransaction", "Factoring", new { InvoiceID = _objCustModel.InvoiceID, ProgramType = _objCustModel.ProgramType });
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                }
                else
                {
                    if (Request["BackIndex"] != null)
                    {
                        return RedirectToAction("FundingPartnerObligors", "Factoring", new { ProgramType = _objCustModel.ProgramType });
                    }
                    else if (Request["Home"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                }
                return View(_objCustModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult FundingPartnerSuppliers(string ProgramType = "", bool IsConnected = false)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objCustModel._currentUser = _currentUser; }
                    _objCustModel.CustomerTypeID = CONT.SupplierCustomerTypeID;
                    _objCustModel.Status = CONT.ddstatus;
                    var data = _clsCust.CustomerMaster_ListAll(_objCustModel).ToList();

                    return View(data);
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
        public ActionResult FundingPartnerSupplierDetails(int InvoiceID = 0, int ObligorID = 0, int SupplierID = 0, string ProgramType = "")
        {
            CustomerInfoModel _objCustModel = new CustomerInfoModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _objCustModel.InvoiceID = InvoiceID;
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objCustModel._currentUser = _currentUser; }
                    if (SupplierID > 0)
                    {
                        _objCustModel.CustomerID = SupplierID;
                        _objCustModel = _clsCust.CustomerInfoDetail_ListAll(_objCustModel).FirstOrDefault();

                        _objModel.CustomerID = SupplierID;
                        ViewBag.ObligorGrid = _clsInvoi.CustomerAccDetail_ListAll(_objModel).ToList();
                        _objCustModel.ObligorID = ObligorID; _objCustModel.InvoiceID = InvoiceID; _objCustModel.SupplierID = SupplierID;
                    }
                    _objCustModel.ProgramType = ProgramType;
                    return View(_objCustModel);
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
            return View(_objCustModel);
        }
        [HttpPost]
        public ActionResult FundingPartnerSupplierDetails(CustomerInfoModel _objCustModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (_objCustModel.InvoiceID > 0)
                {
                    if (Request["Back"] != null)
                    {
                        //return RedirectToAction("FunderTransaction", "Factoring", new { InvoiceID = _objCustModel.InvoiceID, ProgramType = _objCustModel.ProgramType });
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                }
                else
                {
                    if (Request["BackIndex"] != null)
                    {
                        return RedirectToAction("FundingPartnerSuppliers", "Factoring", new { ProgramType = _objCustModel.ProgramType });
                    }
                    else if (Request["Home"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                }
                return View(_objCustModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult FundingPartnerBuyer(string ProgramType = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objCustModel._currentUser = _currentUser; }
                    _objCustModel.CustomerTypeID = CONT.BuyerCustomerTypeID;
                    _objCustModel.Status = CONT.ddstatus;
                    var data = _clsCust.CustomerMaster_ListAll(_objCustModel).ToList();
                    return View(data);
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
        public ActionResult FundingPartnerBuyerDetails(int InvoiceID = 0, int BuyerID = 0, string ProgramType = "")
        {
            CustomerInfoModel _objCustModel = new CustomerInfoModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _objCustModel.InvoiceID = InvoiceID;
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objCustModel._currentUser = _currentUser; }
                    if (BuyerID > 0)
                    {
                        _objCustModel.CustomerID = BuyerID;
                        _objCustModel = _clsCust.CustomerInfoDetail_ListAll(_objCustModel).FirstOrDefault();

                        _objModel.CustomerID = BuyerID;
                        ViewBag.BuyerGrid = _clsInvoi.CustomerAccDetail_ListAll(_objModel).ToList();
                        _objCustModel.BuyerID = BuyerID; _objCustModel.InvoiceID = InvoiceID;

                        CustomerMasterModel _objCustDocModel = new CustomerMasterModel();
                        _objCustDocModel.CustomerID = BuyerID;
                        ViewBag.Document = _clsCust.CustomerMaster_BindAllDocument(_objCustDocModel).Where(x => x.DocumentName == "Memorandum of Association").FirstOrDefault();
                    }
                    _objCustModel.ProgramType = ProgramType;
                    _objCustModel._currentUser = _currentUser;
                    return View(_objCustModel);
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
            return View(_objCustModel);
        }
        [HttpPost]
        public ActionResult FundingPartnerBuyerDetails(CustomerInfoModel _objCustModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (_objCustModel.InvoiceID > 0)
                {
                    if (Request["Back"] != null)
                    {
                        //return RedirectToAction("FunderTransaction", "Factoring", new { InvoiceID = _objCustModel.InvoiceID, ProgramType = _objCustModel.ProgramType });
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                }
                else
                {
                    if (Request["BackIndex"] != null)
                    {
                        return RedirectToAction("FundingPartnerBuyer", "Factoring", new { ProgramType = _objCustModel.ProgramType });
                    }
                    else if (Request["Home"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }

                }
                return View(_objCustModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult FunderCompanyMoreInfo(int CustomerID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            try
            {
                _objModel.CustomerID = CustomerID;
                _objModel.CustType = "D,A";
                _objModel.Status = CONT.ddstatus;
                ViewBag.DirAuthSignGrid = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                _objModel.CustType = "S";
                _objModel.Status = CONT.ddstatus;
                ViewBag.SharHoldGrid = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                _objModel.CustType = "C";
                _objModel.Status = CONT.ddstatus;
                ViewBag.CompSharHoldGrid = _clsCust.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                CustomerInfoModel _objCredit = new CustomerInfoModel();
                _objCredit.CustomerID = CustomerID;
                _objCredit.Status = CONT.mkstatus;
                ViewBag.CreditGrid = _clsCust.CustomerBankCreditDetailHistory_ListAllBind(_objCredit);
                InvoiceTransactionModel _InvObjModel = new InvoiceTransactionModel();
                _InvObjModel.CompanyID = CustomerID;
                ViewBag.Obligors = _clsInvoi.CustomerObligorDetail_ListAll(_InvObjModel).ToList();

                return PartialView(_objModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult FunderDashboard(string ProgramType = "")
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }
                    _objModel.Status = _currentStatus(ProgramType);
                    ViewBag.ProgramType = _objModel.ProgramType = ProgramType;
                    var data = _clsInvoi.InvoiceMaster_ListAll(_objModel).ToList();

                    return View(data);
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
        public ActionResult FunderTransaction(int InvoiceID = 0, string ProgramType = "", bool hideDiv = false)
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }
                    _objModel.UserID = Convert.ToInt32(Session["UserID"]);
                    ViewBag._currentUser = _clsInvoi.UserRole_GetDetail(_objModel).FirstOrDefault();
                    if (InvoiceID > 0)
                    {
                        _objModel.InvoiceID = InvoiceID;
                        _objModel.ProgramType = ProgramType;
                        _objModel = _clsInvoi.InvoiceMaster_ListAll(_objModel).FirstOrDefault();
                    }
                    ViewBag.hideDiv = hideDiv;
                    return View(_objModel);
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
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult FunderTransaction(InvoiceTransactionModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {

                try
                {
                    if (Request["Accept"] != null || Request["Approve"] != null || Request["Reject"] != null)
                    {
                        _clsInvoi.Conn = ClsAppDatabase.GetCon();
                        if (_clsInvoi.Conn.State == ConnectionState.Closed) { _clsInvoi.Conn.Open(); } else { _clsInvoi.Conn.Close(); _clsInvoi.Conn.Open(); }
                        _clsInvoi.Tras = _clsInvoi.Conn.BeginTransaction();
                        if (Request["Approve"] != null)
                        {
                            if (_objModel.IsInsurance == false)
                            {
                                if (_objModel.ProgramType == CONT.FType)
                                { _objModel.Status = CONT.OE; }
                                else
                                { _objModel.Status = CONT.IE; }
                            }
                            else
                            {
                                if (_objModel.ProgramType == CONT.FType)
                                { _objModel.Status = CONT.FH; }
                                else
                                { _objModel.Status = CONT.FG; }
                            }
                        }
                        else if (Request["Reject"] != null)
                        {
                            if (_objModel.ProgramType == CONT.FType)
                            { _objModel.Status = CONT.FE; }
                            else
                            { _objModel.Status = CONT.FF; }
                        }
                        else if (Request["Accept"] != null)
                        {
                            _objModel.IsFunderConf = true;
                            if (_objModel.ProgramType == CONT.FType)
                            { _objModel.Status = CONT.FB; }
                            else
                            { _objModel.Status = CONT.FD; }
                        }
                        _clsInvoi.InvoiceMaster_UpdateAllStatus(_objModel);
                        _clsInvoi.Tras.Commit(); _clsInvoi.Conn.Close();

                        //26/07/2020
                        //if (_objModel.Status == CONT.FD)
                            //sendNotification(_objModel.InvoiceID, CONT.Exporter_LtrofAwrns);
                    }
                    if (Request["Back"] != null || Request["Cancel"] != null || Request["Accept"] != null || Request["Approve"] != null || Request["Reject"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    else if (Request["Summary"] != null)
                    {
                        if (_objModel.ProgramType == CONT.RType)
                        {
                            _objModel.ObligorID = _objModel.BuyerID;
                        }
                       
                        return RedirectToAction("FundingPartnerObligorDetails", "Factoring", new { InvoiceID = _objModel.InvoiceID, ObligorID = _objModel.ObligorID, SupplierID = _objModel.SupplierID, ProgramType = _objModel.ProgramType });
                        
                    }
                    else if (Request["Fund"] != null)
                    {
                        if (Request["cb1"] != null)
                        {
                            _objModel.IsInvOffer = true;
                        }
                        return RedirectToAction("FunderConfirmation", "Factoring", new { InvoiceID = _objModel.InvoiceID, ProgramType = _objModel.ProgramType, IsInvOffer = _objModel.IsInvOffer });
                    }
                }
                catch (Exception ex)
                {
                    if (_clsInvoi.Conn.State == ConnectionState.Open)
                    { _clsInvoi.Conn.Close(); }
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
        public ActionResult FunderConfirmation(int InvoiceID = 0, string ProgramType = "", bool IsInvOffer =false)
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }

                    if (InvoiceID > 0)
                    {
                        _objModel.InvoiceID = InvoiceID; _objModel.ProgramType = ProgramType;
                        _objModel = _clsInvoi.InvoiceMaster_ListAll(_objModel).FirstOrDefault();
                        _objModel.IsInvOffer = IsInvOffer;
                    }
                    return View(_objModel);
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
            return View(_objModel);
        }
        [HttpPost]
        public ActionResult FunderConfirmation(InvoiceTransactionModel _objModel)
        {
            string[] LoginStatus = fn.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {

                    if (Request["Agree"] != null)
                    {
                        _clsInvoi.Conn = ClsAppDatabase.GetCon();
                        if (_clsInvoi.Conn.State == ConnectionState.Closed) { _clsInvoi.Conn.Open(); } else { _clsInvoi.Conn.Close(); _clsInvoi.Conn.Open(); }
                        _clsInvoi.Tras = _clsInvoi.Conn.BeginTransaction();
                        _objModel.FunderID = Convert.ToInt32(Session["CustomerID"]);
                        if (_objModel.ProgramType == CONT.FType)
                        {
                            _objModel.Status = CONT.FA;
                        }
                        else
                        {
                            _objModel.Status = CONT.FC;
                        }

                        _objModel.IsFunderAgree = true;
                        _clsInvoi.InvoiceMaster_UpdateAllStatus(_objModel);
                        _clsInvoi.Tras.Commit(); _clsInvoi.Conn.Close();
                    }
                    return RedirectToAction("CMNDashboard", "Home");
                }

                catch (Exception ex)
                {
                    if (_clsInvoi.Conn.State == ConnectionState.Open)
                    { _clsInvoi.Conn.Close(); }
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
        #endregion

        #region Function
        public void FillCustomerCombo(string ProgramType = "")
        {
            try
            {
                InvoiceTransactionModel _objSubModel = new InvoiceTransactionModel();
                if (ProgramType == CONT.FType)
                {
                    _objSubModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                }
                else if (ProgramType == CONT.RType)
                {
                    _objSubModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);
                }
                var DDLCustomerIDData = _clsInvoi.InvoiceMaster_BindCompany(_objSubModel).OrderBy(x => x.CompanyName).ToList();
                if (DDLCustomerIDData != null && DDLCustomerIDData.Count > 0)
                    ViewBag.DDLCustomerID = DDLCustomerIDData;
                else
                    ViewBag.DDLCustomerID = new List<SelectListItem> { };
            }
            catch (Exception ex)
            {
                ViewBag.DDLCustomerID = new List<SelectListItem> { };
                throw ex;
            }
        }
        public void FillCurrencyCombo(string ProgramType = "")
        {
            try
            {
                InvoiceTransactionModel _objSubModel = new InvoiceTransactionModel();
                if (ProgramType == CONT.FType)
                {
                    _objSubModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                }
                else if (ProgramType == CONT.RType)
                {
                    _objSubModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);
                }
                var DDLCurrencyData = _clsInvoi.InvoiceMaster_BindCurrency(_objSubModel).OrderBy(x => x.CurrencyCode).ToList();
                if (DDLCurrencyData != null && DDLCurrencyData.Count > 0)
                    ViewBag.DDLCurrencyID = DDLCurrencyData;
                else
                    ViewBag.DDLCurrencyID = new List<SelectListItem> { };
            }
            catch (Exception ex)
            {
                ViewBag.DDLCurrencyID = new List<SelectListItem> { };
                throw ex;
            }
        }

        protected void sendNotification(int InvoiceID, string tempName = "")
        {
            string sub = "";
            string toMail = "";
            DataTable DT = new DataTable();
            DT = _clsInvoi.InvoiceHTMLDetail(InvoiceID);
            toMail = Convert.ToString(DT.Rows[0]["ExporterEmailID"]);
            fn.GetMasterData(tempName, DT, toMail, sub, "", 0, InvoiceID);
        }
        public string _RightsNoaccessPage()
        {
            string MenuId = Convert.ToString(Session["MenuID"]);
            int ParentMenuID = 0;
            int.TryParse(MenuId, out ParentMenuID);
            var UserRight = fn.CheckUserRight("", "", ParentMenuID);
            if (UserRight != null)
            {
                return UserRight.MenuName;
            }
            else
            {
                //return null; **Dashboard changes
                return "";
            }
        }
        public string _currentStatus(string ProgramType = "")
        {
            string _status = "";
            string MenuId = Convert.ToString(Session["MenuID"]);
            int ParentMenuID = 0;
            int.TryParse(MenuId, out ParentMenuID);
            var UserRight = fn.CheckUserRight("", "", ParentMenuID);
            if (UserRight != null)
            {
                if (ProgramType == CONT.RType)
                {
                    if (UserRight.MenuName == CONT.mBuyer_Maker || UserRight.MenuName == CONT.mBuyer_Viewer) { _status = CONT.Buyer_Maker; }
                    else if (UserRight.MenuName == CONT.mBuyer_Checker) { _status = CONT.Buyer_Checker; }
                    else if (UserRight.MenuName == CONT.mIscf_Maker || UserRight.MenuName == CONT.mIscf_Viewer) { _status = CONT.rIscf_Maker; }
                    else if (UserRight.MenuName == CONT.mIscf_Checker) { _status = CONT.rIscf_Checker; }
                    else if (UserRight.MenuName == CONT.mInsurance_Maker || UserRight.MenuName == CONT.mInsurance_Viewer) { _status = CONT.rInsurance_Maker; }
                    else if (UserRight.MenuName == CONT.mInsurance_Checker) { _status = CONT.rInsurance_Checker; }
                    else if (UserRight.MenuName == CONT.mFunder_Checker || UserRight.MenuName == CONT.mFunder_Viewer) { _status = CONT.rFunder_Checker; }
                }
                else
                {
                    if (UserRight.MenuName == CONT.mSupplier_Maker || UserRight.MenuName == CONT.mSupplier_Viewer) { _status = CONT.Supplier_Maker; }
                    else if (UserRight.MenuName == CONT.mSupplier_Checker) { _status = CONT.Supplier_Checker; }
                    else if (UserRight.MenuName == CONT.mIscf_Maker || UserRight.MenuName == CONT.mIscf_Viewer) { _status = CONT.Iscf_Maker; }
                    else if (UserRight.MenuName == CONT.mIscf_Checker) { _status = CONT.Iscf_Checker; }
                    else if (UserRight.MenuName == CONT.mObligor_Maker || UserRight.MenuName == CONT.mObligor_Viewer) { _status = CONT.Obligor_Maker; }
                    else if (UserRight.MenuName == CONT.mObligor_Checker) { _status = CONT.Obligor_Checker; }
                    else if (UserRight.MenuName == CONT.mInsurance_Maker || UserRight.MenuName == CONT.mInsurance_Viewer) { _status = CONT.Insurance_Maker; }
                    else if (UserRight.MenuName == CONT.mInsurance_Checker) { _status = CONT.Insurance_Checker; }
                    else if (UserRight.MenuName == CONT.mFunder_Checker || UserRight.MenuName == CONT.mFunder_Viewer) { _status = CONT.Funder_Checker; }
                }

            }

            return _status;
        }
        #endregion
    }
}