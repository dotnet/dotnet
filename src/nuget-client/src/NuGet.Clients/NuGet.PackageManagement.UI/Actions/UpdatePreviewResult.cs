// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI
{
    public class UpdatePreviewResult
    {
        public PackageIdentity Old { get; }
        public PackageIdentity New { get; }
        public string AutomationName { get; }

        public UpdatePreviewResult(PackageIdentity oldPackage, PackageIdentity newPackage)
        {
            Old = oldPackage;
            New = newPackage;
            AutomationName = string.Format(
                CultureInfo.CurrentCulture,
                Resources.Preview_PackageUpdate,
                Old.Id, Old.Version.ToNormalizedString(),
                New.Id, New.Version.ToNormalizedString());
        }

        public override string ToString()
        {
            return Old + " -> " + New;
        }
    }
}
