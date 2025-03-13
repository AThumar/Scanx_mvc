using System.IO;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
[Route("Pdf")]

public class PdfController : Controller
{
    public IActionResult Viewer(string fileName)
    {
        // ✅ Define folderPath before using it
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        // ✅ Now, use it correctly
        string filePath = Path.Combine(folderPath, fileName);
        ViewBag.PdfPath = "/uploads/" + fileName;

        // ✅ Fix: Mark ExtractTextFromPdf as static
        ViewBag.PdfText = ExtractTextFromPdf(filePath);

        return View();
    }

    // ✅ Step 2: Make the method static (Fix Warning CA1822)
    public static string ExtractTextFromPdf(string filePath)
    {
        using PdfReader reader = new PdfReader(filePath);
        using PdfDocument pdfDoc = new PdfDocument(reader);
        StringWriter output = new();

        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
        {
            output.WriteLine(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
        }

        return output.ToString();
    }
    [HttpPost]
    public IActionResult Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file selected.");
        }

        // Define upload folder
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        // Create directory if it doesn't exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Save the uploaded file
        string filePath = Path.Combine(folderPath, file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Ok(new { filePath });
    }

}
