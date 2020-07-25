using System.Web;
using System.Web.Optimization;

namespace Dal
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

            bundles.Add(new StyleBundle("~/Content/stylecss").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));
            
            bundles.Add(new StyleBundle("~/Content/2css").Include(
                   "~/Content/bootstrap.css",
                   "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Content/scriptstemplate").Include(
                   "~/content/jscript/theme-scripts.js",
                   "~/content/jscript/lightbox.js",
                   "~/content/jscript/iscroll.js",
                   "~/content/jscript/modernizr.custom.50878.js",
                   "~/content/jscript/dat-menu.js",
                   "~/content/jscript/demo-settings.js"));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
