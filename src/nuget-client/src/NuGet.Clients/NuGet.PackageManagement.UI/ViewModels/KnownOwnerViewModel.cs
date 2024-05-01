// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.ViewModels
{
    public class KnownOwnerViewModel
    {
        public KnownOwnerViewModel(KnownOwner knownOwner)
        {
            Name = knownOwner.Name;
            Link = knownOwner.Link;
        }

        public string Name { get; }

        public Uri Link { get; }
    }
}
