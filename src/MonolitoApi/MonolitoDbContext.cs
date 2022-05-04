using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonolitoApi.Models;
// using MySql.Data.EntityFrameworkCore.Infraestructure;
using MySql.EntityFrameworkCore.Infrastructure;

namespace MonolitoApi
{
    public class MonolitoDbContext : DbContext
    {
        private readonly string _contextConnection;
        public DbSet<Person> Person { get; set; }
        public DbSet<Image> Image { get; set; }
        
        public MonolitoDbContext(DbContextOptions<MonolitoDbContext> options, IOptions<Settings.DbContextConnection> contextConnection)
            : base(options)
        {
            _contextConnection = contextConnection.Value.ConnectionString;
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Action<MySQLDbContextOptionsBuilder> mysqlDbContextOB = x => {
                var m = new MySQLDbContextOptionsBuilder(optionsBuilder);
            };

            optionsBuilder.UseMySQL(_contextConnection, mysqlDbContextOB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasIndex(e => e.Id);
                entity.Property(e => e.Name);
                entity.Property(e => e.Uuid);
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