// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Serialization;

internal interface IRazorProjectInfoDeserializer
{
    RazorProjectInfo? DeserializeFromFile(string filePath);
}
