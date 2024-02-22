using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;
using TMP.DAL;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;

namespace TMP.Controllers
{
    public class HTMLTemplateController : Controller
    {
        ConString db = new ConString();
        ClsHTMLTemplate _clsTemp = new ClsHTMLTemplate();
        Function FN = new Function();
        private DataTable _dt;
        public DataTable DT
        {
            get { return _dt; }
            set { _dt = value; }
        }

        public ActionResult Index(HTMLTemplate_ListAll_Result HTML)
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
                        if (UserRight.IsMaker)
                        {
                            ViewBag.UserRight = UserRight;
                            var data = _clsTemp.HTMLTemplate_ListAll(0, "", -1, "", false, "", 0).ToList();
                            ViewBag.StatusList = data.Select(dataRow => new mRoleMaster
                            {
                                StatusUserDesc = dataRow.StatusUserDesc
                            }).GroupBy(model => model.StatusUserDesc).Select(group => group.First());

                            return View(data);
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
                    if (ex.InnerException == null)
                        ViewBag.ErrorMesssage = ex.Message;
                    else
                        ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View(HTML);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult Editor(HTMLTemplate_ListAll_Result HTML)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (frm.AllKeys.Contains("createnew"))
                    {
                        return RedirectToAction("Index", "HTMLTemplate");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    }
                    else
                    {
                        ViewBag.ErrorMsg = ex.Message;
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
        public PartialViewResult ViewEmailSample(int? id, HTMLTemplate_ListAll_Result html)
        {
            try
            {
                if (html.HtmlContent != null && html.HtmlContent != "")
                {

                    html.HtmlText = html.HtmlContent;
                    if (html.Subject != null && html.Subject != "")
                    {
                        html.Subject = html.Subject;
                    }
                    return PartialView(html);
                }
                else if (html.SMSText != null && html.SMSText != "")
                {
                    html.HtmlText = html.SMSText;
                    return PartialView(html);
                }
                else
                {
                    if (html.isEmailvalue != null)
                    {
                        html.HtmlContent = "";

                        html.HtmlText = html.HtmlContent;
                        if (html.Subject != null && html.Subject != "")
                        {

                            html.Subject = html.Subject;
                        }
                        return PartialView(html);
                    }
                    else if (html.issmsvalue != null)
                    {
                        html.SMSText = "";

                        html.HtmlText = html.SMSText;
                        return PartialView(html);
                    }
                    else
                    {
                        html.HtmlContent = "";

                        html.HtmlText = html.HtmlContent;
                        if (html.Subject != null && html.Subject != "")
                        {
                            html.Subject = html.Subject;
                        }
                        return PartialView(html);
                    }
                }
                return PartialView(html);
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                throw;
            }
        }
        public ActionResult Test()
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    HTMLTemplate_ListAll_Result html = new HTMLTemplate_ListAll_Result();
                    return View(html);
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    throw;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult Create(int? id, HTMLTemplate_ListAll_Result html)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    DataSet ds = GetDataSetUser("InvoiceHTMLDetail", 1);
                    DataTable DT = ds.Tables[0];
                    if (DT != null && DT.Rows.Count > 0)
                    {
                        string[] ColumnName = DT.Columns.Cast<DataColumn>()
                            .Select(x => x.ColumnName)
                            .ToArray();
                        SelectList list = new SelectList(ColumnName);
                        ViewBag.ColumnsName = list;
                    }

                    FillTemplateCombo();
                    if (id != null)
                    {
                        var objHTMLTemplate_ListAll_Result = _clsTemp.HTMLTemplate_ListAll(id, "", -1, "", false, "", 0).FirstOrDefault();
                        if (objHTMLTemplate_ListAll_Result == null)
                        {
                            return HttpNotFound();
                        }
                        html.HtmlTemplateID = objHTMLTemplate_ListAll_Result.HtmlTemplateID;
                        html.HtmlName = objHTMLTemplate_ListAll_Result.HtmlName;
                        if (objHTMLTemplate_ListAll_Result.IsSMS == true)
                        {
                            objHTMLTemplate_ListAll_Result.IsSMS = true;
                            html.issmsvalue = "SMS";
                            html.SMSText = objHTMLTemplate_ListAll_Result.HtmlText;
                        }
                        else
                        {
                            objHTMLTemplate_ListAll_Result.IsSMS = true;
                            html.isEmailvalue = "Email";
                            html.RichText1FullHtml = objHTMLTemplate_ListAll_Result.HtmlText;
                            ViewData["check"] = objHTMLTemplate_ListAll_Result.HtmlText;
                            html.SMSText = "";

                        }
                        html.IsSMS = objHTMLTemplate_ListAll_Result.IsSMS;
                        html.RichText1FullHtml = objHTMLTemplate_ListAll_Result.RichText1FullHtml;
                        return View(objHTMLTemplate_ListAll_Result);
                    }
                    return View();
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException != null)
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        [HttpPost]
        public ActionResult Create(HTMLTemplate_ListAll_Result html, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);

                string SMS = Request.Form["IsSMS"];
                
                ObjectContext octx = ((IObjectContextAdapter)db).ObjectContext;
                octx.Connection.Open();
                IDbTransaction trans = octx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                try
                {
                    html.RichText1FullHtml = html.RichText1;
                    html.RichText1 = null;
                    if (html.HtmlTemplateID == 0)
                    {
                        ObjectParameter HtmlTemplateID = new ObjectParameter("pHtmlTemplateID", typeof(int));
                        if (frm["issmsvalue"] == "SMS")
                            _clsTemp.HTMLTemplate_Add(HtmlTemplateID, html.HtmlName, frm["SMSText"], html.Subject, html.TemplateID, true, false, UserId, FN.GetSystemIP());
                        else if (frm["isEmailvalue"] == "Email")
                            _clsTemp.HTMLTemplate_Add(HtmlTemplateID, html.HtmlName, html.RichText1FullHtml, html.Subject, html.TemplateID, false, false, UserId, FN.GetSystemIP());
                        else if (frm["isMemovalue"] == "Memo")
                            _clsTemp.HTMLTemplate_Add(HtmlTemplateID, html.HtmlName, html.RichText1FullHtml, html.Subject, html.TemplateID, false, true, UserId, FN.GetSystemIP());
                    }
                    else
                    {
                        if (html.Status == CONT.NM)
                        {
                            if (frm["issmsvalue"] == "SMS")
                                _clsTemp.HTMLTemplate_Update(html.HtmlTemplateID, html.HtmlName, frm["SMSText"], html.Subject, html.TemplateID, true, false, UserId, FN.GetSystemIP());
                            else if (frm["isEmailvalue"] == "Email")
                                _clsTemp.HTMLTemplate_Update(html.HtmlTemplateID, html.HtmlName, html.RichText1FullHtml, html.Subject, html.TemplateID, false, false, UserId, FN.GetSystemIP());
                            else if (frm["isMemovalue"] == "Memo")
                                _clsTemp.HTMLTemplate_Update(html.HtmlTemplateID, html.HtmlName, html.RichText1FullHtml, html.Subject, html.TemplateID, false, true, UserId, FN.GetSystemIP());

                        }
                        else if (html.Status == CONT.NA || html.Status == CONT.UA || html.Status == CONT.UM)
                        {
                            ObjectParameter pHtmlTemplateProcessHistoryID = new ObjectParameter("pHtmlTemplateProcessHistoryID", typeof(int));
                            if (frm["issmsvalue"] == "SMS")
                                _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, html.HtmlTemplateID, html.HtmlName, frm["SMSText"], html.Subject, html.TemplateID, true, false, true, CONT.UR, "", UserId, FN.GetSystemIP());
                            else if (frm["isEmailvalue"] == "Email")
                                _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, html.HtmlTemplateID, html.HtmlName, html.RichText1FullHtml, html.Subject, html.TemplateID, true, false, true, CONT.UR, "", UserId, FN.GetSystemIP());
                            else if (frm["isEmailvalue"] == "Email")
                                _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, html.HtmlTemplateID, html.HtmlName, html.RichText1FullHtml, html.Subject, html.TemplateID, true, true, true, CONT.UR, "", UserId, FN.GetSystemIP());
                        }
                    }
                    trans.Commit();
                    octx.Connection.Close();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    trans.Rollback();
                    octx.Connection.Close();
                    if (ex.InnerException != null)
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View(html);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public DataSet GetDataSet(string SPName, Nullable<int> pCityID, Nullable<int> pStateID, string pCityName, Nullable<short> pIsActive, string pStatus, Nullable<bool> pIsKeywordSearch, string pKeywordvalue)
        {
            DataSet dsAc = new DataSet();
            try
            {
                SqlCommand cmd = ClsAppDatabase.GetSPName(SPName);
                ClsAppDatabase.AddInParameter(cmd, "@pCityID", SqlDbType.VarChar, pCityID);
                ClsAppDatabase.AddInParameter(cmd, "@pStateID", SqlDbType.VarChar, pStateID);
                ClsAppDatabase.AddInParameter(cmd, "@pCityName", SqlDbType.VarChar, pCityName);
                ClsAppDatabase.AddInParameter(cmd, "@pIsActive", SqlDbType.VarChar, pIsActive);
                ClsAppDatabase.AddInParameter(cmd, "@pStatus", SqlDbType.VarChar, pStatus == null ? "" : pStatus);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dsAc);
            }
            catch (Exception ex)
            { dsAc = null; }
            return dsAc;
        }

        public DataSet GetDataSetUser(string SPName, Nullable<int> pInvoiceID)
        {
            DataSet dsAc = new DataSet();
            try
            {
                SqlCommand cmd = ClsAppDatabase.GetSPName(SPName);
                ClsAppDatabase.AddInParameter(cmd, "@pInvoiceID", SqlDbType.Int, pInvoiceID);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dsAc);
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                dsAc = null;
            }
            return dsAc;
        }

        public ActionResult Edit(int? id, HTMLTemplate_ListAll_Result html)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    DataSet ds = GetDataSetUser("InvoiceHTMLDetail", 1);
                    DataTable DT = ds.Tables[0];
                    if (DT != null && DT.Rows.Count > 0)
                    {
                        string[] ColumnName = DT.Columns.Cast<DataColumn>()
                            .Select(x => x.ColumnName)
                            .ToArray();
                        SelectList list = new SelectList(ColumnName);
                        ViewBag.ColumnsName = list;
                    }
                    var data = _clsTemp.HTMLTemplate_ListAll(id, "", -1, "", false, "", 0).FirstOrDefault();
                    html.RichText1FullHtml = data.HtmlText;
                    if (data == null)
                    {
                        return HttpNotFound();
                    }
                    return View(data);
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    throw;
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HTMLTemplate_ListAll_Result html)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                
                ObjectContext octx = ((IObjectContextAdapter)db).ObjectContext;
                octx.Connection.Open();
                IDbTransaction trans = octx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                try
                {
                    var data = _clsTemp.HTMLTemplate_Update(html.HtmlTemplateID, html.HtmlName, html.HtmlText, html.Subject, html.TemplateID, html.IsSMS, html.IsMemo, UserId, FN.GetSystemIP());
                    trans.Commit();
                    octx.Connection.Close();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    trans.Rollback();
                    octx.Connection.Close();
                    if (ex.InnerException != null)
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View(html);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        public ActionResult Delete(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    return View();
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    throw;
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult Details(int? id, int? idh, HTMLTemplate_ListAll_Result html)
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
                            int MuserID = 0;
                            string staus = string.Empty;
                            int UserHistoryID = 0;
                            int.TryParse(id.ToString(), out UserHistoryID);

                            var data = _clsTemp.HTMLTemplateProcessHistory_ListAllBind_Result(idh, id, 0, "", 0).FirstOrDefault();
                            html.HtmlName = data.HtmlName;
                            html.IsSMS = data.IsSMS;
                            html.IsMemo = data.IsMemo;
                            html.HtmlText = data.HtmlText;
                            html.HtmlTemplateID = data.HtmlTemplateID;
                            html.HtmlTemplateProcessHistoryID = data.HtmlTemplateProcessHistoryID;
                            html.Status = data.Status;
                            html.Name = data.Name;
                            html.ProcessRemark = data.ProcessRemark;
                            html.Subject = data.Subject;
                            return View(html);
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
                return View(html);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult DetailsApprover(int? id, int? idh, HTMLTemplate_ListAll_Result html)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    var data = _clsTemp.HTMLTemplateProcessHistory_ListAllBind_Result(idh, id, 0, "", 0).FirstOrDefault();
                    html.HtmlName = data.HtmlName;
                    html.IsSMS = data.IsSMS;
                    html.IsMemo = data.IsMemo;
                    html.HtmlText = data.HtmlText;
                    html.HtmlTemplateID = data.HtmlTemplateID;
                    html.HtmlTemplateProcessHistoryID = data.HtmlTemplateProcessHistoryID;
                    html.Status = data.Status;
                    html.Name = data.Name;
                    html.Subject = data.Subject;
                    html.ProcessRemark = data.ProcessRemark;
                    return View(html);
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
                return View(html);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult CheckerList()
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
                            ViewData["CheckerView"] = "Checker";
                            return View(_clsTemp.HTMLTemplateProcessHistory_ListAllBind_Result(0, 0, 0, CONT.HTMLchecker, 0).ToList());
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
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult ApproverList()
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
                            ViewData["CheckerView"] = "Approver";
                            return View(_clsTemp.HTMLTemplateProcessHistory_ListAllBind_Result(0, 0, 0, CONT.HTMLApprover, 0).ToList());
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
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DetailsAmend(HTMLTemplate_ListAll_Result m, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);

                
                _clsTemp.conn = ClsAppDatabase.GetCon();

                if (_clsTemp.conn.State == ConnectionState.Closed)
                {
                    _clsTemp.conn.Open();
                }
                else { _clsTemp.conn.Close(); _clsTemp.conn.Open(); }
                _clsTemp.tras = _clsTemp.conn.BeginTransaction();
                try
                {
                    string Staus = frm["Status"];
                    string ProcessRemark = m.ProcessRemark;
                    string ProcessStatus = string.Empty;
                    int HtmlTemplateProcessHistoryID = Convert.ToInt32(frm["HtmlTemplateProcessHistoryID"]);
                    int HtmlTemplateID = Convert.ToInt32(frm["HtmlTemplateID"]);
                    string userRight = string.Empty;

                    //CheckerAmend Code
                    if (Staus == CONT.NR) { ProcessStatus = CONT.NM; userRight = "CheckerAmend"; }
                    else if (Staus == CONT.UR) { ProcessStatus = CONT.UM; userRight = "CheckerAmend"; }
                    else if (Staus == CONT.DR) { ProcessStatus = CONT.DC; userRight = "CheckerAmend"; }
                    else if (Staus == CONT.AR) { ProcessStatus = CONT.AC; userRight = "CheckerAmend"; }
                    else if (Staus == CONT.IR) { ProcessStatus = CONT.IC; userRight = "CheckerAmend"; }
                    if (userRight == "CheckerAmend")
                    {
                        if (ProcessRemark == null)
                        { ProcessRemark = ""; }
                        _clsTemp.HTMLTemplateProcessHistory_ProcessStatus(HtmlTemplateProcessHistoryID, HtmlTemplateID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        _clsTemp.tras.Commit();
                        _clsTemp.conn.Close();
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
                        if (ProcessRemark == null)
                        { ProcessRemark = ""; }
                        _clsTemp.HTMLTemplateProcessHistory_ProcessStatus(HtmlTemplateProcessHistoryID, HtmlTemplateID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        _clsTemp.tras.Commit();
                        _clsTemp.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    _clsTemp.tras.Rollback();
                    _clsTemp.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException != null)
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View("Details", m);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detailsapprove(HTMLTemplate_ListAll_Result m, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);

                
                _clsTemp.conn = ClsAppDatabase.GetCon();

                if (_clsTemp.conn.State == ConnectionState.Closed)
                { _clsTemp.conn.Open(); }
                else { _clsTemp.conn.Close(); _clsTemp.conn.Open(); }
                _clsTemp.tras = _clsTemp.conn.BeginTransaction();
                try
                {
                    string Staus = frm["Status"];
                    string ProcessRemark = m.ProcessRemark;
                    string ProcessStatus = string.Empty;
                    int HtmlTemplateProcessHistoryID = Convert.ToInt32(frm["HtmlTemplateProcessHistoryID"]);
                    int HtmlTemplateID = Convert.ToInt32(frm["HtmlTemplateID"]);
                    string userRight = string.Empty;

                    if (Staus == CONT.NR) { ProcessStatus = CONT.NC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.UR) { ProcessStatus = CONT.UC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.DR) { ProcessStatus = CONT.DC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.AR) { ProcessStatus = CONT.AC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.IR) { ProcessStatus = CONT.IC; userRight = "CheckerApprove"; }

                    if (userRight == "CheckerApprove")
                    {
                        if (ProcessRemark == null)
                        { ProcessRemark = ""; }
                        _clsTemp.HTMLTemplateProcessHistory_ProcessStatus(HtmlTemplateProcessHistoryID, HtmlTemplateID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        _clsTemp.tras.Commit();
                        _clsTemp.conn.Close();
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
                        if (ProcessRemark == null)
                        { ProcessRemark = ""; }
                        _clsTemp.HTMLTemplateProcessHistory_ProcessStatus(HtmlTemplateProcessHistoryID, HtmlTemplateID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        _clsTemp.tras.Commit();
                        _clsTemp.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    _clsTemp.tras.Rollback();
                    _clsTemp.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException != null)                  
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View("DetailsApprover", m);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        public ActionResult ViewSample(int? id, HTMLTemplate_ListAll_Result html, string Status = "")
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (id != null)
                    {  
                        var data = _clsTemp.HTMLTemplate_ListAll(id, "", -1, "", false, "", 0).LastOrDefault();
                        FillTemplateCombo();
                    }
                    else
                    {
                        string MenuId = Convert.ToString(Session["MenuID"]);
                        int ParentMenuID = 0;
                        int.TryParse(MenuId, out ParentMenuID);
                        //var UserRight = FN.CheckUserRight("", "Add", ParentMenuID);
                        var UserRight = FN.CheckUserRight("", "Maker", ParentMenuID);
                        if (UserRight != null)
                        {
                            if (UserRight.IsMaker)
                            {
                                FillTemplateCombo();
                            }
                            else
                            { return RedirectToAction("NoaccessPage", "MasterPage"); }
                        }
                        else
                        { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    }
                    ViewBag.DynamicTextID = new SelectList(_clsTemp.HtmlDynamicText_ListAll(0, "", false, "").OrderBy(x => x.DynamicTextName).ToList(), "DynamicTextName", "DynamicTextName");
                    if (id != null)
                    {
                        string MenuId = Convert.ToString(Session["MenuID"]);
                        int ParentMenuID = 0;
                        int.TryParse(MenuId, out ParentMenuID);
                        var UserRight = FN.CheckUserRight("", "Update", ParentMenuID);
                        if (Status == CONT.UJ)
                        {
                            var objHTMLTemplate_ListAll_Result = _clsTemp.HTMLTemplate_ListAll(id, "", 1, "", false, "", 0).LastOrDefault();
                            if (objHTMLTemplate_ListAll_Result != null)
                            {
                                html.HtmlTemplateID = objHTMLTemplate_ListAll_Result.HtmlTemplateID;
                                html.HtmlName = objHTMLTemplate_ListAll_Result.HtmlName;
                                if (objHTMLTemplate_ListAll_Result.IsSMS == true)
                                {
                                    objHTMLTemplate_ListAll_Result.IsSMS = true;
                                    html.issmsvalue = "SMS";
                                    html.SMSText = objHTMLTemplate_ListAll_Result.HtmlText;
                                    ViewData["viewSample"] = html.SMSText;
                                }
                                else
                                {
                                    objHTMLTemplate_ListAll_Result.IsSMS = false;
                                    html.isEmailvalue = "Email";
                                    ViewData["check"] = objHTMLTemplate_ListAll_Result.HtmlText;
                                    ViewData["viewSample"] = ViewData["check"];
                                    html.SMSText = "";
                                }
                                html.IsSMS = objHTMLTemplate_ListAll_Result.IsSMS;
                                html.TemplateID = objHTMLTemplate_ListAll_Result.TemplateID;
                                html.Status = objHTMLTemplate_ListAll_Result.Status;
                            }
                            return View(objHTMLTemplate_ListAll_Result);
                        }
                        else
                        {
                            var objHTMLTemplate_ListAll_Result = _clsTemp.HTMLTemplateProcessHistory_ListAllBind_Result(0, id, 0, "", 0).LastOrDefault();
                            if (objHTMLTemplate_ListAll_Result != null)
                            {
                                html.HtmlTemplateID = objHTMLTemplate_ListAll_Result.HtmlTemplateID;
                                html.HtmlName = objHTMLTemplate_ListAll_Result.HtmlName;
                                if (objHTMLTemplate_ListAll_Result.IsSMS == true)
                                {
                                    objHTMLTemplate_ListAll_Result.IsSMS = true;
                                    html.issmsvalue = "SMS";
                                    html.SMSText = objHTMLTemplate_ListAll_Result.HtmlText;
                                    ViewData["viewSample"] = html.SMSText;
                                }
                                else
                                {
                                    objHTMLTemplate_ListAll_Result.IsSMS = false;
                                    html.isEmailvalue = "Email";
                                    ViewData["check"] = objHTMLTemplate_ListAll_Result.HtmlText;
                                    ViewData["viewSample"] = ViewData["check"];
                                    html.SMSText = "";
                                }
                                html.IsSMS = objHTMLTemplate_ListAll_Result.IsSMS;
                                html.TemplateID = objHTMLTemplate_ListAll_Result.TemplateID;
                                html.Status = objHTMLTemplate_ListAll_Result.Status;                              
                            }
                            return View(objHTMLTemplate_ListAll_Result);
                        }
                    }
                    return View(html);
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    FillTemplateCombo();
                    ViewBag.DynamicTextID = new SelectList(_clsTemp.HtmlDynamicText_ListAll(0, "", false, "").OrderBy(x => x.DynamicTextName).ToList(), "DynamicTextName", "DynamicTextName");
                    if (ex.InnerException != null)                    
                        ViewBag.ErrorMsg = ex.InnerException.Message;
                    else                    
                        ViewBag.ErrorMsg = ex.Message;
                    return View(html);
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ViewSample(HTMLTemplate_ListAll_Result html, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                FillTemplateCombo();
                int isHtmlTemplateID = Convert.ToInt32(frm["isHtmlTemplateID"]);
                string SMS = Request.Form["IsSMS"];
                string Status = frm["Status"];
                
                string Remarks = "";
                Remarks = frm["ProcessRemark"];
                
                ObjectContext octx = ((IObjectContextAdapter)db).ObjectContext;
                octx.Connection.Open();
                IDbTransaction trans = octx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                _clsTemp.conn = ClsAppDatabase.GetCon();

                if (_clsTemp.conn.State == ConnectionState.Closed)
                { _clsTemp.conn.Open(); }
                else { _clsTemp.conn.Close(); _clsTemp.conn.Open(); }
                _clsTemp.tras = _clsTemp.conn.BeginTransaction();
                try
                {
                    html.RichText1FullHtml = html.RichText1;
                    html.RichText1 = null;
                    if (isHtmlTemplateID == 0)
                    {
                        ObjectParameter HtmlTemplateID = new ObjectParameter("pHtmlTemplateID", typeof(int));
                        if (frm["issmsvalue"] == "SMS")
                        {
                            if (frm["SMSText"] == null)
                            {
                                frm["SMSText"] = "";
                            }
                            _clsTemp.HTMLTemplate_Add(HtmlTemplateID, frm["isHtmlName"], frm["SMSText"], frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), true, false, UserId, FN.GetSystemIP());
                        }
                        else if (frm["isEmailvalue"] == "Email")
                        {
                            if (html.HtmlContent == null)
                            {
                                html.HtmlContent = "";
                            }
                            _clsTemp.HTMLTemplate_Add(HtmlTemplateID, frm["isHtmlName"], html.HtmlContent, frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), false, false, UserId, FN.GetSystemIP());
                        }
                        else if (frm["isMemovalue"] == "Memo")
                        {
                            if (html.HtmlContent == null)
                            {
                                html.HtmlContent = "";
                            }
                            _clsTemp.HTMLTemplate_Add(HtmlTemplateID, frm["isHtmlName"], html.HtmlContent, frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), false, true, UserId, FN.GetSystemIP());
                        }
                    }
                    else
                    {
                        if (Status == CONT.NM)
                        {
                            if (frm["issmsvalue"] == "SMS")
                            {
                                _clsTemp.HTMLTemplate_Update(html.HtmlTemplateID, frm["isHtmlName"], frm["SMSText"], frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), true, false, UserId, FN.GetSystemIP());
                            }
                            else if (frm["isEmailvalue"] == "Email")
                            {
                                _clsTemp.HTMLTemplate_Update(html.HtmlTemplateID, frm["isHtmlName"], html.HtmlContent, frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), false, false, UserId, FN.GetSystemIP());
                            }
                            else if (frm["isMemovalue"] == "Memo")
                            {
                                _clsTemp.HTMLTemplate_Update(html.HtmlTemplateID, frm["isHtmlName"], html.HtmlContent, frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), false, true, UserId, FN.GetSystemIP());
                            }
                        }
                        else if (Status == CONT.NA || Status == CONT.UA || Status == CONT.UM || Status == CONT.UJ)
                        {
                            ObjectParameter pHtmlTemplateProcessHistoryID = new ObjectParameter("pHtmlTemplateProcessHistoryID", typeof(int));
                            if (frm["issmsvalue"] == "SMS")
                            {
                                _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, Convert.ToInt32(frm["isHtmlTemplateID"]), frm["isHtmlName"] == null ? "" : frm["isHtmlName"], frm["SMSText"] == null ? "" : frm["SMSText"], frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), true, false, true, CONT.UR, Remarks == null ? "" : Remarks, UserId, FN.GetSystemIP());
                            }
                            else if (frm["isEmailvalue"] == "Email")
                            {
                                _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, Convert.ToInt32(frm["isHtmlTemplateID"]), frm["isHtmlName"] == null ? "" : frm["isHtmlName"], html.HtmlContent == null ? "" : html.HtmlContent, frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), false, false, true, CONT.UR, Remarks == null ? "" : Remarks, UserId, FN.GetSystemIP());
                            }
                            else if (frm["isMemovalue"] == "Memo")
                            {
                                _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, Convert.ToInt32(frm["isHtmlTemplateID"]), frm["isHtmlName"] == null ? "" : frm["isHtmlName"], html.HtmlContent == null ? "" : html.HtmlContent, frm["Subject"], Convert.ToInt32(frm["isTemplateID"]), false, true, true, CONT.UR, Remarks == null ? "" : Remarks, UserId, FN.GetSystemIP());
                            }
                        }
                    }
                    trans.Commit();
                    octx.Connection.Close();

                    _clsTemp.tras.Commit();
                    _clsTemp.conn.Close();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    octx.Connection.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    FillTemplateCombo();
                    ViewBag.DynamicTextID = new SelectList(_clsTemp.HtmlDynamicText_ListAll(0, "", false, "").OrderBy(x => x.DynamicTextName).ToList(), "DynamicTextName", "DynamicTextName");
                    if (ex.InnerException != null)                    
                        ViewBag.ErrorMsg = ex.InnerException.Message;                    
                    else                    
                        ViewBag.ErrorMsg = ex.Message;
                }
                return View(html);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public string SMSSendingProccess(int? id, string msg)
        {
            try
            {
                string viewsample = "";               
                return viewsample;
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                throw;
            }
        }

        private string GetSMSText(DataRow dr, string msg)
        {
            string strSMS = msg;
            try
            {
                while (strSMS.Contains("{"))
                {
                    string ColumnName = strSMS.Substring(strSMS.IndexOf("{") + 1, strSMS.IndexOf("}") - strSMS.IndexOf("{") - 1);
                    strSMS = strSMS.Replace("{" + ColumnName + "}", GetPlainText(dr[ColumnName], dr.Table.Columns[ColumnName].DataType.ToString()).Trim());
                }
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
            }
            return strSMS;
        }
        private string GetPlainText(object obj, string Type)
        {
            try
            {
                if (Type.Equals("System.DateTime"))
                {
                    return Convert.ToDateTime(obj.ToString()).Date.ToString("dd-MMM-yyyy");
                }

                if (Type.Equals("System.Decimal"))
                {
                    return Convert.ToDecimal(obj.ToString()).ToString("#,##0.00");                    
                }
                return obj.ToString();
            }
            catch (Exception ex)
            {
                string errorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(errorMessage);
                throw;
            }
        }
        public PartialViewResult AIDPopup(string ActionName, string TemplateName)
        {
            ViewData["ActionName"] = "";
            ViewData["ActionName"] = ActionName;
            ViewData["TemplateName"] = TemplateName;
            return PartialView();
        }
        public string Act_InA_Del(string ActionName, int HtmlTempateID, string ProcessRemarks)
        {
            string resp = "";
            string Status = "";
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                
                try
                {
                    int UpdatedBy = 0;
                    int.TryParse(LoginStatus[1].ToString(), out UpdatedBy);
                    ClsUserMaster UM = new ClsUserMaster();

                    var mHTMLTemplate = _clsTemp.HTMLTemplate_ListAll(HtmlTempateID, "", -1, "", false, "", 0).FirstOrDefault();
                    ObjectParameter pHtmlTemplateProcessHistoryID = new ObjectParameter("pHtmlTemplateProcessHistoryID", typeof(int));
                    if (mHTMLTemplate != null)
                        Status = mHTMLTemplate.Status;

                    int mHtmlTempateID = 0;

                    _clsTemp.conn = ClsAppDatabase.GetCon();
                    if (_clsTemp.conn.State == ConnectionState.Closed)
                    { _clsTemp.conn.Open(); }
                    else { _clsTemp.conn.Close(); _clsTemp.conn.Open(); }
                    _clsTemp.tras = _clsTemp.conn.BeginTransaction();
                    if (ActionName.Equals("Delete"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA|| Status == CONT.AA || Status == CONT.IA)
                        {
                            mHtmlTempateID = _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, mHTMLTemplate.HtmlTemplateID, mHTMLTemplate.HtmlName, mHTMLTemplate.HtmlText, mHTMLTemplate.Subject, mHTMLTemplate.TemplateID, true, false, mHTMLTemplate.IsActive, CONT.DR, "", UpdatedBy, FN.GetSystemIP());
                        }
                    }
                    else if (ActionName.Equals("Active"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.IA)
                        {
                            mHtmlTempateID = _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, mHTMLTemplate.HtmlTemplateID, mHTMLTemplate.HtmlName, mHTMLTemplate.HtmlText, mHTMLTemplate.Subject, mHTMLTemplate.TemplateID, true, false, true, CONT.AR, "", UpdatedBy, FN.GetSystemIP());
                        }
                    }
                    else if (ActionName.Equals("InActive"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA)
                        {
                            mHtmlTempateID = _clsTemp.HTMLTemplateProcessHistory_Add(pHtmlTemplateProcessHistoryID, mHTMLTemplate.HtmlTemplateID, mHTMLTemplate.HtmlName, mHTMLTemplate.HtmlText, mHTMLTemplate.Subject, mHTMLTemplate.TemplateID, true, false, false, CONT.IR, "", UpdatedBy, FN.GetSystemIP());
                        }
                    }
                    resp = "1$SuccessFully";
                    _clsTemp.tras.Commit();
                    _clsTemp.conn.Close();
                }
                catch (Exception ex)
                {
                    _clsTemp.tras.Rollback();
                    _clsTemp.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    resp = "0$" + ex.Message;
                }
            }
            else
            {
                resp = "2$Please Login.";
            }
            return resp;
        }
        public ActionResult HTMLTemplateViewerIndex()
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
                        { return View(_clsTemp.HTMLTemplateProcessHistory_ListAllBind_Result(0, 0, 0, "", 0).ToList()); }
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
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        public ActionResult HTMLTemplateViewer(int? id, int? idh, HTMLTemplate_ListAll_Result html)
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
                            int MuserID = 0;
                            string staus = string.Empty;
                            int UserHistoryID = 0;
                            int.TryParse(id.ToString(), out UserHistoryID);
                            
                            var data = _clsTemp.HTMLTemplateProcessHistory_ListAllBind_Result(idh, id, 0, "", 0).FirstOrDefault();
                            html.HtmlName = data.HtmlName;
                            html.IsSMS = data.IsSMS;
                            html.IsMemo = data.IsMemo;
                            html.HtmlText = data.HtmlText;
                            html.HtmlTemplateID = data.HtmlTemplateID;
                            html.HtmlTemplateProcessHistoryID = data.HtmlTemplateProcessHistoryID;
                            html.Status = data.Status;
                            html.Name = data.Name;
                            html.Subject = data.Subject;
                            return View(html);
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
                return View(html);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }

        public PartialViewResult CommentResult(int HtmlTemplateID)
        {
            ViewBag.lstCustomerMaster = _clsTemp.HTMLTemplate_ListAllRemarks(HtmlTemplateID).ToList();
            return PartialView();
        }
        public void FillTemplateCombo()
        {
            var DDLTemplateIData = _clsTemp.Template_ListAll_Result(0, "").OrderBy(x => x.Name).ToList();
            if (DDLTemplateIData != null && DDLTemplateIData.Count > 0)
                ViewBag.DDLTemplateID = DDLTemplateIData;
            else
                ViewBag.DDLTemplateID = new List<SelectListItem> { };
        }
    }
}