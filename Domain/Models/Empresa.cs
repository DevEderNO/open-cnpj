using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Infra.JsonConverters;
using Infra.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class Empresa(VoRaizCnpj cnpjBasico, string razaoSocial, string naturezaJuridica, string qualificacaoResponsavel, decimal capitalSocial, string porteEmpresa, string enteFederativoResponsavel, DateTime modificationDate)
{
  [Key,JsonConverter(typeof(JcRaizCnpj)), StringLength(8)] public VoRaizCnpj CnpjBasico { get; set; } = cnpjBasico; 
  [Required, MaxLength(255)] public string RazaoSocial { get; set; } = razaoSocial;
  [Required, StringLength(4)] public string NaturezaJuridica { get; set; } = naturezaJuridica;
  [Required, StringLength(2)] public string QualificacaoResponsavel { get; set; } = qualificacaoResponsavel;
  [Required] public decimal CapitalSocial { get; set; } = capitalSocial;
  [Required, StringLength(2)] public string PorteEmpresa { get; set; } = porteEmpresa;
  [Required, MaxLength(255)] public string EnteFederativoResponsavel { get; set; } = enteFederativoResponsavel;
  [Required]
  public DateTime ModificationDate { get; set; } = modificationDate;
}
