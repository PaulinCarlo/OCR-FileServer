using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OCR_Core.Models;

namespace OCR_Core.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<ScannedDocument> ScannedDocuments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
