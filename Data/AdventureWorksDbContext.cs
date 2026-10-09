using JwtDemoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtDemoApi.Data
{
    public class AdventureWorksDbContext : DbContext
    {
        public AdventureWorksDbContext(
            DbContextOptions<AdventureWorksDbContext> options)
            : base(options)
        {             
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ApiUser> ApiUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiUser>(entity =>
            {
                entity.ToTable("ApiUsers", "dbo");

                entity.HasKey(x => x.Id);

                entity.HasIndex(x => x.Username)
                      .IsUnique();

                entity.Property(x => x.Username)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(x => x.PasswordHash)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(x => x.Role)
                      .HasMaxLength(50)
                      .IsRequired();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee", "HumanResources");

                entity.HasKey(e => e.BusinessEntityID);
            });
        }


    }
}
