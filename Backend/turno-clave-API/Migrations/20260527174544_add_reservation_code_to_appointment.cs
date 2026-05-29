using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class add_reservation_code_to_appointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "reservation_code",
                table: "appointments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_reservation_code",
                table: "appointments",
                column: "reservation_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "i_x_appointments_reservation_code",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "reservation_code",
                table: "appointments");
        }
    }
}
