using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace UMSReportsAPI.SwaggerExamples
{
    public class UMSReportRequestExample: IExamplesProvider
    {
        public object GetExamples()
        {
            string json = @"
{
    ""reportPath"": ""~/Reports/UMS"",
    ""reportFileName"": ""CodeList.rpt"",
    ""exportFilename"": ""CodeList.pdf"",
    ""filter"": {
        ""ShId"": 215,
        ""EsId"": 0,
        ""Cent"": """"
    }
}";
            JObject exaple = JObject.Parse(json);
            return exaple;
        }
    }
}