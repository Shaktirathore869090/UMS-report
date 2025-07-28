using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
// Security
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Web;

namespace UMSReportsAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Security
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                if (e.Exception is SecurityException || e.Exception is MethodAccessException)
                {
                    EventLog.WriteEntry("Security", "Unauthorized reflection detected! API is shutting down.", EventLogEntryType.Error);
                    Environment.Exit(1);
                }
            };
        }

        protected void Application_BeginRequest()
        {
            // Security
            if (Debugger.IsAttached || Debugger.IsLogging())
            {
                //HttpContext.Current.Response.StatusCode = 403; // Forbidden
                //HttpContext.Current.Response.End();
            }
        }
    }
}