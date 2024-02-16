using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TMP.Controllers;
using TMP.DAL;
using TMP.Models;

namespace TMP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {   
            string IsMaintenance = Convert.ToString(WebConfigurationManager.AppSettings["IsMaintenance"]);
            if (IsMaintenance.ToLower().ToString() == "true")
            {
                ClsCountryMaster _clsCon = new ClsCountryMaster();
                CountryMaster _objModel = new CountryMaster();
                _objModel = _clsCon.SystemPerameter_ListAll().FirstOrDefault();
                if (_objModel != null)
                {
                    if (_objModel.IsMaintenance == true)
                    {
                        HttpContext.Current.RewritePath("~/MasterPage/UnderDevelopment");
                    }
                }
            }
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["CurrentSessionID"] = HttpContext.Current.Session.SessionID;
        }
    }
}
