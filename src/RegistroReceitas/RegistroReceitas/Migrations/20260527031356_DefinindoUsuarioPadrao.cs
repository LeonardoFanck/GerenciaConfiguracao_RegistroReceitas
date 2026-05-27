using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegistroReceitas.Migrations
{
    /// <inheritdoc />
    public partial class DefinindoUsuarioPadrao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "Email", "Login", "Nome", "Senha", "Situacao" },
                values: new object[] { new Guid("f736be24-92ab-46db-9059-f31e73c23523"), "leonardofanck@gmail.com", "leonardo", "Leonardo Fanck", "123", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuario",
                keyColumn: "Id",
                keyValue: new Guid("f736be24-92ab-46db-9059-f31e73c23523"));
        }
    }
}
