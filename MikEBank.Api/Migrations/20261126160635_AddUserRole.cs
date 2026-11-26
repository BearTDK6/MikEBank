using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MikEBank.Api.Migrations;

/// <inheritdoc />
public partial class _20261126160635_AddUserRole : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Role",
            table: "users",
            type: "character varying(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "customer");

        migrationBuilder.Sql("""
            INSERT INTO users
                (full_name, "IdentityNumber", "DateOfBirth", "Gender", "Nationality",
                 "PlaceOfOrigin", "PlaceOfResidence", "DateOfIssue", "DateOfExpiry",
                 "Email", "Phone", "Role")
            VALUES
                ('LÊ THỊ THU VÂN', '030189004780', DATE '1989-03-21', 'Nữ', 'Việt Nam',
                 'Bình Xuyên, Bình Giang, Hải Dương', 'Thái Hòa, Bình Giang, Hải Dương',
                 DATE '2021-05-10', DATE '2029-03-21', 'admin@example.test', NULL, 'admin');
            """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DELETE FROM users
            WHERE full_name = 'LÊ THỊ THU VÂN'
              AND "IdentityNumber" = '030189004780'
              AND "DateOfBirth" = DATE '1989-03-21'
              AND "Gender" = 'Nữ'
              AND "Nationality" = 'Việt Nam'
              AND "PlaceOfOrigin" = 'Bình Xuyên, Bình Giang, Hải Dương'
              AND "PlaceOfResidence" = 'Thái Hòa, Bình Giang, Hải Dương'
              AND "DateOfIssue" = DATE '2021-05-10'
              AND "DateOfExpiry" = DATE '2029-03-21'
              AND "Email" = 'admin@example.test'
              AND "Phone" IS NULL
              AND "Role" = 'admin';
            """);

        migrationBuilder.DropColumn(
            name: "Role",
            table: "users");
    }
}
