using Microsoft.EntityFrameworkCore;

namespace DysonSphereBlueprints.Db;

public class BlueprintContext : DbContext
{
    public DbSet<Blueprint> Blueprints { get; set; }
    public DbSet<BlueprintImage> BlueprintImages { get; set; }

    public BlueprintContext(DbContextOptions<BlueprintContext> contextOptions) : base(contextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Blueprint>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<BlueprintImage>()
            .HasKey(s => s.ImageId);

        modelBuilder.Entity<Blueprint>()
            .HasMany(s => s.Images)
            .WithOne(s => s.Blueprint)
            .HasPrincipalKey(s => s.Id)
            .HasForeignKey(s => s.BlueprintId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}