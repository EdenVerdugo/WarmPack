using System;
using System.Text;
using System.Text.RegularExpressions;

namespace WarmPack.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > 1)
            {
                return (char.ToLowerInvariant(value[0]) + value.Substring(1)).Replace("_", string.Empty);
            }

            return value;
        }

        public static void ForEach(this string value, Action<char> action)
        {
            for (int i = 0; i < value.Length; i++)
            {
                action(value[i]);
            }
        }

        public static long CountChar(this string value, char character)
        {
            long result = 0;
            value.ForEach(c =>
            {
                if (character == c)
                {
                    result++;
                }
            });

            return result;
        }

        public static bool IsNumeric(this string str)
        {
            bool result = true;

            if (str.CountChar('.') > 1)
                return false;

            str.ForEach(c =>
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    result = false;
                    return;
                }
            });

            return result;
        }

        /// <summary>
        /// Regresa true o false si la cadena de texto contiene alguno de los valores proporcionados.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="contains"></param>
        /// <returns></returns>
        public static bool Contains(this string text, params string[] containsAny)
        {
            bool contains = false;
            containsAny.ForEach(item =>
            {
                if (text.Contains(item) && !contains)
                {
                    contains = true;
                }
            });

            return contains;
        }

        public static string RemoveIlegalCharactersXML(this string text)
        {
            text = text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;")
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u")
                //.Replace("ñ", "&ntilde;")
                .Replace("Á", "A")
                .Replace("É", "E")
                .Replace("Í", "I")
                .Replace("Ó", "O")
                .Replace("Ú", "U")
                //.Replace("Ñ", "&Ntilde;")
                ;

            string textNormalize = text.Normalize(NormalizationForm.FormD);
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            string textResult = reg.Replace(textNormalize, "");

            return textResult;
        }

        public static string ToUTF8Encoding(this string text)
        {
            text = text.RemoveIlegalCharactersXML();
            var bytes = Encoding.UTF8.GetBytes(text);

            var returnText = Encoding.UTF8.GetString(bytes);
            return returnText;
        }

        public static bool IsNullOrWhiteSpace(this string text)
        {
            text = text.Trim();

            return string.IsNullOrEmpty(text);
        }

        public static string RemoveAccent(this string text)
        {
            byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return Encoding.ASCII.GetString(bytes);
        }

        private static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            
            str = Regex.Replace(str, @"\s+", " ").Trim();
            
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); 
            return str;
        }

        public static string ToUrlSlug(string value)
        {            
            value = value.ToLowerInvariant();

            value = RemoveAccent(value);

            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            value = value.Trim('-', '_');

            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
