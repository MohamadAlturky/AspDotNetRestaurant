using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

public partial class asdsafds : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "NotificationMessage",
			columns: table => new
			{
				Id = table.Column<long>(type: "bigint", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				MessageSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
				MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
				SentAt = table.Column<DateTime>(type: "datetime2", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_NotificationMessage", x => x.Id);
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "NotificationMessage");
	}
}
