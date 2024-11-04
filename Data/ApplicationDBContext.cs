using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.Models.Account;
using Project.Models.Table;
using System.Text.Json;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserTable> UserTables { get; set; }
    public DbSet<TableColumn> TableColumns { get; set; }
    public DbSet<TableRow> TableRows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // UserTable and its relationships
        modelBuilder.Entity<UserTable>()
            .HasMany(ut => ut.Columns)
            .WithOne(utc => utc.UserTable)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserTable>()
            .HasMany(ut => ut.Rows)
            .WithOne(utr => utr.UserTable)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
               .HasMany(u => u.UserTables)
               .WithOne(t => t.User);
          

    


        base.OnModelCreating(modelBuilder);
    }
}
