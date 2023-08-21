// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;

namespace NuGet.Configuration
{
    internal class SearchNode
    {
        public readonly Dictionary<char, SearchNode> Children;
        public bool IsGlobbing { get; set; }
        public List<string>? PackageSources;

        public SearchNode()
        {
            Children = new Dictionary<char, SearchNode>();
            IsGlobbing = false;
        }
    }
}
