// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Remote;

[DataContract]
internal struct RemoteClientInitializationOptions
{
    [DataMember(Order = 0)]
    internal bool UseRazorCohostServer;

    [DataMember(Order = 1)]
    internal bool UsePreciseSemanticTokenRanges;
}
