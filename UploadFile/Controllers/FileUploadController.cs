using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UploadFile.Data;
using UploadFile.Models;

namespace UploadFile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FileUploadController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            string message = "";
            try
            {
                if (file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        // Create a new FileModel instance and populate it with file information
                        var fileModel = new PdfModel
                        {
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            Content = memoryStream.ToArray()
                        };

                        // Add the FileModel instance to the context and save changes to the database
                        _context.Pdfs.Add(fileModel);
                        await _context.SaveChangesAsync();

                        message = "Your File is uploaded successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Sorry, something went wrong. Please try again.";
            }

            return Ok(new { message = message });
        }
        [HttpGet("pdfs")]
        public async Task<IActionResult> GetPdfs()
        {
            try
            {
                var pdfFiles = await _context.Pdfs.ToListAsync();
                if(pdfFiles.Count == 0)
                {
                    return NotFound("No pdf found");
                }

                var pdfFileData = pdfFiles.Select(file => new
                {
                    FileName = file.FileName,
                    Content = Convert.ToBase64String(file.Content)
                }).ToList();
                return Ok(pdfFileData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
