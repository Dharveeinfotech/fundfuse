using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMP.Infrastructure.Web
{   
    public enum WorkflowUserType
    {
        UnKnown = 0,
        Maker,
        Checker,
        Viewer
    }

    public enum SubscriptionTabs
    {
        GLOBALSCF,
        MYPORTFOLIO,
        MYACCOUNT
    }
    public static class Common
    {

        public const string LoginPageUrl = "~/mUserMasters/Login";
        public const string NoAccessPageUrl = "~/MasterPage/NoAccessPage";

        public static void RedirectToLoginPage()
        {
            HttpContext.Current.Response.Redirect(LoginPageUrl, true);
        }

        public static void RedirectToNoAccessPage()
        {
            HttpContext.Current.Response.Redirect(NoAccessPageUrl, true);
        }

        public static ActionResult RedirectToNoAccessAction()
        {
            return new RedirectResult(NoAccessPageUrl, false);
        }

        public static bool IsAjaxRequest
        {
            get
            {
                bool _isAjaxRequest = false;

                var _request = HttpContext.Current.Request;

                if (_request == null)
                {
                    throw new ArgumentNullException("SIW.Common.IsAjaxRequest: Request is null");
                }

                _isAjaxRequest = (_request["X-Requested-With"] == "XMLHttpRequest") ||
                                 ((_request.Headers != null) && (_request.Headers["X-Requested-With"] == "XMLHttpRequest"));

                return _isAjaxRequest;
            }
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

    }
}