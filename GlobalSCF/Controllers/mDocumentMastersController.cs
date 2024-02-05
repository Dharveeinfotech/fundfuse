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
using System.IO;
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;
namespace TMP.Controllers
{
    public class mDocumentMastersController : Controller
    {
        #region Local Variable & Object        
        string indexStatus = CONT.Document_IndexStatus;
        ClsDocumentMaster _clsDoc = new ClsDocumentMaster();
        ClsCustomerMaster ClsCTy = new ClsCustomerMaster();
        Function FN = new Function();
        #endregion
        public ActionResult Index(DocumentMaster_ListAll_Result obj)
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
                        //ViewBag.UserRight = UserRight;
                        ViewBag.UserRight = UserRight.MenuName;
                        var data = _clsDoc.DocumentMaster_ListAll(0, "", -1, -1, indexStatus, false, "");
                        return View(data);
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
                return View(obj);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult Details(int? id, int? idh)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                var Data = _clsDoc.DocumentMasterProcessHistory_ListAllBind(idh, id, CONT.HTMLchecker, 0).FirstOrDefault();
                try
                {
                    if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
                    {
                        ViewBag.ErrorMsg = Request.QueryString["ErrorMsg"].ToString();
                    }
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "", ParentMenuID);

                    if (UserRight != null)
                    {
                        if (UserRight.IsChecker || UserRight.IsApprover)
                        {
                            var list = ClsCTy.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                            List<CustomerMaster_ListAll_Result> objList = new List<CustomerMaster_ListAll_Result>();
                            for (int i = 0; i < list.Count(); i++)
                            {
                                CustomerMaster_ListAll_Result obj1 = (CustomerMaster_ListAll_Result)list[i];
                                objList.Add(obj1);
                            }
                            ViewBag.lstCustomerTypeList = objList;
                            ViewBag.lstCustomerTypeIDList = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, id, CONT.Document_Details, 0).ToList();
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
                return View(Data);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult DetailsApprover(int? id, int? idh)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
                {
                    ViewBag.ErrorMsg = Request.QueryString["ErrorMsg"].ToString();
                }
                var Data = _clsDoc.DocumentMasterProcessHistory_ListAllBind(idh, id, "", 0).FirstOrDefault();
                try
                {
                    var list = ClsCTy.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                    List<CustomerMaster_ListAll_Result> objList = new List<CustomerMaster_ListAll_Result>();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        CustomerMaster_ListAll_Result obj1 = (CustomerMaster_ListAll_Result)list[i];
                        if (obj1.CustomerTypeCode != "OBSUP" && obj1.CustomerTypeCode != "FUEXP" && obj1.CustomerTypeCode != "INVES")
                        { objList.Add(obj1); }
                    }
                    ViewBag.lstCustomerTypeList = objList;
                    ViewBag.lstCustomerTypeIDList = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, id, CONT.HTMLApprover, 0).ToList();
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
                return View(Data);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult CreateNew()
        {
            return RedirectToAction("Create", "mDocumentMasters");
        }
        public ActionResult Create(DocumentMaster_ListAll_Result objlist)
        {
            if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
            {
                ViewBag.ErrorMsg = Request.QueryString["ErrorMsg"].ToString();
            }
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
                        var list = ClsCTy.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                        List<CustomerMaster_ListAll_Result> objList = new List<CustomerMaster_ListAll_Result>();
                        for (int i = 0; i < list.Count(); i++)
                        {
                            CustomerMaster_ListAll_Result obj = (CustomerMaster_ListAll_Result)list[i];
                            objList.Add(obj);
                        }
                        objlist.lstCustomerTypeList = objList;
                        return View(objlist);
                    }
                    else
                    { return RedirectToAction("NoaccessPage", "MasterPage"); }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    throw;
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult IsView()
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                { return View(); }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    throw;
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsView(FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                ObjectContext octx = ((IObjectContextAdapter)_clsDoc).ObjectContext;
                octx.Connection.Open();
                IDbTransaction trans = octx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);

                int DocumentID = Convert.ToInt32(Request.QueryString["DocumentID"]);
                int DocumentProcessHistoryID = Convert.ToInt32(Request.QueryString["DocumentProcessHistoryID"]);
                string ProcessRemark = (frm["ProcessRemark"]);

