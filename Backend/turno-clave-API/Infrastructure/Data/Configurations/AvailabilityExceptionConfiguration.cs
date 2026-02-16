using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class AvailabilityExceptionConfiguration : IEntityTypeConfiguration<AvailabilityException>
    {
        public void Configure(EntityTypeBuilder<AvailabilityException> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StartDateTime).IsRequired();
            builder.Property(x => x.EndDateTime).IsRequired();
            builder.Property(x => x.Type).HasConversion<string>().IsRequired();

            builder.HasOne(x => x.Business)
                   .WithMany()
                   .HasForeignKey(x => x.BusinessId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}