// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using System.Collections.Generic;

namespace Internal.Microsoft.Extensions.DependencyModel
{
    internal class RuntimeAssetGroup
    {
        public RuntimeAssetGroup(string runtime, params string[] assetPaths) : this(runtime, (IEnumerable<string>)assetPaths) { }

        public RuntimeAssetGroup(string runtime, IEnumerable<string> assetPaths)
        {
            Runtime = runtime;
            AssetPaths = assetPaths.ToArray();
        }

        /// <summary>
        /// The runtime ID associated with this group (may be empty if the group is runtime-agnostic)
        /// </summary>
        public string Runtime { get; }

        /// <summary>
        /// Gets a list of assets provided in this runtime group
        /// </summary>
        public IReadOnlyList<string> AssetPaths { get; }
    }
}
