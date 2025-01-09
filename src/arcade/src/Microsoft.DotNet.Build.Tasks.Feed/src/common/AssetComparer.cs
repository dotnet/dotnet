// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET472_OR_GREATER
using Microsoft.DotNet.ProductConstructionService.Client.Models;
using System.Collections.Generic;

namespace Microsoft.DotNet.Build.Tasks.Feed
{
    class AssetComparer : IEqualityComparer<Asset>
    {
        public bool Equals(Asset obj1, Asset obj2)
        {
            // if both are null this will return true
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (obj1 == null || obj2 == null)
            {
                return false;
            }

            Asset assetA = (Asset)obj1;
            Asset assetB = (Asset)obj2;

            return
                (assetA != null) &&
                (assetB != null) &&
                (assetA.Id == assetB.Id) &&
                (assetA.Name == assetB.Name) &&
                (assetA.Version == assetB.Version);
        }

        public int GetHashCode(Asset asset)
        {
            return (asset.Id, asset.Name, asset.Version).GetHashCode();
        }
    }
}
#endif
