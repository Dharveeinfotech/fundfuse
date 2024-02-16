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
    public class ServiceProviderController : Controller
    {
        #region Local Variable
        ClsCountryMaster _clsContr = new ClsCountryMaster();
        ClsCustomerMaster _clsCustomer = new ClsCustomerMaster();
        ClsCurrency _clsCurry = new ClsCurrency();
        ClsCommon _clsComm = new ClsCommon();
        ClsBankMaster _clsBank = new ClsBankMaster();
        ClsCityMaster _clsCity = new ClsCityMaster();
        ClsDesignationMaster _clsDesig = new ClsDesignationMaster();
        Function FN = new Function();
        string mkstatus = CONT.mkstatus;
        ServiceProviderModel _Model = new ServiceProviderModel();
        ClsCompanyTypeMaster _clsCompType = new ClsCompanyTypeMaster();
        ClsServiceProvider _ClsServiceProvider = new ClsServiceProvider();
        ClsCustomerMaster _ClsCustomerMaster = new ClsCustomerMaster();
        #endregion
        [HttpPost]
        public ActionResult CreateNew()
        {
            return RedirectToAction("CompanyInfo", "ServiceProvider");
        }
        public ActionResult Index(string status = "")
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ServiceProviderModel _objModel = new ServiceProviderModel();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                    if (UserRight != null)
                    {
                        ViewBag.UserRight = UserRight.MenuName;                        
                    }

                    _objModel.CustomerID = 0;
                    _objModel.CustomerTypeID = CONT.ServiceProviderCustomerTypeID;
                    _objModel.Status = status;
                    if (status == "" || UserRight.MenuName == "Viewer")
                    {
                        var Data = _ClsServiceProvider.CustomerMaster_ListAll(_objModel);
                        return View(Data);
                    }
                    else {
                        var Data = _ClsServiceProvider.CustomerMasterHistory_ListAllBind(_objModel);
                        return View(Data);
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult Index(mCustomerMasterModel _objModel)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    return RedirectToAction("CompanyInfo", "ServiceProvider");
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult NewRegistration(mCustomerMasterModel _objModel)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    return RedirectToAction("CompanyInfo", "ServiceProvider");
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult CompanyInfo(int CustomerID = 0,bool IsEditProfile =false)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _Model.CustomerID = CustomerID;
                    _Model.CustomerTypeID = CONT.ServiceProviderCustomerTypeID;
                    _Model.Status = "";
                    if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                    {
                        Session["IsEditProfile"] = true;
                    }
                    else { Session["IsEditProfile"] = false; }
                    if (_Model.CustomerID > 0)
                    {
                        _Model = _ClsServiceProvider.CustomerMasterHistory_ListAllBind(_Model).FirstOrDefault();
                        var Data = _ClsServiceProvider.CustomerInfoDetailHistory_ListAllBind(_Model).FirstOrDefault();
                        _Model.CustomerInfoID = Data.CustomerInfoID;
                        _Model.TradeLicNo = Data.TradeLicNo; _Model.TradeLicNo = Data.TradeLicNo;
                        _Model.LicExpDate = Data.LicExpDate; _Model.IncCountryID = Data.IncCountryID;
                        _Model.IncDate = Data.IncDate; _Model.Address = Data.Address;
                        _Model.CountryID = Data.CountryID; _Model.GetCityID = Data.CityID;
                        _Model.CityID = Data.CityID; _Model.TelNoInfo = Data.TelNo;
                        _Model.CompanyTypeID = Data.CompanyTypeID;
                        _Model.OtherCompanyType = Data.OtherCompanyType;
                        _Model.Website = Data.Website; _Model.OtherCity = Data.OtherCity;
                    }
                }

                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                FillCountryCombo();
                FillCompanyTypeCombo();
                FillDesignationCombo();
                FillGenderCombo();
                return View(_Model);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult CompanyInfo(ServiceProviderModel _objModel)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _objModel.CustomerTypeID = CONT.ServiceProviderCustomerTypeID;

                    _ClsServiceProvider.Conn = ClsAppDatabase.GetCon();
                    if (_ClsServiceProvider.Conn.State == ConnectionState.Closed) _ClsServiceProvider.Conn.Open();
                    else { _ClsServiceProvider.Conn.Close(); _ClsServiceProvider.Conn.Open(); }
                    _ClsServiceProvider.Tras = _ClsServiceProvider.Conn.BeginTransaction();
                    if (_objModel.CustomerID > 0)
                    {
                        if (_objModel.Status == CONT.RQ || _objModel.Status == CONT.NM)
                        {
                            _ClsServiceProvider.CustomerMaster_Update(_objModel);
                            _ClsServiceProvider.CustomerInfoDetail_Update(_objModel);
                        }
                        else
                        {
                            _objModel.Status = CONT.UR;
                            _ClsServiceProvider.CustomerMasterHistory_Add(_objModel);
                            _ClsServiceProvider.CustomerInfoDetailHistory_Add(_objModel);
                        }
                    }
                    else
                    {
                        _objModel.CustomerID = _ClsServiceProvider.CustomerMaster_Add(_objModel);
                        _objModel.Status = CONT.RQ;
                        _objModel.CustomerInfoID = _ClsServiceProvider.CustomerInfoDetail_Add(_objModel);
                    }
                    _ClsServiceProvider.Tras.Commit(); _ClsServiceProvider.Conn.Close();
                    return RedirectToAction("BankAccountDetails", "ServiceProvider", new { CustomerID = _objModel.CustomerID });
                }

                catch (Exception ex)
                {
                    if (_ClsServiceProvider.Conn.State == ConnectionState.Open)
                        _ClsServiceProvider.Conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo();
                FillCompanyTypeCombo();
                FillDesignationCombo();
                FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult BankAccountDetails(int CustomerID = 0, int Cbid = 0, int did = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerBankAccDetail _objModel = new CustomerBankAccDetail();
                try
                {
                    _objModel.CustomerID = CustomerID;
                    if (Cbid > 0)
                    {
                        _objModel.CustomerBankAccID = Cbid;

                        _objModel = _ClsCustomerMaster.CustomerBankAccDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                    }
                    else if (did > 0)
                    {
                        _ClsCustomerMaster.Conn = ClsAppDatabase.GetCon();
                        if (_ClsCustomerMaster.Conn.State == ConnectionState.Closed) { _ClsCustomerMaster.Conn.Open(); }
                        else { _ClsCustomerMaster.Conn.Close(); _ClsCustomerMaster.Conn.Open(); }
                        _ClsCustomerMaster.Tras = _ClsCustomerMaster.Conn.BeginTransaction();
                        _objModel.CustomerBankAccID = did;
                        _ClsCustomerMaster.CustomerBankAccDetail_Delete(_objModel);
                        _ClsCustomerMaster.Tras.Commit(); _ClsCustomerMaster.Conn.Close();
                        return RedirectToAction("BankAccountDetails", "ServiceProvider", new { CustomerID = _objModel.CustomerID });
                    }
                }
                catch (Exception ex)
                {
                    if (_ClsCustomerMaster.Conn.State == ConnectionState.Open)
                    { _ClsCustomerMaster.Conn.Close(); }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                FillCountryCombo();
                FillCurrencyCombo();
                FillBankCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult BankAccountDetails(CustomerBankAccDetail _objModel)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _ClsCustomerMaster.Conn = ClsAppDatabase.GetCon();
                    if (_ClsCustomerMaster.Conn.State == ConnectionState.Closed) _ClsCustomerMaster.Conn.Open();
                    else { _ClsCustomerMaster.Conn.Close(); _ClsCustomerMaster.Conn.Open(); }
                    _ClsCustomerMaster.Tras = _ClsCustomerMaster.Conn.BeginTransaction();

                    if (_objModel.CustomerBankAccID > 0)
                    {
                        if (_objModel.Status == CONT.RQ || _objModel.Status == CONT.NM)
                        {
                            _ClsCustomerMaster.CustomerBankAccDetail_Update(_objModel);
                        }
                        else
                        {
                            _objModel.Status = CONT.UR;
                            _ClsCustomerMaster.CustomerBankAccDetailHistory_Add(_objModel);
                        }
                    }
                    else
                    {
                        _objModel.Status = CONT.RQ;
                        _ClsCustomerMaster.CustomerBankAccDetail_Add(_objModel);
                    }
                    _ClsCustomerMaster.Tras.Commit(); _ClsCustomerMaster.Conn.Close();
                    if (Request["Save"] != null)
                        return RedirectToAction("BankAccountDetails", "ServiceProvider", new { CustomerID = _objModel.CustomerID });
                    else if (Request["NextSave"] != null)
                        return RedirectToAction("Documents", "ServiceProvider", new { CustomerID = _objModel.CustomerID });
                }

                catch (Exception ex)
                {
                    if (_ClsCustomerMaster.Conn.State == ConnectionState.Open)
                        _ClsCustomerMaster.Conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                FillCountryCombo();
                FillCurrencyCombo();
                FillBankCombo();
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
            _objModel.CustomerID = CustomerID; _objModel.Status = mkstatus;
            //var Data = _ClsCustomerMaster.CustomerBankAccDetail_ListAll(_objModel);
            var Data = _ClsCustomerMaster.CustomerBankAccDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        #region Documents Method
        public ActionResult Documents(int CustomerID)
        {
            if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
            {
                ViewBag.ErrorMesssage = Request.QueryString["ErrorMsg"].ToString();
            }
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerDocModel _objModel = new CustomerDocModel();
                CustomerTypeDocumentDetails_ListAll_Result _objSubModel = new CustomerTypeDocumentDetails_ListAll_Result();
                try
                {
                    _Model.CustomerID = CustomerID;
                    var intCustomerTypeID = _ClsServiceProvider.CustomerMasterHistory_ListAllBind(_Model).FirstOrDefault();

                    _objSubModel.CustomerTypeID = intCustomerTypeID.CustomerTypeID;
                    _objSubModel.Status = CONT.ddstatus;
                    _objModel.CustomerID = CustomerID;
                    _objSubModel.IsActive = true;
                    _objModel.lstCustomerTypeDocumentList = _clsCustomer.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();

                    if (_objModel.CustomerID > 0)
                    {
                        _objModel.IsUserDocInt = -1; _objModel.IsInvestorInt = -1;
                        var attachList = _clsCustomer.CustomerDocDetail_ListAll(_objModel).ToList();
                        if (attachList.Count == 0)
                            _objModel.lstCustomerTypeDocumentList = _clsCustomer.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();
                        else
                        {
                            _objModel.lstCustomerDocDetail = _clsCustomer.CustomerDocDetailHistory_ListAllBind(_objModel).ToList();
                            ViewBag.Status = _objModel.lstCustomerDocDetail[0].Status;

                            _objModel.lstCustomerTypeDocumentList = _clsCustomer.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();
                            for (int i = 0; i < _objModel.lstCustomerDocDetail.Count; i++)
                            {
                                if (_objModel.lstCustomerDocDetail[i].Attachment != "")
                                    Session["hdAttchFileName_" + _objModel.lstCustomerDocDetail[i].DocumentID] = _objModel.lstCustomerDocDetail[i].Attachment;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _clsCustomer.Conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
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
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                _clsCustomer.Conn = ClsAppDatabase.GetCon();
                if (_clsCustomer.Conn.State == ConnectionState.Closed) _clsCustomer.Conn.Open();
                else { _clsCustomer.Conn.Close(); _clsCustomer.Conn.Open(); }
                _clsCustomer.Tras = _clsCustomer.Conn.BeginTransaction();
                _objModel.IsUserDocInt = -1; _objModel.IsInvestorInt = -1;
                var attaDocList = _clsCustomer.CustomerDocDetail_ListAll(_objModel).ToList();
                try
                {
                    if (Request["Submit"] != null)
                    {
                        string Status = Convert.ToString(Request["CuStatus"]);

                        CustomerMasterModel _UpdateModel = new CustomerMasterModel();

                        if (Status == CONT.NA)
                        { _UpdateModel.Status = CONT.UR; }
                        else if (Status == CONT.UA)
                        { _UpdateModel.Status = CONT.UR; }
                        else { _UpdateModel.Status = CONT.NR; }
                        _UpdateModel.CustomerID = _objModel.CustomerID;
                        _clsCustomer.CustomerMaster_UpdateAllStatus(_UpdateModel);

                        _clsCustomer.Tras.Commit(); _clsCustomer.Conn.Close();
                        return RedirectToAction("Index", "ServiceProvider", new { Tid = 0, temp_name = CONT.Thanku_3rd });
                    }

                    if (attaDocList.Count == 0)
                    {
                        int intMenuCount = Convert.ToInt32(Request.Form["intMenuCount"]);
                        for (int i = 0; i < intMenuCount; i++)
                        {
                            for (int j = 0; j < frm.Count; j++)
                            {
                                string GetAllkeysStringValue = frm.AllKeys[j];
                                string DocumentIDChar = Convert.ToString(frm["item.DocumentID"]);
                                var ID2 = DocumentIDChar.Split(',');
                                int DocumentID;
                                DocumentID = Convert.ToInt32(ID2[i]);
                                if (GetAllkeysStringValue.Equals("chk_" + DocumentID + ""))
                                {
                                    string IsUploadChar = Convert.ToString(frm["IsUpload"]);
                                    bool IsUpload = false;
                                    if (IsUploadChar != null)
                                    {
                                        var ID = IsUploadChar.Split(',');
                                        IsUpload = Convert.ToBoolean(ID[i]);
                                    }

                                    //Comment
                                    string CommentChar = Convert.ToString(frm["Comment"]);
                                    string Comment = "";
                                    if (CommentChar != null)
                                    {
                                        var ID4 = CommentChar.Split(',');
                                        Comment = Convert.ToString(ID4[i]);
                                    }
                                    var path = "";
                                    HttpFileCollectionBase files = Request.Files;
                                    var fileName = "";
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
                                    _objModel.DocumentID = DocumentID; _objModel.IsUpload = IsUpload; _objModel.Comment = Comment;
                                    _objModel.Attachment = FileLocation; _objModel.Status = CONT.RQ;
                                    _clsCustomer.CustomerDocDetail_Add(_objModel);
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
                                    int CustomerDocID = 0;
                                    for (int p = 0; p < attaDocList.Count; p++)
                                    {
                                        CustomerDocModel obj = (CustomerDocModel)attaDocList[p];
                                        if (DocumentID == obj.DocumentID)
                                        {
                                            CustomerDocID = obj.CustomerDocID;
                                        }
                                    }


                                    bool IsUpload = false;
                                    string IsUploadChar = Convert.ToString(frm["item1.IsUpload"] + "," + frm["IsUpload"]);
                                    if (IsUploadChar != null)
                                    {
                                        var ID = IsUploadChar.Split(',');
                                        if (ID[i] != "0")
                                            IsUpload = Convert.ToBoolean(ID[i]);
                                        else
                                            IsUpload = false;
                                    }

                                    //Comment
                                    string CommentChar = Convert.ToString(frm["item1.Comment"] + "," + frm["Comment"]);
                                    string Comment = "";
                                    if (CommentChar != null)
                                    {
                                        var ID4 = CommentChar.Split(',');
                                        Comment = Convert.ToString(ID4[i]);
                                    }

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

                                    _objModel.DocumentID = DocumentID; _objModel.IsUpload = IsUpload; _objModel.Comment = Comment;
                                    _objModel.Attachment = fileName; _objModel.CustomerDocID = CustomerDocID;
                                    if (Convert.ToInt32(CustomerDocID) > 0)
                                    {
                                        if (_objModel.Status == CONT.RQ || _objModel.Status == CONT.NM)
                                        {
                                            _clsCustomer.CustomerDocDetail_Update(_objModel);
                                        }
                                        else
                                        {
                                            _objModel.Status = CONT.UR;
                                            _clsCustomer.CustomerDocDetailHistory_Add(_objModel);
                                        }
                                    }
                                    else
                                    {
                                        _objModel.Status = CONT.RQ;
                                        _clsCustomer.CustomerDocDetail_Add(_objModel);
                                    }
                                }
                            }
                        }
                    }

                    _clsCustomer.Tras.Commit(); _clsCustomer.Conn.Close();
                    return RedirectToAction("Documents", "ServiceProvider", new { CustomerID = _objModel.CustomerID });
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (_clsCustomer.Conn.State == ConnectionState.Open)
                    {
                        _clsCustomer.Tras.Rollback();
                        _clsCustomer.Conn.Close();
                    }
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else ModelState.AddModelError("", ex.InnerException.Message);

                    string strErrorMsg = "";
                    if (ex.InnerException == null)

                        strErrorMsg = ex.Message;

                    else
                        strErrorMsg = ex.InnerException.Message;
                    CustomerTypeDocumentDetails_ListAll_Result _objSubModel = new CustomerTypeDocumentDetails_ListAll_Result();
                    _objSubModel.CustomerTypeID = CONT.ServiceProviderCustomerTypeID;

                    _objModel.lstCustomerTypeDocumentList = _clsCustomer.CustomerTypeDocumentDetails_ListAll(_objSubModel).ToList();
                    return RedirectToAction("Documents", "ServiceProvider", new { CustomerID = _objModel.CustomerID, ErrorMsg = strErrorMsg });
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        #endregion
        public ActionResult MakerDetails(int CustomerID, bool IsEditProfile=false)
        {
            ViewData["CustomerID"] = CustomerID;
            if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
            {
                ViewBag.ErrorMsg = Request.QueryString["ErrorMsg"].ToString();
            }
            try
            {
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                {
                    Session["IsEditProfile"] = true;
                }
                else { Session["IsEditProfile"] = false; }
                string MenuId = Convert.ToString(Session["MenuID"]);
                int ParentMenuID = 0;
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                    if (UserRight != null)
                    {
                        ViewBag.IsCheckerOrApprover = UserRight.MenuName;
                    }
                    ServiceProviderModel _ServiceProviderModel = new ServiceProviderModel();
                    _ServiceProviderModel.CustomerID = CustomerID;
                    _ServiceProviderModel.CustomerTypeID = CONT.ServiceProviderCustomerTypeID;
                    var CustomerMasterData = _ClsServiceProvider.CustomerMasterHistory_ListAllBind(_ServiceProviderModel).FirstOrDefault();
                    _ServiceProviderModel = CustomerMasterData;
                    ViewBag.mCustomerMaster = CustomerMasterData;
                    _ServiceProviderModel.Status = CustomerMasterData.Status;

                    var Data = _ClsServiceProvider.CustomerInfoDetailHistory_ListAllBind(_ServiceProviderModel).FirstOrDefault();
                    _ServiceProviderModel.TradeLicNo = Data.TradeLicNo; _ServiceProviderModel.TradeLicNo = Data.TradeLicNo;
                    _ServiceProviderModel.LicExpDate = Data.LicExpDate; _ServiceProviderModel.IncCountryID = Data.IncCountryID;
                    _ServiceProviderModel.CompanyTypeName = Data.CompanyTypeName;
                    _ServiceProviderModel.OtherCompanyType = Data.OtherCompanyType;
                    _ServiceProviderModel.IncCountryName = Data.IncCountryName;
                    _ServiceProviderModel.LicExpDate1 = Convert.ToDateTime(Data.LicExpDate);
                    _ServiceProviderModel.IncDate = Data.IncDate;
                    _ServiceProviderModel.IncDate1 = Convert.ToDateTime(Data.IncDate);
                    _ServiceProviderModel.Address = Data.Address;
                    _ServiceProviderModel.CountryID = Data.CountryID; _ServiceProviderModel.GetCityID = Data.CityID;
                    _ServiceProviderModel.CountryName = Data.CountryName; _ServiceProviderModel.CityName = Data.CityName;
                    _ServiceProviderModel.CityID = Data.CityID; _ServiceProviderModel.TelNoInfo = Data.TelNo;
                    _ServiceProviderModel.Website = Data.Website; _ServiceProviderModel.OtherCity = Data.OtherCity;

                    CustomerDocModel _CustomerDocModel = new CustomerDocModel();
                    _CustomerDocModel.CustomerID = CustomerID; _CustomerDocModel.Status = mkstatus;
                    _CustomerDocModel.CustomerTypeID = CONT.ServiceProviderCustomerTypeID;
                    _CustomerDocModel.IsUserDocInt = -1; _CustomerDocModel.IsInvestorInt = -1;
                    ViewBag.sCustomerDocDetail = _ClsCustomerMaster.CustomerDocDetailHistory_ListAllBind(_CustomerDocModel).ToList();

                    CustomerBankAccDetail _CustomerBankAccDetail = new CustomerBankAccDetail();
                    _CustomerBankAccDetail.CustomerID = CustomerID; _CustomerBankAccDetail.Status = mkstatus;
                    ViewBag.bankdata = _ClsCustomerMaster.CustomerBankAccDetailHistory_ListAllBind(_CustomerBankAccDetail).ToList();

                    ViewData["PageStatus"] = "Maker";
                    ViewData["CustomerID"] = CustomerID;

                    return View(_ServiceProviderModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
            }
            return View();
        }
        [HttpPost]
        public ActionResult MakerDetails(int CustomerID, FormCollection frm)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ClsCustomerMaster.Conn = ClsAppDatabase.GetCon();
                    if (_ClsCustomerMaster.Conn.State == ConnectionState.Closed) _ClsCustomerMaster.Conn.Open();
                    else { _ClsCustomerMaster.Conn.Close(); _ClsCustomerMaster.Conn.Open(); }
                    _ClsCustomerMaster.Tras = _ClsCustomerMaster.Conn.BeginTransaction();

                    string CurrentStatus = Convert.ToString(Request["Status"]);
                    string command = Convert.ToString(Request["command"]);
                    string Remarks = Convert.ToString(Request["Remarks"]);
                    string Comments = Convert.ToString(Request["Comments"]);
                    
                    CustomerMasterModel _CustomerMasterModel = new CustomerMasterModel();
                    _CustomerMasterModel.ProcessRemark = Remarks;
                    _CustomerMasterModel.CustomerID = CustomerID;
                    if (Request["command"] != null)
                    {
                        if (CurrentStatus == CONT.RQ || CurrentStatus == CONT.NM)
                            _CustomerMasterModel.Status = CONT.NM;
                        else if (CurrentStatus == CONT.NA || CurrentStatus == CONT.UM || CurrentStatus == CONT.UA || CurrentStatus == CONT.UJ)
                            _CustomerMasterModel.Status = CONT.UR;

                        _ClsCustomerMaster.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _ClsCustomerMaster.Tras.Commit(); _ClsCustomerMaster.Conn.Close();
                        return RedirectToAction("Index", "ServiceProvider", new { Tid = 0, temp_name = CONT.Thanku_3rd });
                    }
                    else if (Request["btnCheckerReturn"] != null)
                    {
                        if (CurrentStatus == CONT.NR)
                            _CustomerMasterModel.Status = CONT.NM;
                        else if (CurrentStatus == CONT.UR)
                            _CustomerMasterModel.Status = CONT.UM;

                        _ClsCustomerMaster.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _ClsCustomerMaster.Tras.Commit(); _ClsCustomerMaster.Conn.Close();
                        return RedirectToAction("Index", "ServiceProvider", new { status = CONT.Home_statusChecker1 });
                    }
                    else if (Request["btnCheckerSubmit"] != null)
                    {
                        if (CurrentStatus == CONT.NR)
                            _CustomerMasterModel.Status = CONT.NC;
                        else if (CurrentStatus == CONT.UR)
                            _CustomerMasterModel.Status = CONT.UC;

                        _ClsCustomerMaster.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _ClsCustomerMaster.Tras.Commit(); _ClsCustomerMaster.Conn.Close();
                        return RedirectToAction("Index", "ServiceProvider", new { status = CONT.Home_statusChecker1 });
                    }
                    else if (Request["btnApproverReject"] != null)
                    {
                        if (CurrentStatus == CONT.NC)
                            _CustomerMasterModel.Status = CONT.NJ;
                        else if (CurrentStatus == CONT.UC)
                            _CustomerMasterModel.Status = CONT.UJ;

                        _ClsCustomerMaster.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _ClsCustomerMaster.Tras.Commit(); _ClsCustomerMaster.Conn.Close();
                        return RedirectToAction("Index", "ServiceProvider", new { status = CONT.Home_statusApprover });
                    }
                    else if (Request["btnApprove"] != null)
                    {
                        if (CurrentStatus == CONT.NC)
                            _CustomerMasterModel.Status = CONT.NA;
                        else if (CurrentStatus == CONT.UC)
                            _CustomerMasterModel.Status = CONT.UA;

                        _ClsCustomerMaster.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _ClsCustomerMaster.Tras.Commit(); _ClsCustomerMaster.Conn.Close();

                        #region Send E-Mail
                        if (_CustomerMasterModel.Status == CONT.NA)
                        {
                            sendNotification(_CustomerMasterModel.CustomerID, CONT.Admin_Confirmation_Mail);
                        }
                        #endregion

                        return RedirectToAction("Index", "ServiceProvider", new { status = CONT.Home_statusApprover });
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                if (ex.InnerException != null)
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                else
                    ViewBag.ErrorMsg = ex.Message;
            }
            return View();
        }
        protected void sendNotification(int Tid, string tempName = "", string toMail = "")
        {
            string sub = "";
            DataTable DT = new DataTable();
            CustomerMasterModel _objModel = new CustomerMasterModel();
            ClsCustomerMaster _ClsCustomerMaster = new ClsCustomerMaster();
            _objModel.CustomerID = Tid;
            DT = _ClsCustomerMaster.CustomerMaster_ListAllTable(_objModel);            
            FN.GetMasterData(tempName, DT, DT.Rows[0]["EmailID"].ToString(), sub);
        }

        [HttpPost]
        public JsonResult GetCityData(int CountryID)
        {
            ClsServiceProvider _ClsServiceProvider = new ClsServiceProvider();
            ServiceProviderModel _objModel = new ServiceProviderModel();
            _objModel.StateID = CountryID;
            var result = _ClsServiceProvider.CityMaster_BindCityName(_objModel).ToList();
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

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
        public void FillDesignationCombo()
        {
            var DDLDesignationData = _clsDesig.DesignationMaster_ListAll(0, "", 1, "", false, "").OrderBy(x => x.DesignationName).ToList();
            if (DDLDesignationData != null && DDLDesignationData.Count > 0)
                ViewBag.DDLDesignationID = DDLDesignationData;
            else
                ViewBag.DDLDesignationID = new List<SelectListItem> { };
        }
        public void FillGenderCombo()
        {
            ViewBag.DDLGenderID = new List<SelectListItem> { new SelectListItem() { Text = "Male", Value = "Male" },
                                                     new SelectListItem() { Text = "Female", Value = "Female" } };
        }
    }
}