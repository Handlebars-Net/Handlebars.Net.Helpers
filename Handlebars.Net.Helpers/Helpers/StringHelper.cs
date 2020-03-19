using System;
using System.Linq;
using System.Text;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    //[HandlebarsHelper]

    /// <summary>
    /// Some code copied from https://www.30secondsofcode.org/c-sharp/t/string/p/1
    /// and based on https://github.com/helpers/handlebars-helpers#string
    /// </summary>
    internal class StringHelper : IHelper
    {
        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Append(string value, string append)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrEmpty(append))
            {
                return value;
            }

            return value + append;
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Capitalize(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            char[] chars = value.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string ToUpper(string value)
        {
            return value?.ToUpper();
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string ToLower(string value)
        {
            return value?.ToLower();
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string ToCamelCase(string value)
        {
            if (string.IsNullOrEmpty(value) || value.Length < 2)
            {
                return value;
            }

            string[] words = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            var builder = new StringBuilder(words[0].ToLower());
            for (int i = 1; i < words.Length; i++)
            {
                builder.Append(words[i].Substring(0, 1).ToUpper());
                builder.Append(words[i].Substring(1));
            }

            return builder.ToString();
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string ToPascalCase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (value.Length < 2)
            {
                return value.ToUpper();
            }

            string[] words = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

            var builder = new StringBuilder();
            foreach (string word in words)
            {
                builder.Append(word.Substring(0, 1).ToUpper());
                builder.Append(word.Substring(1));
            }

            return builder.ToString();
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Reverse(string value)
        {
            return new string(value.ToCharArray().Reverse().ToArray());
        }
    }
}