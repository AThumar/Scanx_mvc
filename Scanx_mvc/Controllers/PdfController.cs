using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scanx_mvc.Models; // ✅ Use the correct namespace

namespace Scanx_mvc.Controllers
{
    public class PdfController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public PdfController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Upload()
        {
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Ensure you're using Models.PdfFile
            var pdfFiles = Directory.GetFiles(uploadPath, "*.pdf")
                                    .Select(file => new PdfFile  // ✅ Correct class from Models
                                    {
                                        FileName = Path.GetFileName(file),
                                        FilePath = $"/uploads/{Path.GetFileName(file)}"
                                    })
                                    .ToList();

            return View(pdfFiles); // ✅ Ensure View is expecting List<Models.PdfFile>
        }


        [HttpPost]
        public IActionResult UploadPdf(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string filePath = Path.Combine(uploadPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return RedirectToAction("Upload");
        }
    }

    public class PdfFile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
