using System;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Brokers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:ad_action", "created,delete,update")
                .Annotation("Npgsql:Enum:ad_state", "fully_finished,furnished,half_finished,unfinished")
                .Annotation("Npgsql:Enum:ad_type", "rent_monthly,rent_seasonal,rent_yearly,sale_cash,sale_installment")
                .Annotation("Npgsql:Enum:credits_log_action", "gift,purchase,refund,spend")
                .Annotation("Npgsql:Enum:payment_status", "completed,failed,pending")
                .Annotation("Npgsql:Enum:property_type", "apartment,building,chalet,commercial_shop,compound,farm,garage,hotel,house,land,medical_clinic,office,studio,villa,warehouse")
                .Annotation("Npgsql:Enum:report_reason", "fake_listing,fraud,harassment,other")
                .Annotation("Npgsql:Enum:report_status", "dismissed,pending,resolved,under_review")
                .OldAnnotation("Npgsql:Enum:ad_action", "created,delete,update")
                .OldAnnotation("Npgsql:Enum:ad_state", "fully_finished,furnished,half_finished,unfinished")
                .OldAnnotation("Npgsql:Enum:ad_type", "rent_monthly,rent_seasonal,rent_yearly,sale_cash,sale_installment")
                .OldAnnotation("Npgsql:Enum:credits_log_action", "gift,purchase,refund,spend")
                .OldAnnotation("Npgsql:Enum:payment_status", "completed,failed,pending")
                .OldAnnotation("Npgsql:Enum:property_type", "apartment,building,chalet,commercial_shop,compound,farm,garage,hotel,house,land,medical_clinic,office,studio,villa,warehouse");

            migrationBuilder.CreateTable(
                name: "BrokerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LicenseNumber = table.Column<string>(type: "text", nullable: true),
                    LicenseExpiryDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    WhatsAppNumber = table.Column<string>(type: "text", nullable: false),
                    CoverPhoto = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrokerReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BrokerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<ReportReason>(type: "report_reason", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<ReportStatus>(type: "report_status", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerReports_AspNetUsers_BrokerUserId",
                        column: x => x.BrokerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrokerReports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrokerReports_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BrokerReviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BrokerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerReviews_AspNetUsers_BrokerUserId",
                        column: x => x.BrokerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrokerReviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrokerReviews_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdLogs_UserId",
                table: "AdLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerProfiles_UserId",
                table: "BrokerProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerReports_BrokerUserId_UserId",
                table: "BrokerReports",
                columns: new[] { "BrokerUserId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerReports_UserId",
                table: "BrokerReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerReports_UserId1",
                table: "BrokerReports",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerReviews_BrokerUserId_UserId",
                table: "BrokerReviews",
                columns: new[] { "BrokerUserId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerReviews_UserId",
                table: "BrokerReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerReviews_UserId1",
                table: "BrokerReviews",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AdLogs_AspNetUsers_UserId",
                table: "AdLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdLogs_AspNetUsers_UserId",
                table: "AdLogs");

            migrationBuilder.DropTable(
                name: "BrokerProfiles");

            migrationBuilder.DropTable(
                name: "BrokerReports");

            migrationBuilder.DropTable(
                name: "BrokerReviews");

            migrationBuilder.DropIndex(
                name: "IX_AdLogs_UserId",
                table: "AdLogs");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:ad_action", "created,delete,update")
                .Annotation("Npgsql:Enum:ad_state", "fully_finished,furnished,half_finished,unfinished")
                .Annotation("Npgsql:Enum:ad_type", "rent_monthly,rent_seasonal,rent_yearly,sale_cash,sale_installment")
                .Annotation("Npgsql:Enum:credits_log_action", "gift,purchase,refund,spend")
                .Annotation("Npgsql:Enum:payment_status", "completed,failed,pending")
                .Annotation("Npgsql:Enum:property_type", "apartment,building,chalet,commercial_shop,compound,farm,garage,hotel,house,land,medical_clinic,office,studio,villa,warehouse")
                .OldAnnotation("Npgsql:Enum:ad_action", "created,delete,update")
                .OldAnnotation("Npgsql:Enum:ad_state", "fully_finished,furnished,half_finished,unfinished")
                .OldAnnotation("Npgsql:Enum:ad_type", "rent_monthly,rent_seasonal,rent_yearly,sale_cash,sale_installment")
                .OldAnnotation("Npgsql:Enum:credits_log_action", "gift,purchase,refund,spend")
                .OldAnnotation("Npgsql:Enum:payment_status", "completed,failed,pending")
                .OldAnnotation("Npgsql:Enum:property_type", "apartment,building,chalet,commercial_shop,compound,farm,garage,hotel,house,land,medical_clinic,office,studio,villa,warehouse")
                .OldAnnotation("Npgsql:Enum:report_reason", "fake_listing,fraud,harassment,other")
                .OldAnnotation("Npgsql:Enum:report_status", "dismissed,pending,resolved,under_review");
        }
    }
}
