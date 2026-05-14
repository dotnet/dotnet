// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Tests
{
    public static class Extensions
    {
        public static TaskItem WithMetadata(this TaskItem item, string metadataName, string metadataValue)
        {
            item.SetMetadata(metadataName, metadataValue);
            return item;
        }
    }
}
