using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.API.Migrations
{
    /// <inheritdoc />
    public partial class datacontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_States_CountryId_Name",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Cities_StateId_Name",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId_Name",
                table: "States",
                columns: new[] { "CountryId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId_Name",
                table: "Cities",
                columns: new[] { "StateId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_States_CountryId_Name",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Cities_StateId_Name",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId_Name",
                table: "States",
                columns: new[] { "CountryId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId_Name",
                table: "Cities",
                columns: new[] { "StateId", "Name" });
        }
    }
}
