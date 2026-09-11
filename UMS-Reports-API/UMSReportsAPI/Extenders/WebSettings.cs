using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace UMSReportsAPI.Extenders
{
    public static class WebSettings
    {
        /// <summary>
        /// Read a config value, preferring environment variables over Web.config.
        /// This allows Docker / container deployments to override settings without
        /// modifying Web.config (e.g. -e DBServer=192.168.1.10).
        /// </summary>
        private static string Resolve(string key)
        {
            return Environment.GetEnvironmentVariable(key).Coalesce()
                ?? ConfigurationManager.AppSettings[key].Coalesce();
        }

        public static string ReadKey(string key) { return Resolve(key); }
        public static string DBServer { get { return Resolve("DBServer"); } }
        public static string DBName { get { return Resolve("DBName"); } }
        public static string DBCredentials { get { return Resolve("DBCredentials"); } }
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