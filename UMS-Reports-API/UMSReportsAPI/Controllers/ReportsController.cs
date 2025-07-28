using UMSReportsAPI.Models;
using UMSReportsAPI.Utilities;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Web.Http;
using UMSReportsAPI.Extenders;
using Swashbuckle.Examples;
using UMSReportsAPI.SwaggerExamples;
using UMSReportsAPI.BO;

using log4net;
using log4net.Config;

namespace UMSReportsAPI.Controllers
{
    [RoutePrefix("api/Reports")]
    public class ReportsController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReportsController));
        //[AllowAnonymous]
        //[Route("UMS/test")]
        //[HttpGet]
        //[ClientCacheWithEtag(60)]  //1 min client side caching
        //public HttpResponseMessage EDPSReport()
        //{
        //    string reportPath = "~/Reports/EDPS";
        //    string reportFileName = "CodeList.rpt";
        //    string exportFilename = "CodeList.pdf";

        //    HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename);
        //    return result;
        //}

        [AllowAnonymous]
        [Route("UMS/RenderReport")]
        [HttpPost]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        [SwaggerRequestExample(typeof(JObject), typeof(UMSReportRequestExample))]
        public HttpResponseMessage UMSReport([FromBody] JObject formBody)
        {
            HttpResponseMessage result = CrystalReport.RenderReport(formBody);

            return result;
        }

        //[AllowAnonymous]
        //[Route("EDPS/test2/{shId}/{esId}/{cent}")]
        //[HttpGet]
        //[ClientCacheWithEtag(60)]  //1 min client side caching
        //public HttpResponseMessage EDPSReport3(int shId, int esId, string cent = "")
        //{
        //    string reportPath = "~/Reports/EDPS";
        //    string reportFileName = "CodeList.rpt";
        //    string exportFilename = "CodeList.pdf";

        //    HttpResponseMessage result = CrystalReport.RenderReport(reportPath, reportFileName, exportFilename);
        //    return result;
        //}

        [AllowAnonymous]
        [Route("UMS/EncodeCredentials")]
        [HttpGet]
        [ClientCacheWithEtag(60)]
        public HttpResponseMessage EncodeCredentials(string username, string password)
        {
            JObject keyValuePairs = new JObject();
            keyValuePairs["credentials"] = ConversionUtils.EncodeToBase64(username, password);
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, keyValuePairs);
        }

        [AllowAnonymous]
        [Route("UMS/RenderReportV2")]
        [HttpPost]
        [ClientCacheWithEtag(60)]  //1 min client side caching
        [SwaggerRequestExample(typeof(JObject), typeof(UMSReportRequestExample))]
        public HttpResponseMessage UMSLegacyReports([FromBody] JObject formBody)
        {
            HttpResponseMessage result = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            string reportPath = "";
            string reportFileName = "";
            string exportFilename = "";
            int loginId = 0;
            JObject filter = null;
            try { log.Debug(formBody.ToString()); } catch { }
            try
            {
                filter = (JObject)formBody["filter"];
                reportPath = formBody["reportPath"].ToString();
                exportFilename = formBody["exportFilename"].ToString();

                foreach (var property in filter)
                {
                    try
                    {
                        if (property.Key.ToUpper() == "LoginId".ToUpper())
                        {
                            loginId = property.Value.ToString().ToInt32();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine(ex.StackTrace);
                    }
                }
            }
            catch (Exception error)
            {
                log.Error(error.Message, error);
            }
            try
            {
                //return new LegacyReports().PrintReport(loginId, reportPath, filter);
                result = new LegacyReports().PrintReport( loginId, reportPath, filter);
                if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return new LegacyReports().NoContent();
            }
            catch (Exception error)
            {
                log.Error(error.Message, error);
            }
            return result;
        }
    }
}