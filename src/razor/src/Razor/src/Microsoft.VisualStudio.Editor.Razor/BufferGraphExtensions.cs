// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Text.Projection;

namespace Microsoft.VisualStudio.Text;

internal static class BufferGraphExtensions
{
    public static Collection<ITextBuffer> GetRazorBuffers(this IBufferGraph bufferGraph)
    {
        if (bufferGraph is null)
        {
            throw new ArgumentNullException(nameof(bufferGraph));
        }

        return bufferGraph.GetTextBuffers(TextBufferExtensions.IsLegacyCoreRazorBuffer);
    }
}
