using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class BusinessAvailabilityConfiguration : IEntityTypeConfiguration<BusinessAvailability>
    {
        public void Configure(EntityTypeBuilder<BusinessAvailability> builder)
        {
            builder.ToTable("business_availabilities");

            builder.HasKey(x => x.Id).HasName("p_k_business_availabilities");

            builder.Property(x => x.ExternalId).IsRequired().HasColumnName("external_id");
            builder.Property(x => x.BusinessId).IsRequired().HasColumnName("business_id");
            builder.Property(x => x.Day).IsRequired().HasColumnName("day");
            builder.Property(x => x.StartTime).IsRequired().HasColumnName("start_time");
            builder.Property(x => x.EndTime).IsRequired().HasColumnName("end_time");
            builder.Property(x => x.IsActive).IsRequired().HasColumnName("is_active");
            builder.Property(x => x.CreatedAt).IsRequired().HasColumnName("created_at");
            builder.Property(x => x.UpdatedAt).IsRequired().HasColumnName("updated_at");

            builder.HasIndex(x => x.BusinessId).HasDatabaseName("i_x_business_availabilities_business_id");

            builder.HasOne(x => x.Business)
                .WithMany(b => b.BusinessAvailabilities)
                .HasForeignKey(x => x.BusinessId)
                .HasConstraintName("f_k_business_availabilities_businesses_business_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
