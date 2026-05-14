// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using NuGet.Frameworks;

namespace NuGet.VisualStudio.Internal.Contracts
{
    public interface IProjectMetadataContextInfo
    {
        string? FullPath { get; }
        string? Name { get; }
        string? ProjectId { get; }
        IReadOnlyCollection<NuGetFramework>? SupportedFrameworks { get; }
        NuGetFramework? TargetFramework { get; }
        string? UniqueName { get; }
    }
}
