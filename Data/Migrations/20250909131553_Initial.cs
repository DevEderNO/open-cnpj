using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cnaes",
                columns: table => new
                {
                    Cnae = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cnaes", x => x.Cnae);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    CnpjBasico = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    RazaoSocial = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NaturezaJuridica = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    QualificacaoResponsavel = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CapitalSocial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorteEmpresa = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    EnteFederativoResponsavel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.CnpjBasico);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Progress = table.Column<float>(type: "real", nullable: false),
                    LineNumber = table.Column<long>(type: "bigint", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Url);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Url);
                });

            migrationBuilder.CreateTable(
                name: "Estabelecimentos",
                columns: table => new
                {
                    Cnpj = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    CnpjBasico = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    CnpjOrdem = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CnpjDv = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    IdentificadorMatrizFilial = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    NomeFantasia = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    SituacaoCadastral = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DataSituacaoCadastral = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MotivoSituacaoCadastral = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    NomeCidadeNoExterior = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    DataInicioAtividade = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CnaeFiscalPrincipalId = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    TipoLogradouro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Logradouro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Numero = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Complemento = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Bairro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Cep = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Uf = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Municipio = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Ddd1 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Telefone1 = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    Ddd2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Telefone2 = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    DddFax = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SituacaoEspecial = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DataSituacaoEspecial = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DCnaeCnae = table.Column<string>(type: "nvarchar(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estabelecimentos", x => x.Cnpj);
                    table.ForeignKey(
                        name: "FK_Estabelecimentos_Cnaes_DCnaeCnae",
                        column: x => x.DCnaeCnae,
                        principalTable: "Cnaes",
                        principalColumn: "Cnae");
                });

            migrationBuilder.CreateTable(
                name: "EstabelecimentoCnaesSecundarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstabelecimentoId = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    CnaeSecundarioId = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstabelecimentoCnaesSecundarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstabelecimentoCnaesSecundarios_Cnaes_CnaeSecundarioId",
                        column: x => x.CnaeSecundarioId,
                        principalTable: "Cnaes",
                        principalColumn: "Cnae",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstabelecimentoCnaesSecundarios_Estabelecimentos_EstabelecimentoId",
                        column: x => x.EstabelecimentoId,
                        principalTable: "Estabelecimentos",
                        principalColumn: "Cnpj",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstabelecimentoCnaesSecundarios_CnaeSecundarioId",
                table: "EstabelecimentoCnaesSecundarios",
                column: "CnaeSecundarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EstabelecimentoCnaesSecundarios_EstabelecimentoId",
                table: "EstabelecimentoCnaesSecundarios",
                column: "EstabelecimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Estabelecimentos_DCnaeCnae",
                table: "Estabelecimentos",
                column: "DCnaeCnae");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "EstabelecimentoCnaesSecundarios");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Estabelecimentos");

            migrationBuilder.DropTable(
                name: "Cnaes");
        }
    }
}
