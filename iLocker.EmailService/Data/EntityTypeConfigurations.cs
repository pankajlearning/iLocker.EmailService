using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using iLocker.EmailService.Data.Entities;

namespace iLocker.EmailService.Data
{
    public class EmailAccountEntityTypeConfiguration : IEntityTypeConfiguration<EmailAccount>
    {
        public void Configure(EntityTypeBuilder<EmailAccount> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(x => x.Email)
             .HasMaxLength(255)
             .IsRequired();

            builder.Property(x => x.Host)
             .HasMaxLength(255)
             .IsRequired();

            builder.Property(x => x.Username)
            .HasMaxLength(255)
            .IsRequired();

            builder.Property(x => x.Password)
            .HasMaxLength(255)
            .IsRequired();

            builder.Property(x => x.DisplayName)
           .HasMaxLength(255)
           .IsRequired();

            builder.Property(x => x.Port)
           .HasMaxLength(10)
           .IsRequired();
        }
    }

    public class QueuedEmailEntityTypeConfiguration : IEntityTypeConfiguration<QueuedEmail>
    {
        public void Configure(EntityTypeBuilder<QueuedEmail> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(x => x.Body)
             .IsRequired();

            builder.Property(x => x.Subject)
             .IsRequired();

            builder.Property(x => x.From)
            .HasMaxLength(255)
            .IsRequired();

            builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

            builder.Property(x => x.EmailAccountId)
           .IsRequired();

            builder.Property(x => x.Priority)
            .HasMaxLength(3)
            .IsRequired();
        }
    }
}
