// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.CodeAnalysis.ExternalAccess.Razor;

namespace Microsoft.VisualStudio.Razor.DynamicFiles;

// We have IVT access to the Roslyn APIs for product code, but not for testing.
internal enum ExcerptModeInternal
{
    SingleLine = RazorExcerptMode.SingleLine,
    Tooltip = RazorExcerptMode.Tooltip,
}
