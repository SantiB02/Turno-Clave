using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class add_some_external_ids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "external_id",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "external_id",
                table: "businesses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "external_id",
                table: "appointments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "external_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "external_id",
                table: "businesses");

            migrationBuilder.DropColumn(
                name: "external_id",
                table: "appointments");
        }
    }
}
