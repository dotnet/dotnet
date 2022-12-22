// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Editor.Razor;

internal abstract class ProjectCapabilityResolver
{
    public abstract bool HasCapability(object project, string capability);

    public abstract bool HasCapability(string documentFilePath, object project, string capability);
}
