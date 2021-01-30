using System;
using HandlebarsDotNet.Helpers.Enums;

namespace HandlebarsDotNet.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HandlebarsWriterAttribute : Attribute
    {
        public WriterType Type { get; }

        public string? Name { get; set; }

        public HelperUsage Usage { get; set; }

        public bool PassContext { get; set; }

        public HandlebarsWriterAttribute(WriterType type, string? name = null) : this(type, HelperUsage.Both, name)
        {
        }

        public HandlebarsWriterAttribute(WriterType type, HelperUsage usage, string? name = null) : this(type, usage, false, name)
        {
        }

        public HandlebarsWriterAttribute(WriterType type, HelperUsage usage, bool passContext, string? name = null)
        {
            Type = type;
            Name = name;
            Usage = usage;
            PassContext = passContext;
        }
    }
}