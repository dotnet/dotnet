// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;

namespace Microsoft.CodeAnalysis.Razor.Protocol.DocumentMapping;

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
