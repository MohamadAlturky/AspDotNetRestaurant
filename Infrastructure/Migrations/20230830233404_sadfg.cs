using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

public partial class sadfg : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_MealEntry_AtDay",
			table: "MealEntry");

		migrationBuilder.CreateIndex(
			name: "IX_MealEntry_AtDay1",
			table: "MealEntry",
			column: "AtDay",
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_MealEntry_AtDay1",
			table: "MealEntry");

		migrationBuilder.CreateIndex(
			name: "IX_MealEntry_AtDay",
			table: "MealEntry",
			column: "AtDay");
	}
}
