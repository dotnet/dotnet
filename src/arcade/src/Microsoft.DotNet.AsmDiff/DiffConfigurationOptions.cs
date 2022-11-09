// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace Microsoft.DotNet.AsmDiff
{
    [Flags]
    public enum DiffConfigurationOptions
    {
        None = 0x0,
        IncludeUnchanged = 0x2,
        IncludeAdded = 0x4,
        IncludeRemoved = 0x8,
        IncludeChanged = 0x10,
        IncludeInternals = 0x20,
        IncludePrivates = 0x40,
        IncludeGenerated = 0x80,
        DiffAttributes = 0x100,
        DiffAssemblyInfo = 0x200,
        GroupByAssembly = 0x400,
        FlattenTypes = 0x800,
        TypesOnly = 0x1000,
        HighlightBaseMembers = 0x2000,
        AlwaysDiffMembers = 0x4000,
        IncludeAddedTypes = 0x8000,
        IncludeRemovedTypes = 0x10000,
        StrikeRemoved = 0x20000 
    }
}
