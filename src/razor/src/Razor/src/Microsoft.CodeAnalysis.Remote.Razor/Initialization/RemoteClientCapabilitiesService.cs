// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Shared]
[Export(typeof(IClientCapabilitiesService))]
[Export(typeof(RemoteClientCapabilitiesService))]
internal sealed class RemoteClientCapabilitiesService : AbstractClientCapabilitiesService;
