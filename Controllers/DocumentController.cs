using CLDVPart3.Data;
using CLDVPart3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDVPart3.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public DocumentController(ApplicationDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload(Document model, IFormFile file)
        {
            // Ensure the file is provided
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please upload a PDF file.");
                return View(model);
            }

            var validMimeType = "application/pdf";
            var maxFileSize = 15 * 1024 * 1024; // 15MB

            // Validate the file type
            if (file.ContentType != validMimeType || !file.FileName.EndsWith(".pdf"))
            {
                ModelState.AddModelError("", "Only PDF files are allowed.");
                return View(model);
            }

            // Validate file size
            if (file.Length > maxFileSize)
            {
                ModelState.AddModelError("", "File size must be 15MB or smaller.");
                return View(model);
            }

            // Set the FilePath immediately after validation checks
            model.FilePath = Path.Combine("uploads", file.FileName);

            // Save the file to the uploads folder
            var filePath = Path.Combine(_environment.WebRootPath, model.FilePath);
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"File upload failed: {ex.Message}");
                return View(model);
            }

            // Set upload date
            model.UploadDate = DateTime.Now;

            // Save document details to the database
            _context.Documents.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.WebRootPath, document.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var documents = await _context.Documents.ToListAsync();
            return View(documents);
        }

        [AllowAnonymous]
        public IActionResult Download(int id)
        {
            var document = _context.Documents.Find(id);
            if (document == null) return NotFound();

            var filePath = Path.Combine(_environment.WebRootPath, document.FilePath);
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(document.FilePath));
        }
    }
}
