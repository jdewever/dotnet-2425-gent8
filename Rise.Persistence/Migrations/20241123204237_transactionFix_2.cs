using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rise.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class transactionFix_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionItems",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TransactionItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TransactionItems");

            migrationBuilder.RenameTable(
                name: "TransactionItems",
                newName: "TransactionItem");

            migrationBuilder.RenameColumn(
                name: "TransactionID",
                table: "TransactionItem",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "TransactionItem",
                newName: "ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Transaction",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionItem",
                table: "TransactionItem",
                columns: new[] { "ProductId", "TransactionId" });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItem_TransactionId",
                table: "TransactionItem",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Product_ProductId",
                table: "TransactionItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Transaction_TransactionId",
                table: "TransactionItem",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Product_ProductId",
                table: "TransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Transaction_TransactionId",
                table: "TransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionItem",
                table: "TransactionItem");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItem_TransactionId",
                table: "TransactionItem");

            migrationBuilder.RenameTable(
                name: "TransactionItem",
                newName: "TransactionItems");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "TransactionItems",
                newName: "TransactionID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "TransactionItems",
                newName: "ProductID");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Transaction",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TransactionItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TransactionItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TransactionItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TransactionItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionItems",
                table: "TransactionItems",
                column: "Id");
        }
    }
}
