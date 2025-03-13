using System.IO;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

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
        using PdfReader reader = new(filePath);
        StringWriter output = new();

        for (int i = 1; i <= reader.NumberOfPages; i++)
        {
            output.WriteLine(PdfTextExtractor.GetTextFromPage(reader, i));
        }

        return output.ToString();
    }
}
