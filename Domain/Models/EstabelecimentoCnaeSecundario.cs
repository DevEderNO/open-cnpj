using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Infra.JsonConverters;
using Infra.ValueObjects;

namespace Domain.Models;

public class EstabelecimentoCnaeSecundario
{
    [Key]
    public int Id { get; set; }
    
    [JsonConverter(typeof(JcCnpj)), StringLength(14)]
    public VoCnpj EstabelecimentoId { get; set; }
    public virtual Estabelecimento Estabelecimento { get; set; }

    [JsonConverter(typeof(JcCnae)), StringLength(7)]
    public VoCnae CnaeSecundarioId { get; set; }
    public virtual DCnae CnaeSecundario { get; set; }
}