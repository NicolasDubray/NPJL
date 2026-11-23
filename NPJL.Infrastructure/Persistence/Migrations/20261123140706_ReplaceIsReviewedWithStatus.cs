using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NPJL.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceIsReviewedWithStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReviewed",
                table: "Customer_Request");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Customer_Request",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "InReview");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customer_Request");
        
            migrationBuilder.AddColumn<bool>(
                name: "IsReviewed",
                table: "Customer_Request",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
