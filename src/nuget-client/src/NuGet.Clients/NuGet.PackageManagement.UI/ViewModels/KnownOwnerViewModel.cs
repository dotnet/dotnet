// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI.ViewModels
{
    public class KnownOwnerViewModel
    {
        private string _name;
        private Uri _link;

        public KnownOwnerViewModel(KnownOwner knownOwner)
        {
            _name = knownOwner.Name;
            _link = knownOwner.Link;
        }

        public string Name => _name;

        public Uri Link => _link;
    }
}
