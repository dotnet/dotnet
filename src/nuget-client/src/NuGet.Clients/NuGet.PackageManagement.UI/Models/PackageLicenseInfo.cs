// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.PackageManagement.UI
{
    public class PackageLicenseInfo
    {
        public PackageLicenseInfo(
            string id,
            IReadOnlyList<IText> license,
            string authors)
        {
            Id = id;
            License = license;
            Authors = authors;
        }

        public string Id { get; }

        public IReadOnlyList<IText> License { get; }

        public string Authors { get; }
    }
}
