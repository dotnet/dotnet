// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Protocol.Core.Types
{
    public static class NuGetTestMode
    {
        private const string _testModeEnvironmentVariableName = "NuGetTestModeEnabled";
        public const string NuGetTestClientName = "NuGet Test Client";

        static NuGetTestMode()
        {
            StaticState.BuildEnded += ResetCache;
        }

        private static bool? s_enabled;

        public static bool Enabled
        {
            get
            {
                // Computed on first use rather than in the reset, so a process reused across builds reads
                // NuGetTestModeEnabled from the environment of the build that uses it.
                s_enabled ??= FromEnvironmentVariable();
                return s_enabled.Value;
            }
            private set => s_enabled = value;
        }

        /// <summary>Discards the cached <c>NuGetTestModeEnabled</c> value so it is re-read on next use.</summary>
        internal static void ResetCache() => s_enabled = null;

        private static bool FromEnvironmentVariable()
        {
#pragma warning disable RS0030 // Do not use banned APIs
            var testMode = Environment.GetEnvironmentVariable(_testModeEnvironmentVariableName);
#pragma warning restore RS0030 // Do not use banned APIs
            if (String.IsNullOrEmpty(testMode))
            {
                return false;
            }

            bool isEnabled;
            return Boolean.TryParse(testMode, out isEnabled) && isEnabled;
        }


        /// <summary>
        /// Intended for internal use only: utility method for testing purposes.
        /// </summary>
        public static T InvokeTestFunctionAgainstTestMode<T>(Func<T> function, bool testModeEnabled)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            var valueBeforeTestRun = Enabled;

            Enabled = testModeEnabled;

            var result = function();

            Enabled = valueBeforeTestRun;

            return result;
        }
    }
}
