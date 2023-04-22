using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SchoolProject.Models;

namespace SchoolProject.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .IsRequired(false);

            modelBuilder.Entity<Student>()
                .Property(s => s.Hobbies)
                .IsRequired(false)
                .HasConversion(
                    hobbies => string.Join(',', hobbies),
                    hobbies => hobbies.Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray()
                );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Seed the database after it has been created
            optionsBuilder
                .UseInMemoryDatabase(databaseName: "SchoolDatabase")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .EnableSensitiveDataLogging();
        }
    }

}
