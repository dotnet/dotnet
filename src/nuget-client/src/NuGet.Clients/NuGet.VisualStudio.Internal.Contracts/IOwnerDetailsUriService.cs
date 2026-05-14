// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface IOwnerDetailsUriService
    {
        bool SupportsKnownOwners { get; }
        Uri GetOwnerDetailsUri(string ownerName);
    }
}
