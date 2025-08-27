using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infra.JsonConverters;

  public class JcList<T>: JsonConverter<List<T>>
{
    public override List<T>? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var list = reader.GetString();
        return list == null ? null : JsonSerializer.Deserialize<List<T>>(list, options);
    }

    public override void Write(
        Utf8JsonWriter writer,
        List<T> value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(JsonSerializer.Serialize(value, options));
    }
}
