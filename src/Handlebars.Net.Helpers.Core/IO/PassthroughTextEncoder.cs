using System.Collections.Generic;
using System.Text;

namespace HandlebarsDotNet.Helpers.IO;

/// <summary>
/// A text encoder that implements <see cref="ITextEncoder"/> and directly passes through the input text to the target without any encoding.
/// </summary>
public class PassthroughTextEncoder : ITextEncoder
{
    /// <summary>
    /// Writes the given StringBuilder text directly to the target TextWriter.
    /// </summary>
    /// <param name="text">The StringBuilder containing the text.</param>
    /// <param name="target">The TextWriter to write the text to.</param>
    public void Encode(StringBuilder text, TextWriter target)
    {
        target.Write(text);
    }

    /// <summary>
    /// Writes the given string text directly to the target TextWriter.
    /// </summary>
    /// <param name="text">The string containing the text.</param>
    /// <param name="target">The TextWriter to write the text to.</param>
    public void Encode(string text, TextWriter target)
    {
        target.Write(text);
    }

    /// <summary>
    /// Writes the given text enumerator directly to the target TextWriter.
    /// </summary>
    /// <typeparam name="T">The type of the text enumerator.</typeparam>
    /// <param name="text">The enumerator containing the text.</param>
    /// <param name="target">The TextWriter to write the text to.</param>
    public void Encode<T>(T text, TextWriter target) where T : IEnumerator<char>
    {
        while (text.MoveNext())
        {
            target.Write(text.Current);
        }
    }
}