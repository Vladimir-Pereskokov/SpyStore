using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SpyStore.DAL.EF.Migrations
{
    public partial class FINAL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Products",
                schema: "Store",
                newSchema: "Store");

            migrationBuilder.AddColumn<decimal>(
                name: "OrderTotal",
                schema: "Store",
                table: "Orders",
                type: "money",
                nullable: true,
                computedColumnSql: "Store.GetOrderTotal([Id])");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTotal",
                schema: "Store",
                table: "Orders");

            migrationBuilder.EnsureSchema(
                name: "Store");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "Store",
                newSchema: "Store");
        }
    }
}
