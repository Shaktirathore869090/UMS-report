using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using PDFtoImage;
using SkiaSharp;

using DocXReports.Extenders;
using Newtonsoft.Json.Linq;

namespace DocXReports.BO
{
    public class PdfToImage
    {
        public static List<string> RenderAllPdfPagesToImages(string pdfPath, string outputFolder, int fileNo = 1)
        {
            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            List<string> imagePaths = new List<string>();
            FileInfo fileInfo = new FileInfo(pdfPath);
            string fileName = fileInfo.Name.Replace(".pdf", "");
            Stream pdfStream = File.OpenRead(pdfPath);
            var bitmaps = Conversion.ToImages(pdfStream, false);
            int pageNo = 0;
            foreach (var image in bitmaps)
            {
                string outputFile = Path.Combine(outputFolder, $"T{fileNo.ToString().TrimLeftPadding('0', 3)}_{pageNo++.ToString().TrimLeftPadding('0', 3)}_{fileName}.png");
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    using (var stream = File.OpenWrite(outputFile))
                    {
                        data.SaveTo(stream);
                        imagePaths.Add(outputFile);
                    }
                }
            }

            return imagePaths;
        }

        public static JArray ConvertPdfToPngBase64(string base64Pdf)
        {
            // Convert Base64 string to stream
            byte[] pdfBytes = Convert.FromBase64String(base64Pdf);
            using (var pdfStream = new MemoryStream(pdfBytes))
            {
                // Convert PDF to images
                var bitmaps = Conversion.ToImages(pdfStream, false);

                // Initialize JArray to hold PNG byte arrays
                JArray pngByteArray = new JArray();

                foreach (var image in bitmaps)
                {
                    // Encode each bitmap to PNG
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        // Convert PNG data to byte array
                        byte[] pngBytes = data.ToArray();
                        pngByteArray.Add(JToken.FromObject(pngBytes));
                    }
                }

                return pngByteArray;
            }
        }

        public static JArray ConvertPdfFileToPngBase64(string filePath)
        {
            JArray pngByteArray = new JArray();
            Stream pdfStream = File.OpenRead(filePath);
            var bitmaps = Conversion.ToImages(pdfStream, false);
            foreach (var image in bitmaps)
            {
                // Encode each bitmap to PNG
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    // Convert PNG data to byte array
                    byte[] pngBytes = data.ToArray();
                    pngByteArray.Add(JToken.FromObject(pngBytes));
                }
            }

            return pngByteArray;
        }
    }
}
