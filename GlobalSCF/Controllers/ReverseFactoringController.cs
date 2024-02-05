using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;
using TMP.DAL;
using System.Data;

namespace TMP.Controllers
{
    public class ReverseFactoringController : Controller
    {
        #region MyRegion
        Function FN = new Function();
        Models.InvoiceTransactionModel _ObjModel = new InvoiceTransactionModel();
        ClsInvoiceTransaction _ClsInvoiceTransaction = new ClsInvoiceTransaction();
        #endregion
        public ActionResult Index()
        {
            Session["InvoiceID"] = null;
            _ObjModel.ProgramType = "R";
            _ObjModel.Status = "";
            _ObjModel.CustomerID = Function.LoggedInUserID;
            var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel);
            return View(Data);
        }

        #region Profit And Processing Fees
        public ActionResult ProfitAndFees(int InvoiceID = 0, string IndexStatus = "")
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ObjModel.InvoiceID = InvoiceID; _ObjModel.ProgramType = "F"; _ObjModel.Status = IndexStatus;
                    var Data = _ClsInvoiceTransaction.InvoiceMaster_ListAll(_ObjModel).FirstOrDefault();
                    _ObjModel.TranRefNo = Data.TranRefNo; _ObjModel.CurrencyCode = Data.CurrencyCode;

                    _ObjModel.ProfitAmt = 1000; _ObjModel.ProcessFeeAmt = 2000;
                    _ObjModel.IndexStatus = IndexStatus;
                    FillCurrencyCombo(); FillBankCombo();
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
        public ActionResult ProfitAndFees(InvoiceTransactionModel _Model, FormCollection frm)
        {
            try
            {
                string[] LoginStatus = FN.Checkcredentials();
                if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
                {
                    _ClsInvoiceTransaction.Conn = ClsAppDatabase.GetCon();
                    if (_ClsInvoiceTransaction.Conn.State == ConnectionState.Closed) _ClsInvoiceTransaction.Conn.Open();
                    else { _ClsInvoiceTransaction.Conn.Close(); _ClsInvoiceTransaction.Conn.Open(); }
                    _ClsInvoiceTransaction.Tras = _ClsInvoiceTransaction.Conn.BeginTransaction();

                    if (Request["btnSubmit"] != null)
                    {
                        _ClsInvoiceTransaction.InvoiceMaster_UpdatePaymentFees(_Model);
                        _ClsInvoiceTransaction.Tras.Commit(); _ClsInvoiceTransaction.Conn.Close();
                        return RedirectToAction("SettlementIndex", "InvoiceCommon", new { ProgramType = _ObjModel.ProgramType, IndexStatus = _Model.IndexStatus });
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
                FillCurrencyCombo(); FillBankCombo();
                return View(_Model);
            }
            FillCurrencyCombo(); FillBankCombo();
            return View(_Model);
        }
        #endregion

        #region Common Function
        public void FillCurrencyCombo()
        {
            ClsCurrency _ClsCurrency = new ClsCurrency();
            var DDLCurrencyData = _ClsCurrency.CurrencyMaster_ListAll(0, "", "", 1, "", false, "").OrderBy(x => x.CurrencyCode).ToList();
            if (DDLCurrencyData != null && DDLCurrencyData.Count > 0)
                ViewBag.DDLCurrencyID = DDLCurrencyData;
            else
                ViewBag.DDLCurrencyID = new List<SelectListItem> { };
        }
        public void FillBankCombo()
        {
            ClsBankMaster _clsBank = new ClsBankMaster();
            var DDLBankData = _clsBank.BankMaster_ListAll(0, "", 0, 0, 0, 1, "", false, "").OrderBy(x => x.BankName).ToList();
            if (DDLBankData != null && DDLBankData.Count > 0)
                ViewBag.DDLBankID = DDLBankData;
            else
                ViewBag.DDLBankID = new List<SelectListItem> { };
        }
        #endregion
    }
}