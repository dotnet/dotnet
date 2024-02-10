// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LegacyEditor.Razor;

internal interface IProjectPathProvider
{
    bool TryGetProjectPath(ITextBuffer textBuffer, [NotNullWhen(true)] out string? filePath);
}
