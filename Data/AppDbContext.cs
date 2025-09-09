using Domain.Models;
using Infra.ValueConverters;
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
    public DbSet<EstabelecimentoCnaeSecundario> EstabelecimentoCnaesSecundarios { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração da entidade DCnae
        modelBuilder.Entity<DCnae>()
            .Property(c => c.Cnae)
            .HasConversion(VcCnae.Converter);
        
        // Configuração da entidade File
        modelBuilder.Entity<File>()
            .Property(f => f.Size)
            .HasConversion(VcFileSize.Converter);
        
        // Configuração da entidade Empresa
        modelBuilder.Entity<Empresa>()
            .Property(e => e.CnpjBasico)
            .HasConversion(VcRaizCnpj.Converter);

            // Configuração da entidade Estabelecimento
        modelBuilder.Entity<Estabelecimento>()
            .Property(e => e.Cnpj)
            .HasConversion(VcCnpj.Converter);
        
        // Configuração da entidade Estabelecimento
        modelBuilder.Entity<Estabelecimento>()
            .Property(e => e.CnpjBasico)
            .HasConversion(VcRaizCnpj.Converter);
        
        modelBuilder.Entity<Estabelecimento>()
            .Property(e => e.Email)
            .HasConversion(VcEmail.Converter!);
        
        modelBuilder.Entity<Estabelecimento>()
            .Property(e => e.CnaeFiscalPrincipalId)
            .HasConversion(VcCnae.Converter);
        
        modelBuilder.Entity<EstabelecimentoCnaeSecundario>()
            .HasOne(ec => ec.CnaeSecundario)
            .WithMany(c => c.EstabelecimentoCnaesSecundarios)
            .HasForeignKey(ec => ec.CnaeSecundarioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EstabelecimentoCnaeSecundario>()
            .HasOne(ec => ec.Estabelecimento)
            .WithMany(e => e.EstabelecimentoCnaesSecundarios)
            .HasForeignKey(ec => ec.EstabelecimentoId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<EstabelecimentoCnaeSecundario>()
            .Property(ec => ec.CnaeSecundarioId)
            .HasConversion(VcCnae.Converter);
    }
}