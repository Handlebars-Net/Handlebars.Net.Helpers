using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers
{
    /// <summary>
    /// Some code copied from https://www.30secondsofcode.org/c-sharp/t/string/p/1
    /// and based on https://github.com/helpers/handlebars-helpers#string
    /// </summary>
    internal class StringHelpers : IHelpers
    {
        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Append(string value, string append)
        {
            if (string.IsNullOrEmpty(value))
            {
                return append;
            }

            if (string.IsNullOrEmpty(append))
            {
                return value;
            }

            return value + append;
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Camelcase(string value)
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
        public string Ellipsis(string value, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException();
            }

            if (string.IsNullOrEmpty(value) || value.Length <= length)
            {
                return value;
            }

            return value.Substring(0, length) + "...";
        }

        [HandlebarsWriter(WriterType.Write)]
        public bool IsString(object value)
        {
            return value is string;
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Join(IEnumerable<object> values, string? separator)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return separator is null ? string.Join(string.Empty, values) : string.Join(separator, values);
        }


        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Lowercase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.ToLower();
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string PascalCase(string value)
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
        public string Prepend(string value, string pre)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return pre + value;
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Replace(string value, string oldValue, string newValue)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            return value.Replace(oldValue, newValue);
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Reverse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return new string(value.ToCharArray().Reverse().ToArray());
        }

        [HandlebarsWriter(WriterType.Write)]
        public string[] Split(string value, string separator)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (separator == null)
            {
                throw new ArgumentNullException(nameof(separator));
            }

            return separator.Length == 1 ? value.Split(separator[0]) : value.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        [HandlebarsWriter(WriterType.Write)]
        public bool StartsWith(string value, string test)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.StartsWith(test);
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Trim(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Trim();
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string TrimEnd(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.TrimEnd();
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string TrimStart(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.TrimStart();
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Truncate(string value, int length)
        {
            if (length < 0)
            {
                throw new ArgumentException();
            }

            if (string.IsNullOrEmpty(value) || value.Length <= length)
            {
                return value;
            }

            return value.Substring(0, length);
        }

        [HandlebarsWriter(WriterType.WriteSafeString)]
        public string Uppercase(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.ToUpper();
        }
    }
}