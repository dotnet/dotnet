// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;

namespace NuGet.Packaging.Signing
{
    internal static class X509ChainBuildPolicyFactory
    {
        private const string DefaultValue = "3,1000";
        // These fields are non-private only to facilitate testing.
        internal const string DisabledValue = "0";
        internal const string EnvironmentVariableName = "NUGET_EXPERIMENTAL_CHAIN_BUILD_RETRY_POLICY";
        internal const char ValueDelimiter = ',';

        private static readonly object LockObject = new object();
        private static IX509ChainBuildPolicy Policy;

        internal static IX509ChainBuildPolicy Create(IEnvironmentVariableReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (Policy is object)
            {
                return Policy;
            }

            lock (LockObject)
            {
                Policy ??= CreateWithoutCaching(reader);
            }

            return Policy;
        }

        // This is non-private only to facilitate testing.
        internal static IX509ChainBuildPolicy CreateWithoutCaching(IEnvironmentVariableReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (RuntimeEnvironmentHelper.IsWindows)
            {
                string value = reader.GetEnvironmentVariable(EnvironmentVariableName) ?? DefaultValue;

                if (string.Equals(value, DisabledValue, StringComparison.Ordinal))
                {
                    return DefaultX509ChainBuildPolicy.Instance;
                }

                string[] parts = value.Split(ValueDelimiter);

                if (parts.Length == 2
                    && int.TryParse(parts[0], out int retryCount)
                    && retryCount > 0
                    && int.TryParse(parts[1], out int sleepIntervalInMilliseconds)
                    && sleepIntervalInMilliseconds >= 0)
                {
                    TimeSpan sleepInterval = TimeSpan.FromMilliseconds(sleepIntervalInMilliseconds);

                    return new RetriableX509ChainBuildPolicy(DefaultX509ChainBuildPolicy.Instance, retryCount, sleepInterval);
                }
            }

            return DefaultX509ChainBuildPolicy.Instance;
        }
    }
}
