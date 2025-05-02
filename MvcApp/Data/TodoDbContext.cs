using Microsoft.EntityFrameworkCore;
using MvcApp.Domain;

namespace MvcApp.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
        public DbSet<Domain.Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Store Tags as a comma-separated string
            modelBuilder.Entity<Todo>()
                .Property(t => t.Tags)
                .HasConversion(
                    v => string.Join(",", v),
                    v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(',', System.StringSplitOptions.None).ToList()
                );

            // Configure Attachments as owned entities
            modelBuilder.Entity<Todo>()
                .OwnsMany(t => t.Attachments, a =>
                {
                    a.WithOwner().HasForeignKey("TodoId");
                    a.Property<int>("Id");
                    a.HasKey("Id");
                });
        }
    }
}
