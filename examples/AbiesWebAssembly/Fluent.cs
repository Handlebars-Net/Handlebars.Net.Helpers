using System.Runtime.CompilerServices;
using Abies.DOM;
using static Abies.Html.Elements;

namespace AbiesWebAssembly;

public static class Fluent
{
    public static Element button(Attribute[] attributes, Element[] children, [CallerLineNumber] int id = 0)
            => element("fluent-button", attributes, children, id);

    public static Node pre(Attribute[] attributes, Node[] children, [CallerLineNumber] int id = 0)
    {
        return element("pre", attributes, children, id);
    }

    public static Node br([CallerLineNumber] int id = 0)
    {
        return element("br", [], [], id);
    }
}