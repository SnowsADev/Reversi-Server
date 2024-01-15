using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReversiMvcApp.Helpers
{
    public class SpelerJsonConverter : JsonConverter<Speler>
    {
        public override Speler Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Speler speler, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("Id", speler.Id);
            writer.WriteString("Naam", speler.Naam);
            writer.WriteNumber("Kleur", (int)speler.Kleur);

            writer.WriteEndObject();

        }
    }
}
