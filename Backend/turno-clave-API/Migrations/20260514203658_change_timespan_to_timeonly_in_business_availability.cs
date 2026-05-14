using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class change_timespan_to_timeonly_in_business_availability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE business_availabilities
                ALTER COLUMN start_time
                TYPE time without time zone
                USING start_time::time;
            """);

            migrationBuilder.Sql("""
                ALTER TABLE business_availabilities
                ALTER COLUMN end_time
                TYPE time without time zone
                USING end_time::time;
            """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE business_availabilities
                ALTER COLUMN start_time
                TYPE interval
                USING start_time::interval;
            """);

            migrationBuilder.Sql("""
                ALTER TABLE business_availabilities
                ALTER COLUMN end_time
                TYPE interval
                USING end_time::interval;
            """);
        }
    }
}
