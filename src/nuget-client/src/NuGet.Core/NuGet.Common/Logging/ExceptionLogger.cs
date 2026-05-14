// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Common
{
    public class ExceptionLogger
    {
        public ExceptionLogger(IEnvironmentVariableReader reader)
        {
            // We can cache this value since environment variables should be fixed during runtime.
            ShowStack = ShouldShowStack(reader);
        }

        /// <summary>
        /// Determines whether the full exception (including stack trace) should be displayed to
        /// the user. In prerelease or dogfooding scenarios, it is useful to have a non-verbose
        /// logging level but, in the case of an unhandled exception, print the full exception for
        /// bug reporting.
        /// </summary>
        /// <returns>
        /// True if the exception stack should be displayed to the user. False, otherwise.
        /// </returns>
        public bool ShowStack { get; }

        private static bool ShouldShowStack(IEnvironmentVariableReader reader)
        {
            var rawShowStack = reader.GetEnvironmentVariable("NUGET_SHOW_STACK");

            if (rawShowStack == null)
            {
                return false;
            }

            return string.Equals(rawShowStack.Trim(), "true", StringComparison.OrdinalIgnoreCase);
        }

        public static ExceptionLogger Instance { get; } = new ExceptionLogger(EnvironmentVariableWrapper.Instance);
    }
}
