// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using Microsoft.CodeAnalysis.Razor.DocumentMapping;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;

namespace Microsoft.CodeAnalysis.Remote.Razor.DocumentMapping;

[Export(typeof(IRazorEditService)), Shared]
[method: ImportingConstructor]
internal sealed class RemoteRazorEditService(IDocumentMappingService documentMappingService, IClientSettingsManager clientSettingsManager, ITelemetryReporter telemetryReporter)
    : RazorEditService(documentMappingService, clientSettingsManager, telemetryReporter);
