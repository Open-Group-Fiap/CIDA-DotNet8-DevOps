using CIDA.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cida.Data;

public class CidaDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Arquivo> Arquivos { get; set; }
    public DbSet<Resumo> Resumos { get; set; }
    public DbSet<Insight> Insights { get; set; } 
    public DbSet<Autenticacao> Autenticacoes { get; set; }
    
    public CidaDbContext(DbContextOptions<CidaDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var converter = new ValueConverter<Status, string>(
            v => v.ToString(),
            v => (Status)Enum.Parse(typeof(Status), v));
        
        modelBuilder.Entity<Usuario>()
            .Property(u => u.Status)
            .HasConversion(converter);
        
        var converter2 = new ValueConverter<TipoDocumento, string>(
            v => v.ToString(),
            v => (TipoDocumento)Enum.Parse(typeof(TipoDocumento), v));

        modelBuilder.Entity<Usuario>()
            .Property(u => u.TipoDocumento)
            .HasConversion(converter2);
        
    }
}