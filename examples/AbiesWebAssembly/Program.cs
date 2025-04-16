using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Abies;
using AbiesWebAssembly;
using HandlebarsDotNet;
using HandlebarsDotNet.Helpers;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;
using static Abies.Html.Elements;

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
            "{{Abs -1}}",
            "{{Abs -1.1234}}",

            "{{Sign -1}}",
            "{{Sign " + long.MinValue + "}}",
            "{{Sign -1.1234}}",
            "{{Abs -1,1234}}",

            "{{Now}}",
            "{{UtcNow}}",
            "{{Now \"yyyy-MM-dd\"}}",
            "{{Format (Now) \"yyyy-MM-dd\"}}",

            "{{Reverse \"abc def\"}}",

            "{{FirstOrDefault a }}",
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
        var table = div([], model.Results
            .Select(r => Fluent.pre([], [text($"{r.Test} : {r.Result}")]))
            .ToArray());

        return new Document("Handlebars.Net",
            div([],
            [
                h1([], [text("Results")]),

                table,

                Fluent.br(),

                text($"Total results: {model.Results.Count}")
            ])
        );
    }
}