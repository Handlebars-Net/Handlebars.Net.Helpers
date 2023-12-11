using System;
using System.Globalization;
using System.IO;
using Stef.Validation;

namespace HandlebarsDotNet.Helpers.Utils;

/// <summary>
/// Based on https://www.nuget.org/packages/SoftwareKobo.Net.WebUtility
/// </summary>
public static class HtmlUtils
{
    public static void HtmlDecode(string? value, TextWriter output)
    {
        Guard.NotNull(output);

        if (value == null)
        {
            return;
        }

        for (var index = 0; index < value.Length; ++index)
        {
            var ch1 = value[index];
            if (ch1 == '&')
            {
                if (value[index + 1] == '#' && index + 2 < value.Length && char.IsDigit(value[index + 2]))
                {
                    var ch2 = value[index + 2];
                    if (index + 3 < value.Length)
                    {
                        if (value[index + 3] == ';')
                        {
                            var num = int.Parse(new string(new[] { ch2 }));
                            if (num is > 0 and < 65536)
                            {
                                output.Write((char)num);
                                index += 3;
                                continue;
                            }
                        }
                        else if (char.IsDigit(value[index + 3]))
                        {
                            var ch3 = value[index + 3];
                            if (index + 4 < value.Length)
                            {
                                if (value[index + 4] == ';')
                                {
                                    var num = int.Parse(new string(new[] { ch2, ch3 }));
                                    if (num is > 0 and < 65536)
                                    {
                                        output.Write((char)num);
                                        index += 4;
                                        continue;
                                    }
                                }
                                else if (char.IsDigit(value[index + 4]))
                                {
                                    var ch4 = value[index + 4];
                                    if (index + 5 < value.Length)
                                    {
                                        if (value[index + 5] == ';')
                                        {
                                            var num = int.Parse(new string(new[] { ch2, ch3, ch4 }));
                                            if (num is > 0 and < 65536)
                                            {
                                                output.Write((char)num);
                                                index += 5;
                                                continue;
                                            }
                                        }
                                        else if (char.IsDigit(value[index + 5]))
                                        {
                                            var ch5 = value[index + 5];
                                            if (index + 6 < value.Length)
                                            {
                                                if (value[index + 6] == ';')
                                                {
                                                    var num = int.Parse(new string(new[] { ch2, ch3, ch4, ch5 }));
                                                    if (num is > 0 and < 65536)
                                                    {
                                                        output.Write((char)num);
                                                        index += 6;
                                                        continue;
                                                    }
                                                }
                                                else if (char.IsDigit(value[index + 6]))
                                                {
                                                    var ch6 = value[index + 6];
                                                    if (index + 7 < value.Length && value[index + 7] == ';')
                                                    {
                                                        var num = int.Parse(new string(new[] { ch2, ch3, ch4, ch5, ch6 }));
                                                        if (num is > 0 and < 65536)
                                                        {
                                                            output.Write((char)num);
                                                            index += 7;
                                                            continue;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (value.IndexOf("quot;", index + 1, StringComparison.Ordinal) == index + 1)
                {
                    output.Write('"');
                    index += 5;
                }
                else if (value.IndexOf("amp;", index + 1, StringComparison.Ordinal) == index + 1)
                {
                    output.Write('&');
                    index += 4;
                }
                else if (value.IndexOf("lt;", index + 1, StringComparison.Ordinal) == index + 1)
                {
                    output.Write('<');
                    index += 3;
                }
                else if (value.IndexOf("gt;", index + 1, StringComparison.Ordinal) == index + 1)
                {
                    output.Write('>');
                    index += 3;
                }
                else
                {
                    output.Write(ch1);
                }
            }
            else
            {
                output.Write(ch1);
            }
        }
    }

    public static string HtmlDecode(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var output = new StringWriter(CultureInfo.InvariantCulture);
        HtmlDecode(value, output);
        return output.ToString();
    }

    public static void HtmlEncode(string? value, TextWriter output)
    {
        Guard.NotNull(output);

        if (value == null)
        {
            return;
        }

        foreach (var ch in value)
        {
            if (ch is >= ' ' and <= 'ÿ')
            {
                output.Write("&#" + Convert.ToInt32(ch) + ";");
            }
            else
            {
                switch (ch)
                {
                    case '"':
                        output.Write("&quot;");
                        continue;

                    case '&':
                        output.Write("&amp;");
                        continue;

                    case '\'':
                        output.Write("&#39;");
                        continue;

                    case '<':
                        output.Write("&lt;");
                        continue;

                    case '>':
                        output.Write("&gt;");
                        continue;

                    default:
                        output.Write(ch);
                        continue;
                }
            }
        }
    }

    public static string HtmlEncode(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var output = new StringWriter(CultureInfo.InvariantCulture);
        HtmlEncode(value, output);
        return output.ToString();
    }

    //public static string? UrlEncode(string? value) => value == null ? null : Uri.EscapeDataString(value);

    //public static string? UrlDecode(string? value)
    //{
    //    if (value == null)
    //    {
    //        return null;
    //    }

    //    value = value.Replace('+', ' ');
    //    return Uri.UnescapeDataString(value);
    //}
}