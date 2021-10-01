using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infra
{
    public class Context : DbContext
    {

        public Context(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyEntity>().HasData(
                new MyEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "foo",
                    Value = "bar"
                });
        }

        public DbSet<MyEntity> MyEntities { get; set; }
    }
}
