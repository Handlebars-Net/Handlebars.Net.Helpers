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
                "{{#each ar}}_{{this}}_{{/each}}",
                "{{#each (Skip ['a', 'b', 'c'] 1)}}{{this}}{{/each}}",

                "{{Append \"abc\" \"!def\"}}",
                "{{Capitalize \"abc def\"}}",
                "{{Ellipsis \"abcfskdagdghsjfjd\" 5}}",
                "{{Reverse \"abc def\"}}",
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

                "{{Min 42 5}}",
                "{{Min 42 5.2}}",
                "{{Min 42.1 5}}",
            };

            foreach (string test in tests)
            {
                var x = new
                {
                    ar = new object[] { 1, 2, 3 }
                };
                var template = handlebars.Compile(test);
                var result = template.Invoke(x);
                Console.WriteLine(result);
            }
        }
    }
}