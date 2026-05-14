// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Globalization;
using NuGet.Packaging.Core;

namespace NuGet.PackageManagement.UI
{
    /// <summary>
    /// Represents a package identity with an acessible name in the Preview Window Dialog in the PM UI
    /// </summary>
    public sealed class AccessiblePackageIdentity : PackageIdentity
    {
        public string AutomationName { get; }

        public AccessiblePackageIdentity(PackageIdentity id)
            : base(id.Id, id.Version)
        {
            AutomationName = string.Format(
               CultureInfo.CurrentCulture,
               Resources.Accessibility_PackageIdentity,
               Id,
               Version.ToNormalizedString());
        }
    }
}
