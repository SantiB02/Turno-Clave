using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class ProfessionalServiceConfiguration : IEntityTypeConfiguration<ProfessionalService>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProfessionalService> builder)
        {
            builder.HasKey(ps => new { ps.ProfessionalId, ps.ServiceId });

            builder.HasOne(ps => ps.Professional)
                   .WithMany(p => p.ProfessionalServices)
                   .HasForeignKey(ps => ps.ProfessionalId);

            builder.HasOne(ps => ps.Service)
                   .WithMany(s => s.ProfessionalServices)
                   .HasForeignKey(ps => ps.ServiceId);

            builder.HasQueryFilter(x => x.Professional.IsActive);
        }
    }
}
