using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class rename_users_businesses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_users_businesses_businesses_business_id",
                table: "users_businesses");

            migrationBuilder.DropForeignKey(
                name: "f_k_users_businesses_users_user_id",
                table: "users_businesses");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_users_businesses",
                table: "users_businesses");

            migrationBuilder.RenameTable(
                name: "users_businesses",
                newName: "user_businesses");

            migrationBuilder.RenameIndex(
                name: "i_x_users_businesses_business_id",
                table: "user_businesses",
                newName: "i_x_user_businesses_business_id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_user_businesses",
                table: "user_businesses",
                columns: new[] { "user_id", "business_id" });

            migrationBuilder.AddForeignKey(
                name: "f_k_user_businesses_businesses_business_id",
                table: "user_businesses",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_user_businesses_users_user_id",
                table: "user_businesses",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_user_businesses_businesses_business_id",
                table: "user_businesses");

            migrationBuilder.DropForeignKey(
                name: "f_k_user_businesses_users_user_id",
                table: "user_businesses");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_user_businesses",
                table: "user_businesses");

            migrationBuilder.RenameTable(
                name: "user_businesses",
                newName: "users_businesses");

            migrationBuilder.RenameIndex(
                name: "i_x_user_businesses_business_id",
                table: "users_businesses",
                newName: "i_x_users_businesses_business_id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_users_businesses",
                table: "users_businesses",
                columns: new[] { "user_id", "business_id" });

            migrationBuilder.AddForeignKey(
                name: "f_k_users_businesses_businesses_business_id",
                table: "users_businesses",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_users_businesses_users_user_id",
                table: "users_businesses",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
