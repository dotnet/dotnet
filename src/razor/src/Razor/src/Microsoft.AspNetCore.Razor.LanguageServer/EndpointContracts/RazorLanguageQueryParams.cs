// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;

[DataContract]
internal class RazorLanguageQueryParams
{
    [DataMember(Name = "uri")]
    public required Uri Uri { get; set; }

    [DataMember(Name ="position")]
    public required Position Position { get; set; }
}
