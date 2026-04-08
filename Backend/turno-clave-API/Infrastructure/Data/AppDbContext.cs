using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using turno_clave_API.Domain.Entities;
using turno_clave_API.Domain.Enums;
using System;

namespace turno_clave_API.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Business> Businesses { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBusiness> UserBusinesses { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<AvailabilityException> AvailabilityExceptions { get; set; }
        public DbSet<Professional> Professionals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from separate classes to keep DbContext clean
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Convert table, column and constraint names to snake_case to follow
            // PostgreSQL naming conventions (lowercase with underscores).
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Table name
                var tableName = entity.GetTableName();
                if (!string.IsNullOrEmpty(tableName))
                    entity.SetTableName(ToSnakeCase(tableName));

                // Columns
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.Name));
                }

                // Primary keys
                foreach (var key in entity.GetKeys())
                {
                    if (!string.IsNullOrEmpty(key.GetName()))
                        key.SetName(ToSnakeCase(key.GetName()));
                }

                // Foreign keys (constraints)
                foreach (var fk in entity.GetForeignKeys())
                {
                    if (!string.IsNullOrEmpty(fk.GetConstraintName()))
                        fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));
                }

                // Indexes
                foreach (var index in entity.GetIndexes())
                {
                    if (!string.IsNullOrEmpty(index.GetDatabaseName()))
                        index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
                }
            }
        }
        
        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var builder = new System.Text.StringBuilder();
            var previousCategory = System.Globalization.UnicodeCategory.OtherLetter;

            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (char.IsUpper(c))
                {
                    // Add underscore if not the first character and previous wasn't an underscore
                    if (i > 0 && previousCategory != System.Globalization.UnicodeCategory.SpaceSeparator)
                        builder.Append('_');

                    builder.Append(char.ToLowerInvariant(c));
                    previousCategory = System.Globalization.UnicodeCategory.UppercaseLetter;
                }
                else
                {
                    builder.Append(c);
                    previousCategory = char.IsWhiteSpace(c)
                        ? System.Globalization.UnicodeCategory.SpaceSeparator
                        : System.Globalization.UnicodeCategory.LowercaseLetter;
                }
            }

            return builder.ToString().Replace('-', '_');
        }
        
    }
}
