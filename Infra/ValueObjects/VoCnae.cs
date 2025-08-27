using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Infra.ValueObjects;

public partial class VoCnae(string code) : IValidatableObject
{
    public string Code { get; private set; } = code;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Code)) yield return new ValidationResult("CNAE is required");
        if (Code.Length != 7) yield return new ValidationResult("CNAE must be 7 characters long");            
    }

    public override string ToString() => Code;

    public static implicit operator string(VoCnae voCnae) => voCnae.Code;
    public static implicit operator VoCnae(string code) => new(code);

    [GeneratedRegex("^[0-9]{7}$")]
    private static partial Regex CnaeRegex();
}