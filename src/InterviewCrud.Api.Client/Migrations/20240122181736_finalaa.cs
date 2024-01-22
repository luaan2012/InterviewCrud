using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewCrud.Api.Client.Migrations
{
    /// <inheritdoc />
    public partial class finalaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Contacts",
                newName: "NameContact");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameContact",
                table: "Contacts",
                newName: "Name");
        }
    }
}
