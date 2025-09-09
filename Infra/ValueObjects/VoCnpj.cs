using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Infra.ValueObjects;

public partial class VoCnpj(string numero) : IValidatableObject
{
    public string Numero { get; private set; } = numero;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(Numero)) yield return new ValidationResult("Numero CNPJ is required");
        if (Numero.Length != 14) yield return new ValidationResult("Numero CNPJ must be 14 characters long");
    }

    public override string ToString() => Numero;

    public static implicit operator string(VoCnpj voCnpj) => voCnpj.Numero;
    public static implicit operator VoCnpj(string numero) => new(numero);

    [GeneratedRegex("^[0-9]{14}$")]
    private static partial Regex NumeroRegex();

    public string RaizCnpj => Numero[..8];
}
