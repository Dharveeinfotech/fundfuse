using System.Web;
using System.Web.Optimization;

namespace TMP
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                       "~/Content/onlineReg.css"));
            bundles.Add(new ScriptBundle("~/bundles/onlineJS").Include(
                       "~/Scripts/jquery-ui.min.js",
                       "~/TableJs/jquery.dataTables.js",
                       "~/TableJs/dataTables.jqueryui.js",
                       "~/Scripts/jquery.validationEngine.js",
                       "~/Scripts/jquery.validationEngine-en.js",
                       "~/Scripts/onlineJS.js",
                       "~/Scripts/jquery.maskedinput.js",
                       "~/Scripts/jquery.maskedinput.min.js",
                       "~/Scripts/intlTelInput.min.js",
                       "~/Scripts/loadingoverlay.min.js",
                       "~/Scripts/loadingoverlay_progress.min.js",
                        "~/Scripts/bootstrap-multiselect.js"
                       ));
            bundles.Add(new StyleBundle("~/Content/onlineCss").Include(
                       "~/TableJs/jquery-ui.css",
                       "~/TableJs/dataTables.jqueryui.css",
                        "~/Content/all.css",
                       "~/Content/onlineReg.css",
                       "~/Content/ValidationEngine.css",
                       "~/Content/intlTelInput.css",
                       "~/Content/bootstrap-multiselect.css"));
        }
    }
}
