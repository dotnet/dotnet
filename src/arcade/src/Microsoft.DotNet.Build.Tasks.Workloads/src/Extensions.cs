// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Build.Tasks.Workloads
{
    public static class Extensions
    {
        /// <summary>
        /// Determines if the item contains a the specified metadata.
        /// </summary>
        /// <param name="item">The item to evaluate.</param>
        /// <param name="metadataName">The name of the metadata to check.</param>
        /// <returns><see langword="true"/> if the metadata exists; <see langword="false"/> otherwise.</returns>
        public static bool HasMetadata(this ITaskItem item, string metadataName)
        {
            foreach (string name in item.MetadataNames)
            {
                if (string.Equals(metadataName, name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
