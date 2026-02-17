using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace turno_clave_API.Migrations
{
    /// <inheritdoc />
    public partial class rename_to_snake_case : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Businesses_BusinessId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Businesses_BusinessId1",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clients_ClientId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Clients_ClientId1",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceId1",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Businesses_BusinessId",
                table: "Availabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Businesses_BusinessId1",
                table: "Availabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilityExceptions_Businesses_BusinessId",
                table: "AvailabilityExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AvailabilityExceptions_Businesses_BusinessId1",
                table: "AvailabilityExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Businesses_BusinessId",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Businesses_BusinessId1",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Businesses_BusinessId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Businesses_BusinessId1",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Businesses_BusinessId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Services",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clients",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Businesses",
                table: "Businesses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailabilityExceptions",
                table: "AvailabilityExceptions");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Services",
                newName: "services");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "clients");

            migrationBuilder.RenameTable(
                name: "Businesses",
                newName: "businesses");

            migrationBuilder.RenameTable(
                name: "Availabilities",
                newName: "availabilities");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "appointments");

            migrationBuilder.RenameTable(
                name: "AvailabilityExceptions",
                newName: "availability_exceptions");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "users",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "users",
                newName: "business_id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "users",
                newName: "i_x_users_email");

            migrationBuilder.RenameIndex(
                name: "IX_Users_BusinessId",
                table: "users",
                newName: "i_x_users_business_id");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "services",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "services",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "services",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "services",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "services",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "services",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "DurationMinutes",
                table: "services",
                newName: "duration_minutes");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "services",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BusinessId1",
                table: "services",
                newName: "business_id1");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "services",
                newName: "business_id");

            migrationBuilder.RenameIndex(
                name: "IX_Services_BusinessId1",
                table: "services",
                newName: "i_x_services_business_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Services_BusinessId_Name",
                table: "services",
                newName: "i_x_services_business_id_name");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "clients",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "clients",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "clients",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "clients",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "clients",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "clients",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "clients",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BusinessId1",
                table: "clients",
                newName: "business_id1");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "clients",
                newName: "business_id");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_BusinessId1",
                table: "clients",
                newName: "i_x_clients_business_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Clients_BusinessId_Email",
                table: "clients",
                newName: "i_x_clients_business_id_email");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "businesses",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "businesses",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "businesses",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "businesses",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "businesses",
                newName: "country");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "businesses",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "businesses",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "businesses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "businesses",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TimeZone",
                table: "businesses",
                newName: "time_zone");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "businesses",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "businesses",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "availabilities",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "availabilities",
                newName: "start_time");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "availabilities",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "availabilities",
                newName: "end_time");

            migrationBuilder.RenameColumn(
                name: "DayOfWeek",
                table: "availabilities",
                newName: "day_of_week");

            migrationBuilder.RenameColumn(
                name: "BusinessId1",
                table: "availabilities",
                newName: "business_id1");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "availabilities",
                newName: "business_id");

            migrationBuilder.RenameIndex(
                name: "IX_Availabilities_BusinessId1",
                table: "availabilities",
                newName: "i_x_availabilities_business_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Availabilities_BusinessId",
                table: "availabilities",
                newName: "i_x_availabilities_business_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "appointments",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "appointments",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "appointments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "appointments",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "appointments",
                newName: "start_date_time");

            migrationBuilder.RenameColumn(
                name: "ServiceId1",
                table: "appointments",
                newName: "service_id1");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "appointments",
                newName: "service_id");

            migrationBuilder.RenameColumn(
                name: "EndDateTime",
                table: "appointments",
                newName: "end_date_time");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "appointments",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ClientId1",
                table: "appointments",
                newName: "client_id1");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "appointments",
                newName: "client_id");

            migrationBuilder.RenameColumn(
                name: "BusinessId1",
                table: "appointments",
                newName: "business_id1");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "appointments",
                newName: "business_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ServiceId1",
                table: "appointments",
                newName: "i_x_appointments_service_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ServiceId",
                table: "appointments",
                newName: "i_x_appointments_service_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ClientId1",
                table: "appointments",
                newName: "i_x_appointments_client_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_ClientId",
                table: "appointments",
                newName: "i_x_appointments_client_id");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_BusinessId1",
                table: "appointments",
                newName: "i_x_appointments_business_id1");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_BusinessId",
                table: "appointments",
                newName: "i_x_appointments_business_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "availability_exceptions",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "availability_exceptions",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "availability_exceptions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "availability_exceptions",
                newName: "start_date_time");

            migrationBuilder.RenameColumn(
                name: "EndDateTime",
                table: "availability_exceptions",
                newName: "end_date_time");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "availability_exceptions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BusinessId1",
                table: "availability_exceptions",
                newName: "business_id1");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "availability_exceptions",
                newName: "business_id");

            migrationBuilder.RenameIndex(
                name: "IX_AvailabilityExceptions_BusinessId1",
                table: "availability_exceptions",
                newName: "i_x_availability_exceptions_business_id1");

            migrationBuilder.RenameIndex(
                name: "IX_AvailabilityExceptions_BusinessId",
                table: "availability_exceptions",
                newName: "i_x_availability_exceptions_business_id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_services",
                table: "services",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_clients",
                table: "clients",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_businesses",
                table: "businesses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_availabilities",
                table: "availabilities",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_appointments",
                table: "appointments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_availability_exceptions",
                table: "availability_exceptions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__businesses_business_id",
                table: "appointments",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__businesses_business_id1",
                table: "appointments",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__clients_client_id",
                table: "appointments",
                column: "client_id",
                principalTable: "clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__clients_client_id1",
                table: "appointments",
                column: "client_id1",
                principalTable: "clients",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__services_service_id",
                table: "appointments",
                column: "service_id",
                principalTable: "services",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "f_k_appointments__services_service_id1",
                table: "appointments",
                column: "service_id1",
                principalTable: "services",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_availabilities__businesses_business_id",
                table: "availabilities",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_availabilities__businesses_business_id1",
                table: "availabilities",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_availability_exceptions__businesses_business_id",
                table: "availability_exceptions",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_availability_exceptions__businesses_business_id1",
                table: "availability_exceptions",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_clients_businesses_business_id",
                table: "clients",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_clients_businesses_business_id1",
                table: "clients",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_services_businesses_business_id",
                table: "services",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "f_k_services_businesses_business_id1",
                table: "services",
                column: "business_id1",
                principalTable: "businesses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_users_businesses_business_id",
                table: "users",
                column: "business_id",
                principalTable: "businesses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__businesses_business_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__businesses_business_id1",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__clients_client_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__clients_client_id1",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__services_service_id",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_appointments__services_service_id1",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "f_k_availabilities__businesses_business_id",
                table: "availabilities");

            migrationBuilder.DropForeignKey(
                name: "f_k_availabilities__businesses_business_id1",
                table: "availabilities");

            migrationBuilder.DropForeignKey(
                name: "f_k_availability_exceptions__businesses_business_id",
                table: "availability_exceptions");

            migrationBuilder.DropForeignKey(
                name: "f_k_availability_exceptions__businesses_business_id1",
                table: "availability_exceptions");

            migrationBuilder.DropForeignKey(
                name: "f_k_clients_businesses_business_id",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "f_k_clients_businesses_business_id1",
                table: "clients");

            migrationBuilder.DropForeignKey(
                name: "f_k_services_businesses_business_id",
                table: "services");

            migrationBuilder.DropForeignKey(
                name: "f_k_services_businesses_business_id1",
                table: "services");

            migrationBuilder.DropForeignKey(
                name: "f_k_users_businesses_business_id",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_services",
                table: "services");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_clients",
                table: "clients");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_businesses",
                table: "businesses");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_availabilities",
                table: "availabilities");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_appointments",
                table: "appointments");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_availability_exceptions",
                table: "availability_exceptions");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "services",
                newName: "Services");

            migrationBuilder.RenameTable(
                name: "clients",
                newName: "Clients");

            migrationBuilder.RenameTable(
                name: "businesses",
                newName: "Businesses");

            migrationBuilder.RenameTable(
                name: "availabilities",
                newName: "Availabilities");

            migrationBuilder.RenameTable(
                name: "appointments",
                newName: "Appointments");

            migrationBuilder.RenameTable(
                name: "availability_exceptions",
                newName: "AvailabilityExceptions");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Users",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "business_id",
                table: "Users",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "i_x_users_email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "i_x_users_business_id",
                table: "Users",
                newName: "IX_Users_BusinessId");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Services",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Services",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Services",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Services",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Services",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Services",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "duration_minutes",
                table: "Services",
                newName: "DurationMinutes");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Services",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "business_id1",
                table: "Services",
                newName: "BusinessId1");

            migrationBuilder.RenameColumn(
                name: "business_id",
                table: "Services",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "i_x_services_business_id1",
                table: "Services",
                newName: "IX_Services_BusinessId1");

            migrationBuilder.RenameIndex(
                name: "i_x_services_business_id_name",
                table: "Services",
                newName: "IX_Services_BusinessId_Name");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Clients",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Clients",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Clients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Clients",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Clients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Clients",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Clients",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "business_id1",
                table: "Clients",
                newName: "BusinessId1");

            migrationBuilder.RenameColumn(
                name: "business_id",
                table: "Clients",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "i_x_clients_business_id1",
                table: "Clients",
                newName: "IX_Clients_BusinessId1");

            migrationBuilder.RenameIndex(
                name: "i_x_clients_business_id_email",
                table: "Clients",
                newName: "IX_Clients_BusinessId_Email");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Businesses",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Businesses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Businesses",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Businesses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "country",
                table: "Businesses",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "Businesses",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Businesses",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Businesses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Businesses",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "time_zone",
                table: "Businesses",
                newName: "TimeZone");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Businesses",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Businesses",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Availabilities",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "start_time",
                table: "Availabilities",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Availabilities",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "end_time",
                table: "Availabilities",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "day_of_week",
                table: "Availabilities",
                newName: "DayOfWeek");

            migrationBuilder.RenameColumn(
                name: "business_id1",
                table: "Availabilities",
                newName: "BusinessId1");

            migrationBuilder.RenameColumn(
                name: "business_id",
                table: "Availabilities",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "i_x_availabilities_business_id1",
                table: "Availabilities",
                newName: "IX_Availabilities_BusinessId1");

            migrationBuilder.RenameIndex(
                name: "i_x_availabilities_business_id",
                table: "Availabilities",
                newName: "IX_Availabilities_BusinessId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Appointments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Appointments",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Appointments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Appointments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "start_date_time",
                table: "Appointments",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "service_id1",
                table: "Appointments",
                newName: "ServiceId1");

            migrationBuilder.RenameColumn(
                name: "service_id",
                table: "Appointments",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "end_date_time",
                table: "Appointments",
                newName: "EndDateTime");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Appointments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "client_id1",
                table: "Appointments",
                newName: "ClientId1");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Appointments",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "business_id1",
                table: "Appointments",
                newName: "BusinessId1");

            migrationBuilder.RenameColumn(
                name: "business_id",
                table: "Appointments",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "i_x_appointments_service_id1",
                table: "Appointments",
                newName: "IX_Appointments_ServiceId1");

            migrationBuilder.RenameIndex(
                name: "i_x_appointments_service_id",
                table: "Appointments",
                newName: "IX_Appointments_ServiceId");

            migrationBuilder.RenameIndex(
                name: "i_x_appointments_client_id1",
                table: "Appointments",
                newName: "IX_Appointments_ClientId1");

            migrationBuilder.RenameIndex(
                name: "i_x_appointments_client_id",
                table: "Appointments",
                newName: "IX_Appointments_ClientId");

            migrationBuilder.RenameIndex(
                name: "i_x_appointments_business_id1",
                table: "Appointments",
                newName: "IX_Appointments_BusinessId1");

            migrationBuilder.RenameIndex(
                name: "i_x_appointments_business_id",
                table: "Appointments",
                newName: "IX_Appointments_BusinessId");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "AvailabilityExceptions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "AvailabilityExceptions",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AvailabilityExceptions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "start_date_time",
                table: "AvailabilityExceptions",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "end_date_time",
                table: "AvailabilityExceptions",
                newName: "EndDateTime");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "AvailabilityExceptions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "business_id1",
                table: "AvailabilityExceptions",
                newName: "BusinessId1");

            migrationBuilder.RenameColumn(
                name: "business_id",
                table: "AvailabilityExceptions",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "i_x_availability_exceptions_business_id1",
                table: "AvailabilityExceptions",
                newName: "IX_AvailabilityExceptions_BusinessId1");

            migrationBuilder.RenameIndex(
                name: "i_x_availability_exceptions_business_id",
                table: "AvailabilityExceptions",
                newName: "IX_AvailabilityExceptions_BusinessId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Services",
                table: "Services",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clients",
                table: "Clients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Businesses",
                table: "Businesses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailabilityExceptions",
                table: "AvailabilityExceptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Businesses_BusinessId",
                table: "Appointments",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Businesses_BusinessId1",
                table: "Appointments",
                column: "BusinessId1",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clients_ClientId",
                table: "Appointments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Clients_ClientId1",
                table: "Appointments",
                column: "ClientId1",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceId",
                table: "Appointments",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceId1",
                table: "Appointments",
                column: "ServiceId1",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Businesses_BusinessId",
                table: "Availabilities",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Businesses_BusinessId1",
                table: "Availabilities",
                column: "BusinessId1",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilityExceptions_Businesses_BusinessId",
                table: "AvailabilityExceptions",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvailabilityExceptions_Businesses_BusinessId1",
                table: "AvailabilityExceptions",
                column: "BusinessId1",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Businesses_BusinessId",
                table: "Clients",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Businesses_BusinessId1",
                table: "Clients",
                column: "BusinessId1",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Businesses_BusinessId",
                table: "Services",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Businesses_BusinessId1",
                table: "Services",
                column: "BusinessId1",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Businesses_BusinessId",
                table: "Users",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id");
        }
    }
}
