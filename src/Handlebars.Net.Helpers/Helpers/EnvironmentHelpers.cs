﻿using System;
using System.Collections;
using HandlebarsDotNet.Helpers.Attributes;
using HandlebarsDotNet.Helpers.Enums;
using HandlebarsDotNet.Helpers.Options;

namespace HandlebarsDotNet.Helpers.Helpers;

internal class EnvironmentHelpers(IHandlebars context, HandlebarsHelpersOptions options) : BaseHelpers(context, options), IHelpers
{
#if NETSTANDARD1_3
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
#else
    private const string Process = nameof(EnvironmentVariableTarget.Process);

    [HandlebarsWriter(WriterType.String)]
    public string? GetEnvironmentVariable(string variable, string target = Process)
    {
        return Environment.GetEnvironmentVariable(variable, Parse(target));
    }

    public IDictionary GetEnvironmentVariables(string target = Process)
    {
        return Environment.GetEnvironmentVariables(Parse(target));
    }

    private static EnvironmentVariableTarget Parse(string target)
    {
        return Enum.TryParse<EnvironmentVariableTarget>(target, out var @enum) ? @enum : EnvironmentVariableTarget.Process;
    }
#endif

    public Category Category => Category.Environment;
}