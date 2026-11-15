using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPJL.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AllowStandaloneAdminAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_AspNetUsers_ExactlyOneOwner",
                table: "AspNetUsers");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AspNetUsers_ExactlyOneOwner",
                table: "AspNetUsers",
                sql: "(\r\n    [CustomerId] IS NOT NULL\r\n    AND [EmployeeId] IS NULL\r\n)\r\nOR\r\n(\r\n    [CustomerId] IS NULL\r\n    AND [EmployeeId] IS NOT NULL\r\n)\r\nOR\r\n(\r\n    [CustomerId] IS NULL\r\n    AND [EmployeeId] IS NULL\r\n)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_AspNetUsers_ExactlyOneOwner",
                table: "AspNetUsers");

            migrationBuilder.AddCheckConstraint(
                name: "CK_AspNetUsers_ExactlyOneOwner",
                table: "AspNetUsers",
                sql: "(\r\n    [CustomerId] IS NOT NULL\r\n    AND [EmployeeId] IS NULL\r\n)\r\nOR\r\n(\r\n    [CustomerId] IS NULL\r\n    AND [EmployeeId] IS NOT NULL\r\n)");
        }
    }
}
