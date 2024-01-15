using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiMvcApp.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
            };
    }

    public static class IntegerExensions
    {
        public static int LimitToRange(this int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum)
            {
                return inclusiveMinimum;
            }

            if (value > inclusiveMaximum)
            {
                return inclusiveMaximum;
            }

            return value;
        }

    }
}
