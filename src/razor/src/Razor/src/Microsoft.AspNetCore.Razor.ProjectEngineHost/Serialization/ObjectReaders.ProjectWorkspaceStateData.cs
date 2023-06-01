// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.AspNetCore.Razor.Serialization;

internal static partial class ObjectReaders
{
    private record struct ProjectWorkspaceStateData(ImmutableArray<TagHelperDescriptor> TagHelpers, LanguageVersion CSharpLanguageVersion)
    {
        public static readonly PropertyMap<ProjectWorkspaceStateData> PropertyMap = new(
            (nameof(TagHelpers), ReadTagHelpers),
            (nameof(CSharpLanguageVersion), ReadCSharpLanguageVersion));

        private static void ReadTagHelpers(JsonDataReader reader, ref ProjectWorkspaceStateData data)
            => data.TagHelpers = reader.ReadImmutableArray(
                static r => ReadTagHelper(r, useCache: true));

        private static void ReadCSharpLanguageVersion(JsonDataReader reader, ref ProjectWorkspaceStateData data)
            => data.CSharpLanguageVersion = (LanguageVersion)reader.ReadInt32();
    }
}
