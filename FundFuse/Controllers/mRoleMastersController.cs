using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using TMP.Models;
using TMP.DAL;
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;
namespace TMP.Controllers

{
    public class mRoleMastersController : Controller
    {
        #region Local Variable
        ConString db = new ConString();
        Function FN = new Function();
        ClsRoleMaster rolemaster = new ClsRoleMaster();
        ClsMenuMaster objMenuMaster = new ClsMenuMaster();
        ClsMenuRights objMenuRights = new ClsMenuRights();
        ClsMenuRoleRights objMenuRoleRights = new ClsMenuRoleRights();
        #endregion
        public ActionResult Index(string status = "")
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                mRoleMaster _objModel = new mRoleMaster();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                    if (UserRight != null)
                    {
                        ViewBag.UserRight = UserRight.MenuName;
                        _objModel.lstRoleMaster = rolemaster.RoleMaster_ListAll(0, "", -1, status, false, "").ToList();
                    }
                    else
                    {
                        return RedirectToAction("NoaccessPage", "MasterPage");
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
                return View(_objModel);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult CreateUaer(int? id)
        {
            return RedirectToAction("Create", "mRoleMasters");
        }
        public ActionResult CheckerList(int? Page)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
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

                            int pageNum = (Page ?? 1);
                            mRoleMaster objlist = new mRoleMaster();
                            ViewData["CheckerView"] = "Checker";

                            objlist.lstRoleMasterListBind = rolemaster.RoleMasterProcessHistory_ListAllBind(0, 0, CONT.Document_Checker, 0).ToList();
                            return View(objlist);
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

                    if (ex.InnerException != null)
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult ApproverList(int? Page)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
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
                            int.TryParse(LoginStatus[1].ToString(), out UserId);

                            int pageNum = (Page ?? 1);
                            mRoleMaster objlist = new mRoleMaster();

                            ViewData["CheckerView"] = "Approver";
                            objlist.lstRoleMasterListBind = rolemaster.RoleMasterProcessHistory_ListAllBind(0, 0, CONT.HTMLApprover, 0).ToList();
                            return View(objlist);
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
                    if (ex.InnerException != null)
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult Details(int? iRoleID, int? iRoleProcessHistoryID, string icheck)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                List<mRoleMaster> RoleMaster_ListAll_Result = new List<mRoleMaster>();
                mRoleMaster m = new mRoleMaster();
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
                            int UserId = 0;
                            int.TryParse(LoginStatus[1].ToString(), out UserId);
                            if (iRoleID == null)
                            {
                                ViewBag.Error = "invelid Expression";
                                return RedirectToAction("Index", "mRolemasters");
                            }
                            m.lstRoleMasterListBind = rolemaster.RoleMasterProcessHistory_ListAllBind(iRoleProcessHistoryID, iRoleID, "", 0).ToList();

                            string selectedmaker = "";
                            string selectedapporver = "";
                            string selectedchecker = "";
                            string selectedisview = "";

                            var data = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, iRoleID, 0, "", 0).ToList();
                            for (int i = 0; i <= data.Count - 1; i++)
                            {
                                if (data[i].IsMaker == true)
                                { selectedmaker = selectedmaker + "," + data[i].MenuID; }
                                if (data[i].IsChecker == true)
                                { selectedchecker = selectedchecker + "," + data[i].MenuID; }
                                if (data[i].IsApprover == true)
                                { selectedapporver = selectedapporver + "," + data[i].MenuID; }
                                if (data[i].IsView == true)
                                { selectedisview = selectedisview + "," + data[i].MenuID; }
                            }

                            ViewData["selectedmaker"] = selectedmaker;
                            ViewData["selectedchecker"] = selectedchecker;
                            ViewData["selectedapporver"] = selectedapporver;
                            ViewData["selectedisview"] = selectedisview;
                            ViewData["roleid"] = iRoleID;
                            ViewData["iCheck"] = icheck;
                            ViewData["iRoleProcessHistoryID"] = iRoleProcessHistoryID;

                            m.lstMenuRightsListBind = data;

