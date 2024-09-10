using Events.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.Data.EntityTypeConfigurations;

public class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipant>
{
    public void Configure(EntityTypeBuilder<EventParticipant> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.LastName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.Email)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.DateOfBirth)
               .IsRequired();

        builder.Property(p => p.RegistrationDate)
               .IsRequired();

        builder.HasIndex(p => p.UserId).IsUnique(false);

        builder.HasIndex(p => new { p.UserId, p.EventId }).IsUnique();
    }
}
