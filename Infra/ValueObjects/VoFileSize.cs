using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Infra.ValueObjects;

public class VoFileSize : IValidatableObject
{
    public VoFileSize(string size)
    {
        Size = size switch {
            _ when size.Contains("kb", StringComparison.CurrentCultureIgnoreCase) => Kb * decimal.Parse(size.Replace("kb", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ when size.Contains("mb", StringComparison.CurrentCultureIgnoreCase) => Mb * decimal.Parse(size.Replace("mb", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ when size.Contains("gb", StringComparison.CurrentCultureIgnoreCase) => Gb * decimal.Parse(size.Replace("gb", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ when size.Contains("tb", StringComparison.CurrentCultureIgnoreCase) => Tb * decimal.Parse(size.Replace("tb", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ when size.Contains('k', StringComparison.CurrentCultureIgnoreCase) => Kb * decimal.Parse(size.Replace("k", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ when size.Contains('m', StringComparison.CurrentCultureIgnoreCase) => Mb * decimal.Parse(size.Replace("m", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ when size.Contains('g', StringComparison.CurrentCultureIgnoreCase) => Gb * decimal.Parse(size.Replace("g", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ when size.Contains('t', StringComparison.CurrentCultureIgnoreCase) => Tb * decimal.Parse(size.Replace("t", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.Any, CultureInfo.InvariantCulture),
            _ => 0
        };
    }

    private const decimal Kb = 1024;
    private const decimal Mb = Kb * 1024;
    private const decimal Gb = Mb * 1024;
    private const decimal Tb = Gb * 1024;

    /// <summary>
    /// Convert the size to bytes
    /// </summary>
    public decimal Size { get; set; }

    /// <summary>
    /// Convert the size to a string with the unit
    /// </summary>
    public override string ToString() => Size switch
    {
        var size and < Mb => $"{size / Kb} KB",
        var size and < Gb => $"{size / Mb} MB",
        var size and < Tb => $"{size / Gb} GB",
        var size and >= Tb => $"{size / Tb} TB"
    };

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Size <= 0) yield return new ValidationResult("Size must be greater than 0");
    }

    public static implicit operator decimal(VoFileSize voFileSize) => voFileSize.Size;
    public static implicit operator VoFileSize(decimal size) => new(size.ToString());

    public static implicit operator string(VoFileSize voFileSize) => voFileSize.ToString();
    public static implicit operator VoFileSize(string size) => new(size);
}