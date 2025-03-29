// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Internal;

namespace Microsoft.AspNetCore.Razor.ProjectSystem;

internal sealed partial class ProjectEngineFactory
{
    private readonly record struct InitializerKey(string ConfigurationName, string AssemblyName)
    {
        public bool Equals(InitializerKey other)
            => ConfigurationName == other.ConfigurationName &&
               AssemblyName == other.AssemblyName;

        public override int GetHashCode()
        {
            var hash = HashCodeCombiner.Start();
            hash.Add(ConfigurationName, StringComparer.Ordinal);
            hash.Add(AssemblyName, StringComparer.Ordinal);

            return hash.CombinedHash;
        }
    }
}
