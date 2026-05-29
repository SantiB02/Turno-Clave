using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.ExternalId)
                   .IsUnique();

            builder.HasIndex(x => x.ReservationCode)
                   .IsUnique();

            builder.Property(x => x.StartDateTime)
                   .IsRequired();

            builder.Property(x => x.EndDateTime)
                   .IsRequired();

            builder.Property(x => x.Status)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.Property(x => x.UpdatedAt)
                   .IsRequired();

            builder.HasOne(x => x.Business)
                   .WithMany(b => b.Appointments)
                   .HasForeignKey(x => x.BusinessId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Client)
                   .WithMany(c => c.Appointments)
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Items)
                   .WithOne(i => i.Appointment)
                   .HasForeignKey(i => i.AppointmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}