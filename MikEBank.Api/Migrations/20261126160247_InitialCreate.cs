using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MikEBank.Api.Migrations;

/// <inheritdoc />
public partial class _20261126160247_InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                IdentityNumber = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                Nationality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                PlaceOfOrigin = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                PlaceOfResidence = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                DateOfIssue = table.Column<DateOnly>(type: "date", nullable: false),
                DateOfExpiry = table.Column<DateOnly>(type: "date", nullable: false),
                Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_users", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_users_Email",
            table: "users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_users_IdentityNumber",
            table: "users",
            column: "IdentityNumber",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "users");
    }
}
