using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using UMSReportsAPI.Extenders;

namespace UMSReportsAPI.Utilities
{
    public static class CrystalReport
    {
        //public static HttpResponseMessage RenderReport(string reportPath, string reportFileName, string exportFilename, JObject formBody = null)
        //{
        //    var rd = new ReportDocument();
        //    rd.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName));
        //    rd.SetDatabaseLogon("sa", "Pass@123", "192.168.1.101\\SQLEXPRESS", "EDPS_NAN");
        //    if (formBody != null)
        //    {
        //        foreach (var property in (JObject)formBody)
        //        {
        //            try
        //            {
        //                rd.SetParameterValue("@" + property.Key, property.Value.ToString());
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.ToString());
        //                Console.WriteLine(ex.StackTrace);
        //            }
        //        }
        //        //rd.SetParameterValue("@ShId", 209);
        //        //rd.SetParameterValue("@EsId", 0);
        //        //rd.SetParameterValue("@Cent", "");
        //    }

        //    MemoryStream ms = new MemoryStream();
        //    using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
        //    {
        //        stream.CopyTo(ms);
        //    }

        //    var result = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(ms.ToArray())
        //    };
        //    result.Content.Headers.ContentDisposition =
        //        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //        {
        //            FileName = exportFilename
        //        };
        //    result.Content.Headers.ContentType =
        //        new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
        //    return result;
        //}
        public static HttpResponseMessage RenderReport(JObject formBody)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var rd = new ReportDocument();

            string reportPath = "";
            string reportFileName = "";
            string exportFilename = "";
            JObject filter = null;

            try
            {
                filter = (JObject)formBody["filter"];
                reportPath = formBody["reportPath"].ToString();
                reportFileName = formBody["reportFileName"].ToString();
                exportFilename = formBody["exportFilename"].ToString();
            }
            catch (Exception error) { }

            rd.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName));
            (string userId, string password) = ConversionUtils.DecodeBase64Credentials(WebSettings.DBCredentials);
            rd.DataSourceConnections[0].SetConnection(WebSettings.DBServer, WebSettings.DBName, userId, password);
            
            //rd.SetDatabaseLogon("sa", "Pass@123");
            //rd.DataSourceConnections[0].SetConnection("127.0.0.1", "EDPS_NAN", "sa", "Pass@123");
            //rd.SetDatabaseLogon("sa", "sa");

            //rd.SetDatabaseLogon("sa", "Pass@123", "192.168.1.101\\SQLEXPRESS", "EDPS_NAN");
            //rd.SetDatabaseLogon("sa", "Pass@123");
            //rd.DataSourceConnections[0].SetConnection("194.163.145.88:61443", "EDPS_NAN", "sa", "Flixir@2023");
            //rd.DataSourceConnections[0].SetConnection("Driver={ODBC Driver 17 for SQL Server};Server=194.163.145.88:61443;", "EDPS_NAN", "sa", "Flixir@2023");
            //rd.DataSourceConnections[0].SetConnection("Driver={ODBC Driver 17 for SQL Server};Server=127.0.0.1", "EDPS_NAN", "sa", "Pass@123");
            //rd.DataSourceConnections[0].SetConnection("192.168.1.103", "EDPS_NAN", "sa", "Pass@123");

            //rd.SetDatabaseLogon("sa", "Pass@123", "192.168.1.253", "EDPS_NAN");
            //rd.DataSourceConnections[0].SetConnection(".", "EDPS_NAN", "sa", "Pass@123");

            //rd.DataSourceConnections[0].SetConnection("192.168.1.253\\SQLEXPRESS", "EDPS_NAN", "sa", "sa");



            if (formBody != null)
            {
                foreach (var property in filter)
                {
                    try
                    {
                        rd.SetParameterValue("@" + property.Key, property.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }

            MemoryStream ms = new MemoryStream();
            using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
            {
                stream.CopyTo(ms);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = exportFilename
                };
            result.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            return result;
        }

        //public static HttpResponseMessage RenderReport1(JObject formBody)
        //{
        //    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //    var rd = new ReportDocument();

        //    string reportPath = "";
        //    string reportFileName = "";
        //    string exportFilename = "";
        //    JObject filter = null;

        //    try
        //    {
        //        filter = (JObject)formBody["filter"];
        //        reportPath = formBody["reportPath"].ToString();
        //        reportFileName = formBody["reportFileName"].ToString();
        //        exportFilename = formBody["exportFilename"].ToString();
        //    }
        //    catch (Exception error) { }

        //    rd.Load(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(reportPath), reportFileName));

        //    // Create an instance of the ReportDocument class
        //    //ReportDocument report = new ReportDocument();

        //    // Load the report file
        //    //report.Load("path_to_your_report_file.rpt");

        //    // Get the database tables used in the report
        //    Tables tables = rd.Database.Tables;

        //    // Create a new ConnectionInfo object with the updated database connection details
        //    ConnectionInfo connectionInfo = new ConnectionInfo();
        //    connectionInfo.ServerName = "192.168.1.253\\SQLEXPRESS";
        //    connectionInfo.DatabaseName = "EDPS_NAN";
        //    connectionInfo.UserID = "sa";
        //    connectionInfo.Password = "sa";

        //    // Iterate through each table and update the database connection
        //    foreach (Table table in tables)
        //    {
        //        TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
        //        tableLogOnInfo.ConnectionInfo = connectionInfo;

        //        tableLogOnInfo.TableName = "Proc(Rpt_Ledger;1)"; // Replace with the actual table name

        //        table.ApplyLogOnInfo(tableLogOnInfo);
        //    }

        //    // Refresh the report to apply the updated database connection
        //    //rd.Refresh();

        //    if (formBody != null)
        //    {
        //        foreach (var property in filter)
        //        {
        //            try
        //            {
        //                rd.SetParameterValue("@" + property.Key, property.Value.ToString());
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.ToString());
        //                Console.WriteLine(ex.StackTrace);
        //            }
        //        }
        //    }

        //    MemoryStream ms = new MemoryStream();
        //    using (var stream = rd.ExportToStream(ExportFormatType.PortableDocFormat))
        //    {
        //        stream.CopyTo(ms);
        //    }

        //    var result = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(ms.ToArray())
        //    };
        //    result.Content.Headers.ContentDisposition =
        //        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //        {
        //            FileName = exportFilename
        //        };
        //    result.Content.Headers.ContentType =
        //        new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
        //    return result;
        //}
    }
}