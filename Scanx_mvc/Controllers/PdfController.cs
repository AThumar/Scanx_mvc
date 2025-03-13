using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Mvc;
using System.IO;

public class PdfController : Controller
{
    public IActionResult Viewer(string fileName)
    {
        string filePath = Path.Combine("wwwroot/uploads", fileName);
        ViewBag.PdfPath = "/uploads/" + fileName;
        ViewBag.PdfText = ExtractTextFromPdf(filePath); // Store extracted text for AI search
        return View();
    }

    private string ExtractTextFromPdf(string filePath)
    {
        using (PdfReader reader = new PdfReader(filePath))
        {
            string text = "";
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, i);
            }
            return text;
        }
    }
}
