using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Scanx_mvc.Controllers
{
    public class PdfController : Controller
    {
        private readonly string _uploadsFolder;

        public PdfController()
        {
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        // ✅ 1️⃣ Upload PDF File
        [HttpPost("Pdf/Upload")]
        public IActionResult Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected.");
            }

            string filePath = Path.Combine(_uploadsFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // ✅ Return a public URL for the uploaded file
            string fileUrl = $"/uploads/{file.FileName}";

            return Ok(new { fileUrl });
        }

        // ✅ 2️⃣ View PDF in Browser
        [HttpGet("Pdf/View")]
        public IActionResult ViewPdf(string fileName)
        {
            string fileUrl = $"/uploads/{fileName}";
            return Ok(new { fileUrl });
        }

        // ✅ 3️⃣ Extract Text from PDF
        [HttpGet("Pdf/ExtractText")]
        public IActionResult ExtractText(string fileName)
        {
            string filePath = Path.Combine(_uploadsFolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            using (PdfReader reader = new PdfReader(filePath))
            using (PdfDocument pdfDoc = new PdfDocument(reader))
            {
                System.Text.StringBuilder extractedText = new System.Text.StringBuilder();

                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    extractedText.AppendLine(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
                }

                return Ok(new { extractedText = extractedText.ToString() });
            }
        }
    }
}