                try
                {
                    if (frm["Checked"] == "Checked")
                    {
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(DocumentProcessHistoryID, DocumentID, CONT.NC, ProcessRemark, UserId, FN.GetSystemIP());
                        trans.Commit(); octx.Connection.Close();
                        return RedirectToAction("CheckerList");
                    }
                    else if (frm["Checked"] == "Amendment")
                    {
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(DocumentProcessHistoryID, DocumentID, CONT.NM, ProcessRemark, UserId, FN.GetSystemIP());
                        trans.Commit(); octx.Connection.Close();
                        return RedirectToAction("CheckerList");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    trans.Rollback();
                    octx.Connection.Close();
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DocumentMaster_ListAll_Result objmDocumentMaster, FormCollection frm, ControllerContext controllerContext)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                try
                {
                    _clsDoc.conn = ClsAppDatabase.GetCon();
                    if (_clsDoc.conn.State == ConnectionState.Closed)
                        _clsDoc.conn.Open();
                    else { _clsDoc.conn.Close(); _clsDoc.conn.Open(); }
                    _clsDoc.tras = _clsDoc.conn.BeginTransaction();
                    ObjectParameter pDocumentID = new ObjectParameter("pDocumentID", typeof(int));
                    if (objmDocumentMaster.DocumentDesc == null)
                    { objmDocumentMaster.DocumentDesc = ""; }
                    int DocumentID = _clsDoc.DocumentMaster_Add(pDocumentID, objmDocumentMaster.DocumentName, objmDocumentMaster.DocumentDesc, objmDocumentMaster.IsUserDoc,
                        UserId, FN.GetSystemIP(), objmDocumentMaster.IsInvestor);
                    string Ids = Convert.ToString(frm["chkBox[]"]);
                    if (Ids != null)
                    {
                        var ID = Ids.Split(',');
                        foreach (var item in ID)
                        {
                            int CustomerTypeID = Convert.ToInt16(item.ToString());

                            var path = "";
                            HttpFileCollectionBase files = Request.Files;
                            var fileName = "";
                            string gu = Guid.NewGuid().ToString().Substring(0, 12);
                            string FileLocation = "";
                            for (int z = 0; z < files.Count; z++)
                            {
                                string GetAllkeysUploadFile = files.AllKeys[z];

                                if (GetAllkeysUploadFile.Equals("fileCantrol_" + CustomerTypeID + ""))
                                {
                                    HttpPostedFileBase file = files[z];
                                    if (file != null && file.ContentLength > 0)
                                    {
                                        fileName = System.IO.Path.GetFileName(file.FileName);
                                        FileLocation = "1_" + CustomerTypeID + "_" + fileName;
                                        path = Path.Combine(Server.MapPath("~\\Files\\Upload\\"), FileLocation);
                                        file.SaveAs(path);
                                    }
                                    ObjectParameter pCustomerTypeDocDetID = new ObjectParameter("pCustomerTypeDocDetID", typeof(int));
                                    _clsDoc.CustomerTypeDocumentDetails_Add(pCustomerTypeDocDetID, CustomerTypeID, DocumentID, CONT.NR, UserId, FN.GetSystemIP(), FileLocation);
                                }
                            }
                        }
                    }
                    _clsDoc.tras.Commit(); _clsDoc.conn.Close();
                }
                catch (Exception ex)
                {
                    _clsDoc.tras.Rollback(); _clsDoc.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    ViewBag.ErrorMsg = "";
                    string strErrorMsg = "";

                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;

                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Create", "mDocumentMasters", new { ErrorMsg = strErrorMsg });
                }
                return RedirectToAction("Index", "mDocumentMasters");
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult Edit(int? id, string status = "")
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
                {
                    ViewBag.ErrorMsg = Request.QueryString["ErrorMsg"].ToString();
                }
                List<DocumentMaster_ListAll_Result> CustomerTypeDocumentDetails_ListAll_Result = new List<DocumentMaster_ListAll_Result>();
                DocumentMaster_ListAll_Result m = new DocumentMaster_ListAll_Result();
                try
                {
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Maker", ParentMenuID);
                    var list = ClsCTy.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                    List<CustomerMaster_ListAll_Result> objList = new List<CustomerMaster_ListAll_Result>();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        CustomerMaster_ListAll_Result obj = (CustomerMaster_ListAll_Result)list[i];
                        objList.Add(obj);
                    }
                    m.lstCustomerTypeList = objList;
                    if (status == CONT.UJ)
                    {
                        m.lstCustomerTypeIDList = _clsDoc.CustomerTypeDocumentDetails_ListAll(0, 0, id, 1, "", false, "", -1, -1).ToList();
                        m.lstmDocumentMasterList = _clsDoc.DocumentMaster_ListAll(id, "", -1, -1, "", false, "").ToList();
                    }
                    else
                    {
                        m.lstCustomerTypeIDList = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind1(0, 0, id, CONT.Document_Edit, 0).ToList();
                        m.lstmDocumentMasterList = _clsDoc.DocumentMasterProcessHistory_ListAllBind1(0, id, "", 0).ToList();
                    }
                    var data = m.lstCustomerTypeIDList.ToList();
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].Attachment != "")
                        {
                            Session["hdAttchFileName_" + data[i].CustomerTypeID] = data[i].Attachment;
                        }
                    }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DocumentMaster_ListAll_Result Result, FormCollection frm, CustomerTypeDocumentDetails_ListAll_Result obj, ControllerContext controllerContext)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                string FileLocation = "";

                string ProcessRemarks = frm["ProcessRemark"];
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);

                List<DocumentMaster_ListAll_Result> CustomerTypeDocumentDetails_ListAll_Result = new List<DocumentMaster_ListAll_Result>();
                DocumentMaster_ListAll_Result m = new DocumentMaster_ListAll_Result();

                var list = ClsCTy.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                List<CustomerMaster_ListAll_Result> objList = new List<CustomerMaster_ListAll_Result>();

                for (int i = 0; i < list.Count(); i++)
                {
                    CustomerMaster_ListAll_Result obj1 = (CustomerMaster_ListAll_Result)list[i];
                    objList.Add(obj1);
                }
                m.lstCustomerTypeList = objList;
                m.lstCustomerTypeIDList = _clsDoc.CustomerTypeDocumentDetails_ListAll(0, 0, Convert.ToInt32(frm["lstmDocumentMasterList[0].DocumentID"]), -1, "", false, "", -1, -1).ToList();
                m.lstmDocumentMasterList = _clsDoc.DocumentMaster_ListAll(Convert.ToInt32(frm["lstmDocumentMasterList[0].DocumentID"]), "", -1, -1, "", false, "").ToList();

                _clsDoc.conn = ClsAppDatabase.GetCon();
                if (_clsDoc.conn.State == ConnectionState.Closed)
                    _clsDoc.conn.Open();
                else { _clsDoc.conn.Close(); _clsDoc.conn.Open(); }
                _clsDoc.tras = _clsDoc.conn.BeginTransaction();

                DocumentMaster_ListAll_Result ac = Result.lstmDocumentMasterList[0];
                int DocumentID = Convert.ToInt32(ac.DocumentID);
                try
                {
                    if (ac.DocumentDesc == null)
                    { ac.DocumentDesc = ""; }
                    string status = frm["lstmDocumentMasterList[0].Status"];
                    if (status == CONT.RQ || status == CONT.NM)
                    {
                        _clsDoc.DocumentMaster_Update(ac.DocumentID, ac.DocumentName, ac.DocumentDesc, ac.IsUserDoc, UserId, FN.GetSystemIP(), ac.IsInvestor);
                    }
                    else
                    {
                        ObjectParameter pDocumentProcessHistoryID = new ObjectParameter("pDocumentProcessHistoryID", typeof(int));
                        int DocumentProcessHistoryID = _clsDoc.DocumentMasterProcessHistory_Add(pDocumentProcessHistoryID, ac.DocumentID, ac.DocumentName,
                            ac.DocumentDesc, ac.IsUserDoc, m.lstmDocumentMasterList[0].IsActive, CONT.UR, "", UserId, FN.GetSystemIP(), ac.IsInvestor);
                    }

                    string Ids = Convert.ToString(frm["chkBox[]"]);
                    if (Ids != null)
                    {
                        var ID = Ids.Split(',');
                        foreach (var item in ID)
                        {
                            bool Found = false;
                            foreach (var item1 in m.lstCustomerTypeIDList)
                            {
                                if (Convert.ToInt32(item) == item1.CustomerTypeID)
                                {
                                    Found = true;

                                    int CustomerTypeID = Convert.ToInt16(item.ToString());
                                    var path = "";
                                    HttpFileCollectionBase files = Request.Files;
                                    var fileName = "";
                                    string gu = Guid.NewGuid().ToString().Substring(0, 12);


                                    if (Session["hdAttchFileName_" + item1.CustomerTypeID] != null && Convert.ToString(Session["hdAttchFileName_" + item1.CustomerTypeID]) != "")
                                    {
                                        FileLocation = Convert.ToString(Session["hdAttchFileName_" + item1.CustomerTypeID]);
                                    }
                                    for (int z = 0; z < files.Count; z++)
                                    {
                                        string GetAllkeysUploadFile = files.AllKeys[z];

                                        if (GetAllkeysUploadFile.Equals("fileCantrol_" + CustomerTypeID + ""))
                                        {
                                            HttpPostedFileBase file = files[z];
                                            if (file != null && file.ContentLength > 0)
                                            {
                                                fileName = System.IO.Path.GetFileName(file.FileName);
                                                FileLocation = "1_" + CustomerTypeID + "_" + fileName;
                                                path = Path.Combine(Server.MapPath("~\\Files\\Upload\\"), FileLocation);
                                                file.SaveAs(path);
                                            }

                                            if (status == CONT.NM)
                                            {
                                                _clsDoc.CustomerTypeDocumentDetails_Update(item1.CustomerTypeDocDetID, Convert.ToInt32(item), DocumentID, UserId, FN.GetSystemIP(), FileLocation);
                                            }
                                            else
                                            {
                                                ObjectParameter pCustomerTypeDocProcessHistoryID = new ObjectParameter("pCustomerTypeDocProcessHistoryID", typeof(int));
                                                _clsDoc.CustomerTypeDocumentDetailsProcessHistory_Add(pCustomerTypeDocProcessHistoryID, item1.CustomerTypeDocDetID, Convert.ToInt32(item)
                                                    , DocumentID, m.lstmDocumentMasterList[0].IsActive, CONT.UR, ProcessRemarks, UserId, FN.GetSystemIP(), FileLocation);
                                            }
                                        }
                                    }
                                }
                            }
                            if (Found == false)
                            {
                                #region Upload Image
                                int CustomerTypeID = Convert.ToInt16(item.ToString());
                                var path = "";
                                HttpFileCollectionBase files = Request.Files;
                                var fileName = "";
                                string gu = Guid.NewGuid().ToString().Substring(0, 12);


                                if (Session["hdAttchFileName_" + Convert.ToInt32(item)] != null && Convert.ToString(Session["hdAttchFileName_" + Convert.ToInt32(item)]) != "")
                                {
                                    FileLocation = Convert.ToString(Session["hdAttchFileName_" + Convert.ToInt32(item)]);
                                }
                                for (int z = 0; z < files.Count; z++)
                                {
                                    string GetAllkeysUploadFile = files.AllKeys[z];

                                    if (GetAllkeysUploadFile.Equals("fileCantrol_" + CustomerTypeID + ""))
                                    {
                                        HttpPostedFileBase file = files[z];
                                        if (file != null && file.ContentLength > 0)
                                        {
                                            fileName = System.IO.Path.GetFileName(file.FileName);
                                            FileLocation = "1_" + CustomerTypeID + "_" + fileName;
                                            path = Path.Combine(Server.MapPath("~\\Files\\Upload\\"), FileLocation);
                                            file.SaveAs(path);
                                        }

                                        if (status == CONT.NM)
                                        {
                                            ObjectParameter pCustomerTypeDocDetID = new ObjectParameter("pCustomerTypeDocDetID", typeof(int));
                                            _clsDoc.CustomerTypeDocumentDetails_Add(pCustomerTypeDocDetID, Convert.ToInt32(item), DocumentID, CONT.NR, UserId, FN.GetSystemIP(), FileLocation);
                                        }
                                        else
                                        {
                                            ObjectParameter pCustomerTypeDocDetID = new ObjectParameter("pCustomerTypeDocDetID", typeof(int));
                                            _clsDoc.CustomerTypeDocumentDetails_Add(pCustomerTypeDocDetID, Convert.ToInt32(item), DocumentID, CONT.UR, UserId, FN.GetSystemIP(), FileLocation);
                                        }
                                    }
                                }
                                #endregion                                
                            }
                        }
                    }

                    string IdsForInactive = Convert.ToString(frm["chkBoxunselected[]"]);
                    if (IdsForInactive != null)
                    {
                        var IDInactive = IdsForInactive.Split(',');
                        foreach (var item in IDInactive)
                        {
                            int CutomerTypeDocDetID = Convert.ToInt32(item.Split('_')[0]);
                            int CustomerTypeID = Convert.ToInt32(item.Split('_')[1]);
                            if (status == CONT.NM)
                            {
                                _clsDoc.CustomerTypeDocumentDetails_Delete(CutomerTypeDocDetID, UserId, FN.GetSystemIP());
                            }
                            else
                            {
                                ObjectParameter pCustomerTypeDocProcessHistoryID = new ObjectParameter("pCustomerTypeDocProcessHistoryID", typeof(int));
                                _clsDoc.CustomerTypeDocumentDetailsProcessHistory_Add(pCustomerTypeDocProcessHistoryID, CutomerTypeDocDetID, CustomerTypeID
                                    , DocumentID, false, CONT.UR, ProcessRemarks, UserId, FN.GetSystemIP(), FileLocation);
                            }
                        }
                    }
                    _clsDoc.tras.Commit(); _clsDoc.conn.Close();
                }
                catch (Exception ex)
                {
                    _clsDoc.tras.Rollback(); _clsDoc.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    ViewBag.ErrorMsg = "";
                    string strErrorMsg = "";
                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;
                    ViewBag.ErrorMsg = strErrorMsg;
                    return RedirectToAction("Edit", "mDocumentMasters", new { id = DocumentID, ErrorMsg = strErrorMsg });
                }
                return RedirectToAction("Index", "mDocumentMasters");
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult Delete(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    if (id == null)
                    { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
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
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                { return RedirectToAction("Index"); }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    throw;
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
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
                            return View(_clsDoc.DocumentMasterProcessHistory_ListAllBind(0, 0, CONT.Document_Checker, 0));
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
            { return RedirectToAction("Login", "mUserMasters"); }
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
                            return View(_clsDoc.DocumentMasterProcessHistory_ListAllBind(0, 0, CONT.HTMLApprover, 0));
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
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult IsViewApprover(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                DocumentMaster_ListAll_Result Data = _clsDoc.DocumentMasterProcessHistory_ListAllBind(id, 0, "", 0).FirstOrDefault();
                try
                {
                    if (id == 0)
                    { return HttpNotFound(); }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View(Data);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsAmend(DocumentMaster_ListAll_Result m, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                _clsDoc.conn = ClsAppDatabase.GetCon();
                if (_clsDoc.conn.State == ConnectionState.Closed)
                    _clsDoc.conn.Open();
                else { _clsDoc.conn.Close(); _clsDoc.conn.Open(); }
                _clsDoc.tras = _clsDoc.conn.BeginTransaction();
                int HtmlTemplateProcessHistoryID = Convert.ToInt32(frm["DocumentProcessHistoryID"]);
                int HtmlTemplateID = Convert.ToInt32(frm["DocumentID"]);
                string userRight = string.Empty;
                try
                {
                    string Staus = frm["Status"];
                    string ProcessRemark = m.ProcessRemark;
                    string ProcessStatus = string.Empty;
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
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(HtmlTemplateProcessHistoryID, HtmlTemplateID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        var abc = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, HtmlTemplateID, CONT.HTMLchecker, 0).ToList();

                        for (int i = 0; i < abc.Count; i++)
                        {
                            int CustomerTypeDocProcessHistoryID = abc[i].CustomerTypeDocProcessHistoryID;
                            int CustomerTypeDocDetID = abc[i].CustomerTypeDocDetID;
                            _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ProcessStatus(CustomerTypeDocProcessHistoryID, CustomerTypeDocDetID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        }
                        _clsDoc.tras.Commit(); _clsDoc.conn.Close();
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
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(HtmlTemplateProcessHistoryID, HtmlTemplateID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        var abc = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, HtmlTemplateID, CONT.HTMLApprover, 0).ToList();
                        for (int i = 0; i < abc.Count; i++)
                        {
                            int CustomerTypeDocProcessHistoryID = abc[i].CustomerTypeDocProcessHistoryID;
                            int CustomerTypeDocDetID = abc[i].CustomerTypeDocDetID;
                            _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ProcessStatus(CustomerTypeDocProcessHistoryID, CustomerTypeDocDetID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        }
                        _clsDoc.tras.Commit(); _clsDoc.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    _clsDoc.tras.Rollback(); _clsDoc.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    ViewBag.ErrorMsg = "";
                    string strErrorMsg = "";

                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;

                    ViewBag.ErrorMsg = strErrorMsg;

                    if (userRight == "ApproverReject")
                    { return RedirectToAction("DetailsApprover", "mDocumentMasters", new { id = HtmlTemplateID, idh = HtmlTemplateProcessHistoryID, ErrorMsg = strErrorMsg }); }

                    if (userRight == "CheckerAmend")
                    { return RedirectToAction("Details", "mDocumentMasters", new { id = HtmlTemplateID, idh = HtmlTemplateProcessHistoryID, ErrorMsg = strErrorMsg }); }
                }
                return View("Details", m);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detailsapprove(DocumentMaster_ListAll_Result m, FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                _clsDoc.conn = ClsAppDatabase.GetCon();
                if (_clsDoc.conn.State == ConnectionState.Closed)
                    _clsDoc.conn.Open();
                else { _clsDoc.conn.Close(); _clsDoc.conn.Open(); }
                _clsDoc.tras = _clsDoc.conn.BeginTransaction();
                int DocumentProcessHistoryID = Convert.ToInt32(frm["DocumentProcessHistoryID"]);
                int DocumentID = Convert.ToInt32(frm["DocumentID"]);
                string userRight = string.Empty;
                try
                {
                    string Staus = frm["Status"];
                    string ProcessRemark = m.ProcessRemark;
                    string ProcessStatus = string.Empty;
                    //CheckerApprove code
                    if (Staus == CONT.NR) { ProcessStatus = CONT.NC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.UR) { ProcessStatus = CONT.UC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.DR) { ProcessStatus = CONT.DC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.AR) { ProcessStatus = CONT.AC; userRight = "CheckerApprove"; }
                    else if (Staus == CONT.IR) { ProcessStatus = CONT.IC; userRight = "CheckerApprove"; }
                    if (userRight == "CheckerApprove")
                    {
                        if (ProcessRemark == null)
                        { ProcessRemark = ""; }
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(DocumentProcessHistoryID, DocumentID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        var abc = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, DocumentID, CONT.Document_Checker, 0).ToList();
                        for (int i = 0; i < abc.Count; i++)
                        {
                            int CustomerTypeDocProcessHistoryID = abc[i].CustomerTypeDocProcessHistoryID;
                            int CustomerTypeDocDetID = abc[i].CustomerTypeDocDetID;
                            _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ProcessStatus(CustomerTypeDocProcessHistoryID, CustomerTypeDocDetID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        }
                        _clsDoc.tras.Commit(); _clsDoc.conn.Close();
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
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(DocumentProcessHistoryID, DocumentID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        var abc = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, DocumentID, CONT.HTMLApprover, 0).ToList();

                        for (int i = 0; i < abc.Count; i++)
                        {
                            int CustomerTypeDocProcessHistoryID = abc[i].CustomerTypeDocProcessHistoryID;
                            int CustomerTypeDocDetID = abc[i].CustomerTypeDocDetID;
                            _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ProcessStatus(CustomerTypeDocProcessHistoryID, CustomerTypeDocDetID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        }
                        _clsDoc.tras.Commit(); _clsDoc.conn.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    _clsDoc.tras.Rollback(); _clsDoc.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    ViewBag.ErrorMsg = "";
                    string strErrorMsg = "";

                    if (ex.InnerException == null)
                        strErrorMsg = ex.Message;
                    else
                        strErrorMsg = ex.InnerException.Message;

                    ViewBag.ErrorMsg = strErrorMsg;

                    if (userRight == "ApproverApprove")
                    { return RedirectToAction("DetailsApprover", "mDocumentMasters", new { id = DocumentID, idh = DocumentProcessHistoryID, ErrorMsg = strErrorMsg }); }

                    if (userRight == "CheckerApprove")
                    { return RedirectToAction("Details", "mDocumentMasters", new { id = DocumentID, idh = DocumentProcessHistoryID, ErrorMsg = strErrorMsg }); }
                }
                return View("DetailsApprover", m);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsViewApprover(FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                ObjectContext octx = ((IObjectContextAdapter)_clsDoc).ObjectContext;
                octx.Connection.Open();
                IDbTransaction trans = octx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                try
                {
                    string Staus = frm["Status"]; string ProcessRemark = frm["ProcessRemark"]; string ProcessStatus = string.Empty;
                    int DocumentProcessHistoryID = Convert.ToInt32(frm["DocumentProcessHistoryID"]);
                    int DocumentID = Convert.ToInt32(frm["DocumentID"]);
                    if (frm["Approved"] == "Approved")
                    {
                        if (Staus == CONT.NC)
                        { ProcessStatus = CONT.NA; }
                        else if (Staus == CONT.UC)
                        { ProcessStatus = CONT.UA; }
                        else if (Staus == CONT.DC)
                        { ProcessStatus = CONT.DA; }
                        else if (Staus == CONT.AC)
                        { ProcessStatus = CONT.AA; }
                        else if (Staus == CONT.IC)
                        { ProcessStatus = CONT.IA; }
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(DocumentProcessHistoryID, DocumentID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        trans.Commit(); octx.Connection.Close();
                        return RedirectToAction("ApproverList");
                    }
                    else if (frm["Approved"] == "Reject")
                    {
                        if (Staus == CONT.NC)
                        { ProcessStatus = CONT.NJ; }
                        else if (Staus == CONT.UC)
                        { ProcessStatus = CONT.UJ; }
                        else if (Staus == CONT.DC)
                        { ProcessStatus = CONT.DJ; }
                        else if (Staus == CONT.AC)
                        { ProcessStatus = CONT.AJ; }
                        else if (Staus == CONT.IC)
                        { ProcessStatus = CONT.IJ; }
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(DocumentProcessHistoryID, DocumentID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        trans.Commit(); octx.Connection.Close();
                        return RedirectToAction("ApproverList");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    trans.Rollback(); octx.Connection.Close();
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult IsViewChecker(int? id)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                DocumentMaster_ListAll_Result Data = _clsDoc.DocumentMasterProcessHistory_ListAllBind(id, 0, "", 0).FirstOrDefault();
                try
                {
                    if (id == 0)
                    { return HttpNotFound(); }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    ViewBag.ErrorMesssage = ex.InnerException.Message;
                }
                return View(Data);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        [HttpPost]
        public ActionResult IsViewChecker(FormCollection frm)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                ObjectContext octx = ((IObjectContextAdapter)_clsDoc).ObjectContext;
                octx.Connection.Open();
                IDbTransaction trans = octx.Connection.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                try
                {
                    string Status = frm["Status"];
                    string ProcessRemark = frm["ProcessRemark"];
                    string ProcessStatus = string.Empty;
                    int DocumentProcessHistoryID = Convert.ToInt32(frm["DocumentProcessHistoryID"]);
                    int DocumentID = Convert.ToInt32(frm["DocumentID"]);
                    if (frm["Checked"] == "Checked")
                    {
                        if (Status == CONT.NR)
                            ProcessStatus = CONT.NC;
                        else if (Status == CONT.UR)
                            ProcessStatus = CONT.UC;
                        else if (Status == CONT.DR)
                            ProcessStatus = CONT.DC;
                        else if (Status == CONT.AR)
                            ProcessStatus = CONT.AC;
                        else if (Status == CONT.IR)
                            ProcessStatus = CONT.IC;

                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(DocumentProcessHistoryID, DocumentID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        trans.Commit(); octx.Connection.Close();
                        return RedirectToAction("CheckerList");
                    }
                    else if (frm["Checked"] == "Amendment")
                    {
                        if (Status == CONT.NR)
                            ProcessStatus = CONT.NM;
                        else if (Status == CONT.UR)
                            ProcessStatus = CONT.UM;
                        else if (Status == CONT.DR)
                            ProcessStatus = CONT.DM;
                        else if (Status == CONT.AR)
                            ProcessStatus = CONT.AM;
                        else if (Status == CONT.IR)
                            ProcessStatus = CONT.IM;
                        _clsDoc.DocumentMasterProcessHistory_ProcessStatus(DocumentProcessHistoryID, DocumentID, ProcessStatus, ProcessRemark, UserId, FN.GetSystemIP());
                        trans.Commit();
                        octx.Connection.Close();
                        return RedirectToAction("CheckerList");
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    trans.Rollback();
                    octx.Connection.Close();
                    ModelState.AddModelError("", ex.InnerException.Message);
                }
                return View();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult AddDocumentType(string sortOrder, string CurrentSort, int? page)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    ViewBag.CurrentSort = sortOrder;
                    sortOrder = String.IsNullOrEmpty(sortOrder) ? "Emp_ID" : sortOrder;
                    return View(ClsCTy.CustomerTypeMaster_ListAll(0, "", "", -1, "", false, ""));
                }
                catch (Exception ex)
                {
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    throw;
                }
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public PartialViewResult AIDPopup(string ActionName, string DocumentName)
        {
            ViewData["ActionName"] = ""; ViewData["ActionName"] = ActionName;
            ViewData["DocumentName"] = DocumentName;
            return PartialView();
        }
        public string Act_InA_Del(string ActionName, int DocumentID, string ProcessRemarks)
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
                    var mDocumentMaster = _clsDoc.DocumentMaster_ListAll(DocumentID, "", -1, -1, "", false, "").FirstOrDefault();
                    ObjectParameter DocumentMasterHistoryID = new ObjectParameter("pDocumentProcessHistoryID", typeof(int));
                    if (mDocumentMaster != null)
                        Status = mDocumentMaster.Status;

                    int mDocumentID = 0;

                    _clsDoc.conn = ClsAppDatabase.GetCon();
                    if (_clsDoc.conn.State == ConnectionState.Closed)
                        _clsDoc.conn.Open();
                    else { _clsDoc.conn.Close(); _clsDoc.conn.Open(); }
                    _clsDoc.tras = _clsDoc.conn.BeginTransaction();

                    if (ActionName.Equals("Delete"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA || Status == CONT.IA)
                        {
                            mDocumentID = _clsDoc.DocumentMasterProcessHistory_Add(DocumentMasterHistoryID, mDocumentMaster.DocumentID, mDocumentMaster.DocumentName,
                                mDocumentMaster.DocumentDesc, mDocumentMaster.IsUserDoc, mDocumentMaster.IsActive, CONT.DR, ProcessRemarks,
                                UpdatedBy, FN.GetSystemIP(), mDocumentMaster.IsInvestor);
                            var getCustomerTypeDocumentDetailsID = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, mDocumentMaster.DocumentID, CONT.Document_Delete, 0).ToList();
                            for (int i = 0; i < getCustomerTypeDocumentDetailsID.Count; i++)
                            {
                                ObjectParameter OutCustomerTypeDocProcessHistoryID = new ObjectParameter("pCustomerTypeDocProcessHistoryID", typeof(int));
                                _clsDoc.CustomerTypeDocumentDetailsProcessHistory_Add(OutCustomerTypeDocProcessHistoryID, getCustomerTypeDocumentDetailsID[i].CustomerTypeDocDetID
                                    , getCustomerTypeDocumentDetailsID[i].CustomerTypeID, getCustomerTypeDocumentDetailsID[i].DocumentID, getCustomerTypeDocumentDetailsID[i].IsActive
                                    , CONT.DR, ProcessRemarks, UpdatedBy, FN.GetSystemIP(), getCustomerTypeDocumentDetailsID[i].Attachment);
                            }
                        }
                    }
                    else if (ActionName.Equals("Active"))
                    {

                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.IA)
                        {
                            mDocumentID = _clsDoc.DocumentMasterProcessHistory_Add(DocumentMasterHistoryID, mDocumentMaster.DocumentID, mDocumentMaster.DocumentName,
                                mDocumentMaster.DocumentDesc, true, true, CONT.AR, ProcessRemarks, UpdatedBy, FN.GetSystemIP(), mDocumentMaster.IsInvestor);

                            var getCustomerTypeDocumentDetailsID = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, mDocumentMaster.DocumentID, CONT.Document_Active, 0).ToList();
                            for (int i = 0; i < getCustomerTypeDocumentDetailsID.Count; i++)
                            {
                                ObjectParameter OutCustomerTypeDocProcessHistoryID = new ObjectParameter("pCustomerTypeDocProcessHistoryID", typeof(int));
                                _clsDoc.CustomerTypeDocumentDetailsProcessHistory_Add(OutCustomerTypeDocProcessHistoryID, getCustomerTypeDocumentDetailsID[i].CustomerTypeDocDetID
                                    , getCustomerTypeDocumentDetailsID[i].CustomerTypeID, getCustomerTypeDocumentDetailsID[i].DocumentID, true
                                    , CONT.AR, ProcessRemarks, UpdatedBy, FN.GetSystemIP(), getCustomerTypeDocumentDetailsID[i].Attachment);
                            }
                        }
                    }
                    else if (ActionName.Equals("InActive"))
                    {
                        if (Status == CONT.NA || Status == CONT.UA || Status == CONT.AA)
                        {
                            mDocumentID = _clsDoc.DocumentMasterProcessHistory_Add(DocumentMasterHistoryID, mDocumentMaster.DocumentID, mDocumentMaster.DocumentName,
                                mDocumentMaster.DocumentDesc, false, false, CONT.IR, ProcessRemarks, UpdatedBy, FN.GetSystemIP(), mDocumentMaster.IsInvestor);
                            var getCustomerTypeDocumentDetailsID = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, mDocumentMaster.DocumentID, CONT.Document_InActive, 0).ToList();
                            for (int i = 0; i < getCustomerTypeDocumentDetailsID.Count; i++)
                            {
                                ObjectParameter OutCustomerTypeDocProcessHistoryID = new ObjectParameter("pCustomerTypeDocProcessHistoryID", typeof(int));
                                _clsDoc.CustomerTypeDocumentDetailsProcessHistory_Add(OutCustomerTypeDocProcessHistoryID, getCustomerTypeDocumentDetailsID[i].CustomerTypeDocDetID
                                    , getCustomerTypeDocumentDetailsID[i].CustomerTypeID, getCustomerTypeDocumentDetailsID[i].DocumentID, false
                                    , CONT.IR, ProcessRemarks, UpdatedBy, FN.GetSystemIP(), getCustomerTypeDocumentDetailsID[i].Attachment);
                            }
                        }
                    }
                    resp = "1$SuccessFully";
                    _clsDoc.tras.Commit(); _clsDoc.conn.Close();
                }
                catch (Exception ex)
                {
                    _clsDoc.tras.Rollback(); _clsDoc.conn.Close();
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    resp = "0$" + ex.Message;
                }
            }
            else
            { resp = "2$Please Login."; }
            return resp;
        }
        public ActionResult DocumentMasterViewerIndex()
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
                        { return View(_clsDoc.DocumentMasterProcessHistory_ListAllBind(0, 0, "", 0)); }
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
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public ActionResult DocumentMasterViewer(int? id, int? idh)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                var Data = _clsDoc.DocumentMasterProcessHistory_ListAllBind(idh, id, "", 0).FirstOrDefault();
                try
                {
                    if (Request.QueryString["ErrorMsg"] != null && Request.QueryString["ErrorMsg"] != "")
                    {
                        ViewBag.ErrorMsg = Request.QueryString["ErrorMsg"].ToString();
                    }
                    string MenuId = Convert.ToString(Session["MenuID"]);
                    int ParentMenuID = 0;
                    int.TryParse(MenuId, out ParentMenuID);
                    var UserRight = FN.CheckUserRight("", "Viewer", ParentMenuID);

                    if (UserRight != null)
                    {
                        if (UserRight.IsView)
                        {
                            var list = ClsCTy.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                            List<CustomerMaster_ListAll_Result> objList = new List<CustomerMaster_ListAll_Result>();
                            for (int i = 0; i < list.Count(); i++)
                            {
                                CustomerMaster_ListAll_Result obj1 = (CustomerMaster_ListAll_Result)list[i];
                                objList.Add(obj1);
                            }
                            ViewBag.lstCustomerTypeList = objList;
                            ViewBag.lstCustomerTypeIDList = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, id, "", 0).ToList();
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
                return View(Data);
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
        public PartialViewResult CommentResult(int DocumentID)
        {
            ClsDocumentMaster dbP = new ClsDocumentMaster();
            ViewBag.lstCustomerMaster = dbP.DocumentMaster_ListAllRemarks(DocumentID).ToList();
            return PartialView();
        }
        public ActionResult View(int? DocID)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                try
                {
                    var Data = _clsDoc.DocumentMasterProcessHistory_ListAllBind(0, DocID, "", 0).FirstOrDefault();

                    var list = ClsCTy.CustomerTypeMaster_ListAll(0, "", "", 1, "", false, "").ToList();
                    List<CustomerMaster_ListAll_Result> objList = new List<CustomerMaster_ListAll_Result>();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        CustomerMaster_ListAll_Result obj = (CustomerMaster_ListAll_Result)list[i];
                        objList.Add(obj);
                    }

                    ViewBag.lstCustomerTypeList = objList;
                    ViewBag.lstCustomerTypeIDList = _clsDoc.CustomerTypeDocumentDetailsProcessHistory_ListAllBind(0, 0, DocID, CONT.Document_Details, 0).ToList();
                    return PartialView(Data);
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
                return PartialView();
            }
            else
            { return RedirectToAction("Login", "mUserMasters"); }
        }
    }
}
