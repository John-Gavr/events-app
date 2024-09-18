using Events.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.Data.EntityTypeConfigurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(e => e.Description)
               .HasMaxLength(500);

        builder.Property(e => e.EventDateTime)
               .IsRequired();

        builder.Property(e => e.Location)
               .HasMaxLength(200);

        builder.Property(e => e.Category)
               .HasMaxLength(50);

        builder.Property(e => e.MaxParticipants)
               .IsRequired();

        builder.HasMany(e => e.Participants)
               .WithOne()
               .HasForeignKey(p => p.Id)
               .OnDelete(DeleteBehavior.Cascade);


    }
}
