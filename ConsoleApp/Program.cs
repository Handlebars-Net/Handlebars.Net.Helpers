using System;
using HandlebarsDotNet.Helpers;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var handlebars = HandlebarsDotNet.Handlebars.Create();

            HandleBarsHelpers.Register(handlebars);

            var tests = new[]
            {
                "{{Reverse \"abc def\"}}",
                "{{Capitalize \"abc def\"}}",
                "{{ToCamelCase \"abc def\"}}",
                "{{ToPascalCase \"abc def\"}}",
                "{{ToUpper \"abc\"}}",
                "{{ToLower \"XYZ\"}}",

                "{{Abs -1}}",
                "{{Abs -1.1234}}",
                "{{Sign -1}}",
                "{{Sign " + long.MinValue + "}}",
                "{{Sign -1.1234}}",
                "{{Abs -1,1234}}",
                // "{{Abs \"x\"}}"
            };

            foreach (string test in tests)
            {
                var template = handlebars.Compile(test);
                var result = template.Invoke("xx");
                Console.WriteLine(result);
            }
        }
    }
}