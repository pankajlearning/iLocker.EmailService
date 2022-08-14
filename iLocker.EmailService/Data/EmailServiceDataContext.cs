using iLocker.EmailService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace iLocker.EmailService.Data;

/// <summary>
/// Represents an Email service data context
/// </summary>
public class EmailServiceDataContext : DbContext
{
    public EmailServiceDataContext(DbContextOptions<EmailServiceDataContext> dbContextOptions) 
        : base(dbContextOptions)
    {
         
    }
    public DbSet<EmailAccount> EmailAccounts { get; set; }
    public DbSet<QueuedEmail> QueuedEmails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmailServiceDataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
