using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Infra.JsonConverters;
using Infra.ValueConverters;
using Infra.ValueObjects;

namespace Domain.Models;

public class Estabelecimento(
    VoRaizCnpj cnpjBasico,
    string cnpjOrdem,
    string cnpjDv,
    string identificadorMatrizFilial,
    string? nomeFantasia,
    string situacaoCadastral,
    DateTime dataSituacaoCadastral,
    string motivoSituacaoCadastral,
    string? nomeCidadeNoExterior,
    string? pais,
    DateTime dataInicioAtividade,
    string cnaeFiscalPrincipal,
    List<VoCnae>? cnaeFiscalSecundaria,
    string? tipoLogradouro,
    string logradouro,
    string numero,
    string? complemento,
    string? bairro,
    string? cep,
    string uf,
    string municipio,
    string? ddd1,
    string? telefone1,
    string? ddd2,
    string? telefone2,
    string? dddFax,
    string? fax,
    VoEmail? email,
    string? situacaoEspecial,
    DateTime? dataSituacaoEspecial,
    DateTime modificationDate)
{
    [Key, JsonConverter(typeof(JcRaizCnpj)), StringLength(8)]
    public VoRaizCnpj CnpjBasico { get; set; } = cnpjBasico;
    [MaxLength(4)]
    public string CnpjOrdem { get; set; } = cnpjOrdem;
    [MaxLength(2)]
    public string CnpjDv { get; set; } = cnpjDv;
    [MaxLength(1)]
    public string IdentificadorMatrizFilial { get; set; } = identificadorMatrizFilial;
    [MaxLength(60)]
    public string? NomeFantasia { get; set; } = nomeFantasia;
    [MaxLength(2)]
    public string SituacaoCadastral { get; set; } = situacaoCadastral;
    public DateTime DataSituacaoCadastral { get; set; } = dataSituacaoCadastral;
    [MaxLength(2)]
    public string MotivoSituacaoCadastral { get; set; } = motivoSituacaoCadastral;
    [MaxLength(255)]
    public string? NomeCidadeNoExterior { get; set; } = nomeCidadeNoExterior;
    [MaxLength(4)]
    public string? Pais { get; set; } = pais;
    public DateTime DataInicioAtividade { get; set; } = dataInicioAtividade;
    [MaxLength(7)]
    public string CnaeFiscalPrincipal { get; set; } = cnaeFiscalPrincipal;
    [MaxLength(2000), JsonConverter(typeof(JcList<VoCnae>))]
    public List<VoCnae>? CnaeFiscalSecundaria { get; set; } = cnaeFiscalSecundaria;
    [MaxLength(255)]
    public string? TipoLogradouro { get; set; } = tipoLogradouro;
    [MaxLength(255)]
    public string? Logradouro { get; set; } = logradouro;
    [MaxLength(10)]
    public string Numero { get; set; } = numero;
    [MaxLength(255)]
    public string? Complemento { get; set; } = complemento;
    [MaxLength(255)]
    public string? Bairro { get; set; } = bairro;
    [MaxLength(8)]
    public string? Cep { get; set; } = cep;
    [MaxLength(2)]
    public string? Uf { get; set; } = uf;
    [MaxLength(4)]
    public string? Municipio { get; set; } = municipio;
    [MaxLength(2)]
    public string? Ddd1 { get; set; } = ddd1;
    [MaxLength(9)]
    public string? Telefone1 { get; set; } = telefone1;
    [MaxLength(2)]
    public string? Ddd2 { get; set; } = ddd2;
    [MaxLength(9)]
    public string? Telefone2 { get; set; } = telefone2;
    [MaxLength(2)]
    public string? DddFax { get; set; } = dddFax;
    [MaxLength(9)]
    public string? Fax { get; set; } = fax;
    [JsonConverter(typeof(JcEmail))]    
    public VoEmail? Email { get; set; } = email;
    [MaxLength(255)]
    public string? SituacaoEspecial { get; set; } = situacaoEspecial;
    public DateTime? DataSituacaoEspecial { get; set; } = dataSituacaoEspecial;
    [Required, Column(TypeName = "timestamp without time zone")]
    public DateTime ModificationDate { get; set; } = modificationDate;
}