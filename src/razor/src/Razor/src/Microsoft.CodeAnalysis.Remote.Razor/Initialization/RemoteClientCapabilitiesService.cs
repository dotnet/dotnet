// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Shared]
[Export(typeof(IClientCapabilitiesService))]
[Export(typeof(RemoteClientCapabilitiesService))]
internal sealed class RemoteClientCapabilitiesService : AbstractClientCapabilitiesService;
