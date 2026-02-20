using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class make_availability_dependent_on_professional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_availabilities__businesses_business_id",
                table: "availabilities");

            migrationBuilder.RenameColumn(
                name: "business_id",
                table: "availabilities",
                newName: "professional_id");

            migrationBuilder.RenameIndex(
                name: "i_x_availabilities_business_id",
                table: "availabilities",
                newName: "i_x_availabilities_professional_id");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "start_date_time",
                table: "availability_exceptions",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "end_date_time",
                table: "availability_exceptions",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<DateOnly>(
                name: "date",
                table: "availability_exceptions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "professional_id",
                table: "availability_exceptions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_availability_exceptions_professional_id",
                table: "availability_exceptions",
                column: "professional_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_availabilities__professionals_professional_id",
                table: "availabilities",
                column: "professional_id",
                principalTable: "professionals",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_availability_exceptions__professionals_professional_id",
                table: "availability_exceptions",
                column: "professional_id",
                principalTable: "professionals",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_availabilities__professionals_professional_id",
                table: "availabilities");

            migrationBuilder.DropForeignKey(
                name: "f_k_availability_exceptions__professionals_professional_id",
                table: "availability_exceptions");

            migrationBuilder.DropIndex(
                name: "i_x_availability_exceptions_professional_id",
                table: "availability_exceptions");

            migrationBuilder.DropColumn(
                name: "date",
                table: "availability_exceptions");

            migrationBuilder.DropColumn(
                name: "professional_id",
                table: "availability_exceptions");

            migrationBuilder.RenameColumn(
                name: "professional_id",
                table: "availabilities",
                newName: "business_id");

            migrationBuilder.RenameIndex(
                name: "i_x_availabilities_professional_id",
                table: "availabilities",
                newName: "i_x_availabilities_business_id");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "start_date_time",
                table: "availability_exceptions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "end_date_time",
                table: "availability_exceptions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AddForeignKey(
                name: "f_k_availabilities__businesses_business_id",
                table: "availabilities",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
