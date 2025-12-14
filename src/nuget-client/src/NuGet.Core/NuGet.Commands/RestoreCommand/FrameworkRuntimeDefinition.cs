// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using NuGet.Frameworks;

namespace NuGet.Commands
{
    internal sealed record FrameworkRuntimeDefinition
    {
        public string TargetAlias { get; }

        public NuGetFramework Framework { get; }

        public string RuntimeIdentifier { get; }

        public string Name { get; }

        public FrameworkRuntimeDefinition(string targetAlias, NuGetFramework framework, string? runtimeIdentifier)
        {
            TargetAlias = targetAlias ?? string.Empty;
            Framework = framework ?? throw new ArgumentNullException(nameof(framework));
            RuntimeIdentifier = runtimeIdentifier ?? string.Empty;
            Name = FrameworkRuntimePair.GetTargetGraphName(framework, runtimeIdentifier);
        }

        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0}~{1}~{2}",
                TargetAlias,
                Framework.GetShortFolderName(),
                RuntimeIdentifier);
        }
    }
}
