using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Net;
using System.Web.Mvc;
using TMP.Models;
using System.Data.Entity.Core.Objects;
using TMP.DAL;
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;
namespace TMP.Controllers
{
    public class mFeeMastersController : Controller
    {
        #region Local Variable
        Function FN = new Function();
        string indexStatus = CONT.Document_IndexStatus;
        ClsFeeMaster objFeeMaster = new ClsFeeMaster();
        ClsCustomerTypeMaster objCustomerTypeMaster = new ClsCustomerTypeMaster();
        #endregion
        public ActionResult Index(string sKeywordvalue)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
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
                        var data = objFeeMaster.FeeMaster_ListAll(0, "", -1, indexStatus, false, "");
                        ViewBag.UserID = UserId;
                        //ViewBag.UserRight = UserRight;
                        ViewBag.UserRight = UserRight.MenuName;
                        return View(data);
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
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult CreateUaer(int? id)
        {
            Session["MenuID"] = id;
            return RedirectToAction("Create", "mFeeMasters");
        }
        public ActionResult CheckerList(int? page)
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
                            ViewData["CheckerView"] = "Checker";
                            var data = objFeeMaster.FeeMasterProcessHistory_ListAllBind1(0, 0, CONT.Document_Checker, 0);
                            return View(data);
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
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult ApproverList(int? page)
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
                            ViewData["CheckerView"] = "Approver";
                            var data = objFeeMaster.FeeMasterProcessHistory_ListAllBind1(0, 0, CONT.HTMLApprover, 0);
                            return View(data);
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
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult Details(int? iFeeID, string icheck = "")
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
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
                            List<FeeMaster_ListAll_Result> FeeMaster_ListAll_Result = new List<FeeMaster_ListAll_Result>();
                            FeeMaster_ListAll_Model m = new FeeMaster_ListAll_Model();
                            if (iFeeID == null)
                            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                            if (icheck == null)
                            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                            m.lstFeeMasterListBind = objFeeMaster.FeeMasterProcessHistory_ListAllBind(0, iFeeID, "", 0).ToList();
                            m.lstFeeDetPHListBind = objFeeMaster.FeeDetailProcessHistory_ListAllBind(0, iFeeID, 0, CONT.Fee_Details, 0).ToList();
                            m.lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                            if (FeeMaster_ListAll_Result == null)
                            { return HttpNotFound(); }
                            ViewData["iCheck"] = icheck;
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
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult CreateNew()
        {
            return RedirectToAction("Create", "mFeeMasters");
        }
        public ActionResult Create()
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Maker", ParentMenuID);
                    if (UserRight != null)
                    {
                        var a = new Object();
                        FeeMaster_ListAll_Model obj = new FeeMaster_ListAll_Model();
                        obj.lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                        return View(obj);
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                    return RedirectToAction("Index", "mFeeMasters");
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeeMaster_ListAll_Model mFeeMaster, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string strFeeName = Request.Form["FeeName"];
                int intCustomerTypeMaster = Convert.ToInt32(Request.Form["intCustomerTypeMaster"]);

                objFeeMaster.conn = ClsAppDatabase.GetCon();
                if (objFeeMaster.conn.State == ConnectionState.Closed)
                { objFeeMaster.conn.Open(); }
                else { objFeeMaster.conn.Close(); objFeeMaster.conn.Open(); }
                objFeeMaster.tras = objFeeMaster.conn.BeginTransaction();

                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);

