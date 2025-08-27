using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class BaseLink(string url, DateTime modificationDate)
{
    [Key, StringLength(255)]
    public string Url { get; set; } = url;
    [Required, Column(TypeName = "timestamp without time zone")]
    public DateTime ModificationDate { get; set; } = modificationDate;
}