using System;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;

namespace ConsoleApp
{
    class Program
    {
        private static string GetArgumentValue(object argument)
        {
            if (argument.GetType().Name == "UndefinedBindingResult")
            {
                return (string)argument.GetType().GetField("Value").GetValue(argument);
            }

            return argument.ToString();
        }

        static void Main(string[] args)
        {
            var handlebars = Handlebars.Create();

            handlebars.RegisterHelper("ifCond", (output, context, arguments) =>
            {
                var value1 = GetArgumentValue(arguments[0]);
                var operation = GetArgumentValue(arguments[1]);
                var value2 = GetArgumentValue(arguments[2]);
                var returnValue1 = GetArgumentValue(arguments[3]);
                var returnValue2 = GetArgumentValue(arguments[4]);

                switch (operation)
                {
                    case "eq":
                        output.Write(value1 == value2 ? returnValue1 : returnValue2);
                        break;
                    case "ne":
                        output.Write(value1 != value2 ? returnValue1 : returnValue2);
                        break;
                    default:
                        throw new Exception("ifCond: Unrecognized operation");
                }
            });

            var template1 = handlebars.Compile("{{ifCond MyData eq 01 X Y}}");
            var result1 = template1.Invoke(new { MyData = "01" });

            HandlebarsHelpers.Register(handlebars, options => { options.UseCategoryPrefix = false; });

            //handlebars.RegisterHelper("ArrayTest", (context, arguments) =>
            //{
            //    var array = new object[]
            //    {
            //        1,
            //        "two"
            //    };

            //    return array;
            //});

            //var templateX = handlebars.Compile("{{#each (ArrayTest min=6)}}_{{this}}_{{/each}}");
            //var resultX = templateX.Invoke("");
            //Console.WriteLine("ArrayTest = " + resultX);

            Environment.SetEnvironmentVariable("x", DateTime.Now.ToString());

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
                "{{Skip [\"a\", \"b\", \"c\", 1] 1}}",

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
                "{{Xeger.Generate \"[1-9]{1}\\d{3}\"}}",
                "{{Random Type=\"Integer\" Min=1000 Max=9999}}",
                "{{GetEnvironmentVariable \"x\"}}",
                "{{GetEnvironmentVariable \"x\" \"User\"}}",
                "{{GetEnvironmentVariables}}",

                "{{Linq 'test' 'SubString(0, 2)'}}",
                "{{Linq x 'it.AddYears(1)'}}",
                "{{Where a 'it.Contains(\"s\")'}}",
                "{{FirstOrDefault a }}",
                "{{FirstOrDefault a 'it.Contains(\"o\")'}}",
                "{{FirstOrDefault a 'it.Contains(\"z\")'}}",
                "{{LastOrDefault a }}",
                "{{LastOrDefault a 'it.Contains(\"o\")'}}",
                "{{LastOrDefault a 'it.Contains(\"z\")'}}",
                "{{Count a }}",
                "{{Count a 'it.Contains(\"o\")'}}",
                "{{Count a 'it.Contains(\"z\")'}}",
                "{{DynamicLinq.Max a }}",
                "{{DynamicLinq.Max a 'it.Contains(\"o\")'}}",
                "{{DynamicLinq.Max a 'it.Contains(\"z\")'}}",
                "{{Any a 'it.Contains(\"o\")'}}",
                "{{All a 'it.Contains(\"o\")'}}",
                "{{All a 'it.Length > 1'}}",
                "{{Distinct dup 'it.Length > 1'}}",
                "{{Average i}}",
                "{{Skip a 1}}",
                "{{Take a 2}}",
                "{{SkipAndTake a 1 1}}",
                "{{Where d 'Year > 2022'}}",
                "{{OrderBy a 'it'}}",
                "{{OrderBy a 'it desc'}}",
                "{{OfType a 'int'}}",
                "{{DynamicLinq.Sum i}}"
            };

            foreach (string test in tests)
            {
                var t = new
                {
                    x = DateTime.Now,
                    i = new[] { 1, 2, 4 },
                    a = new[] { "stef", "test", "other" },
                    dup = new[] { "stef", "stef", "other" },
                    d = new[] { new DateTime(2022, 1, 1), DateTime.Now }
                };
                var template = handlebars.Compile(test);
                var result = template.Invoke(t);
                Console.WriteLine($"{test} : '{result}'");
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
                Console.WriteLine($"{test} : '{result}'");
            }
        }
    }
}