using System;
using System.Linq;

namespace FoxTales.Infrastructure.Extensions.String
{
    public static class StringExtensions
    {
        public static string[] SplitLines(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return new string[0];
            return str.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string Sort(this string str, string seperator)
        {
            var parts = str.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries).ToList().Where(e => !string.IsNullOrWhiteSpace(e)).Select(e => e.Trim()).ToArray();
            Array.Sort(parts);

            return string.Join(string.Format("{0} ", seperator), parts);
        }

        public static string PadLeftEnforceLength(this string str, int totalWidth, char paddingChar)
        {
            if (str.Length >= totalWidth) return str.Substring(0, totalWidth);
            return str.PadLeft(totalWidth, paddingChar);
        }

        public static string PadRightEnforceLength(this string str, int totalWidth, char paddingChar)
        {
            if (str.Length >= totalWidth) return str.Substring(0, totalWidth);
            return str.PadRight(totalWidth, paddingChar);
        }
    }
}
