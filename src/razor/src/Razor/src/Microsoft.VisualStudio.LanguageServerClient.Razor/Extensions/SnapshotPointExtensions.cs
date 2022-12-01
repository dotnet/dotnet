// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Extensions;

internal static class SnapshotPointExtensions
{
    public static Position AsPosition(this SnapshotPoint point)
    {
        var line = point.GetContainingLine();
        var character = point.Position - line.Start.Position;
        var lineNumber = line.LineNumber;
        var position = new Position()
        {
            Character = character,
            Line = lineNumber,
        };

        return position;
    }
}
