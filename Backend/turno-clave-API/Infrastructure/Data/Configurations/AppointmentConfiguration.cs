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
            builder.Property(x => x.StartDateTime).IsRequired();
            builder.Property(x => x.EndDateTime).IsRequired();
            builder.Property(x => x.Status).HasConversion<string>().IsRequired();

            builder.HasOne(x => x.Business)
                   .WithMany(b => b.Appointments)
                   .HasForeignKey(x => x.BusinessId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Client)
                   .WithMany(cl => cl.Appointments)
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Service)
                   .WithMany(s => s.Appointments)
                   .HasForeignKey(x => x.ServiceId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}