// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.IO;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Serialization;

internal interface IRazorProjectInfoDeserializer
{
    RazorProjectInfo? DeserializeFromString(string? base64String);
    RazorProjectInfo? DeserializeFromFile(string filePath);
    RazorProjectInfo? DeserializeFromStream(Stream stream);
}
