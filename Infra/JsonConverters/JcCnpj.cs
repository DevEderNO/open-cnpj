using System;
using Infra.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infra.JsonConverters;

public class JcCnpj: JsonConverter<VoCnpj>
{
    public override VoCnpj? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var numero = reader.GetString();
        return numero == null ? null : new VoCnpj(numero);
    }

    public override void Write(
        Utf8JsonWriter writer,
        VoCnpj value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value.Numero);
    }
}