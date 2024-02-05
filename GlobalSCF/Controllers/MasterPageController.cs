using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.DAL;
using TMP.Models;
using CONT = TMP.Infrastructure.Web.StatusManager.OtherDetail.Constant;

namespace TMP.Controllers
{
    public class MasterPageController : Controller
    {
        ClsCustomerMaster cm = new ClsCustomerMaster();
        ClsInvoiceTransaction _inv = new ClsInvoiceTransaction();
        Function FN = new Function();
        public ActionResult _MenuPartialPage()
        {
            return PartialView();
        }
        public ActionResult NoaccessPage()
        {
            return View("NoaccessPage");
        }
        public ActionResult UnderDevelopment()
        {
            Infrastructure.Web.SessionManager.ClearSession();
            ClsCountryMaster _clsCon = new ClsCountryMaster();
            CountryMaster _objModel = new CountryMaster();
            _objModel = _clsCon.SystemPerameter_ListAll().FirstOrDefault();
            ViewBag.ThankYouMsg = _objModel.MaintenanceMsg;
            return View();
        }
        public ActionResult TermsCondistions(string tempName = "")
        {
            string MessageText = string.Empty;
            ClsHTMLTemplate dbHT = new ClsHTMLTemplate();
            var TempText = dbHT.HTMLTemplate_ListAll(0, tempName, 1, "", false, "", 0).FirstOrDefault();
            if (!string.IsNullOrEmpty(TempText.HtmlText))
            {
                ViewBag.TermsConditions = TempText.HtmlText.ToString();
            }
            return PartialView();
        }
        public ActionResult ServiceProviderPartialPage(int CustomerID = 0, int _tab = 0)
        {
            mCustomerMasterModel _mCustomerMasterModel = new mCustomerMasterModel();
            if (CustomerID > 0)
            {
                _mCustomerMasterModel.CustomerID = CustomerID;
                ViewBag.SelectedTab = _tab;
            }
            return PartialView(_mCustomerMasterModel);
        }
        public ActionResult InsuranceProviderPartialPage(int CustomerID = 0, int _tab = 0)
        {
            CustomerMasterModel _objModel = new CustomerMasterModel();
            if (CustomerID > 0)
            {
                _objModel.CustomerID = CustomerID;
                _objModel = cm.CustomerMaster_ListAll(_objModel).FirstOrDefault();
                ViewBag.SelectedTab = _tab;
            }
            return PartialView(_objModel);
        }
        public ActionResult InsuranceProviderPartialPageView(int CustomerID = 0, int _tab = 0, int CustomerTypeID = 0)
        {
            mCustomerMasterModel _mCustomerMasterModel = new mCustomerMasterModel();
            if (CustomerID > 0)
            {
                _mCustomerMasterModel.CustomerID = CustomerID;
                ViewBag.SelectedTab = _tab;
                _mCustomerMasterModel.CustomerTypeID = CustomerTypeID;
            }
            return PartialView(_mCustomerMasterModel);
        }
        public ActionResult _TMPWebsite(int CustomerID = 0, int _tab = 0)
        {
            mCustomerMasterModel _mCustomerMasterModel = new mCustomerMasterModel();
            if (CustomerID > 0)
            {
                _mCustomerMasterModel.CustomerID = CustomerID;
                ViewBag.SelectedTab = _tab;
            }
            return PartialView(_mCustomerMasterModel);
        }
        public ActionResult _LoginFullwidth(int CustomerID = 0, int _tab = 0)
        {
            mCustomerMasterModel _mCustomerMasterModel = new mCustomerMasterModel();
            if (CustomerID > 0)
            {
                _mCustomerMasterModel.CustomerID = CustomerID;
                ViewBag.SelectedTab = _tab;
            }
            return PartialView(_mCustomerMasterModel);
        }
        public ActionResult _dashboardMenu(int CustomerID = 0, int _tab = 0)
        {
            mCustomerMasterModel _mCustomerMasterModel = new mCustomerMasterModel();
            if (CustomerID > 0)
            {
                _mCustomerMasterModel.CustomerID = CustomerID;
                ViewBag.SelectedTab = _tab;
            }
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
                        var ParentMenuName = db.MenuRoleRights_ListAll(RoleId, 0, -1, FN.UserRoletatus()).ToList();
                        ViewBag.ParentMenuName = ParentMenuName;
                        Session["bredCrum"] = "Administration";
                    }
                    else
                    { return RedirectToAction("Administration", "mUserMasters"); }
                }
                else
                { return RedirectToAction("Login", "mUserMasters"); }
                return PartialView(MenuRole);
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
            return PartialView(MenuRole);
        }
        public ActionResult _CustomerMenuPartialPage(int CustomerID = 0, int _tab = 0)
        {
            CustomerMasterModel _objModel = new CustomerMasterModel();

            if (CustomerID > 0)
            {
                _objModel.CustomerID = CustomerID;
                if (_objModel.CustomerID > 0)
                {
                    _objModel = cm.CustomerMasterHistory_ListAllBind(_objModel).FirstOrDefault();
                }
                ViewBag.SelectedTab = _tab;
            }
            return PartialView(_objModel);
        }
        public ActionResult _CustomerDetailMenuPartialPage(int CustomerID = 0, int _tab = 0)
        {
            CustomerMasterModel _objModel = new CustomerMasterModel();

            if (CustomerID > 0)
            {
                _objModel.CustomerID = CustomerID;
                if (_objModel.CustomerID > 0)
                {
                    _objModel = cm.CustomerMasterHistory_ListAllBind(_objModel).FirstOrDefault();
                }
                ViewBag.SelectedTab = _tab;

                string MenuId = Convert.ToString(Session["MenuID"]);
                int ParentMenuID = 0;
                int.TryParse(MenuId, out ParentMenuID);
                var UserRight = FN.CheckUserRight("", "", ParentMenuID);
                if (UserRight != null)
                {
                    ViewBag.Menu = UserRight.MenuName;
                }
            }
            return PartialView(_objModel);
        }
        public ActionResult _funderSummaryMenu(int InvoiceID = 0, int ObligorID = 0, int SupplierID = 0, int _tab = 0,string ProgramType="")
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            if (InvoiceID > 0)
            {
                _objModel.InvoiceID = InvoiceID; _objModel.ProgramType = ProgramType;
                _objModel = _inv.InvoiceMaster_ListAll(_objModel).FirstOrDefault();
                ViewBag.SelectedTab = _tab;
            }
            return PartialView(_objModel);
        }
        public ActionResult _TransactionButton(string ProgramType="")
        {
            InvoiceTransactionModel _objModel = new InvoiceTransactionModel();
            _objModel.ProgramType = ProgramType;
            string _currentUser = _RightsNoaccessPage();
            if (_currentUser != null)
             { _objModel._currentUser = _currentUser; }
            return PartialView(_objModel);
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
            { return null; }
        }
    }
}