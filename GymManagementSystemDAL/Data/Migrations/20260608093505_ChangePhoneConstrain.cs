using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystemDAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePhoneConstrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "Phone LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "Phone LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers");

            migrationBuilder.DropCheckConstraint(
                name: "PhoneCheck",
                table: "Members");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck1",
                table: "Trainers",
                sql: "Phone like '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");

            migrationBuilder.AddCheckConstraint(
                name: "PhoneCheck",
                table: "Members",
                sql: "Phone like '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'");
        }
    }
}
