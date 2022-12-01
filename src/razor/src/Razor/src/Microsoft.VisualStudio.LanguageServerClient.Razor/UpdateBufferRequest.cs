// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor;

internal class UpdateBufferRequest
{
    public int? HostDocumentVersion { get; init; }

    public string? HostDocumentFilePath { get; init; }

    public required IReadOnlyList<TextChange> Changes { get; init; }
}
