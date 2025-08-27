using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infra.ValueConverters;

public class VcList<T>
{
    public static ValueConverter<List<T>, string> Converter =>
        new(
            static list => JsonSerializer.Serialize(list, Options),
            static json => JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>()
        );

    private static JsonSerializerOptions Options => new() { WriteIndented = true };
}
