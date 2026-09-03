using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebDiario.Models;

namespace WebDiario.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Diario> Diarios { get; set; }
    public DbSet<Livro> Livros { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configurações e restrições adicionais da entidade Diario
        builder.Entity<Diario>(entity =>
        {
            entity.ToTable("Diarios");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Titulo).IsRequired().HasMaxLength(150);
            entity.Property(d => d.Conteudo).IsRequired();
            entity.Property(d => d.NivelHumor).IsRequired();
            entity.Property(d => d.UsuarioId).IsRequired();

            // Índice para agilizar consultas por usuário e data
            entity.HasIndex(d => new { d.UsuarioId, d.DataRegistro });
        });

        // Configurações e restrições adicionais da entidade Livro
        builder.Entity<Livro>(entity =>
        {
            entity.ToTable("Livros");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Titulo).IsRequired().HasMaxLength(150);
            entity.Property(l => l.Autor).HasMaxLength(100);
            entity.Property(l => l.Categoria).HasMaxLength(50);
            entity.Property(l => l.Status).IsRequired().HasMaxLength(20);
            entity.Property(l => l.UsuarioId).IsRequired();

            // Índice para agilizar buscas por usuário e título
            entity.HasIndex(l => new { l.UsuarioId, l.Titulo });
        });
    }
}