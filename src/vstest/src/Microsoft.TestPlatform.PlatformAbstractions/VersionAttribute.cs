// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace Microsoft.TestPlatform.PlatformAbstractions;

[Conditional("DEBUG")]
[AttributeUsage(AttributeTargets.Method)]
internal class VersionAttribute : Attribute
{
    public string? Description { get; set; }

#pragma warning disable IDE0060 // Remove unused parameter // TODO: Do something with version or remove it
    public VersionAttribute(int version)
#pragma warning restore IDE0060 // Remove unused parameter
    {

    }
}
