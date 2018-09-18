using System.Web;
using System.Web.Optimization;

namespace AbcapGestionComercialMVC
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-dropdown-checkbox.min.js",
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/dataTables.fixedColumns.min.js",
                      "~/Scripts/jquery-ui.min.js",
                      "~/Scripts/dropdown.js",
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/PopupMensajes.js",
                      "~/Scripts/rollups/sha512.js",
                      "~/Scripts/fileinput.js",
                      "~/Scripts/locales/es.js",
                      "~/Scripts/jquery.number.js",
                      "~/Scripts/Contextual/jquery.contextMenu.min.js",
                      "~/Scripts/Contextual/jquery.ui.position.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content/bootstrap-dropdown-checkbox.css",
                      "~/Content/jquery.dataTables.min.css",
                      "~/Content/fixedColumns.dataTables.min.css",
                      "~/Content/bootstrap-select.min.css",
                      "~/Content/site.css",
                      "~/Content/fileinput.css",
                      "~/Content/Contextual/jquery.contextMenu.min.css"));
        }
    }
}
