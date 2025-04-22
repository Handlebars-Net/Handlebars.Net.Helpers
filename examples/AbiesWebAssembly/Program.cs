using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Abies;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;
using static Abies.Html.Elements;
using static Abies.Html.Attributes;

Console.WriteLine("Bootstrapping...");

var counter = Browser.Application<HandlebarsNetTestApp, Arguments, Model>();

await Runtime.Run(counter, new Arguments());

public static partial class Interop
{
    [JSImport("writeToConsole", "abies.js")]
    public static partial Task WriteToConsole(string message);
}
public record Arguments
{
}

public record Model(List<Data> Results);

public class Data
{
    public required string Test { get; set; }

    public required string Result { get; set; }
}

public record Increment : Message;

public record Decrement : Message;

public class HandlebarsNetTestApp : Application<Model, Arguments>
{
    public static Model Initialize(Url url, Arguments argument)
    {
        Interop.WriteToConsole("initialized");

        var tests = new[]
        {
            "{{[lookup] data x1}}",
            "{{[lookup] data x2}}",
            "{{[lookup] data 4 \"n/a\"}}",

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
            "{{Substring \"foobar\" 3 3}}",
            "{{Substring \"foobar\" 3}}",
            "{{Substring \"foobar\" 2 3}}",
            "{{Now}}",
            "{{UtcNow}}",
            "{{Now \"yyyy-MM-dd\"}}",
            "{{Format (Now) \"yyyy-MM-dd\"}}",
            "{{Xeger.Generate \"[1-9]{1}\\d{3}\"}}",
            "{{Xeger.Generate '{[\"]A[\"]:[\"]A[0-9]{3}[1-9][\"]}'}}",
            "{{Random Type=\"Integer\" Min=1000 Max=9999}}",
            "{{GetEnvironmentVariable \"x\"}}",
            "{{GetEnvironmentVariable \"x\" \"User\"}}",
            "{{GetEnvironmentVariables}}",
            "{{Object.Equal 1 2}}",

            "{{Linq 'test' 'SubString(0, 2)'}}",
            "{{Linq x 'it.AddYears(1)'}}",
            "{{Linq data 'it[\"1\"] ?? \"n/a\"'}}",
            "{{Linq data 'it[\"9\"] ?? \"n/a\"'}}",

            "{{Expression 'test' 'SubString(0, 2)'}}",
            "{{Expression 'DateTime.Now'}}",
            "{{Expression 'DateTime.Now.AddDays(99)'}}",
            "{{Expression '1+2'}}",

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
            "{{DynamicLinq.Sum i}}",

            "Object {{Count o.a }}",
            "Object {{Where o.a 'it.Contains(\"s\")'}}",
            "Object {{Count o.a2 }}",
            "Object {{Where o.a2 'X.Contains(\"x\")'}}",

            "{{a.length}}"
        };

        var handlebars = Handlebars.Create();

        HandlebarsHelpers.Register(handlebars, options =>
        {
            options.UseCategoryPrefix = false;
            options.Categories = HandlebarsHelpersOptions.DefaultAllowedHandlebarsHelpers.Concat([Category.DynamicLinq, Category.Environment]).ToArray();
            options.Helpers =
            [
                new DynamicLinqHelpers(handlebars, options),
                new HumanizerHelpers(handlebars, options),
                new JsonPathHelpers(handlebars, options),
                new RandomHelpers(handlebars, options),
                new XegerHelpers(handlebars, options),
                new XPathHelpers(handlebars, options),
                new XsltHelpers(handlebars, options)
            ];
        });

        var results = new List<Data>();
        foreach (string test in tests)
        {
            var p1 = new
            {
                X = "x"
            };
            var p2 = new
            {
                X = "y"
            };
            var o = new
            {
                Id = 9,
                Name = "Test",
                a = new[] { "stef", "test", "other" },
                a2 = new[] { p1, p2 }
            };

            var dictionary = new
            {
                x1 = "one",
                x2 = "two",
                d = "ddd"
            };

            var t = new
            {
                x = DateTime.Now,
                i = new[] { 1, 2, 4 },
                a = new[] { "stef", "test", "other" },
                dup = new[] { "stef", "stef", "other" },
                d = new[] { new DateTime(2022, 1, 1), DateTime.Now },
                o = o,
                data = dictionary
            };
            var template = handlebars.Compile(test);
            var result = template.Invoke(t);

            results.Add(new Data { Test = test, Result = result });
        }

        return new Model(results);
    }

    public static Message OnLinkClicked(UrlRequest urlRequest)
    {
        Interop.WriteToConsole($"link clicked");
        return new Increment();
    }

    public static Message OnUrlChanged(Url url)
    {
        Interop.WriteToConsole($"url changed");
        return new Increment();
    }

    public static Subscription Subscriptions(Model model) => new();

    public static (Model model, IEnumerable<Command> commands) Update(Message message, Model model)
    {
        return (model, Array.Empty<Command>());
    }

    public static Document View(Model model)
    {
        var rows = model.Results.Select(r =>
            tr([],
            [
                td([], [pre([], [text(r.Test)])]),
                td([], [pre([], [text(r.Result)])])
            ])
        ).ToArray();

        var tableElement = table([className("table")],
            [
                thead([],
                [
                    tr([],
                    [
                        th([], [text("Test")]),
                        th([], [text("Result")])
                    ])
                ]),
                tbody([], rows)
            ]);

        return new Document("Handlebars.Net",
            div([],
            [
                h1([], [text("Results")]),

                tableElement,

                br([]),

                text($"Total results: {model.Results.Count}")
            ])
        );
    }
}