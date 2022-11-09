// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.ContentModel;
using NuGet.Frameworks;

namespace NuGet.Packaging.Core
{
    internal static class ContentExtractor
    {
        internal static void GetContentForPattern(ContentItemCollection collection, PatternSet pattern, IList<ContentItemGroup> itemGroups)
        {
            collection.PopulateItemGroups(pattern, itemGroups);
        }

        internal static IEnumerable<NuGetFramework> GetGroupFrameworks(IEnumerable<ContentItemGroup> groups)
        {
            return groups.Select(e => ((NuGetFramework)e.Properties["tfm"]));
        }
    }
}
