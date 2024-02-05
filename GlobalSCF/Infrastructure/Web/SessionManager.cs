using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TMP.Infrastructure.Web
{
    public class SessionManager
    {
        private const string m_SessionName_MenuID = "MenuID";
        private const string m_SessionName_InvoiceID = "InvoiceID";
        private const string m_SessionName_AuctionProgConfigType = "AuctionProgConfigType";
        private const string m_SessionName_LoggedInUser = "LoggedInUser";
        private const string m_SessionName_RoleID = "RoleId";
        private const string m_SessionName_CustomerID = "CustomerTypeID";
        private const string m_SessionName_SupperAdmin = "SuperAdmin";

        public static LoggedInUser CurrentLoggedInUser
        {
            get
            {
                if (HttpContext.Current.Session[m_SessionName_LoggedInUser] != null)
                {
                    return (HttpContext.Current.Session[m_SessionName_LoggedInUser] as LoggedInUser);
                }
                return null;
            }
        }

        public static int SelectedMenuID
        {
            get
            {
                if (HttpContext.Current.Session[m_SessionName_MenuID] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session[m_SessionName_MenuID]);
                }
                return 0;
            }
            set
            {
                HttpContext.Current.Session[m_SessionName_MenuID] = value;
            }
        }

        public static int CurrentInvoiceID
        {
            get
            {
                if (HttpContext.Current.Session[m_SessionName_InvoiceID] != null)
                {
                    return Convert.ToInt32(HttpContext.Current.Session[m_SessionName_InvoiceID]);
                }
                return 0;
            }
            set
            {
                HttpContext.Current.Session[m_SessionName_InvoiceID] = value;
            }
        }

        public static string CurrentAuctionProgConfigType
        {
            get
            {
                if (HttpContext.Current.Session[m_SessionName_AuctionProgConfigType] != null)
                {
                    return Convert.ToString(HttpContext.Current.Session[m_SessionName_AuctionProgConfigType]);
                }
                return "F";
            }
            set
            {
                HttpContext.Current.Session[m_SessionName_AuctionProgConfigType] = value;
            }
        }

        public static bool SetLoggedInUserSession(string loginName, DataTable dt)
        {
            bool _success = false;
            try
            {
                LoggedInUser _user = new LoggedInUser();

                _user.UserName = loginName;
                _user.UserID = Convert.ToInt16(dt.Rows[0]["UserID"].ToString());
                _user.CustomerID = Convert.ToInt16(dt.Rows[0]["CustomerID"].ToString());
                _user.DisplayName = dt.Rows[0]["FirstName"].ToString();
                _user.ReminderCustomerID = Convert.ToInt16(dt.Rows[0]["CustomerID"].ToString());

                if (HttpContext.Current.Session[m_SessionName_RoleID] != null)
                {
                    _user.RoleID = HttpContext.Current.Session[m_SessionName_RoleID].ToString();
                }
                if (HttpContext.Current.Session[m_SessionName_CustomerID] != null)
                {
                    _user.CustomerTypeID = HttpContext.Current.Session[m_SessionName_CustomerID].ToString();
                }
                if (HttpContext.Current.Session[m_SessionName_SupperAdmin] != null)
                {
                    _user.IsSuperAdmin = (HttpContext.Current.Session[m_SessionName_SupperAdmin].ToString() == "1");
                }
                
                HttpContext.Current.Session[m_SessionName_LoggedInUser] = _user;
                                
            }
            catch
            {
                _success = false;
            }
            return _success;
        }

        public static void ClearSession()
        {
            HttpContext _httpContext = HttpContext.Current;
            if (_httpContext != null)
            {
                _httpContext.Session.Clear();
                _httpContext.Session.Abandon();
                _httpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                _httpContext.Response.Cache.SetExpires(DateTime.UtcNow);
                _httpContext.Response.Cache.SetNoStore();
            }
        }
    }

    public class LoggedInUser
    {
        public int UserID { get; set; }
        public int CustomerID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int ReminderCustomerID { get; set; }
        public string RoleID { get; set; }
        public string CustomerTypeID { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}