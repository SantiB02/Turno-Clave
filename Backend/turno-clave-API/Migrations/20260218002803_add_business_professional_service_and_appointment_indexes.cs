using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class add_business_professional_service_and_appointment_indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__professionals_professional_id",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "i_x_professionals_business_id",
                table: "professionals");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_professional_id",
                table: "appointments");

            migrationBuilder.AlterColumn<int>(
                name: "professional_id",
                table: "appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_professionals_business_id_name",
                table: "professionals",
                columns: new[] { "business_id", "name" });

            migrationBuilder.CreateIndex(
                name: "i_x_businesses_slug",
                table: "businesses",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_external_id",
                table: "appointments",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_professional_id_start_date_time",
                table: "appointments",
                columns: new[] { "professional_id", "start_date_time" });

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__professionals_professional_id",
                table: "appointments",
                column: "professional_id",
                principalTable: "professionals",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__professionals_professional_id",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "i_x_professionals_business_id_name",
                table: "professionals");

            migrationBuilder.DropIndex(
                name: "i_x_businesses_slug",
                table: "businesses");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_external_id",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_professional_id_start_date_time",
                table: "appointments");

            migrationBuilder.AlterColumn<int>(
                name: "professional_id",
                table: "appointments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "i_x_professionals_business_id",
                table: "professionals",
                column: "business_id");

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_professional_id",
                table: "appointments",
                column: "professional_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__professionals_professional_id",
                table: "appointments",
                column: "professional_id",
                principalTable: "professionals",
                principalColumn: "id");
        }
    }
}
