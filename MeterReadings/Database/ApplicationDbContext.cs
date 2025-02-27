namespace MeterReadings.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {       
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    public DbSet<Account> Accounts { get; set; }

    public DbSet<MeterReading> MeterReadings { get; set; }
}
