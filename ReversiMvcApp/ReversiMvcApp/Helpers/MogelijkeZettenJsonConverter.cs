using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReversiMvcApp.Helpers
{
    public class MogelijkeZettenJsonConverter : JsonConverter<List<List<int>>>
    {
        public override List<List<int>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, List<List<int>> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (List<int> kvp in value)
            {
                writer.WriteStartObject();
                writer.WriteNumber("x", kvp[0]);
                writer.WriteNumber("y", kvp[1]);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }
    }
}
