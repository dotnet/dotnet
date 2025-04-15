// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.AspNetCore.Razor.Serialization.Json;

internal static class SerializationFormat
{
    // This version number must be incremented if the serialization format for RazorProjectInfo
    // or any of the types that compose it changes. This includes: RazorConfiguration,
    // ProjectWorkspaceState, TagHelperDescriptor, and DocumentSnapshotHandle.
    // NOTE: If this version is changed, a coordinated insertion is required between Roslyn and Razor for the C# extension.
    public const int Version = 8;
}
