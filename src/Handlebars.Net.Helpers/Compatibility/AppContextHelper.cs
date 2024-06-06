using System;
using System.IO;

namespace HandlebarsDotNet.Helpers.Compatibility;

internal static class AppContextHelper
{
    public static string GetBaseDirectory()
    {
#if NET6_0_OR_GREATER
        return AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
#elif NETSTANDARD1_3
        return AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
#else
        return AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
#endif
    }
}