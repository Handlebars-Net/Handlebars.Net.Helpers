// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// Copied from https://github.com/aspnet/EntityFramework/blob/dev/src/Shared/Check.cs
namespace HandlebarsDotNet.Helpers.Validation
{
    [DebuggerStepThrough]
    public static class Guard
    {
        public static T Condition<T>(T value, Predicate<T> condition, string parameterName)
        {
            NotNull(condition, nameof(condition));
            NotNull(value, nameof(value));

            if (!condition(value))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        public static T NotNull<T>(T value, string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static T NotNull<T>(T value, string parameterName, string propertyName)
        {
            if (ReferenceEquals(value, null))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));
                NotNullOrEmpty(propertyName, nameof(propertyName));

                throw new ArgumentException(CoreStrings.ArgumentPropertyNull(propertyName, parameterName));
            }

            return value;
        }

        public static IList<T> NotNullOrEmpty<T>(IList<T> value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Count == 0)
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(CoreStrings.CollectionArgumentIsEmpty(parameterName));
            }

            return value;
        }

        public static string? NotNullOrEmpty(string? value, string parameterName)
        {
            Exception? e = null;
            if (ReferenceEquals(value, null))
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException(CoreStrings.ArgumentIsEmpty(parameterName));
            }

            if (e != null)
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw e;
            }

            return value;
        }

        public static string? NullButNotEmpty(string? value, string parameterName)
        {
            if (!ReferenceEquals(value, null) && (value.Length == 0))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(CoreStrings.ArgumentIsEmpty(parameterName));
            }

            return value;
        }

        public static IEnumerable<T?> HasNoNulls<T>(IEnumerable<T?> value, string parameterName)
            where T : class
        {
            NotNull(value, parameterName);

            if (value.Any(e => e == null))
            {
                NotNullOrEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName);
            }

            return value;
        }
    }
}