using Microsoft.EntityFrameworkCore;
using turno_clave_API.Domain.Entities;

namespace turno_clave_API.Infrastructure.Data.Configurations
{
    public class UserBusinessConfiguration : IEntityTypeConfiguration<UserBusiness>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserBusiness> builder)
        {
            builder.HasKey(ub => new { ub.UserId, ub.BusinessId });

            builder.HasOne(ub => ub.User)
                   .WithMany(u => u.UserBusinesses)
                   .HasForeignKey(ub => ub.UserId);

            builder.HasOne(ub => ub.Business)
                   .WithMany(b => b.UserBusinesses)
                   .HasForeignKey(ub => ub.BusinessId);
        }
    }
}