                try
                {
                    ObjectParameter outFeeID = new ObjectParameter("pFeeID", typeof(int));
                    bool IsFix;
                    if (frm.AllKeys.Contains("chkIsFix"))
                    { IsFix = true; }
                    else
                    { IsFix = false; }
                    int FeeID = objFeeMaster.FeeMaster_Add(outFeeID, strFeeName, IsFix, UserId, FN.GetSystemIP());

                    for (int j = 0; j < frm.Count; j++)
                    {
                        string GetAllkeysStringValue = frm.AllKeys[j];
                        if (GetAllkeysStringValue.Contains("chk_"))
                        {
                            string[] strSelected = GetAllkeysStringValue.Split('_');
                            int CustomerTypeID = Convert.ToInt32(strSelected[1]);
                            decimal txtPer; decimal txtAmt;

                            if (frm["txtper_" + CustomerTypeID + ""] == "")
                            { txtPer = 0; }
                            else
                            { txtPer = Convert.ToDecimal(frm["txtper_" + CustomerTypeID + ""]); }

                            if (frm["txtAmt_" + CustomerTypeID + ""] == "")
                            { txtAmt = 0; }
                            else
                            { txtAmt = Convert.ToDecimal(frm["txtAmt_" + CustomerTypeID + ""]); }

                            ObjectParameter outFeeDetId = new ObjectParameter("pFeeDetId", typeof(int));
                            objFeeMaster.FeeDetail_Add(outFeeDetId, FeeID, CustomerTypeID, txtPer, txtAmt, CONT.NR, UserId, FN.GetSystemIP());
                        }
                    }
                    objFeeMaster.tras.Commit();
                    objFeeMaster.conn.Close();
                }
                catch (Exception ex)
                {
                    objFeeMaster.tras.Rollback();
                    objFeeMaster.conn.Close();

                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                    mFeeMaster.lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                    return View(mFeeMaster);
                }
                return RedirectToAction("Index", "mFeeMasters");
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult Edit(int? id, string Status = "")
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                List<FeeMaster_ListAll_Result> FeeMaster_ListAll_Result = new List<FeeMaster_ListAll_Result>();
                FeeMaster_ListAll_Model m = new FeeMaster_ListAll_Model();
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
                        if (id == null)
                        { return RedirectToAction("Index", "mFeeMasters"); }
                        m.lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                        if (Status == CONT.UJ)
                        {
                            m.lstFeeMaster = objFeeMaster.FeeMaster_ListAll(id, "", 1, "", false, "").ToList();
                            m.lstFeeDetailMaster = objFeeMaster.FeeDetail_ListAll(0, id, 0, 1,
                           CONT.Document_IndexStatus,
                            false, "").ToList();
                        }
                        else
                        {
                            m.lstFeeMaster = objFeeMaster.FeeMasterProcessHistory_ListAllBind2(0, id, "", 0).ToList();
                            m.lstFeeDetailMaster = objFeeMaster.FeeDetailProcessHistory_ListAllBind1(0, id, 0,
                           CONT.Document_IndexStatus, 0).ToList();
                        }
                        return View(m);
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                    return View(m);
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FeeMaster_ListAll_Model mFeeMaster, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);

                int intCustomerTypeMaster = Convert.ToInt32(Request.Form["intCustomerTypeMaster"]);
                string ProcessRemarks = Request.Form["ProcessRemark"];

                List<FeeDetail_ListAll_Result> lstFeeDetail = new List<FeeDetail_ListAll_Result>();
                lstFeeDetail = objFeeMaster.FeeDetail_ListAll(0, mFeeMaster.FeeId, 0, -1,
              CONT.Document_Edit, false, "").ToList();

                List<CustomerTypeMaster_ListAll_Result> lstCustomerTypeMaster = new List<CustomerTypeMaster_ListAll_Result>();
                lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();

                objFeeMaster.conn = ClsAppDatabase.GetCon();
                if (objFeeMaster.conn.State == ConnectionState.Closed)
                { objFeeMaster.conn.Open(); }
                else { objFeeMaster.conn.Close(); objFeeMaster.conn.Open(); }
                objFeeMaster.tras = objFeeMaster.conn.BeginTransaction();

