using System;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class HandlebarsWriterAttribute : Attribute
    {
        public WriterType Type { get; }

        public string? Name{ get; set; }

        public HandlebarsWriterAttribute(WriterType type, string? name = null)
        {
            Type = type;
            Name = name;
        }
    }
}