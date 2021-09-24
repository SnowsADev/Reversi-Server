
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using ReversiRestApi.Models;

namespace ReversiRestApi.Helpers
{
    public class KleurMultiArrayConverter : JsonConverter<Kleur[,]>
    {
        public override Kleur[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Kleur[,] bord, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            for (int i = 0; i < bord.GetLength(0); i++)
            {
                writer.WriteStartArray();
                for (int j = 0; j < bord.GetLength(1); j++)
                {
                    writer.WriteNumberValue((int)bord[i, j]);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();

        }
    }
}