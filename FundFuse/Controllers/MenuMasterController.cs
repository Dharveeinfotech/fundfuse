using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.DAL;
using TMP.Models;
namespace TMP.Controllers
{
    public class MenuMasterController : Controller
    {
        #region Local Variable
        ClsMenuMaster _ClsMenuMaster = new ClsMenuMaster();
        MenuMaster _MenuMaster = new MenuMaster();
        Function FN = new Function();
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuLayer()
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                _MenuMaster.MenuID = 0; _MenuMaster.ParentMenuID = -1; _MenuMaster.IsActive = true;
                var Data = _ClsMenuMaster.MenuMaster_ListAllMultiLayer(_MenuMaster);
                ViewBag.listMenuName = Data;
                return View(Data);
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
        [HttpPost]
        public ActionResult MenuLayer(FormCollection frm, MenuMaster MenuMaster)
        {
            string[] LoginStatus = FN.Checkcredentials();
            if (!string.IsNullOrEmpty(LoginStatus[0]) && LoginStatus[0] == "pass")
            {
                int UserId = 0;
                int.TryParse(LoginStatus[1].ToString(), out UserId);
                try
                {
                    _ClsMenuMaster.Conn = ClsAppDatabase.GetCon();
                    if (_ClsMenuMaster.Conn.State == System.Data.ConnectionState.Closed)
                    { _ClsMenuMaster.Conn.Open(); }
                    else { _ClsMenuMaster.Conn.Close(); _ClsMenuMaster.Conn.Open(); }
                    _ClsMenuMaster.Tras = _ClsMenuMaster.Conn.BeginTransaction();

                    _MenuMaster.MenuID = 0; _MenuMaster.ParentMenuID = -1; _MenuMaster.IsActive = true;
                    var Data = _ClsMenuMaster.MenuMaster_ListAllMultiLayer(_MenuMaster);
                    for (int n = 0; n < Data.Count; n++)
                    {
                        if (Data[n].Level != 0)
                        {
                            _MenuMaster.MenuID = Data[n].MenuID; _MenuMaster.IsEnable = false;
                            _ClsMenuMaster.MenuMaster_UpdateEnable(_MenuMaster);
                        }
                    }

                    for (int i = 0; i < Data.Count; i++)
                    {
                        int MenuID = Data[i].MenuID;
                        for (int j = 0; j < frm.Count; j++)
                        {
                            string GetAllkeysStringValue = frm.AllKeys[j];
                            if (GetAllkeysStringValue.Equals("checkbox_C_" + MenuID + ""))
                            {
                                _MenuMaster.MenuID = MenuID; _MenuMaster.IsEnable = true;
                                _ClsMenuMaster.MenuMaster_UpdateEnable(_MenuMaster);
                            }
                        }
                    }
                    _ClsMenuMaster.Tras.Commit();
                    _ClsMenuMaster.Conn.Close();
                    return RedirectToAction("MenuLayer", "MenuMaster");
                }
                catch (Exception ex)
                {
                    if (_ClsMenuMaster.Conn.State == System.Data.ConnectionState.Open)
                    {
                        _ClsMenuMaster.Tras.Rollback();
                        _ClsMenuMaster.Conn.Close();
                    }
                    string errorMessage = FN.CreateErrorMessage(ex);
                    FN.LogFileWrite(errorMessage);
                    if (ex.InnerException != null)
                    { ViewBag.ErrorMsg = ex.InnerException.Message; }
                    else
                    { ViewBag.ErrorMsg = ex.Message; }
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "mUserMasters");
            }
        }
    }
}