                            if (RoleMaster_ListAll_Result == null)
                            { return HttpNotFound(); }
                            return View(m);
                        }
                        else
                        {
                            return RedirectToAction("NoaccessPage", "MasterPage");
                        }
                    }
                    else
                    {
                        return RedirectToAction("NoaccessPage", "MasterPage");
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
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int? iRoleID, int? iRoleProcessHistoryID, FormCollection frm, mRoleMaster m)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                ClsMenuRights menurights = new ClsMenuRights();
                List<mRoleMaster> RoleMaster_ListAll_Result = new List<mRoleMaster>();
                DAL.ClsMenuRights M2 = new DAL.ClsMenuRights();
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    if (iRoleID == null)
                    { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                    ViewBag.Data = rolemaster.RoleMasterProcessHistory_ListAllBind(iRoleProcessHistoryID, iRoleID, "", 0).ToList();
                    ViewBag.Data1 = menurights.MenuRightsProcessHistory_ListAllBind(0, iRoleID, 0, "", 0).ToList();

                    if (RoleMaster_ListAll_Result == null)
                    { return HttpNotFound(); }
                    ViewData["roleid"] = iRoleID;
                    ViewData["iRoleProcessHistoryID"] = iRoleProcessHistoryID;
                    return View();
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ex.Message;
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult CreateRole()
        {
            return RedirectToAction("RoleManagement", "mRoleMasters", new { RoleID = 0 });
        }

        public ActionResult RoleMasterDesign()
        {
            return View();
        }

        public ActionResult RoleManagement(int RoleID = 0)
        {
            mRoleMaster obj = new mRoleMaster();
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
                        ViewBag.UserRight = UserRight.MenuName;

                    FillCustomerTypeCombo();
                    if (RoleID > 0)
                    {
                        var GetDataInListAllBind = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, RoleID, 0, "", 0).ToList();
                        ViewBag.intMenuCount = GetDataInListAllBind.Count; ViewBag.MenuRights = GetDataInListAllBind;
                        ViewBag.RoleID = GetDataInListAllBind[0].RoleID; ViewBag.Status = GetDataInListAllBind[0].Status;
                        obj.RoleName = GetDataInListAllBind[0].RoleName; obj.RoleDesc = GetDataInListAllBind[0].RoleDesc;
                        obj.CustomerTypeIDs = GetDataInListAllBind[0].CustomerTypeIDs; obj.GetCustomerTypeIDs = GetDataInListAllBind[0].CustomerTypeIDs;
                        ViewBag.IsUpdate = true;
                    }
                    else
                    {
                        var GetDataInListAll = objMenuRights.MenuRights_ListAllNew(0, 0, 0, "", false, "").ToList();
                        ViewBag.intMenuCount = GetDataInListAll.Count; ViewBag.MenuRights = GetDataInListAll;
                        ViewBag.RoleID = GetDataInListAll[0].RoleID; ViewBag.Status = GetDataInListAll[0].Status;
                        obj.RoleName = GetDataInListAll[0].RoleName; obj.RoleDesc = GetDataInListAll[0].RoleDesc;
                        obj.CustomerTypeIDs = GetDataInListAll[0].CustomerTypeIDs; obj.GetCustomerTypeIDs = GetDataInListAll[0].CustomerTypeIDs;
                        ViewBag.IsUpdate = false;
                    }
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
            FillCustomerTypeCombo();
            return View(obj);
        }
        [HttpPost]
        public ActionResult RoleManagement(FormCollection frm, mRoleMaster mRoleMaster)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    rolemaster.conn = ClsAppDatabase.GetCon();
                    if (rolemaster.conn.State == ConnectionState.Closed)
                        rolemaster.conn.Open();
                    else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                    rolemaster.tras = rolemaster.conn.BeginTransaction();

                    string strRoleName = Request.Form["RoleName"];
                    string strRoleDesc = Request.Form["RoleDesc"];
                    string strCustomerTypeIDs = Request.Form["CustomerTypeIDs"];
                    string Status = Request.Form["Status"];
                    if (Status == null)
                        Status = "";
                    int intMenuCount = Convert.ToInt32(Request.Form["intMenuCount"]);
                    int intRoleID = Convert.ToInt32(Request.Form["hdnRoleID"]);

                    var GetMenuRights = objMenuRights.MenuRights_ListAllNew(0, 0, intRoleID, "", false, "").ToList();

                    ObjectParameter outRoleID = new ObjectParameter("pRoleID", typeof(int));
                    if (mRoleMaster.RoleDesc == null)
                        mRoleMaster.RoleDesc = "";

                    int UserId = 0;
                    //int.TryParse(LoginStatus[1].ToString(), out UserId);


                    int MenuRightsID;
                    if (Status == "")
                    {
                        //Int32 RoleID = rolemaster.RoleMaster_Add(outRoleID, mRoleMaster.RoleName, mRoleMaster.RoleDesc, UserId, FN.GetSystemIP(), strCustomerTypeIDs);
                        mRoleMaster.CustomerTypeIDs = strCustomerTypeIDs;
                        Int32 RoleID = rolemaster.RoleMaster_AddNew(mRoleMaster);
                        #region Add
                        for (int i = 0; i < GetMenuRights.Count; i++)
                        {
                            int MenuID = GetMenuRights[i].MenuID;

                            string strSelectedWithEndTrim = ""; string strSelected = "";
                            for (int j = 0; j < frm.Count; j++)
                            {
                                string GetAllkeysStringValue = frm.AllKeys[j];
                                if (GetAllkeysStringValue.Equals("chk_M_" + MenuID + "") || GetAllkeysStringValue.Equals("chk_C_" + MenuID + "") ||
                                    GetAllkeysStringValue.Equals("chk_A_" + MenuID + "") || GetAllkeysStringValue.Equals("chk_V_" + MenuID + ""))
                                { strSelected += GetAllkeysStringValue + ","; }
                                strSelectedWithEndTrim = strSelected.TrimEnd(',');
                            }

                            if (strSelectedWithEndTrim == "")
                            {
                                ObjectParameter outMenuRightsID = new ObjectParameter("pMenuRightsID", typeof(int));
                                MenuRightsID = rolemaster.MenuRights_Add(outMenuRightsID, MenuID, RoleID, false, false, false, false, UserId, FN.GetSystemIP());
                            }
                            else
                            {
                                string[] strSelectedWithEndTrimSplit = strSelectedWithEndTrim.Split(',');
                                bool IsMekar = false; bool IsChecker = false; bool IsApproval = false; bool IsView = false;
                                int MenuIDChecked = 0;
                                for (int z = 0; z < strSelectedWithEndTrimSplit.Count(); z++)
                                {
                                    string strSelectedWithEndTrimSplitValue = strSelectedWithEndTrimSplit[z];
                                    string[] strSelectedWithEndTrimSplitValueSplit = strSelectedWithEndTrimSplitValue.Split('_');
                                    MenuIDChecked = Convert.ToInt32(strSelectedWithEndTrimSplitValueSplit[2]);

                                    if (strSelectedWithEndTrimSplitValueSplit[1] == "M") { IsMekar = true; }
                                    else if (strSelectedWithEndTrimSplitValueSplit[1] == "C") { IsChecker = true; }
                                    else if (strSelectedWithEndTrimSplitValueSplit[1] == "A") { IsApproval = true; }
                                    else if (strSelectedWithEndTrimSplitValueSplit[1] == "V") { IsView = true; }
                                }
                                ObjectParameter outMenuRightsID = new ObjectParameter("pMenuRightsID", typeof(int));
                                MenuRightsID = rolemaster.MenuRights_Add(outMenuRightsID, MenuIDChecked,
                                    RoleID, IsMekar, IsChecker, IsApproval, IsView, UserId, FN.GetSystemIP());
                            }
                        }
                        rolemaster.tras.Commit(); rolemaster.conn.Close();
                        #endregion
                    }
                    else
                    {
                        #region Update
                        if (!frm.AllKeys.Contains("chkDeleteRole"))
                        {
                            ObjectParameter outRoleid = new ObjectParameter("pRoleProcessHistoryID", typeof(int));
                            if (Status == CONT.NM)
                            {
                                if (strRoleDesc == null) { strRoleDesc = ""; }
                                //rolemaster.RoleMaster_Update(intRoleID, strRoleName, strRoleDesc, UserId, FN.GetSystemIP(), strCustomerTypeIDs);
                                mRoleMaster.CustomerTypeIDs = strCustomerTypeIDs;
                                rolemaster.RoleMaster_UpdateNew(mRoleMaster);
                            }
                            else
                            {
                                if (strRoleDesc == null) { strRoleDesc = ""; }
                                //rolemaster.RoleMasterProcessHistory_Add(outRoleid, intRoleID, strRoleName, strRoleDesc, true, CONT.UR, "", UserId, FN.GetSystemIP(), strCustomerTypeIDs);
                                mRoleMaster.RoleID = intRoleID;
                                mRoleMaster.IsActive = true; mRoleMaster.Status = CONT.UR;
                                mRoleMaster.CustomerTypeIDs = strCustomerTypeIDs;
                                rolemaster.RoleMasterProcessHistory_AddNew(mRoleMaster);
                            }

                            objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;
                            int MenuRightsProcessHistoryID;
                            for (int i = 0; i <= GetMenuRights.Count - 1; i++)
                            {
                                int MenuID = GetMenuRights[i].MenuID;
                                string strSelectedWithEndTrim = ""; string strSelected = "";
                                for (int j = 0; j < frm.Count; j++)
                                {
                                    string GetAllkeysStringValue = frm.AllKeys[j];
                                    if (GetAllkeysStringValue.Equals("chk_M_" + MenuID + "") || GetAllkeysStringValue.Equals("chk_C_" + MenuID + "") ||
                                        GetAllkeysStringValue.Equals("chk_A_" + MenuID + "") || GetAllkeysStringValue.Equals("chk_V_" + MenuID + ""))
                                    {
                                        strSelected += GetAllkeysStringValue + ",";
                                    }
                                    strSelectedWithEndTrim = strSelected.TrimEnd(',');
                                }
                                if (strSelectedWithEndTrim == "")
                                {
                                    if (Status == CONT.NM)
                                    {
                                        objMenuRights.MenuRights_Update(GetMenuRights[i].MenuRightsID, GetMenuRights[i].MenuID, intRoleID, false, false,
                                            false, false, UserId, FN.GetSystemIP());
                                    }
                                    else
                                    {
                                        ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                        MenuRightsProcessHistoryID = objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, GetMenuRights[i].MenuRightsID,
                                        GetMenuRights[i].MenuID, intRoleID, false, false, false, false, CONT.UR, "", UserId, FN.GetSystemIP());
                                    }
                                }
                                else
                                {
                                    string[] strSelectedWithEndTrimSplit = strSelectedWithEndTrim.Split(',');
                                    bool IsMekar = false; bool IsChecker = false; bool IsApproval = false; bool IsView = false; int MenuIDChecked = 0;
                                    for (int z = 0; z < strSelectedWithEndTrimSplit.Count(); z++)
                                    {

                                        string strSelectedWithEndTrimSplitValue = strSelectedWithEndTrimSplit[z];
                                        string[] strSelectedWithEndTrimSplitValueSplit = strSelectedWithEndTrimSplitValue.Split('_');
                                        MenuIDChecked = Convert.ToInt32(strSelectedWithEndTrimSplitValueSplit[2]);

                                        if (strSelectedWithEndTrimSplitValueSplit[1] == "M")
                                        { IsMekar = true; }
                                        else if (strSelectedWithEndTrimSplitValueSplit[1] == "C")
                                        { IsChecker = true; }
                                        else if (strSelectedWithEndTrimSplitValueSplit[1] == "A")
                                        { IsApproval = true; }
                                        else if (strSelectedWithEndTrimSplitValueSplit[1] == "V")
                                        { IsView = true; }
                                    }

                                    if (Status == CONT.NM)
                                    {
                                        objMenuRights.MenuRights_Update(GetMenuRights[i].MenuRightsID, GetMenuRights[i].MenuID, intRoleID, IsMekar, IsChecker,
                                            IsApproval, IsView, UserId, FN.GetSystemIP());
                                    }
                                    else
                                    {
                                        ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                        MenuRightsProcessHistoryID = objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, GetMenuRights[i].MenuRightsID,
                                        GetMenuRights[i].MenuID, intRoleID, IsMekar, IsChecker, IsApproval, IsView, CONT.UR, "", UserId, FN.GetSystemIP());
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA || Status == CONT.IA)
                            {
                                ObjectParameter outRoleid = new ObjectParameter("pRoleProcessHistoryID", typeof(int));
                                if (strRoleDesc == null)
                                { strRoleDesc = ""; }
                                rolemaster.RoleMasterProcessHistory_Add(outRoleid, intRoleID, strRoleName, strRoleDesc, false, CONT.DR, "", UserId, FN.GetSystemIP());

                                objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                                for (int i = 0; i < GetMenuRights.Count; i++)
                                {
                                    string MenuRightsStatus = GetMenuRights[i].Status;
                                    MenuRightsID = GetMenuRights[i].MenuRightsID;
                                    int MenuID = GetMenuRights[i].MenuID;
                                    bool IsMaker = GetMenuRights[i].IsMaker;
                                    bool IsChecker = GetMenuRights[i].IsChecker;
                                    bool IsApprover = GetMenuRights[i].IsApprover;
                                    bool IsView = GetMenuRights[i].IsView;

                                    ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                    objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, MenuRightsID, MenuID, intRoleID, IsMaker, IsChecker, IsApprover,
                                    IsView, CONT.DR, "", UserId, FN.GetSystemIP());
                                }
                            }
                        }
                        rolemaster.tras.Commit(); rolemaster.conn.Close();
                        #endregion
                    }
                    return RedirectToAction("Index", "mRoleMasters");
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                if (rolemaster.conn.State == ConnectionState.Open)
                {
                    rolemaster.tras.Rollback();
                    rolemaster.conn.Close();
                }
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);

                if (ex.InnerException != null)
                {
                    ViewBag.ErrorMsg = ex.InnerException.Message;
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
                else
                {
                    ViewBag.ErrorMsg = ex.Message;
                    ModelState.AddModelError("", ex.Message);
                }
                mRoleMaster.RoleDesc = mRoleMaster.RoleDesc;

            }
            var Data = objMenuRights.MenuRights_ListAllNew(0, 0, 0, "", false, "").ToList();

            ViewBag.intMenuCount = Data.Count;
            ViewBag.MenuRights = Data;
            ViewBag.RoleID = Data[0].RoleID;
            ViewBag.Status = Data[0].Status;
            ViewBag.IsUpdate = false;
            FillCustomerTypeCombo();
            return View(mRoleMaster);
        }

        public ActionResult RoleDetails(int RoleID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                mRoleMaster objlist = new mRoleMaster();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;

                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                    if (UserRight != null)
                    {
                        ViewBag.IsCheckerOrApprover = UserRight.MenuName;
                    }
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);

                    var GetDataInListAllBind = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, RoleID, 0, "", 0).ToList();
                    ViewBag.intMenuCount = GetDataInListAllBind.Count;
                    ViewBag.MenuRights = GetDataInListAllBind;
                    ViewBag.RoleID = GetDataInListAllBind[0].RoleID;
                    ViewBag.Status = GetDataInListAllBind[0].Status;
                    ViewBag.CustomerTypeIDs = GetDataInListAllBind[0].CustomerTypeIDs;
                    ViewBag.GetCustomerTypeIDs = GetDataInListAllBind[0].CustomerTypeIDs;



                    objlist.RoleID = GetDataInListAllBind[0].RoleID;
                    objlist.RoleName = GetDataInListAllBind[0].RoleName;
                    objlist.RoleDesc = GetDataInListAllBind[0].RoleDesc;
                    objlist.CustomerTypeIDs = GetDataInListAllBind[0].CustomerTypeIDs;
                    objlist.GetCustomerTypeIDs = GetDataInListAllBind[0].CustomerTypeIDs;
                    objlist.Status = GetDataInListAllBind[0].Status;
                    objlist.IsMakerRole = GetDataInListAllBind[0].IsMakerRole;
                    objlist.IsCheckerRole = GetDataInListAllBind[0].IsCheckerRole;
                    objlist.IsApproverRole = GetDataInListAllBind[0].IsApproverRole;
                    objlist.IsViewerRole = GetDataInListAllBind[0].IsViewerRole;
                    FillCustomerTypeCombo();
                    return View(objlist);
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
                return View(objlist);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult RoleDetails(FormCollection frm)
        {
            try
            {
                string CurrentStatus = Convert.ToString(Request["Status"]);
                int intRoleID = Convert.ToInt16(Request["RoleID"]);
                string Remarks = Convert.ToString(Request["Remarks"]);
                mRoleMaster _mRoleMaster = new mRoleMaster();
                _mRoleMaster.ProcessRemark = Remarks;
                _mRoleMaster.RoleID = intRoleID;

                rolemaster.conn = ClsAppDatabase.GetCon();
                if (rolemaster.conn.State == ConnectionState.Closed) rolemaster.conn.Open();
                else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                rolemaster.tras = rolemaster.conn.BeginTransaction();

                if (Request["btnCheckerReturn"] != null)
                {
                    if (CurrentStatus == CONT.NR)
                        _mRoleMaster.Status = CONT.NM;
                    else if (CurrentStatus == CONT.UR)
                        _mRoleMaster.Status = CONT.UM;

                    rolemaster.RoleMaster_UpdateAllStatus(_mRoleMaster);
                    rolemaster.tras.Commit(); rolemaster.conn.Close();
                    return RedirectToAction("Index", "mRoleMasters", new { status = CONT.Home_statusChecker1 });
                }
                else if (Request["btnCheckerSubmit"] != null)
                {
                    if (CurrentStatus == CONT.NR)
                        _mRoleMaster.Status = CONT.NC;
                    else if (CurrentStatus == CONT.UR)
                        _mRoleMaster.Status = CONT.UC;

                    rolemaster.RoleMaster_UpdateAllStatus(_mRoleMaster);
                    rolemaster.tras.Commit(); rolemaster.conn.Close();
                    return RedirectToAction("Index", "mRoleMasters", new { status = CONT.Home_statusChecker1 });
                }
                else if (Request["btnApproverReject"] != null)
                {
                    if (CurrentStatus == CONT.NC)
                        _mRoleMaster.Status = CONT.NJ;
                    else if (CurrentStatus == CONT.UC)
                        _mRoleMaster.Status = CONT.UJ;

                    rolemaster.RoleMaster_UpdateAllStatus(_mRoleMaster);
                    rolemaster.tras.Commit(); rolemaster.conn.Close();
                    return RedirectToAction("Index", "mRoleMasters", new { status = CONT.Home_statusApprover });
                }
                else if (Request["btnApprove"] != null)
                {
                    if (CurrentStatus == CONT.NC)
                        _mRoleMaster.Status = CONT.NA;
                    else if (CurrentStatus == CONT.UC)
                        _mRoleMaster.Status = CONT.UA;

                    rolemaster.RoleMaster_UpdateAllStatus(_mRoleMaster);
                    rolemaster.tras.Commit(); rolemaster.conn.Close();
                    return RedirectToAction("Index", "mRoleMasters", new { status = CONT.Home_statusApprover });
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
            FillCustomerTypeCombo();
            return View();
        }

        public void FillCustomerTypeCombo()
        {
            ClsCustomerMaster _ClsCustomerMaster = new ClsCustomerMaster();
            var DDLCustomerTypeData = _ClsCustomerMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
            if (DDLCustomerTypeData != null && DDLCustomerTypeData.Count > 0)
                ViewBag.DDLCustTypID = DDLCustomerTypeData;
            else
                ViewBag.DDLCustTypID = new List<SelectListItem> { };
        }
        [HttpPost]
        public ActionResult RoleDetailsPopup(int RoleID = 0)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                mRoleMaster objlist = new mRoleMaster();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;

                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                    if (UserRight != null)
                    {
                        ViewBag.IsCheckerOrApprover = UserRight.MenuName;
                    }
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);

                    var GetDataInListAllBind = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, RoleID, 0, "", 0).ToList();
                    ViewBag.intMenuCount = GetDataInListAllBind.Count;
                    ViewBag.MenuRights = GetDataInListAllBind;
                    ViewBag.RoleID = GetDataInListAllBind[0].RoleID;
                    ViewBag.Status = GetDataInListAllBind[0].Status;

                    objlist.RoleID = GetDataInListAllBind[0].RoleID;
                    objlist.RoleName = GetDataInListAllBind[0].RoleName;
                    objlist.RoleDesc = GetDataInListAllBind[0].RoleDesc;
                    objlist.Status = GetDataInListAllBind[0].Status;
                    objlist.IsMaker = GetDataInListAllBind[0].IsMaker;
                    objlist.IsChecker = GetDataInListAllBind[0].IsChecker;
                    objlist.IsApprover = GetDataInListAllBind[0].IsApprover;
                    objlist.IsViewer = GetDataInListAllBind[0].IsViewer;

                    return View(objlist);
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
                return View(objlist);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult Create()
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                mRoleMaster obj = new mRoleMaster();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Maker", ParentMenuID);
                    if (UserRight != null)
                    {

                        int UserId = 0;
                        int.TryParse(LoginStatus[1].ToString(), out UserId);
                        var a = new Object();

                        string sKeywordvalue = "";
                        bool blsKeywordSearch = false;
                        obj.lstMenuMasterAdd = objMenuMaster.MenuMaster_ListAll(0, -1, "", 1, "", blsKeywordSearch, sKeywordvalue, 1).ToList();
                        return View(obj);
                    }
                    else
                    {
                        return RedirectToAction("NoaccessPage", "MasterPage");
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

                    return RedirectToAction("Index", "mRoleMasters");
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult Create(FormCollection frm, mRoleMaster mRoleMaster)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    rolemaster.conn = ClsAppDatabase.GetCon();
                    if (rolemaster.conn.State == ConnectionState.Closed)
                        rolemaster.conn.Open();
                    else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                    rolemaster.tras = rolemaster.conn.BeginTransaction();

                    string strRoleName = Request.Form["RoleName"];
                    string strRoleDesc = Request.Form["RoleDesc"];

                    int intMenuCount = Convert.ToInt32(Request.Form["intMenuCount"]);
                    List<MenuMaster> lstMenuMasterAdd = new List<MenuMaster>();
                    lstMenuMasterAdd = objMenuMaster.MenuMaster_ListAll(0, -1, "", 1, "", false, "", 1).ToList();

                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);

                    ObjectParameter outRoleID = new ObjectParameter("pRoleID", typeof(int));
                    if (mRoleMaster.RoleDesc == null)
                        mRoleMaster.RoleDesc = "";

                    Int32 RoleID = rolemaster.RoleMaster_Add(outRoleID, mRoleMaster.RoleName, mRoleMaster.RoleDesc, UserId, FN.GetSystemIP());

                    int MenuRightsID;
                    for (int i = 0; i < intMenuCount; i++)
                    {
                        int MenuID = lstMenuMasterAdd[i].MenuID;

                        string strSelectedWithEndTrim = "";
                        string strSelected = "";
                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];

                            if (GetAllkeysStringValue.Equals("chk_M_" + MenuID + "") || GetAllkeysStringValue.Equals("chk_C_" + MenuID + "") ||

                                GetAllkeysStringValue.Equals("chk_A_" + MenuID + "") || GetAllkeysStringValue.Equals("chk_V_" + MenuID + ""))
                            {
                                strSelected += GetAllkeysStringValue + ",";

                            }
                            strSelectedWithEndTrim = strSelected.TrimEnd(',');
                        }

                        if (strSelectedWithEndTrim == "")
                        {
                            ObjectParameter outMenuRightsID = new ObjectParameter("pMenuRightsID", typeof(int));
                            MenuRightsID = rolemaster.MenuRights_Add(outMenuRightsID, MenuID, RoleID, false, false, false, false, UserId, FN.GetSystemIP());
                        }
                        else
                        {
                            string[] strSelectedWithEndTrimSplit = strSelectedWithEndTrim.Split(',');
                            bool IsMekar = false; bool IsChecker = false; bool IsApproval = false; bool IsView = false;
                            int MenuIDChecked = 0;
                            for (int z = 0; z < strSelectedWithEndTrimSplit.Count(); z++)
                            {
                                string strSelectedWithEndTrimSplitValue = strSelectedWithEndTrimSplit[z];
                                string[] strSelectedWithEndTrimSplitValueSplit = strSelectedWithEndTrimSplitValue.Split('_');
                                MenuIDChecked = Convert.ToInt32(strSelectedWithEndTrimSplitValueSplit[2]);

                                if (strSelectedWithEndTrimSplitValueSplit[1] == "M")
                                { IsMekar = true; }
                                else if (strSelectedWithEndTrimSplitValueSplit[1] == "C")
                                { IsChecker = true; }
                                else if (strSelectedWithEndTrimSplitValueSplit[1] == "A")
                                { IsApproval = true; }
                                else if (strSelectedWithEndTrimSplitValueSplit[1] == "V")
                                { IsView = true; }
                            }
                            ObjectParameter outMenuRightsID = new ObjectParameter("pMenuRightsID", typeof(int));
                            MenuRightsID = rolemaster.MenuRights_Add(outMenuRightsID, MenuIDChecked,
                                RoleID, IsMekar, IsChecker, IsApproval, IsView, UserId, FN.GetSystemIP());
                        }
                    }
                    rolemaster.tras.Commit(); rolemaster.conn.Close();
                }
                catch (Exception ex)
                {
                    if (rolemaster.conn.State == ConnectionState.Open)
                    {
                        rolemaster.tras.Rollback();
                        rolemaster.conn.Close();
                    }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ex.Message;
                        ModelState.AddModelError("", ex.Message);
                    }
                    mRoleMaster.RoleDesc = mRoleMaster.RoleDesc;
                    mRoleMaster.lstMenuMasterAdd = objMenuMaster.MenuMaster_ListAll(0, -1, "", 1, "", false, "", 1).ToList();
                    return View(mRoleMaster);
                }
                return RedirectToAction("Index", "mRolemasters");
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult Edit(int? iRoleId, string Status)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                List<mRoleMaster> RoleMaster_ListAll_Result = new List<mRoleMaster>();
                mRoleMaster m = new mRoleMaster();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Maker", ParentMenuID);
                    if (UserRight != null)
                    {
                        int UserId = 0;
                        int.TryParse(LoginStatus[1].ToString(), out UserId);
                        if (iRoleId == null)
                        {
                            ViewBag.Error = "invelid Expression";
                            return RedirectToAction("Index", "mRolemasters");
                        }
                        string selectedmaker = ""; string selectedapporver = ""; string selectedchecker = ""; string selectedisview = "";

                        if (Status == CONT.UJ)
                        {
                            m.lstRoleMasterlstallbind = rolemaster.RoleMaster_ListAllNew(iRoleId, "", 1, "", false, "").ToList();
                            var data = objMenuRights.MenuRights_ListAllNew(0, 0, iRoleId, "", false, "").ToList();

                            for (int i = 0; i <= data.Count - 1; i++)
                            {
                                if (data[i].IsMaker == true)
                                { selectedmaker = selectedmaker + "," + data[i].MenuID; }
                                if (data[i].IsChecker == true)
                                { selectedchecker = selectedchecker + "," + data[i].MenuID; }
                                if (data[i].IsApprover == true)
                                { selectedapporver = selectedapporver + "," + data[i].MenuID; }
                                if (data[i].IsView == true)
                                { selectedisview = selectedisview + "," + data[i].MenuID; }
                            }
                            m.lstMenuRightslstallbind = data;
                        }
                        else
                        {
                            m.lstRoleMasterlstallbind = rolemaster.RoleMasterProcessHistory_ListAllBind(0, iRoleId, "", 0).ToList();
                            var data = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, iRoleId, 0, "", 0).ToList();

                            for (int i = 0; i <= data.Count - 1; i++)
                            {
                                if (data[i].IsMaker == true)
                                { selectedmaker = selectedmaker + "," + data[i].MenuID; }
                                if (data[i].IsChecker == true)
                                { selectedchecker = selectedchecker + "," + data[i].MenuID; }
                                if (data[i].IsApprover == true)
                                { selectedapporver = selectedapporver + "," + data[i].MenuID; }
                                if (data[i].IsView == true)
                                { selectedisview = selectedisview + "," + data[i].MenuID; }
                            }
                            m.lstMenuRightslstallbind = data;
                        }

                        ViewData["selectedmaker"] = selectedmaker;
                        ViewData["selectedchecker"] = selectedchecker;
                        ViewData["selectedapporver"] = selectedapporver;
                        ViewData["selectedisview"] = selectedisview;


                        if (RoleMaster_ListAll_Result == null)
                        { return HttpNotFound(); }
                        return View(m);
                    }
                    else
                    {
                        return RedirectToAction("NoaccessPage", "MasterPage");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ex.Message;
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection frm, mRoleMaster r)
        {
            int intRoleID = Convert.ToInt32(Request.Form["lstRoleMasterlstallbind[0].RoleID"]);
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);

                    string status = Request.Form["lstRoleMasterlstallbind[0].Status"];
                    string strRoleName = Request.Form["lstRoleMasterlstallbind[0].RoleName"];
                    string strRoleDesc = Request.Form["lstRoleMasterlstallbind[0].RoleDesc"];
                    int intMenuRightsCount = Convert.ToInt32(Request.Form["intMenuRightsCount"]);
                    string ProcessRemarks = Request.Form["ProcessRemark"];

                    var RoleMasterData = rolemaster.RoleMaster_ListAll(intRoleID, "", -1, "", false, "").ToList();

                    List<MenuRights> lstMenuRights = new List<MenuRights>();
                    lstMenuRights = objMenuRights.MenuRights_ListAll(0, 0, intRoleID, "", false, "").ToList();

                    rolemaster.conn = ClsAppDatabase.GetCon();
                    if (rolemaster.conn.State == ConnectionState.Closed)
                        rolemaster.conn.Open();
                    else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                    rolemaster.tras = rolemaster.conn.BeginTransaction();

                    if (!frm.AllKeys.Contains("chkDeleteRole"))
                    {
                        ObjectParameter outRoleid = new ObjectParameter("pRoleProcessHistoryID", typeof(int));
                        if (status == CONT.NM)
                        {
                            if (strRoleDesc == null)
                            { strRoleDesc = ""; }
                            rolemaster.RoleMaster_Update(intRoleID, strRoleName, strRoleDesc, UserId, FN.GetSystemIP());
                        }
                        else
                        {
                            if (strRoleDesc == null)
                            { strRoleDesc = ""; }
                            rolemaster.RoleMasterProcessHistory_Add(outRoleid, intRoleID, strRoleName, strRoleDesc, r.IsActive,
                             CONT.UR, ProcessRemarks, UserId, FN.GetSystemIP());
                        }

                        objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                        int MenuRightsProcessHistoryID;
                        for (int i = 0; i <= lstMenuRights.Count - 1; i++)
                        {
                            int MenuID = lstMenuRights[i].MenuID;
                            string strSelectedWithEndTrim = ""; string strSelected = "";
                            for (int j = 0; j < frm.Count; j++)
                            {
                                string GetAllkeysStringValue = frm.AllKeys[j];
                                if (GetAllkeysStringValue.Equals("chk_M_" + MenuID + "") || GetAllkeysStringValue.Equals("chk_C_" + MenuID + "") ||
                                    GetAllkeysStringValue.Equals("chk_A_" + MenuID + "") || GetAllkeysStringValue.Equals("chk_V_" + MenuID + ""))
                                {
                                    strSelected += GetAllkeysStringValue + ",";
                                }
                                strSelectedWithEndTrim = strSelected.TrimEnd(',');
                            }
                            if (strSelectedWithEndTrim == "")
                            {
                                if (status == CONT.NM)
                                {
                                    objMenuRights.MenuRights_Update(lstMenuRights[i].MenuRightsID, lstMenuRights[i].MenuID, intRoleID, false, false,
                                        false, false, UserId, FN.GetSystemIP());
                                }
                                else
                                {
                                    ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                    MenuRightsProcessHistoryID = objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, lstMenuRights[i].MenuRightsID, lstMenuRights[i].MenuID,
                                    intRoleID, false, false, false, false, CONT.UR, ProcessRemarks, UserId, FN.GetSystemIP());
                                }
                            }
                            else
                            {
                                string[] strSelectedWithEndTrimSplit = strSelectedWithEndTrim.Split(',');
                                bool IsMekar = false; bool IsChecker = false; bool IsApproval = false; bool IsView = false; int MenuIDChecked = 0;
                                for (int z = 0; z < strSelectedWithEndTrimSplit.Count(); z++)
                                {

                                    string strSelectedWithEndTrimSplitValue = strSelectedWithEndTrimSplit[z];
                                    string[] strSelectedWithEndTrimSplitValueSplit = strSelectedWithEndTrimSplitValue.Split('_');
                                    MenuIDChecked = Convert.ToInt32(strSelectedWithEndTrimSplitValueSplit[2]);

                                    if (strSelectedWithEndTrimSplitValueSplit[1] == "M")
                                    { IsMekar = true; }
                                    else if (strSelectedWithEndTrimSplitValueSplit[1] == "C")
                                    { IsChecker = true; }
                                    else if (strSelectedWithEndTrimSplitValueSplit[1] == "A")
                                    { IsApproval = true; }
                                    else if (strSelectedWithEndTrimSplitValueSplit[1] == "V")
                                    { IsView = true; }
                                }

                                if (status == CONT.NM)
                                {
                                    objMenuRights.MenuRights_Update(lstMenuRights[i].MenuRightsID, lstMenuRights[i].MenuID, intRoleID, IsMekar, IsChecker,
                                        IsApproval, IsView, UserId, FN.GetSystemIP());
                                }
                                else
                                {
                                    ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                    MenuRightsProcessHistoryID = objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, lstMenuRights[i].MenuRightsID, lstMenuRights[i].MenuID,
                                    intRoleID, IsMekar, IsChecker, IsApproval, IsView, CONT.UR, ProcessRemarks, UserId, FN.GetSystemIP());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (status == CONT.NA || status == CONT.UA || status == CONT.AA || status == CONT.IA)
                        {
                            ObjectParameter outRoleid = new ObjectParameter("pRoleProcessHistoryID", typeof(int));
                            if (strRoleDesc == null)
                            { strRoleDesc = ""; }
                            rolemaster.RoleMasterProcessHistory_Add(outRoleid, intRoleID, strRoleName, strRoleDesc, false, CONT.DR, ProcessRemarks, UserId, FN.GetSystemIP());

                            objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                            for (int i = 0; i < lstMenuRights.Count; i++)
                            {
                                string MenuRightsStatus = lstMenuRights[i].Status;
                                int MenuRightsID = lstMenuRights[i].MenuRightsID;
                                int MenuID = lstMenuRights[i].MenuID;
                                bool IsMaker = lstMenuRights[i].IsMaker;
                                bool IsChecker = lstMenuRights[i].IsChecker;
                                bool IsApprover = lstMenuRights[i].IsApprover;
                                bool IsView = lstMenuRights[i].IsView;

                                ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, MenuRightsID, MenuID, intRoleID, IsMaker, IsChecker, IsApprover, IsView, CONT.DR, ProcessRemarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    rolemaster.tras.Commit(); rolemaster.conn.Close();
                }
                catch (Exception ex)
                {
                    if (rolemaster.conn.State == ConnectionState.Open)
                    {
                        rolemaster.tras.Rollback();
                        rolemaster.conn.Close();
                    }

                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ex.Message;
                        ModelState.AddModelError("", ex.Message);
                    }
                    r.lstRoleMaster = rolemaster.RoleMaster_ListAll(intRoleID, "", -1, "", false, "").ToList();
                    r.lstMenuRights = objMenuRights.MenuRights_ListAll(0, 0, intRoleID, "", false, "").ToList();
                    return View(r);
                }
                return RedirectToAction("Index", "mRoleMasters");
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsAmend(int iRoleID, int iRoleProcessHistoryID, FormCollection frm, mRoleMaster m)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string Status = ""; string CurrentStatus = "";
                string ProcessRemarks = (frm["ProcessRemark"]);
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);

                    var data1 = rolemaster.RoleMasterProcessHistory_ListAllBind(iRoleProcessHistoryID, iRoleID, "", 0).ToList();
                    var data = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, iRoleID, 0, "", 0).ToList();

                    string Checkpage = string.Empty;
                    if (data1 != null)
                        Status = data1[0].Status;

                    if (Status == CONT.NR)
                    {
                        CurrentStatus = CONT.NM;
                        Checkpage = "CheckerAmend";
                    }
                    else if (Status == CONT.UR)
                    {
                        CurrentStatus = CONT.UM;
                        Checkpage = "CheckerAmend";
                    }
                    else if (Status == CONT.DR)
                    {
                        CurrentStatus = CONT.DC;
                        Checkpage = "CheckerAmend";
                    }
                    else if (Status == CONT.AR)
                    {
                        CurrentStatus = CONT.AC;
                        Checkpage = "CheckerAmend";
                    }
                    else if (Status == CONT.IR)
                    {
                        CurrentStatus = CONT.IC;
                        Checkpage = "CheckerAmend";
                    }

                    if (Checkpage == "CheckerAmend")
                    {
                        rolemaster.conn = ClsAppDatabase.GetCon();
                        if (rolemaster.conn.State == ConnectionState.Closed)
                            rolemaster.conn.Open();
                        else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                        rolemaster.tras = rolemaster.conn.BeginTransaction();
                        rolemaster.RoleMasterProcessHistory_ProcessStatus(iRoleProcessHistoryID, iRoleID, CurrentStatus, ProcessRemarks, UserId, FN.GetSystemIP());

                        objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                        for (int i = 0; i <= data.Count - 1; i++)
                        {
                            objMenuRights.MenuRightsProcessHistory_ProcessStatus(data[i].MenuRightsProcessHistoryID, data[i].MenuRightsID, UserId, FN.GetSystemIP(), CurrentStatus, ProcessRemarks);
                        }
                        rolemaster.tras.Commit(); rolemaster.conn.Close();
                        return RedirectToAction("CheckerList");
                    }

                    if (Status == CONT.NC)
                    {
                        CurrentStatus = CONT.NJ;
                        Checkpage = "ApproverReject";
                    }
                    else if (Status == CONT.UC)
                    {
                        CurrentStatus = CONT.UJ;
                        Checkpage = "ApproverReject";
                    }
                    else if (Status == CONT.DC)
                    {
                        CurrentStatus = CONT.DJ;
                        Checkpage = "ApproverReject";
                    }
                    else if (Status == CONT.AC)
                    {
                        CurrentStatus = CONT.AJ;
                        Checkpage = "ApproverReject";
                    }
                    else if (Status == CONT.IC)
                    {
                        CurrentStatus = CONT.IJ;
                        Checkpage = "ApproverReject";
                    }
                    if (Checkpage == "ApproverReject")
                    {
                        rolemaster.conn = ClsAppDatabase.GetCon();
                        if (rolemaster.conn.State == ConnectionState.Closed)
                            rolemaster.conn.Open();
                        else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                        rolemaster.tras = rolemaster.conn.BeginTransaction();

                        rolemaster.RoleMasterProcessHistory_ProcessStatus(iRoleProcessHistoryID, iRoleID, CurrentStatus, ProcessRemarks, UserId, FN.GetSystemIP());

                        objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                        for (int i = 0; i <= data.Count - 1; i++)
                        {
                            objMenuRights.MenuRightsProcessHistory_ProcessStatus(data[i].MenuRightsProcessHistoryID, data[i].MenuRightsID, UserId, FN.GetSystemIP(), CurrentStatus, ProcessRemarks);
                        }
                        rolemaster.tras.Commit(); rolemaster.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    if (rolemaster.conn.State == ConnectionState.Open)
                    {
                        rolemaster.tras.Rollback();
                        rolemaster.conn.Close();
                    }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ex.Message;
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                ViewData["roleid"] = iRoleID;
                ViewData["iRoleProcessHistoryID"] = iRoleProcessHistoryID;
                return View("Details", m);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public string Act_InA_Del(string ActionName, int RoleID, string ProcessRemarks)
        {
            string resp = ""; string Status = "";
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    rolemaster.conn = ClsAppDatabase.GetCon();
                    if (rolemaster.conn.State == ConnectionState.Closed)
                        rolemaster.conn.Open();
                    else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                    rolemaster.tras = rolemaster.conn.BeginTransaction();

                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);

                    var data1 = rolemaster.RoleMaster_ListAll(RoleID, "", -1, "", false, "").ToList();
                    var data = objMenuRights.MenuRights_ListAll(0, 0, RoleID, "", false, "").ToList();
                    ObjectParameter outRoleid = new ObjectParameter("pRoleProcessHistoryID", typeof(int));
                    string strRoleName = data1[0].RoleName;
                    string strRoleDesc = data1[0].RoleDesc;
                    if (data1 != null)
                        Status = data1[0].Status;

                    if (ActionName.Equals("Delete"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA || Status == CONT.IA)
                        {
                            if (strRoleDesc == null)
                            { strRoleDesc = ""; }
                            rolemaster.RoleMasterProcessHistory_Add(outRoleid, RoleID, strRoleName, strRoleDesc, data1[0].IsActive, CONT.DR, ProcessRemarks, UserId, FN.GetSystemIP());

                            objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                            for (int i = 0; i < data.Count; i++)
                            {
                                string MenuRightsStatus = data[i].Status;
                                int MenuRightsID = data[i].MenuRightsID;
                                int MenuID = data[i].MenuID;
                                bool IsMaker = data[i].IsMaker;
                                bool IsChecker = data[i].IsChecker;
                                bool IsApprover = data[i].IsApprover;
                                bool IsView = data[i].IsView;

                                ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, MenuRightsID, MenuID, RoleID, IsMaker, IsChecker, IsApprover, IsView, CONT.DR, ProcessRemarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    else if (ActionName.Equals("Active"))
                    {

                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.IA)
                        {
                            if (strRoleDesc == null)
                            { strRoleDesc = ""; }
                            rolemaster.RoleMasterProcessHistory_Add(outRoleid, RoleID, strRoleName, strRoleDesc, true, CONT.AR, ProcessRemarks, UserId, FN.GetSystemIP());

                            objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                            for (int i = 0; i < data.Count; i++)
                            {
                                string MenuRightsStatus = data[i].Status;
                                int MenuRightsID = data[i].MenuRightsID;
                                int MenuID = data[i].MenuID;
                                bool IsMaker = data[i].IsMaker;
                                bool IsChecker = data[i].IsChecker;
                                bool IsApprover = data[i].IsApprover;
                                bool IsView = data[i].IsView;

                                ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, MenuRightsID, MenuID, RoleID, IsMaker, IsChecker, IsApprover, IsView, CONT.AR, ProcessRemarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    else if (ActionName.Equals("InActive"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA)
                        {
                            if (strRoleDesc == null)
                            { strRoleDesc = ""; }

                            rolemaster.RoleMasterProcessHistory_Add(outRoleid, RoleID, strRoleName, strRoleDesc, false, CONT.IR, ProcessRemarks, UserId, FN.GetSystemIP());
                            objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                            for (int i = 0; i < data.Count; i++)
                            {
                                string MenuRightsStatus = data[i].Status;
                                int MenuRightsID = data[i].MenuRightsID;
                                int MenuID = data[i].MenuID;
                                bool IsMaker = data[i].IsMaker;
                                bool IsChecker = data[i].IsChecker;
                                bool IsApprover = data[i].IsApprover;
                                bool IsView = data[i].IsView;

                                ObjectParameter OutMenuRightsProcessHistoryID = new ObjectParameter("pMenuRightsProcessHistoryID", typeof(int));
                                objMenuRights.MenuRightsProcessHistory_Add(OutMenuRightsProcessHistoryID, MenuRightsID, MenuID, RoleID, IsMaker, IsChecker, IsApprover, IsView, CONT.IR, ProcessRemarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    rolemaster.tras.Commit(); rolemaster.conn.Close();
                    resp = "1$SuccessFully";
                }
                catch (Exception ex)
                {
                    if (rolemaster.conn.State == ConnectionState.Open)
                    {
                        rolemaster.tras.Rollback();
                        rolemaster.conn.Close();
                    }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    resp = "0$" + ex.Message;
                }
                return resp;
            }
            else
            {
                resp = "0$Redirect";
            }
            return resp;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detailsapprove(int? iRoleID, int? iRoleProcessHistoryID, FormCollection frm, mRoleMaster m)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string Status = "";
                string ProcessRemarks = (frm["ProcessRemark"]);
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);

                    var data1 = rolemaster.RoleMasterProcessHistory_ListAllBind(iRoleProcessHistoryID, iRoleID, "", 0).ToList();
                    var data = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, iRoleID, 0, "", 0).ToList();
                    if (data1 != null)
                        Status = data1[0].Status;
                    string Checkpage = string.Empty;
                    string ProcessStatus = string.Empty;
                    if (Status == CONT.NR) { ProcessStatus = CONT.NC; Checkpage = "CheckerApprove"; }
                    else if (Status == CONT.UR) { ProcessStatus = CONT.UC; Checkpage = "CheckerApprove"; }
                    else if (Status == CONT.DR) { ProcessStatus = CONT.DC; Checkpage = "CheckerApprove"; }
                    else if (Status == CONT.AR) { ProcessStatus = CONT.AC; Checkpage = "CheckerApprove"; }
                    else if (Status == CONT.IR) { ProcessStatus = CONT.IC; Checkpage = "CheckerApprove"; }
                    if (Checkpage == "CheckerApprove")
                    {
                        rolemaster.conn = ClsAppDatabase.GetCon();
                        if (rolemaster.conn.State == ConnectionState.Closed)
                            rolemaster.conn.Open();
                        else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                        rolemaster.tras = rolemaster.conn.BeginTransaction();

                        rolemaster.RoleMasterProcessHistory_ProcessStatus(iRoleProcessHistoryID, iRoleID, ProcessStatus, ProcessRemarks, UserId, FN.GetSystemIP());

                        objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;

                        for (int i = 0; i <= data.Count - 1; i++)
                        {
                            objMenuRights.MenuRightsProcessHistory_ProcessStatus(data[i].MenuRightsProcessHistoryID, data[i].MenuRightsID, UserId, FN.GetSystemIP(), ProcessStatus, ProcessRemarks);
                        }
                        rolemaster.tras.Commit(); rolemaster.conn.Close();
                        return RedirectToAction("CheckerList");
                    }

                    if (Status == CONT.NC) { ProcessStatus = CONT.NA; Checkpage = "ApproverApprove"; }
                    else if (Status == CONT.UC) { ProcessStatus = CONT.UA; Checkpage = "ApproverApprove"; }
                    else if (Status == CONT.DC) { ProcessStatus = CONT.DA; Checkpage = "ApproverApprove"; }
                    else if (Status == CONT.AC) { ProcessStatus = CONT.AA; Checkpage = "ApproverApprove"; }
                    else if (Status == CONT.IC) { ProcessStatus = CONT.IA; Checkpage = "ApproverApprove"; }
                    if (Checkpage == "ApproverApprove")
                    {
                        rolemaster.conn = ClsAppDatabase.GetCon();
                        if (rolemaster.conn.State == ConnectionState.Closed)
                            rolemaster.conn.Open();
                        else { rolemaster.conn.Close(); rolemaster.conn.Open(); }
                        rolemaster.tras = rolemaster.conn.BeginTransaction();

                        rolemaster.RoleMasterProcessHistory_ProcessStatus(iRoleProcessHistoryID, iRoleID, ProcessStatus, ProcessRemarks, UserId, FN.GetSystemIP());

                        objMenuRights.conn = rolemaster.conn; objMenuRights.tras = rolemaster.tras;
                        for (int i = 0; i <= data.Count - 1; i++)
                        {
                            objMenuRights.MenuRightsProcessHistory_ProcessStatus(data[i].MenuRightsProcessHistoryID, data[i].MenuRightsID, UserId, FN.GetSystemIP(), ProcessStatus, ProcessRemarks);
                        }
                        objMenuRights.tras.Commit(); objMenuRights.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    if (rolemaster.conn.State == ConnectionState.Open)
                    {
                        rolemaster.tras.Rollback(); rolemaster.conn.Close();
                    }

                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                        ModelState.AddModelError("", ex.InnerException.Message);
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ex.Message;
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                ViewBag.Data1 = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, iRoleID, 0, "", 0).ToList();
                ViewBag.Data = rolemaster.RoleMasterProcessHistory_ListAllBind(iRoleProcessHistoryID, iRoleID, "", 0).ToList();
                ViewData["roleid"] = iRoleID;
                ViewData["iCheck"] = "Checker";
                ViewData["iRoleProcessHistoryID"] = iRoleProcessHistoryID;
                return View("Details", m);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                ModelState.AddModelError("", ex.InnerException.Message);
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            { db.Dispose(); }
            base.Dispose(disposing);
        }
        public ActionResult DisableEditTimeData(int iRoleId)
        {
            List<mRoleMaster> RoleMaster_ListAll_Result = new List<mRoleMaster>();
            mRoleMaster m = new mRoleMaster();

            if (iRoleId == null)
            {
                ViewBag.Error = "invelid Expression";
                return RedirectToAction("Index", "mRolemasters");
            }
            m.lstRoleMaster = rolemaster.RoleMaster_ListAll(iRoleId, "", -1, "", false, "").ToList();
            m.lstMenuRights = objMenuRights.MenuRights_ListAll(0, 0, iRoleId, "", false, "").ToList();

            if (RoleMaster_ListAll_Result == null)
            { return HttpNotFound(); }
            return PartialView(m);
        }
        public ActionResult IsView(int? iRoleId)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                List<mRoleMaster> RoleMaster_ListAll_Result = new List<mRoleMaster>();
                mRoleMaster m = new mRoleMaster();
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    if (iRoleId == null)
                    {
                        ViewBag.Error = "invelid Expression";
                        return RedirectToAction("Index", "mRolemasters");
                    }
                    m.lstRoleMasterlstallbind = rolemaster.RoleMasterProcessHistory_ListAllBind(0, iRoleId, "", 0).ToList();

                    string selectedmaker = ""; string selectedapporver = ""; string selectedchecker = ""; string selectedisview = "";

                    var data = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, iRoleId, 0, "", 0).ToList();
                    for (int i = 0; i <= data.Count - 1; i++)
                    {
                        if (data[i].IsMaker == true)
                        { selectedmaker = selectedmaker + "," + data[i].MenuID; }
                        if (data[i].IsChecker == true)
                        { selectedchecker = selectedchecker + "," + data[i].MenuID; }
                        if (data[i].IsApprover == true)
                        { selectedapporver = selectedapporver + "," + data[i].MenuID; }
                        if (data[i].IsView == true)
                        { selectedisview = selectedisview + "," + data[i].MenuID; }
                    }

                    ViewData["selectedmaker"] = selectedmaker; ViewData["selectedchecker"] = selectedchecker;
                    ViewData["selectedapporver"] = selectedapporver; ViewData["selectedisview"] = selectedisview;

                    m.lstMenuRightslstallbind = data;
                    if (RoleMaster_ListAll_Result == null)
                    { return HttpNotFound(); }
                    return View(m);
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
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult UserRoleRightsView(int? iRoleId)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                List<mRoleMaster> RoleMaster_ListAll_Result = new List<mRoleMaster>();
                mRoleMaster m = new mRoleMaster();
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    if (iRoleId == null)
                    {
                        ViewBag.Error = "invelid Expression";
                        return RedirectToAction("Index", "mRolemasters");
                    }
                    m.lstRoleMasterlstallbind = rolemaster.RoleMasterProcessHistory_ListAllBind(0, iRoleId, "", 0).ToList();

                    string selectedmaker = ""; string selectedapporver = "";
                    string selectedchecker = ""; string selectedisview = "";

                    var data = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, iRoleId, 0, "", 0).ToList();
                    for (int i = 0; i <= data.Count - 1; i++)
                    {
                        if (data[i].IsMaker == true)
                        { selectedmaker = selectedmaker + "," + data[i].MenuID; }
                        if (data[i].IsChecker == true)
                        { selectedchecker = selectedchecker + "," + data[i].MenuID; }
                        if (data[i].IsApprover == true)
                        { selectedapporver = selectedapporver + "," + data[i].MenuID; }
                        if (data[i].IsView == true)
                        { selectedisview = selectedisview + "," + data[i].MenuID; }
                    }

                    ViewData["selectedmaker"] = selectedmaker; ViewData["selectedchecker"] = selectedchecker;
                    ViewData["selectedapporver"] = selectedapporver; ViewData["selectedisview"] = selectedisview;

                    m.lstMenuRightslstallbind = data;
                    if (RoleMaster_ListAll_Result == null)
                    { return HttpNotFound(); }

                    return PartialView(m);
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
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult RoleView(int? RoleId)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                List<mRoleMaster> RoleMaster_ListAll_Result = new List<mRoleMaster>();
                mRoleMaster m = new mRoleMaster();
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    if (RoleId == null)
                    {
                        ViewBag.Error = "invelid Expression";
                        return RedirectToAction("Index", "mRolemasters");
                    }
                    m.lstRoleMasterlstallbind = rolemaster.RoleMasterProcessHistory_ListAllBind(0, RoleId, "", 0).ToList();
                    ViewBag.MenuRights = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, RoleId, 0, "", 0).ToList();
                    FillCustomerTypeCombo();
                    if (RoleMaster_ListAll_Result == null)
                    { return HttpNotFound(); }

                    return PartialView(m);
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
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult Active_InActive_Del_Par(string ActionName, string RoleName)
        {
            ViewData["ActionName"] = ActionName;
            ViewData["RoleName"] = RoleName;
            return PartialView();
        }
        public ActionResult RoleMasterViewerIndex(int? Page)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
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
                            mRoleMaster objlist = new mRoleMaster();
                            objlist.lstRoleMasterlstallbind = rolemaster.RoleMasterProcessHistory_ListAllBind(0, 0, "", 0).ToList();
                            return View(objlist);
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

                    if (ex.InnerException != null)
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult RoleMasterViewer(int? iRoleID, int? iRoleProcessHistoryID, string icheck)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                List<mRoleMaster> RoleMaster_ListAll_Result = new List<mRoleMaster>();
                mRoleMaster m = new mRoleMaster();
                try
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
                            if (iRoleID == null)
                            {
                                ViewBag.Error = "invelid Expression";
                                return RedirectToAction("Index", "mRolemasters");
                            }
                            m.lstRoleMasterListBind = rolemaster.RoleMasterProcessHistory_ListAllBind(iRoleProcessHistoryID, iRoleID, "", 0).ToList();

                            string selectedmaker = ""; string selectedapporver = "";
                            string selectedchecker = ""; string selectedisview = "";

                            var data = objMenuRights.MenuRightsProcessHistory_ListAllBind(0, iRoleID, 0, "", 0).ToList();
                            for (int i = 0; i <= data.Count - 1; i++)
                            {
                                if (data[i].IsMaker == true)
                                { selectedmaker = selectedmaker + "," + data[i].MenuID; }
                                if (data[i].IsChecker == true)
                                { selectedchecker = selectedchecker + "," + data[i].MenuID; }
                                if (data[i].IsApprover == true)
                                { selectedapporver = selectedapporver + "," + data[i].MenuID; }
                                if (data[i].IsView == true)
                                { selectedisview = selectedisview + "," + data[i].MenuID; }
                            }

                            ViewData["selectedmaker"] = selectedmaker; ViewData["selectedchecker"] = selectedchecker;
                            ViewData["selectedapporver"] = selectedapporver; ViewData["selectedisview"] = selectedisview;
                            ViewData["roleid"] = iRoleID; ViewData["iCheck"] = icheck;
                            ViewData["iRoleProcessHistoryID"] = iRoleProcessHistoryID;

                            m.lstMenuRightsListBind = data;
                            if (RoleMaster_ListAll_Result == null)
                            { return HttpNotFound(); }
                            return View(m);
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

                    if (ex.InnerException != null)
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public PartialViewResult CommentResult(int RoleID)
        {
            ClsRoleMaster dbP = new ClsRoleMaster();
            ViewBag.lstCustomerMaster = dbP.RoleMaster_ListAllRemarks(RoleID).ToList();
            return PartialView();
        }
    }
}
