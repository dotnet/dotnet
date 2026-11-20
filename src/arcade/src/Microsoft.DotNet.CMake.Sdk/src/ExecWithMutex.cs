// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Tasks;

namespace Microsoft.DotNet.CMake.Sdk;

/// <summary>
/// Executes a command while holding a named, cross-process mutex.
/// </summary>
public sealed class ExecWithMutex : Exec
{
    [Required]
    public string MutexName { get; set; }

    public override bool Execute()
    {
        using var mutex = CMakeBuildMutex.Acquire(TaskEnvironment, MutexName);
        return base.Execute();
    }
}
