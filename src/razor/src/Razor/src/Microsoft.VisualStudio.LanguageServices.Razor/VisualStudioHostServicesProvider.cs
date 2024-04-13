// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Host;

namespace Microsoft.VisualStudio.Razor;

internal abstract class VisualStudioHostServicesProvider
{
    public abstract HostServices GetServices();
}
