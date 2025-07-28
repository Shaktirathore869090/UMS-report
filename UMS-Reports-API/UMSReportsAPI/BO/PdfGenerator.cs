using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.UI.WebControls;
using UMSReportsAPI.Extenders;

namespace UMSReportsAPI.BO
{
    public class PdfGenerator
    {
        public static HttpResponseMessage GeneratePdf(
            string[] textLines,
            PdfSharp.PageSize pageSize = PdfSharp.PageSize.A4,
            PdfSharp.PageOrientation pageOrientation = PdfSharp.PageOrientation.Portrait,
            double leftMargin = 50,
            double rightMargin = 50,
            double topMargin = 50,
            double bottomMargin = 50,
            string reportHeader = null,
            string reportFooter = null,
            string pageHeader = null,
            string pageFooter = null,
            bool pageNumbering = false,
            string documentTitle = null,
            int fontSize = 8)
        {
            // Create a new PDF document with custom page size
            PdfDocument document = new PdfDocument();
            document.Info.Title = documentTitle.Coalesce();

            // Initialize page numbering
            int currentPage = 1;

            // Create the first page
            PdfPage page = document.AddPage();
            page.Size = pageSize;
            page.Orientation = pageOrientation;
            XGraphics gfx = XGraphics.FromPdfPage(page);
            //XFont font = new XFont("Courier New", 12);
            XFont font = new XFont("Courier New", fontSize);

            // Initialize coordinates for text drawing
            double xPos = leftMargin;
            double yPos = topMargin;

            foreach (string line in textLines)
            {
                // Calculate the height of the current line of text
                double lineHeight = gfx.MeasureString(line, font).Height;

                // Calculate available height for text on the page
                double availableHeight = page.Height - yPos - bottomMargin;

                // Check if there's enough space on the current page for the current line
                if (lineHeight <= availableHeight && line != "^m")
                {
                    // Draw the current line on the current page
                    gfx.DrawString(line, font, XBrushes.Black, new XPoint(xPos, yPos));
                    yPos += lineHeight;
                }
                else
                {
                    // Start a new page
                    currentPage++;
                    page = document.AddPage();
                    page.Size = pageSize;
                    page.Orientation = pageOrientation;
                    gfx = XGraphics.FromPdfPage(page);

                    // Reset coordinates for the new page
                    xPos = leftMargin;
                    yPos = topMargin;

                    // Draw page header on the new page
                    if (!string.IsNullOrEmpty(pageHeader))
                    {
                        gfx.DrawString(pageHeader, font, XBrushes.Black, new XPoint(xPos, yPos));
                        yPos += gfx.MeasureString(pageHeader, font).Height;
                    }

                    // Draw the current line on the new page
                    gfx.DrawString(line, font, XBrushes.Black, new XPoint(xPos, yPos));
                    yPos += lineHeight;
                }

                // Draw report footer on each page if provided
                if (!string.IsNullOrEmpty(reportFooter))
                {
                    gfx.DrawString(reportFooter, font, XBrushes.Black, new XPoint(leftMargin, page.Height - bottomMargin));
                }

                // Draw page number if requested
                if (pageNumbering)
                {
                    string pageNumber = $"Page {currentPage}";
                    double pageNumberWidth = gfx.MeasureString(pageNumber, font).Width;
                    gfx.DrawString(pageNumber, font, XBrushes.Black, new XPoint(page.Width - rightMargin - pageNumberWidth, page.Height - bottomMargin));
                }

            }

            // Save the PDF to a memory stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);
            stream.Seek(0, SeekOrigin.Begin);

            // Set the response content type to PDF
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
            {
                FileName = "Line_Report.pdf"
            };

            return response;
        }
    }
}