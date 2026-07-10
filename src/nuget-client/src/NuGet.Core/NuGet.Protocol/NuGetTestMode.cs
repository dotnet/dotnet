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
            // cached for the life-time of the app domain
            Enabled = FromEnvironmentVariable();
            StaticState.StartMSBuildRestoreTasks += ResetCache;
        }

        public static bool Enabled { get; private set; }

        /// <summary>Re-reads <c>NuGetTestModeEnabled</c> from the current environment.</summary>
        internal static void ResetCache() => Enabled = FromEnvironmentVariable();

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
