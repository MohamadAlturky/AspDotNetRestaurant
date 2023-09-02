using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

public partial class ddd : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "DummyMealInformation",
			columns: table => new
			{
				Id = table.Column<long>(type: "bigint", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				NumberOfCalories = table.Column<int>(type: "int", nullable: false),
				Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
				ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_DummyMealInformation", x => x.Id);
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "DummyMealInformation");
	}
}
