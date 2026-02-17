using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class fix_many_relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__businesses_business_id1",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__clients_client_id1",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__services_service_id1",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_availabilities__businesses_business_id1",
                table: "availabilities");

            migrationBuilder.DropForeignKey(
                name: "f_k_availability_exceptions__businesses_business_id1",
                table: "availability_exceptions");

            migrationBuilder.DropForeignKey(
                name: "f_k_clients_businesses_business_id1",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "f_k_services_businesses_business_id1",
                table: "services");

            migrationBuilder.DropIndex(
                name: "i_x_services_business_id1",
                table: "services");

            migrationBuilder.DropIndex(
                name: "i_x_clients_business_id1",
                table: "clients");

            migrationBuilder.DropIndex(
                name: "i_x_availability_exceptions_business_id1",
                table: "availability_exceptions");

            migrationBuilder.DropIndex(
                name: "i_x_availabilities_business_id1",
                table: "availabilities");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_business_id1",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_client_id1",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_service_id1",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "business_id1",
                table: "services");

            migrationBuilder.DropColumn(
                name: "business_id1",
                table: "clients");

            migrationBuilder.DropColumn(
                name: "business_id1",
                table: "availability_exceptions");

            migrationBuilder.DropColumn(
                name: "business_id1",
                table: "availabilities");

            migrationBuilder.DropColumn(
                name: "business_id1",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "client_id1",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "service_id1",
                table: "appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "business_id1",
                table: "services",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "business_id1",
                table: "clients",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "business_id1",
                table: "availability_exceptions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "business_id1",
                table: "availabilities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "business_id1",
                table: "appointments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "client_id1",
                table: "appointments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "service_id1",
                table: "appointments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_services_business_id1",
                table: "services",
                column: "business_id1");

            migrationBuilder.CreateIndex(
                name: "i_x_clients_business_id1",
                table: "clients",
                column: "business_id1");

            migrationBuilder.CreateIndex(
                name: "i_x_availability_exceptions_business_id1",
                table: "availability_exceptions",
                column: "business_id1");

            migrationBuilder.CreateIndex(
                name: "i_x_availabilities_business_id1",
                table: "availabilities",
                column: "business_id1");

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_business_id1",
                table: "appointments",
                column: "business_id1");

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_client_id1",
                table: "appointments",
                column: "client_id1");

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_service_id1",
                table: "appointments",
                column: "service_id1");

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__businesses_business_id1",
                table: "appointments",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__clients_client_id1",
                table: "appointments",
                column: "client_id1",
                principalTable: "clients",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__services_service_id1",
                table: "appointments",
                column: "service_id1",
                principalTable: "services",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_availabilities__businesses_business_id1",
                table: "availabilities",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_availability_exceptions__businesses_business_id1",
                table: "availability_exceptions",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_clients_businesses_business_id1",
                table: "clients",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_services_businesses_business_id1",
                table: "services",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");
        }
    }
}
