// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NuGet.Common;

namespace NuGet.PackageManagement.UI
{
    public class PreviewResult
    {
        public IEnumerable<AccessiblePackageIdentity> Deleted { get; }

        public IEnumerable<AccessiblePackageIdentity> Added { get; }

        public IEnumerable<UpdatePreviewResult> Updated { get; }

        public ImmutableDictionary<string, SortedSet<string>>? NewSourceMappings { get; }

        public string Name { get; }

        public NuGetOperationStatus NuGetOperationStatus { get; }

        public PreviewResult(
            string projectName,
            IEnumerable<AccessiblePackageIdentity> added,
            IEnumerable<AccessiblePackageIdentity> deleted,
            IEnumerable<UpdatePreviewResult> updated)
        {
            Name = projectName;
            Added = added;
            Deleted = deleted;
            Updated = updated;
            NuGetOperationStatus = NuGetOperationStatus.Succeeded;
        }

        public PreviewResult(Dictionary<string, SortedSet<string>>? newSourceMappings, NuGetOperationStatus nuGetOperationStatus)
        {
            Name = Resources.Label_Solution;
            NewSourceMappings = newSourceMappings?.ToImmutableDictionary();
            Added = Enumerable.Empty<AccessiblePackageIdentity>();
            Deleted = Enumerable.Empty<AccessiblePackageIdentity>();
            Updated = Enumerable.Empty<UpdatePreviewResult>();
            NuGetOperationStatus = nuGetOperationStatus;
        }
    }
}
