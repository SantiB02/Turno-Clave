using Microsoft.EntityFrameworkCore;
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

        public DbSet<Business> Businesses { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<Availability> Availabilities { get; set; } = null!;
        public DbSet<AvailabilityException> AvailabilityExceptions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Business relations
            modelBuilder.Entity<Business>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired();
                b.Property(x => x.Email).IsRequired();
                b.Property(x => x.Phone).IsRequired();
                b.Property(x => x.Address).IsRequired();
                b.Property(x => x.City).IsRequired();
                b.Property(x => x.Country).IsRequired();
                b.Property(x => x.TimeZone).IsRequired();
            });

            // Service -> Business
            modelBuilder.Entity<Service>(s =>
            {
                s.HasKey(x => x.Id);
                s.Property(x => x.Name).IsRequired();
                s.HasOne(x => x.Business)
                 .WithMany()
                 .HasForeignKey(x => x.BusinessId)
                 .OnDelete(DeleteBehavior.Cascade);

                s.HasIndex(x => new { x.BusinessId, x.Name });
            });

            // Client -> Business
            modelBuilder.Entity<Client>(c =>
            {
                c.HasKey(x => x.Id);
                c.Property(x => x.Name).IsRequired();
                c.Property(x => x.Email).IsRequired();
                c.Property(x => x.Phone).IsRequired();
                c.HasOne(x => x.Business)
                 .WithMany()
                 .HasForeignKey(x => x.BusinessId)
                 .OnDelete(DeleteBehavior.Cascade);

                c.HasIndex(x => new { x.BusinessId, x.Email });
            });

            // User
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(x => x.Id);
                u.Property(x => x.Name).IsRequired();
                u.Property(x => x.Email).IsRequired();
                u.Property(x => x.PasswordHash).IsRequired();
                // store role as string for readability
                u.Property(x => x.Role)
                 .HasConversion<string>()
                 .IsRequired();

                u.HasIndex(x => x.Email).IsUnique();
            });

            // Appointment -> Business, Client, Service
            modelBuilder.Entity<Appointment>(a =>
            {
                a.HasKey(x => x.Id);
                a.Property(x => x.StartDateTime).IsRequired();
                a.Property(x => x.EndDateTime).IsRequired();
                a.Property(x => x.Status)
                 .HasConversion<string>()
                 .IsRequired();

                a.HasOne(x => x.Business)
                 .WithMany()
                 .HasForeignKey(x => x.BusinessId)
                 .OnDelete(DeleteBehavior.Cascade);

                a.HasOne(x => x.Client)
                 .WithMany()
                 .HasForeignKey(x => x.ClientId)
                 .OnDelete(DeleteBehavior.Cascade);

                a.HasOne(x => x.Service)
                 .WithMany()
                 .HasForeignKey(x => x.ServiceId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Availability -> Business
            modelBuilder.Entity<Availability>(av =>
            {
                av.HasKey(x => x.Id);
                // TimeOnly mapping
                av.Property(x => x.StartTime)
                  .HasConversion(
                      v => v.ToTimeSpan(),
                      v => TimeOnly.FromTimeSpan(v))
                  .HasColumnType("time")
                  .IsRequired();

                av.Property(x => x.EndTime)
                  .HasConversion(
                      v => v.ToTimeSpan(),
                      v => TimeOnly.FromTimeSpan(v))
                  .HasColumnType("time")
                  .IsRequired();

                av.HasOne(x => x.Business)
                  .WithMany()
                  .HasForeignKey(x => x.BusinessId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            // AvailabilityException -> Business
            modelBuilder.Entity<AvailabilityException>(ae =>
            {
                ae.HasKey(x => x.Id);
                ae.Property(x => x.StartDateTime).IsRequired();
                ae.Property(x => x.EndDateTime).IsRequired();
                ae.Property(x => x.Type).HasConversion<string>().IsRequired();

                ae.HasOne(x => x.Business)
                  .WithMany()
                  .HasForeignKey(x => x.BusinessId)
                  .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
