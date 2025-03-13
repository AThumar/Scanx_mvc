using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

[Route("api/pdf")]
[ApiController]
public class PdfController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public PdfController(IWebHostEnvironment env)
    {
        _env = env;
    }

    // 📌 Upload PDF and Extract Text
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile pdfFile, string fileName)
    {
        if (pdfFile == null || pdfFile.Length == 0)
        {
            ModelState.AddModelError("", "Please select a file.");
            return BadRequest(new { error = "Please select a file." });
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

        var filePath = Path.Combine(uploadsFolder, fileName + Path.GetExtension(pdfFile.FileName));

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await pdfFile.CopyToAsync(stream);
        }

        return RedirectToAction("Index");
    }

    // 📌 Send extracted text to AI for response
    [HttpPost("ai-answer")]
    public async Task<IActionResult> GetAiAnswer([FromBody] AiRequestModel request)
    {
        string aiResponse = await GetAiResponse(request.Text);
        return Ok(new { success = true, fileName = request.FileName, answer = aiResponse });
    }

    // 📌 Extract text from PDF (STATIC)
    private static string ExtractTextFromPdf(string filePath)
    {
        using var pdfReader = new PdfReader(filePath);
        using var pdfDocument = new PdfDocument(pdfReader);
        string text = "";

        for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
        {
            text += PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i));
        }
        return text;
    }

    // 📌 OpenAI API Call (STATIC)
    private static async Task<string> GetAiResponse(string inputText)
    {
        string apiKey = "YOUR_OPENAI_API_KEY";
        string endpoint = "https://api.openai.com/v1/chat/completions";

        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestData = new
        {
            model = "gpt-4",
            messages = new[] { new { role = "user", content = inputText } }
        };

        string jsonContent = JsonConvert.SerializeObject(requestData);
        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync(endpoint, content);
        string result = await response.Content.ReadAsStringAsync();

        dynamic jsonResponse = JsonConvert.DeserializeObject(result);
        return jsonResponse.choices[0].message.content;
    }
}

// Model for AI request
public class AiRequestModel
{
    public string Text { get; set; }
    public string FileName { get; set; }
}
