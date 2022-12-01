// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;
using Microsoft.Extensions.Internal;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.HtmlCSharp;

// Note: This type should be kept in sync with the one in Razor.LanguageServer assembly.
[DataContract]
internal class RazorMapToDocumentRangesParams : IEquatable<RazorMapToDocumentRangesParams>
{
    [DataMember(Name = "kind")]
    public RazorLanguageKind Kind { get; init; }

    [DataMember(Name = "razorDocumentUri")]
    public required Uri RazorDocumentUri { get; init; }

    [DataMember(Name = "projectedRanges")]
    public required Range[] ProjectedRanges { get; init; }

    [DataMember(Name = "mappingBehavior")]
    public LanguageServerMappingBehavior MappingBehavior { get; init; }

    public bool Equals(RazorMapToDocumentRangesParams? other)
    {
        return
            other is not null &&
            Kind == other.Kind &&
            RazorDocumentUri == other.RazorDocumentUri &&
            MappingBehavior == other.MappingBehavior &&
            ProjectedRanges.SequenceEqual(other.ProjectedRanges);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as RazorMapToDocumentRangesParams);
    }

    public override int GetHashCode()
    {
        var hash = new HashCodeCombiner();
        hash.Add(Kind);
        hash.Add(RazorDocumentUri);
        hash.Add(ProjectedRanges);
        hash.Add(MappingBehavior);
        return hash;
    }
}
