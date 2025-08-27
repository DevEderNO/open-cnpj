using Domain;
using Domain.Models;
using Infra.ValueConverters;
using Infra.ValueObjects;
using Microsoft.EntityFrameworkCore;
using File = Domain.Models.File;

namespace Data;

public class AppDbContext: DbContext
{
    public DbSet<DCnae> Cnaes { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Estabelecimento> Estabelecimentos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DCnae>().Property(c => c.Cnae).HasConversion(VcCnae.Converter);
        modelBuilder.Entity<File>().Property(f => f.Size).HasConversion(VcFileSize.Converter);
        modelBuilder.Entity<Empresa>().Property(e => e.CnpjBasico).HasConversion(VcRaizCnpj.Converter);
        modelBuilder.Entity<Estabelecimento>().Property(e => e.CnpjBasico).HasConversion(VcRaizCnpj.Converter);
        modelBuilder.Entity<Estabelecimento>().Property(e => e.CnaeFiscalSecundaria).HasConversion(VcList<VoCnae>.Converter);
        modelBuilder.Entity<Estabelecimento>().Property(e => e.Email).HasConversion(VcEmail.Converter);
    }
}