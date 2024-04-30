// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public class KnownOwner
    {
        private string _name;
        private Uri _link;

        public KnownOwner(string name, Uri link)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _link = link ?? throw new ArgumentNullException(nameof(link));
        }

        public string Name => _name;

        public Uri Link => _link;
    }
}
