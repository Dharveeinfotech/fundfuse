using TMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TMP.Infrastructure.Core
{
    public enum UserRightsType
    {
        Maker = 1,
        Checker = 2,
        MakerOrChecker = 4,
        MakerAndChecker = 8,
        Viewer = 16
    }

    public class UserRightsActionFilterAttribute : ActionFilterAttribute
    {

        private const string RouteDataParamName_IsMaker = "IsMaker";
        private const string RouteDataParamName_IsChecker = "IsChecker";
        private const string RouteDataParamName_UserRights = "UserRights";

        public UserRightsActionFilterAttribute()
        {
            RedirectToNoAccess = true;
            UserRightsRequired = UserRightsType.MakerOrChecker;
        }

        public UserRightsType UserRightsRequired { get;set;}
        public bool RedirectToNoAccess { get; set; }
        public static bool IsMaker
        {
            get
            {
                bool _returnVal = false;
                var _routeParamValue = GetRouteParamValue(RouteDataParamName_IsMaker);
                if (_routeParamValue != null)
                {
                    if (_routeParamValue is bool)
                    {
                        _returnVal = Convert.ToBoolean(_routeParamValue);
                    }
                }
                return _returnVal;
            }
        }

        public static bool IsChecker
        {
            get
            {
                bool _returnVal = false;
                var _routeParamValue = GetRouteParamValue(RouteDataParamName_IsChecker);
                if (_routeParamValue != null)
                {
                    if (_routeParamValue is bool)
                    {
                        _returnVal = Convert.ToBoolean(_routeParamValue);
                    }
                }
                return _returnVal;
            }
        }

        public static UserRights_Result UserRights
        {
            get
            {
                UserRights_Result _returnVal = null;
                var _routeParamValue = GetRouteParamValue(RouteDataParamName_UserRights);
                if (_routeParamValue != null)
                {
                    if (_routeParamValue is UserRights_Result)
                    {
                        _returnVal = (UserRights_Result)_routeParamValue;
                    }
                }
                return _returnVal;
            }
        }

        private static object GetRouteParamValue(string paramName)
        {
            object _returnVal = null;
            RouteData _routeData = HttpContext.Current.Request.RequestContext.RouteData;
            if (_routeData != null)
            {
                _returnVal = _routeData.Values[paramName];
            }
            return _returnVal;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            bool _isValidRights = false;
            bool _isMaker = false;
            bool _isChecker = false;
            bool _isViewer = false;
            UserRights_Result _userRights = null;
            Function _FN = null;
            try
            {
                _FN = new Function();
                _userRights = _FN.CheckUserRight("", string.Empty, Web.SessionManager.SelectedMenuID);
                if (_userRights != null)
                {
                    _isMaker = _userRights.IsMaker;
                    _isChecker = _userRights.IsChecker;
                    _isViewer = _userRights.IsView;
                }
            }
            finally
            {
                _FN = null;

                filterContext.RouteData.Values.Add(RouteDataParamName_IsMaker, _isMaker);
                filterContext.RouteData.Values.Add(RouteDataParamName_IsChecker, _isChecker);
                filterContext.RouteData.Values.Add(RouteDataParamName_UserRights, _userRights);

                switch (UserRightsRequired)
                {
                    case UserRightsType.Maker:
                        _isValidRights = _isMaker;
                        break;
                    case UserRightsType.Checker:
                        _isValidRights = _isChecker;
                        break;
                    case UserRightsType.MakerOrChecker:
                        _isValidRights = _isMaker || _isChecker;
                        break;
                    case UserRightsType.MakerAndChecker:
                        _isValidRights = _isMaker && _isChecker;
                        break;
                    case UserRightsType.Viewer:
                        _isValidRights = _isViewer;
                        break;
                    default:
                        _isValidRights = false;
                        break;
                }
                
                if (!_isValidRights && RedirectToNoAccess)
                {
                    filterContext.Result = Web.Common.RedirectToNoAccessAction();
                }
            }
        }
    }
}