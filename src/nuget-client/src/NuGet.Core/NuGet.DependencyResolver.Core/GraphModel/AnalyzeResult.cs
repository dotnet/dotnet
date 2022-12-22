// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.DependencyResolver
{
    public class AnalyzeResult<TItem>
    {
        public List<DowngradeResult<TItem>> Downgrades { get; }
        public List<VersionConflictResult<TItem>> VersionConflicts { get; }
        public List<GraphNode<TItem>> Cycles { get; }

        public AnalyzeResult()
        {
            Downgrades = new List<DowngradeResult<TItem>>();
            VersionConflicts = new List<VersionConflictResult<TItem>>();
            Cycles = new List<GraphNode<TItem>>();
        }

        public void Combine(AnalyzeResult<TItem> result)
        {
            Downgrades.AddRange(result.Downgrades);
            VersionConflicts.AddRange(result.VersionConflicts);
            Cycles.AddRange(result.Cycles);
        }
    }
}
