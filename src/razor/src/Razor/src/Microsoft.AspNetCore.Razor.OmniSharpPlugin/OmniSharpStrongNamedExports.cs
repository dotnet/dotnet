// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Composition;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.OmniSharpPlugin.StrongNamed;
using OmniSharp;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

// We need to re-export MEF based services from the OmniSharp plugin strong named assembly in order
// to make those services available via MEF. This isn't an issue for Roslyn based services because
// we're able to hook into OmniSharp's Roslyn service aggregator to allow it to inspect the strong
// named plugin assembly.

[Shared]
[Export(typeof(OmniSharpProjectSnapshotManagerDispatcher))]
internal class ExportOmniSharpProjectSnapshotManagerDispatcher : DefaultOmniSharpProjectSnapshotManagerDispatcher
{
}

[Shared]
[Export(typeof(RemoteTextLoaderFactory))]
internal class ExportRemoteTextLoaderFactory : DefaultRemoteTextLoaderFactory
{
}

[Shared]
[Export(typeof(OmniSharpProjectSnapshotManagerAccessor))]
internal class ExportDefaultOmniSharpProjectSnapshotManagerAccessor : DefaultOmniSharpProjectSnapshotManagerAccessor
{
    [ImportingConstructor]
    public ExportDefaultOmniSharpProjectSnapshotManagerAccessor(
        RemoteTextLoaderFactory remoteTextLoaderFactory,
        [ImportMany] IEnumerable<IOmniSharpProjectSnapshotManagerChangeTrigger> projectChangeTriggers,
        OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        OmniSharpWorkspace workspace) : base(remoteTextLoaderFactory, projectChangeTriggers, projectSnapshotManagerDispatcher, workspace)
    {
    }
}

[Shared]
[Export(typeof(IOmniSharpProjectSnapshotManagerChangeTrigger))]
internal class ExportOmniSharpWorkspaceProjectStateChangeDetector : OmniSharpWorkspaceProjectStateChangeDetector
{
    [ImportingConstructor]
    public ExportOmniSharpWorkspaceProjectStateChangeDetector(
        OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        OmniSharpProjectWorkspaceStateGenerator workspaceStateGenerator,
        OmniSharpLanguageServerFeatureOptions languageServerFeatureOptions)
        : base(projectSnapshotManagerDispatcher, workspaceStateGenerator, languageServerFeatureOptions)
    {
    }
}

[Shared]
[Export(typeof(IOmniSharpProjectSnapshotManagerChangeTrigger))]
[Export(typeof(OmniSharpProjectWorkspaceStateGenerator))]
internal class ExportOmniSharpProjectWorkspaceStateGenerator : OmniSharpProjectWorkspaceStateGenerator
{
    [ImportingConstructor]
    public ExportOmniSharpProjectWorkspaceStateGenerator(OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher) : base(projectSnapshotManagerDispatcher)
    {
    }
}

[Shared]
[Export(typeof(IOmniSharpProjectSnapshotManagerChangeTrigger))]
internal class ExportOmniSharpBackgroundDocumentGenerator : OmniSharpBackgroundDocumentGenerator
{
    [ImportingConstructor]
    public ExportOmniSharpBackgroundDocumentGenerator(
        OmniSharpProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        RemoteTextLoaderFactory remoteTextLoaderFactory,
        [ImportMany] IEnumerable<OmniSharpDocumentProcessedListener> documentProcessedListeners) : base(projectSnapshotManagerDispatcher, remoteTextLoaderFactory, documentProcessedListeners)
    {
    }
}

[Shared]
[Export(typeof(OmniSharpLanguageServerFeatureOptions))]
internal class ExportOmniSharpLanguageServerFeatureOptions : OmniSharpLanguageServerFeatureOptions
{
    [ImportingConstructor]
    public ExportOmniSharpLanguageServerFeatureOptions() : base()
    {
    }
}
