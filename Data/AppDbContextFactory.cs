using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data;

public class AppDbContextFactory: IDesignTimeDbContextFactory<AppDbContext>
{
  public AppDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    optionsBuilder.UseNpgsql("Host=localhost;Database=OpenCnpj;Username=postgres;Password=postgres");
    return new AppDbContext(optionsBuilder.Options);
  }
}
