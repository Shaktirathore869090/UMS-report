using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace UMSReportsAPI.Extenders
{
    public static class WebSettings
    {
        public static string ReadKey(string key) { return ConfigurationManager.AppSettings[key].Coalesce(); }
        public static string DBServer { get { return ConfigurationManager.AppSettings["DBServer"].Coalesce(); } }
        public static string DBName { get { return ConfigurationManager.AppSettings["DBName"].Coalesce(); } }
        public static string DBCredentials { get { return ConfigurationManager.AppSettings["DBCredentials"].Coalesce(); } }
        public static string ConnectionString
        {
            get
            {
                (string userId, string password) = ConversionUtils.DecodeBase64Credentials(WebSettings.DBCredentials);
                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
                connectionString = connectionString.Replace("{{server}}", DBServer);
                connectionString = connectionString.Replace("{{database}}", DBName);
                connectionString = connectionString.Replace("{{userId}}", userId);
                connectionString = connectionString.Replace("{{password}}", password);
                return connectionString;
            }
        }
    }
}