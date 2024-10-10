using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCProject2.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "orderHeaders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SesstionId",
                table: "orderHeaders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "orderHeaders");

            migrationBuilder.DropColumn(
                name: "SesstionId",
                table: "orderHeaders");

            
        }
    }
}
