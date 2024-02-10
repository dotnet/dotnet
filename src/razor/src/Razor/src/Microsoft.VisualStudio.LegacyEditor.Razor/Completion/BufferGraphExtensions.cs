// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
