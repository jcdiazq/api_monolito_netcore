using Microsoft.EntityFrameworkCore;
using MonolitoApi.Models;
// using MySql.Data.EntityFrameworkCore.Infraestructure;
using MySql.EntityFrameworkCore.Infrastructure;

namespace MonolitoApi
{
    public class MonolitoDbContext : DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<Image> Image { get; set; }
        
        public MonolitoDbContext(DbContextOptions<MonolitoDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Action<MySQLDbContextOptionsBuilder> mysqlDbContextOB = x => {
                var m = new MySQLDbContextOptionsBuilder(optionsBuilder);
            };

            optionsBuilder.UseMySQL("server=localhost;database=db;user=user;password=password", mysqlDbContextOB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Name);
                entity.Property(e => e.UUID);
                entity.HasOne(i => i.Person)
                .WithOne(p => p.Image)
                .HasForeignKey<Image>(f => f.PersonId);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.FirtName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
            });
        }      
    }
}