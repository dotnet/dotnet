// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Remote;

[DataContract]
internal struct RemoteClientInitializationOptions
{
    [DataMember(Order = 0)]
    internal required bool UseRazorCohostServer;

    [DataMember(Order = 1)]
    internal required bool UsePreciseSemanticTokenRanges;

    [DataMember(Order = 2)]
    internal required string CSharpVirtualDocumentSuffix;

    [DataMember(Order = 3)]
    internal required string HtmlVirtualDocumentSuffix;

    [DataMember(Order = 4)]
    internal required bool IncludeProjectKeyInGeneratedFilePath;
}
