using Microsoft.EntityFrameworkCore;
using POCSchedulerWorker;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public DbSet<ScheduledTask> ScheduledTasks { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=mssql-brandturijobs.live.stepstone.tools;Initial Catalog=Turijobs.master;User ID=tjweb;Password=g32qmYUId;TrustServerCertificate=True");
        }
    }
}
