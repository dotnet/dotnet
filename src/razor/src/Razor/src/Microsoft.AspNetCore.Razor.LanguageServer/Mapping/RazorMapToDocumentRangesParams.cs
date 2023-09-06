// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Razor.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Mapping;

// Note: This type should be kept in sync with the one in VisualStudio.LanguageServerClient assembly.
[DataContract]
internal class RazorMapToDocumentRangesParams
{
    [DataMember(Name = "kind")]
    public RazorLanguageKind Kind { get; init; }

    [DataMember(Name = "razorDocumentUri")]
    public required Uri RazorDocumentUri { get; init; }

    [DataMember(Name = "projectedRanges")]
    public required Range[] ProjectedRanges { get; init; }

    [DataMember(Name = "mappingBehavior")]
    public MappingBehavior MappingBehavior { get; init; }
}
