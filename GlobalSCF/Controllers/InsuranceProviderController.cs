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
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;

namespace TMP.Controllers
{
    public class InsuranceProviderController : Controller
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
        ServiceProviderModel _Model = new ServiceProviderModel();
        ClsCompanyTypeMaster _clsCompType = new ClsCompanyTypeMaster();
        ClsServiceProvider _ClsServiceProvider = new ClsServiceProvider();


        ClsUserMaster _ClsUserMaster = new ClsUserMaster();
        UserMaster_ListAll_Result _objUserMaster = new UserMaster_ListAll_Result();
        UserRoleLinkMaster_ListAll_Result _objRolelink = new UserRoleLinkMaster_ListAll_Result();

        ServiceProviderModel _ObjServiceProvider = new ServiceProviderModel();
        #endregion

        #region Index Method
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
                    _objModel.CustomerTypeIDs = CONT.InsuranceProviderTypeID + "," + CONT.InsuranceBrokerTypeID;
                    _objModel.Status = status;
                    if (status == "" || UserRight.MenuName == "Viewer")
                    {
                        var Data = _ClsServiceProvider.CustomerMaster_ListAll(_objModel);
                        return View(Data);
                    }
                    else
                    {
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
        public ActionResult CreateNew()
        {
            return RedirectToAction("CompanyInfo", "InsuranceProvider");
        }
        [HttpPost]
        public ActionResult Index(mCustomerMasterModel _objModel)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    return RedirectToAction("CompanyInfo", "InsuranceProvider");
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
                    return RedirectToAction("CompanyInfo", "InsuranceProvider");
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
        #endregion

        #region Company Info Method
        public ActionResult CompanyInfo(int CustomerID = 0, bool IsEditProfile = false)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _Model.CustomerID = CustomerID;
                    _Model.Status = "";
                    ViewBag.IsUpdate = false;
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
                        _Model.CompanyTypeName = Data.CompanyTypeName;
                        _Model.CompanyTypeID = Data.CompanyTypeID;
                        _Model.OtherCompanyType = Data.OtherCompanyType;
                        _Model.CountryID = Data.CountryID; _Model.GetCityID = Data.CityID;
                        _Model.CityID = Data.CityID; _Model.TelNoInfo = Data.TelNo;
                        _Model.Website = Data.Website; _Model.CompanyTypeName = Data.CompanyTypeName;
                        _Model.OtherCity = Data.OtherCity;
                        ViewBag.CustomerTypeID = Data.CustomerTypeID;
                        ViewBag.IsCompPubList = Data.IsCompPubList;
                        ViewBag.IsUpdate = true;
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
        public ActionResult CompanyInfo(ServiceProviderModel _objModel, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string RdoIP = Convert.ToString(frm["RdoIP"]);
                    if (RdoIP != null) { _objModel.CustomerTypeID = CONT.InsuranceProviderTypeID; }

                    string RdoIB = Convert.ToString(frm["RdoIB"]);
                    if (RdoIB != null) { _objModel.CustomerTypeID = CONT.InsuranceBrokerTypeID; }

                    string ChkYes = Convert.ToString(frm["ChkYes"]);
                    if (ChkYes != null) { _objModel.IsCompPubList = true; }

                    string ChkNo = Convert.ToString(frm["ChkNo"]);
                    if (ChkNo != null) { _objModel.IsCompPubList = false; }

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
                    return RedirectToAction("AuthSignDetails", "InsuranceProvider", new { CustomerID = _objModel.CustomerID, CustomerTypeID = _objModel.CustomerTypeID });
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
        #endregion

        #region User Master
        public ActionResult CreateUser(int CustomerID = 0, int CustomerTypeID = 0, int EditID = 0, int DeleteID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string strRoleIDs = "";
                    FillRoleCombo(); FillCustomerCombo(); FillDocumentCombo(); FillDesignationCombo();
                    FillGenderCombo(); FillCountryCombo();
                    if (EditID > 0)
                    {
                        _objUserMaster.UserID = EditID; _objRolelink.UserID = EditID; _objUserMaster.CustomerID = CustomerID;
                        _objUserMaster.IsActive = true;
                        _objUserMaster = _ClsUserMaster.UserMasterHistory_ListAllBind(_objUserMaster).FirstOrDefault();
                        var Data = _ClsUserMaster.UserRoleLinkMaster_ListAll(_objRolelink).ToList();

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
                        _ClsUserMaster.conn = ClsAppDatabase.GetCon();
                        if (_ClsUserMaster.conn.State == ConnectionState.Closed) _ClsUserMaster.conn.Open();
                        else
                        { _ClsUserMaster.conn.Close(); _ClsUserMaster.conn.Open(); }
                        _ClsUserMaster.tras = _ClsUserMaster.conn.BeginTransaction();

                        _objUserMaster.UserID = DeleteID; _objUserMaster.IsActive = true;
                        var GetUserStatus = _ClsUserMaster.UserMaster_ListAll(_objUserMaster).FirstOrDefault();
                        string status = GetUserStatus.Status;

                        Boolean IsRequird = false; _objUserMaster.IsNoRequired = IsRequird;
                        if (status == CONT.NA || status == CONT.UM || status == CONT.UA)
                            _objUserMaster.IsNoRequired = true;

                        _objUserMaster.UserID = DeleteID;
                        int intDeleteID = _ClsUserMaster.UserMaster_Delete(_objUserMaster);
                        _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();

                        return RedirectToAction("CreateUser", "InsuranceProvider", new { CustomerID = CustomerID, CustomerTypeID = CustomerTypeID });
                    }

                    _objUserMaster.CustomerID = CustomerID; _objUserMaster.CustomerTypeID = CustomerTypeID;
                    _ObjServiceProvider.CustomerID = CustomerID; _ObjServiceProvider.CustomerTypeID = CustomerTypeID;
                    var GetCompanyName = _ClsServiceProvider.CustomerMaster_ListAll(_ObjServiceProvider).FirstOrDefault();

                    _objUserMaster.CompanyName = GetCompanyName.CompanyName;
                    return View(_objUserMaster);
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
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
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string strRoleIDs = "";
                if (Request["RoleIDs"] != null) { strRoleIDs = Request["RoleIDs"]; }

                try
                {
                    _ClsUserMaster.conn = ClsAppDatabase.GetCon(); if (_ClsUserMaster.conn.State == ConnectionState.Closed) { _ClsUserMaster.conn.Open(); }
                    else { _ClsUserMaster.conn.Close(); _ClsUserMaster.conn.Open(); }
                    _ClsUserMaster.tras = _ClsUserMaster.conn.BeginTransaction();

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
                            _ClsUserMaster.UserMasterHistory_Add(mUserMaster); RoleMaster_AddDelete(mUserMaster.UserID, CONT.UR);
                        }
                        else
                        {
                            mUserMaster.Status = CONT.RQ;
                            _ClsUserMaster.UserMaster_Update(mUserMaster); RoleMaster_AddDelete(mUserMaster.UserID, CONT.RQ);
                        }
                    }
                    else
                    {
                        mUserMaster.Status = CONT.RQ;
                        mUserMaster.UserID = _ClsUserMaster.UserMaster_Add(mUserMaster);
                        if (mUserMaster.UserID > 0)
                            RolLinkMaster_Add(mUserMaster.UserID, CONT.RQ);
                    }
                    _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();

                    mUserMaster.RoleIDs = strRoleIDs;
                    if (Request["createsave"] != null)
                        return RedirectToAction("CreateUser", "InsuranceProvider", new { CustomerID = mUserMaster.CustomerID, CustomerTypeID = mUserMaster.CustomerTypeID });
                    else if (Request["UserNextSave"] != null)
                        return RedirectToAction("Documents", "InsuranceProvider", new { CustomerID = mUserMaster.CustomerID });
                }
                catch (Exception ex)
                {
                    _ClsUserMaster.tras.Rollback(); _ClsUserMaster.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillRoleCombo(); FillCustomerCombo(); FillDocumentCombo(); FillDesignationCombo();
                FillGenderCombo(); FillCountryCombo();
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
                    var GetData = _ClsUserMaster.UserRoleLinkMaster_ListAll(_objRolelink).ToList();
                    if (GetData != null && GetData.Count > 0)
                    {
                        for (int i = 0; i < GetData.Count; i++)
                        {
                            _objRolelink.UserRoleLinkID = GetData[i].UserRoleLinkID;
                            _ClsUserMaster.UserRoleLinkMaster_Delete(_objRolelink);
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
                        getid = _ClsUserMaster.UserRoleLinkMaster_Add(_objRolelink);
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
                        getid = _ClsUserMaster.UserRoleLinkMaster_Add(_objRolelink);
                    }
                }
            }
        }
        public PartialViewResult GridUserMaster(int CustomerID = 0)
        {
            _objUserMaster.CustomerID = CustomerID; _objUserMaster.Status = CONT.mkstatus;
            var Data = _ClsUserMaster.UserMasterHistory_ListAllBind(_objUserMaster).ToList();
            return PartialView(Data);
        }
        #endregion

