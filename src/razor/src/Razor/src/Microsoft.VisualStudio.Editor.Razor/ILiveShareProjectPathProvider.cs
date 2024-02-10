// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Editor.Razor;

internal interface ILiveShareProjectPathProvider
{
    bool TryGetProjectPath(ITextBuffer textBuffer, [NotNullWhen(true)] out string? filePath);
}
