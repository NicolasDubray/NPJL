using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPJL.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceToCustomerRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Customer_Request_Type",
                table: "Customer_Request");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Customer_Request",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "Customer_Request",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Request_InvoiceId",
                table: "Customer_Request",
                column: "InvoiceId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Customer_Request_ExactlyOneTarget",
                table: "Customer_Request",
                sql: "([BookingId] IS NOT NULL AND [InvoiceId] IS NULL) OR ([BookingId] IS NULL AND [InvoiceId] IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Customer_Request_Type",
                table: "Customer_Request",
                sql: "[Type] IN ('Cancel', 'Update', 'Question')");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Request_Invoices_InvoiceId",
                table: "Customer_Request",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Request_Invoices_InvoiceId",
                table: "Customer_Request");

            migrationBuilder.DropIndex(
                name: "IX_Customer_Request_InvoiceId",
                table: "Customer_Request");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Customer_Request_ExactlyOneTarget",
                table: "Customer_Request");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Customer_Request_Type",
                table: "Customer_Request");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Customer_Request");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Customer_Request",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Customer_Request_Type",
                table: "Customer_Request",
                sql: "[Type] IN ('Cancel', 'Update')");
        }
    }
}
