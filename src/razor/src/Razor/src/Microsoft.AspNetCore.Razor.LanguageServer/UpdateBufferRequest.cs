// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class UpdateBufferRequest
{
    public int? HostDocumentVersion { get; set; }

    public string? ProjectKeyId { get; set; }

    public required string HostDocumentFilePath { get; set; }

    public required IReadOnlyList<TextChange> Changes { get; set; }

    public bool PreviousWasEmpty { get; set; }
}
