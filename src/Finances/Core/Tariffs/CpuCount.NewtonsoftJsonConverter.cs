using Newtonsoft.Json;

namespace Code.Example.Finances.Tariffs;

[JsonConverter(typeof(CpuCountNameNewtonsoftJsonConverter))]
public partial class CpuCount
{
    public class CpuCountNameNewtonsoftJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CpuCount);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value is CpuCount ram ? ram.Value : null);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<int?>(reader);

            return value is null ? null : Trusted(value.Value);
        }
    }
}
