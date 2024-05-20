using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RequestAndSharePosition.Migrations.Commons
{
    /// <inheritdoc />
    public partial class adsenderName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "Requests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "Requests");
        }
    }
}
