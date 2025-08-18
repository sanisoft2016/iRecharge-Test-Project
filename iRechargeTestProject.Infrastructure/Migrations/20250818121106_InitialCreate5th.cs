using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iRechargeTestProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate5th : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "UserType",
                table: "AspNetUsers",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AspNetUsers");
        }
    }
}