        #region Auth Sign Details Method
        public ActionResult AuthSignDetails(int CustomerID = 0, int CDid = 0, int CAid = 0, int did = 0, string CustType = "", int CustomerTypeID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
                try
                {
                    _objModel.CustomerID = CustomerID;
                    _objModel.CustomerTypeID = CustomerTypeID;
                    if (_objModel.CustomerID > 0)
                    {
                        _objModel.CustType = "D,A";
                        var authCount = _clsCustomer.CustomerShareHoldDetail_ListAll(_objModel).ToList();

                    }
                    if (CDid > 0)
                    {
                        _objModel.CustomerShareHoldID = CDid;
                        _objModel.CustType = CustType;
                        ViewBag.CustType = CustType;
                        _objModel = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        _objModel.GetRoleIDs = _objModel.RoleIDs;
                    }
                    else if (did > 0)
                    {
                        _clsCustomer.Conn = ClsAppDatabase.GetCon();
                        if (_clsCustomer.Conn.State == ConnectionState.Closed) { _clsCustomer.Conn.Open(); } else { _clsCustomer.Conn.Close(); _clsCustomer.Conn.Open(); }
                        _clsCustomer.Tras = _clsCustomer.Conn.BeginTransaction();
                        _objModel.CustomerShareHoldID = did;
                        _clsCustomer.CustomerShareHoldDetail_Delete(_objModel);
                        _clsCustomer.Tras.Commit();
                        _clsCustomer.Conn.Close();
                        return RedirectToAction("AuthSignDetails", "InsuranceProvider", new { CustomerID = _objModel.CustomerID });
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo();
                FillDesignationCombo();
                FillRoleCombo();
                FillGenderCombo();
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
            string[] LoginStatus = FN.Checkcredentials();
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

                    string selRoles = Request.Form["RoleIDs"];

                    if (!string.IsNullOrEmpty(selRoles))
                    {
                        _objModel.RoleIDs = selRoles;
                    }

                    _clsCustomer.Conn = ClsAppDatabase.GetCon();
                    if (_clsCustomer.Conn.State == ConnectionState.Closed) { _clsCustomer.Conn.Open(); } else { _clsCustomer.Conn.Close(); _clsCustomer.Conn.Open(); }
                    _clsCustomer.Tras = _clsCustomer.Conn.BeginTransaction();
                    if (_objModel.CustomerShareHoldID > 0)
                    {
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                        {
                            _objModel.Status = CONT.UR;
                            _clsCustomer.CustomerShareHoldDetailHistory_Add(_objModel);
                        }
                        else
                        {
                            _clsCustomer.CustomerShareHoldDetail_Update(_objModel);
                        }
                    }
                    else
                    {
                        _objModel.Status = CONT.RQ;
                        _clsCustomer.CustomerShareHoldDetail_Add(_objModel);
                    }
                    _clsCustomer.Tras.Commit();
                    _clsCustomer.Conn.Close();
                    if (Request["Save"] != null)
                    {
                        return RedirectToAction("AuthSignDetails", "InsuranceProvider", new { CustomerID = _objModel.CustomerID });
                    }
                    else if (Request["AuthNextSave"] != null)
                    {
                        if (_objModel.CustomerTypeID == CONT.InsuranceBrokerTypeID)
                            return RedirectToAction("CreateUser", "InsuranceProvider", new { CustomerID = _objModel.CustomerID, CustomerTypeID = _objModel.CustomerTypeID });
                        else
                            return RedirectToAction("Documents", "InsuranceProvider", new { CustomerID = _objModel.CustomerID });
                    }
                }

                catch (Exception ex)
                {
                    if (_clsCustomer.Conn.State == ConnectionState.Open)
                    { _clsCustomer.Conn.Close(); }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public PartialViewResult GridDirectorDetails(int CustomerID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.CustType = "A,D";
            var Data = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult GridAuthSignDetails(int CustomerID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.CustType = "A,D";
            var Data = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        [HttpPost]
        public JsonResult GetAuthSignDetails(int CustomerAuthSignID)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerShareHoldID = CustomerAuthSignID;
            _objModel.CustType = "D,A";
            var result = _clsCustomer.CustomerShareHoldDetail_ListAll(_objModel);
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        #region Shareholders Method
        public ActionResult Shareholders(int CustomerID = 11, int CIid = 0, int CCid = 0, int CBid = 0, int did = 0, string compIndi = "")
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
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
                        _objModel = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        ViewBag.compIndi = "Indi";
                    }
                    else if (CCid > 0)
                    {
                        _objModel.CustomerShareHoldID = CCid;
                        _objModel = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                        _objModel.CompCustomerShareHoldID = CCid;
                        _objModel.CompSharePer = _objModel.SharePer;
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
                        _clsCustomer.Conn = ClsAppDatabase.GetCon();
                        if (_clsCustomer.Conn.State == ConnectionState.Closed) { _clsCustomer.Conn.Open(); } else { _clsCustomer.Conn.Close(); _clsCustomer.Conn.Open(); }
                        _clsCustomer.Tras = _clsCustomer.Conn.BeginTransaction();
                        _objModel.CustomerShareHoldID = did;
                        _clsCustomer.CustomerShareHoldDetail_Delete(_objModel);
                        _clsCustomer.Tras.Commit();
                        _clsCustomer.Conn.Close();
                        return RedirectToAction("Shareholders", "InsuranceProvider", new { CustomerID = _objModel.CustomerID });
                    }
                }
                catch (Exception ex)
                {
                    if (_clsCustomer.Conn.State == ConnectionState.Open)
                    { _clsCustomer.Conn.Close(); }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                _objModel.CustType = "D,A";
                ViewBag.GetAuthSignCombo = new SelectList(_clsCustomer.CustomerShareHoldDetail_ListAll(_objModel).OrderBy(x => x.FirstName).ToList(), "CustomerShareHoldID", "FirstName", _objModel.CustomerShareHoldID);
                FillCountryCombo(); FillGenderCombo();
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult Shareholders(CustomerShareHolderModel _objModel, HttpPostedFileBase UpdPassport, HttpPostedFileBase UpdNatIden, HttpPostedFileBase ShareUpdPassport, HttpPostedFileBase ShareUpdNatIden, HttpPostedFileBase UpdTradeLic)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _clsCustomer.Conn = ClsAppDatabase.GetCon();
                    if (_clsCustomer.Conn.State == ConnectionState.Closed) { _clsCustomer.Conn.Open(); } else { _clsCustomer.Conn.Close(); _clsCustomer.Conn.Open(); }
                    _clsCustomer.Tras = _clsCustomer.Conn.BeginTransaction();
                    if (Request["Save"] != null)
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
                    else if (Request["parSave"] != null)
                    {
                        string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                        if (ShareUpdPassport != null)
                        {
                            if (ShareUpdPassport.FileName != null)
                            {
                                _objModel.AuthUpdPassport = newfilenm + ShareUpdPassport.FileName.ToString();
                                ShareUpdPassport.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.ShareUpdPassport));
                            }
                        }
                        string newfilenm1 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (ShareUpdNatIden != null)
                        {
                            if (ShareUpdNatIden.FileName != null)
                            {
                                _objModel.AuthUpdPOA = newfilenm1 + ShareUpdNatIden.FileName.ToString();
                                ShareUpdNatIden.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.ShareUpdNatIden));
                            }
                        }
                        string newfilenm2 = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdTradeLic != null)
                        {
                            if (UpdTradeLic.FileName != null)
                            {
                                _objModel.AuthUpdPOA = newfilenm2 + UpdTradeLic.FileName.ToString();
                                UpdTradeLic.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdTradeLic));
                            }
                        }
                        if (_objModel.ParentID == 0)
                        {
                            _objModel.SharePer = _objModel.CompSharePer;
                            _objModel.Status = CONT.RQ;
                            _objModel.CustType = "C";
                            int parentId = _clsCustomer.CustomerShareHoldDetail_Add(_objModel);
                            _objModel.ParentID = parentId;
                        }

                        _objModel.CustType = "B";
                        _objModel.CustomerShareHoldID = _objModel.ShareCustomerShareHoldID;
                        _objModel.FirstName = _objModel.ShareFirstName; _objModel.MiddleName = _objModel.ShareMiddleName;
                        _objModel.LastName = _objModel.ShareLastName; _objModel.NatCountryID = _objModel.ShareNatCountryID;
                        _objModel.PassNo = _objModel.SharePassNo; _objModel.UpdPassport = _objModel.ShareUpdPassport;
                        _objModel.UpdNatIden = _objModel.ShareUpdNatIden; _objModel.PassIssueDate = _objModel.SharePassIssueDate;
                        _objModel.PassCountryID = _objModel.SharePassCountryID;
                        _objModel.PassExpDate = _objModel.SharePassExpDate; _objModel.SharePer = _objModel.childSharePer;

                    }
                    else if (Request["ComSaveAdd"] != null || Request["ComNextSave"] != null)
                    {
                        string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                        if (UpdTradeLic != null)
                        {
                            if (UpdTradeLic.FileName != null)
                            {
                                _objModel.AuthUpdPOA = newfilenm + UpdTradeLic.FileName.ToString();
                                UpdTradeLic.SaveAs(Server.MapPath("\\Files\\Upload\\" + _objModel.UpdTradeLic));
                            }
                        }
                        _objModel.SharePer = _objModel.CompSharePer;
                        _objModel.CustType = "C";
                        _objModel.CustomerShareHoldID = _objModel.CompCustomerShareHoldID;
                        _objModel.ParentID = 0;
                    }

                    if (_objModel.CustomerShareHoldID > 0)
                    {
                        if (_objModel.Status == CONT.NA || _objModel.Status == CONT.UR || _objModel.Status == CONT.UA)
                        {
                            _objModel.Status = CONT.UR;
                            _clsCustomer.CustomerShareHoldDetailHistory_Add(_objModel);
                        }
                        else
                        {
                            _clsCustomer.CustomerShareHoldDetail_Update(_objModel);
                        }
                    }
                    else
                    {
                        _objModel.Status = CONT.RQ;
                        _clsCustomer.CustomerShareHoldDetail_Add(_objModel);
                    }
                    _clsCustomer.Tras.Commit();
                    _clsCustomer.Conn.Close();

                    if (Request["parSave"] != null)
                    {
                        return RedirectToAction("Shareholders", "InsuranceProvider", new { CustomerID = _objModel.CustomerID, CCid = _objModel.ParentID, compIndi = "Comp" });
                    }
                    else if (Request["NextSave"] != null)
                    {
                        return RedirectToAction("Documents", "InsuranceProvider", new { CustomerID = _objModel.CustomerID });
                    }
                    else if (Request["Save"] != null)
                    {
                        return RedirectToAction("Shareholders", "InsuranceProvider", new { CustomerID = _objModel.CustomerID, compIndi = "Indi" });
                    }
                    else if (Request["ComSaveAdd"] != null)
                    {
                        return RedirectToAction("Shareholders", "InsuranceProvider", new { CustomerID = _objModel.CustomerID, compIndi = "Comp" });
                    }

                }

                catch (Exception ex)
                {
                    if (_clsCustomer.Conn.State == ConnectionState.Open)
                    { _clsCustomer.Conn.Close(); }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillCountryCombo(); FillGenderCombo();
                _objModel.CustType = "D,A";
                ViewBag.GetAuthSignCombo = new SelectList(_clsCustomer.CustomerShareHoldDetail_ListAll(_objModel).OrderBy(x => x.FirstName).ToList(), "CustomerAuthSignID", "FullName", _objModel.CustomerShareHoldID);
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
            var Data = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public ActionResult ShareholdersBeneficial(int CustomerID = 0, int ParentID = 0, int CBid = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            FillCountryCombo();
            if (CBid > 0)
            {
                _objModel.CustomerShareHoldID = CBid;
                _objModel = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel).FirstOrDefault();

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
        public PartialViewResult GridShareholdersBeneficial(int CustomerID = 0, int ParentID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.ParentID = ParentID;
            _objModel.CustType = "B";
            var Data = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult GridShareholdersCompany(int CustomerID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.CustType = "C";
            var Data = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        public PartialViewResult GridShareholdersCompIndiPop(int CustomerID = 0, int ParentID = 0)
        {
            CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
            _objModel.CustomerID = CustomerID;
            _objModel.Status = CONT.mkstatus;
            _objModel.ParentID = ParentID;
            _objModel.CustType = "B";
            var Data = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);
            return PartialView(Data);
        }
        #endregion

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
                        return RedirectToAction("Index", "InsuranceProvider", new { Tid = 0, temp_name = CONT.Thanku_3rd });
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
                    return RedirectToAction("Documents", "InsuranceProvider", new { CustomerID = _objModel.CustomerID });
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
                    return RedirectToAction("Documents", "InsuranceProvider", new { CustomerID = _objModel.CustomerID, ErrorMsg = strErrorMsg });
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        #endregion

        #region Viewer Method
        public ActionResult CompanyInfoView(int CustomerID, int CustomerTypeID = 0, bool IsEditProfile = false)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (IsEditProfile || Convert.ToBoolean(Session["IsEditProfile"]))
                {
                    Session["IsEditProfile"] = true;
                }
                else { Session["IsEditProfile"] = false; }
                _Model.CustomerID = CustomerID;
                _Model.Status = "";
                if (_Model.CustomerID > 0)
                {
                    _Model = _ClsServiceProvider.CustomerMasterHistory_ListAllBind(_Model).FirstOrDefault();
                    var Data = _ClsServiceProvider.CustomerInfoDetailHistory_ListAllBind(_Model).FirstOrDefault();
                    _Model.CustomerInfoID = Data.CustomerInfoID;
                    _Model.TradeLicNo = Data.TradeLicNo; _Model.TradeLicNo = Data.TradeLicNo;
                    _Model.LicExpDate = Data.LicExpDate; _Model.IncCountryID = Data.IncCountryID;
                    _Model.IncCountryName = Data.IncCountryName;
                    _Model.IncDate = Data.IncDate; _Model.Address = Data.Address;
                    _Model.CountryID = Data.CountryID; _Model.GetCityID = Data.CityID;
                    _Model.CityID = Data.CityID; _Model.TelNoInfo = Data.TelNo;
                    _Model.CountryName = Data.CountryName; _Model.CityName = Data.CityName;
                    _Model.Website = Data.Website; _Model.CompanyTypeName = Data.CompanyTypeName;
                    _Model.OtherCity = Data.OtherCity;
                    ViewBag.CustomerID = _Model.CustomerID; _Model.OtherCompanyType = Data.OtherCompanyType;
                    ViewBag.CustomerTypeID = _Model.CustomerTypeID;
                    ViewBag.IsCompPubList = Data.IsCompPubList;

                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
            return View(_Model);
        }
        public ActionResult CommonDetails(int CustomerID = 0, int CustomerTypeID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerShareHolderModel _objModel = new CustomerShareHolderModel();
                string currentStatus = "";
                try
                {
                    _objModel.CustomerID = CustomerID;

                    if (_objModel.CustomerID > 0)
                    {
                        _objModel.CustomerID = CustomerID;
                        _objModel.CustType = "D,A";
                        _objModel.Status = CONT.NA;
                        ViewBag.DirAuthSignCheck = _clsCustomer.CustomerShareHoldDetailHistory_ListAll(_objModel);

                        _objModel.Status = currentStatus;
                        ViewBag.DirAuthSignGrid = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                        //_objModel.CustType = "A";
                        //_objModel.Status = CONT.NA;
                        //ViewBag.AuthSignCheck = _clsCustomer.CustomerShareHoldDetailHistory_ListAll(_objModel);

                        //_objModel.Status = currentStatus;
                        //ViewBag.AuthSignGrid = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                        _objModel.CustType = "S";
                        _objModel.Status = CONT.NA;
                        ViewBag.SharHoldCheck = _clsCustomer.CustomerShareHoldDetailHistory_ListAll(_objModel);

                        _objModel.Status = currentStatus;
                        ViewBag.SharHoldGrid = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                        _objModel.CustType = "C";
                        _objModel.Status = CONT.NA;
                        ViewBag.CompSharHoldCheck = _clsCustomer.CustomerShareHoldDetailHistory_ListAll(_objModel);

                        _objModel.Status = currentStatus;
                        ViewBag.CompSharHoldGrid = _clsCustomer.CustomerShareHoldDetailHistory_ListAllBind(_objModel);

                        //if (ViewBag.AuthSignCheck.Count == 0) { ViewBag.AuthSignCheck = ViewBag.AuthSignGrid; }
                        if (ViewBag.DirAuthSignCheck.Count == 0) { ViewBag.DirAuthSignCheck = ViewBag.DirAuthSignGrid; }
                        if (ViewBag.SharHoldCheck.Count == 0) { ViewBag.SharHoldCheck = ViewBag.SharHoldGrid; }
                        if (ViewBag.CompSharHoldCheck.Count == 0) { ViewBag.CompSharHoldCheck = ViewBag.CompSharHoldGrid; }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                ViewBag.CustomerTypeID = CustomerTypeID;
                ViewBag.CustomerID = CustomerID;
                _objModel.CustomerID = CustomerID;
                _objModel.CustomerTypeID = CustomerTypeID;
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult ViewUserMaster(int CustomerID = 0, int CustomerTypeID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (CustomerID > 0)
                    {
                        _objUserMaster.CustomerID = CustomerID;
                        var Data = _ClsUserMaster.UserMasterHistory_ListAllBind(_objUserMaster).ToList();
                        ViewBag.UserMasterData = Data;
                        ViewBag.CustomerTypeID = CustomerTypeID;
                        ViewBag.CustomerID = CustomerID;
                        _objUserMaster.CustomerID = CustomerID;
                        _objUserMaster.CustomerTypeID = CustomerTypeID;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
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
        public ActionResult DocumentsDetails(int CustomerID = 0, int CustomerTypeID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                CustomerDocModel _objModel = new CustomerDocModel();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                    //ViewBag.IsCheckerOrApprover = "Checker";
                    if (UserRight != null)
                    {
                        ViewBag.IsCheckerOrApprover = UserRight.MenuName;
                    }
                    _Model.CustomerID = CustomerID;
                    var strcurrentStatus = _ClsServiceProvider.CustomerMasterHistory_ListAllBind(_Model).FirstOrDefault();

                    string currentStatus = strcurrentStatus.Status;

                    _objModel.Status = currentStatus;
                    _objModel.CustomerID = CustomerID;
                    _objModel.CustomerTypeID = CustomerTypeID;
                    _objModel.IsUserDocInt = -1;
                    _objModel.IsInvestorInt = 1;
                    _objModel.Status = CONT.NA;
                    ViewBag.DocCheck = _clsCustomer.CustomerDocDetailHistory_ListAll(_objModel).ToList();

                    _objModel.Status = currentStatus;
                    var attachList = _clsCustomer.CustomerDocDetailHistory_ListAllBind(_objModel).ToList();
                    ViewBag.attachList = attachList;

                    CustomerMasterModel _objCustModel = new CustomerMasterModel();
                    _objCustModel.CustomerID = CustomerID;
                    _objCustModel.Status = currentStatus;
                    var userAttach = _clsCustomer.CustomerMaster_BindAllDocument(_objCustModel);
                    ViewBag.userAttach = userAttach;
                    ViewData["CustomerID"] = CustomerID;
                }

                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                if (ViewBag.DocCheck.Count == 0) { ViewBag.DocCheck = ViewBag.attachList; }
                _objModel.CustomerID = CustomerID; _objModel.CustomerTypeID = CustomerTypeID;
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult DocumentsDetails(int CustomerID, FormCollection frm, CustomerDocModel _model)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _clsCustomer.Conn = ClsAppDatabase.GetCon();
                    if (_clsCustomer.Conn.State == ConnectionState.Closed) _clsCustomer.Conn.Open();
                    else { _clsCustomer.Conn.Close(); _clsCustomer.Conn.Open(); }
                    _clsCustomer.Tras = _clsCustomer.Conn.BeginTransaction();

                    string CurrentStatus = Convert.ToString(Request["Status"]);
                    string command = Convert.ToString(Request["command"]);
                    string Remarks = Convert.ToString(Request["Remarks"]);
                    string Comments = Convert.ToString(Request["Comments"]);
                    int CustomerTypeID = Convert.ToInt16(Request["CustomerTypeID"]);


                    CustomerMasterModel _CustomerMasterModel = new CustomerMasterModel();
                    _CustomerMasterModel.ProcessRemark = Remarks;
                    _CustomerMasterModel.CustomerID = CustomerID;
                    if (Request["command"] != null)
                    {
                        if (CurrentStatus == CONT.RQ || CurrentStatus == CONT.NM)
                            _CustomerMasterModel.Status = CONT.NM;
                        else if (CurrentStatus == CONT.NA || CurrentStatus == CONT.UM || CurrentStatus == CONT.UA || CurrentStatus == CONT.UJ)
                            _CustomerMasterModel.Status = CONT.UR;

                        _clsCustomer.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _clsCustomer.Tras.Commit(); _clsCustomer.Conn.Close();
                        return RedirectToAction("Index", "InsuranceProvider", new { Tid = 0, temp_name = CONT.Thanku_3rd });
                    }
                    else if (Request["btnCheckerReturn"] != null)
                    {
                        if (CurrentStatus == CONT.NR)
                            _CustomerMasterModel.Status = CONT.NM;
                        else if (CurrentStatus == CONT.UR)
                            _CustomerMasterModel.Status = CONT.UM;

                        _clsCustomer.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _clsCustomer.Tras.Commit(); _clsCustomer.Conn.Close();
                        return RedirectToAction("Index", "InsuranceProvider", new { status = CONT.Home_statusChecker1 });
                    }
                    else if (Request["btnCheckerSubmit"] != null)
                    {
                        if (CurrentStatus == CONT.NR)
                            _CustomerMasterModel.Status = CONT.NC;
                        else if (CurrentStatus == CONT.UR)
                            _CustomerMasterModel.Status = CONT.UC;

                        _clsCustomer.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _clsCustomer.Tras.Commit(); _clsCustomer.Conn.Close();
                        return RedirectToAction("Index", "InsuranceProvider", new { status = CONT.Home_statusChecker1 });
                    }
                    else if (Request["btnApproverReject"] != null)
                    {
                        if (CurrentStatus == CONT.NC)
                            _CustomerMasterModel.Status = CONT.NJ;
                        else if (CurrentStatus == CONT.UC)
                            _CustomerMasterModel.Status = CONT.UJ;

                        _clsCustomer.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _clsCustomer.Tras.Commit(); _clsCustomer.Conn.Close();
                        return RedirectToAction("Index", "InsuranceProvider", new { status = CONT.Home_statusApprover });
                    }
                    else if (Request["btnApprove"] != null)
                    {
                        if (CurrentStatus == CONT.NC)
                            _CustomerMasterModel.Status = CONT.NA;
                        else if (CurrentStatus == CONT.UC)
                            _CustomerMasterModel.Status = CONT.UA;

                        _clsCustomer.CustomerMaster_UpdateAllStatus(_CustomerMasterModel);
                        _clsCustomer.Tras.Commit(); _clsCustomer.Conn.Close();

                        #region Send Notification To User
                        if (_CustomerMasterModel.Status == CONT.NA || _CustomerMasterModel.Status == CONT.UA)
                        {
                            if (CustomerTypeID == CONT.InsuranceBrokerTypeID)
                                sendNotification(CustomerID, CONT.Email_3rd);

                            _objUserMaster.CustomerID = CustomerID; _objUserMaster.IsActive = true;
                            var Data = _ClsUserMaster.UserMaster_ListAll(_objUserMaster).ToList();
                            if (Data != null)
                            {
                                foreach (var item in Data)
                                {
                                    if (item.IsLoginMailSend == 0)
                                        sendNotificationToUser(item.UserID, CONT.SupplierEmail_2st);
                                }
                            }
                        }
                        #endregion

                        #region Send E-Mail
                        if (_CustomerMasterModel.Status == CONT.NA)
                        {
                            sendNotification(_CustomerMasterModel.CustomerID, CONT.Admin_Confirmation_Mail);
                        }
                        #endregion

                        return RedirectToAction("Index", "InsuranceProvider", new { status = CONT.Home_statusApprover });
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
            return View(_model);
        }
        #endregion

        #region Common Function
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
            FN.GetMasterData(tempName, DT, toMail, sub, "", UserID);
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
        public void FillCustomerCombo()
        {
            //ClsCustomerMaster _CustomerMaster = new ClsCustomerMaster();
            //var DDLCustomerIDData = _CustomerMaster.UserMaster_CustomerBindDetails(0, 0, CONT.ddstatus).OrderBy(x => x.CompanyName).ToList();
            //if (DDLCustomerIDData != null && DDLCustomerIDData.Count > 0)
            //    ViewBag.DDLCustomerID = DDLCustomerIDData;
            //else
            //    ViewBag.DDLCustomerID = new List<SelectListItem> { };
        }
        public void FillRoleCombo()
        {
            ClsRoleMaster dbR = new ClsRoleMaster();
            UserMaster_ListAll_Result obj = new UserMaster_ListAll_Result();
            var DDLRoleData = dbR.RoleMaster_ListAll(0, "", 1, CONT.ddstatus, false, "", CONT.InsuranceBrokerTypeID).OrderBy(x => x.RoleName).ToList();
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
        public void FillGenderCombo()
        {
            ViewBag.DDLGenderID = new List<SelectListItem> { new SelectListItem() { Text = "Male", Value = "Male" },
                                                     new SelectListItem() { Text = "Female", Value = "Female" } };
        }
        #endregion
    }
}