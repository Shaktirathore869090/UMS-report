using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DocXReports.BO;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace MiscUtil
{
    public partial class frmDocXReporter : Form
    {
        public frmDocXReporter()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            btnProcess.Enabled = false;
            if (txtSource.Text.IsNullOrEmpty())
            {
                List<string> images = GeneratePDFImages();
                string strPayload = @"{
    ""filters"": {
        ""LoginId"": 1,
        ""DBName"": ""EDPS_MUHS_V1""
    },
    ""reportFileName"": ""TRANSCRIPT.docx"",
    ""exportFilename"": ""Report.pdf"",
    ""reportPath"": """",
    ""payload"": {
        ""RgNo"": ""ENGME/00000077"",
        ""StuFullName"": ""Digambar Ramesh Koli"",
        ""StuFromDate"": ""2023-12-13T18:30:00"",
        ""StuToDate"": ""2024-11-03T18:30:00"",
        ""CoCode"": ""ENGME"",
        ""CoName"": ""Mechanical Engineering"",
        ""CgName"": ""Sanjay Ghodawat Institute"",
        ""StuInternshipFrom"": ""2024-06-02"",
        ""StuInternshipTo"":""2024-11-01""
    }
}
";
                JObject formBody = JObject.Parse(strPayload);

                JArray imageArray = new JArray();
                foreach (string image in images)
                {
                    var imageBytes = File.ReadAllBytes(image);
                    imageArray.Add(imageBytes);
                    //imageArray.Add(new JObject { { "imgData", imageBytes } });
                    //imageArray.Add(new JObject { { "src", image } });
                }
                JObject payload = formBody["payload"] as JObject;
                payload.Add("Pages", imageArray);
                formBody["payload"] = payload;

                string libraOfficePath = "C:\\LibreOfficePortable\\App\\libreoffice\\program\\soffice.exe";
                DocXReport.GeneratePDF(formBody, libraOfficePath);
            } else
            {
                JObject formBody = JObject.Parse(txtSource.Text);
                string libraOfficePath = "C:\\LibreOfficePortable\\App\\libreoffice\\program\\soffice.exe";
                DocXReport.GeneratePDF(formBody, libraOfficePath);
            }

                btnProcess.Enabled = true;
        }

        private List<string> GeneratePDFImages()
        {
            string pdfFolderPath = "SamplePDF";

            string outputFolder = $"TempImages\\{Guid.NewGuid().ToString()}";

            List<string> images = new List<string>();
            int fileNo = 0;
            foreach (string pdfPath in Directory.GetFiles(pdfFolderPath))
            {
                List<string> pdfImages = PdfToImage.RenderAllPdfPagesToImages(pdfPath, outputFolder, fileNo++);
                images.AddRange(pdfImages);
            }

            return images;
        }
    }
}