                try
                {
                    bool IsFix;
                    if (frm.AllKeys.Contains("chkIsFix"))
                    { IsFix = true; }
                    else
                    { IsFix = false; }
                    if (mFeeMaster.Status == CONT.NM)
                    { objFeeMaster.FeeMaster_Update(mFeeMaster.FeeId, mFeeMaster.FeeName, IsFix, UserId, FN.GetSystemIP()); }
                    else
                    {
                        ObjectParameter outpFeeProcessHistoryID = new ObjectParameter("pFeeProcessHistoryID", typeof(int));
                        objFeeMaster.FeeMasterProcessHistory_Add(outpFeeProcessHistoryID, mFeeMaster.FeeId, mFeeMaster.FeeName, IsFix, mFeeMaster.IsActive,CONT.UR, ProcessRemarks, UserId, FN.GetSystemIP());
                    }

                    for (int j = 0; j < frm.Count; j++)
                    {
                        string GetAllkeysStringValue = frm.AllKeys[j];

                        for (int z = 0; z < lstFeeDetail.Count; z++)
                        {
                            int CustomerTypeIDExistInFeeDetail = lstFeeDetail[z].CustomerTypeId;

                            int FeeDetID = lstFeeDetail[z].FeeDetId;
                            if (GetAllkeysStringValue.Equals("chk_" + CustomerTypeIDExistInFeeDetail + " _" + FeeDetID + ""))
                            {
                                decimal txtPer; decimal txtAmt;

                                if (frm["txtper_" + CustomerTypeIDExistInFeeDetail + " _" + FeeDetID + ""] == "")
                                { txtPer = 0; }
                                else
                                { txtPer = Convert.ToDecimal(frm["txtper_" + CustomerTypeIDExistInFeeDetail + " _" + FeeDetID + ""]); }

                                if (frm["txtAmt_" + CustomerTypeIDExistInFeeDetail + " _" + FeeDetID + ""] == "")
                                { txtAmt = 0; }
                                else
                                { txtAmt = Convert.ToDecimal(frm["txtAmt_" + CustomerTypeIDExistInFeeDetail + " _" + FeeDetID + ""]); }

                                if (mFeeMaster.Status == CONT.NM)
                                {
                                    objFeeMaster.FeeDetail_Update(FeeDetID, mFeeMaster.FeeId, CustomerTypeIDExistInFeeDetail, txtPer, txtAmt, UserId, FN.GetSystemIP());
                                }
                                else
                                {
                                    ObjectParameter outpFeeDetProcessHistoryID = new ObjectParameter("pFeeDetProcessHistoryID", typeof(int));
                                    objFeeMaster.FeeDetailProcessHistory_Add(outpFeeDetProcessHistoryID, FeeDetID, mFeeMaster.FeeId, CustomerTypeIDExistInFeeDetail, txtPer, txtAmt, mFeeMaster.IsActive, CONT.UR, ProcessRemarks, UserId, FN.GetSystemIP());
                                }
                            }
                        }
                    }

                    for (int i = 0; i < lstCustomerTypeMaster.Count; i++)
                    {
                        int CustomerTypeID = lstCustomerTypeMaster[i].CustomerTypeID;
                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("chk_" + CustomerTypeID + ""))
                            {
                                decimal txtPerInsert; decimal txtAmtinsert;

                                if (frm["txtper_" + CustomerTypeID + ""] == "")
                                { txtPerInsert = 0; }
                                else
                                { txtPerInsert = Convert.ToDecimal(frm["txtper_" + CustomerTypeID + ""]); }

                                if (frm["txtAmt_" + CustomerTypeID + ""] == "")
                                { txtAmtinsert = 0; }
                                else
                                { txtAmtinsert = Convert.ToDecimal(frm["txtAmt_" + CustomerTypeID + ""]); }

                                if (mFeeMaster.Status == CONT.NM)
                                {
                                    ObjectParameter outFeeDetId = new ObjectParameter("pFeeDetId", typeof(int));
                                    objFeeMaster.FeeDetail_Add(outFeeDetId, mFeeMaster.FeeId, CustomerTypeID, txtPerInsert, txtAmtinsert, CONT.NR, UserId, FN.GetSystemIP());
                                }
                                else
                                {
                                    ObjectParameter outFeeDetId = new ObjectParameter("pFeeDetId", typeof(int));
                                    objFeeMaster.FeeDetail_Add(outFeeDetId, mFeeMaster.FeeId, CustomerTypeID, txtPerInsert, txtAmtinsert, CONT.UR, UserId, FN.GetSystemIP());
                                }
                            }
                        }
                    }
                    for (int j = 0; j < frm.Count; j++)
                    {
                        string GetAllkeysStringValue = frm.AllKeys[j];
                        if (GetAllkeysStringValue.Contains("chkDelete_"))
                        {
                            string[] a = GetAllkeysStringValue.Split('_');
                            int FeeDetID = Convert.ToInt32(a[1]);
                            if (mFeeMaster.Status == CONT.NM)
                            {

                            }
                            else
                            {
                                ObjectParameter outpFeeDetProcessHistoryID = new ObjectParameter("pFeeDetProcessHistoryID", typeof(int));
                                objFeeMaster.FeeDetailProcessHistory_Add(outpFeeDetProcessHistoryID, FeeDetID, lstFeeDetail[0].FeeId,
                                lstFeeDetail[0].CustomerTypeId, lstFeeDetail[0].FeePer, lstFeeDetail[0].FeeAmt, false, CONT.UR,
                                ProcessRemarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    objFeeMaster.tras.Commit();
                    objFeeMaster.conn.Close();
                }
                catch (Exception ex)
                {
                    objFeeMaster.tras.Rollback();
                    objFeeMaster.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                    mFeeMaster.lstFeeMaster = objFeeMaster.FeeMaster_ListAll(mFeeMaster.FeeId, "", -1, "", false, "").ToList();
                    mFeeMaster.lstFeeDetailMaster = objFeeMaster.FeeDetail_ListAll(0, mFeeMaster.FeeId, 0, -1, "", false, "").ToList();
                    mFeeMaster.lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                    return View(mFeeMaster);
                }
                return RedirectToAction("Index", "mFeeMasters");
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsAmend(int FeeID, FormCollection frm, FeeMaster_ListAll_Model M)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string Status = ""; string CurrentStatus = "";
                string ProcessRemarks = (frm["ProcessRemark"]);
                string Checkpage = string.Empty;
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);

                    var data1 = objFeeMaster.FeeMasterProcessHistory_ListAllBind1(0, FeeID, "", 0).ToList();
                    var data = objFeeMaster.FeeDetailProcessHistory_ListAllBind(0, FeeID, 0, CONT.Fee_Amend, 0).ToList();
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
                        objFeeMaster.conn = ClsAppDatabase.GetCon();
                        if (objFeeMaster.conn.State == ConnectionState.Closed)
                        { objFeeMaster.conn.Open(); }
                        else { objFeeMaster.conn.Close(); objFeeMaster.conn.Open(); }
                        objFeeMaster.tras = objFeeMaster.conn.BeginTransaction();
                        objFeeMaster.FeeMasterProcessHistory_ProcessStatus(data1[0].FeeProcessHistoryID, FeeID, CurrentStatus, ProcessRemarks == null ? "" : ProcessRemarks, UserId,
                        FN.GetSystemIP());

                        objFeeMaster.tras.Commit();
                        objFeeMaster.conn.Close();
                        return RedirectToAction("CheckerList");
                    }
                    if (Status == CONT.NC) { CurrentStatus = CONT.NJ; Checkpage = "ApproverReject"; }
                    else if (Status == CONT.UC) { CurrentStatus = CONT.UJ; Checkpage = "ApproverReject"; }
                    else if (Status == CONT.DC) { CurrentStatus = CONT.DJ; Checkpage = "ApproverReject"; }
                    else if (Status == CONT.AC) { CurrentStatus = CONT.AJ; Checkpage = "ApproverReject"; }
                    else if (Status == CONT.IC) { CurrentStatus = CONT.IJ; Checkpage = "ApproverReject"; }
                    if (Checkpage == "ApproverReject")
                    {
                        objFeeMaster.conn = ClsAppDatabase.GetCon();
                        if (objFeeMaster.conn.State == ConnectionState.Closed)
                        { objFeeMaster.conn.Open(); }
                        else { objFeeMaster.conn.Close(); objFeeMaster.conn.Open(); }
                        objFeeMaster.tras = objFeeMaster.conn.BeginTransaction();
                        objFeeMaster.FeeMasterProcessHistory_ProcessStatus(data1[0].FeeProcessHistoryID, FeeID, CurrentStatus, ProcessRemarks == null ? "" : ProcessRemarks, UserId, FN.GetSystemIP());


                        objFeeMaster.tras.Commit();
                        objFeeMaster.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    objFeeMaster.tras.Rollback();
                    objFeeMaster.conn.Close();

                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (Checkpage == "CheckerApprove")
                    { RedirectToAction("Details", "mFeeMasters", new { iFeeID = FeeID, icheck = "Checker" }); }
                    else
                    { RedirectToAction("Details", "mFeeMasters", new { iFeeID = FeeID, icheck = "Approver" }); }
                }
                return View("Details", M);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult Detailsapprove(int FeeID, FormCollection frm, FeeMaster_ListAll_Model M)
        {
            string Status = "";
            string ProcessRemarks = (frm["ProcessRemark"]);
            string Checkpage = string.Empty;
            string ProcessStatus = string.Empty;
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    var data1 = objFeeMaster.FeeMasterProcessHistory_ListAllBind1(0, FeeID, "", 0).ToList();
                    var data = objFeeMaster.FeeDetailProcessHistory_ListAllBind(0, FeeID, 0, CONT.Fee_Approve, 0).ToList();
                    if (data1 != null)
                        Status = data1[0].Status;

                    if (Status == CONT.NR) { ProcessStatus = CONT.NC; Checkpage = "CheckerApprove"; }
                    else if (Status == CONT.UR) { ProcessStatus = CONT.UC; Checkpage = "CheckerApprove"; }
                    else if (Status == CONT.DR) { ProcessStatus = CONT.DC; Checkpage = "CheckerApprove"; }
                    else if (Status == CONT.AR) { ProcessStatus = CONT.AC; Checkpage = "CheckerApprove"; }
                    else if (Status == CONT.IR) { ProcessStatus = CONT.IC; Checkpage = "CheckerApprove"; }

                    if (Checkpage == "CheckerApprove")
                    {
                        objFeeMaster.conn = ClsAppDatabase.GetCon();
                        if (objFeeMaster.conn.State == ConnectionState.Closed)
                        { objFeeMaster.conn.Open(); }
                        else { objFeeMaster.conn.Close(); objFeeMaster.conn.Open(); }
                        objFeeMaster.tras = objFeeMaster.conn.BeginTransaction();

                        objFeeMaster.FeeMasterProcessHistory_ProcessStatus(data1[0].FeeProcessHistoryID, FeeID, ProcessStatus, ProcessRemarks == null ? "" : ProcessRemarks, UserId, FN.GetSystemIP());

                        objFeeMaster.tras.Commit();
                        objFeeMaster.conn.Close();
                        return RedirectToAction("CheckerList");
                    }

                    if (Status == CONT.NC) { ProcessStatus = CONT.NA; Checkpage = "ApproverApprove"; }
                    else if (Status == CONT.UC) { ProcessStatus = CONT.UA; Checkpage = "ApproverApprove"; }
                    else if (Status == CONT.DC) { ProcessStatus = CONT.DA; Checkpage = "ApproverApprove"; }
                    else if (Status == CONT.AC) { ProcessStatus = CONT.AA; Checkpage = "ApproverApprove"; }
                    else if (Status == CONT.IC) { ProcessStatus = CONT.IA; Checkpage = "ApproverApprove"; }
                    if (Checkpage == "ApproverApprove")
                    {
                        objFeeMaster.conn = ClsAppDatabase.GetCon();
                        if (objFeeMaster.conn.State == ConnectionState.Closed)
                        { objFeeMaster.conn.Open(); }
                        else { objFeeMaster.conn.Close(); objFeeMaster.conn.Open(); }
                        objFeeMaster.tras = objFeeMaster.conn.BeginTransaction();

                        objFeeMaster.FeeMasterProcessHistory_ProcessStatus(data1[0].FeeProcessHistoryID, FeeID, ProcessStatus, ProcessRemarks == null ? "" : ProcessRemarks, UserId,
                        FN.GetSystemIP());

                        objFeeMaster.tras.Commit();
                        objFeeMaster.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    objFeeMaster.tras.Rollback();
                    objFeeMaster.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                    if (Checkpage == "CheckerApprove")
                    { RedirectToAction("Details", "mFeeMasters", new { iFeeID = FeeID, icheck = "Checker" }); }
                    else
                    { RedirectToAction("Details", "mFeeMasters", new { iFeeID = FeeID, icheck = "Approver" }); }
                }
                return View("Details", M);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public string Act_InA_Del(string ActionName, int FeeID, string ProcessRemarks)
        {
            string resp = ""; string Status = "";
            var TemplateName = objFeeMaster.FeeDetail_ListAll(0, FeeID, 0, -1, "", false, "").ToList();
            ViewData["TemplateName"] = TemplateName[0].FeeName;
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                objFeeMaster.conn = ClsAppDatabase.GetCon();
                if (objFeeMaster.conn.State == ConnectionState.Closed)
                { objFeeMaster.conn.Open(); }
                else { objFeeMaster.conn.Close(); objFeeMaster.conn.Open(); }
                objFeeMaster.tras = objFeeMaster.conn.BeginTransaction();

                try
                {
                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    var data = objFeeMaster.FeeDetail_ListAll(0, FeeID, 0, -1, "", false, "").ToList();
                    var data2 = objFeeMaster.FeeDetailProcessHistory_ListAllBind(0, FeeID, 0, CONT.Fee_Del, 0).ToList();
                    ObjectParameter outFeeProcessHistoryID = new ObjectParameter("pFeeProcessHistoryID", typeof(int));
                    if (data != null)
                        Status = data[0].Status;

                    if (ActionName.Equals("Delete"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA || Status == CONT.IA)
                        {
                            objFeeMaster.FeeMasterProcessHistory_Add(outFeeProcessHistoryID, FeeID, data[0].FeeName, false,
                            data[0].IsActive, CONT.DR, ProcessRemarks, UserId, FN.GetSystemIP());
                            for (int i = 0; i < data2.Count; i++)
                            {
                                ObjectParameter OutFeeDetProcessHistoryID = new ObjectParameter("pFeeDetProcessHistoryID", typeof(int));
                                objFeeMaster.FeeDetailProcessHistory_Add(OutFeeDetProcessHistoryID, data2[i].FeeDetID, data2[i].FeeId, data2[i].CustomerTypeID
                                    , data2[i].FeePer, data2[i].FeeAmt, data2[i].IsActive,
                                    CONT.DR, ProcessRemarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    else if (ActionName.Equals("Active"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.IA)
                        {
                            objFeeMaster.FeeMasterProcessHistory_Add(outFeeProcessHistoryID, FeeID, data[0].FeeName, false, true, CONT.AR, ProcessRemarks, UserId, FN.GetSystemIP());
                            for (int i = 0; i < data2.Count; i++)
                            {
                                ObjectParameter OutFeeDetProcessHistoryID = new ObjectParameter("pFeeDetProcessHistoryID", typeof(int));
                                objFeeMaster.FeeDetailProcessHistory_Add(OutFeeDetProcessHistoryID, data2[i].FeeDetID, data2[i].FeeId, data2[i].CustomerTypeID,
                                data2[i].FeePer, data2[i].FeeAmt, true, CONT.AR, ProcessRemarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    else if (ActionName.Equals("InActive"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA)
                        {
                            objFeeMaster.FeeMasterProcessHistory_Add(outFeeProcessHistoryID, FeeID, data[0].FeeName, false,
                            false, CONT.IR, ProcessRemarks, UserId, FN.GetSystemIP());

                            for (int i = 0; i < data2.Count; i++)
                            {
                                ObjectParameter OutFeeDetProcessHistoryID = new ObjectParameter("pFeeDetProcessHistoryID", typeof(int));
                                objFeeMaster.FeeDetailProcessHistory_Add(OutFeeDetProcessHistoryID, data2[i].FeeDetID, data2[i].FeeId, data2[i].CustomerTypeID,
                                data2[i].FeePer, data2[i].FeeAmt, false, CONT.IR, ProcessRemarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    objFeeMaster.tras.Commit();
                    objFeeMaster.conn.Close();
                    resp = "1$SuccessFully";
                }
                catch (Exception ex)
                {
                    objFeeMaster.tras.Rollback();
                    objFeeMaster.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    resp = "0$" + ex.Message;
                }
                return resp;
            }
            else
            { resp = "0$Redirect"; }
            return resp;
        }
        public ActionResult IsView(int? iFeeID)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    List<FeeMaster_ListAll_Result> FeeMaster_ListAll_Result = new List<FeeMaster_ListAll_Result>();
                    FeeMaster_ListAll_Model m = new FeeMaster_ListAll_Model();
                    if (iFeeID == null)
                    {
                        return RedirectToAction("Index", "mFeeMasters");
                    }
                    m.lstFeeMaster = objFeeMaster.FeeMaster_ListAll(iFeeID, "", -1, "", false, "").ToList();
                    m.lstFeeDetailMaster = objFeeMaster.FeeDetail_ListAll(0, iFeeID, 0, -1, "", false, "").ToList();
                    m.lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                    if (FeeMaster_ListAll_Result == null)
                    { return HttpNotFound(); }
                    return View(m);
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult View(int? iFeeID)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);

                    int UserId = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UserId);
                    List<FeeMaster_ListAll_Result> FeeMaster_ListAll_Result = new List<FeeMaster_ListAll_Result>();
                    FeeMaster_ListAll_Model m = new FeeMaster_ListAll_Model();

                    m.lstFeeMasterListBind = objFeeMaster.FeeMasterProcessHistory_ListAllBind(0, iFeeID, "", 0).ToList();
                    m.lstFeeDetPHListBind = objFeeMaster.FeeDetailProcessHistory_ListAllBind(0, iFeeID, 0, CONT.Fee_View, 0).ToList();
                    m.lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                    if (FeeMaster_ListAll_Result == null)
                    { return HttpNotFound(); }
                    return PartialView(m);
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);

                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public string CheckFeeName(string input, int FeeID)
        {
            if (input != null && input != "")
            {
                var FeeName = objFeeMaster.FeeMaster_ListAll(0, input, -1, "", false, "").FirstOrDefault();

                if (FeeName == null)
                { return "Available"; }
                else
                {
                    if (FeeID == 0)
                    { return "Not Available"; }
                    else
                    {
                        if (FeeName.FeeId == FeeID)
                        { return "Available"; }
                        else
                        { return "Not Available"; }
                    }
                }
            }
            return "";
        }
        public ActionResult Active_InActive_Del_Par(string ActionName, string FeesName)
        {
            ViewData["ActionName"] = ActionName;
            ViewData["FeesName"] = FeesName;
            return PartialView();
        }
        public ActionResult FeeMasterViewerIndex(int? page)
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

                            var data = objFeeMaster.FeeMasterProcessHistory_ListAllBind1(0, 0, "", 0);
                            return View(data);
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
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult FeeMasterViewer(int? iFeeID)
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
                            List<FeeMaster_ListAll_Result> FeeMaster_ListAll_Result = new List<FeeMaster_ListAll_Result>();
                            FeeMaster_ListAll_Model m = new FeeMaster_ListAll_Model();
                            if (iFeeID == null)
                            { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                            m.lstFeeMasterListBind = objFeeMaster.FeeMasterProcessHistory_ListAllBind(0, iFeeID, "", 0).ToList();
                            m.lstFeeDetPHListBind = objFeeMaster.FeeDetailProcessHistory_ListAllBind(0, iFeeID, 0, CONT.Fee_Viewer, 0).ToList();
                            m.lstCustomerTypeMaster = objCustomerTypeMaster.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                            if (FeeMaster_ListAll_Result == null)
                            {
                                return HttpNotFound();
                            }
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
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public PartialViewResult CommentResult(int FeeId)
        {
            ClsFeeMaster dbP = new ClsFeeMaster();
            ViewBag.lstCustomerMaster = dbP.FeeMaster_ListAllRemarks(FeeId).ToList();
            return PartialView();
        }
    }
}