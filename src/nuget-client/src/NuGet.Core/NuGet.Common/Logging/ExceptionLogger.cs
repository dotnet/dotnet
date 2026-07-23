// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Common
{
    public class ExceptionLogger
    {
        static ExceptionLogger()
        {
            StaticState.StartMSBuildRestoreTasks += ResetInstance;
        }

        public ExceptionLogger(IEnvironmentVariableReader reader)
        {
            // We can cache this value since environment variables should be fixed during a restore. In a host that
            // reuses the process across builds, ResetInstance re-reads it at the start of the next restore.
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

        public static ExceptionLogger Instance { get; private set; } = new ExceptionLogger(EnvironmentVariableWrapper.Instance);

        /// <summary>
        /// Recreates <see cref="Instance" /> from the current environment so a reused process observes the
        /// current <c>NUGET_SHOW_STACK</c> value on the next restore.
        /// </summary>
        internal static void ResetInstance() => Instance = new ExceptionLogger(EnvironmentVariableWrapper.Instance);
    }
}
