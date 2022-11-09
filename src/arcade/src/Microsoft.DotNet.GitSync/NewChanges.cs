// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;

namespace Microsoft.DotNet.GitSync
{
    public class NewChanges
    {
        public NewChanges(RepositoryInfo targetRepository)
        {
            TargetRepository = targetRepository;
        }

        public RepositoryInfo TargetRepository { get; }

        public Dictionary<string, List<string>> changes { get; } = new Dictionary<string, List<string>>();
    }
}
