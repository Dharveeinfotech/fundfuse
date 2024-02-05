using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMP.Infrastructure.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EncryptedActionParameterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string _qsParamName = Utilities.QueryStringEncryption.PARAMETER_NAME.Replace("=", "");
            HttpRequest _request = HttpContext.Current.Request;
            Dictionary<string, object> _decryptedParams = new Dictionary<string, object>();
            string _encryptedQueryString = _request.QueryString[_qsParamName];
            if (!string.IsNullOrEmpty(_encryptedQueryString))
            {
                string _decryptedString = Utilities.QueryStringEncryption.Decrypt(_encryptedQueryString);
                string[] _qsParams = _decryptedString.Split('&');
                var _actionParams = filterContext.ActionDescriptor.GetParameters();
                foreach(var _qsItem in _qsParams)
                {
                    var _pair = _qsItem.Split('=');
                    if(_pair == null || _pair.Length < 2)
                    {
                        continue;
                    }
                    var _actionParam = _actionParams.FirstOrDefault(i => i.ParameterName.Equals(_pair[0],StringComparison.OrdinalIgnoreCase));
                    if (_actionParam != null)
                    {
                        var _nullType = Nullable.GetUnderlyingType(_actionParam.ParameterType);
                        if (_nullType != null)
                        {
                            filterContext.ActionParameters[_actionParam.ParameterName] = Convert.ChangeType(_pair[1], _nullType);
                        }
                        else
                        {
                            filterContext.ActionParameters[_actionParam.ParameterName] = Convert.ChangeType(_pair[1], _actionParam.ParameterType);
                        }
                    }
                }
            }            
        }
    }
}