using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Domain.Enums;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class BusinessConfiguration : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Slug).IsUnique();
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Phone).IsRequired();

            var paymentMethodsProperty = builder.Property(x => x.PaymentMethods)
                .HasColumnType("text[]")
                .HasConversion(
                    v => v.Select(x => x.ToString()).ToArray(),
                    v => v.Select(x => Enum.Parse<PaymentMethod>(x)).ToList()
                )
                .HasDefaultValueSql("'{}'");

            paymentMethodsProperty.Metadata.SetValueComparer(
                new ValueComparer<List<PaymentMethod>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                )
            );

            builder.Property(x => x.Address).IsRequired();
            builder.Property(x => x.City).IsRequired();
            builder.Property(x => x.Country).IsRequired();
            builder.Property(x => x.TimeZone).IsRequired();

            builder.HasQueryFilter(x => x.IsActive);
        }
    }
}