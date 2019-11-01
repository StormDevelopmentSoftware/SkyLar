// File StringExtensions.cs created by Animadoria (me@animadoria.cf) at 29/10/2019 19:18.
// (C) Animadoria 2019 - All Rights Reserved
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace SkyLar.Utilities
{
    public static class StringExtensions
    {
        public static bool NotNull(this string value)
        {
            return value != null && !string.IsNullOrEmpty(value);
        }

        public static bool IsNull(this string value)
        {
            return value == null && !string.IsNullOrEmpty(value);
        }

        public static bool NotWhiteSpace(this string value)
        {
            return value != null && !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsWhiteSpace(this string value)
        {
            return value == null && string.IsNullOrWhiteSpace(value);
        }

        public static string ComputeHash(this string value, HashAlgorithm algorithm)
        {
            return BitConverter.ToString(algorithm.ComputeHash(value.GetBytes())).Replace("-", "").ToLower();
        }

        public static byte[] GetBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string GetString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string HelpCommandFormat(this List<Command> list)
        {
            return string.Join(", ", list.Distinct().Select(x => Formatter.InlineCode(x.Name)));
        }
    }
}
