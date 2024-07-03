// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.VisualStudio.Razor;

internal interface IProjectCapabilityResolver
{
    /// <summary>
    /// Determines whether the project associated with the specified document has the given <paramref name="capability"/>.
    /// </summary>
    bool ResolveCapability(string capability, string documentFilePath);
}
