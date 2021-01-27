using System;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var handlebars = Handlebars.Create();
            HandlebarsHelpers.Register(handlebars, options => { options.UseCategoryPrefix = false; });

            handlebars.RegisterHelper("ArrayTest", (context, arguments) =>
            {
                var array = new object[]
                {
                    1,
                    "two"
                };

                return array;
            });

            var templateX = handlebars.Compile("{{#each (ArrayTest min=6)}}_{{this}}_{{/each}}");
            var resultX = templateX.Invoke("");
            Console.WriteLine("ArrayTest = " + resultX);

            var tests = new[]
            {
                "{{Abs -1}}",
                "{{Abs -1.1234}}",

                "{{Add 1 2}}",
                "{{Add 1 '2'}}",

                "{{Sign -1}}",
                "{{Sign " + long.MinValue + "}}",
                "{{Sign -1.1234}}",
                "{{Abs -1,1234}}",

                "{{Min 42 5}}",
                "{{Min 42 5.2}}",
                "{{Min 42.1 5}}",

                "{{this}}",
                "{{[Constants.Math.PI]}}",
                "{{#IsMatch \"Hello\" \"Hello\"}}yes{{else}}no{{/IsMatch}}",
                "{{#IsMatch \"Hello\" \"hello\"}}yes{{else}}no{{/IsMatch}}",
                "{{#IsMatch \"Hello\" \"hello\" 'i'}}yesI{{else}}noI{{/IsMatch}}",
                "{{#StartsWith \"Hello\" \"x\"}}Hi{{else}}Goodbye{{/StartsWith}}",
                "{{Skip ['a', 'b', 'c', 1] 1}}",

                "{{StartsWith \"abc\" \"!def\"}}",
                "{{Append \"abc\" \"!def\"}}",
                "{{Capitalize \"abc def\"}}",
                "{{Ellipsis \"abcfskdagdghsjfjd\" 5}}",
                "{{Reverse \"abc def\"}}",
                "{{Truncate \"abc def\" 166}}",
                "{{Camelcase \"abc def\"}}",
                "{{Pascalcase \"abc def\"}}",
                "{{Uppercase \"abc\"}}",
                "{{Lowercase \"XYZ\"}}",
                "{{Format x \"o\"}}",
                "{{Now}}",
                "{{UtcNow}}",
                "{{Now \"yyyy-MM-dd\"}}",
                "{{Format (Now) \"yyyy-MM-dd\"}}",
            };

            foreach (string test in tests)
            {
                var t = new
                {
                    x = DateTime.Now
                };
                var template = handlebars.Compile(test);
                var result = template.Invoke(t);
                Console.WriteLine($"{test} : {result}");
            }

            Console.WriteLine(new string('-', 80));

            var handlebars2 = Handlebars.Create();
            HandlebarsHelpers.Register(handlebars2, options => { options.UseCategoryPrefix = true; });

            var tests2 = new[]
            {
                "{{[Math.Abs] -42}}",
                "{{Math.Abs -42}}"
            };

            foreach (string test in tests2)
            {
                var x = DateTime.Now;
                var template = handlebars2.Compile(test);
                var result = template.Invoke(x);
                Console.WriteLine($"{test} : {result}");
            }
        }
    }
}