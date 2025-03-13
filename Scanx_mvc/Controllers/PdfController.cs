using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Scanx_mvc.Controllers
{
    public class PdfController : Controller
    {
        private readonly string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public PdfController()
        {
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile pdfFile, string fileName)
        {
            if (pdfFile == null || pdfFile.Length == 0)
                return Json(new { success = false, error = "No file selected" });

            string filePath = Path.Combine(uploadDirectory, fileName + ".pdf");

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
