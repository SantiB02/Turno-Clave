using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class rename_day_to_day_of_week_in_business_availability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "day",
                table: "business_availabilities",
                newName: "day_of_week");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "day_of_week",
                table: "business_availabilities",
                newName: "day");
        }
    }
}
