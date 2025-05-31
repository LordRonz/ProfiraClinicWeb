using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;

namespace ProfiraClinicWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly string _imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public ImagesController()
        {
            if (!Directory.Exists(_imageFolder))
            {
                Directory.CreateDirectory(_imageFolder);
            }
        }

        // POST: api/images/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (file.Length > MaxFileSize)
                return BadRequest("File size exceeds the maximum limit of 5MB.");

            // Get original file extension
            var fileExtension = Path.GetExtension(file.FileName);

            // Generate random filename with original extension
            var randomFileName = $"{Guid.NewGuid()}{fileExtension}";

            // Sanitize filename (shouldn’t really be needed for a GUID, but good practice!)
            var safeFileName = SanitizeFileName(randomFileName);

            var filePath = Path.Combine(_imageFolder, safeFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { message = "Image uploaded successfully!", fileName = safeFileName });
        }

        // GET: api/images/{filename}
        [HttpGet("{filename}")]
        public IActionResult GetImage(string filename)
        {
            var safeFileName = SanitizeFileName(filename);
            var filePath = Path.Combine(_imageFolder, safeFileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not found.");

            var mimeType = GetMimeType(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, mimeType);
        }

        // Helper method to get the MIME type based on file extension
        private static string GetMimeType(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                _ => "application/octet-stream",
            };
        }

        // Helper method to sanitize filename
        private static string SanitizeFileName(string fileName)
        {
            // Remove any path traversal attempts
            fileName = Path.GetFileName(fileName);

            // Optionally, remove special characters
            fileName = Regex.Replace(fileName, @"[^a-zA-Z0-9_\.\-]", "_");

            return fileName;
        }
    }
}
