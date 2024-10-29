using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CIDA.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_OP_ARQUIVO",
                columns: table => new
                {
                    ID_ARQUIVO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false),
                    ID_RESUMO = table.Column<int>(type: "int", nullable: true),
                    NOME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EXTENSAO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TAMANHO = table.Column<int>(type: "int", nullable: false),
                    DATA_UPLOAD = table.Column<DateTime>(type: "datetime2", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_ARQUIVO", x => x.ID_ARQUIVO);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_AUTENTICACAO",
                columns: table => new
                {
                    ID_AUTENTICACAO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMAIL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HASH_SENHA = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_AUTENTICACAO", x => x.ID_AUTENTICACAO);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_RESUMO",
                columns: table => new
                {
                    ID_RESUMO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false),
                    DATA_GERACAO = table.Column<DateTime>(type: "DATE", nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(max)", maxLength: 1000000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_RESUMO", x => x.ID_RESUMO);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_AUTENTICACAO = table.Column<int>(type: "int", nullable: false),
                    NOME = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TIPO_DOCUMENTO = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    NUM_DOCUMENTO = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TELEFONE = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DATA_CRIACAO = table.Column<DateTime>(type: "DATE", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_USUARIO", x => x.ID_USUARIO);
                    table.ForeignKey(
                        name: "FK_T_OP_USUARIO_T_OP_AUTENTICACAO_ID_AUTENTICACAO",
                        column: x => x.ID_AUTENTICACAO,
                        principalTable: "T_OP_AUTENTICACAO",
                        principalColumn: "ID_AUTENTICACAO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_OP_INSIGHT",
                columns: table => new
                {
                    ID_INSIGHT = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_USUARIO = table.Column<int>(type: "int", nullable: false),
                    ID_RESUMO = table.Column<int>(type: "int", nullable: false),
                    DATA_GERACAO = table.Column<DateTime>(type: "DATE", nullable: false),
                    DESCRICAO = table.Column<string>(type: "nvarchar(max)", maxLength: 1000000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_OP_INSIGHT", x => x.ID_INSIGHT);
                    table.ForeignKey(
                        name: "FK_T_OP_INSIGHT_T_OP_USUARIO_ID_USUARIO",
                        column: x => x.ID_USUARIO,
                        principalTable: "T_OP_USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_ARQUIVO_URL",
                table: "T_OP_ARQUIVO",
                column: "URL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_AUTENTICACAO_EMAIL",
                table: "T_OP_AUTENTICACAO",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_INSIGHT_ID_RESUMO",
                table: "T_OP_INSIGHT",
                column: "ID_RESUMO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_INSIGHT_ID_USUARIO",
                table: "T_OP_INSIGHT",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_USUARIO_ID_AUTENTICACAO",
                table: "T_OP_USUARIO",
                column: "ID_AUTENTICACAO");

            migrationBuilder.CreateIndex(
                name: "IX_T_OP_USUARIO_NUM_DOCUMENTO_ID_AUTENTICACAO",
                table: "T_OP_USUARIO",
                columns: new[] { "NUM_DOCUMENTO", "ID_AUTENTICACAO" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_OP_ARQUIVO");

            migrationBuilder.DropTable(
                name: "T_OP_INSIGHT");

            migrationBuilder.DropTable(
                name: "T_OP_RESUMO");

            migrationBuilder.DropTable(
                name: "T_OP_USUARIO");

            migrationBuilder.DropTable(
                name: "T_OP_AUTENTICACAO");
        }
    }
}
