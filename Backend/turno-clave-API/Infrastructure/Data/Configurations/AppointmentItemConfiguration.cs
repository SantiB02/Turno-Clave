using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class AppointmentItemConfiguration : IEntityTypeConfiguration<AppointmentItem>
    {
        public void Configure(EntityTypeBuilder<AppointmentItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.StartDateTime)
                   .IsRequired();

            builder.Property(x => x.EndDateTime)
                   .IsRequired();

            // Useful for availability checks
            builder.HasIndex(x =>
                new { x.ProfessionalId, x.StartDateTime });

            builder.HasOne(x => x.Service)
                   .WithMany(s => s.AppointmentItems)
                   .HasForeignKey(x => x.ServiceId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Professional)
                   .WithMany(p => p.AppointmentItems)
                   .HasForeignKey(x => x.ProfessionalId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
