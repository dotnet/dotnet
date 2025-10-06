// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Projection;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

internal static class BufferGraphExtensions
{
    public static IEnumerable<ITextBuffer> GetRazorBuffers(this IBufferGraph bufferGraph)
    {
        if (bufferGraph is null)
        {
            throw new ArgumentNullException(nameof(bufferGraph));
        }

        return bufferGraph.GetTextBuffers(TextBufferExtensions.IsLegacyCoreRazorBuffer);
    }
}
