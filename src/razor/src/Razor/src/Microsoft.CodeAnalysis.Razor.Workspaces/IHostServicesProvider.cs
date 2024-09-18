// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Host;

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal interface IHostServicesProvider
{
    HostServices GetServices();
}
