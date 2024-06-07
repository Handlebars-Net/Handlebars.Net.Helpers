using System;
using System.IO;

namespace HandlebarsDotNet.Helpers.Compatibility;

internal static class AppContextHelper
{
    public static string GetBaseDirectory()
    {
#if NETSTANDARD1_3_OR_GREATER || NET6_0_OR_GREATER
        return AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
#else
        return AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
#endif
    }
}