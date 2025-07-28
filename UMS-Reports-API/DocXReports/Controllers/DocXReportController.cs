using DocXReports.BO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DocXReports.Controllers
{
    [ApiController]
    [Route("api/DocXReports")]
    public class DocXReportController : ControllerBase
    {
        [HttpPost("RenderV1")]
        public async Task<IActionResult> RenderDocXReportV1([FromBody] JObject formBody)
        {
            try
            {
                string pdfFilePath = DocXReport.GeneratePDF(formBody);
                // Open the file stream
                var fileStream = new FileStream(pdfFilePath, FileMode.Open, FileAccess.Read);

                // // Return the file as a stream
                //return File(fileStream, "application/pdf", "Report.pdf");

                // Use a file stream result to return the file
                var fileResult = File(fileStream, "application/pdf", "Report.pdf");

                // Dispose the stream and delete the file after returning the file result
                fileStream.Dispose();

                System.IO.File.Delete(pdfFilePath);
                System.IO.File.Delete(pdfFilePath.Replace(".pdf", ".docx"));

                return fileResult;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while rendering the PDF: {ex.Message}");
            }
        }

        [HttpPost("Render")]
        public async Task<IActionResult> RenderDocXReport([FromBody] JObject formBody)
        {
            string pdfFilePath = null;
            try
            {
                Console.WriteLine($"Received formBody: {formBody.ToString()}");

                // Generate the PDF file
                pdfFilePath = DocXReport.GeneratePDF(formBody);

                // Resolve the absolute path
                string absolutePdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), pdfFilePath);

                // Return the file as a PhysicalFileResult (file path)
                var result = PhysicalFile(absolutePdfFilePath, "application/pdf", "Report.pdf");

                // Schedule file deletion after response has been sent
                HttpContext.Response.OnCompleted(() =>
                {
                    try
                    {
                        if (System.IO.File.Exists(pdfFilePath))
                            System.IO.File.Delete(pdfFilePath);
                        string wordFilePath = pdfFilePath.Replace(".pdf", ".docx");
                        if (System.IO.File.Exists(wordFilePath))
                            System.IO.File.Delete(wordFilePath);
                    }
                    catch (Exception ex)
                    {
                        // Log deletion failure
                        Console.WriteLine($"Failed to delete file {pdfFilePath}: {ex.Message}");
                    }

                    return Task.CompletedTask;
                });

                return result;
            }
            catch (Exception ex)
            {
                // Ensure the file is deleted if an exception occurs
                if (!string.IsNullOrEmpty(pdfFilePath) && System.IO.File.Exists(pdfFilePath))
                {
                    System.IO.File.Delete(pdfFilePath);
                }

               // return StatusCode(500, $"An error occurred while rendering the PDF: {ex.Message}");
                return StatusCode(500, new { Error = ex.Message, FormBody = formBody.ToString() });
            }
        }

    }
}
