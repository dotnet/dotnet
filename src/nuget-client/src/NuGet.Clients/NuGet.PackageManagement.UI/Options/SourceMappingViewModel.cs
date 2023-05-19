// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.Options
{
    public class SourceMappingViewModel
    {
        public string ID { get; private set; }
        public IReadOnlyList<PackageSourceContextInfo> Sources { get; private set; }

        public string SourcesString => string.Join(", ", Sources.Select(s => s.Name));

        public override string ToString()
        {
            return ID + " " + SourcesString;
        }

        public SourceMappingViewModel(string packageId, List<PackageSourceContextInfo> packageSources)
        {

            ID = packageId ?? throw new ArgumentNullException(nameof(packageId));
            Sources = packageSources ?? throw new ArgumentNullException(nameof(packageSources));
        }
    }
}
