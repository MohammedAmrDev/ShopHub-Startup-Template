using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myshop.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductImageURLPropertyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Img",
                table: "Products",
                newName: "ImageURL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Products",
                newName: "Img");
        }
    }
}
