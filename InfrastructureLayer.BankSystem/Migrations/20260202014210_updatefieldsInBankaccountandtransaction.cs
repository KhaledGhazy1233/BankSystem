using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.BankSystem.Migrations
{
    /// <inheritdoc />
    public partial class updatefieldsInBankaccountandtransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_BankAccounts_FromAccountId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_BankAccounts_ToAccountId",
                table: "transactions");

            migrationBuilder.AlterColumn<int>(
                name: "ToAccountId",
                table: "transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FromAccountId",
                table: "transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfter",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceBefore",
                table: "transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FailedPinAttempts",
                table: "BankAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPinLocked",
                table: "BankAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_BankAccounts_FromAccountId",
                table: "transactions",
                column: "FromAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_BankAccounts_ToAccountId",
                table: "transactions",
                column: "ToAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_BankAccounts_FromAccountId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_BankAccounts_ToAccountId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "BalanceAfter",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "BalanceBefore",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "FailedPinAttempts",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "IsPinLocked",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "BankAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "ToAccountId",
                table: "transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FromAccountId",
                table: "transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_BankAccounts_FromAccountId",
                table: "transactions",
                column: "FromAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_BankAccounts_ToAccountId",
                table: "transactions",
                column: "ToAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
