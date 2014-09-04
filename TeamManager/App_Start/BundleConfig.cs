using System.Web;
using System.Web.Optimization;

namespace TeamManager
{
    public class BundleConfig
    {
        // Pour plus d'informations sur Bundling, accédez à l'adresse http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Utilisez la version de développement de Modernizr pour développer et apprendre. Puis, lorsque vous êtes
            // prêt pour la production, utilisez l'outil de génération sur http://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Dependencies/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/js").Include(
                "~/Dependencies/Scripts/jquery-{version}.js",
                "~/Dependencies/Scripts/jquery-migrate-{version}.js",
                "~/Dependencies/Scripts/bootstrap.js",
                "~/Dependencies/Scripts/jquery.validate.js",
                "~/Dependencies/Scripts/jquery.validate.unobtrusive.js",
                "~/Dependencies/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js"
                ));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-mvc-validation.css",
                "~/Content/site.css"
                ));
        }
    }
}