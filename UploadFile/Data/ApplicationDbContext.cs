using Microsoft.EntityFrameworkCore;
using UploadFile.Models;

namespace UploadFile.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<PdfModel> Pdfs { get; set; }
    }
}
