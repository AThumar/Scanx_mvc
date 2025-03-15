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

        [HttpGet, HttpPost]
        public IActionResult Upload(IFormFile? file)
        {
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // ✅ Handle file upload (POST request)
            if (file != null && file.Length > 0)
            {
                string filePath = Path.Combine(uploadPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return RedirectToAction("Upload"); // Refresh the page to show the updated list
            }

            // ✅ Fetch all uploaded PDFs (GET request)
            var pdfFiles = Directory.GetFiles(uploadPath, "*.pdf")
                                    .Select(file => new PdfFile
                                    {
                                        FileName = Path.GetFileName(file),
                                        FilePath = $"/uploads/{Path.GetFileName(file)}"
                                    })
                                    .ToList();

            return View("Upload", pdfFiles); // ✅ Load "Upload.cshtml" with the updated file list
        }

        public IActionResult ViewPdf(string fileName)
        {
            var model = new PdfViewModel
            {
                FileName = fileName
            };
            return View(model);
        }




        public IActionResult Dashboard()
        {
            string uploadPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Fetch all PDF files dynamically
            var pdfFiles = Directory.GetFiles(uploadPath, "*.pdf")
                                    .Select(Path.GetFileName)
                                    .ToList();

            return View(pdfFiles);  // ✅ Pass the latest files to the Dashboard view
        }


    }

}
