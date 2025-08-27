using System.Text.Json;
using System.Text.Json.Serialization;
using Infra.ValueObjects;

namespace Infra.JsonConverters;

public class JcCnae: JsonConverter<VoCnae>
{
    public override VoCnae? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var cnae = reader.GetString();
        return cnae == null ? null : new VoCnae(cnae);
    }

    public override void Write(
        Utf8JsonWriter writer,
        VoCnae value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value.Code);
    }
}