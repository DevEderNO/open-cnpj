using Infra.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infra.ValueConverters;

public class VcFileSize
{
    public static ValueConverter<VoFileSize, string> Converter =>
        new(
            size => size,
            size => new VoFileSize(size.ToString())
        );
}