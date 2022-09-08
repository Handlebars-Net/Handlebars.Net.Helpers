using System;
using System.Collections;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class EnvironmentHelpers : BaseHelpers, IHelpers
{
    [HandlebarsWriter(WriterType.String)]
    public string? GetEnvironmentVariable(string variable)
    {
        return Environment.GetEnvironmentVariable(variable);
    }

    [HandlebarsWriter(WriterType.String)]
    public IDictionary GetEnvironmentVariables()
    {
        return Environment.GetEnvironmentVariables();
    }

    public EnvironmentHelpers(IHandlebars context) : base(context)
    {
    }
}