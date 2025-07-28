using DocumentFormat.OpenXml.Wordprocessing;
using DocxTemplater;
using DocxTemplater.Images;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Pipes;
using DocumentFormat.OpenXml.EMMA;
using SkiaSharp;
using PDFtoImage;
using System.Xml.Linq;
using DocxTemplater.Processors;
using System.IO.Compression;

namespace DocXReports.BO
{
    public class DocXReport
    {
        public static string GeneratePDF(JObject formBody, string libraOfficePath = "")
        {
            string reportPath = "";
            string reportFileName = "";
            string exportFilename = "";
            JObject filter = null;
            JObject model = null;

            try { filter = (JObject)formBody["filter"]; } catch (Exception error) { }
            try { reportPath = formBody["reportPath"].ToString(); } catch (Exception error) { }
            try { reportFileName = formBody["reportFileName"].ToString(); } catch (Exception error) { }
            try { exportFilename = formBody["exportFilename"].ToString(); } catch (Exception error) { }

            try
            {
                model = (JObject)formBody["payload"];
            }
            catch (Exception error) { }

            if (model["PDFFiles"] != null)
            {
                ConvertPdfPagesToImages(ref model);
            }

            reportPath = reportPath == "" ? "Templates" : reportPath;

            using var fileStream = File.OpenRead(Path.Combine(reportPath, reportFileName));

            var docTemplate = new DocxTemplate(fileStream);
            docTemplate.RegisterFormatter(new ImageFormatter());

            object m = ConvertToExpandoObjectWithBase64Handling(model);
            dynamic expando = (ExpandoObject)m;

            docTemplate.BindModel("expando", expando);

            var result = docTemplate.Process();
            docTemplate.Validate();
            result.Position = 0;

            if (!Directory.Exists("Resources"))
                Directory.CreateDirectory("Resources");

            string fileName = $"Resources/Rendered_{Guid.NewGuid().ToString()}.docx";
            using (var outputFileStream = File.OpenWrite(fileName))
            {
                result.CopyTo(outputFileStream);
            }

            // Table merging
            try
            {
                ProcessDocument(fileName);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
                Console.WriteLine(error.StackTrace);
            }

            LibreOfficeWrapper.Convert(fileName, fileName.Replace(".docx", ".pdf"), libraOfficePath);

            string pdfFilePath = fileName.Replace(".docx", ".pdf");

            return pdfFilePath;

        }

        internal static void ProcessDocument(string docxPath)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "temp_docx");
            Directory.CreateDirectory(tempPath);

            // 🔹 1. Extract the .docx file (which is a ZIP archive)
            ZipFile.ExtractToDirectory(docxPath, tempPath, true);
            string xmlPath = Path.Combine(tempPath, "word/document.xml");

            if (!File.Exists(xmlPath))
                throw new FileNotFoundException("word/document.xml not found in DOCX");

            // 🔹 2. Load and modify the XML content
            XDocument doc = XDocument.Load(xmlPath);

            // Optional: If you need to modify tables
            var tables = doc.Descendants().Where(e => e.Name.LocalName == "tbl").ToList();
            TableProcessor tableProcessor = new TableProcessor();
            foreach (var table in tables)
            {
                tableProcessor.ProcessTable(table);
            }

            // 🔹 3. Save the modified document.xml
            doc.Save(xmlPath);

            // 🔹 4. Repack the modified files into a new .docx file
            string filePrefix = ""; //"Updated_";
            string newDocxPath = Path.Combine(Path.GetDirectoryName(docxPath), filePrefix + Path.GetFileName(docxPath));
            if (File.Exists(newDocxPath))
                File.Delete(newDocxPath);

            ZipFile.CreateFromDirectory(tempPath, newDocxPath);

            // 🔹 5. Cleanup temp folder
            Directory.Delete(tempPath, true);

            Console.WriteLine("Document processed and saved as: " + newDocxPath);
        }
        internal static XDocument LoadDocxAsXml(string docxPath)
        {
            // Extract `word/document.xml` from the .docx archive
            using (ZipArchive archive = ZipFile.OpenRead(docxPath))
            {
                var entry = archive.GetEntry("word/document.xml");
                if (entry == null)
                    throw new FileNotFoundException("word/document.xml not found in DOCX");

                using (var stream = entry.Open())
                {
                    return XDocument.Load(stream);
                }
            }
        }
        internal static object ConvertToExpandoObjectWithBase64Handling(JToken token)
        {
            if (token.Type == JTokenType.Object)
            {
                var expando = new ExpandoObject();
                var dict = (IDictionary<string, object>)expando;

                foreach (var prop in (JObject)token)
                {
                    dict.Add(prop.Key, ConvertToExpandoObjectWithBase64Handling(prop.Value));
                }

                return expando;
            }
            else if (token.Type == JTokenType.Array)
            {
                return token.Select(ConvertToExpandoObjectWithBase64Handling).ToList();
            }
            else if (token.Type == JTokenType.String)
            {
                string value = token.ToString();
                return IsBase64String(value) ? Convert.FromBase64String(value) : value;
            }
            else
            {
                return token.ToObject<object>();
            }
        }

        internal static bool IsBase64String(string value)
         {
            //if (value == "Ayurvedacharya")
            //{
            //    value = value;
            //}
            if (string.IsNullOrEmpty(value) || value.Length % 4 != 0)
                return false;

            // Check if it contains only valid Base64 characters
            if (!Regex.IsMatch(value, @"^[A-Za-z0-9+/]*={0,2}$"))
                return false;

            try
            {
                byte[] decodedBytes = Convert.FromBase64String(value);
                string reEncoded = Convert.ToBase64String(decodedBytes);

                // Check only for images
                if (!IsImageFormat(decodedBytes))
                    return false;

                // Ensure the re-encoded string matches the input (to prevent false positives)
                return value.TrimEnd('=') == reEncoded.TrimEnd('=');
            }
            catch
            {
                return false;
            }
        }

        internal static bool IsImageFormat(byte[] data)
        {
            try
            {
                if (data == null || data.Length < 4)
                    return false;

                // Known image format signatures
                byte[][] imageSignatures = new byte[][] {
                new byte[] { 0x89, 0x50, 0x4E, 0x47 },      // PNG
                new byte[] { 0xFF, 0xD8, 0xFF },            // JPEG
                new byte[] { 0x47, 0x49, 0x46, 0x38 },      // GIF
                new byte[] { 0x42, 0x4D },                  // BMP
                new byte[] { 0x49, 0x49, 0x2A, 0x00 },      // TIFF (little-endian)
                new byte[] { 0x4D, 0x4D, 0x00, 0x2A }       // TIFF (big-endian)
            };

                return imageSignatures.Any(sig => data.Take(sig.Length).SequenceEqual(sig));
            }
            catch { return false; }
        }

        internal static void ConvertPdfPagesToImages(ref JObject model)
        {
            JArray pdfFiles = (JArray)model["PDFFiles"];
            JArray pdfPageImages = new JArray();
            foreach (string pdfFilePath in pdfFiles)
            {
                JArray filePages = PdfToImage.ConvertPdfFileToPngBase64(pdfFilePath);
                foreach (var pdfPage in filePages)
                    pdfPageImages.Add(pdfPage);

                // try deleting temp file
                try
                {
                    File.Delete(pdfFilePath);
                }
                catch (Exception err) { }
            }

            model.Add("PDFPages", pdfPageImages);
        }
    }
}