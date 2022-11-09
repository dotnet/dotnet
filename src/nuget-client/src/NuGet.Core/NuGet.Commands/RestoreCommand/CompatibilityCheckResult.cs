// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Shared;

namespace NuGet.Commands
{
    public class CompatibilityCheckResult
    {
        public RestoreTargetGraph Graph { get; }

        public IReadOnlyList<CompatibilityIssue> Issues { get; }

        public bool Success => !Issues.Any();

        public CompatibilityCheckResult(RestoreTargetGraph graph, IEnumerable<CompatibilityIssue> issues)
        {
            Graph = graph;
            Issues = issues.AsList().AsReadOnly();
        }
    }
}
