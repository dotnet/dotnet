// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class DocumentInteractionManager
{
    public abstract Task OpenDocumentAsync(string filePath, CancellationToken cancellationToken);
}
