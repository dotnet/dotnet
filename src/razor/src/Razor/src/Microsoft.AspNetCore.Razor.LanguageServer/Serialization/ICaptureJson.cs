// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Serialization;

/// <summary>
/// Used by an interface to capture the <see cref="JToken"/> representation of a request so no data loss occurs. This should be used sparringly
/// because converting to a <see cref="JToken"/> and then an actual type is not as efficient.
/// </summary>
internal interface ICaptureJson
{
    public JToken Json { get; set; }
}

