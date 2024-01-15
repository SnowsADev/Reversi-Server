using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReversiMvcApp.Models;

namespace ReversiMvcApp.Helpers
{
    public class KleurMultiArrayJsonConverter : JsonConverter<Kleur[,]>
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



    // Ben nog niet helemaal blij met idee om hier een extension op string van te maken, maar het is tot nu toe het makkelijkst
    public static class BordExtensions
    {
        public static string ConvertToString(this Kleur[,] bord)
        {
            string sResult = "";

            sResult += "[";
            for (int i = 0; i < bord.GetLength(0); i++)
            {
                sResult += "[";
                for (int j = 0; j < bord.GetLength(1); j++)
                {
                    sResult += (int)bord[i, j];
                }
                sResult +=  "]";
            }
            sResult += "]";

            return sResult;
        }

        public static Kleur[,] ConvertBordFromString(this string value)
        {
            Kleur[,] bord = null;

            //eerste en laatste teken weghalen
            string sLines = value.Substring(1, value.Length - 2);
            string[] lines = sLines.Split(new char[] { ']' }, System.StringSplitOptions.RemoveEmptyEntries);

            string sLine = "";

            for (int i = 0; i < lines.Length; i++)
            {
                sLine = lines[i].Replace("[", "");
                sLine = sLine.Replace(",", "");

                if (bord == null)
                {
                    bord = new Kleur[sLine.Length, sLine.Length];
                }

                for (int j = 0; j < sLine.Length; j++)
                {
                    string x = sLine.Substring(j, 1);
                    bord[i, j] = (Kleur)int.Parse(x);
                }
            }

            return bord;
        }
    }
}