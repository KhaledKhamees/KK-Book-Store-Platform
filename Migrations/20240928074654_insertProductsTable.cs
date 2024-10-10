using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCProject2.Migrations
{
    /// <inheritdoc />
    public partial class insertProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "Name", "Price", "Price100", "Price50" },
                values: new object[,]
                {
                    { 1, "William Shakespeare", "A Shakespearean drama about revenge and tragedy.", "1111111111", 29.989999999999998, "The Tragedy of Hamlet", 25.989999999999998, 20.989999999999998, 23.989999999999998 },
                    { 2, "Sun Tzu", "An ancient Chinese military treatise attributed to Sun Tzu.", "2222222222", 19.989999999999998, "The Art of War", 17.989999999999998, 12.99, 15.99 },
                    { 3, "Stephen Hawking", "An exploration of cosmology and the universe.", "3333333333", 35.990000000000002, "A Brief History of Time", 32.990000000000002, 28.989999999999998, 30.989999999999998 },
                    { 4, "George Orwell", "A dystopian science fiction novel about a totalitarian regime.", "4444444444", 24.989999999999998, "1984", 22.989999999999998, 18.989999999999998, 20.989999999999998 },
                    { 5, "John Romer", "A deep dive into the civilization of ancient Egypt.", "5555555555", 40.990000000000002, "The History of Ancient Egypt", 37.990000000000002, 32.990000000000002, 35.990000000000002 },
                    { 6, "E. H. Gombrich", "A renowned book that traces the history of visual arts.", "6666666666", 45.990000000000002, "The Story of Art", 42.990000000000002, 36.990000000000002, 39.990000000000002 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
