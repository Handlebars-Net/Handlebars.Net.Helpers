using System;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;
using HandlebarsDotNet.Helpers.Enums;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var handlebars = HandlebarsDotNet.Handlebars.Create();

            //handlebars.RegisterHelper("ArrayTest", (writer, context, arguments) =>
            //{
            //    var array = new object[]
            //    {
            //        1,
            //        "two"
            //    };

            //    writer.WriteSafeString($"[{string.Join(", ", array)}]");
            //});

            //var templateX = handlebars.Compile("{{#each (ArrayTest)}}_{{this}}_{{/each}}");
            //var resultX = templateX.Invoke("");
            //Console.WriteLine("ArrayTest = " + resultX);

            HandleBarsHelpers.Register(handlebars);

            var tests = new[]
            {
                //"{{#each (ArrayTest)}}_{{this}}_{{/each}}",
                //"{{#each ar}}_{{this}}_{{/each}}",
                
                "{{this}}",
                "{{Constants.Math.PI}}",
                "{{#IsMatch \"Hello\" \"Hello\"}}yes{{else}}no{{/IsMatch}}",
                "{{#IsMatch \"Hello\" \"hello\" 'i'}}yesI{{else}}noI{{/IsMatch}}",
                "{{#StartsWith \"Hello\" \"x\"}}Hi{{else}}Goodbye{{/StartsWith}}",
                "{{Skip ['a', 'b', 'c', 1] 1}}",

                "{{StartsWith \"abc\" \"!def\"}}",
                "{{Append \"abc\" \"!def\"}}",
                "{{Capitalize \"abc def\"}}",
                "{{Ellipsis \"abcfskdagdghsjfjd\" 5}}",
                "{{Reverse \"abc def\"}}",
                "{{Camelcase \"abc def\"}}",
                "{{Pascalcase \"abc def\"}}",
                "{{Uppercase \"abc\"}}",
                "{{Lowercase \"XYZ\"}}",

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
                var x = DateTime.Now;
                var template = handlebars.Compile(test);
                var result = template.Invoke(x);
                Console.WriteLine(result);
            }
        }
    }
}