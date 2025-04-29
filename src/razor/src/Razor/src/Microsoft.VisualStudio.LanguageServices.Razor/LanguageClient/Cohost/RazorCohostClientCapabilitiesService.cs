// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(RazorCohostClientCapabilitiesService))]
[Export(typeof(IClientCapabilitiesService))]
internal sealed class RazorCohostClientCapabilitiesService : AbstractClientCapabilitiesService;
