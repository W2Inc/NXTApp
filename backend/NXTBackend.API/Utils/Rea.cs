using System.Text.Json;
using System.Text.Json.Serialization;

// public class NullGuidJsonConverter : JsonConverter<Guid>
// {
//     public override void WriteJson(Utf8JsonReader writer, Guid value, JsonSerializer serializer)
//     {
//         writer.WriteValue(value == Guid.Empty ? null : value.ToString());
//     }

//     public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue, JsonSerializer serializer)
//     {
//         var value = reader.Value.ToString();
//         return reader.Value == null ? Guid.Empty : Guid.Parse(value);
//     }
// }
