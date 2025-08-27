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
                    Cnae = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cnaes", x => x.Cnae);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    CnpjBasico = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    RazaoSocial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NaturezaJuridica = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    QualificacaoResponsavel = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CapitalSocial = table.Column<decimal>(type: "numeric", nullable: false),
                    PorteEmpresa = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    EnteFederativoResponsavel = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.CnpjBasico);
                });

            migrationBuilder.CreateTable(
                name: "Estabelecimentos",
                columns: table => new
                {
                    CnpjBasico = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    CnpjOrdem = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    CnpjDv = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    IdentificadorMatrizFilial = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    NomeFantasia = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    SituacaoCadastral = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    DataSituacaoCadastral = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MotivoSituacaoCadastral = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    NomeCidadeNoExterior = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Pais = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    DataInicioAtividade = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CnaeFiscalPrincipal = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    CnaeFiscalSecundaria = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    TipoLogradouro = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Logradouro = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Complemento = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Cep = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    Uf = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Municipio = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    Ddd1 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Telefone1 = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    Ddd2 = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Telefone2 = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    DddFax = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Fax = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    SituacaoEspecial = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DataSituacaoEspecial = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estabelecimentos", x => x.CnpjBasico);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    Progress = table.Column<float>(type: "real", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Url);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Url);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cnaes");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Estabelecimentos");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Folders");
        }
    }
}
