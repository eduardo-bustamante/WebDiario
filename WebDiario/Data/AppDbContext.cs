using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebDiario.Models;


namespace WebDiario.Data;

// 2. Troque "DbContext" por "IdentityDbContext"
public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<EntradaDiario> EntradasDiario => Set<EntradaDiario>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Essencial para configurar as chaves do Identity
    }
}
