// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;

namespace ValidateVmrChanges;

internal class PrInfo
{
    internal string TargetBranch { get; }
    internal List<string> ChangedFiles { get; }
    internal PrInfo(string targetBranch, List<string> changedFiles)
    {
        TargetBranch = targetBranch;
        ChangedFiles = changedFiles;
    }
}
