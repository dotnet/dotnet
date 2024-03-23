// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Razor.Remote;

[DataContract]
internal struct RemoteClientLSPInitializationOptions
{
    [DataMember(Order = 0)]
    internal required string[] TokenTypes;

    [DataMember(Order = 1)]
    internal required string[] TokenModifiers;
}
