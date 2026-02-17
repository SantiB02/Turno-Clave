using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StartTime)
                   .HasConversion(
                       v => v.ToTimeSpan(),
                       v => TimeOnly.FromTimeSpan(v))
                   .HasColumnType("time")
                   .IsRequired();

            builder.Property(x => x.EndTime)
                   .HasConversion(
                       v => v.ToTimeSpan(),
                       v => TimeOnly.FromTimeSpan(v))
                   .HasColumnType("time")
                   .IsRequired();

            builder.HasOne(x => x.Business)
                   .WithMany(b => b.Availabilities)
                   .HasForeignKey(x => x.BusinessId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}