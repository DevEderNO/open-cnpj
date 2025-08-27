using System;
using Infra.ValueObjects;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infra.JsonConverters;

public class JcEmail : JsonConverter<VoEmail>
{
    public override VoEmail Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new VoEmail(reader.GetString() ?? string.Empty);
    }

    public override void Write(Utf8JsonWriter writer, VoEmail value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Address);
    }
}
