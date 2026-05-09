using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
    {
        public void Configure(EntityTypeBuilder<Professional> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => new { x.BusinessId, x.Name });

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasOne(x => x.Business)
                   .WithMany(b => b.Professionals)
                   .HasForeignKey(x => x.BusinessId);

            builder.HasQueryFilter(x => x.IsActive);
        }
    }
}
