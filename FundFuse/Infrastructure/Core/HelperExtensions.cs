using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using TMP.Infrastructure.Utilities;
using System.Web.Routing;

namespace TMP.Infrastructure.Core
{
    public static class HelperExtensions
    {
        private static string GetEncryptedData(object args)
        {
            var _list = new RouteValueDictionary(args);
            var _items = new List<string>();

            foreach (var _entry in _list)
            {
                _items.Add(_entry.Key + "=" + ((_entry.Value != null) ? _entry.Value.ToString() : ""));
            }

            string _encryptedUrl = Utilities.QueryStringEncryption.EncryptQuery(string.Join("&", _items));
            if (_encryptedUrl.StartsWith(Utilities.QueryStringEncryption.PARAMETER_NAME, StringComparison.OrdinalIgnoreCase))
            {
                _encryptedUrl = _encryptedUrl.Replace(Utilities.QueryStringEncryption.PARAMETER_NAME, string.Empty);
            }
            return _encryptedUrl;
        }

        public static ActionResult RedirectToActionEncrypted(this Controller controllerBase, string action, object routeValues)
        {
            if (!Utilities.QueryStringEncryption.IsEnabled)
            {
                var _newRouteValues = new RouteValueDictionary(routeValues);
                _newRouteValues.Add("action", action);
                return new RedirectToRouteResult(_newRouteValues);
            }
            var _encryptedArgs = GetEncryptedData(routeValues);
            return new RedirectToRouteResult(new RouteValueDictionary(new
            {
                action = action,
                qs = _encryptedArgs
            }));
        }

        public static ActionResult RedirectToActionEncrypted(this Controller controllerBase, string action, string controller, object routeValues)
        {
            if (!Utilities.QueryStringEncryption.IsEnabled)
            {
                var _newRouteValues = new RouteValueDictionary();
                _newRouteValues.Add("action", action);
                _newRouteValues.Add("controller", controller);
                return new RedirectToRouteResult(_newRouteValues);
            }
            var _encryptedArgs = GetEncryptedData(routeValues);
            return new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = controller,
                action = action,
                qs = _encryptedArgs
            }));
        }

        public static string ActionEncrypted(this UrlHelper url, string action, string controller, object routeValues)
        {
            if (!Utilities.QueryStringEncryption.IsEnabled)
            {
                return url.Action(action, controller, routeValues);
            }
            var _encryptedArgs = GetEncryptedData(routeValues);
            return url.Action(action, controller, new { qs = _encryptedArgs });
        }

        public static MvcHtmlString ActionLinkEncrypted(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            if (!Utilities.QueryStringEncryption.IsEnabled)
            {
                return htmlHelper.ActionLink(linkText: linkText,
                                         actionName: actionName,
                                         controllerName: controllerName,
                                         routeValues: routeValues,
                                         htmlAttributes: htmlAttributes);
            }
            var _encryptedArgs = GetEncryptedData(routeValues);
            return htmlHelper.ActionLink(linkText: linkText,
                                         actionName: actionName,
                                         controllerName: controllerName,
                                         routeValues: new { qs = _encryptedArgs },
                                         htmlAttributes: htmlAttributes);
        }

    }
}