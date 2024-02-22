using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TMP.DAL;
using TMP.Models;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;
using System.Web;

namespace TMP.Controllers
{

    public class mUserMastersController : Controller
    {
        #region Local Variable
        Function FN = new Function();
        string ddstatus = CONT.ddstatus;

        ClsUserMaster _ClsUserMaster = new ClsUserMaster();
        mUserDeptDetail _mUserDeptDetail = new mUserDeptDetail();
        mCustomerMasterModel _mCustomerMasterModel = new mCustomerMasterModel();
        ClsCountryMaster _clsCon = new ClsCountryMaster();
        UserMaster_ListAll_Result _objUserMaster = new UserMaster_ListAll_Result();
        UserRoleLinkMaster_ListAll_Result _objRolelink = new UserRoleLinkMaster_ListAll_Result();
        ClsDesignationMaster _clsDesig = new ClsDesignationMaster();
        #endregion

        [HttpGet]
        public ActionResult OpenNewWin()
        {
            return View();
        }
        public ActionResult Login(string ReturnMessages)
        {
            if (Session["LoginDevice"] != null)
            {
                ViewBag.LoginDevice = Convert.ToString(Session["LoginDevice"]);
                ViewBag.UserName = Convert.ToString(Session["UserName"]);
            }

            ClearSession();
            //string cLoginName = FN.CheckForCookieUserName();
            UserMaster_ListAll_Result userlog = new UserMaster_ListAll_Result();
            //if (!string.IsNullOrEmpty(cLoginName))
            //{
            //    userlog.LoginName = cLoginName; userlog.RememberMe = true;
            //}

            if (!string.IsNullOrEmpty(ReturnMessages))
            {
                ViewBag.ErrorV = "error";
                ModelState.AddModelError("", ReturnMessages);
            }

            CountryMaster _objModel = new CountryMaster();
            _objModel = _clsCon.SystemPerameter_ListAll().FirstOrDefault();
            if (_objModel != null)
            {
                if (_objModel.IsMaintenance == true)
                {
                    ClearSession();
                    return RedirectToAction("UnderDevelopment", "MasterPage");
                }
            }
            return View(userlog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(UserMaster_ListAll_Result userLog)
        {
            ClsUserMaster dbh = new ClsUserMaster();
            UserLoginHistory_ListAll_Result OTPNUmber = new UserLoginHistory_ListAll_Result();
            ObjectParameter OutCustomer = new ObjectParameter("pIsCustomer", typeof(bool));
            ObjectParameter OutUserLoginHistoryID = new ObjectParameter("pUserLoginHistoryID", typeof(int));
            Function fn = new Function();
            ClsCustomerMaster objcm = new ClsCustomerMaster();
            try
            {
                dbh.conn = ClsAppDatabase.GetCon();
                if (dbh.conn.State == ConnectionState.Closed)
                { dbh.conn.Close(); }
                else { dbh.conn.Close(); }
                DataSet ds = new DataSet();
                ds = dbh.UserMaster_VerifyUser(userLog.LoginName, userLog.Password);
                dbh.conn = ClsAppDatabase.GetCon();
                if (dbh.conn.State == ConnectionState.Closed)
                { dbh.conn.Open(); }
                else { dbh.conn.Close(); dbh.conn.Open(); }
                dbh.tras = dbh.conn.BeginTransaction();

                if (ds != null)
                {
                    bool IsNew = false; bool IsResetPwd = false; bool IsRole = false; string MobileNo = string.Empty; int SiteIdleSessionMin = 0;
                    DataTable mUserMaster = new DataTable();
                    mUserMaster = ds.Tables["Table"];
                    if (mUserMaster.Rows.Count > 0)
                    {
                        Session["UserName"] = userLog.LoginName.ToString();
                        Session["DisplayName"] = ds.Tables["Table"].Rows[0]["ContactFullName"].ToString();
                        Session["UserId"] = ds.Tables["Table"].Rows[0]["UserID"].ToString();
                        Session["CustomerID"] = ds.Tables["Table"].Rows[0]["CustomerID"].ToString();
                        Session["ReminderCustID"] = ds.Tables["Table"].Rows[0]["CustomerID"].ToString();
                        Session["onlineCustomerTypeID"] = ds.Tables["Table"].Rows[0]["CustomerTypeID"].ToString();
                        Session["CustomerShareHoldID"] = ds.Tables["Table"].Rows[0]["CustomerShareHoldID"].ToString();
                        Session["CustomerStatus"] = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]);
                        Session["IsMenuPanelHide"] = Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsMenuPanelHide"]);
                        Session["AuthSignType"] = Convert.ToString(ds.Tables["Table"].Rows[0]["AuthSignType"]);
                        Session["IsAgree1st"] = Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAgree"]);
                        Session["LastLoginTime"] = Convert.ToDateTime(ds.Tables["Table"].Rows[0]["LastLoginTime"]);
                        MobileNo = ds.Tables["Table"].Rows[0]["MobileNo"].ToString();
                        IsNew = Convert.ToBoolean(ds.Tables["Table"].Rows[0]["AlreadyLogin"]);
                        DateTime PwdExpiryDate = Convert.ToDateTime(ds.Tables["Table"].Rows[0]["PwdExpiryDate"]);
                        Guid GU = Guid.NewGuid();
                        Session["LoginKey"] = GU.ToString();
                        Session["LoginMatch"] = GU.ToString();

                        SiteIdleSessionMin = Convert.ToInt32(ds.Tables["Table5"].Rows[0]["SiteIdleSessionMin"]);

                        DataTable mUserRoleLink = new DataTable();
                        mUserRoleLink = ds.Tables["Table1"];
                        if (mUserRoleLink.Rows.Count > 0)
                        {
                            string RoleId = string.Empty;
                            Session["RoleId"] = null;
                            for (int i = 0; i < mUserRoleLink.Rows.Count; i++)
                            {
                                if (i == 0)
                                    RoleId = Convert.ToString(ds.Tables["Table1"].Rows[i]["RoleID"]);
                                else
                                    RoleId += "," + Convert.ToString(ds.Tables["Table1"].Rows[i]["RoleID"]);
                            }
                            Session["RoleId"] = RoleId;
                            IsRole = true;
                        }

                        DataTable CustomerType = ds.Tables["Table3"];
                        if (CustomerType.Rows.Count > 0)
                        {
                            string CustomerTypeID = string.Empty;
                            Session["CustomerTypeID"] = null;
                            for (int i = 0; i < CustomerType.Rows.Count; i++)
                            {
                                if (i == 0)
                                    CustomerTypeID = Convert.ToString(ds.Tables["Table3"].Rows[i]["CustomerTypeID"]);
                                else
                                    CustomerTypeID += "," + Convert.ToString(ds.Tables["Table3"].Rows[i]["CustomerTypeID"]);
                            }
                            Session["CustomerTypeID"] = CustomerTypeID;
                            string SuperAdmin = string.Empty;
                            Session["SuperAdmin"] = null;
                            for (int i = 0; i < CustomerType.Rows.Count; i++)
                            {
                                if (Convert.ToInt32(ds.Tables["Table3"].Rows[i]["IsSuperAdmin"]) > 0)
                                    SuperAdmin = Convert.ToString(ds.Tables["Table3"].Rows[i]["IsSuperAdmin"]);
                            }
                            Session["SuperAdmin"] = Convert.ToString(SuperAdmin);
                        }
                        if (userLog.OTP == null)
                        {
                            userLog.UserID = Convert.ToInt32(Session["UserId"]); userLog.CurrentSessionID = Convert.ToString(Session["CurrentSessionID"]);
                            HttpBrowserCapabilitiesBase browse = Request.Browser;
                            userLog.Request = Request.UserAgent;
                            var platform = GetUserPlatform(); var browser = Request.Browser;
                            userLog.LoginDevice = "" + browser.Browser + " (" + platform + ")";
                            dbh.UserSessionHistory_Add(userLog);
                            dbh.tras.Commit();
                            dbh.conn.Close();
                        }
                        DataTable CheckResetPwd = ds.Tables["Table4"];
                        if (CheckResetPwd.Rows.Count > 0)
                        {
                            bool ResetPwd = false;
                            ResetPwd = Convert.ToBoolean(ds.Tables["Table4"].Rows[0]["IsResetPwd"]);
                            if (ResetPwd)
                            {
                                DateTime LoginDateTime = Convert.ToDateTime(ds.Tables["Table4"].Rows[0]["LoginDateTime"]);
                                DateTime Currentdatetime = System.DateTime.Now;
                                int min = Convert.ToInt32(Math.Floor((Currentdatetime - LoginDateTime).TotalMinutes));
                                return RedirectToAction("ChangePasswordExpDate", "mUserMasters");
                            }
                        }

                        int PwdExpiryDay = Convert.ToInt32(ds.Tables["Table5"].Rows[0]["PwdExpiryDay"]);
                        int days = 0;
                        days = FN.GetPasswordExpriyDay(PwdExpiryDate);
                        if (days > PwdExpiryDay)
                            return RedirectToAction("ChangePasswordExpDate", "mUserMasters");
                    }
                    if (IsNew)
                    {
                        string OTPNumber = string.Empty; int pUserID = 0; int.TryParse(Convert.ToString(Session["UserId"]), out pUserID);
                        if (userLog.OTP == null)
                        {
                            OTPNumber = FN.CreateRandomOTPNumber(6);
                            string strotpLive = string.Empty;
                            strotpLive = System.Configuration.ConfigurationManager.AppSettings["IsLiveOtp"];
                            if (strotpLive.ToLower() == "false")
                                FN.SendOTPNumberandPassword("", OTPNumber, MobileNo);
                            dbh.conn = ClsAppDatabase.GetCon();
                            if (dbh.conn.State == ConnectionState.Closed)
                            { dbh.conn.Open(); }
                            else { dbh.conn.Close(); dbh.conn.Open(); }
                            dbh.tras = dbh.conn.BeginTransaction();

                            dbh.UserLoginHistory_Add(OutUserLoginHistoryID, pUserID, userLog.LoginName.ToString(), userLog.Password, OTPNumber, true,
                                false, "Success Send OTP", FN.GetSystemIP());
                            dbh.tras.Commit();
                            dbh.conn.Close();

                            ViewBag.OTPNumber = true; userLog.OTP = string.Empty;
                            strotpLive = System.Configuration.ConfigurationManager.AppSettings["IsLiveOtp"];
                            if (strotpLive.ToLower() == "true")
                                userLog.OTP = OTPNumber;
                            return View(userLog);
                        }
                        else
                        {
                            OTPNUmber = dbh.UserLoginHistory_OTPNumber(pUserID).FirstOrDefault();
                            string LoginName = string.Empty;
                            string Password = string.Empty;
                            string OTP = string.Empty;
                            if (OTPNUmber != null)
                            {
                                LoginName = OTPNUmber.LoginName; Password = OTPNUmber.Password; OTP = OTPNUmber.OTP;
                                DateTime LoginDateTime = OTPNUmber.LoginDateTime; DateTime Currentdatetime = System.DateTime.Now;
                                int min = Convert.ToInt32(Math.Abs((Currentdatetime - LoginDateTime).Minutes));
                                if (min < 5)
                                {
                                    if (!userLog.OTP.Equals(OTP))
                                    {
                                        ViewBag.ErrorV = "error";
                                        ModelState.AddModelError("", "Invalid OTP Number.");
                                        ViewBag.OTPNumber = true;
                                        return View(userLog);
                                    }
                                }
                                else
                                {
                                    userLog.OTP = string.Empty; ViewBag.OTPNumber = null;
                                    ModelState.AddModelError("", "Your OTP Number is expired kindly login again for new OTP");
                                    //return RedirectToAction("Login", "mUserMasters");
                                }
                            }
                        }
                        //if (userLog.RememberMe)
                        //    FN.CreateRememberMeCookie(userLog.LoginName);
                        //else
                        //    FN.RemoveRememberMeCookie();
                        dbh.conn = ClsAppDatabase.GetCon();
                        if (dbh.conn.State == ConnectionState.Closed)
                        { dbh.conn.Open(); }
                        else { dbh.conn.Close(); dbh.conn.Open(); }
                        dbh.tras = dbh.conn.BeginTransaction();

                        dbh.UserLoginHistory_Add(OutUserLoginHistoryID, Convert.ToInt32(Session["UserId"]), userLog.LoginName, userLog.Password, "", true,
                            false, "Success Login", fn.GetSystemIP());
                        dbh.tras.Commit();
                        dbh.conn.Close();

                        DataSet des = new DataSet();
                        string RoleId = Convert.ToString(Session["RoleId"]);
                        des = dbh.MenuRoleRights_ListAll_Result(RoleId, 0, 0, "");
                        DataView DV3 = des.Tables[0].DefaultView;
                        DV3.RowFilter = "MenuName like 'Administration'";
                        if (DV3.Count > 0)
                            Session["AdminPanel"] = Convert.ToString("1");

                        Infrastructure.Web.SessionManager.SetLoggedInUserSession(userLog.LoginName, mUserMaster);
                        Session.Timeout = SiteIdleSessionMin;
                        int intCustType = Convert.ToInt32(Session["onlineCustomerTypeID"]);
                        if (intCustType == CONT.SupplierCustomerTypeID || intCustType == CONT.ObligorCustomerTypeID ||
                            intCustType == CONT.FunderCustomerTypeID || intCustType == CONT.BuyerCustomerTypeID ||
                            intCustType == CONT.BothObligorAndBuyerTypeID || intCustType == CONT.InsuranceBrokerTypeID || intCustType == CONT.InsuranceProviderTypeID)
                        {

                            if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAuthorizedUser"]))
                            {
                                if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAuthorizedSuccess"].ToString()) == false)
                                {

                                    if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAgree"].ToString()) == false)
                                    {
                                        return RedirectToAction("RegistrationAgree", "OnlineReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]), AuthorizedUser = "Y", ProgramType = Convert.ToString(ds.Tables["Table"].Rows[0]["ProgramType"]) });
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.SupplierCustomerTypeID)
                                        {
                                            return RedirectToAction("SupplierAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                                        }
                                        if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.ObligorCustomerTypeID || Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BuyerCustomerTypeID
                                            || Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BothObligorAndBuyerTypeID)
                                        {
                                            return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]), ProgramType = Convert.ToString(ds.Tables["Table"].Rows[0]["ProgramType"]) });
                                        }
                                        if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.FunderCustomerTypeID)
                                        {
                                            return RedirectToAction("FunderAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                                        }
                                    }
                                }
                                else
                                {
                                    //return RedirectToAction("UserDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                    if (Convert.ToBoolean(Session["IsMenuPanelHide"]))
                                    {
                                        return RedirectToAction("BlankDashboard", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("CMNDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                    }

                                }
                            }
                            else
                            {
                                if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAgree"].ToString()) == false)
                                {
                                    return RedirectToAction("RegistrationAgree", "OnlineReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                                }
                                else
                                {
                                    if (Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) == CONT.RQ)
                                    {
                                        return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                    }
                                    //else if (Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) == CONT.NC || 
                                    //    Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) == CONT.NR)
                                    //{
                                    //    return RedirectToAction("CompanyInfoDetails", "CustomerReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                                    //}
                                    else
                                    {

                                        //return RedirectToAction("UserDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                        if (Convert.ToBoolean(Session["IsMenuPanelHide"]))
                                        {
                                            return RedirectToAction("BlankDashboard", "Home");
                                        }
                                        else
                                        {
                                            return RedirectToAction("CMNDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            return RedirectToAction("CMNDashboard", "Home");
                        }
                    }
                    else
                        return RedirectToAction("ChangePasswordExpDate", "mUserMasters");
                    //return RedirectToAction("ChangePasswordOTP", "mUserMasters", new { iTC = 1 });
                }
                return View(userLog);
            }
            catch (Exception Ex)
            {
                if (dbh.conn.State == ConnectionState.Open)
                { dbh.tras.Rollback(); dbh.conn.Close(); }
                string errorMessage = FN.CreateErrorMessage(Ex);
                FN.LogFileWrite(errorMessage);

                dbh.conn = ClsAppDatabase.GetCon();
                if (dbh.conn.State == ConnectionState.Closed)
                { dbh.conn.Open(); }
                else { dbh.conn.Close(); dbh.conn.Open(); }
                dbh.tras = dbh.conn.BeginTransaction();
                ViewBag.ErrorV = "error";
                if (Ex.InnerException == null)
                {
                    dbh.UserLoginHistory_Add(OutUserLoginHistoryID, 0, userLog.LoginName, userLog.Password, "", true, false, Ex.Message.ToString(), fn.GetSystemIP());
                    dbh.tras.Commit(); dbh.conn.Close();
                    ModelState.AddModelError("", Ex.Message.ToString());
                }
                else
                {
                    dbh.UserLoginHistory_Add(OutUserLoginHistoryID, 0, userLog.LoginName, userLog.Password, "", true, false, Ex.Message.ToString(), fn.GetSystemIP());
                    dbh.tras.Commit(); dbh.conn.Close();
                    ModelState.AddModelError("", Ex.InnerException.Message.ToString());
                }
                return View(userLog);
            }
        }

        #region User Master
        public ActionResult CreateUser(int UserID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string strRoleIDs = "";
                    FillRoleCombo(); FillCustomerCombo(); FillDocumentCombo(); FillDesignationCombo();

                    if (UserID > 0)
                    {
                        _objUserMaster.UserID = UserID; _objRolelink.UserID = UserID;
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
                    //if (DeleteID > 0)
                    //{
                    //    _ClsUserMaster.conn = ClsAppDatabase.GetCon();
                    //    if (_ClsUserMaster.conn.State == ConnectionState.Closed) _ClsUserMaster.conn.Open();
                    //    else
                    //    { _ClsUserMaster.conn.Close(); _ClsUserMaster.conn.Open(); }
                    //    _ClsUserMaster.tras = _ClsUserMaster.conn.BeginTransaction();

                    //    _objUserMaster.UserID = DeleteID; _objUserMaster.IsActive = true;
                    //    var GetUserStatus = _ClsUserMaster.UserMaster_ListAll(_objUserMaster).FirstOrDefault();
                    //    string status = GetUserStatus.Status;

                    //    Boolean IsRequird = false; _objUserMaster.IsNoRequired = IsRequird;
                    //    if (status == CONT.NA || status == CONT.UM || status == CONT.UA)
                    //        _objUserMaster.IsNoRequired = true;

                    //    _objUserMaster.UserID = DeleteID;
                    //    int intDeleteID = _ClsUserMaster.UserMaster_Delete(_objUserMaster);
                    //    _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();

                    //    //return RedirectToAction("CreateUser", "CustomerReg", new { CustomerID = CustomerID, CustomerTypeID = CustomerTypeID });
                    //}
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
                        if (mUserMaster.Status == CONT.NA || mUserMaster.Status == CONT.UM || mUserMaster.Status == CONT.UR || mUserMaster.Status == CONT.UA)
                        {
                            mUserMaster.Status = CONT.UR;
                            _ClsUserMaster.UserMasterHistory_Add(mUserMaster); RoleMaster_AddDelete(mUserMaster.UserID, CONT.UR);
                        }
                        else
                        {
                            if (mUserMaster.Status == CONT.NM)
                                mUserMaster.Status = CONT.NR;
                            _ClsUserMaster.UserMaster_Update(mUserMaster); RoleMaster_AddDelete(mUserMaster.UserID, CONT.NR);
                        }
                    }
                    else
                    {
                        mUserMaster.Status = CONT.NR;
                        mUserMaster.UserID = _ClsUserMaster.UserMaster_Add(mUserMaster);
                        if (mUserMaster.UserID > 0)
                            RolLinkMaster_Add(mUserMaster.UserID, CONT.NR);
                    }
                    _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();

                    mUserMaster.RoleIDs = strRoleIDs;
                    if (Request["createsave"] != null)
                        return RedirectToAction("Index", "mUserMasters");
                }
                catch (Exception ex)
                {
                    _ClsUserMaster.tras.Rollback(); _ClsUserMaster.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillRoleCombo(); FillCustomerCombo(); FillDocumentCombo(); FillDesignationCombo();
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
        public ActionResult UserDetailsView(int UserID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    ViewData["UserID"] = UserID;
                    if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
                    {
                        ViewBag.ErrorMsg = Request.QueryString["ErrorMsg"].ToString();
                    }
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0; string strRoleIDs = "";
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                    if (UserRight != null)
                    {
                        ViewBag.IsCheckerOrApprover = UserRight.MenuName;
                    }
                    _objUserMaster.UserID = UserID; _objRolelink.UserID = UserID;
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
                    FillRoleCombo(); FillCustomerCombo(); FillDocumentCombo(); FillDesignationCombo();
                    return View(_objUserMaster);
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                return View(_objUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult UserDetailsView(FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    _ClsUserMaster.conn = ClsAppDatabase.GetCon();
                    if (_ClsUserMaster.conn.State == ConnectionState.Closed) _ClsUserMaster.conn.Open();
                    else { _ClsUserMaster.conn.Close(); _ClsUserMaster.conn.Open(); }
                    _ClsUserMaster.tras = _ClsUserMaster.conn.BeginTransaction();

                    int UserID = Convert.ToInt16(Request["UserID"]);
                    string CurrentStatus = Convert.ToString(Request["Status"]);
                    string ProcessRemark = Convert.ToString(Request["ProcessRemark"]);

                    _objUserMaster.ProcessRemark = ProcessRemark;
                    _objUserMaster.UserID = UserID;

                    if (Request["btnCheckerReturn"] != null)
                    {
                        if (CurrentStatus == CONT.NR)
                            _objUserMaster.Status = CONT.NM;
                        else if (CurrentStatus == CONT.UR)
                            _objUserMaster.Status = CONT.UM;

                        _ClsUserMaster.UserMaster_UpdateAllStatus(_objUserMaster);
                        _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();
                        return RedirectToAction("CheckerApproverIndex", "mUserMasters", new { status = CONT.Document_Checker });
                    }
                    else if (Request["btnCheckerSubmit"] != null)
                    {
                        if (CurrentStatus == CONT.NR)
                            _objUserMaster.Status = CONT.NC;
                        else if (CurrentStatus == CONT.UR)
                            _objUserMaster.Status = CONT.UC;

                        _ClsUserMaster.UserMaster_UpdateAllStatus(_objUserMaster);
                        _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();
                        return RedirectToAction("CheckerApproverIndex", "mUserMasters", new { status = CONT.Document_Checker });
                    }
                    else if (Request["btnApproverReject"] != null)
                    {
                        if (CurrentStatus == CONT.NC)
                            _objUserMaster.Status = CONT.NJ;
                        else if (CurrentStatus == CONT.UC)
                            _objUserMaster.Status = CONT.UJ;

                        _ClsUserMaster.UserMaster_UpdateAllStatus(_objUserMaster);
                        _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();
                        return RedirectToAction("CheckerApproverIndex", "mUserMasters", new { status = CONT.User_Approve });
                    }
                    else if (Request["btnApprove"] != null)
                    {
                        if (CurrentStatus == CONT.NC)
                            _objUserMaster.Status = CONT.NA;
                        else if (CurrentStatus == CONT.UC)
                            _objUserMaster.Status = CONT.UA;

                        _ClsUserMaster.UserMaster_UpdateAllStatus(_objUserMaster);
                        _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();

                        return RedirectToAction("CheckerApproverIndex", "mUserMasters", new { status = CONT.User_Approve });
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                }
                FillRoleCombo(); FillCustomerCombo(); FillDocumentCombo(); FillDesignationCombo();
                return View(_objUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult CheckerApproverIndex(string status = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                    if (UserRight != null)
                    {
                        if (UserRight != null)
                        {
                            ViewBag.UserRight = UserRight.MenuName;
                        }
                        _objUserMaster.UserID = 0; _objUserMaster.Status = status;
                        _objUserMaster.IsActive = true;
                        if (status == "" || UserRight.MenuName == "Viewer")
                        {
                            var Data = _ClsUserMaster.UserMaster_ListAllNew(_objUserMaster);

                            ViewBag.StatusList = Data.Select(dataRow => new mRoleMaster
                            {
                                StatusUserDesc = dataRow.StatusUserDesc
                            }).GroupBy(model => model.StatusUserDesc).Select(group => group.First());
                            return View(Data);
                        }
                        else
                        {
                            var Data = _ClsUserMaster.UserMasterHistory_ListAllBindNew(_objUserMaster);

                            ViewBag.StatusList = Data.Select(dataRow => new mRoleMaster
                            {
                                StatusUserDesc = dataRow.StatusUserDesc
                            }).GroupBy(model => model.StatusUserDesc).Select(group => group.First());
                            return View(Data);
                        }
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);

                if (ex.InnerException == null)
                    ViewBag.ErrorMesssage = ex.Message;
                else
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            return View();
        }
        //public PartialViewResult GridUserMaster(int CustomerID = 0)
        //{
        //    _objUserMaster.CustomerID = CustomerID; _objUserMaster.Status = CONT.mkstatus;
        //    var Data = _ClsUserMaster.UserMasterHistory_ListAllBind(_objUserMaster).ToList();
        //    return PartialView(Data);
        //}
        //public ActionResult ViewUserMaster(int CustomerID = 0, int CustomerTypeID = 0)
        //{
        //    string[] LoginStatus = FN.Checkcredentials();
        //    if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
        //    {
        //        try
        //        {
        //            if (CustomerID > 0)
        //            {
        //                _objUserMaster.CustomerID = CustomerID; _objUserMaster.Status = currentStatus;
        //                var Data = _clsUser.UserMasterHistory_ListAllBind(_objUserMaster).ToList();
        //                ViewBag.UserMasterData = Data;
        //                ViewBag.CustomerTypeID = CustomerTypeID;
        //                ViewBag.CustomerID = CustomerID;
        //                _objUserMaster.CustomerID = CustomerID;
        //                _objUserMaster.CustomerTypeID = CustomerTypeID;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string errorMessage = fn.CreateErrorMessage(ex);
        //            fn.LogFileWrite(errorMessage);
        //            if (ex.InnerException == null) { ViewBag.ErrorMesssage = ex.Message; } else { ViewBag.ErrorMesssage = ex.InnerException.Message; }
        //        }
        //        ViewBag.CustomerTypeID = CustomerTypeID;
        //        ViewBag.CustomerID = CustomerID;
        //        _objUserMaster.CustomerID = CustomerID;
        //        _objUserMaster.CustomerTypeID = CustomerTypeID;
        //        return View(_objUserMaster);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "mUserMasters");
        //    }
        //}
        public void FillRoleCombo()
        {
            ClsRoleMaster dbR = new ClsRoleMaster();
            UserMaster_ListAll_Result obj = new UserMaster_ListAll_Result();
            int intCustType = Convert.ToInt32(Session["onlineCustomerTypeID"]);
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
        public void FillDesignationCombo()
        {
            var DDLDesignationData = _clsDesig.DesignationMaster_ListAll(0, "", 1, "", false, "").OrderBy(x => x.DesignationName).ToList();
            if (DDLDesignationData != null && DDLDesignationData.Count > 0)
                ViewBag.DDLDesignationID = DDLDesignationData;
            else
                ViewBag.DDLDesignationID = new List<SelectListItem> { };
        }
        #endregion
        public ActionResult LogOutUser()
        {
            ClearSession();
            return RedirectToAction("Login", "mUserMasters");
        }
        public void ClearSession()
        {
            Session["UserName"] = null;
            Session["DisplayName"] = null;
            Session["UserId"] = null;
            Session["LoginKey"] = null;
            Session["LoginMatch"] = null;
            Session["RoleId"] = null;
            Session["MenuID"] = null;
            Session["AddMenuID"] = null;
            Session["UpdateMenuID"] = null;
            Session["PageName"] = null;
            Session["CustomerTypeID"] = null;
            Session["CustomerID"] = null;
            Session["DAT"] = null;
            Session["TI"] = null;
            Session["AdminPanel"] = null;
            Session["SuperAdmin"] = null;
            Session["currentStatus"] = null;
            Session["onlineCustomerTypeID"] = null;
            Session["CustomerShareHoldID"] = null;
            Session["IsMenuPanelHide"] = null;
            Session["AuthSignType"] = null;
            Session["hideMsg"] = null;
            Session["CustomerStatus"] = null;
            Session["LastLoginTime"] = null;
            Session["LoginDevice"] = null;
            Infrastructure.Web.SessionManager.ClearSession();
        }
        //ChangePassword With OTPNumber
        public ActionResult ChangePasswordOTP(int? iTC)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                Session["bredCrum"] = "Change Password";
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                UserMaster_ListAll_Result userPass = new UserMaster_ListAll_Result();
                userPass.UserID = UserId;
                if (iTC == 1)
                {
                    string MessageText = string.Empty;
                    ClsHTMLTemplate dbHT = new ClsHTMLTemplate();
                    var TeText = dbHT.HTMLTemplate_ListAll(0, "User Login Details", 1, "", false, "", 0).FirstOrDefault();
                    MessageText = FN.SMSSendingProccess(UserId, TeText.HtmlText, 0);
                    var TempText = dbHT.HTMLTemplate_ListAll(0, "Terms-Conditions", 1, "", false, "", 0).FirstOrDefault();
                    if (!string.IsNullOrEmpty(TempText.HtmlText))
                        ViewBag.TermsConditions = TempText.HtmlText.ToString();
                    else
                        ViewBag.TermsConditions = null;
                }
                return View(userPass);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordOTP(UserMaster_ListAll_Result userPass)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                UserLoginHistory_ListAll_Result OTPNUmber = new UserLoginHistory_ListAll_Result();
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    OTPNUmber = db.UserLoginHistory_OTPNumber(UserId).FirstOrDefault();
                    string LoginName = string.Empty;
                    string Password = string.Empty;
                    string OTP = string.Empty;
                    bool IsResetPwd = false;
                        if (OTPNUmber != null)
                    {
                        LoginName = OTPNUmber.LoginName;
                        Password = OTPNUmber.Password;
                        OTP = OTPNUmber.OTP;
                        IsResetPwd = OTPNUmber.IsResetPwd;
                        DateTime LoginDateTime = OTPNUmber.LoginDateTime;
                        if (IsResetPwd)
                        {
                            DateTime Currentdatetime = System.DateTime.Now;
                            int min = Convert.ToInt32(Math.Floor((Currentdatetime - LoginDateTime).TotalMinutes));
                            if (min < 15)
                            {
                                if (userPass.OldPassword == Password && userPass.OPTNumber == OTP)
                                {
                                    db.conn = ClsAppDatabase.GetCon();
                                    if (db.conn.State == ConnectionState.Closed)
                                    { db.conn.Open(); }
                                    else { db.conn.Close(); db.conn.Open(); }
                                    db.tras = db.conn.BeginTransaction();

                                    Function fn = new Function();
                                    db.UserMaster_ChangePassword(UserId, userPass.OldPassword, userPass.NewPassword, UserId, fn.GetSystemIP());
                                    db.UserMaster_UpdateAgree(UserId, UserId, fn.GetSystemIP());

                                    ObjectParameter OutUserLoginHistoryID = new ObjectParameter("pUserLoginHistoryID", typeof(int));
                                    db.UserLoginHistory_Add(OutUserLoginHistoryID, UserId, LoginName, Password, "", true, false, "Success Login", fn.GetSystemIP());
                                    db.tras.Commit(); db.conn.Close(); ClearSession();
                                    return RedirectToAction("Login", "mUserMasters", new { ReturnMessages = "Your password has been changed. Kindly re-login" });
                                }
                                else
                                {
                                    string MessageText = string.Empty;
                                    ClsHTMLTemplate dbHT = new ClsHTMLTemplate();
                                    var TempText = dbHT.HTMLTemplate_ListAll(0, "Terms-Conditions", 1, "", false, "", 0).FirstOrDefault();
                                    if (!string.IsNullOrEmpty(TempText.HtmlText))
                                        ViewBag.TermsConditions = TempText.HtmlText.ToString();
                                    else
                                        ViewBag.TermsConditions = null;
                                    ModelState.AddModelError("", "Invalid OTPNumber/Password");
                                }
                            }
                            else
                            { return RedirectToAction("ForgotPassword", "mUserMasters", new { ReturnMessages = 1 }); }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    db.tras.Rollback(); db.conn.Close();

                    string errorMessage = FN.CreateErrorMessage(Ex);
                    FN.LogFileWrite(errorMessage);

                    if (Ex.InnerException == null)
                        ModelState.AddModelError("", Ex.Message);
                    else
                        ModelState.AddModelError("", Ex.InnerException.Message);
                    ViewBag.TermsConditions = null;
                }
                return View(userPass);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        //ChangePassword
        public ActionResult ChangePassword()
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                Session["bredCrum"] = "Change Password";
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                UserMaster_ListAll_Result userPass = new UserMaster_ListAll_Result();
                userPass.UserID = UserId;
                return View(userPass);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UserMaster_ListAll_Result userPass)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                try
                {
                    db.conn = ClsAppDatabase.GetCon();
                    if (db.conn.State == ConnectionState.Closed)
                    { db.conn.Open(); }
                    else { db.conn.Close(); db.conn.Open(); }
                    db.tras = db.conn.BeginTransaction();

                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    Function fn = new Function();
                    db.UserMaster_ChangePassword(UserId, userPass.OldPassword, userPass.NewPassword, UserId, fn.GetSystemIP());
                    db.tras.Commit();
                    db.conn.Close();

                    //ClearSession();
                    //return RedirectToAction("Login", "mUserMasters", new { ReturnMessages = "Your password has been changed. Kindly re-login" });

                    DataSet des = new DataSet();
                    string RoleId = Convert.ToString(Session["RoleId"]);
                    des = db.MenuRoleRights_ListAll_Result(RoleId, 0, 0, "");
                    DataView DV3 = des.Tables[0].DefaultView;
                    DV3.RowFilter = "MenuName like 'Administration'";
                    if (DV3.Count > 0)
                        Session["AdminPanel"] = Convert.ToString("1");

                    userPass.LoginName = Convert.ToString(Session["UserName"]);
                    DataSet ds = new DataSet();
                    ds = db.UserMaster_VerifyUser(userPass.LoginName, userPass.NewPassword);
                    int intCustType = Convert.ToInt32(Session["onlineCustomerTypeID"]);
                    if (intCustType == CONT.SupplierCustomerTypeID || intCustType == CONT.ObligorCustomerTypeID ||
                        intCustType == CONT.FunderCustomerTypeID || intCustType == CONT.BuyerCustomerTypeID ||
                        intCustType == CONT.BothObligorAndBuyerTypeID || intCustType == CONT.InsuranceBrokerTypeID || intCustType == CONT.InsuranceProviderTypeID)
                    {

                        if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAuthorizedUser"]))
                        {
                            if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAuthorizedSuccess"].ToString()) == false)
                            {

                                if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAgree"].ToString()) == false)
                                {
                                    return RedirectToAction("RegistrationAgree", "OnlineReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]), AuthorizedUser = "Y", ProgramType = Convert.ToString(ds.Tables["Table"].Rows[0]["ProgramType"]) });
                                }
                                else
                                {
                                    if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.SupplierCustomerTypeID)
                                    {
                                        return RedirectToAction("SupplierAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                                    }
                                    if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.ObligorCustomerTypeID || Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BuyerCustomerTypeID
                                        || Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BothObligorAndBuyerTypeID)
                                    {
                                        return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]), ProgramType = Convert.ToString(ds.Tables["Table"].Rows[0]["ProgramType"]) });
                                    }
                                    if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.FunderCustomerTypeID)
                                    {
                                        return RedirectToAction("FunderAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToBoolean(Session["IsMenuPanelHide"]))
                                {
                                    return RedirectToAction("BlankDashboard", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("CMNDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                }

                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAgree"].ToString()) == false)
                            {
                                return RedirectToAction("RegistrationAgree", "OnlineReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) == CONT.RQ)
                                {
                                    return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                }

                                else
                                {
                                    if (Convert.ToBoolean(Session["IsMenuPanelHide"]))
                                    {
                                        return RedirectToAction("BlankDashboard", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("CMNDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (intCustType == CONT.ServiceProviderCustomerTypeID)
                        {
                            return RedirectToAction("CMNDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                        }
                    }
                }
                catch (Exception Ex)
                {
                    db.tras.Rollback(); db.conn.Close();

                    string errorMessage = FN.CreateErrorMessage(Ex);
                    FN.LogFileWrite(errorMessage);

                    if (Ex.InnerException == null)
                        ModelState.AddModelError("", Ex.Message);
                    else
                        ModelState.AddModelError("", Ex.InnerException.Message);
                }
                return View(userPass);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        //ChangePasswordExpDate
        public ActionResult ChangePasswordExpDate()
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                Session["bredCrum"] = "Change Password";
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                UserMaster_ListAll_Result userPass = new UserMaster_ListAll_Result();
                userPass.UserID = UserId;
                Session["hideMsg"] = true;
                return View(userPass);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePasswordExpDate(UserMaster_ListAll_Result userPass)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                try
                {
                    ObjectParameter OutUserLoginHistoryID = new ObjectParameter("pUserLoginHistoryID", typeof(int));
                    db.conn = ClsAppDatabase.GetCon();
                    if (db.conn.State == ConnectionState.Closed)
                    { db.conn.Open(); }
                    else { db.conn.Close(); db.conn.Open(); }
                    db.tras = db.conn.BeginTransaction();

                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    Function fn = new Function();
                    db.UserMaster_ChangePassword(UserId, userPass.OldPassword, userPass.NewPassword, UserId, fn.GetSystemIP());

                    db.UserLoginHistory_Add(OutUserLoginHistoryID, UserId, Session["UserName"].ToString(), userPass.NewPassword, "", true,
                        false, "Successfully changed password.", FN.GetSystemIP());

                    db.tras.Commit(); db.conn.Close();

                    //return RedirectToAction("UserDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                    //ClearSession();
                    //return RedirectToAction("Login", "mUserMasters", new { ReturnMessages = "Your password has been changed. Kindly re-login" });

                    DataSet des = new DataSet();
                    string RoleId = Convert.ToString(Session["RoleId"]);
                    des = db.MenuRoleRights_ListAll_Result(RoleId, 0, 0, "");
                    DataView DV3 = des.Tables[0].DefaultView;
                    DV3.RowFilter = "MenuName like 'Administration'";
                    if (DV3.Count > 0)
                        Session["AdminPanel"] = Convert.ToString("1");

                    userPass.LoginName = Convert.ToString(Session["UserName"]);
                    DataSet ds = new DataSet();
                    ds = db.UserMaster_VerifyUser(userPass.LoginName, userPass.NewPassword);

                    int intCustType = Convert.ToInt32(Session["onlineCustomerTypeID"]);
                    if (intCustType == CONT.SupplierCustomerTypeID || intCustType == CONT.ObligorCustomerTypeID ||
                        intCustType == CONT.FunderCustomerTypeID || intCustType == CONT.BuyerCustomerTypeID ||
                        intCustType == CONT.BothObligorAndBuyerTypeID || intCustType == CONT.InsuranceBrokerTypeID || intCustType == CONT.InsuranceProviderTypeID)
                    {

                        if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAuthorizedUser"]))
                        {
                            if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAuthorizedSuccess"].ToString()) == false)
                            {

                                if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAgree"].ToString()) == false)
                                {
                                    return RedirectToAction("RegistrationAgree", "OnlineReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]), AuthorizedUser = "Y", ProgramType = Convert.ToString(ds.Tables["Table"].Rows[0]["ProgramType"]) });
                                }
                                else
                                {
                                    if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.SupplierCustomerTypeID)
                                    {
                                        return RedirectToAction("SupplierAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                                    }
                                    if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.ObligorCustomerTypeID || Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BuyerCustomerTypeID
                                        || Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.BothObligorAndBuyerTypeID)
                                    {
                                        return RedirectToAction("ObligorAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]), ProgramType = Convert.ToString(ds.Tables["Table"].Rows[0]["ProgramType"]) });
                                    }
                                    if (Convert.ToInt32(Session["onlineCustomerTypeID"]) == CONT.FunderCustomerTypeID)
                                    {
                                        return RedirectToAction("FunderAgreement", "OnlineReg", new { UserID = Convert.ToInt32(Session["UserId"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToBoolean(Session["IsMenuPanelHide"]))
                                {
                                    return RedirectToAction("BlankDashboard", "Home");
                                }
                                else
                                {
                                    return RedirectToAction("CMNDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                }

                            }
                        }
                        else
                        {
                            if (Convert.ToBoolean(ds.Tables["Table"].Rows[0]["IsAgree"].ToString()) == false)
                            {
                                return RedirectToAction("RegistrationAgree", "OnlineReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]), currentStatus = Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) });
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables["Table"].Rows[0]["CustomerStatus"]) == CONT.RQ)
                                {
                                    return RedirectToAction("CompanyInfo", "CustomerReg", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                }

                                else
                                {
                                    if (Convert.ToBoolean(Session["IsMenuPanelHide"]))
                                    {
                                        return RedirectToAction("BlankDashboard", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("CMNDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (intCustType == CONT.ServiceProviderCustomerTypeID)
                        {
                            return RedirectToAction("CMNDashboard", "Home", new { CustomerID = Convert.ToInt32(Session["CustomerID"]) });
                        }
                    }
                }
                catch (Exception Ex)
                {
                    db.tras.Rollback(); db.conn.Close();

                    string errorMessage = FN.CreateErrorMessage(Ex);
                    FN.LogFileWrite(errorMessage);

                    if (Ex.InnerException == null)
                        ModelState.AddModelError("", Ex.Message);
                    else
                        ModelState.AddModelError("", Ex.InnerException.Message);
                }
                return View(userPass);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        //ForgotPassword
        public ActionResult ForgotPassword(int? ReturnMessages)
        {
            Session["bredCrum"] = "Forgot Password";
            if (ReturnMessages == 1)
                ModelState.AddModelError("", "Your OTP Number has been expired. Please enter your Login Name to re-generate.");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string LoginSearch)
        {
            ClsUserMaster db = new ClsUserMaster();
            List<UserMaster_ListAll_Result> SearchLoginUser = new List<UserMaster_ListAll_Result>().ToList();
            var mUserMaster = new Object();
            try
            {
                if (!string.IsNullOrEmpty(LoginSearch))
                {
                    mUserMaster = db.UserMaster_ListAll(0, 0, LoginSearch, "", "", 1, "", false, "").FirstOrDefault();
                    if (mUserMaster != null)
                    { ViewBag.Status = 1; return View(mUserMaster); }
                    else
                    { ModelState.AddModelError("", "Invalid LoginName"); return View(); }
                }
                else
                    return View("");
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);

                if (ex.InnerException == null)
                    ModelState.AddModelError("", ex.Message);
                else
                    ModelState.AddModelError("", ex.InnerException.Message);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPasswordSubmit(UserMaster_ListAll_Result userForgot, string LoginSearch, FormCollection frm)
        {
            int intMail = 0;
            ClsUserMaster db = new ClsUserMaster();
            ObjectParameter pPassword = new ObjectParameter("pPassword", typeof(int));
            ObjectParameter OutUserLoginHistoryID = new ObjectParameter("pUserLoginHistoryID", typeof(int));
            try
            {
                db.conn = ClsAppDatabase.GetCon();
                if (db.conn.State == ConnectionState.Closed)
                { db.conn.Open(); }
                else { db.conn.Close(); db.conn.Open(); }
                db.tras = db.conn.BeginTransaction();

                string checkedName = frm["ChkMobileNo"];
                if (!string.IsNullOrEmpty(checkedName))
                {
                    db.UserMaster_ForgotPassword(userForgot.EmailID, userForgot.MobileNo, FN.GetSystemIP(), pPassword);
                    db.tras.Commit(); db.conn.Close();

                    string MessageText = string.Empty;
                    ClsHTMLTemplate dbHT = new ClsHTMLTemplate();
                    var TempText = dbHT.HTMLTemplate_ListAll(0, CONT.Resetpassword, 1, "", false, "", 0).FirstOrDefault();
                    if (!string.IsNullOrEmpty(TempText.HtmlText))
                    {
                        MessageText = FN.SMSSendingProccess(userForgot.UserID, TempText.HtmlText, 1);
                        if (!string.IsNullOrEmpty(MessageText))
                        {
                            intMail = 1;
                            FN.SendEmail(userForgot.EmailID, TempText.Subject, FN.AddHTML(MessageText), TempText.HtmlTemplateID, 0, 0, "Forgot Password E-Mail sent successfully", 1);
                            ClearSession();
                        }
                    }

                    return RedirectToAction("Login", "mUserMasters", new { ReturnMessages = "Your password has been changed. Please check your E-Mail/SMS." });
                }
                else
                {
                    ViewBag.Status = 1;
                    ModelState.AddModelError("", "Please select E-Mail or Mobile Number.");
                    return View("ForgotPassword", userForgot);
                }
            }
            catch (Exception Ex)
            {
                if (db.conn.State != ConnectionState.Closed)
                { db.tras.Rollback(); db.conn.Close(); }
                string errorMessage = FN.CreateErrorMessage(Ex);
                FN.LogFileWrite(errorMessage);
                if (intMail == 1)
                    ModelState.AddModelError("", Ex.Message);
                else
                {
                    if (Ex.InnerException == null)
                        ModelState.AddModelError("", Ex.Message);
                    else
                        ModelState.AddModelError("", Ex.InnerException.Message);
                }
            }
            return View("ForgotPassword", userForgot);
        }
        //Resetpassword
        public ActionResult ResetPassword(string LoginSearch, bool IsReset = false)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                Session["bredCrum"] = "Reset Password";
                ClsUserMaster db = new ClsUserMaster();
                List<UserMaster_ListAll_Result> SearchLoginUser = new List<UserMaster_ListAll_Result>().ToList();
                var mUserMaster = new Object();
                try
                {
                    if (!string.IsNullOrEmpty(LoginSearch))
                    {
                        mUserMaster = db.UserMaster_ListAll(0, 0, LoginSearch, "", "", 1, "", false, "").FirstOrDefault();
                        if (mUserMaster != null)
                        {
                            ViewBag.Status = 1;
                            return View(mUserMaster);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Login Name Not Found.");
                            return View();
                        }
                    }
                    else
                        return View("");
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string LoginSearch)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                List<UserMaster_ListAll_Result> SearchLoginUser = new List<UserMaster_ListAll_Result>().ToList();
                var mUserMaster = new Object();
                try
                {
                    if (!string.IsNullOrEmpty(LoginSearch))
                    {
                        mUserMaster = db.UserMaster_ListAll(0, 0, LoginSearch, "", "", 1, "", false, "").FirstOrDefault();
                        if (mUserMaster != null)
                        {
                            ViewBag.Status = 1;
                            return View(mUserMaster);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Login Name Not Found.");
                            return View();
                        }
                    }
                    else
                        return View("");
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordSubmit(UserMaster_ListAll_Result userForgot, string LoginSearch, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int intMail = 0;
                ClsUserMaster db = new ClsUserMaster();
                ObjectParameter OutUserLoginHistoryID = new ObjectParameter("pUserLoginHistoryID", typeof(int));
                ObjectParameter pPassword = new ObjectParameter("pPassword", typeof(int));
                try
                {
                    db.conn = ClsAppDatabase.GetCon();
                    if (db.conn.State == ConnectionState.Closed)
                    { db.conn.Open(); }
                    else { db.conn.Close(); db.conn.Open(); }
                    db.tras = db.conn.BeginTransaction();

                    string checkedName = frm["chkemail"];
                    string Password = string.Empty;
                    string OTPNumber = string.Empty;

                    Password = db.UserMaster_ForgotPassword(userForgot.EmailID, userForgot.MobileNo, FN.GetSystemIP(), pPassword);
                    db.tras.Commit(); db.conn.Close();
                    OTPNumber = FN.CreateRandomOTPNumber(6);

                    if (!string.IsNullOrEmpty(checkedName))
                    {
                        string MessageText = string.Empty;
                        ClsHTMLTemplate dbHT = new ClsHTMLTemplate();
                        var TempText = dbHT.HTMLTemplate_ListAll(0, CONT.Resetpassword, 1, "", false, "", 0).FirstOrDefault();
                        if (!string.IsNullOrEmpty(TempText.HtmlText))
                        {

                            MessageText = FN.SMSSendingProccess(userForgot.UserID, TempText.HtmlText, 0);
                            if (!string.IsNullOrEmpty(MessageText))
                            {
                                intMail = 1;
                                if (!string.IsNullOrEmpty(checkedName))
                                {
                                    FN.SendEmail(userForgot.EmailID, "Reset Password", FN.AddHTML(MessageText), TempText.HtmlTemplateID, 0, 0, "Password changed successfully", 1);
                                }
                            }
                        }
                    }
                    ModelState.AddModelError("", "Your password has been changed.");
                    return View("ResetPassword", userForgot);
                }
                catch (Exception Ex)
                {
                    if (db.conn.State != ConnectionState.Closed)
                        db.tras.Rollback(); db.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(Ex);
                    FN.LogFileWrite(errorMessage);
                    if (intMail == 1)
                        ModelState.AddModelError("", Ex.Message);
                    else
                    {
                        if (Ex.InnerException == null)
                            ModelState.AddModelError("", Ex.Message);
                        else
                            ModelState.AddModelError("", Ex.InnerException.Message);
                    }
                    return View("ResetPassword", userForgot);
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        //Administration
        public ActionResult Administration()
        {
            List<MenuRoleRights> MenuRole = new List<MenuRoleRights>().ToList();
            try
            {
                ClsMenuRoleRights db = new ClsMenuRoleRights();
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (!string.IsNullOrEmpty(Session["RoleId"].ToString()))
                    {
                        string RoleId = Session["RoleId"].ToString();
                        MenuRole = db.MenuRoleRights_ListAll(RoleId, 0, 0, FN.UserRoletatus()).ToList();
                        var ParentMenuName = db.MenuRoleRights_ListAll(RoleId, 0, 3, FN.UserRoletatus()).ToList();
                        ViewBag.ParentMenuName = ParentMenuName;
                        Session["bredCrum"] = "Administration";
                    }
                    else
                    { return RedirectToAction("Administration", "mUserMasters"); }
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
                return View(MenuRole);
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);

                if (ex.InnerException == null)
                    ViewBag.ErrorMesssage = ex.Message;
                else
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            return View(MenuRole);
        }

        //RedirectToPageName
        //public ActionResult RedirectToPageName(int? id, string bredCrum)
        public string RedirectToPageName(int? id, string bredCrum)
        {
            string RedirectUrl = string.Empty;
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int MenuID = 0;
                int.TryParse(id.ToString(), out MenuID);
                if (MenuID > 0)
                {
                    ClsPageMaster dbp = new ClsPageMaster();
                    PageMaster_ListAll_Result PageMaster = new PageMaster_ListAll_Result();
                    PageMaster = dbp.PageMaster_ListAll(0, MenuID, "", 1, "").FirstOrDefault();
                    if (PageMaster != null)
                    {
                        string path = Request.Url.AbsolutePath;
                        string[] Url = Request.Url.AbsoluteUri.Split('?');
                        string FUrl = Url[0];
                        string PageUrl = "/" + PageMaster.PageURL;
                        RedirectUrl = FUrl.Replace(path, PageUrl);
                        Session["MenuID"] = null;
                        Session["MenuID"] = MenuID;
                        Session["bredCrumpage"] = bredCrum;
                        Session["PageName"] = PageMaster.MenuName;
                        return RedirectUrl;
                    }
                    else
                    {
                        ViewBag.Message = "Please click another.";
                        return "#";
                    }
                }
                else
                { return "/mUserMasters/Administration"; }
            }
            else
            { return "/mUserMasters/Login"; }
        }

        //// GET: mUserMasters
        //MakerList
        public ActionResult Index()
        {
            ClsUserMaster db = new ClsUserMaster();
            List<UserMaster_ListAll_Result> UserMaster = new List<UserMaster_ListAll_Result>();
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Maker", ParentMenuID);
                    if (UserRight != null)
                    {
                        int UserId = 0;
                        int.TryParse(LoginStatus[1].ToString(), out UserId);
                        UserMaster = db.UserMaster_ListAll(0, 0, "", "", "", -1, CONT.User_Index, false, "").Where(C => C.UserID != UserId).ToList();
                        //UserMaster = db.UserMaster_ListAll(0, 0, "", "", "", -1, CONT.User_Index, false, "").ToList();

                        ViewBag.StatusList = UserMaster.Select(dataRow => new mRoleMaster
                        {
                            StatusUserDesc = dataRow.StatusUserDesc
                        }).GroupBy(model => model.StatusUserDesc).Select(group => group.First());
                        ViewBag.UserRight = UserRight;
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);

                if (ex.InnerException == null)
                    ViewBag.ErrorMesssage = ex.Message;
                else
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            return View(UserMaster);
        }



        //create userLink
        [HttpPost]
        public ActionResult CreateUaer()
        {
            //return RedirectToAction("Create", "mUserMasters");
            return RedirectToAction("CreateUser", "mUserMasters");
        }

        //CheckerList
        public ActionResult CheckerList()
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Checker", ParentMenuID);
                    if (UserRight != null)
                    {
                        if (UserRight.IsChecker)
                        {
                            int UserId = 0;
                            int.TryParse(LoginStatus[1].ToString(), out UserId);
                            ViewData["CheckerView"] = "Checker";
                            ClsUserMaster dbH = new ClsUserMaster();
                            //return View(dbH.UserMasterHistory_ListAllBind(0, 0, CONT.Document_Checker, 0).Where(x => x.CustomerTempID == 0).ToList());
                            return View(dbH.UserMasterHistory_ListAllBind(0, 0, CONT.Document_Checker, 0).ToList());

                        }
                        else
                        { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);

                if (ex.InnerException == null)
                    ViewBag.ErrorMesssage = ex.Message;
                else
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            return View();
        }

        //GET:ApproverList
        public ActionResult ApproverList()
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Approver", ParentMenuID);
                    if (UserRight != null)
                    {
                        if (UserRight.IsApprover)
                        {
                            int UserId = 0;
                            ClsUserMaster dbH = new DAL.ClsUserMaster();
                            //return View(dbH.UserMasterHistory_ListAllBind(0, UserId, CONT.User_Approve, 0).Where(x => x.CustomerTempID == 0).ToList());
                            return View(dbH.UserMasterHistory_ListAllBind(0, UserId, CONT.User_Approve, 0).ToList());
                        }
                        else
                        { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);

                if (ex.InnerException == null)
                    ViewBag.ErrorMesssage = ex.Message;
                else ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            return View();
        }

        //checker and Approver Single Details
        public ActionResult Details(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                UserMaster_ListAll_Result m = new Models.UserMaster_ListAll_Result();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);

                    if (UserRight != null)
                    {
                        if (UserRight.IsChecker || UserRight.IsApprover)
                        {
                            int MuserID = 0;
                            string staus = string.Empty;
                            int UserHistoryID = 0;
                            int.TryParse(id.ToString(), out UserHistoryID);

                            if (id > 0)
                            {
                                ClsUserMaster dbP = new ClsUserMaster();
                                ClsUserDocDetail dbUDD = new ClsUserDocDetail();
                                List<UserDocDetail_ListAll_Result> Userdocdetail = new List<UserDocDetail_ListAll_Result>().ToList();
                                m = dbP.UserMasterHistory_ListAllBind(UserHistoryID, MuserID, "", 0).FirstOrDefault();
                                MuserID = m.UserID; staus = m.Status;

                                string status = string.Empty; status = m.Status;
                                ClsUserRoleLinkMaster dbR = new ClsUserRoleLinkMaster();
                                ClsUserRoleLinkMaster dbRH = new ClsUserRoleLinkMaster();
                                m.lstUserRoleLinkMaster_ListAll_Result = dbRH.UserRoleLinkMasterHistory_ListAllBind(0, 0, MuserID, staus, 0).ToList();
                                //_mUserDeptDetail.UserID = MuserID; _mUserDeptDetail.Status = staus; _mUserDeptDetail.Keywordvalue = "";
                                //ViewBag.lstDepartment = _ClsUserMaster.UserDeptDetail_ListAll(_mUserDeptDetail);

                                string UserStatus = m.Status;
                                if (UserStatus == CONT.NR || UserStatus == CONT.UR || UserStatus == CONT.DR || UserStatus == CONT.AR || UserStatus == CONT.IR)
                                {
                                    ViewData["iCheck"] = "Checker";
                                }
                                else if (UserStatus == CONT.NC || UserStatus == CONT.UC || UserStatus == CONT.DC || UserStatus == CONT.AC || UserStatus == CONT.IC)
                                {
                                    ViewData["iCheck"] = "Approver";
                                }
                            }
                            else
                            {
                                Response.Write("User not exist.");
                                Response.End();
                            }
                        }
                        else
                        { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View(m);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        //Checker and Approver Amendment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsAmend(UserMaster_ListAll_Result m)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                ClsUserRoleLinkMaster dbRLH = new ClsUserRoleLinkMaster();
                try
                {
                    int LoginUserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out LoginUserId);
                    string Staus = m.Status;
                    string ProcessRemark = m.ProcessRemark;
                    string ProcessStatus = string.Empty;
                    int UserHistoryID = Convert.ToInt32(m.UserHistoryID);
                    int UserID = Convert.ToInt32(m.UserID);
                    string userRight = string.Empty;

                    db.conn = ClsAppDatabase.GetCon();
                    if (db.conn.State == ConnectionState.Closed)
                        db.conn.Open();
                    else
                    { db.conn.Close(); db.conn.Open(); }
                    db.tras = db.conn.BeginTransaction();

                    _ClsUserMaster.conn = db.conn;
                    _ClsUserMaster.tras = db.tras;

                    //CheckerAmend Code
                    if (Staus == CONT.NR) { ProcessStatus = CONT.NM; userRight = "CheckerAmend"; }
                    else if (Staus == CONT.UR) { ProcessStatus = CONT.UM; userRight = "CheckerAmend"; }
                    else if (Staus == CONT.DR) { ProcessStatus = CONT.DC; userRight = "CheckerAmend"; }
                    else if (Staus == CONT.AR) { ProcessStatus = CONT.AC; userRight = "CheckerAmend"; }
                    else if (Staus == CONT.IR) { ProcessStatus = CONT.IC; userRight = "CheckerAmend"; }
                    if (userRight == "CheckerAmend")
                    {
                        ViewData["iCheck"] = "Checker";
                        m.Status = ProcessStatus;
                        m.ProcessRemark = ProcessRemark;
                        m.UserID = UserID;
                        _ClsUserMaster.UserMaster_UpdateAllStatus(m);

                        //_mUserDeptDetail.UserID = UserID; _mUserDeptDetail.Status = Staus;
                        //var DepartmentData = _ClsUserMaster.UserDeptDetailHistory_ListAllBind(_mUserDeptDetail).ToList();
                        //if (DepartmentData.Count > 0)
                        //{
                        //    _ClsUserMaster.conn = db.conn; _ClsUserMaster.tras = db.tras;
                        //    for (int i = 0; i < DepartmentData.Count; i++)
                        //    {
                        //        _mUserDeptDetail.UserDeptDetHistoryID = DepartmentData[i].UserDeptDetHistoryID;
                        //        _mUserDeptDetail.DepartmentID = DepartmentData[i].DepartmentID;
                        //        _mUserDeptDetail.UserDeptDetID = DepartmentData[i].UserDeptDetID;
                        //        _mUserDeptDetail.Status = ProcessStatus;
                        //        _mUserDeptDetail.ProcessRemark = ProcessRemark;
                        //        _ClsUserMaster.UserDeptDetailHistory_ProcessStatus(_mUserDeptDetail);
                        //    }
                        //}

                        //m.lstUserRoleLinkMaster_ListAll_Result = dbRLH.UserRoleLinkMasterHistory_ListAllBind(0, 0, UserID, Staus, 0).ToList();
                        //db.UserMasterHistory_ProcessStatus(UserHistoryID, UserID, ProcessStatus, ProcessRemark, LoginUserId, FN.GetSystemIP());
                        //if (m.lstUserRoleLinkMaster_ListAll_Result.Count > 0)
                        //{
                        //    var RoleLinkMaster = m.lstUserRoleLinkMaster_ListAll_Result.ToList();
                        //    dbRLH.conn = db.conn; dbRLH.tras = db.tras;
                        //    for (int i = 0; i < RoleLinkMaster.Count; i++)
                        //    {
                        //        dbRLH.UserRoleLinkMasterHistory_ProcessStatus(RoleLinkMaster[i].UserRoleLinkHistoryID, RoleLinkMaster[i].UserRoleLinkID,
                        //            ProcessStatus, ProcessRemark, LoginUserId, FN.GetSystemIP());
                        //    }
                        //}

                        db.tras.Commit(); db.conn.Close();
                        return RedirectToAction("CheckerList");
                    }

                    //ApproverReject Code
                    if (Staus == CONT.NC) { ProcessStatus = CONT.NJ; userRight = "ApproverReject"; }
                    else if (Staus == CONT.UC) { ProcessStatus = CONT.UJ; userRight = "ApproverReject"; }
                    else if (Staus == CONT.DC) { ProcessStatus = CONT.DJ; userRight = "ApproverReject"; }
                    else if (Staus == CONT.AC) { ProcessStatus = CONT.AJ; userRight = "ApproverReject"; }
                    else if (Staus == CONT.IC) { ProcessStatus = CONT.IJ; userRight = "ApproverReject"; }

                    if (userRight == "ApproverReject")
                    {
                        ViewData["iCheck"] = "Approver";
                        m.Status = ProcessStatus;
                        m.ProcessRemark = ProcessRemark;
                        m.UserID = UserID;
                        _ClsUserMaster.UserMaster_UpdateAllStatus(m);

                        //_mUserDeptDetail.UserID = UserID; _mUserDeptDetail.Status = Staus;
                        //var DepartmentData = _ClsUserMaster.UserDeptDetailHistory_ListAllBind(_mUserDeptDetail).ToList();
                        //if (DepartmentData.Count > 0)
                        //{

                        //    _ClsUserMaster.conn = db.conn;
                        //    _ClsUserMaster.tras = db.tras;
                        //    for (int i = 0; i < DepartmentData.Count; i++)
                        //    {
                        //        _mUserDeptDetail.UserDeptDetHistoryID = DepartmentData[i].UserDeptDetHistoryID;
                        //        _mUserDeptDetail.DepartmentID = DepartmentData[i].DepartmentID;
                        //        _mUserDeptDetail.UserDeptDetID = DepartmentData[i].UserDeptDetID;
                        //        _mUserDeptDetail.Status = ProcessStatus;
                        //        _mUserDeptDetail.ProcessRemark = ProcessRemark;
                        //        _ClsUserMaster.UserDeptDetailHistory_ProcessStatus(_mUserDeptDetail);
                        //    }
                        //}

                        //m.lstUserRoleLinkMaster_ListAll_Result = dbRLH.UserRoleLinkMasterHistory_ListAllBind(0, 0, UserID, Staus, 0).ToList();
                        //db.UserMasterHistory_ProcessStatus(UserHistoryID, UserID, ProcessStatus, ProcessRemark, 1, FN.GetSystemIP());
                        //if (m.lstUserRoleLinkMaster_ListAll_Result.Count > 0)
                        //{
                        //    var RoleLinkMaster = m.lstUserRoleLinkMaster_ListAll_Result.ToList();
                        //    dbRLH.conn = db.conn; dbRLH.tras = db.tras;
                        //    for (int i = 0; i < RoleLinkMaster.Count; i++)
                        //    {
                        //        dbRLH.UserRoleLinkMasterHistory_ProcessStatus(RoleLinkMaster[i].UserRoleLinkHistoryID, RoleLinkMaster[i].UserRoleLinkID, ProcessStatus, ProcessRemark,
                        //            LoginUserId, FN.GetSystemIP());
                        //    }
                        //}
                        db.tras.Commit(); db.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    db.tras.Rollback(); db.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException == null)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                    else
                    {
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
            return View("Details", m);
        }

        //Checker and Approver Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detailsapprove(UserMaster_ListAll_Result m)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {

                Function fn = new Function();
                ClsUserMaster db = new ClsUserMaster();
                ClsUserRoleLinkMaster dbRLH = new ClsUserRoleLinkMaster();

                int intMail = 0;
                try
                {
                    int LoginUserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out LoginUserId);
                    string Staus = m.Status;
                    if (string.IsNullOrEmpty(m.ProcessRemark)) { m.ProcessRemark = ""; }
                    string ProcessRemark = m.ProcessRemark;
                    string ProcessStatus = string.Empty;
                    int UserHistoryID = Convert.ToInt32(m.UserHistoryID);
                    int UserID = Convert.ToInt32(m.UserID);
                    string userRight = string.Empty;


                    db.conn = ClsAppDatabase.GetCon();
                    if (db.conn.State == ConnectionState.Closed)
                    { db.conn.Open(); }
                    else
                    {
                        db.conn.Close(); db.conn.Open();
                    }
                    db.tras = db.conn.BeginTransaction();

                    _ClsUserMaster.conn = db.conn;
                    _ClsUserMaster.tras = db.tras;

                    //CheckerApprove code
                    if (Staus == CONT.NR) { ProcessStatus = CONT.NC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.UR) { ProcessStatus = CONT.UC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.DR) { ProcessStatus = CONT.DC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.AR) { ProcessStatus = CONT.AC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.IR) { ProcessStatus = CONT.IC; userRight = "CheckerApprove"; }

                    if (userRight == "CheckerApprove")
                    {
                        ViewData["iCheck"] = "Checker";
                        m.Status = ProcessStatus;
                        m.ProcessRemark = ProcessRemark;
                        m.UserID = UserID;
                        _ClsUserMaster.UserMaster_UpdateAllStatus(m);

                        //m.lstUserRoleLinkMaster_ListAll_Result = dbRLH.UserRoleLinkMasterHistory_ListAllBind(0, 0, UserID, Staus, 0).ToList();
                        //db.UserMasterHistory_ProcessStatus(UserHistoryID, UserID, ProcessStatus, ProcessRemark, LoginUserId, fn.GetSystemIP());
                        //if (m.lstUserRoleLinkMaster_ListAll_Result.Count > 0)
                        //{
                        //    var RoleLinkMaster = m.lstUserRoleLinkMaster_ListAll_Result.ToList();
                        //    dbRLH.conn = db.conn;
                        //    dbRLH.tras = db.tras;
                        //    for (int i = 0; i < RoleLinkMaster.Count; i++)
                        //    {
                        //        dbRLH.UserRoleLinkMasterHistory_ProcessStatus(RoleLinkMaster[i].UserRoleLinkHistoryID, RoleLinkMaster[i].UserRoleLinkID, ProcessStatus, ProcessRemark, LoginUserId, fn.GetSystemIP());
                        //    }
                        //}


                        //_mUserDeptDetail.UserID = UserID; _mUserDeptDetail.Status = Staus;
                        //var DepartmentData = _ClsUserMaster.UserDeptDetailHistory_ListAllBind(_mUserDeptDetail).ToList();
                        //if (DepartmentData.Count > 0)
                        //{

                        //    _ClsUserMaster.conn = db.conn;
                        //    _ClsUserMaster.tras = db.tras;
                        //    for (int i = 0; i < DepartmentData.Count; i++)
                        //    {
                        //        _mUserDeptDetail.UserDeptDetHistoryID = DepartmentData[i].UserDeptDetHistoryID;
                        //        _mUserDeptDetail.UserDeptDetID = DepartmentData[i].UserDeptDetID;

                        //        _mUserDeptDetail.Status = ProcessStatus;
                        //        _mUserDeptDetail.ProcessRemark = ProcessRemark;
                        //        _ClsUserMaster.UserDeptDetailHistory_ProcessStatus(_mUserDeptDetail);
                        //    }
                        //}
                        //_mUserDeptDetail.UserID = UserID; _mUserDeptDetail.Status = Staus;
                        db.tras.Commit();
                        db.conn.Close();
                        return RedirectToAction("CheckerList");
                    }

                    //ApproverApprove code

                    if (Staus == CONT.NC) { ProcessStatus = CONT.NA; userRight = "ApproverApprove"; }
                    else if (Staus == CONT.UC) { ProcessStatus = CONT.UA; userRight = "ApproverApprove"; }
                    else if (Staus == CONT.DC) { ProcessStatus = CONT.DA; userRight = "ApproverApprove"; }
                    else if (Staus == CONT.AC) { ProcessStatus = CONT.AA; userRight = "ApproverApprove"; }
                    else if (Staus == CONT.IC) { ProcessStatus = CONT.IA; userRight = "ApproverApprove"; }

                    if (userRight == "ApproverApprove")
                    {
                        ViewData["iCheck"] = "Approver";
                        m.Status = ProcessStatus;
                        m.ProcessRemark = ProcessRemark;
                        m.UserID = UserID;
                        _ClsUserMaster.UserMaster_UpdateAllStatus(m);

                        //_mUserDeptDetail.UserID = UserID; _mUserDeptDetail.Status = Staus; _mUserDeptDetail.Keywordvalue = "";
                        //var DepartmentData = _ClsUserMaster.UserDeptDetailHistory_ListAllBind(_mUserDeptDetail).ToList();
                        //if (DepartmentData.Count > 0)
                        //{

                        //    _ClsUserMaster.conn = db.conn;
                        //    _ClsUserMaster.tras = db.tras;
                        //    for (int i = 0; i < DepartmentData.Count; i++)
                        //    {
                        //        _mUserDeptDetail.UserDeptDetHistoryID = DepartmentData[i].UserDeptDetHistoryID;
                        //        _mUserDeptDetail.UserDeptDetID = DepartmentData[i].UserDeptDetID;
                        //        _mUserDeptDetail.Status = ProcessStatus;
                        //        _mUserDeptDetail.ProcessRemark = ProcessRemark;
                        //        _ClsUserMaster.UserDeptDetailHistory_ProcessStatus(_mUserDeptDetail);
                        //    }
                        //}

                        //m.lstUserRoleLinkMaster_ListAll_Result = dbRLH.UserRoleLinkMasterHistory_ListAllBind(0, 0, UserID, Staus, 0).ToList();
                        //db.UserMasterHistory_ProcessStatus(UserHistoryID, UserID, ProcessStatus, ProcessRemark, LoginUserId, fn.GetSystemIP());
                        //if (m.lstUserRoleLinkMaster_ListAll_Result.Count > 0)
                        //{
                        //    var RoleLinkMaster = m.lstUserRoleLinkMaster_ListAll_Result.ToList();
                        //    dbRLH.conn = db.conn;
                        //    dbRLH.tras = db.tras;
                        //    for (int i = 0; i < RoleLinkMaster.Count; i++)
                        //    {
                        //        dbRLH.UserRoleLinkMasterHistory_ProcessStatus(RoleLinkMaster[i].UserRoleLinkHistoryID, RoleLinkMaster[i].UserRoleLinkID, ProcessStatus, ProcessRemark, LoginUserId, fn.GetSystemIP());
                        //    }
                        //}

                        db.tras.Commit(); db.conn.Close();
                        if (ProcessStatus == CONT.NA)
                        {
                            string MessageText = string.Empty;
                            ClsHTMLTemplate dbHT = new ClsHTMLTemplate();
                            var TempText = dbHT.HTMLTemplate_ListAll(0, Infrastructure.Web.StatusManager.OtherDetail.Constant.UserLoginDetails, 1, "", false, "", 0).FirstOrDefault();
                            if (TempText != null)
                                MessageText = FN.SMSSendingProccess(UserID, TempText.HtmlText, 1);

                            string strLive = string.Empty;
                            strLive = System.Configuration.ConfigurationManager.AppSettings["IsLive"];
                            if (strLive.ToLower() == "true")
                            {
                                if (!string.IsNullOrEmpty(MessageText))
                                {
                                    intMail = 1;
                                    fn.SendEmail(m.EmailID, TempText.Subject, fn.AddHTML(MessageText), TempText.HtmlTemplateID, 0, 0, "User created successfully", 1);
                                }
                            }
                        }

                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    if (db.conn.State != ConnectionState.Closed)
                    {
                        db.tras.Rollback(); db.conn.Close();
                    }

                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (intMail == 1)
                        ModelState.AddModelError("", ex.Message);
                    else
                    {
                        if (ex.InnerException == null)
                            ModelState.AddModelError("", ex.Message);
                        else
                            ModelState.AddModelError("", ex.InnerException.Message);
                    }
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
            return View("Details", m);
        }

        //user create popup
        public ActionResult UserCreatePopup(string loginId, string userfn, string userln)
        {
            if (!string.IsNullOrEmpty(loginId)) { ViewData["LoginName"] = loginId; }
            if (!string.IsNullOrEmpty(userfn)) { ViewData["Loginfn"] = userfn; }
            if (!string.IsNullOrEmpty(userln)) { ViewData["Loginln"] = userln; }
            return PartialView();
        }
        //user Create 
        //// GET: mUserMasters/Create
        public ActionResult Create()
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string MenuId = Convert.ToString(Session["MenuID"]);
                int ParentMenuID = 0;
                int.TryParse(MenuId, out ParentMenuID);
                var UserRight = FN.CheckUserRight("", "Maker", ParentMenuID);
                if (UserRight != null)
                {
                    if (UserRight.IsMaker)
                    {
                        ClsRoleMaster dbR = new ClsRoleMaster();
                        ClsDesignationMaster dbD = new ClsDesignationMaster();
                        ClsCustomerMaster dbC = new ClsCustomerMaster();
                        ClsDocumentMaster dbDM = new ClsDocumentMaster();
                        int UserId = 0;
                        int.TryParse(LoginStatus[1].ToString(), out UserId);
                        UserMaster_ListAll_Result obj = new UserMaster_ListAll_Result();
                        SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                        ViewBag.Roles = list;
                        _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                        ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                        //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                        FillCustomerCombo();
                        ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName");
                        return View(obj);
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                else
                { return RedirectToAction("NoaccessPage", "MasterPage"); }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserMaster_ListAll_Result mUserMaster, HttpPostedFileBase Attach, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                ClsRoleMaster dbR = new ClsRoleMaster();
                ClsUserRoleLinkMaster dbRL = new ClsUserRoleLinkMaster();
                ClsDesignationMaster dbD = new ClsDesignationMaster();
                ClsCustomerMaster dbC = new ClsCustomerMaster();
                ClsDocumentMaster dbDM = new ClsDocumentMaster();

                try
                {
                    db.conn = ClsAppDatabase.GetCon();
                    if (db.conn.State == ConnectionState.Closed)
                    { db.conn.Open(); }
                    else
                    { db.conn.Close(); db.conn.Open(); }
                    db.tras = db.conn.BeginTransaction();


                    string newfilenm = Guid.NewGuid().ToString().Substring(0, 5);
                    if (Attach != null)
                    {
                        if (Attach.FileName != null)
                        {
                            mUserMaster.Attach = newfilenm + Attach.FileName.ToString();
                            Attach.SaveAs(Server.MapPath("\\Files\\Upload\\" + mUserMaster.Attach));
                        }
                    }

                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    ObjectParameter OutUserID = new ObjectParameter("pUserID", typeof(int));
                    ObjectParameter pLoginName = new ObjectParameter("pLoginName", typeof(string));
                    pLoginName.Value = mUserMaster.FirstName;
                    int mUserID = 0;
                    int DesignationID = 0;
                    int.TryParse(mUserMaster.DesignationID.ToString(), out DesignationID);
                    if (string.IsNullOrEmpty(mUserMaster.ServicePeriod)) { mUserMaster.ServicePeriod = ""; }
                    if (string.IsNullOrEmpty(mUserMaster.StaffID)) { mUserMaster.StaffID = ""; }

                    mUserID = db.UserMaster_Add(OutUserID, pLoginName, mUserMaster.FirstName, mUserMaster.MiddleName, mUserMaster.LastName,
                        mUserMaster.CustomerID, DesignationID, mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "",
                        mUserMaster.IsEmail, mUserMaster.IsSMS, UserId, FN.GetSystemIP(), mUserMaster.ServLength,
                        mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach, CONT.NR);
                    if (mUserID > 0)
                    {
                        RolLinkMaster_Add(mUserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.NR, db.conn, db.tras);
                        DeptDetail_Add(mUserID, CONT.NR, db.conn, db.tras);
                        UserDocDetail_Add(mUserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.NR, db.conn, db.tras);
                        db.tras.Commit(); db.conn.Close();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    db.tras.Rollback(); db.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                    ViewBag.Roles = list;
                    _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                    //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                    ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                    ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName");
                    FillCustomerCombo();

                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View(mUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        //User Edit
        //// GET: mUserMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string MenuId = Convert.ToString(Session["MenuID"]);
                int ParentMenuID = 0;
                int.TryParse(MenuId, out ParentMenuID);
                var UserRight = FN.CheckUserRight("", "Update", ParentMenuID);
                if (UserRight != null)
                {
                    ClsUserMaster db = new ClsUserMaster();
                    ClsCustomerMaster dbC = new ClsCustomerMaster();
                    ClsDesignationMaster dbD = new ClsDesignationMaster();
                    ClsRoleMaster dbR = new ClsRoleMaster();
                    ClsDocumentMaster dbDM = new ClsDocumentMaster();
                    ClsUserRoleLinkMaster dbRl = new ClsUserRoleLinkMaster();
                    ClsUserDocDetail dbUDD = new ClsUserDocDetail();
                    UserMaster_ListAll_Result EditUserMaster = new UserMaster_ListAll_Result();
                    try
                    {
                        int MuserID = 0;
                        int.TryParse(id.ToString(), out MuserID);
                        if (MuserID > 0)
                        {
                            List<UserDocDetail_ListAll_Result> Userdocdetail = new List<UserDocDetail_ListAll_Result>().ToList();
                            EditUserMaster = db.UserMaster_ListAll(MuserID, 0, "", "", "", -1, "", false, "").FirstOrDefault();
                            string status = string.Empty;
                            status = EditUserMaster.Status;
                            EditUserMaster.lstUserRoleLinkMaster_ListAll_Result = dbRl.UserRoleLinkMaster_ListAll(0, MuserID, 0, 1, EditUserMaster.Status, false, "").ToList();
                            //_mUserDeptDetail.UserID = MuserID;
                            //_mUserDeptDetail.Status = EditUserMaster.Status;
                            //ViewBag.lstDepartment = _ClsUserMaster.UserDeptDetail_ListAll(_mUserDeptDetail);

                            SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                            ViewBag.Roles = list;

                            _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                            //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                            ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                            ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName");
                            FillCustomerCombo();
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = FN.CreateErrorMessage(ex);
                        FN.LogFileWrite(errorMessage);

                        SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                        ViewBag.Roles = list;

                        _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                        //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                        ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                        ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName");
                        FillCustomerCombo();

                        if (ex.InnerException == null)
                            ViewBag.ErrorMesssage = ex.Message;
                        else
                            ViewBag.ErrorMesssage = ex.InnerException.Message;
                    }
                    return View(EditUserMaster);
                }
                else
                { return RedirectToAction("NoaccessPage", "MasterPage"); }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserMaster_ListAll_Result mUserMaster, HttpPostedFileBase Attach)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                ClsCustomerMaster dbC = new ClsCustomerMaster();
                ClsDesignationMaster dbD = new ClsDesignationMaster();
                ClsRoleMaster dbR = new ClsRoleMaster();
                ClsDocumentMaster dbDM = new ClsDocumentMaster();
                ClsUserRoleLinkMaster dbRl = new ClsUserRoleLinkMaster();

                try
                {
                    db.conn = ClsAppDatabase.GetCon();
                    if (db.conn.State == ConnectionState.Closed)
                    { db.conn.Open(); }
                    else
                    { db.conn.Close(); db.conn.Open(); }
                    db.tras = db.conn.BeginTransaction();

                    _ClsUserMaster.conn = db.conn;
                    _ClsUserMaster.tras = db.tras;

                    string newfilenm = Guid.NewGuid().ToString().Substring(0, 5);
                    if (Attach != null)
                    {
                        if (Attach.FileName != null)
                        {
                            mUserMaster.Attach = newfilenm + Attach.FileName.ToString();
                            Attach.SaveAs(Server.MapPath("\\Files\\Upload\\" + mUserMaster.Attach));
                        }
                    }


                    int UpdatedBy = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UpdatedBy);
                    int DesignationID = 0;
                    int.TryParse(mUserMaster.DesignationID.ToString(), out DesignationID);
                    mUserMaster.DesignationID = DesignationID;
                    if (string.IsNullOrEmpty(mUserMaster.ServicePeriod)) { mUserMaster.ServicePeriod = ""; }
                    if (string.IsNullOrEmpty(mUserMaster.StaffID)) { mUserMaster.StaffID = ""; }
                    string PassUserStatus = string.Empty;
                    string PassStatusRemarks = string.Empty;
                    int mUserID = 0;
                    ObjectParameter UserHistoryID = new ObjectParameter("pUserHistoryID", typeof(int));
                    if (mUserMaster.IsDelete)
                    {
                        string ProStaus = CONT.DR;
                        mUserID = db.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                            mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                            mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                            mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.DR, mUserMaster.ProcessRemark,
                            UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                             mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);
                        UserRoleLinkMasterProcessHistory_ProcessStatus(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus,
                            mUserMaster.ProcessRemark, mUserMaster.Status, db.conn, db.tras);
                        DeptDetailProcessHistory_ProcessStatus(mUserMaster.UserID, ProStaus, mUserMaster.ProcessRemark, mUserMaster.Status);
                        _mUserDeptDetail.UserID = mUserMaster.UserID; _mUserDeptDetail.Status = ProStaus; _mUserDeptDetail.ProcessRemark = mUserMaster.ProcessRemark;

                        db.tras.Commit(); db.conn.Close();
                    }
                    else
                    {
                        if (mUserMaster.Status == CONT.NM)
                        {
                            string ProStaus = CONT.NR;
                            if (!string.IsNullOrEmpty(mUserMaster.StaffID))
                            {
                                mUserMaster.LoginName = mUserMaster.StaffID;
                            }
                            mUserID = db.UserMaster_Update(mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName, mUserMaster.LastName, mUserMaster.LoginName,
                                mUserMaster.CustomerID, mUserMaster.DesignationID, mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo,
                                mUserMaster.StaffID, "", mUserMaster.IsEmail, mUserMaster.IsSMS, UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach, CONT.RQ);




                            RolLinkMaster_Update(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, mUserMaster.Status, db.conn, db.tras);

                            DeptDetail_Update(mUserMaster.UserID, ProStaus, db.conn, db.tras);
                            RolLinkMaster_Add(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, db.conn, db.tras);
                            DeptDetail_Add(mUserMaster.UserID, ProStaus, db.conn, db.tras);
                            if (mUserMaster.DocumentID == mUserMaster.IdType)
                            { UserDocDetail_Update(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.NR, mUserMaster.Status, db.conn, db.tras); }
                            else
                            { UserDocDetail_Add(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.NR, db.conn, db.tras); }
                            db.tras.Commit(); db.conn.Close();
                        }
                        else if (mUserMaster.Status == CONT.NA || mUserMaster.Status == CONT.UM || mUserMaster.Status == CONT.UA)
                        {
                            string ProStaus = CONT.UR;
                            mUserID = db.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "",
                                mUserMaster.IsEmail, mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.UR,
                                mUserMaster.ProcessRemark, UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);



                            //UserRoleLinkMasterHistory_ProcessStatus(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus,
                            //    mUserMaster.ProcessRemark, mUserMaster.Status, db.conn, db.tras);

                            //DeptDetailProcessHistory_ProcessStatus(mUserMaster.UserID, ProStaus, mUserMaster.ProcessRemark, mUserMaster.Status);

                            RolLinkMaster_Update(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, mUserMaster.Status, db.conn, db.tras);
                            DeptDetail_Update(mUserMaster.UserID, ProStaus, db.conn, db.tras);
                            _mUserDeptDetail.UserID = mUserMaster.UserID; _mUserDeptDetail.Status = ProStaus;
                            _mUserDeptDetail.ProcessRemark = mUserMaster.ProcessRemark;

                            RolLinkMaster_Add(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, db.conn, db.tras);
                            DeptDetail_Add(mUserMaster.UserID, ProStaus, db.conn, db.tras);
                            db.tras.Commit(); db.conn.Close();
                        }
                        else if (mUserMaster.Status == CONT.IA)
                        {
                            mUserID = db.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                                mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.IR, mUserMaster.ProcessRemark,
                                UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);
                            db.tras.Commit(); db.conn.Close();
                        }
                        else if (mUserMaster.Status == CONT.AA)
                        {
                            mUserID = db.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                                mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.AR, mUserMaster.ProcessRemark,
                                UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);
                            db.tras.Commit(); db.conn.Close();
                        }
                        int UserID = 0;
                        int.TryParse(mUserID.ToString(), out UserID);
                        if (UserID != 0)
                        { return RedirectToAction("Index"); }
                        else
                        {
                            SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                            ViewBag.Roles = list;
                            //ViewBag.lstDepartment = _ClsUserMaster.UserDeptDetail_ListAll(_mUserDeptDetail);
                            _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                            //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                            ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                            ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName");
                            FillCustomerCombo();
                            ModelState.AddModelError("", "User Not Exist.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.tras.Rollback(); db.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                    ViewBag.Roles = list;
                    _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                    ViewBag.lstDepartment = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                    _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                    //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                    ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                    ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName");
                    FillCustomerCombo();
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View(mUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        //User Update
        public ActionResult Update(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string MenuId = Convert.ToString(Session["MenuID"]);
                int ParentMenuID = 0;
                int.TryParse(MenuId, out ParentMenuID);
                var UserRight = FN.CheckUserRight("", "Maker", ParentMenuID);
                if (UserRight != null)
                {
                    ClsUserMaster db = new ClsUserMaster();
                    ClsCustomerMaster dbC = new ClsCustomerMaster();
                    ClsDesignationMaster dbD = new ClsDesignationMaster();
                    ClsRoleMaster dbR = new ClsRoleMaster();
                    ClsDocumentMaster dbDM = new ClsDocumentMaster();
                    ClsUserRoleLinkMaster dbRl = new ClsUserRoleLinkMaster();
                    ClsUserDocDetail dbUDD = new ClsUserDocDetail();
                    UserMaster_ListAll_Result EditUserMaster = new UserMaster_ListAll_Result();
                    try
                    {
                        int MuserID = 0;
                        int.TryParse(id.ToString(), out MuserID);
                        if (MuserID > 0)
                        {
                            List<UserDocDetail_ListAll_Result> Userdocdetail = new List<UserDocDetail_ListAll_Result>().ToList();
                            EditUserMaster = db.UserMaster_ListAll(MuserID, 0, "", "", "", -1, "", false, "").FirstOrDefault();
                            string status = string.Empty;
                            status = EditUserMaster.Status;
                            List<UserRoleLinkMaster_ListAll_Result> RoleList = new List<UserRoleLinkMaster_ListAll_Result>();
                            //_mUserDeptDetail.UserID = MuserID;
                            //_mUserDeptDetail.Status = EditUserMaster.Status;
                            //ViewBag.lstDepartment = _ClsUserMaster.UserDeptDetail_ListAll(_mUserDeptDetail);
                            EditUserMaster.lstUserRoleLinkMaster_ListAll_Result = dbRl.UserRoleLinkMaster_ListAll(0, MuserID, 0, 1, EditUserMaster.Status, false, "").ToList();
                            SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                            ViewBag.Roles = list;
                            _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                            //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                            ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName", EditUserMaster.DesignationID);
                            ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName", EditUserMaster.DocumentID);
                            FillCustomerCombo();
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = FN.CreateErrorMessage(ex);
                        FN.LogFileWrite(errorMessage);
                        SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                        ViewBag.Roles = list;
                        _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                        //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                        _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                        ViewBag.lstDepartment = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                        ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                        ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName");
                        FillCustomerCombo();
                        if (ex.InnerException == null)
                        { ViewBag.ErrorMesssage = ex.Message; }
                        else
                        { ViewBag.ErrorMesssage = ex.InnerException.Message; }
                    }
                    return View(EditUserMaster);
                }
                else
                { return RedirectToAction("NoaccessPage", "MasterPage"); }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UserMaster_ListAll_Result mUserMaster, HttpPostedFileBase Attach)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                ClsCustomerMaster dbC = new ClsCustomerMaster();
                ClsDesignationMaster dbD = new ClsDesignationMaster();
                ClsRoleMaster dbR = new ClsRoleMaster();
                ClsDocumentMaster dbDM = new ClsDocumentMaster();
                ClsUserRoleLinkMaster dbRl = new ClsUserRoleLinkMaster();
                try
                {
                    db.conn = ClsAppDatabase.GetCon();
                    if (db.conn.State == ConnectionState.Closed)
                        db.conn.Open();
                    else
                    { db.conn.Close(); db.conn.Open(); }
                    db.tras = db.conn.BeginTransaction();

                    _ClsUserMaster.conn = db.conn;
                    _ClsUserMaster.tras = db.tras;

                    string newfilenm = Guid.NewGuid().ToString().Substring(0, 5);
                    if (Attach != null)
                    {
                        if (Attach.FileName != null)
                        {
                            mUserMaster.Attach = newfilenm + Attach.FileName.ToString();
                            Attach.SaveAs(Server.MapPath("\\Files\\Upload\\" + mUserMaster.Attach));
                        }
                    }

                    int UpdatedBy = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UpdatedBy);
                    int DesignationID = 0;
                    int.TryParse(mUserMaster.DesignationID.ToString(), out DesignationID);
                    mUserMaster.DesignationID = DesignationID;
                    if (string.IsNullOrEmpty(mUserMaster.ServicePeriod)) { mUserMaster.ServicePeriod = ""; }
                    if (string.IsNullOrEmpty(mUserMaster.StaffID)) { mUserMaster.StaffID = ""; }
                    string PassUserStatus = string.Empty; string PassStatusRemarks = string.Empty;
                    int mUserID = 0;
                    ObjectParameter UserHistoryID = new ObjectParameter("pUserHistoryID", typeof(int));

                    if (mUserMaster.IsDelete)
                    {
                        string ProStaus = CONT.DR;
                        mUserID = db.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                            mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                            mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail, mUserMaster.IsSMS,
                            mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.DR, mUserMaster.ProcessRemark, UpdatedBy, FN.GetSystemIP(),
                            mUserMaster.ServLength,
                            mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);

                        UserRoleLinkMasterProcessHistory_ProcessStatus(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus,
                                 mUserMaster.ProcessRemark, mUserMaster.Status, db.conn, db.tras);

                        DeptDetailProcessHistory_ProcessStatus(mUserMaster.UserID, ProStaus, mUserMaster.ProcessRemark, mUserMaster.Status);

                        RolLinkMaster_Update(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, mUserMaster.Status, db.conn, db.tras);
                        RolLinkMaster_Add(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, db.conn, db.tras);
                        DeptDetail_Update(mUserID, ProStaus, db.conn, db.tras);
                        DeptDetail_Add(mUserID, ProStaus, db.conn, db.tras);
                        db.tras.Commit(); db.conn.Close();
                    }
                    else
                    {
                        if (mUserMaster.Status == CONT.NM)
                        {
                            db.conn = db.conn; db.tras = db.tras;
                            string ProStaus = CONT.NR;
                            mUserID = db.UserMaster_Update(mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName, mUserMaster.LastName,
                                mUserMaster.LoginName, mUserMaster.CustomerID, mUserMaster.DesignationID, mUserMaster.ServicePeriod, mUserMaster.EmailID,
                                mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail, mUserMaster.IsSMS, UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach, CONT.NR);

                            RolLinkMaster_Update(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, mUserMaster.Status, db.conn, db.tras);
                            DeptDetail_Update(mUserMaster.UserID, ProStaus, db.conn, db.tras);
                            RolLinkMaster_Add(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, db.conn, db.tras);
                            DeptDetail_Add(mUserID, ProStaus, db.conn, db.tras);
                            if (mUserMaster.DocumentID == mUserMaster.IdType)
                                UserDocDetail_Update(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.NR, mUserMaster.Status, db.conn, db.tras);
                            else
                                UserDocDetail_Add(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.NR, db.conn, db.tras);
                            db.tras.Commit(); db.conn.Close();
                        }
                        else if (mUserMaster.Status == CONT.NA || mUserMaster.Status == CONT.UM || mUserMaster.Status == CONT.UA)
                        {
                            string ProStaus = CONT.UR;
                            mUserID = db.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                                mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.UR, mUserMaster.ProcessRemark,
                                UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);


                            UserRoleLinkMasterProcessHistory_ProcessStatus(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus,
                                   mUserMaster.ProcessRemark, mUserMaster.Status, db.conn, db.tras);

                            //DeptDetailProcessHistory_ProcessStatus(mUserMaster.UserID, ProStaus, mUserMaster.ProcessRemark, mUserMaster.Status);

                            RolLinkMaster_Update(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, mUserMaster.Status, db.conn, db.tras);
                            RolLinkMaster_Add(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, ProStaus, db.conn, db.tras);
                            //DeptDetail_Update(mUserID, ProStaus, db.conn, db.tras);
                            //DeptDetail_Add(mUserID, ProStaus, db.conn, db.tras);
                            db.tras.Commit(); db.conn.Close();
                        }
                        else if (mUserMaster.Status == CONT.IA)
                        {
                            mUserID = db.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                                mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.IR, mUserMaster.ProcessRemark,
                                UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);
                            db.tras.Commit(); db.conn.Close();
                        }
                        else if (mUserMaster.Status == CONT.AA)
                        {
                            mUserID = db.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                                mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.AR, mUserMaster.ProcessRemark,
                                UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);
                            db.tras.Commit(); db.conn.Close();
                        }
                        int UserID = 0;
                        int.TryParse(mUserID.ToString(), out UserID);
                        if (UserID != 0)
                            return RedirectToAction("Index");
                        else
                        {
                            SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                            ViewBag.Roles = list;
                            _mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                            ViewBag.lstDepartment = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                            FillCustomerCombo();
                            ModelState.AddModelError("", "User Not Exist.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.tras.Rollback(); db.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    SelectList list = new SelectList(dbR.RoleMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.RoleName).ToList(), "RoleID", "RoleName");
                    ViewBag.Roles = list;
                    //_mUserDeptDetail.DepartmentID = 0; _mUserDeptDetail.IsActive = true;
                    //ViewBag.DeptDetail = new SelectList(_ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail).ToList(), "DepartmentID", "DepartmentName");
                    FillCustomerCombo();
                    ViewBag.DesignationID = new SelectList(dbD.DesignationMaster_ListAll(0, "", 1, ddstatus, false, "").OrderBy(x => x.DesignationName).ToList(), "DesignationID", "DesignationName");
                    ViewBag.DocumentID = new SelectList(dbDM.DocumentMaster_ListAll(0, "", 1, 1, ddstatus, false, "").OrderBy(x => x.DocumentName).ToList(), "DocumentID", "DocumentName");
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View(mUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        //user readonly view popup
        public PartialViewResult UserView(string id)
        {
            UserMaster_ListAll_Result m = new UserMaster_ListAll_Result();
            try
            {
                int UserID = 0;
                string staus = string.Empty;
                int.TryParse(id.ToString(), out UserID);
                if (UserID > 0)
                {
                    ClsUserMaster db = new ClsUserMaster();
                    m = db.UserMaster_ListAll(UserID, 0, "", "", "", 1, "", false, "").FirstOrDefault();

                    if (m.Status != null)
                        staus = m.Status;
                    ClsUserRoleLinkMaster dbR = new ClsUserRoleLinkMaster();
                    m.lstUserRoleLinkMaster_ListAll_Result = dbR.UserRoleLinkMaster_ListAll(0, UserID, 0, 1, staus, false, "").ToList();
                    UserDocDetail_ListAll_Result Userdocdetail = new UserDocDetail_ListAll_Result();
                    ClsUserDocDetail UDD = new ClsUserDocDetail();

                    //_mUserDeptDetail.UserID = UserID;
                    //_mUserDeptDetail.Status = staus;
                    //ViewBag.lstDepartment = _ClsUserMaster.UserDeptDetail_ListAll(_mUserDeptDetail);
                }
                else
                {
                    Response.Write("User not exist."); Response.End();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                if (ex.InnerException == null)
                    ViewBag.ErrorMesssage = ex.Message;
                else
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            return PartialView(m);
        }
        //user Active InActive and Delete Popup
        public PartialViewResult AIDPopup(string ActionName, string UserName)
        {
            ViewData["ActionName"] = "";
            ViewData["ActionName"] = ActionName;
            ViewData["UserName"] = UserName;
            return PartialView();
        }
        public string Act_InA_Del(string ActionName, int UserID, string ProcessRemarks)
        {
            string resp = ""; string Status = "";
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster dbH = new ClsUserMaster();
                try
                {
                    int UpdatedBy = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UpdatedBy);
                    var mUserMaster = dbH.UserMaster_ListAll(UserID, 0, "", "", "", -1, "", false, "").FirstOrDefault();
                    ObjectParameter UserHistoryID = new ObjectParameter("pUserHistoryID", typeof(int));
                    if (mUserMaster != null)
                        Status = mUserMaster.Status;
                    int mUserID = 0;
                    dbH.conn = ClsAppDatabase.GetCon();
                    if (dbH.conn.State == ConnectionState.Closed)
                        dbH.conn.Open();
                    else
                    { dbH.conn.Close(); dbH.conn.Open(); }
                    dbH.tras = dbH.conn.BeginTransaction();

                    _ClsUserMaster.conn = dbH.conn;
                    _ClsUserMaster.tras = dbH.tras;

                    if (ActionName.Equals("Delete"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA || Status == CONT.IA)
                        {
                            mUserID = dbH.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                                mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.DR, ProcessRemarks,
                                UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);

                            UserRoleLinkMasterProcessHistory_ProcessStatus(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.DR,
                                mUserMaster.ProcessRemark, mUserMaster.Status, dbH.conn, dbH.tras);

                            DeptDetailProcessHistory_ProcessStatus(mUserMaster.UserID, CONT.DR, mUserMaster.ProcessRemark, mUserMaster.Status);

                            _mUserDeptDetail.UserID = mUserMaster.UserID; _mUserDeptDetail.Status = CONT.DR;
                            _mUserDeptDetail.ProcessRemark = mUserMaster.ProcessRemark;
                        }
                    }
                    else if (ActionName.Equals("Active"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.IA)
                        {
                            mUserID = dbH.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                                mUserMaster.IsSMS, true, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.AR, ProcessRemarks, UpdatedBy, FN.GetSystemIP(),
                                mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);

                            UserRoleLinkMasterProcessHistory_ProcessStatus(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.AR,
                                mUserMaster.ProcessRemark, mUserMaster.Status, dbH.conn, dbH.tras);

                            DeptDetailProcessHistory_ProcessStatus(mUserMaster.UserID, CONT.AR, mUserMaster.ProcessRemark, mUserMaster.Status);

                            _mUserDeptDetail.UserID = mUserMaster.UserID; _mUserDeptDetail.Status = CONT.AR;
                            _mUserDeptDetail.ProcessRemark = mUserMaster.ProcessRemark;
                        }
                    }
                    else if (ActionName.Equals("InActive"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA || Status == CONT.UJ)
                        {
                            mUserID = dbH.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                                mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                                mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                                mUserMaster.IsSMS, false, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.IR, ProcessRemarks, UpdatedBy, FN.GetSystemIP(),
                                mUserMaster.ServLength,
                                mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);

                            UserRoleLinkMasterProcessHistory_ProcessStatus(mUserMaster.UserID, mUserMaster.IdNumber, mUserMaster.IdType, CONT.IR,
                                mUserMaster.ProcessRemark, mUserMaster.Status, dbH.conn, dbH.tras);

                            DeptDetailProcessHistory_ProcessStatus(mUserMaster.UserID, CONT.IR, mUserMaster.ProcessRemark, mUserMaster.Status);

                            _mUserDeptDetail.UserID = mUserMaster.UserID; _mUserDeptDetail.Status = CONT.IR;
                            _mUserDeptDetail.ProcessRemark = mUserMaster.ProcessRemark;
                        }
                    }
                    dbH.tras.Commit(); dbH.conn.Close();
                    resp = "1$Successfully";
                }
                catch (Exception ex)
                {
                    dbH.tras.Rollback(); dbH.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    resp = "0$" + ex.Message;
                }
            }
            else
            { resp = "2$Please Login."; }
            return resp;
        }
        public void UserRoleLinkMasterProcessHistory_ProcessStatus(int pUserID, string IdNumber, int IdType, string ProStaus, string ProcessRemark, string CurrentStatus, SqlConnection SCon, SqlTransaction STrans)
        {
            int LoginUserId = 0;
            int.TryParse(Session["UserId"].ToString(), out LoginUserId);
            ClsUserRoleLinkMaster dbRLH = new ClsUserRoleLinkMaster();
            List<UserRoleLinkMaster_ListAll_Result> UserStatus = new List<UserRoleLinkMaster_ListAll_Result>().ToList();
            UserStatus = dbRLH.UserRoleLinkMasterHistory_ListAllBind(0, 0, pUserID, CurrentStatus, 0).ToList();

            if (UserStatus.Count > 0)
            {
                dbRLH.conn = SCon; dbRLH.tras = STrans;
                for (int i = 0; i < UserStatus.Count; i++)
                {
                    dbRLH.UserRoleLinkMasterHistory_ProcessStatus(UserStatus[i].UserRoleLinkHistoryID, UserStatus[i].UserRoleLinkID, ProStaus, ProcessRemark, LoginUserId, FN.GetSystemIP());
                }
            }
        }
        public void DeptDetailProcessHistory_ProcessStatus(int pUserID, string ProStaus, string ProcessRemark, string CurrentStatus)
        {
            //int LoginUserId = 0;
            //int.TryParse(Session["UserId"].ToString(), out LoginUserId);
            ////ClsUserRoleLinkMaster dbRLH = new ClsUserRoleLinkMaster();
            //List<mUserDeptDetail> UserStatus = new List<mUserDeptDetail>().ToList();

            ////UserStatus = _ClsUserMaster.UserDeptDetailProcessHistory_ListAllBind(0, 0, pUserID, CurrentStatus, 0).ToList();
            //_mUserDeptDetail.UserID = pUserID; _mUserDeptDetail.Status = CurrentStatus;
            //UserStatus = _ClsUserMaster.UserDeptDetailHistory_ListAllBind(_mUserDeptDetail).ToList();
            //if (UserStatus.Count > 0)
            //{
            //    //dbRLH.conn = SCon; dbRLH.tras = STrans;
            //    _mUserDeptDetail.UserID = LoginUserId; _mUserDeptDetail.Status = ProStaus;
            //    _mUserDeptDetail.ProcessRemark = ProcessRemark;
            //    _ClsUserMaster.UserDeptDetailHistory_ProcessStatus(_mUserDeptDetail);

            //    for (int i = 0; i < UserStatus.Count; i++)
            //    {
            //        _mUserDeptDetail.UserDeptDetHistoryID = UserStatus[i].UserDeptDetHistoryID;
            //        _mUserDeptDetail.UserDeptDetID = UserStatus[i].UserDeptDetID;
            //        _mUserDeptDetail.Status = ProStaus;
            //        _ClsUserMaster.UserDeptDetailHistory_ProcessStatus(_mUserDeptDetail);
            //        //dbRLH.UserRoleLinkMasterProcessHistory_ProcessStatus(UserStatus[i].UserRoleLinkProcessHistoryID, UserStatus[i].UserRoleLinkID, ProStaus, ProcessRemark, LoginUserId, FN.GetSystemIP());
            //    }
            //}
        }
        public void RolLinkMaster_Update(int pUserID, string IdNumber, int IdType, string ProStaus, string CurrentStatus, SqlConnection SCon, SqlTransaction STrans)
        {
            int UserID = 0;
            int.TryParse(pUserID.ToString(), out UserID);
            if (UserID != 0)
            {
                ClsUserRoleLinkMaster dbRL = new ClsUserRoleLinkMaster();
                int LoginUserId = 0;
                int.TryParse(Session["UserId"].ToString(), out LoginUserId);
                List<UserRoleLinkMaster_ListAll_Result> objrollinkmas = new List<UserRoleLinkMaster_ListAll_Result>().ToList();
                objrollinkmas = dbRL.UserRoleLinkMaster_ListAll(0, UserID, 0, 1, "", false, "").ToList();
                dbRL.conn = SCon; dbRL.tras = STrans;
                for (int i = 0; i < objrollinkmas.Count; i++)
                {
                    int getid = 0;
                    //getid = dbRL.UserRoleLinkMaster_Update(objrollinkmas[i].UserRoleLinkID, objrollinkmas[i].UserID, objrollinkmas[i].RoleID, LoginUserId, FN.GetSystemIP());
                    ObjectParameter pUserRoleLinkHistoryID = new ObjectParameter("pUserRoleLinkHistoryID", typeof(int));
                    getid = dbRL.UserRoleLinkMasterHistory_Add(pUserRoleLinkHistoryID, objrollinkmas[i].UserRoleLinkID, objrollinkmas[i].UserID, objrollinkmas[i].RoleID,
                       objrollinkmas[i].Status, "", LoginUserId, FN.GetSystemIP());
                }
            }
        }
        public void DeptDetail_Update(int pUserID, string ProStaus, SqlConnection SCon, SqlTransaction STrans)
        {
            //int UserID = 0;
            //int.TryParse(pUserID.ToString(), out UserID);
            //if (UserID != 0)
            //{
            //    int LoginUserId = 0;
            //    int.TryParse(Session["UserId"].ToString(), out LoginUserId);
            //    _mUserDeptDetail.UserID = UserID;
            //    var Data = _ClsUserMaster.UserDeptDetail_ListAll(_mUserDeptDetail).ToList();

            //    _ClsUserMaster.conn = SCon; _ClsUserMaster.tras = STrans;
            //    for (int i = 0; i < Data.Count; i++)
            //    {
            //        _mUserDeptDetail.UserID = Data[i].UserID;
            //        _mUserDeptDetail.UserDeptDetID = Data[i].UserDeptDetID;
            //        _mUserDeptDetail.DepartmentID = Data[i].DepartmentID;
            //        _ClsUserMaster.UserDeptDetail_Update(_mUserDeptDetail);
            //    }
            //}
        }
        public void RolLinkMaster_Add(int pUserID, string IdNumber, int IdType, string ProStaus, SqlConnection SCon, SqlTransaction STrans)
        {
            int UserID = 0;
            int.TryParse(pUserID.ToString(), out UserID);
            if (UserID != 0)
            {
                ClsUserRoleLinkMaster dbRL = new ClsUserRoleLinkMaster();
                int LoginUserId = 0;
                int.TryParse(Session["UserId"].ToString(), out LoginUserId);
                string selRoles = Request.Form["hidSelRole"];
                if (!string.IsNullOrEmpty(selRoles))
                {
                    selRoles = selRoles.TrimEnd(',');
                    string[] selrol = selRoles.Split(',');
                    dbRL.conn = SCon;
                    dbRL.tras = STrans;
                    for (int i = 0; i < selrol.Length; i++)
                    {
                        ObjectParameter pUserRoleLinkID = new ObjectParameter("pUserRoleLinkID", typeof(int));
                        int RolID = 0;
                        int.TryParse(selrol[i].ToString(), out RolID);
                        int getid = 0;
                        getid = dbRL.UserRoleLinkMaster_Add(pUserRoleLinkID, UserID, RolID, ProStaus, LoginUserId, FN.GetSystemIP());
                    }
                }
            }
        }
        public void DeptDetail_Add(int pUserID, string ProStaus, SqlConnection SCon, SqlTransaction STrans)
        {
            //int UserID = 0;
            //int.TryParse(pUserID.ToString(), out UserID);
            //if (UserID != 0)
            //{
            //    ClsUserMaster _ClsUserMaster = new ClsUserMaster();
            //    int LoginUserId = 0;
            //    int.TryParse(Session["UserId"].ToString(), out LoginUserId);
            //    string selDep = Request.Form["hidSelDeptDetail"];
            //    if (!string.IsNullOrEmpty(selDep))
            //    {
            //        selDep = selDep.TrimEnd(',');
            //        string[] selectDep = selDep.Split(',');
            //        _ClsUserMaster.conn = SCon;
            //        _ClsUserMaster.tras = STrans;
            //        for (int i = 0; i < selectDep.Length; i++)
            //        {
            //            int getid = 0; int DeptID = 0;
            //            int.TryParse(selectDep[i].ToString(), out DeptID);
            //            _mUserDeptDetail.UserID = UserID;
            //            _mUserDeptDetail.DepartmentID = DeptID;
            //            _mUserDeptDetail.Status = ProStaus;
            //            getid = _ClsUserMaster.UserDeptDetail_Add(_mUserDeptDetail);
            //        }
            //    }
            //}
        }
        public void UserDocDetail_Add(int pUserID, string IdNumber, int IdType, string ProStaus, SqlConnection SCon, SqlTransaction STrans)
        {
            //int UserID = 0;
            //int.TryParse(pUserID.ToString(), out UserID);
            //if (UserID != 0)
            //{
            //    int LoginUserId = 0;
            //    int.TryParse(Session["UserId"].ToString(), out LoginUserId);
            //    if (!string.IsNullOrEmpty(IdNumber) && IdType != 0)
            //    {
            //        ClsUserDocDetail dbUDD = new ClsUserDocDetail();
            //        dbUDD.conn = SCon; dbUDD.tras = STrans;
            //        ObjectParameter pUserDocDetID = new ObjectParameter("pUserDocDetID", typeof(int));
            //        dbUDD.UserDocDetail_Add(pUserDocDetID, UserID, IdType, IdNumber, ProStaus, LoginUserId, FN.GetSystemIP());
            //    }
            //}
        }
        public void UserDocDetail_Update(int pUserID, string IdNumber, int IdType, string ProStaus, string CurrentStatus, SqlConnection SCon, SqlTransaction STrans)
        {
            //int UserID = 0;
            //int.TryParse(pUserID.ToString(), out UserID);
            //if (UserID != 0)
            //{
            //    int LoginUserId = 0;
            //    int.TryParse(Session["UserId"].ToString(), out LoginUserId);
            //    if (!string.IsNullOrEmpty(IdNumber) && IdType != 0)
            //    {

            //    }
            //}
        }
        public void UserDocDetailProcessHistory_ProcessStatus(int pUserID, string IdNumber, int IdType, string ProStaus, string ProcessRemark, string CurrentStatus, SqlConnection SCon, SqlTransaction STrans)
        {
            //int LoginUserId = 0;
            //int.TryParse(Session["UserId"].ToString(), out LoginUserId);
        }
        public ActionResult UpdateCancel()
        {
            return RedirectToAction("Index", "mUserMasters");
        }
        public ActionResult ActiveUser(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                UserMaster_ListAll_Result EditUserMaster = new UserMaster_ListAll_Result();
                ClsUserRoleLinkMaster dbR = new ClsUserRoleLinkMaster();
                try
                {
                    int MuserID = 0;
                    int.TryParse(id.ToString(), out MuserID);
                    if (MuserID > 0)
                    {
                        EditUserMaster = db.UserMaster_ListAll(MuserID, 0, "", "", "", -1, "", false, "").FirstOrDefault();
                        int RoleID = 0;
                        EditUserMaster.lstUserRoleLinkMaster_ListAll_Result = dbR.UserRoleLinkMaster_ListAll(0, MuserID, RoleID, -1, "", false, "").ToList();
                    }
                    else
                    {
                        Response.Write("User not exist.");
                        Response.End();
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View(EditUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActiveCurrentUser(UserMaster_ListAll_Result mUserMaster, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster dbH = new ClsUserMaster();
                try
                {
                    dbH.conn = ClsAppDatabase.GetCon();
                    if (dbH.conn.State == ConnectionState.Closed)
                        dbH.conn.Open();
                    else { dbH.conn.Close(); dbH.conn.Open(); }
                    dbH.tras = dbH.conn.BeginTransaction();
                    int UpdatedBy = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UpdatedBy);
                    string PassUserStatus = string.Empty;
                    string PassStatusRemarks = string.Empty;
                    PassStatusRemarks = mUserMaster.ProcessRemark;
                    int mUserID = 0;
                    ObjectParameter UserHistoryID = new ObjectParameter("pUserHistoryID", typeof(int));
                    mUserID = dbH.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                        mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                        mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail,
                        mUserMaster.IsSMS, mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.AR, PassStatusRemarks,
                        UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                        mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);

                    dbH.tras.Commit();
                    dbH.conn.Close();
                    int UserID = 0;
                    int.TryParse(mUserID.ToString(), out UserID);
                    if (UserID != 0)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("", "User Not Exist.");
                }
                catch (Exception ex)
                {
                    dbH.tras.Rollback(); dbH.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View("Delete", mUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        public ActionResult InActiveUser(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                ClsUserRoleLinkMaster dbR = new ClsUserRoleLinkMaster();
                UserMaster_ListAll_Result EditUserMaster = new UserMaster_ListAll_Result();
                try
                {
                    int MuserID = 0;
                    int.TryParse(id.ToString(), out MuserID);
                    if (MuserID > 0)
                    {
                        EditUserMaster = db.UserMaster_ListAll(MuserID, 0, "", "", "", -1, "", false, "").FirstOrDefault();
                        int RoleID = 0;
                        EditUserMaster.lstUserRoleLinkMaster_ListAll_Result = dbR.UserRoleLinkMaster_ListAll(0, MuserID, RoleID, -1, "", false, "").ToList();
                    }
                    else
                    {
                        Response.Write("User not exist."); Response.End();
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View(EditUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InActiveCurrentUser(UserMaster_ListAll_Result mUserMaster, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster dbH = new ClsUserMaster();
                try
                {
                    int UpdatedBy = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UpdatedBy);
                    string PassUserStatus = string.Empty;
                    string PassStatusRemarks = string.Empty;
                    PassStatusRemarks = mUserMaster.ProcessRemark;
                    int mUserID = 0;
                    ObjectParameter UserHistoryID = new ObjectParameter("pUserHistoryID", typeof(int));
                    dbH.conn = ClsAppDatabase.GetCon();
                    if (dbH.conn.State == ConnectionState.Closed)
                        dbH.conn.Open();
                    else { dbH.conn.Close(); dbH.conn.Open(); }
                    dbH.tras = dbH.conn.BeginTransaction();
                    mUserID = dbH.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                        mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                        mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail, mUserMaster.IsSMS,
                        mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.IR, PassStatusRemarks, UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                        mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);
                    dbH.tras.Commit(); dbH.conn.Close();
                    int UserID = 0;
                    int.TryParse(mUserID.ToString(), out UserID);
                    if (UserID != 0)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("", "User Not Exist.");
                }
                catch (Exception ex)
                {
                    dbH.tras.Rollback(); dbH.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View("Delete", mUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        // GET: mUserMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster db = new ClsUserMaster();
                UserMaster_ListAll_Result EditUserMaster = new UserMaster_ListAll_Result();
                try
                {
                    int MuserID = 0;
                    int.TryParse(id.ToString(), out MuserID);
                    if (MuserID > 0)
                    {
                        EditUserMaster = db.UserMaster_ListAll(MuserID, 0, "", "", "", -1, "", false, "").FirstOrDefault();
                    }
                    else
                    {
                        Response.Write("User not exist."); Response.End();
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View(EditUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(UserMaster_ListAll_Result mUserMaster, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsUserMaster dbH = new ClsUserMaster();
                try
                {
                    int UpdatedBy = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UpdatedBy);
                    string PassUserStatus = string.Empty;
                    string PassStatusRemarks = string.Empty;
                    PassStatusRemarks = frm["txtProcessRemark"];
                    int mUserID = 0;
                    ObjectParameter UserHistoryID = new ObjectParameter("pUserHistoryID", typeof(int));
                    dbH.conn = ClsAppDatabase.GetCon();
                    if (dbH.conn.State == ConnectionState.Closed)
                        dbH.conn.Open();
                    else { dbH.conn.Close(); dbH.conn.Open(); }
                    dbH.tras = dbH.conn.BeginTransaction();
                    mUserID = dbH.UserMasterHistory_Add(UserHistoryID, mUserMaster.UserID, mUserMaster.FirstName, mUserMaster.MiddleName,
                        mUserMaster.LastName, mUserMaster.LoginName, mUserMaster.Password, mUserMaster.CustomerID, mUserMaster.DesignationID,
                        mUserMaster.ServicePeriod, mUserMaster.EmailID, mUserMaster.MobileNo, mUserMaster.StaffID, "", mUserMaster.IsEmail, mUserMaster.IsSMS,
                        mUserMaster.IsActive, mUserMaster.PwdExpiryDate, mUserMaster.IsAgree, CONT.DR, PassStatusRemarks, UpdatedBy, FN.GetSystemIP(), mUserMaster.ServLength,
                        mUserMaster.DocumentID, mUserMaster.DocNo, mUserMaster.Attach);
                    dbH.tras.Commit(); dbH.conn.Close();
                    int UserID = 0;
                    int.TryParse(mUserID.ToString(), out UserID);
                    if (UserID != 0)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError("", "User Not Exist.");
                }
                catch (Exception ex)
                {
                    dbH.tras.Rollback(); dbH.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View("Delete", mUserMaster);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [WebMethod]
        public string GetRole(string roleid)
        {
            int mRoleid = 0;
            int.TryParse(roleid, out mRoleid);
            string roles = string.Empty;
            if (mRoleid != 0)
            {
                ClsRoleMaster db = new ClsRoleMaster();
                string RoleString = string.Empty;
                var ObjRole = db.RoleMaster_ListAll(mRoleid, "", 1, "", false, "");
                foreach (var item in ObjRole)
                {
                    ClsMenuRights dbR = new ClsMenuRights();
                    var objRights = dbR.MenuRights_ListAll(0, 0, item.RoleID, "", false, "").ToList();
                    string sRight = string.Empty;
                    if (objRights.Count() > 0)
                    {
                        foreach (var itemRight in objRights)
                        {
                            if (itemRight.IsApprover || itemRight.IsChecker || itemRight.IsMaker || itemRight.IsView)
                            {
                                sRight += "##" + " <b> " + itemRight.MenuName + " </b> " + "%%";
                                if (itemRight.IsApprover)
                                    sRight += " Approver |";
                                if (itemRight.IsChecker)
                                    sRight += " Checker |";
                                if (itemRight.IsMaker)
                                    sRight += " Checker |";
                                if (itemRight.IsView)
                                    sRight += " IsView |";
                            }
                        }
                    }
                    else
                    { sRight = "- -"; }
                    RoleString = item.RoleID + "!#!" + item.RoleName + "!#!" + item.RoleDesc + "!#!" + sRight;
                }
                roles = RoleString;
            }
            else
            { roles = "Roles not exist."; }
            return roles;
        }
        [WebMethod]
        public string GetRoleName(string roleid)
        {
            int mRoleid = 0;
            int.TryParse(roleid, out mRoleid);
            string roles = string.Empty;
            if (mRoleid != 0)
            {
                ClsRoleMaster db = new ClsRoleMaster();
                string RoleString = string.Empty;
                var ObjROle = db.RoleMaster_ListAll(mRoleid, "", 1, "", false, "");
                foreach (var item in ObjROle)
                { RoleString = item.RoleID + "!#!" + item.RoleName; }
                roles = RoleString;
            }
            else
            { roles = "Roles not exist."; }
            return roles;
        }
        [WebMethod]
        public string GetDepartmentName(string deptid)
        {
            int mDeptid = 0;
            int.TryParse(deptid, out mDeptid);
            string department = string.Empty;
            if (mDeptid != 0)
            {
                _mUserDeptDetail.DepartmentID = mDeptid;
                _mUserDeptDetail.IsActive = true;
                _ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail);
                string DeptString = string.Empty;
                var ObjDept = _ClsUserMaster.DepartmentMaster_ListAll(_mUserDeptDetail);
                foreach (var item in ObjDept)
                { DeptString = item.DepartmentID + "!#!" + item.DepartmentName; }
                department = DeptString;
            }
            else
            { department = "Function not exist."; }
            return department;
        }

        [WebMethod]
        public string DeleteRole(string roleid, string status)
        {
            string roles = string.Empty;
            int pUserRoleLinkID = 0;
            int.TryParse(roleid, out pUserRoleLinkID);
            if (pUserRoleLinkID > 0)
            {
                ClsUserRoleLinkMaster dbRL = new ClsUserRoleLinkMaster();
                try
                {
                    int RetVal = 1;
                    dbRL.conn = ClsAppDatabase.GetCon();
                    if (dbRL.conn.State == ConnectionState.Closed)
                    { dbRL.conn.Open(); }
                    else { dbRL.conn.Close(); dbRL.conn.Open(); }
                    dbRL.tras = dbRL.conn.BeginTransaction();
                    if (status == CONT.NM)
                    {
                        RetVal = dbRL.UserRoleLinkMaster_Delete(pUserRoleLinkID, Convert.ToInt32(HttpContext.Session["UserId"]), FN.GetSystemIP());
                        dbRL.tras.Commit(); dbRL.conn.Close();
                    }
                    else
                    {
                        RetVal = dbRL.UserRoleLinkMaster_UpdateNotRequired(pUserRoleLinkID, Convert.ToInt32(HttpContext.Session["UserId"]), FN.GetSystemIP());
                        dbRL.tras.Commit(); dbRL.conn.Close();
                    }

                    if (RetVal == 0)
                        roles = "Roles not exist.";
                }
                catch (Exception ex)
                {
                    roles = "Roles not exist.";
                    dbRL.tras.Rollback(); dbRL.conn.Close();
                }
            }
            else
            { roles = "Roles not exist."; }
            return roles;
        }

        [WebMethod]
        public string DeleteDept(string deptid, string status)
        {
            string roles = string.Empty;
            int pUserDeptDetID = 0;
            int.TryParse(deptid, out pUserDeptDetID);
            if (pUserDeptDetID > 0)
            {
                try
                {
                    int RetVal = 1;
                    _ClsUserMaster.conn = ClsAppDatabase.GetCon();
                    if (_ClsUserMaster.conn.State == ConnectionState.Closed)
                    { _ClsUserMaster.conn.Open(); }
                    else { _ClsUserMaster.conn.Close(); _ClsUserMaster.conn.Open(); }
                    _ClsUserMaster.tras = _ClsUserMaster.conn.BeginTransaction();

                    _mUserDeptDetail.UserDeptDetID = pUserDeptDetID;
                    _mUserDeptDetail.UserID = Convert.ToInt32(HttpContext.Session["UserId"]);
                    if (status == CONT.NM)
                    {
                        RetVal = _ClsUserMaster.UserDeptDetail_Delete(_mUserDeptDetail);
                        _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();
                    }
                    else
                    {
                        RetVal = _ClsUserMaster.UserDeptDetail_UpdateNotRequired(_mUserDeptDetail);
                        _ClsUserMaster.tras.Commit(); _ClsUserMaster.conn.Close();
                    }
                    if (RetVal == 0)
                    { roles = "Function not exist."; }
                }
                catch (Exception ex)
                {
                    roles = "Function not exist.";
                    _ClsUserMaster.tras.Rollback();
                    _ClsUserMaster.conn.Close();
                }
            }
            else
            { roles = "Roles not exist."; }
            return roles;
        }

        public ActionResult ChangeProfilePicture()
        {
            return View();
        }

        public ActionResult UserMasterViewerIndex()
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Viewer", ParentMenuID);
                    if (UserRight != null)
                    {
                        if (UserRight.IsView)
                        {
                            int UserId = 0;
                            int.TryParse(LoginStatus[1].ToString(), out UserId);
                            ClsUserMaster dbH = new ClsUserMaster();
                            return View(dbH.UserMasterHistory_ListAllBind(0, 0, "", 0).ToList());
                        }
                        else
                        { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                if (ex.InnerException == null)
                { ViewBag.ErrorMesssage = ex.Message; }
                else
                { ViewBag.ErrorMesssage = ex.InnerException.Message; }
            }
            return View();
        }

        public ActionResult UserMasterViewer(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                UserMaster_ListAll_Result m = new Models.UserMaster_ListAll_Result();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Viewer", ParentMenuID);

                    if (UserRight != null)
                    {
                        if (UserRight.IsView == true)
                        {
                            int MuserID = 0;
                            string staus = string.Empty;
                            int UserHistoryID = 0;
                            int.TryParse(id.ToString(), out UserHistoryID);
                            if (id > 0)
                            {
                                ClsUserMaster dbP = new ClsUserMaster();
                                ClsUserDocDetail dbUDD = new ClsUserDocDetail();
                                List<UserDocDetail_ListAll_Result> Userdocdetail = new List<UserDocDetail_ListAll_Result>().ToList();
                                m = dbP.UserMasterHistory_ListAllBind(UserHistoryID, MuserID, "", 0).FirstOrDefault();
                                MuserID = m.UserID; staus = m.Status;
                                string status = string.Empty;
                                status = m.Status;
                                //_mUserDeptDetail.UserID = MuserID; _mUserDeptDetail.Status = staus; _mUserDeptDetail.Keywordvalue = "";
                                //ViewBag.lstDepartment = _ClsUserMaster.UserDeptDetail_ListAll(_mUserDeptDetail);

                                ClsUserRoleLinkMaster dbR = new ClsUserRoleLinkMaster();
                                m.lstUserRoleLinkMaster_ListAll_Result = dbR.UserRoleLinkMasterHistory_ListAllBind(0, 0, MuserID, staus, 0).ToList();
                            }
                            else
                            {
                                Response.Write("User not exist."); Response.End();
                            }
                        }
                        else
                        { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException == null)
                        ModelState.AddModelError("", ex.Message);
                    else
                        ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View(m);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }

        [WebMethod]
        public string UserPasswordHistory(int UserID, string Password)
        {
            string rPassword = string.Empty;
            rPassword = "Success";
            try
            {
                ClsUserMaster db = new ClsUserMaster();
                DataTable dt = new DataTable();
                dt = db.UserLoginHistory_PasswordHistory(UserID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string hPassword = string.Empty;
                        hPassword = Convert.ToString(dt.Rows[i]["Password"]);
                        if (hPassword.Equals(Password))
                        {
                            rPassword = "Fail";
                        }
                    }
                }
            }
            catch (Exception)
            {
                rPassword = "Fail";
            }
            return rPassword;
        }
        public PartialViewResult CommentResult(int UserID)
        {
            ClsUserMaster dbP = new ClsUserMaster();
            ViewBag.lstCustomerMaster = dbP.UserMaster_ListAllRemarks(UserID).ToList();
            return PartialView();
        }

        public void FillCustomerCombo()
        {
            ClsCustomerMaster _CustomerMaster = new ClsCustomerMaster();
            var DDLCustomerIDData = _CustomerMaster.UserMaster_CustomerBindDetails(0, 0, ddstatus).OrderBy(x => x.CompanyName).ToList();
            if (DDLCustomerIDData != null && DDLCustomerIDData.Count > 0)
                ViewBag.DDLCustomerID = DDLCustomerIDData;
            else
                ViewBag.DDLCustomerID = new List<SelectListItem> { };
        }
        public String GetUserPlatform()
        {
            var ua = Request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT"))
                return "Windows";

            return Request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }

        public String GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }
    }
}
