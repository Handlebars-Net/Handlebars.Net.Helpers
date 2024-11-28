namespace HandlebarsDotNet.Helpers.Utils;

internal static class RuntimeInformationUtils
{
    public static readonly bool IsBlazorWASM;

    static RuntimeInformationUtils()
    {
#if NET451 || NET452 || NET46 || NETSTANDARD1_3
        IsBlazorWASM = false;
#else
        IsBlazorWASM =
            // Used for Blazor WebAssembly .NET Core 3.x / .NET Standard 2.x
            System.Type.GetType("Mono.Runtime") != null ||

            // Use for Blazor WebAssembly .NET
            // See also https://github.com/mono/mono/pull/19568/files
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Create("BROWSER"));
#endif
    }
}