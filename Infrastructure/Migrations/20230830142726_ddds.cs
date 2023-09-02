using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

public partial class ddds : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<DateTime>(
			name: "CreatedAt",
			table: "DummyMealInformation",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

		migrationBuilder.AddColumn<byte[]>(
			name: "RowVersion",
			table: "DummyMealInformation",
			type: "rowversion",
			rowVersion: true,
			nullable: false,
			defaultValue: new byte[0]);

		migrationBuilder.AddColumn<DateTime>(
			name: "UpdatedAt",
			table: "DummyMealInformation",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "CreatedAt",
			table: "DummyMealInformation");

		migrationBuilder.DropColumn(
			name: "RowVersion",
			table: "DummyMealInformation");

		migrationBuilder.DropColumn(
			name: "UpdatedAt",
			table: "DummyMealInformation");
	}
}
