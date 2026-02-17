using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class create_professionals_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_professional_businesses_business_id",
                table: "professional");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_professional",
                table: "professional");

            migrationBuilder.RenameTable(
                name: "professional",
                newName: "professionals");

            migrationBuilder.RenameIndex(
                name: "i_x_professional_business_id",
                table: "professionals",
                newName: "i_x_professionals_business_id");

            migrationBuilder.AddColumn<int>(
                name: "professional_id",
                table: "appointments",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "professionals",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_professionals",
                table: "professionals",
                column: "id");

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

            migrationBuilder.AddForeignKey(
                name: "f_k_professionals_businesses_business_id",
                table: "professionals",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__professionals_professional_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_professionals_businesses_business_id",
                table: "professionals");

            migrationBuilder.DropIndex(
                name: "i_x_appointments_professional_id",
                table: "appointments");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_professionals",
                table: "professionals");

            migrationBuilder.DropColumn(
                name: "professional_id",
                table: "appointments");

            migrationBuilder.RenameTable(
                name: "professionals",
                newName: "professional");

            migrationBuilder.RenameIndex(
                name: "i_x_professionals_business_id",
                table: "professional",
                newName: "i_x_professional_business_id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "professional",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "p_k_professional",
                table: "professional",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_professional_businesses_business_id",
                table: "professional",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
