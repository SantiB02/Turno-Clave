using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class rename_availability_to_professional_availability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "p_k_availabilities",
                table: "availabilities");

            migrationBuilder.DropForeignKey(
                name: "f_k_availabilities__professionals_professional_id",
                table: "availabilities");

            migrationBuilder.RenameTable(
                name: "availabilities",
                newName: "professional_availabilities");

            migrationBuilder.RenameIndex(
                name: "i_x_availabilities_professional_id",
                table: "professional_availabilities",
                newName: "i_x_professional_availabilities_professional_id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_professional_availabilities",
                table: "professional_availabilities",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_professional_availabilities_professionals_professional_id",
                table: "professional_availabilities",
                column: "professional_id",
                principalTable: "professionals",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "p_k_professional_availabilities",
                table: "professional_availabilities");

            migrationBuilder.DropForeignKey(
                name: "f_k_professional_availabilities_professionals_professional_id",
                table: "professional_availabilities");

            migrationBuilder.RenameTable(
                name: "professional_availabilities",
                newName: "availabilities");

            migrationBuilder.RenameIndex(
                name: "i_x_professional_availabilities_professional_id",
                table: "availabilities",
                newName: "i_x_availabilities_professional_id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_availabilities",
                table: "availabilities",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_availabilities__professionals_professional_id",
                table: "availabilities",
                column: "professional_id",
                principalTable: "professionals",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
        
    }
}
