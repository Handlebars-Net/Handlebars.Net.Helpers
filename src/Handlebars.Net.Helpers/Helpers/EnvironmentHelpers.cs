﻿using System;
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

#if !NETSTANDARD1_3
    [HandlebarsWriter(WriterType.String)]
    public string? GetEnvironmentVariable(string variable, string target)
    {
        return Environment.GetEnvironmentVariable(variable, Parse(target));
    }

    [HandlebarsWriter(WriterType.String)]
    public IDictionary GetEnvironmentVariables(string target)
    {
        return Environment.GetEnvironmentVariables(Parse(target));
    }

    private static EnvironmentVariableTarget Parse(string target)
    {
        return Enum.TryParse<EnvironmentVariableTarget>(target, out var @enum)
            ? @enum
            : EnvironmentVariableTarget.Process;
    }
#endif

    public EnvironmentHelpers(IHandlebars context) : base(context)
    {
    }
}