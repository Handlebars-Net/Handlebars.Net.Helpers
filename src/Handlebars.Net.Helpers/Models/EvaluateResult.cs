namespace HandlebarsDotNet.Helpers.Models;

internal struct EvaluateResult
{
    public object? Result { get; set; }

    public bool IsEvaluated { get; set; }

    public static EvaluateResult NotEvaluated => new()
    {
        IsEvaluated = false,
        Result = null
    };
}