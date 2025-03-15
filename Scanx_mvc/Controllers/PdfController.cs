using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scanx_mvc.Models; // ✅ Correctly using the model namespace

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

            // ✅ Now correctly using Scanx_mvc.Models.PdfFile
            var pdfFiles = Directory.GetFiles(uploadPath, "*.pdf")
                                    .Select(file => new PdfFile
                                    {
                                        FileName = Path.GetFileName(file),
                                        FilePath = $"/uploads/{Path.GetFileName(file)}"
                                    })
                                    .ToList();

            return View(pdfFiles);
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
}
