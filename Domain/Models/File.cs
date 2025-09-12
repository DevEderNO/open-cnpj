using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Infra.JsonConverters;
using Infra.ValueObjects;

namespace Domain.Models;

[method: JsonConstructor]
public class File(
    string url,
    DateTime modificationDate,
    VoFileSize size
) : BaseLink(url, modificationDate)
{
    [Required, JsonConverter(typeof(JcFileSize))]
    public VoFileSize Size { get; set; } = size;
    
    public float Progress { get; set; } = 0;
    
    public long LineNumber { get; set; } = 0;

    public string FileName => Path.GetFileName(Url);
}