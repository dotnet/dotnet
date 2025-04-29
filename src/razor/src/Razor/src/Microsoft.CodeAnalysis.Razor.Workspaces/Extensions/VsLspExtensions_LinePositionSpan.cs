// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.LanguageServer.Protocol;

internal static partial class VsLspExtensions
{
    public static Range ToRange(this LinePositionSpan linePositionSpan)
        => VsLspFactory.CreateRange(linePositionSpan);
}
