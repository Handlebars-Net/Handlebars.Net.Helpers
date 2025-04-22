using System.Runtime.CompilerServices;
using Abies.DOM;
using static Abies.Html.Elements;

namespace AbiesWebAssembly;

public static class Fluent
{
    public static Element button(Attribute[] attributes, Element[] children, [CallerLineNumber] int id = 0)
            => element("fluent-button", attributes, children, id);
}