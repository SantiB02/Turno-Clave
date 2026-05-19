using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class create_appointment_item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__professionals_professional_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__services_service_id",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_professional_id_start_date_time",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_service_id",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "professional_id",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "service_id",
                table: "appointments");

            migrationBuilder.CreateTable(
                name: "appointment_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    appointment_id = table.Column<int>(type: "integer", nullable: false),
                    service_id = table.Column<int>(type: "integer", nullable: false),
                    professional_id = table.Column<int>(type: "integer", nullable: false),
                    start_date_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    end_date_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_appointment_items", x => x.id);
                    table.ForeignKey(
                        name: "f_k_appointment_items__professionals_professional_id",
                        column: x => x.professional_id,
                        principalTable: "professionals",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_appointment_items__services_service_id",
                        column: x => x.service_id,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_appointment_items_appointments_appointment_id",
                        column: x => x.appointment_id,
                        principalTable: "appointments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_appointment_items_appointment_id",
                table: "appointment_items",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "i_x_appointment_items_professional_id_start_date_time",
                table: "appointment_items",
                columns: new[] { "professional_id", "start_date_time" });

            migrationBuilder.CreateIndex(
                name: "i_x_appointment_items_service_id",
                table: "appointment_items",
                column: "service_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appointment_items");

            migrationBuilder.AddColumn<int>(
                name: "professional_id",
                table: "appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "service_id",
                table: "appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_professional_id_start_date_time",
                table: "appointments",
                columns: new[] { "professional_id", "start_date_time" });

            migrationBuilder.CreateIndex(
                name: "i_x_appointments_service_id",
                table: "appointments",
                column: "service_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__professionals_professional_id",
                table: "appointments",
                column: "professional_id",
                principalTable: "professionals",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__services_service_id",
                table: "appointments",
                column: "service_id",
                principalTable: "services",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
