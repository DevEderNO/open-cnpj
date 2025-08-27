using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Infra.ValueObjects;

public partial class VoRaizCnpj(string raizCnpj) : IValidatableObject
{
    public string RaizCnpj { get; private set; } = raizCnpj;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(RaizCnpj)) yield return new ValidationResult("Raiz CNPJ is required");
        if (RaizCnpj.Length != 8) yield return new ValidationResult("Raiz CNPJ must be 8 characters long");
    }

    public override string ToString() => RaizCnpj;

    public static implicit operator string(VoRaizCnpj voRaizCnpj) => voRaizCnpj.RaizCnpj;
    public static implicit operator VoRaizCnpj(string raizCnpj) => new(raizCnpj);

    [GeneratedRegex("^[0-9]{8}$")]
    private static partial Regex RaizCnpjRegex();
}
