using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewCrud.Api.Client.Migrations
{
    /// <inheritdoc />
    public partial class initiala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailUser",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailUser",
                table: "Client");
        }
    }
}
