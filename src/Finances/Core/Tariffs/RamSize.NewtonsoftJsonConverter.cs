using Newtonsoft.Json;

namespace Code.Example.Finances.Tariffs;

[JsonConverter(typeof(RamSizeNameNewtonsoftJsonConverter))]
public partial class RamSize
{
    public class RamSizeNameNewtonsoftJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RamSize);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value is RamSize ram ? ram.Value : null);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = serializer.Deserialize<int?>(reader);

            return value is null ? null : Trusted(value.Value);
        }
    }
}
