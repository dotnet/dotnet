// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    public static Position ToPosition(this LinePosition linePosition)
        => VsLspFactory.CreatePosition(linePosition.Line, linePosition.Character);

    public static Range ToZeroWidthRange(this LinePosition position)
        => VsLspFactory.CreateZeroWidthRange(position);
}
