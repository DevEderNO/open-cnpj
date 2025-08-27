using System.Text.Json;
using System.Text.Json.Serialization;
using Infra.ValueObjects;

namespace Infra.JsonConverters;

public class JcRaizCnpj: JsonConverter<VoRaizCnpj>
{
    public override VoRaizCnpj? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var raizCnpj = reader.GetString();
        return raizCnpj == null ? null : new VoRaizCnpj(raizCnpj);
    }

    public override void Write(
        Utf8JsonWriter writer,
        VoRaizCnpj value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value.RaizCnpj);
    }
}
