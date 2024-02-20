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
using System.Net;
using System.Web.Configuration;
using System.Collections;
using System.Globalization;
using System.IO.Compression;

namespace TMP.Controllers
{
    public class InvoiceCommonController : Controller
    {
        #region Local Variable
        Function FN = new Function();
        InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
        ClsInvoiceTransaction _ClsInvoiceTransaction = new ClsInvoiceTransaction();
        ClsCustomerMaster _ClsCustomerMaster = new ClsCustomerMaster();
        CustomerMasterModel _ObjModelCustomer = new CustomerMasterModel();
        ClsHTMLTemplate _ClsHTMLTemplate = new ClsHTMLTemplate();
        #endregion

        #region Bulk Invoice Upload
        public ActionResult BulkInvoiceUpload(string ProgramType = "", int Delete_InvTempID = 0, int InvTempID = 0)
        {
            try
            {

                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    int intCustType = Convert.ToInt16(Session["onlineCustomerTypeID"]);

                    if (intCustType == CONT.BuyerCustomerTypeID || intCustType == CONT.BothObligorAndBuyerTypeID)
                    {
                        ProgramType = CONT.RType;
                    }
                    else
                    {
                        ProgramType = CONT.FType;
                    }
                    _ObjModel.ProgramType = ProgramType;

                    if (Delete_InvTempID > 0)
                    {
                        _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                        if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                        else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                        _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                        _ObjModel.InvTempID = Delete_InvTempID;
                        _ClsInvoiceTransaction.InvTempMaster_Delete(_ObjModel);
                        _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        return RedirectToAction("BulkInvoiceUpload", "InvoiceCommon", new { ProgramType = ProgramType });
                    }
                    BindBulkInvoiceGrid();
                    _ObjModel.InvTempID = InvTempID;
                    ViewBag.CustTypeID = intCustType;
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                int intCustType = Convert.ToInt16(Session["onlineCustomerTypeID"]);
                ViewBag.CustTypeID = intCustType;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult BulkInvoiceUpload(InvoiceTransactionModel _objModel, HttpPostedFileBase UpdInvoices, FormCollection frm)
        {
            int intCustType = Convert.ToInt16(Session["onlineCustomerTypeID"]);
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    int intInvoiceTempID = 0;
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    string newfilenm = Guid.NewGuid().ToString().Substring(0, 12);
                    if (UpdInvoices != null)
                    {
                        if (UpdInvoices.FileName != null)
                        {
                            DataTable dt = ConvertCSVtoDataTable(UpdInvoices);

                            if (intCustType == CONT.SupplierCustomerTypeID)
                                _objModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                            else if (intCustType == CONT.ObligorCustomerTypeID)
                                _objModel.ObligorID = Convert.ToInt32(Session["CustomerID"]);
                            else if (intCustType == CONT.BuyerCustomerTypeID)
                                _objModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);

                            if (intCustType == CONT.BothObligorAndBuyerTypeID)
                            {
                                if (Convert.ToString(Request.Form["strProgramType"]) != null)
                                {
                                    if (Request.Form["strProgramType"] == "F")
                                    {
                                        _objModel.ObligorID = Convert.ToInt32(Session["CustomerID"]);
                                    }
                                    else if (Request.Form["strProgramType"] == "R")
                                    {
                                        _objModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);
                                    }
                                }
                            }
                            _objModel.Attach = UpdInvoices.FileName.ToString();
                            _objModel.InvTempID = _ClsInvoiceTransaction.InvTempMaster_Add(_objModel);
                            intInvoiceTempID = _objModel.InvTempID;

                            string strFilename = _objModel.InvTempID + "_" + UpdInvoices.FileName.ToString();
                            UpdInvoices.SaveAs(Server.MapPath("\\Files\\UploadedTempInvoiceFiles\\" + strFilename));

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                String strCustTypeCIF = dt.Columns[1].ColumnName;
                                String strCustTypeName = dt.Columns[2].ColumnName;

                                _objModel.InvoiceNo = dt.Rows[i]["Payable No."].ToString();
                                _objModel.CurrencyCode = dt.Rows[i]["Invoice Currency"].ToString();

                                DateTime dtInvoiceDate = DateTime.ParseExact(dt.Rows[i]["Payable Date (dd/mm/yyyy)"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                string strInvoiceDate = dtInvoiceDate.ToString("dd/MM/yyyy");
                                _objModel.InvoiceDate = Convert.ToDateTime(FN.ChangeDate(strInvoiceDate));


                                DateTime dtDueDate = DateTime.ParseExact(dt.Rows[i]["Invoice Due Date (dd/mm/yyyy)"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                string strDueDate = dtDueDate.ToString("dd/MM/yyyy");
                                _objModel.DueDate = Convert.ToDateTime(FN.ChangeDate(strDueDate));


                                //_objModel.InvoiceDate = Convert.ToDateTime(FN.ChangeDate(dt.Rows[i]["Invoice Date (dd/mm/yyyy)"].ToString()));
                                //_objModel.DueDate = Convert.ToDateTime(FN.ChangeDate(dt.Rows[i]["Invoice Due Date (dd/mm/yyyy)"].ToString()));

                                _objModel.Amt = Convert.ToDecimal(dt.Rows[i]["Payable Amount"].ToString());
                                _objModel.Attach = dt.Rows[i]["Attachment"].ToString();


                                String ColmTenor = "Tenor";
                                if (dt.Columns.Contains(ColmTenor) == true)
                                {
                                    if (dt.Rows[i][ColmTenor].ToString() != "")
                                        _objModel.Tenor = Convert.ToInt16(dt.Rows[i][ColmTenor].ToString());
                                }

                                if (strCustTypeName == "Obligor Name") _objModel.ObligorName = dt.Rows[i][strCustTypeName].ToString();
                                else if (strCustTypeName == "Supplier Name") _objModel.SupplierName = dt.Rows[i][strCustTypeName].ToString();
                                else if (strCustTypeName == "Buyer Name") _objModel.BuyerName = dt.Rows[i][strCustTypeName].ToString();
                                else if (strCustTypeName == "Exporter Name") _objModel.ExporterName = dt.Rows[i][strCustTypeName].ToString();

                                if (strCustTypeCIF == "Obligor CIF") _objModel.ObligorCode = dt.Rows[i][strCustTypeCIF].ToString();
                                else if (strCustTypeCIF == "Supplier CIF") _objModel.SupplierCode = dt.Rows[i][strCustTypeCIF].ToString();
                                else if (strCustTypeCIF == "Buyer CIF") _objModel.BuyerCode = dt.Rows[i][strCustTypeCIF].ToString();
                                else if (strCustTypeCIF == "Exporter CIF") _objModel.ExporterCode = dt.Rows[i][strCustTypeCIF].ToString();

                                _objModel.InvTempDetID = _ClsInvoiceTransaction.InvTempDetail_Add(_objModel);
                            }

                            _ClsInvoiceTransaction.InvTempDetail_InvProcess(_objModel);

                            _ObjModel.Attachs = ""; _ObjModel.InvTempID = intInvoiceTempID;
                            _ClsInvoiceTransaction.InvTempDetail_FileVerifyint(_ObjModel);
                            _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        }
                    }

                    _ObjModel.InvTempID = intInvoiceTempID;
                    _ObjModel.ProgramType = _objModel.ProgramType;
                    ViewBag.CustTypeID = intCustType;
                    BindBulkInvoiceGrid();
                    return RedirectToAction("BulkInvoiceUpload", "InvoiceCommon", new { ProgramType = _objModel.ProgramType, InvTempID = intInvoiceTempID });
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.Message.Contains("DateTime"))
                { ViewBag.ErrorMesssage = "Invalid Passing Date."; }
                else
                { if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message; }
                ViewBag.CustTypeID = intCustType;
                _ObjModel.ProgramType = _objModel.ProgramType;
                BindBulkInvoiceGrid();
                return View(_ObjModel);
            }
        }
        private DataTable ConvertCSVtoDataTable(HttpPostedFileBase file)
        {
            DataTable dt = new DataTable();
            try
            {
                StreamReader sr = new StreamReader(file.InputStream);
                string[] headers = sr.ReadLine().Split(',');

                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return dt;
            }
        }
        public ActionResult SaveDropzoneJsUploadedFiles(InvoiceTransactionModel _Model)
        {
            string _errMsg = string.Empty;
            try
            {
                string strFileName = "";
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase _file = Request.Files[i];
                    strFileName = strFileName + _file.FileName + ",";
                }
                strFileName = strFileName.Remove(strFileName.Length - 1);

                try
                {
                    _ObjModel.pIsVerified = -1; _ObjModel.pIsDeleted = 0;
                    _ObjModel.InvTempID = _Model.InvTempID;
                    var GetData = _ClsInvoiceTransaction.InvTempDetail_ListAll(_ObjModel);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    _ObjModel.Attachs = strFileName; _ObjModel.InvTempID = GetData[0].InvTempID;
                    _ClsInvoiceTransaction.InvTempDetail_FileVerifyint(_ObjModel);
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    for (int i = 0; i < GetData.Count; i++)
                    {
                        string[] SpltServerFileName = GetData[i].Attach.Split('_');
                        string ServerFileName = SpltServerFileName[1];

                        for (int ii = 0; ii < Request.Files.Count; ii++)
                        {
                            HttpPostedFileBase _file = Request.Files[ii];
                            strFileName = _file.FileName;

                            if (ServerFileName == strFileName)
                            {
                                _file.SaveAs(Path.Combine(Server.MapPath("~/Files/UploadedTempInvoiceFiles"), GetData[i].Attach));
                                _file.SaveAs(Path.Combine(Server.MapPath("~/Files/Invoice"), GetData[i].Attach));
                            }
                        }
                    }

                    return RedirectToAction("BulkInvoiceUpload", "InvoiceCommon", new { ProgramType = _Model.ProgramType, InvTempID = _Model.InvTempID });
                }
                catch (Exception exx)
                {
                    _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    List<string> errors = new List<string>();
                    errors.Add(exx.Message.ToString());
                    return Json(errors);
                }
                return Json(new { ErrorCode = 1, ErrorMessage = _errMsg });
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                List<string> errors = new List<string>();
                errors.Add(ex.Message.ToString());
                return Json(errors);
            }
        }
        public ActionResult BulkInvoiceView(int InvTempDetID = 0, int InvTempEditID = 0, string ProgramType = "", int InvTempID = 0)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {

                    ViewBag.isEdit = false;
                    ViewBag.CustomerTypeID = Convert.ToInt16(Session["onlineCustomerTypeID"]);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();
                    if (InvTempDetID > 0)
                    {
                        _ObjModel.InvTempDetID = InvTempDetID;
                        _ClsInvoiceTransaction.InvTempDetail_Delete(_ObjModel);
                        _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        return RedirectToAction("BulkInvoiceView", "InvoiceCommon", new { ProgramType = ProgramType, InvTempID = InvTempID });
                    }
                    if (InvTempEditID > 0)
                    {
                        ViewBag.isEdit = true;
                        _ObjModel.InvTempDetID = InvTempEditID;
                        _ObjModel = _ClsInvoiceTransaction.InvTempDetail_ListAll(_ObjModel).FirstOrDefault();
                    }
                    _ObjModel.InvTempID = InvTempID;
                    ViewBag.InvTempID = InvTempID;
                    ViewBag.ProgramType = ProgramType;
                    var GetParentData = _ClsInvoiceTransaction.InvTempMaster_ListAll(_ObjModel);
                    if (GetParentData != null && GetParentData.Count > 0)
                    {
                        ViewBag.ParentData = GetParentData;
                        _ObjModel.pIsVerified = -1; _ObjModel.pIsDeleted = 0;
                        ViewBag.ChildData = _ClsInvoiceTransaction.InvTempDetail_ListAll(_ObjModel);
                        ViewBag.GetViewData = _ClsInvoiceTransaction.InvTempDetail_ListAll(_ObjModel).Where(C => C.Comment != "").ToList();
                        FillCustomerCombo(GetParentData[0].ProgramType);
                        if (GetParentData[0].ObligorID > 0)
                            ViewBag.Name = "Obligor";
                        else if (GetParentData[0].SupplierID > 0)
                            ViewBag.Name = "Supplier";
                        else if (GetParentData[0].BuyerID > 0)
                            ViewBag.Name = "Buyer";
                        else if (GetParentData[0].ExporterID > 0)
                            ViewBag.Name = "Exporter";
                    }
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult BulkInvoiceView(InvoiceTransactionModel _objModel)
        {
            try
            {
                string ProgramType = "";
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    int intCustType = Convert.ToInt16(Session["onlineCustomerTypeID"]);
                    ViewBag.CustomerTypeID = Convert.ToInt16(Session["onlineCustomerTypeID"]);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    var GetParentData = _ClsInvoiceTransaction.InvTempMaster_ListAll(_objModel);

                    if (GetParentData != null)
                    {
                        ProgramType = GetParentData[0].ProgramType;
                        for (int i = 0; i < GetParentData.Count; i++)
                        {
                            _objModel.DueDate = GetParentData[i].DueDate;
                            _objModel.CurrencyID = GetParentData[i].CurrencyID;
                            _objModel.ProgramType = GetParentData[i].ProgramType;

                            if (intCustType == CONT.SupplierCustomerTypeID)
                            {
                                _objModel.SupplierID = GetParentData[i].MstSupplierID; _objModel.ObligorID = GetParentData[i].ObligorID;
                                _objModel.BuyerID = 0; _objModel.ExporterID = 0;
                            }
                            else if (intCustType == CONT.ObligorCustomerTypeID)
                            {
                                _objModel.SupplierID = GetParentData[i].SupplierID; _objModel.ObligorID = GetParentData[i].MstObligorID;
                                _objModel.BuyerID = 0; _objModel.ExporterID = 0; _objModel.IsObligorGen = true;
                            }
                            else if (intCustType == CONT.BuyerCustomerTypeID)
                            {
                                _objModel.ObligorID = 0; _objModel.BuyerID = GetParentData[i].MstBuyerID;
                                _objModel.ExporterID = 0;
                                _objModel.SupplierID = GetParentData[i].SupplierID;
                            }

                            else if (intCustType == CONT.BothObligorAndBuyerTypeID && ProgramType == CONT.FType)
                            {
                                _objModel.SupplierID = GetParentData[i].SupplierID; _objModel.ObligorID = GetParentData[i].MstObligorID;
                                _objModel.BuyerID = 0; _objModel.ExporterID = 0; _objModel.IsObligorGen = true;
                            }
                            else if (intCustType == CONT.BothObligorAndBuyerTypeID && ProgramType == CONT.RType)
                            {
                                _objModel.SupplierID = 0; _objModel.ObligorID = 0; _objModel.BuyerID = GetParentData[i].MstBuyerID;
                                _objModel.ExporterID = GetParentData[i].ExporterID;
                            }
                            int intInvoiceID = _ClsInvoiceTransaction.InvoiceMaster_Add(_objModel);
                            _objModel.InvoiceID = intInvoiceID;

                            _ObjModel.pIsVerified = -1; _ObjModel.pIsDeleted = 0;
                            _ObjModel.InvTempID = GetParentData[i].InvTempID; _ObjModel.SeqNo = GetParentData[i].SeqNo;
                            var ChildData = _ClsInvoiceTransaction.InvTempDetail_ListAll(_ObjModel);
                            for (int ii = 0; ii < ChildData.Count; ii++)
                            {
                                _objModel.InvoiceID = intInvoiceID;
                                _objModel.InvoiceNo = ChildData[ii].InvoiceNo;
                                _objModel.InvoiceDate = ChildData[ii].InvoiceDate;
                                _objModel.Amt = ChildData[ii].Amt;
                                _objModel.Attach = ChildData[ii].Attach;
                                _ClsInvoiceTransaction.InvoiceDetail_Add(_objModel);
                            }

                            _ClsInvoiceTransaction.InvTempMaster_Generate(_objModel);
                        }
                    }
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    if (intCustType == CONT.SupplierCustomerTypeID || intCustType == CONT.BuyerCustomerTypeID)
                        //return RedirectToAction("Index", "Factoring", new { ProgramType = ProgramType });
                        return RedirectToAction("CMNDashboard", "Home");
                    else
                        //return RedirectToAction("ObligorIndex", "Factoring");
                        return RedirectToAction("CMNDashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult BulkInvoiceViewEdit(InvoiceTransactionModel _objModel, FormCollection frm, ControllerContext controllerContext)
        {
            try
            {
                string ProgramType = _objModel.ProgramType;
                int InvTempID = _objModel.InvTempID;

                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();
                    _ObjModel.InvTempID = InvTempID;
                    _ObjModel.pIsVerified = -1; _ObjModel.pIsDeleted = 0;

                    var Data = _ClsInvoiceTransaction.InvTempDetail_ListAll(_ObjModel);

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int InvTempDetID = Data[i].InvTempDetID;
                        _objModel.InvTempDetID = InvTempDetID;
                        HttpFileCollectionBase files = Request.Files;
                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("chk_" + InvTempDetID + ""))
                            {
                                if (_objModel.ObligorName == null && Convert.ToString(frm["GetObligorName_" + InvTempDetID + ""]) != "")
                                    _objModel.ObligorName = Convert.ToString(frm["GetObligorName_" + InvTempDetID + ""]);
                                else if (_objModel.SupplierName == null && Convert.ToString(frm["GetSupplierName_" + InvTempDetID + ""]) != "")
                                    _objModel.SupplierName = Convert.ToString(frm["GetSupplierName_" + InvTempDetID + ""]);
                                else if (_objModel.ExporterName == null && Convert.ToString(frm["GetExporterName_" + InvTempDetID + ""]) != "")
                                    _objModel.ExporterName = Convert.ToString(frm["GetExporterName_" + InvTempDetID + ""]);


                                _objModel.DueDate = Convert.ToDateTime(frm["GetDueDate_" + InvTempDetID + ""]);
                                string old_fileName = Convert.ToString(frm["AttachFile_" + InvTempDetID + ""]);
                                _objModel.Attach = old_fileName;
                                _objModel.Tenor = Convert.ToInt16(frm["GetTenor_" + InvTempDetID + ""]);

                                for (int z = 0; z < files.Count; z++)
                                {
                                    string GetAllkeysUploadFile = files.AllKeys[z];
                                    if (GetAllkeysUploadFile.Equals("Attach_" + InvTempDetID + ""))
                                    {
                                        HttpPostedFileBase file = files[z];
                                        if (file != null && file.ContentLength > 0)
                                        {
                                            string fileName = Convert.ToString(frm["AttachFile_" + InvTempDetID + ""]);
                                            file.SaveAs(Path.Combine(Server.MapPath("~/Files/UploadedTempInvoiceFiles"), fileName));
                                            file.SaveAs(Path.Combine(Server.MapPath("~/Files/Invoice"), fileName));
                                            _objModel.Attach = fileName;
                                            var NewFile = fileName.Split('_'); _ObjModel.InvTempID = InvTempID;
                                            _ObjModel.Attachs = NewFile[1].ToString();
                                            _ClsInvoiceTransaction.InvTempDetail_FileVerifyint(_ObjModel);
                                            _objModel.Attach = fileName;
                                        }
                                    }
                                }

                                if (frm["DueDate_" + InvTempDetID + ""] != null)
                                    _objModel.DueDate = Convert.ToDateTime(frm["DueDate_" + InvTempDetID + ""]);

                                if (frm["Tenor_" + InvTempDetID + ""] != null)
                                    _objModel.Tenor = Convert.ToInt16(frm["Tenor_" + InvTempDetID + ""]);

                                _ClsInvoiceTransaction.InvTempDetail_Update(_objModel);
                            }
                        }
                    }
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    return RedirectToAction("BulkInvoiceView", "InvoiceCommon", new { ProgramType = ProgramType, InvTempID = InvTempID });
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        public void BindBulkInvoiceGrid()
        {
            try
            {
                int intCustType = Convert.ToInt32(Session["onlineCustomerTypeID"]);
                int intCustID = Convert.ToInt32(Session["CustomerID"]);
                if (intCustType == CONT.SupplierCustomerTypeID)
                    _ObjModel.SupplierID = intCustID;
                else if (intCustType == CONT.ObligorCustomerTypeID)
                    _ObjModel.ObligorID = intCustID;
                else if (intCustType == CONT.BuyerCustomerTypeID)
                    _ObjModel.BuyerID = intCustID;
                var GetTmpMasterData = _ClsInvoiceTransaction.InvTempMaster_ListAllBind(_ObjModel).ToList();
                if (GetTmpMasterData != null)
                {
                    ViewBag.TmpMasterData = GetTmpMasterData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FillCustomerCombo(string ProgramType = "")
        {
            try
            {
                ClsInvoiceTransaction _clsInvoi = new ClsInvoiceTransaction();
                InvoiceTransactionModel _objSubModel = new InvoiceTransactionModel();
                if (ProgramType == CONT.FType)
                {
                    if (Convert.ToInt16(Session["onlineCustomerTypeID"]) == CONT.ObligorCustomerTypeID)
                        _objSubModel.ObligorID = Convert.ToInt32(Session["CustomerID"]);
                    else
                        _objSubModel.SupplierID = Convert.ToInt32(Session["CustomerID"]);
                }
                else if (ProgramType == CONT.RType)
                {
                    _objSubModel.BuyerID = Convert.ToInt32(Session["CustomerID"]);
                }
                else if (ProgramType == CONT.BothProgramType)
                {
                    _objSubModel.ObligorBuyerID = Convert.ToInt32(Session["CustomerID"]);
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

        #endregion

        #region Common Partial View
        public PartialViewResult InvoiceDetails(int InvoiceID = 0, bool IsAdd = false)
        {
            try
            {
                ViewBag.IsAdd = IsAdd;
                if (InvoiceID > 0)
                {
                    _ObjModel.InvoiceID = InvoiceID;
                    var Data = _ClsInvoiceTransaction.InvoiceDetail_ListAll(_ObjModel);
                    return PartialView(Data);
                }
                else
                {
                    return PartialView(_ObjModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult TransactionSummary(int InvoiceID = 0, string ProgramType = "")
        {
            try
            {
                if (InvoiceID > 0)
                {
                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType;
                    _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { }
                    else { _ObjModel._currentUser = _currentUser; }
                }
                return PartialView(_ObjModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult OfferDetails(int InvoiceID = 0, string ProgramType = "", bool IsUpdate = false)
        {
            try
            {
                _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType;

                _ObjModel.UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag._currentUser = _ClsInvoiceTransaction.UserRole_GetDetail(_ObjModel).FirstOrDefault();
                var suppName = "";
                if (ProgramType == CONT.RType)
                {
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    CustomerBankAccDetail _objCustModel = new CustomerBankAccDetail();
                    _objCustModel.CustomerID = Data.SupplierID;
                    //_objCustModel.CustomerSuppID = Data.SupplierID;
                    //_objCustModel.IsCustomerSupp = true;
                    _objCustModel.Status = CONT.ddstatus;
                    suppName = Data.SupplierName;
                    ViewBag.CustomerBankAccID = new SelectList(_ClsCustomerMaster.CustomerBankAccDetail_ListAll(_objCustModel).OrderBy(x => x.BankName).ToList(), "CustomerBankAccID", "CustomerAccountNo");
                }

                if (IsUpdate == false)
                {
                    _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                }
                else
                {
                    _ObjModel.IsUpdate = false;
                    _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_UpdateCalculation(_ObjModel).FirstOrDefault();
                    _ObjModel.SupplierName = suppName;
                }

                return PartialView(_ObjModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult FactoringView(int InvoiceID = 0, string ProgramType = "")
        {
            try
            {
                _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType;
                _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();

                _ObjModel.UserID = Convert.ToInt32(Session["UserID"]);
                ViewBag._currentUser = _ClsInvoiceTransaction.UserRole_GetDetail(_ObjModel).FirstOrDefault();
                return PartialView(_ObjModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult CommonDocPopUp(string TemplateName = "")
        {
            try
            {
                if (TemplateName == "")
                {
                    TemplateName = "Test-temp";
                }
                ViewBag.ThankYouMsg = GetThankYouMsg(TemplateName);
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public PartialViewResult CommonAgreementPopUp(string TemplateName = "", int CustomerID = 0)
        {
            try
            {
                CustomerMasterModel _objCustModel = new CustomerMasterModel();
                ClsCustomerMaster _ClsCustomerMaster = new ClsCustomerMaster();
                DataSet DS = new DataSet();
                _objCustModel.CustomerID = CustomerID;

                if (TemplateName != "")
                    DS = _ClsCustomerMaster.CustomerMaster_BindTemplate_DataSet(_objCustModel);
                else
                    TemplateName = "Test-temp";

                ViewBag.ThankYouMsg = GetDynamicText(TemplateName, DS);
                ViewBag.TemplateName = TemplateName;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public PartialViewResult InvoiceDetailsPopup(int InvoiceID = 0)
        {
            try
            {
                _ObjModel.InvoiceID = InvoiceID;
                var Data = _ClsInvoiceTransaction.InvoiceDetail_ListAll(_ObjModel);
                return PartialView(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult InvoiceCommentHistoryPopup(int InvoiceID = 0)
        {
            try
            {
                //int intCustType = Convert.ToInt32(Session["onlineCustomerTypeID"]);
                //int CustomerID = Convert.ToInt32(Session["CustomerID"]);                 
                //if (intCustType == CONT.ObligorCustomerTypeID)
                //    _ObjModel.ObligorID = CustomerID;
                //else if(intCustType == CONT.SupplierCustomerTypeID)
                //    _ObjModel.SupplierID= CustomerID;
                //else if (intCustType == CONT.FunderCustomerTypeID)
                //    _ObjModel.FunderID = CustomerID;
                //else if (intCustType == CONT.InsuranceProviderTypeID)
                //    _ObjModel.InsuranceID = CustomerID;
                //else if (intCustType == CONT.ServiceProviderCustomerTypeID)
                //    _ObjModel.ServiceProviderID = 0;
                //else if (intCustType == CONT.BuyerCustomerTypeID)
                //    _ObjModel.BuyerID = CustomerID;
                //else if (intCustType == CONT.BothObligorAndBuyerTypeID)
                //    _ObjModel.BuyerID = CustomerID;

                _ObjModel.InvoiceID = InvoiceID;
                _ObjModel.Status = "";
                var Data = _ClsInvoiceTransaction.InvoiceMasterHistory_Remarks(_ObjModel);
                return PartialView(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult CommentRemark(int InvoiceID = 0, string ProgramType = "")
        {
            try
            {
                _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType;
                _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                return PartialView(_ObjModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult InsuranceDetails(int InvoiceID = 0, bool IsSelected = false)
        {
            try
            {
                if (IsSelected == false)
                {
                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.IsSelect = -1;
                    var Data = _ClsInvoiceTransaction.InvoiceInsDetail_ListAll(_ObjModel);
                    return PartialView(Data);
                }
                else
                {
                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.IsSelect = 1;
                    var Data = _ClsInvoiceTransaction.InvoiceInsDetail_ListAll(_ObjModel);
                    return PartialView(Data);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult SupplierHeader(int InvoiceID = 0)
        {
            try
            {
                _ObjModel.InvoiceID = InvoiceID;
                var Data = _ClsInvoiceTransaction.CustomerAccDetail_GetByInvoice(_ObjModel).FirstOrDefault();
                return PartialView(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ISCFHeader(int InvoiceID = 0)
        {
            //try
            //{
            _ObjModel.InvoiceID = InvoiceID;
            var Data = _ClsInvoiceTransaction.CustomerAccDetail_GetByInvoice(_ObjModel).FirstOrDefault();
            return PartialView(Data);
            //}
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
        }
        public PartialViewResult ObligorHeader(int InvoiceID = 0)
        {
            try
            {
                _ObjModel.InvoiceID = InvoiceID;
                var Data = _ClsInvoiceTransaction.CustomerAccDetail_GetByInvoice(_ObjModel).FirstOrDefault();
                return PartialView(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult SettlementHistoryPopup(int InvoiceID = 0, string ProgramType = "")
        {
            try
            {
                _ObjModel.InvoiceID = InvoiceID;
                if (ProgramType == CONT.FType)
                    _ObjModel.Status = CONT.IH;
                else
                    _ObjModel.Status = CONT.IO;
                var Data = _ClsInvoiceTransaction.InvoicePayDetail_ListAll(_ObjModel);
                return PartialView(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ObligorCreditBankDetail(int CustomerID = 0)
        {
            try
            {
                CustomerReviewModel _objModel = new CustomerReviewModel();
                _objModel.CustomerID = CustomerID;
                _objModel = _ClsCustomerMaster.CustomerReviewDetailHistory_ListAllBind(_objModel).FirstOrDefault();
                return PartialView(_objModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public PartialViewResult CommonTransPopUp(string TemplateName = "", int InvoiceID = 0,string ProgramType="")
        {
            try
            {
                ClsHTMLTemplate _ClsHTMLTemplate = new ClsHTMLTemplate();
                if (TemplateName == "")
                {
                    TemplateName = "Test-temp";
                }
                var Data = _ClsHTMLTemplate.HTMLTemplate_ListAll(0, TemplateName, -1, "", false, "", 0).SingleOrDefault();
                ViewBag.TransPop = FN.GetInvoiceMasterData(InvoiceID, Data.HtmlText, Convert.ToInt32(Session["UserId"]));
                ViewBag.TemplateName = TemplateName; ViewBag.InvoiceID = InvoiceID; ViewBag.ProgramType = ProgramType;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PreRequestPDF(string TemplateName = "", int InvoiceID = 0, string ProgramType = "")
        {
            try
            {
                int UserSignID = 0;
                int UserId = Convert.ToInt32(Session["UserId"]); int CustomerID = Convert.ToInt32(Session["CustomerID"]);
                InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                _objModel.InvoiceID = InvoiceID; _objModel.ProgramType = ProgramType;
                _objModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_objModel).FirstOrDefault();
                string filename = _objModel.TranRefNo + TemplateName + ".pdf";
                string FilePath = Server.MapPath("\\Files\\UploadSignDocs\\" + filename + "");
                string RequestUrl = Convert.ToString(WebConfigurationManager.AppSettings["TranPDFpageUrl"]) + "?TemplateName=" + TemplateName + "&InvoiceID=" + InvoiceID + "&UserId=" + UserId;
                SaveHttpResponseAsFile(RequestUrl, FilePath);
                var Data = _ClsHTMLTemplate.HTMLTemplate_ListAll(0, TemplateName, -1, "", false, "", 0).FirstOrDefault();
                UserSignDetailModel _objUser = new UserSignDetailModel();
                _objUser.UserID = UserId; _objUser.CustomerID = CustomerID; _objUser.HtmlTemplateID = Data.HtmlTemplateID; _objUser.ReferenceID = InvoiceID;

                var CheckSig = _ClsCustomerMaster.UserSignDetail_ListAll(_objUser).FirstOrDefault();

                if (CheckSig == null)
                {
                    _ClsCustomerMaster.Conn = ClsAppDatabase.GetCon();
                    if (_ClsCustomerMaster.Conn.State == ConnectionState.Closed) _ClsCustomerMaster.Conn.Open();
                    else { _ClsCustomerMaster.Conn.Close(); _ClsCustomerMaster.Conn.Open(); }
                    _ClsCustomerMaster.Tras = _ClsCustomerMaster.Conn.BeginTransaction();

                    _objUser.Type = "T"; _objUser.OrderNo = 1;
                    _objUser.AttachName = filename;

                    UserSignID = _ClsCustomerMaster.UserSignDetail_Add(_objUser);

                    _ClsCustomerMaster.Tras.Commit();
                    _ClsCustomerMaster.Conn.Close();
                }
                else
                {
                    UserSignID = CheckSig.UserSignID;
                }
                var UserSignIDA = new
                {
                    UserSignID = UserSignID
                };
                var jsonResult = Json(UserSignIDA, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                _ClsCustomerMaster.Tras.Rollback(); _ClsCustomerMaster.Conn.Close();
                throw ex;
            }
        }
        public ActionResult FinalRequestPDF(string TemplateName = "", int InvoiceID = 0, int UserId = 0)
        {
            try
            {

                if (TemplateName == "")
                {
                    TemplateName = "Test-temp";
                }
                var Data = _ClsHTMLTemplate.HTMLTemplate_ListAll(0, TemplateName, -1, "", false, "", 0).FirstOrDefault();
                ViewBag.TransPop = FN.GetInvoiceMasterData(InvoiceID, Data.HtmlText, UserId);
                ViewBag.TemplateName = TemplateName; ViewBag.InvoiceID = InvoiceID;


                return new Rotativa.MVC.ViewAsPdf("FinalRequestPDF", ViewBag.TransPop);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PreAgreementRequestPDF(string TemplateName = "")
        {
            CustomerMasterModel _CustomerMasterModel = new CustomerMasterModel();
            UserSignDetailModel _objUser = new UserSignDetailModel();
            CustomerVerifyModel _CustomerVerifyModel = new CustomerVerifyModel();
            try
            {
                int UserSignID = 0;
                int UserId = Convert.ToInt32(Session["UserId"]); int CustomerID = Convert.ToInt32(Session["CustomerID"]);
                _CustomerMasterModel.CustomerID = CustomerID;
                var CustomerData = _ClsCustomerMaster.CustomerMaster_ListAll(_CustomerMasterModel).FirstOrDefault();
                string filename = CustomerData.CustomerCode + "_" + TemplateName + ".pdf";
                string FilePath = Server.MapPath("\\Files\\UploadSignDocs\\" + filename + "");
                if (!System.IO.File.Exists(FilePath))
                {
                    string RequestUrl = Convert.ToString(WebConfigurationManager.AppSettings["AgreementPDFpageUrl"]) + "?TemplateName=" + TemplateName + "&CustomerID=" + CustomerID + "&UserId=" + UserId;
                    SaveHttpResponseAsFile(RequestUrl, FilePath);
                }

                var Data = _ClsHTMLTemplate.HTMLTemplate_ListAll(0, TemplateName, -1, "", false, "", 0).FirstOrDefault();

                _objUser.UserID = UserId;
                _objUser.CustomerID = CustomerID;
                _objUser.HtmlTemplateID = Data.HtmlTemplateID;
                _objUser.ReferenceID = Convert.ToInt32(Session["CustomerShareHoldID"]);

                _ClsCustomerMaster.Conn = ClsAppDatabase.GetCon();
                if (_ClsCustomerMaster.Conn.State == ConnectionState.Closed) _ClsCustomerMaster.Conn.Open();
                else { _ClsCustomerMaster.Conn.Close(); _ClsCustomerMaster.Conn.Open(); }
                _ClsCustomerMaster.Tras = _ClsCustomerMaster.Conn.BeginTransaction();

                var CheckSig = _ClsCustomerMaster.UserSignDetail_ListAll(_objUser).FirstOrDefault();
                _objUser.Type = "A";
                _objUser.AttachName = filename;

                _CustomerVerifyModel.CustomerID = CustomerID;
                var GetOrderNo = _ClsCustomerMaster.UserMaster_AuthorizedListAll(_CustomerVerifyModel).ToList();

                for (int i = 0; i < GetOrderNo.Count; i++)
                {
                    if (GetOrderNo[i].UserID == UserId)
                        _objUser.OrderNo = Convert.ToInt16(GetOrderNo[i].OrderNo);
                }

                if (CheckSig == null)
                {
                    UserSignID = _ClsCustomerMaster.UserSignDetail_Add(_objUser);
                    _ClsCustomerMaster.Tras.Commit(); _ClsCustomerMaster.Conn.Close();
                }
                else
                {
                    _objUser.UserSignID = CheckSig.UserSignID;
                    _ClsCustomerMaster.UserSignDetail_UpdateDetails(_objUser);
                    UserSignID = CheckSig.UserSignID;
                }

                var UserSignIDA = new
                {
                    UserSignID = UserSignID
                };
                var jsonResult = Json(UserSignIDA, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                _ClsCustomerMaster.Tras.Rollback(); _ClsCustomerMaster.Conn.Close();
                throw ex;
            }
        }
        public ActionResult AgreementRequestPDF(int CustomerID = 0, string TemplateName = "")
        {
            try
            {
                CustomerMasterModel _objCustModel = new CustomerMasterModel();
                ClsCustomerMaster _ClsCustomerMaster = new ClsCustomerMaster();
                DataSet DS = new DataSet();
                _objCustModel.CustomerID = CustomerID;

                if (TemplateName != "")
                    DS = _ClsCustomerMaster.CustomerMaster_BindTemplate_DataSet(_objCustModel);
                else
                    TemplateName = "Test-temp";
                ViewBag.TemplateName = TemplateName;
                ViewBag.AgreementRequestPop = GetDynamicText(TemplateName, DS);
                return new Rotativa.MVC.ViewAsPdf("AgreementRequestPDF", ViewBag.AgreementRequestPop);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        #endregion

        #region iSCF Code
        public ActionResult iSCFIndex(string ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { ViewBag._currentUser = _currentUser; }
                    _ObjModel.Status = _currentStatus(ProgramType);
                    ViewBag.ProgramType = _ObjModel.ProgramType = ProgramType;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        public ActionResult iSCFMaker(int InvoiceID = 0, string ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }

                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType;
                    _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult iSCFMaker(InvoiceTransactionModel _ObjModel, FormCollection frm)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();
                    if (Request["isYes"] != null) { _ObjModel.IsInsurance = true; } else { _ObjModel.IsInsurance = false; }

                    if (Request["Reject"] != null)
                    {
                        if (_ObjModel.ProgramType == CONT.FType)
                        {
                            _ObjModel.Status = CONT.IP;
                        }
                        else
                        {
                            _ObjModel.Status = CONT.II;
                        }

                    }
                    else if (Request["Approve"] != null || Request["Yes"] != null)
                    {

                        if (_ObjModel.ProgramType == CONT.FType)
                        {
                            _ObjModel.Status = CONT.IA;
                        }
                        else
                        {
                            _ObjModel.Status = CONT.IF;
                        }
                    }
                    _ObjModel.ServiceProviderID = Convert.ToInt32(Session["CustomerID"]);
                    _ObjModel.IsUpdate = true;
                    _ClsInvoiceTransaction.InvoiceMaster_UpdateCalculation(_ObjModel);
                    _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_ObjModel);
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    return RedirectToAction("CMNDashboard", "Home");

                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        public ActionResult iSCFChecker(int InvoiceID = 0, String ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }

                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType;
                    _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult iSCFChecker(InvoiceTransactionModel _ObjModel, FormCollection frm)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();
                    _ObjModel.ServiceProviderID = Convert.ToInt32(Session["CustomerID"]);
                    if (Request["Approve"] != null || Request["Yes"] != null)
                    {
                        if (_ObjModel.Status == CONT.RA || _ObjModel.Status == CONT.RC)
                        {
                            _ObjModel.InvoiceInsID = Convert.ToInt32(Request.Form["radio"]);
                            if (_ObjModel.ProgramType == CONT.FType)
                            {
                                _ObjModel.Status = CONT.IC;
                            }
                            else
                            {
                                _ObjModel.Status = CONT.IL;
                            }
                            _ObjModel.InsuranceID = _ClsInvoiceTransaction.InvoiceInsDetail_UpdateSelected(_ObjModel);
                        }
                        else
                        {

                            if (_ObjModel.ProgramType == CONT.FType)
                            {
                                if (_ObjModel.IsObligorGen)
                                {
                                    _ObjModel.Status = CONT.IU;
                                }
                                else
                                {
                                    _ObjModel.Status = CONT.FB;//CONT.IB;
                                }

                            }
                            else
                            {
                                _ObjModel.Status = CONT.FD; //CONT.ID;//CONT.IJ;
                            }

                        }
                    }
                    else if (Request["Reject"] != null)
                    {
                        if (_ObjModel.Status == CONT.IA)
                        {

                            if (_ObjModel.ProgramType == CONT.FType)
                            {
                                _ObjModel.Status = CONT.IR;
                            }
                            else
                            {
                                _ObjModel.Status = CONT.IK;
                            }
                        }
                        else
                        {

                            if (_ObjModel.ProgramType == CONT.FType)
                            {
                                _ObjModel.Status = CONT.IT;
                            }
                            else
                            {
                                _ObjModel.Status = CONT.IM;
                            }
                        }

                    }
                    else if (Request["Return"] != null)
                    {
                        if (_ObjModel.ProgramType == CONT.FType)
                        {
                            _ObjModel.Status = CONT.IV;
                        }
                        else
                        {
                            _ObjModel.Status = CONT.IW;
                        }
                    }

                    _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_ObjModel);
                    _ClsInvoiceTransaction.Tras.Commit();
                    _ClsInvoiceTransaction.Conn.Close();

                    if (_ObjModel.ProgramType == CONT.FType)
                    {
                        if (_ObjModel.Status == CONT.IB)
                        {
                            EMailNotification(FN.LoggedUserID, _ObjModel.iSCFEmailID, _ObjModel.SupplierEmailID, _ObjModel.InvoiceID, CONT.iSCFChecker_IB_Supplier, "Approve Successfully.");
                            EMailNotification(FN.LoggedUserID, _ObjModel.iSCFEmailID, _ObjModel.ObligorEmailID, _ObjModel.InvoiceID, CONT.iSCFChecker_IB_Obligor, "Approve Successfully.");
                        }
                        else if (_ObjModel.Status == CONT.IR)
                        {
                            EMailNotification(FN.LoggedUserID, _ObjModel.iSCFEmailID, _ObjModel.SupplierEmailID, _ObjModel.InvoiceID, CONT.iSCFChecker_IR_Supplier, "Reject Successfully.");
                        }
                    }
                    else
                    {
                        if (_ObjModel.Status == CONT.IJ)
                        {
                            EMailNotification(FN.LoggedUserID, _ObjModel.iSCFEmailID, _ObjModel.BuyerEmailID, _ObjModel.InvoiceID, CONT.iSCFChecker_IB_Buyer, "Approve Successfully.");
                        }
                        else if (_ObjModel.Status == CONT.IK)
                        {
                            EMailNotification(FN.LoggedUserID, _ObjModel.iSCFEmailID, _ObjModel.SupplierEmailID, _ObjModel.InvoiceID, CONT.iSCFChecker_IR_Buyer, "Reject Successfully.");
                        }
                    }
                    return RedirectToAction("CMNDashboard", "Home");

                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        #endregion

        #region Invoice Insurance Provider
        public ActionResult InvoiceInsuranceIndex(String ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { ViewBag._currentUser = _currentUser; }
                    _ObjModel.Status = _currentStatus(ProgramType);

                    ViewBag.ProgramType = _ObjModel.ProgramType = ProgramType; _ObjModel.IsInsuranceVal = 1;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                return View(_ObjModel);
            }
        }
        public ActionResult InvoiceInsuranceReview(int InvoiceID = 0, String ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _ObjModel._currentUser = _currentUser; }

                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType; _ObjModel.IsInsuranceVal = 1;
                    _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult InvoiceInsuranceReview(InvoiceTransactionModel _ObjModel)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {

                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    if (Request["Return"] != null)
                    {
                        _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                        if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                        else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                        _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                        _ObjModel.Status = CONT.RP;
                        _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_ObjModel);
                        _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    else if (Request["AddInsurance"] != null)
                    {
                        return RedirectToAction("InvoiceInsuranceAdd", "InvoiceCommon", new { InvoiceID = _ObjModel.InvoiceID, ProgramType = _ObjModel.ProgramType });
                    }
                    else if (Request["Summary"] != null)
                    {


                        if (_ObjModel.ProgramType == CONT.RType)
                        {
                            _ObjModel.ObligorID = _ObjModel.BuyerID;
                        }


                        return RedirectToAction("FundingPartnerObligorDetails", "Factoring", new { InvoiceID = _ObjModel.InvoiceID, ObligorID = _ObjModel.ObligorID, SupplierID = _ObjModel.SupplierID, ProgramType = _ObjModel.ProgramType });

                    }
                    return View(_ObjModel);

                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        public ActionResult InvoiceInsuranceAdd(int InvoiceID = 0, int InvoiceInsID = 0, int EditID = 0, int DeleteID = 0, string ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
                    _ObjModel.ProgramType = ProgramType;
                    _ObjModel.InvoiceID = InvoiceID;
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _ObjModel._currentUser = _currentUser; }

                    if (EditID > 0)
                    {
                        _ObjModel.InvoiceInsID = EditID; _ObjModel.IsSelect = -1;
                        _ObjModel = _ClsInvoiceTransaction.InvoiceInsDetail_ListAll(_ObjModel).FirstOrDefault();
                    }
                    else if (DeleteID > 0)
                    {
                        _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                        if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) { _ClsInvoiceTransaction.Conn.Open(); }
                        else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                        _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();
                        _ObjModel.InvoiceInsID = DeleteID;
                        _ClsInvoiceTransaction.InvoiceInsDetail_Delete(_ObjModel);
                        _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        return RedirectToAction("InvoiceInsuranceAdd", "InvoiceCommon", new { InvoiceID = _ObjModel.InvoiceID, DeleteID = 0, ProgramType = _ObjModel.ProgramType });
                    }

                    FillInsuranceCombo(); FillInsuranceBrokerCombo();
                    int dInsuranceID = _ObjModel.InsuranceID;
                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.CreateBy = 0; _ObjModel.CreateBy = 0; _ObjModel.InsuranceID = 0;
                    var GetTenor = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    _ObjModel.Tenor = GetTenor.Tenor; _ObjModel.InsCnt = GetTenor.InsCnt;
                    _ObjModel.InsuranceID = dInsuranceID; _ObjModel.InsuranceBrokerID = Convert.ToInt32(Session["CustomerID"]);
                    if (EditID == 0)
                    {
                        CustomerVerifyModel _objFundInsModel = new CustomerVerifyModel();
                        _objFundInsModel.CustomerID = GetTenor.FunderID;
                        var GetFunderInsu = _ClsCustomerMaster.CustomerVerifyDetail_ListAll(_objFundInsModel).FirstOrDefault();
                        _ObjModel.PolicyNo = GetFunderInsu.PolicyNo; _ObjModel.IssueDate = GetFunderInsu.IssueDate; _ObjModel.ExpDate = GetFunderInsu.ExpDate;
                        _ObjModel.InsAmt = GetTenor.TotAmt;
                    }
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                FillInsuranceCombo(); FillInsuranceBrokerCombo();
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult InvoiceInsuranceAdd(InvoiceTransactionModel _ObjModel, FormCollection frm)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();
                    if (Request["btnSave"] != null)
                    {
                        if (_ObjModel.InvoiceInsID > 0)
                        {
                            _ClsInvoiceTransaction.InvoiceInsDetail_Update(_ObjModel);
                        }
                        else
                        {
                            _ClsInvoiceTransaction.InvoiceInsDetail_Add(_ObjModel);
                        }
                        _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        return RedirectToAction("InvoiceInsuranceAdd", "InvoiceCommon", new { InvoiceID = _ObjModel.InvoiceID, ProgramType = _ObjModel.ProgramType });
                    }
                    else if (Request["btnSubmit"] != null)
                    {
                        if (_ObjModel.ProgramType == CONT.FType)
                        {
                            _ObjModel.Status = CONT.RA;
                        }
                        else
                        {
                            _ObjModel.Status = CONT.RC;
                        }

                        _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_ObjModel);
                        _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                FillInsuranceCombo(); FillInsuranceBrokerCombo();
                return View(_ObjModel);
            }
        }
        public PartialViewResult InvoiceInsuranceGrid(int InvoiceID = 0)
        {
            _ObjModel.InvoiceID = InvoiceID; _ObjModel.IsSelect = -1;
            var Data = _ClsInvoiceTransaction.InvoiceInsDetail_ListAll(_ObjModel);
            return PartialView(Data);
        }

        public ActionResult InvoiceInsuranceChecker(int InvoiceInsID = 0, String ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _ObjModel._currentUser = _currentUser; }

                    _ObjModel.InvoiceInsID = InvoiceInsID; _ObjModel.ProgramType = ProgramType; _ObjModel.IsSelect = 1;
                    _ObjModel = _ClsInvoiceTransaction.InvoiceInsDetail_ListAll(_ObjModel).FirstOrDefault();
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult InvoiceInsuranceChecker(InvoiceTransactionModel _ObjModel, HttpPostedFileBase Attach)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    string newfilenm1 = Guid.NewGuid().ToString().Substring(0, 12);
                    if (Attach != null)
                    {
                        if (Attach.FileName != null)
                        {
                            _ObjModel.Attach = newfilenm1 + Attach.FileName.ToString();
                            Attach.SaveAs(Server.MapPath("\\Files\\Invoice\\" + _ObjModel.Attach));
                        }
                    }

                    _ClsInvoiceTransaction.InvoiceInsDetail_Update(_ObjModel);

                    if (_ObjModel.ProgramType == CONT.FType)
                    {
                        _ObjModel.Status = CONT.RB;
                    }
                    else
                    {
                        _ObjModel.Status = CONT.RD;
                    }
                    _ObjModel.InsuranceID = _ObjModel.InsuranceBrokerID;
                    _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_ObjModel);
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    return RedirectToAction("CMNDashboard", "Home");

                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        public PartialViewResult iSCFInvoiceInsuranceGrid(int InvoiceID = 0)
        {
            _ObjModel.InvoiceID = InvoiceID; _ObjModel.IsSelect = -1;
            var Data = _ClsInvoiceTransaction.InvoiceInsDetail_ListAll(_ObjModel);
            return PartialView(Data);
        }
        #endregion

        #region Commodity
        public ActionResult CommodityIndex(String ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }
                    _ObjModel.ProgramType = ProgramType;
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }

        public ActionResult FunderToiSCF(int InvoiceID = 0, string ProgramType = "", string Status = "")
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {

                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { _objModel._currentUser = _currentUser; }

                    ViewBag.ProgramType = _objModel.ProgramType = ProgramType; _objModel.Status = Status; _objModel.InvoiceID = InvoiceID;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_objModel);
                    ViewBag.ProgramType = ProgramType; ViewBag.Status = Status;
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_objModel);
            }
        }
        [HttpPost]
        public ActionResult FunderToiSCF(FormCollection frm, InvoiceTransactionModel _Model)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Back"] != null)
                    {
                        return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    }
                    _ObjModel.ProgramType = _Model.ProgramType; _ObjModel.Status = _Model.Status;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int intInvID = Data[i].InvoiceID;

                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("RefNo_" + intInvID + ""))
                            {
                                //change for hk demo
                                //_Model.Status = CONT.CA;
                                //_Model.ComLevel = CONT.CA;
                                if (_Model.ProgramType == CONT.FType)
                                { _Model.Status = CONT.CB; _Model.ComLevel = CONT.CB; }
                                else { _Model.Status = CONT.CE; _Model.ComLevel = CONT.CE; }
                                _Model.InvoiceID = intInvID;

                                _Model.RefNo = Convert.ToString(frm["RefNo_" + intInvID + ""]);
                                _Model.TranDate = Convert.ToDateTime(frm["TranDate_" + intInvID + ""]);
                                _Model.ChargeAmt = Convert.ToDecimal(frm["ChargeAmt_" + intInvID + ""]);

                                _ClsInvoiceTransaction.InvoiceComDetail_Add(_Model);
                                _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_Model);
                            }
                        }
                    }
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    //return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    return RedirectToAction("CMNDashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_Model);
            }
        }

        public ActionResult BuyCommodityConfirmation(int InvoiceID = 0, string ProgramType = "", string Status = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ObjModel.ProgramType = ProgramType; _ObjModel.Status = Status; _ObjModel.InvoiceID = InvoiceID;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);
                    ViewBag.ProgramType = ProgramType; ViewBag.Status = Status;
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult BuyCommodityConfirmation(FormCollection frm, InvoiceTransactionModel _Model)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Back"] != null)
                    {
                        return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    }
                    _ObjModel.ProgramType = _Model.ProgramType; _ObjModel.Status = _Model.Status;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int intInvID = Data[i].InvoiceID;

                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("CertificateNo_" + intInvID + ""))
                            {
                                if (_Model.ProgramType == CONT.FType)
                                { _Model.Status = CONT.CB; _Model.ComLevel = CONT.CB; }
                                else { _Model.Status = CONT.CE; _Model.ComLevel = CONT.CE; }

                                _Model.InvoiceID = intInvID;

                                _Model.CertificateNo = Convert.ToString(frm["CertificateNo_" + intInvID + ""]);
                                _Model.QtyComm = Convert.ToDecimal(frm["QtyComm_" + intInvID + ""]);
                                _Model.CommValue = Convert.ToString(frm["CommValue_" + intInvID + ""]);

                                _ClsInvoiceTransaction.InvoiceComDetail_Add(_Model);
                                _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_Model);
                            }
                        }
                    }
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    //return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    return RedirectToAction("CMNDashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_Model);
            }
        }

        public ActionResult SellCommodityConfirmation(int InvoiceID = 0, string ProgramType = "", string Status = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ObjModel.ProgramType = ProgramType; _ObjModel.Status = Status; _ObjModel.InvoiceID = InvoiceID;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);
                    ViewBag.ProgramType = ProgramType; ViewBag.Status = Status;
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult SellCommodityConfirmation(FormCollection frm, InvoiceTransactionModel _Model)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Back"] != null)
                    {
                        return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    }
                    _ObjModel.ProgramType = _Model.ProgramType; _ObjModel.Status = _Model.Status;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int intInvID = Data[i].InvoiceID;

                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("CertificateNo_" + intInvID + ""))
                            {
                                if (_Model.ProgramType == CONT.FType)
                                { _Model.Status = CONT.CC; _Model.ComLevel = CONT.CC; }
                                else { _Model.Status = CONT.CF; _Model.ComLevel = CONT.CF; }
                                _Model.InvoiceID = intInvID;

                                _Model.CertificateNo = Convert.ToString(frm["CertificateNo_" + intInvID + ""]);
                                _Model.QtyComm = Convert.ToDecimal(frm["QtyComm_" + intInvID + ""]);
                                _Model.CommValue = Convert.ToString(frm["CommValue_" + intInvID + ""]);

                                _ClsInvoiceTransaction.InvoiceComDetail_Add(_Model);
                                _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_Model);
                            }
                        }
                    }
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    //return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    return RedirectToAction("CMNDashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_Model);
            }
        }

        public ActionResult iSCFToSupplier(int InvoiceID = 0, string ProgramType = "", string Status = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ObjModel.ProgramType = ProgramType; _ObjModel.Status = Status; _ObjModel.InvoiceID = InvoiceID;
                    //change for hk demo
                    //var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).Where(d => d.IsDispToCD == true).ToList();
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).ToList();
                    ViewBag.ProgramType = ProgramType; ViewBag.Status = Status;
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult iSCFToSupplier(FormCollection frm, InvoiceTransactionModel _Model)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Back"] != null)
                    {
                        return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    }
                    _ObjModel.ProgramType = _Model.ProgramType; _ObjModel.Status = _Model.Status;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int intInvID = Data[i].InvoiceID;

                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("RefNo_" + intInvID + ""))
                            {
                                _Model.Status = CONT.CD;
                                _Model.ComLevel = CONT.CD;
                                _Model.InvoiceID = intInvID;

                                _Model.RefNo = Convert.ToString(frm["RefNo_" + intInvID + ""]);
                                _Model.TranDate = Convert.ToDateTime(frm["TranDate_" + intInvID + ""]);
                                _Model.ChargeAmt = Convert.ToDecimal(frm["ChargeAmt_" + intInvID + ""]);

                                _ClsInvoiceTransaction.InvoiceComDetail_Add(_Model);
                                _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_Model);
                            }
                        }
                    }
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    //return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    return RedirectToAction("CMNDashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_Model);
            }
        }
        public ActionResult TIToExpoter(int InvoiceID = 0, string ProgramType = "", string Status = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ObjModel.ProgramType = ProgramType; _ObjModel.Status = Status; _ObjModel.InvoiceID = InvoiceID;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).Where(d => d.IsDispToCH == false).ToList();
                    ViewBag.ProgramType = ProgramType; ViewBag.Status = Status;
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult TIToExpoter(FormCollection frm, InvoiceTransactionModel _Model)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Back"] != null)
                    {
                        return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    }
                    _ObjModel.ProgramType = _Model.ProgramType; _ObjModel.Status = _Model.Status;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int intInvID = Data[i].InvoiceID;

                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("RefNo_" + intInvID + ""))
                            {
                                _Model.Status = CONT.CH;
                                _Model.ComLevel = CONT.CH;
                                _Model.InvoiceID = intInvID;

                                _Model.RefNo = Convert.ToString(frm["RefNo_" + intInvID + ""]);
                                _Model.TranDate = Convert.ToDateTime(frm["TranDate_" + intInvID + ""]);
                                _Model.ChargeAmt = Convert.ToDecimal(frm["ChargeAmt_" + intInvID + ""]);

                                _ClsInvoiceTransaction.InvoiceComDetail_Add(_Model);
                                _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_Model);
                            }
                        }
                    }
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    //return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    return RedirectToAction("CMNDashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_Model);
            }
        }

        public ActionResult iSCFToFunder(int InvoiceID = 0, string ProgramType = "", string Status = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ObjModel.ProgramType = ProgramType; _ObjModel.Status = Status; _ObjModel.InvoiceID = InvoiceID;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);
                    ViewBag.ProgramType = ProgramType; ViewBag.Status = Status;
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult iSCFToFunder(FormCollection frm, InvoiceTransactionModel _Model)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Back"] != null)
                    {
                        return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    }
                    _ObjModel.ProgramType = _Model.ProgramType; _ObjModel.Status = _Model.Status;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int intInvID = Data[i].InvoiceID;

                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("RefNo_" + intInvID + ""))
                            {
                                //change for hk demo
                                //_Model.Status = CONT.CG;
                                //_Model.ComLevel = CONT.CG;
                                _Model.InvoiceID = intInvID;
                                if (_Model.ProgramType == CONT.FType)
                                { _Model.Status = CONT.CB; _Model.ComLevel = CONT.CB; }
                                else { _Model.Status = CONT.CE; _Model.ComLevel = CONT.CE; }
                                _Model.RefNo = Convert.ToString(frm["RefNo_" + intInvID + ""]);
                                _Model.TranDate = Convert.ToDateTime(frm["TranDate_" + intInvID + ""]);
                                _Model.ChargeAmt = Convert.ToDecimal(frm["ChargeAmt_" + intInvID + ""]);

                                _ClsInvoiceTransaction.InvoiceComDetail_Add(_Model);
                                _ClsInvoiceTransaction.InvoiceMaster_UpdateAllStatus(_Model);
                            }
                        }
                    }
                    _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                    //return RedirectToAction("CommodityIndex", "InvoiceCommon", new { ProgramType = _Model.ProgramType });
                    return RedirectToAction("CMNDashboard", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_Model);
            }
        }
        public ActionResult CommodityHistory(string ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { ViewBag._currentUser = _currentUser; }
                    if (ProgramType == CONT.FType)
                    {
                        _ObjModel.Status = CONT.commudity_Fhistory;
                    }
                    else
                    {
                        _ObjModel.Status = CONT.commudity_Rhistory;
                    }
                    _ObjModel.ProgramType = ProgramType;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);
                    return View(Data);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        public PartialViewResult commudityHistList(int InvoiceID = 0)
        {
            try
            {
                _ObjModel.InvoiceID = InvoiceID;
                _ObjModel.Status = "";
                var Data = _ClsInvoiceTransaction.InvoiceComDetail_ListAll(_ObjModel);
                return PartialView(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Settlement
        public ActionResult SettlementIndex(String ProgramType = "", string IndexStatus = "", string SettlementType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    string _currentUser = _RightsNoaccessPage();
                    if (_currentUser == null) { return RedirectToAction("NoaccessPage", "MasterPage"); }
                    else { ViewBag._currentUser = _currentUser; }
                    _ObjModel.Status = _currentStatus(ProgramType);

                    ViewBag.ProgramType = _ObjModel.ProgramType = ProgramType;
                    _ObjModel.Status = IndexStatus;
                    ViewBag.SettlementType = SettlementType;
                    if (SettlementType == "Pending")
                    {
                        var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAllPenSettlement(_ObjModel);
                        return View(Data);
                    }
                    else
                    {
                        var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAllSettlement(_ObjModel);
                        return View(Data);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                return View(_ObjModel);
            }
        }
        public ActionResult Settlement(int InvoiceID = 0, String ProgramType = "", string IndexStatus = "", string SettlementType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    FillSettlementStatusCombo();
                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType;
                    _ObjModel = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    _ObjModel.IndexStatus = IndexStatus;
                    _ObjModel.SettlementType = SettlementType;
                    return View(_ObjModel);
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                FillSettlementStatusCombo();
                return View(_ObjModel);
            }
        }
        [HttpPost]
        public ActionResult Settlement(InvoiceTransactionModel _Model, FormCollection frm)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["btnBack"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();


                    if (Request["btnSubmit"] != null)
                    {
                        if (_Model.SettlementType == "Pending")
                        {
                            if (_Model.ProgramType == CONT.FType)
                                _Model.TypeDesc = CONT.F_Pending_Settlement;
                            else
                                _Model.TypeDesc = CONT.R_Pending_Settlement;
                            _ClsInvoiceTransaction.InvoicePayDetail_Add(_Model);

                            if (_Model.ProgramType == CONT.RType)
                            {
                                if (Request["chk_Buyer"] != null)
                                {
                                    _Model.TypeStatus = ""; _Model.RefNo = "";
                                    _Model.TypeDesc = CONT.R_Pending_Settlement_Add_Charges_from_Buyer;
                                    _Model.PayDate = _Model.Buyer_PaidDate;
                                    _Model.Amt = _Model.Buyer_PaidAmt;
                                    _Model.RefNo = _Model.Buyer_RefNo;
                                    _Model.ProcessRemark = _Model.Buyer_ProcessRemark;
                                    _ClsInvoiceTransaction.InvoicePayDetail_Add(_Model);
                                }

                                if (Request["chk_Funder"] != null)
                                {
                                    _Model.TypeStatus = ""; _Model.RefNo = "";
                                    _Model.TypeDesc = CONT.R_Pending_Settlement_Add_Paid_to_Funder;
                                    _Model.PayDate = _Model.Funder_PaidDate;
                                    _Model.Amt = _Model.Funder_PaidAmt;
                                    _Model.RefNo = _Model.Funder_RefNo;
                                    _Model.ProcessRemark = _Model.Funder_ProcessRemark;
                                    _ClsInvoiceTransaction.InvoicePayDetail_Add(_Model);
                                }

                                if (Request["chk_Security"] != null)
                                {
                                    _Model.TypeStatus = ""; _Model.RefNo = "";
                                    _Model.TypeDesc = CONT.R_Pending_Settlement_Security_Margin;
                                    _Model.PayDate = _Model.Security_PaidDate;
                                    _Model.Amt = _Model.Security_PaidAmt;
                                    _Model.RefNo = _Model.Security_RefNo;
                                    _Model.ProcessRemark = _Model.Security_ProcessRemark;
                                    _ClsInvoiceTransaction.InvoicePayDetail_Add(_Model);
                                }
                            }

                            _ClsInvoiceTransaction.InvoiceMaster_UpdatePenSettlement(_Model);
                            _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                            return RedirectToAction("CMNDashboard", "Home");
                        }
                        else
                        {
                            if (_Model.ProgramType == CONT.FType)
                                _Model.TypeDesc = CONT.F_Settlement_by_Obligor;
                            else
                                _Model.TypeDesc = CONT.R_Settlement_by_Buyer;
                            _ClsInvoiceTransaction.InvoicePayDetail_Add(_Model);

                            if (Request["chk_Security"] != null)
                            {
                                _Model.TypeStatus = ""; _Model.RefNo = "";
                                _Model.PayDate = _Model.Security_PaidDate;
                                _Model.Amt = _Model.Security_PaidAmt;
                                _Model.RefNo = _Model.Security_RefNo;
                                _Model.ProcessRemark = _Model.Security_ProcessRemark;
                                _ClsInvoiceTransaction.InvoicePayDetail_Add(_Model);
                            }
                            _ClsInvoiceTransaction.InvoiceMaster_UpdateSettlement(_Model);
                            _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        }


                        return RedirectToAction("CMNDashboard", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
                FillSettlementStatusCombo();
                return View(_Model);
            }
            FillSettlementStatusCombo();
            return View(_Model);
        }
        #endregion

        #region Profit And Processing Fees

        public ActionResult ProfitAndFees(int InvoiceID = 0, string ProgramType = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = ProgramType;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    _ObjModel.TranRefNo = Data.TranRefNo; _ObjModel.CurrencyCode = Data.CurrencyCode;
                    _ObjModel.ProfitAmt = Data.ProfitAmt; _ObjModel.ProcessFeeAmt = Data.ProcessFeeAmt; _ObjModel.TranFeeAmt = Data.TranFeeAmt;
                    FillCurrencyCombo(); FillBankCombo();
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            return View(_ObjModel);
        }
        [HttpPost]
        public ActionResult ProfitAndFees(InvoiceTransactionModel _ObjModel)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    if (Request["Cancel"] != null)
                    {
                        return RedirectToAction("CMNDashboard", "Home");
                    }

                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    if (Request["btnSubmit"] != null)
                    {
                        _ClsInvoiceTransaction.InvoiceMaster_UpdatePaymentFees(_ObjModel);
                        _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        return RedirectToAction("CMNDashboard", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "mUserMasters");
                }
            }
            catch (Exception ex)
            {
                _ClsInvoiceTransaction.Tras.Rollback(); _ClsInvoiceTransaction.Conn.Close();
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
                if (ex.InnerException == null) ViewBag.ErrorMesssage = ex.Message; else ViewBag.ErrorMesssage = ex.InnerException.Message;
            }
            FillCurrencyCombo(); FillBankCombo();
            return View(_ObjModel);
        }
        #endregion

        #region Common Function       
        [HttpPost]
        public JsonResult GetExtendedTenor(int InvoiceID, DateTime ExtDueDate)
        {
            InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
            _ObjModel.InvoiceID = InvoiceID; _ObjModel.ExtDueDate = ExtDueDate;
            var result = _ClsInvoiceTransaction.InvoiceMaster_ExtendedTenor(_ObjModel).FirstOrDefault();
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetProfitRate(int InvoiceID, decimal FunProfitRate)
        {
            InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
            _ObjModel.InvoiceID = InvoiceID; _ObjModel.FunProfitRate = FunProfitRate;
            var result = _ClsInvoiceTransaction.InvoiceMaster_FunProfitAmt(_ObjModel).FirstOrDefault();
            if (result == null)
                return null;
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public void FillCurrencyCombo()
        {
            try
            {
                ClsCurrency _ClsCurrency = new ClsCurrency();
                var DDLCurrencyData = _ClsCurrency.CurrencyMaster_ListAll(0, "", "", 1, "", false, "").OrderBy(x => x.CurrencyCode).ToList();
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
        public void FillBankCombo()
        {
            try
            {
                ClsBankMaster _clsBank = new ClsBankMaster();
                var DDLBankData = _clsBank.BankMaster_ListAll(0, "", 0, 0, 0, 1, "", false, "").OrderBy(x => x.BankName).ToList();
                if (DDLBankData != null && DDLBankData.Count > 0)
                    ViewBag.DDLBankID = DDLBankData;
                else
                    ViewBag.DDLBankID = new List<SelectListItem> { };
            }
            catch (Exception ex)
            {
                ViewBag.DDLBankID = new List<SelectListItem> { };
                throw ex;
            }
        }
        public void FillInsuranceCombo()
        {
            try
            {
                _ObjModelCustomer.CustomerTypeID = CONT.InsuranceProviderTypeID;
                _ObjModelCustomer.Status = CONT.ddstatus;
                var DDLInsuranceIDData = _ClsCustomerMaster.CustomerMaster_ListAll(_ObjModelCustomer);
                if (DDLInsuranceIDData != null && DDLInsuranceIDData.Count > 0)
                    ViewBag.DDLInsuranceID = DDLInsuranceIDData;
                else
                    ViewBag.DDLInsuranceID = new List<SelectListItem> { };
            }
            catch (Exception ex)
            {
                ViewBag.DDLInsuranceID = new List<SelectListItem> { };
                throw ex;
            }
        }
        public void FillInsuranceBrokerCombo()
        {
            try
            {
                CustomerMasterModel _ObjModelCustomer = new CustomerMasterModel();
                //_ObjModelCustomer.CustomerTypeID = CONT.InsuranceBrokerTypeID;
                //_ObjModelCustomer.Status = CONT.ddstatus;
                //for it self broker
                _ObjModelCustomer.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                var DDLInsuranceBrokerIDData = _ClsCustomerMaster.CustomerMaster_ListAll(_ObjModelCustomer);
                if (DDLInsuranceBrokerIDData != null && DDLInsuranceBrokerIDData.Count > 0)
                    ViewBag.DDLInsuranceBrokerID = DDLInsuranceBrokerIDData;
                else
                    ViewBag.DDLInsuranceBrokerID = new List<SelectListItem> { };
            }
            catch (Exception ex)
            {
                ViewBag.DDLInsuranceID = new List<SelectListItem> { };
                throw ex;
            }
        }
        public string _RightsNoaccessPage()
        {
            string MenuId = Convert.ToString(Session["MenuID"]);
            int ParentMenuID = 0;
            int.TryParse(MenuId, out ParentMenuID);
            var UserRight = FN.CheckUserRight("", "", ParentMenuID);
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
            var UserRight = FN.CheckUserRight("", "", ParentMenuID);
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
        protected string GetThankYouMsg(string tempName)
        {
            string msg = string.Empty;
            DataTable DT = null;
            CustomerTempModel _model = new CustomerTempModel();
            msg = FN.GetTempMsg(tempName, DT);
            return msg;
        }
        protected string GetDynamicText(string TemplateName, DataSet DS)
        {
            string MessageText = string.Empty;
            string ViewSample = "";
            HTMLTemplate_ListAll_Result _objTemplate = new HTMLTemplate_ListAll_Result();
            ClsHTMLTemplate _clsTemp = new ClsHTMLTemplate();
            _objTemplate.HtmlTemplateID = 0;
            _objTemplate.HtmlName = TemplateName;
            _objTemplate = _clsTemp.HTMLTemplate_ListAll(_objTemplate.HtmlTemplateID, TemplateName, 1, "", false, "", 0).FirstOrDefault();

            if (DS == null)
            {
                ViewSample = _objTemplate.HtmlText;
            }
            if (_objTemplate != null && DS != null)
            {
                MessageText = _objTemplate.HtmlText;
                for (int i = 0; i < DS.Tables.Count; i++)
                {
                    #region Dynamic Table Generate
                    if (TemplateName == CONT.ES_Master_Factoring_Solution_Agreement || TemplateName == CONT.ES_Factoring_ITS
                     || TemplateName == CONT.ES_Agency_Agreement_2 || TemplateName == CONT.ES_Reverse_Factoring_ITS || TemplateName == CONT.ES_Agency_Agreement
                     || TemplateName == CONT.ES_Master_Assignment_Agreement || TemplateName == CONT.ES_Master_Wakala_Agreement 
                     || TemplateName == CONT.ES_Reverse_Factoring_Master_Murabaha_Agreement || TemplateName == CONT.ES_Letter_of_Awarness)
                    {
                        if (DS.Tables[i].TableName == "Table1")
                        {
                            string htmlAuthoriseAgreement = "";
                            htmlAuthoriseAgreement = GetAuthoriseAgreementHtml(DS, i);
                            DS.Tables[i].Columns.Add("AuthoriseAgreement", typeof(string));
                            DS.Tables[i].Rows[0]["AuthoriseAgreement"] = htmlAuthoriseAgreement;
                        }
                    }
                    #endregion


                    foreach (DataRow dr in DS.Tables[i].Rows)
                    {
                        if (i == 0)
                            ViewSample = FN.GetText(dr, MessageText);
                        else
                            ViewSample = FN.GetText(dr, ViewSample);
                    }
                }
            }
            return ViewSample;
        }       
        public string GetAuthoriseAgreementHtml(DataSet Ds, int TableNo)
        {
            try
            {
                string messageBody = "";
                int i = 0;
                foreach (DataRow dr in Ds.Tables[TableNo].Rows)
                {
                    i++;
                    messageBody = messageBody + "( " + Convert.ToString(i) + " ) <br />";
                    messageBody = messageBody + "For and on behalf of :   <br />";
                    messageBody = messageBody + "<b>" + Convert.ToString(dr["CompanyName"]) + "</b> <br /> <br />";
                    messageBody = messageBody + "Authorised Signatory :  <br /> <br />";
                    messageBody = messageBody + "Name : " + Convert.ToString(dr["ContactFullName"]) + "<br /> <br />";
                    messageBody = messageBody + "Title : " + Convert.ToString(dr["DesignationName"]) + "<br /> <br />";
                    messageBody = messageBody + "Date : " + Convert.ToDateTime(dr["TodayDate"]).ToString("dd-MMM-yyyy") + "<br /> <br /> <br />";
                }
                
                return messageBody;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public void FillSettlementStatusCombo()
        {
            ViewBag.DDLSettlementStatusID = new List<SelectListItem>
            { new SelectListItem() { Text = "Funds Transfer Completed", Value = "Funds Transfer Completed" },
              new SelectListItem() { Text = "Settled by Obligor", Value = "Settled by Obligor" },
              new SelectListItem() { Text = "Paid By Insurance", Value = "Paid By Insurance" } };
        }
        private void EMailNotification(int intFromCustomerID, string FromEmailID, string ToEmailID, int InvoiceID, string HTMLTemplateName, string ResponseText)
        {
            string MessageText = string.Empty;
            string SubjectText = string.Empty;
            ClsHTMLTemplate _ClsHTMLTemplate = new ClsHTMLTemplate();
            try
            {
                if (Convert.ToString(WebConfigurationManager.AppSettings["IsSendTransactionMail"]).ToLower() == "true")
                {
                    //_ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    //if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    //else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    //_ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    if (FromEmailID != null && ToEmailID != null)
                    {
                        var Data = _ClsHTMLTemplate.HTMLTemplate_ListAll(0, HTMLTemplateName, -1, "", false, "", 0).FirstOrDefault();

                        MessageText = _ClsInvoiceTransaction.GetInvoiceMasterData(InvoiceID, Data.HtmlText);
                        SubjectText = _ClsInvoiceTransaction.GetInvoiceMasterData(InvoiceID, Data.Subject);

                        FN.SendEmail(ToEmailID, SubjectText, MessageText, Data.HtmlTemplateID, intFromCustomerID, InvoiceID, ResponseText, 1);
                    }

                }
            }
            catch (Exception ex)
            {
                //if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Open)
                //{
                //    _ClsInvoiceTransaction.Tras.Rollback();
                //    _ClsInvoiceTransaction.Conn.Close();
                //}
                string ErrorMessage = FN.CreateErrorMessage(ex);
                FN.LogFileWrite(ErrorMessage);
            }
        }

        #endregion              
    }
}