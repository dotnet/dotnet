// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Arcade.Sdk;

[MSBuildMultiThreadableTask]
public class GetAssemblyFullName : Task, IMultiThreadableTask
{
    /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
    public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

    [Required]
    public ITaskItem[] Items { get; set; }

    public string PathMetadata { get; set; }

    [Required]
    public string FullNameMetadata { get; set; }

    [Output]
    public ITaskItem[] ItemsWithFullName { get; set; }

    public override bool Execute()
    {
        ItemsWithFullName = Items;

        foreach (var item in Items)
        {
            var assemblyPath = string.IsNullOrEmpty(PathMetadata) ? item.ItemSpec : item.GetMetadata(PathMetadata);
            item.SetMetadata(FullNameMetadata, AssemblyName.GetAssemblyName(TaskEnvironment.GetAbsolutePath(assemblyPath)).FullName);
        }

        return true;
    }
}

