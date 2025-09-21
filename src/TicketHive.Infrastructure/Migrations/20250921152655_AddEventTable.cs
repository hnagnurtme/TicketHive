using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketHive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role",
                table: "users",
                newName: "roles");

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    venue_capacity = table.Column<int>(type: "integer", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sale_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    sale_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    organizer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_featured = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_events_users_organizer_id",
                        column: x => x.organizer_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_events_organizer_id",
                table: "events",
                column: "organizer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.RenameColumn(
                name: "roles",
                table: "users",
                newName: "role");
        }
    }
}
