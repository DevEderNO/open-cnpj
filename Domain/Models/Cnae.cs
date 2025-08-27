using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Infra.JsonConverters;
using Infra.ValueObjects;

namespace Domain.Models;

public class DCnae(VoCnae cnae, string descricao, DateTime modificationDate)
{
    [Key,JsonConverter(typeof(JcCnae)), StringLength(7)] public VoCnae Cnae { get; set; } = cnae;
    [Required, MaxLength(255)] public string Descricao { get; set; } = descricao;
    [Required, Column(TypeName = "timestamp without time zone")]
    public DateTime ModificationDate { get; set; } = modificationDate;
}