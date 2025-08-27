using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Infra.ValueObjects;

namespace Infra.JsonConverters;

public class JcFileSize: JsonConverter<VoFileSize>
{
    public override VoFileSize? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var size = reader.GetString();
        return size == null ? null : new VoFileSize(size);
    }

  public override void Write(
        Utf8JsonWriter writer,
        VoFileSize value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value.ToString());
    }
}
