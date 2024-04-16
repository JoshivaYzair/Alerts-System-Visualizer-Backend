using Alerts.Persistence.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts.Persistence.Data
{
    public class AlertsDbContext : DbContext
    {
        public AlertsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alert>().ToTable("Alert");
            modelBuilder.Entity<Application>().ToTable("Application");
            modelBuilder.Entity<User>().ToTable("User");

            modelBuilder.Entity<Alert>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Alert>()
                .HasOne(a => a.Application)
                .WithMany(ap => ap.Alerts)
                .HasForeignKey(a => a.ApplicationCode)  // Clave foránea de Alert
                .HasPrincipalKey(ap => ap.Code)  // Clave primaria de Application
                .OnDelete(DeleteBehavior.Restrict);  // Restricción ON DELETE RESTRICT
        }

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
